using System;
using System.Collections.Generic;
using System.Linq;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020009E6 RID: 2534
public class CreatureCalorieMonitor : GameStateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>
{
	// Token: 0x06002E9E RID: 11934 RVA: 0x001F50D0 File Offset: 0x001F32D0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.normal;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.root.EventHandler(GameHashes.CaloriesConsumed, delegate(CreatureCalorieMonitor.Instance smi, object data)
		{
			smi.OnCaloriesConsumed(data);
		}).ToggleBehaviour(GameTags.Creatures.Poop, new StateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.Transition.ConditionCallback(CreatureCalorieMonitor.ReadyToPoop), delegate(CreatureCalorieMonitor.Instance smi)
		{
			smi.Poop();
		}).Update(new Action<CreatureCalorieMonitor.Instance, float>(CreatureCalorieMonitor.UpdateMetabolismCalorieModifier), UpdateRate.SIM_200ms, false);
		this.normal.TagTransition(GameTags.Creatures.PausedHunger, this.pause.commonPause, false).Transition(this.hungry, (CreatureCalorieMonitor.Instance smi) => smi.IsHungry(), UpdateRate.SIM_1000ms);
		this.hungry.DefaultState(this.hungry.hungry).ToggleTag(GameTags.Creatures.Hungry).EventTransition(GameHashes.CaloriesConsumed, this.normal, (CreatureCalorieMonitor.Instance smi) => !smi.IsHungry());
		this.hungry.hungry.TagTransition(GameTags.Creatures.PausedHunger, this.pause.commonPause, false).Transition(this.normal, (CreatureCalorieMonitor.Instance smi) => !smi.IsHungry(), UpdateRate.SIM_1000ms).Transition(this.hungry.outofcalories, (CreatureCalorieMonitor.Instance smi) => smi.IsOutOfCalories(), UpdateRate.SIM_1000ms).ToggleStatusItem(Db.Get().CreatureStatusItems.Hungry, null);
		this.hungry.outofcalories.DefaultState(this.hungry.outofcalories.wild).Transition(this.hungry.hungry, (CreatureCalorieMonitor.Instance smi) => !smi.IsOutOfCalories(), UpdateRate.SIM_1000ms);
		this.hungry.outofcalories.wild.TagTransition(GameTags.Creatures.PausedHunger, this.pause.commonPause, false).TagTransition(GameTags.Creatures.Wild, this.hungry.outofcalories.tame, true).ToggleStatusItem(Db.Get().CreatureStatusItems.Hungry, null);
		this.hungry.outofcalories.tame.Enter("StarvationStartTime", new StateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.State.Callback(CreatureCalorieMonitor.StarvationStartTime)).Exit("ClearStarvationTime", delegate(CreatureCalorieMonitor.Instance smi)
		{
			this.starvationStartTime.Set(Mathf.Min(-(GameClock.Instance.GetTime() - this.starvationStartTime.Get(smi)), 0f), smi, false);
		}).Transition(this.hungry.outofcalories.starvedtodeath, (CreatureCalorieMonitor.Instance smi) => smi.GetDeathTimeRemaining() <= 0f, UpdateRate.SIM_1000ms).TagTransition(GameTags.Creatures.PausedHunger, this.pause.starvingPause, false).TagTransition(GameTags.Creatures.Wild, this.hungry.outofcalories.wild, false).ToggleStatusItem(CREATURES.STATUSITEMS.STARVING.NAME, CREATURES.STATUSITEMS.STARVING.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.BadMinor, false, default(HashedString), 129022, (string str, CreatureCalorieMonitor.Instance smi) => str.Replace("{TimeUntilDeath}", GameUtil.GetFormattedCycles(smi.GetDeathTimeRemaining(), "F1", false)), null, null).ToggleNotification((CreatureCalorieMonitor.Instance smi) => new Notification(CREATURES.STATUSITEMS.STARVING.NOTIFICATION_NAME, NotificationType.BadMinor, (List<Notification> notifications, object data) => CREATURES.STATUSITEMS.STARVING.NOTIFICATION_TOOLTIP + notifications.ReduceMessages(false), null, true, 0f, null, null, null, true, false, false)).ToggleEffect((CreatureCalorieMonitor.Instance smi) => this.outOfCaloriesTame);
		this.hungry.outofcalories.starvedtodeath.Enter(delegate(CreatureCalorieMonitor.Instance smi)
		{
			smi.GetSMI<DeathMonitor.Instance>().Kill(Db.Get().Deaths.Starvation);
		});
		this.pause.commonPause.TagTransition(GameTags.Creatures.PausedHunger, this.normal, true);
		this.pause.starvingPause.Exit("Recalculate StarvationStartTime", new StateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.State.Callback(CreatureCalorieMonitor.RecalculateStartTimeOnUnpause)).TagTransition(GameTags.Creatures.PausedHunger, this.hungry.outofcalories.tame, true);
		this.outOfCaloriesTame = new Effect("OutOfCaloriesTame", CREATURES.MODIFIERS.OUT_OF_CALORIES.NAME, CREATURES.MODIFIERS.OUT_OF_CALORIES.TOOLTIP, 0f, false, false, false, null, -1f, 0f, null, "");
		this.outOfCaloriesTame.Add(new AttributeModifier(Db.Get().CritterAttributes.Happiness.Id, -10f, CREATURES.MODIFIERS.OUT_OF_CALORIES.NAME, false, false, true));
	}

	// Token: 0x06002E9F RID: 11935 RVA: 0x001F5560 File Offset: 0x001F3760
	private static bool ReadyToPoop(CreatureCalorieMonitor.Instance smi)
	{
		return smi.stomach.IsReadyToPoop() && Time.time - smi.lastMealOrPoopTime >= smi.def.minimumTimeBeforePooping && !smi.IsInsideState(smi.sm.pause);
	}

	// Token: 0x06002EA0 RID: 11936 RVA: 0x000BE30F File Offset: 0x000BC50F
	private static void UpdateMetabolismCalorieModifier(CreatureCalorieMonitor.Instance smi, float dt)
	{
		if (smi.IsInsideState(smi.sm.pause))
		{
			return;
		}
		smi.deltaCalorieMetabolismModifier.SetValue(1f - smi.metabolism.GetTotalValue() / 100f);
	}

	// Token: 0x06002EA1 RID: 11937 RVA: 0x000BE347 File Offset: 0x000BC547
	private static void StarvationStartTime(CreatureCalorieMonitor.Instance smi)
	{
		if (smi.sm.starvationStartTime.Get(smi) <= 0f)
		{
			smi.sm.starvationStartTime.Set(GameClock.Instance.GetTime(), smi, false);
		}
	}

	// Token: 0x06002EA2 RID: 11938 RVA: 0x001F55B0 File Offset: 0x001F37B0
	private static void RecalculateStartTimeOnUnpause(CreatureCalorieMonitor.Instance smi)
	{
		float num = smi.sm.starvationStartTime.Get(smi);
		if (num < 0f)
		{
			float value = GameClock.Instance.GetTime() - Mathf.Abs(num);
			smi.sm.starvationStartTime.Set(value, smi, false);
		}
	}

	// Token: 0x04001F48 RID: 8008
	public GameStateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.State normal;

	// Token: 0x04001F49 RID: 8009
	public CreatureCalorieMonitor.PauseStates pause;

	// Token: 0x04001F4A RID: 8010
	private CreatureCalorieMonitor.HungryStates hungry;

	// Token: 0x04001F4B RID: 8011
	private Effect outOfCaloriesTame;

	// Token: 0x04001F4C RID: 8012
	public StateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.FloatParameter starvationStartTime;

	// Token: 0x020009E7 RID: 2535
	public struct CaloriesConsumedEvent
	{
		// Token: 0x04001F4D RID: 8013
		public Tag tag;

		// Token: 0x04001F4E RID: 8014
		public float calories;
	}

	// Token: 0x020009E8 RID: 2536
	public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
	{
		// Token: 0x06002EA6 RID: 11942 RVA: 0x000BDDBE File Offset: 0x000BBFBE
		public override void Configure(GameObject prefab)
		{
			prefab.GetComponent<Modifiers>().initialAmounts.Add(Db.Get().Amounts.Calories.Id);
		}

		// Token: 0x06002EA7 RID: 11943 RVA: 0x001F5600 File Offset: 0x001F3800
		public List<Descriptor> GetDescriptors(GameObject obj)
		{
			List<Descriptor> list = new List<Descriptor>();
			list.Add(new Descriptor(UI.BUILDINGEFFECTS.DIET_HEADER, UI.BUILDINGEFFECTS.TOOLTIPS.DIET_HEADER, Descriptor.DescriptorType.Effect, false));
			CreatureCalorieMonitor.Stomach stomach = obj.GetSMI<CreatureCalorieMonitor.Instance>().stomach;
			float calorie_loss_per_second = 0f;
			foreach (AttributeModifier attributeModifier in Db.Get().traits.Get(obj.GetComponent<Modifiers>().initialTraits[0]).SelfModifiers)
			{
				if (attributeModifier.AttributeId == Db.Get().Amounts.Calories.deltaAttribute.Id)
				{
					calorie_loss_per_second = attributeModifier.Value;
				}
			}
			if (stomach.diet.consumedTags.Count > 0)
			{
				string newValue = string.Join(", ", (from t in stomach.diet.consumedTags
				select t.Key.ProperName()).ToArray<string>());
				string text = "";
				if (stomach.diet.CanEatAnyPlantDirectly)
				{
					text = string.Join("\n", (from s in stomach.diet.consumedTags.Select(delegate(KeyValuePair<Tag, float> t)
					{
						float consumer_caloriesLossPerCaloriesPerKG = -calorie_loss_per_second / t.Value;
						GameObject prefab = Assets.GetPrefab(t.Key.ToString());
						IPlantConsumptionInstructions plantConsumptionInstructions = prefab.GetComponent<IPlantConsumptionInstructions>();
						plantConsumptionInstructions = ((plantConsumptionInstructions != null) ? plantConsumptionInstructions : prefab.GetSMI<IPlantConsumptionInstructions>());
						if (plantConsumptionInstructions == null)
						{
							return null;
						}
						return UI.BUILDINGEFFECTS.DIET_CONSUMED_ITEM.text.Replace("{Food}", t.Key.ProperName()).Replace("{Amount}", plantConsumptionInstructions.GetFormattedConsumptionPerCycle(consumer_caloriesLossPerCaloriesPerKG));
					})
					where !string.IsNullOrEmpty(s)
					select s).ToArray<string>());
				}
				if (this.diet.CanEatAnyNonDirectlyEdiblePlant)
				{
					if (this.diet.CanEatAnyPlantDirectly)
					{
						text += "\n";
					}
					Diet.Info info;
					text += string.Join("\n", (from t in stomach.diet.consumedTags.FindAll((KeyValuePair<Tag, float> t) => this.diet.directlyEatenPlantInfos.FirstOrDefault((Diet.Info info) => info.consumedTags.Contains(t.Key)) == null)
					select UI.BUILDINGEFFECTS.DIET_CONSUMED_ITEM.text.Replace("{Food}", t.Key.ProperName()).Replace("{Amount}", GameUtil.GetFormattedMass(-calorie_loss_per_second / t.Value, GameUtil.TimeSlice.PerCycle, GameUtil.MetricMassFormat.Kilogram, true, "{0:0.#}"))).ToArray<string>());
				}
				list.Add(new Descriptor(UI.BUILDINGEFFECTS.DIET_CONSUMED.text.Replace("{Foodlist}", newValue), UI.BUILDINGEFFECTS.TOOLTIPS.DIET_CONSUMED.text.Replace("{Foodlist}", text), Descriptor.DescriptorType.Effect, false));
			}
			if (stomach.diet.producedTags.Count > 0)
			{
				string newValue2 = string.Join(", ", (from t in stomach.diet.producedTags
				select t.Key.ProperName()).ToArray<string>());
				string text2 = "";
				if (stomach.diet.CanEatAnyPlantDirectly)
				{
					List<KeyValuePair<Tag, float>> list2 = new List<KeyValuePair<Tag, float>>();
					foreach (KeyValuePair<Tag, float> keyValuePair in stomach.diet.producedTags)
					{
						foreach (Diet.Info info in this.diet.directlyEatenPlantInfos)
						{
							if (info.producedElement == keyValuePair.Key)
							{
								float consumed_mass = -calorie_loss_per_second / info.caloriesPerKg * 600f;
								float num = info.ConvertConsumptionMassToProducedMass(consumed_mass);
								list2.Add(new KeyValuePair<Tag, float>(keyValuePair.Key, num / 600f));
							}
						}
					}
					text2 = string.Join("\n", (from t in list2
					select UI.BUILDINGEFFECTS.DIET_PRODUCED_ITEM_FROM_PLANT.text.Replace("{Item}", t.Key.ProperName()).Replace("{Amount}", GameUtil.GetFormattedMass(t.Value, GameUtil.TimeSlice.PerCycle, GameUtil.MetricMassFormat.Kilogram, true, "{0:0.#}"))).ToArray<string>());
					text2 += "\n";
				}
				else if (stomach.diet.CanEatAnyNonDirectlyEdiblePlant)
				{
					List<KeyValuePair<Tag, float>> list3 = new List<KeyValuePair<Tag, float>>();
					foreach (KeyValuePair<Tag, float> keyValuePair2 in stomach.diet.producedTags)
					{
						foreach (Diet.Info info2 in this.diet.noPlantInfos)
						{
							if (info2.producedElement == keyValuePair2.Key)
							{
								list3.Add(new KeyValuePair<Tag, float>(info2.producedElement, info2.producedConversionRate));
							}
						}
					}
					text2 += string.Join("\n", (from t in list3
					select UI.BUILDINGEFFECTS.DIET_PRODUCED_ITEM.text.Replace("{Item}", t.Key.ProperName()).Replace("{Percent}", GameUtil.GetFormattedPercent(t.Value * 100f, GameUtil.TimeSlice.None))).ToArray<string>());
				}
				list.Add(new Descriptor(UI.BUILDINGEFFECTS.DIET_PRODUCED.text.Replace("{Items}", newValue2), UI.BUILDINGEFFECTS.TOOLTIPS.DIET_PRODUCED.text.Replace("{Items}", text2), Descriptor.DescriptorType.Effect, false));
			}
			return list;
		}

		// Token: 0x04001F4F RID: 8015
		public Diet diet;

		// Token: 0x04001F50 RID: 8016
		public float minConsumedCaloriesBeforePooping = 100f;

		// Token: 0x04001F51 RID: 8017
		public float maxPoopSizeKG = -1f;

		// Token: 0x04001F52 RID: 8018
		public float minimumTimeBeforePooping = 10f;

		// Token: 0x04001F53 RID: 8019
		public float deathTimer = 6000f;

		// Token: 0x04001F54 RID: 8020
		public bool storePoop;
	}

	// Token: 0x020009EC RID: 2540
	public class PauseStates : GameStateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.State
	{
		// Token: 0x04001F5E RID: 8030
		public GameStateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.State commonPause;

		// Token: 0x04001F5F RID: 8031
		public GameStateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.State starvingPause;
	}

	// Token: 0x020009ED RID: 2541
	public class HungryStates : GameStateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.State
	{
		// Token: 0x04001F60 RID: 8032
		public GameStateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.State hungry;

		// Token: 0x04001F61 RID: 8033
		public CreatureCalorieMonitor.HungryStates.OutOfCaloriesState outofcalories;

		// Token: 0x020009EE RID: 2542
		public class OutOfCaloriesState : GameStateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.State
		{
			// Token: 0x04001F62 RID: 8034
			public GameStateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.State wild;

			// Token: 0x04001F63 RID: 8035
			public GameStateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.State tame;

			// Token: 0x04001F64 RID: 8036
			public GameStateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.State starvedtodeath;
		}
	}

	// Token: 0x020009EF RID: 2543
	[SerializationConfig(MemberSerialization.OptIn)]
	public class Stomach
	{
		// Token: 0x170001CE RID: 462
		// (get) Token: 0x06002EB9 RID: 11961 RVA: 0x000BE46B File Offset: 0x000BC66B
		// (set) Token: 0x06002EBA RID: 11962 RVA: 0x000BE473 File Offset: 0x000BC673
		public Diet diet { get; private set; }

		// Token: 0x06002EBB RID: 11963 RVA: 0x001F5C10 File Offset: 0x001F3E10
		public Stomach(GameObject owner, float minConsumedCaloriesBeforePooping, float max_poop_size_in_kg, bool storePoop)
		{
			this.diet = DietManager.Instance.GetPrefabDiet(owner);
			this.owner = owner;
			this.minConsumedCaloriesBeforePooping = minConsumedCaloriesBeforePooping;
			this.storePoop = storePoop;
			this.maxPoopSizeInKG = max_poop_size_in_kg;
		}

		// Token: 0x06002EBC RID: 11964 RVA: 0x001F5C5C File Offset: 0x001F3E5C
		public void Poop()
		{
			this.shouldContinuingPooping = true;
			float num = 0f;
			Tag tag = Tag.Invalid;
			byte disease_idx = byte.MaxValue;
			int num2 = 0;
			int num3 = 0;
			bool flag = false;
			for (int i = 0; i < this.caloriesConsumed.Count; i++)
			{
				CreatureCalorieMonitor.Stomach.CaloriesConsumedEntry caloriesConsumedEntry = this.caloriesConsumed[i];
				if (caloriesConsumedEntry.calories > 0f)
				{
					Diet.Info dietInfo = this.diet.GetDietInfo(caloriesConsumedEntry.tag);
					if (dietInfo != null && (!(tag != Tag.Invalid) || !(tag != dietInfo.producedElement)))
					{
						float num4 = (this.maxPoopSizeInKG < 0f) ? float.MaxValue : this.maxPoopSizeInKG;
						float b = Mathf.Clamp(num4 - num, 0f, num4);
						float num5 = Mathf.Min(dietInfo.ConvertConsumptionMassToProducedMass(dietInfo.ConvertCaloriesToConsumptionMass(caloriesConsumedEntry.calories)), b);
						num += num5;
						tag = dietInfo.producedElement;
						if (dietInfo.diseaseIdx != 255)
						{
							disease_idx = dietInfo.diseaseIdx;
							if (!this.storePoop && dietInfo.emmitDiseaseOnCell)
							{
								num3 += (int)(dietInfo.diseasePerKgProduced * num5);
							}
							else
							{
								num2 += (int)(dietInfo.diseasePerKgProduced * num5);
							}
						}
						caloriesConsumedEntry.calories = Mathf.Clamp(caloriesConsumedEntry.calories - dietInfo.ConvertConsumptionMassToCalories(dietInfo.ConvertProducedMassToConsumptionMass(num5)), 0f, float.MaxValue);
						this.caloriesConsumed[i] = caloriesConsumedEntry;
						flag = (flag || dietInfo.produceSolidTile);
					}
				}
			}
			if (num <= 0f || tag == Tag.Invalid)
			{
				this.shouldContinuingPooping = false;
				return;
			}
			string text = null;
			Element element = ElementLoader.GetElement(tag);
			if (element != null)
			{
				text = element.name;
			}
			int num6 = Grid.PosToCell(this.owner.transform.GetPosition());
			float temperature = this.owner.GetComponent<PrimaryElement>().Temperature;
			DebugUtil.DevAssert(!this.storePoop || !flag, "Stomach cannot both store poop & create a solid tile.", null);
			if (this.storePoop)
			{
				Storage component = this.owner.GetComponent<Storage>();
				if (element == null)
				{
					GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab(tag), Grid.CellToPos(num6, CellAlignment.Top, Grid.SceneLayer.Ore), Grid.SceneLayer.Ore, null, 0);
					PrimaryElement component2 = gameObject.GetComponent<PrimaryElement>();
					component2.Mass = num;
					component2.AddDisease(disease_idx, num2, "CreatureCalorieMonitor.Poop");
					component2.Temperature = temperature;
					gameObject.SetActive(true);
					component.Store(gameObject, true, false, true, false);
					text = gameObject.GetProperName();
				}
				else if (element.IsLiquid)
				{
					component.AddLiquid(element.id, num, temperature, disease_idx, num2, false, true);
				}
				else if (element.IsGas)
				{
					component.AddGasChunk(element.id, num, temperature, disease_idx, num2, false, true);
				}
				else
				{
					component.AddOre(element.id, num, temperature, disease_idx, num2, false, true);
				}
			}
			else
			{
				if (element == null)
				{
					GameObject gameObject2 = GameUtil.KInstantiate(Assets.GetPrefab(tag), Grid.CellToPos(num6, CellAlignment.Top, Grid.SceneLayer.Ore), Grid.SceneLayer.Ore, null, 0);
					PrimaryElement component3 = gameObject2.GetComponent<PrimaryElement>();
					component3.Mass = num;
					component3.AddDisease(disease_idx, num2, "CreatureCalorieMonitor.Poop");
					component3.Temperature = temperature;
					gameObject2.SetActive(true);
					text = gameObject2.GetProperName();
				}
				else if (element.IsLiquid)
				{
					FallingWater.instance.AddParticle(num6, element.idx, num, temperature, disease_idx, num2, true, false, false, false);
				}
				else if (element.IsGas)
				{
					SimMessages.AddRemoveSubstance(num6, element.idx, CellEventLogger.Instance.ElementConsumerSimUpdate, num, temperature, disease_idx, num2, true, -1);
				}
				else if (flag)
				{
					int num7 = this.owner.GetComponent<Facing>().GetFrontCell();
					if (!Grid.IsValidCell(num7))
					{
						global::Debug.LogWarningFormat("{0} attemping to Poop {1} on invalid cell {2} from cell {3}", new object[]
						{
							this.owner,
							element.name,
							num7,
							num6
						});
						num7 = num6;
					}
					SimMessages.AddRemoveSubstance(num7, element.idx, CellEventLogger.Instance.ElementConsumerSimUpdate, num, temperature, disease_idx, num2, true, -1);
				}
				else
				{
					element.substance.SpawnResource(Grid.CellToPosCCC(num6, Grid.SceneLayer.Ore), num, temperature, disease_idx, num2, false, false, false);
				}
				if (num3 > 0)
				{
					SimMessages.ModifyDiseaseOnCell(num6, disease_idx, num3);
				}
			}
			if (this.GetTotalConsumedCalories() <= 0f)
			{
				this.shouldContinuingPooping = false;
			}
			KPrefabID component4 = this.owner.GetComponent<KPrefabID>();
			if (!Game.Instance.savedInfo.creaturePoopAmount.ContainsKey(component4.PrefabTag))
			{
				Game.Instance.savedInfo.creaturePoopAmount.Add(component4.PrefabTag, 0f);
			}
			Dictionary<Tag, float> creaturePoopAmount = Game.Instance.savedInfo.creaturePoopAmount;
			Tag prefabTag = component4.PrefabTag;
			creaturePoopAmount[prefabTag] += num;
			PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, text, this.owner.transform, 1.5f, false);
			this.owner.Trigger(-1844238272, null);
		}

		// Token: 0x06002EBD RID: 11965 RVA: 0x000BE47C File Offset: 0x000BC67C
		public List<CreatureCalorieMonitor.Stomach.CaloriesConsumedEntry> GetCalorieEntries()
		{
			return this.caloriesConsumed;
		}

		// Token: 0x06002EBE RID: 11966 RVA: 0x001F615C File Offset: 0x001F435C
		public float GetTotalConsumedCalories()
		{
			float num = 0f;
			foreach (CreatureCalorieMonitor.Stomach.CaloriesConsumedEntry caloriesConsumedEntry in this.caloriesConsumed)
			{
				if (caloriesConsumedEntry.calories > 0f)
				{
					Diet.Info dietInfo = this.diet.GetDietInfo(caloriesConsumedEntry.tag);
					if (dietInfo != null && !(dietInfo.producedElement == Tag.Invalid))
					{
						num += caloriesConsumedEntry.calories;
					}
				}
			}
			return num;
		}

		// Token: 0x06002EBF RID: 11967 RVA: 0x000BE484 File Offset: 0x000BC684
		public float GetFullness()
		{
			return this.GetTotalConsumedCalories() / this.minConsumedCaloriesBeforePooping;
		}

		// Token: 0x06002EC0 RID: 11968 RVA: 0x001F61EC File Offset: 0x001F43EC
		public bool IsReadyToPoop()
		{
			float totalConsumedCalories = this.GetTotalConsumedCalories();
			return totalConsumedCalories > 0f && (this.shouldContinuingPooping || totalConsumedCalories >= this.minConsumedCaloriesBeforePooping);
		}

		// Token: 0x06002EC1 RID: 11969 RVA: 0x001F6220 File Offset: 0x001F4420
		public void Consume(Tag tag, float calories)
		{
			for (int i = 0; i < this.caloriesConsumed.Count; i++)
			{
				CreatureCalorieMonitor.Stomach.CaloriesConsumedEntry caloriesConsumedEntry = this.caloriesConsumed[i];
				if (caloriesConsumedEntry.tag == tag)
				{
					caloriesConsumedEntry.calories += calories;
					this.caloriesConsumed[i] = caloriesConsumedEntry;
					this.caloriesConsumed[i] = caloriesConsumedEntry;
					return;
				}
			}
			CreatureCalorieMonitor.Stomach.CaloriesConsumedEntry item = default(CreatureCalorieMonitor.Stomach.CaloriesConsumedEntry);
			item.tag = tag;
			item.calories = calories;
			this.caloriesConsumed.Add(item);
		}

		// Token: 0x06002EC2 RID: 11970 RVA: 0x001F62AC File Offset: 0x001F44AC
		public Tag GetNextPoopEntry()
		{
			for (int i = 0; i < this.caloriesConsumed.Count; i++)
			{
				CreatureCalorieMonitor.Stomach.CaloriesConsumedEntry caloriesConsumedEntry = this.caloriesConsumed[i];
				if (caloriesConsumedEntry.calories > 0f)
				{
					Diet.Info dietInfo = this.diet.GetDietInfo(caloriesConsumedEntry.tag);
					if (dietInfo != null && !(dietInfo.producedElement == Tag.Invalid))
					{
						return dietInfo.producedElement;
					}
				}
			}
			return Tag.Invalid;
		}

		// Token: 0x04001F65 RID: 8037
		[Serialize]
		private List<CreatureCalorieMonitor.Stomach.CaloriesConsumedEntry> caloriesConsumed = new List<CreatureCalorieMonitor.Stomach.CaloriesConsumedEntry>();

		// Token: 0x04001F66 RID: 8038
		[Serialize]
		private bool shouldContinuingPooping;

		// Token: 0x04001F67 RID: 8039
		private float minConsumedCaloriesBeforePooping;

		// Token: 0x04001F68 RID: 8040
		private float maxPoopSizeInKG;

		// Token: 0x04001F6A RID: 8042
		private GameObject owner;

		// Token: 0x04001F6B RID: 8043
		private bool storePoop;

		// Token: 0x020009F0 RID: 2544
		[Serializable]
		public struct CaloriesConsumedEntry
		{
			// Token: 0x04001F6C RID: 8044
			public Tag tag;

			// Token: 0x04001F6D RID: 8045
			public float calories;
		}
	}

	// Token: 0x020009F1 RID: 2545
	public new class Instance : GameStateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.GameInstance
	{
		// Token: 0x06002EC3 RID: 11971 RVA: 0x001F631C File Offset: 0x001F451C
		public Instance(IStateMachineTarget master, CreatureCalorieMonitor.Def def) : base(master, def)
		{
			this.calories = Db.Get().Amounts.Calories.Lookup(base.gameObject);
			this.calories.value = this.calories.GetMax() * 0.9f;
			this.stomach = new CreatureCalorieMonitor.Stomach(master.gameObject, def.minConsumedCaloriesBeforePooping, def.maxPoopSizeKG, def.storePoop);
			this.metabolism = base.gameObject.GetAttributes().Add(Db.Get().CritterAttributes.Metabolism);
			this.deltaCalorieMetabolismModifier = new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, 1f, DUPLICANTS.MODIFIERS.METABOLISM_CALORIE_MODIFIER.NAME, true, false, false);
			this.calories.deltaAttribute.Add(this.deltaCalorieMetabolismModifier);
		}

		// Token: 0x06002EC4 RID: 11972 RVA: 0x000BE493 File Offset: 0x000BC693
		public override void StartSM()
		{
			this.prefabID = base.gameObject.GetComponent<KPrefabID>();
			base.StartSM();
		}

		// Token: 0x06002EC5 RID: 11973 RVA: 0x001F6404 File Offset: 0x001F4604
		public void OnCaloriesConsumed(object data)
		{
			CreatureCalorieMonitor.CaloriesConsumedEvent caloriesConsumedEvent = (CreatureCalorieMonitor.CaloriesConsumedEvent)data;
			this.calories.value += caloriesConsumedEvent.calories;
			this.stomach.Consume(caloriesConsumedEvent.tag, caloriesConsumedEvent.calories);
			this.lastMealOrPoopTime = Time.time;
		}

		// Token: 0x06002EC6 RID: 11974 RVA: 0x000BE4AC File Offset: 0x000BC6AC
		public float GetDeathTimeRemaining()
		{
			return base.smi.def.deathTimer - (GameClock.Instance.GetTime() - base.sm.starvationStartTime.Get(base.smi));
		}

		// Token: 0x06002EC7 RID: 11975 RVA: 0x000BE4E0 File Offset: 0x000BC6E0
		public void Poop()
		{
			this.lastMealOrPoopTime = Time.time;
			this.stomach.Poop();
		}

		// Token: 0x06002EC8 RID: 11976 RVA: 0x000BE4F8 File Offset: 0x000BC6F8
		public float GetCalories0to1()
		{
			return this.calories.value / this.calories.GetMax();
		}

		// Token: 0x06002EC9 RID: 11977 RVA: 0x000BE511 File Offset: 0x000BC711
		public bool IsHungry()
		{
			return this.GetCalories0to1() < 0.9f;
		}

		// Token: 0x06002ECA RID: 11978 RVA: 0x000BE520 File Offset: 0x000BC720
		public bool IsOutOfCalories()
		{
			return this.GetCalories0to1() <= 0f;
		}

		// Token: 0x04001F6E RID: 8046
		public const float HUNGRY_RATIO = 0.9f;

		// Token: 0x04001F6F RID: 8047
		public AmountInstance calories;

		// Token: 0x04001F70 RID: 8048
		[Serialize]
		public CreatureCalorieMonitor.Stomach stomach;

		// Token: 0x04001F71 RID: 8049
		public float lastMealOrPoopTime;

		// Token: 0x04001F72 RID: 8050
		public AttributeInstance metabolism;

		// Token: 0x04001F73 RID: 8051
		public AttributeModifier deltaCalorieMetabolismModifier;

		// Token: 0x04001F74 RID: 8052
		public KPrefabID prefabID;
	}
}
