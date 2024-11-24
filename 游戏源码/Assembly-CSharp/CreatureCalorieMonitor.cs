using System;
using System.Collections.Generic;
using System.Linq;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

public class CreatureCalorieMonitor : GameStateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>
{
	public struct CaloriesConsumedEvent
	{
		public Tag tag;

		public float calories;
	}

	public class Def : BaseDef, IGameObjectEffectDescriptor
	{
		public Diet diet;

		public float minConsumedCaloriesBeforePooping = 100f;

		public float maxPoopSizeKG = -1f;

		public float minimumTimeBeforePooping = 10f;

		public float deathTimer = 6000f;

		public bool storePoop;

		public override void Configure(GameObject prefab)
		{
			prefab.GetComponent<Modifiers>().initialAmounts.Add(Db.Get().Amounts.Calories.Id);
		}

		public List<Descriptor> GetDescriptors(GameObject obj)
		{
			List<Descriptor> list = new List<Descriptor>();
			list.Add(new Descriptor(UI.BUILDINGEFFECTS.DIET_HEADER, UI.BUILDINGEFFECTS.TOOLTIPS.DIET_HEADER));
			Stomach stomach = obj.GetSMI<Instance>().stomach;
			float calorie_loss_per_second = 0f;
			foreach (AttributeModifier selfModifier in Db.Get().traits.Get(obj.GetComponent<Modifiers>().initialTraits[0]).SelfModifiers)
			{
				if (selfModifier.AttributeId == Db.Get().Amounts.Calories.deltaAttribute.Id)
				{
					calorie_loss_per_second = selfModifier.Value;
				}
			}
			if (stomach.diet.consumedTags.Count > 0)
			{
				string newValue = string.Join(", ", stomach.diet.consumedTags.Select((KeyValuePair<Tag, float> t) => t.Key.ProperName()).ToArray());
				string text = "";
				if (stomach.diet.CanEatAnyPlantDirectly)
				{
					text = string.Join("\n", stomach.diet.consumedTags.Select(delegate(KeyValuePair<Tag, float> t)
					{
						float consumer_caloriesLossPerCaloriesPerKG = (0f - calorie_loss_per_second) / t.Value;
						GameObject prefab = Assets.GetPrefab(t.Key.ToString());
						IPlantConsumptionInstructions component = prefab.GetComponent<IPlantConsumptionInstructions>();
						component = ((component != null) ? component : prefab.GetSMI<IPlantConsumptionInstructions>());
						return (component == null) ? "" : UI.BUILDINGEFFECTS.DIET_CONSUMED_ITEM.text.Replace("{Food}", t.Key.ProperName()).Replace("{Amount}", component.GetFormattedConsumptionPerCycle(consumer_caloriesLossPerCaloriesPerKG));
					}).ToArray());
					text += "\n";
				}
				if (diet.CanEatAnyNonDirectlyEdiblePlant)
				{
					text += string.Join("\n", (from t in stomach.diet.consumedTags.FindAll((KeyValuePair<Tag, float> t) => diet.directlyEatenPlantInfos.FirstOrDefault((Diet.Info info) => info.consumedTags.Contains(t.Key)) == null)
						select UI.BUILDINGEFFECTS.DIET_CONSUMED_ITEM.text.Replace("{Food}", t.Key.ProperName()).Replace("{Amount}", GameUtil.GetFormattedMass((0f - calorie_loss_per_second) / t.Value, GameUtil.TimeSlice.PerCycle, GameUtil.MetricMassFormat.Kilogram))).ToArray());
				}
				list.Add(new Descriptor(UI.BUILDINGEFFECTS.DIET_CONSUMED.text.Replace("{Foodlist}", newValue), UI.BUILDINGEFFECTS.TOOLTIPS.DIET_CONSUMED.text.Replace("{Foodlist}", text)));
			}
			if (stomach.diet.producedTags.Count > 0)
			{
				string newValue2 = string.Join(", ", stomach.diet.producedTags.Select((KeyValuePair<Tag, float> t) => t.Key.ProperName()).ToArray());
				string text2 = "";
				if (stomach.diet.CanEatAnyPlantDirectly)
				{
					List<KeyValuePair<Tag, float>> list2 = new List<KeyValuePair<Tag, float>>();
					foreach (KeyValuePair<Tag, float> producedTag in stomach.diet.producedTags)
					{
						Diet.Info[] directlyEatenPlantInfos = diet.directlyEatenPlantInfos;
						foreach (Diet.Info info2 in directlyEatenPlantInfos)
						{
							if (info2.producedElement == producedTag.Key)
							{
								float consumed_mass = (0f - calorie_loss_per_second) / info2.caloriesPerKg * 600f;
								float num = info2.ConvertConsumptionMassToProducedMass(consumed_mass);
								list2.Add(new KeyValuePair<Tag, float>(producedTag.Key, num / 600f));
							}
						}
					}
					text2 = string.Join("\n", list2.Select((KeyValuePair<Tag, float> t) => UI.BUILDINGEFFECTS.DIET_PRODUCED_ITEM_FROM_PLANT.text.Replace("{Item}", t.Key.ProperName()).Replace("{Amount}", GameUtil.GetFormattedMass(t.Value, GameUtil.TimeSlice.PerCycle, GameUtil.MetricMassFormat.Kilogram))).ToArray());
					text2 += "\n";
				}
				else if (stomach.diet.CanEatAnyNonDirectlyEdiblePlant)
				{
					List<KeyValuePair<Tag, float>> list3 = new List<KeyValuePair<Tag, float>>();
					foreach (KeyValuePair<Tag, float> producedTag2 in stomach.diet.producedTags)
					{
						Diet.Info[] directlyEatenPlantInfos = diet.noPlantInfos;
						foreach (Diet.Info info3 in directlyEatenPlantInfos)
						{
							if (info3.producedElement == producedTag2.Key)
							{
								list3.Add(new KeyValuePair<Tag, float>(info3.producedElement, info3.producedConversionRate));
							}
						}
					}
					text2 += string.Join("\n", list3.Select((KeyValuePair<Tag, float> t) => UI.BUILDINGEFFECTS.DIET_PRODUCED_ITEM.text.Replace("{Item}", t.Key.ProperName()).Replace("{Percent}", GameUtil.GetFormattedPercent(t.Value * 100f))).ToArray());
				}
				list.Add(new Descriptor(UI.BUILDINGEFFECTS.DIET_PRODUCED.text.Replace("{Items}", newValue2), UI.BUILDINGEFFECTS.TOOLTIPS.DIET_PRODUCED.text.Replace("{Items}", text2)));
			}
			return list;
		}
	}

	public class PauseStates : State
	{
		public State commonPause;

		public State starvingPause;
	}

	public class HungryStates : State
	{
		public class OutOfCaloriesState : State
		{
			public State wild;

			public State tame;

			public State starvedtodeath;
		}

		public State hungry;

		public OutOfCaloriesState outofcalories;
	}

	[SerializationConfig(MemberSerialization.OptIn)]
	public class Stomach
	{
		[Serializable]
		public struct CaloriesConsumedEntry
		{
			public Tag tag;

			public float calories;
		}

		[Serialize]
		private List<CaloriesConsumedEntry> caloriesConsumed = new List<CaloriesConsumedEntry>();

		[Serialize]
		private bool shouldContinuingPooping;

		private float minConsumedCaloriesBeforePooping;

		private float maxPoopSizeInKG;

		private GameObject owner;

		private bool storePoop;

		public Diet diet { get; private set; }

		public Stomach(GameObject owner, float minConsumedCaloriesBeforePooping, float max_poop_size_in_kg, bool storePoop)
		{
			diet = DietManager.Instance.GetPrefabDiet(owner);
			this.owner = owner;
			this.minConsumedCaloriesBeforePooping = minConsumedCaloriesBeforePooping;
			this.storePoop = storePoop;
			maxPoopSizeInKG = max_poop_size_in_kg;
		}

		public void Poop()
		{
			shouldContinuingPooping = true;
			float num = 0f;
			Tag tag = Tag.Invalid;
			byte disease_idx = byte.MaxValue;
			int num2 = 0;
			int num3 = 0;
			bool flag = false;
			for (int i = 0; i < caloriesConsumed.Count; i++)
			{
				CaloriesConsumedEntry value = caloriesConsumed[i];
				if (value.calories <= 0f)
				{
					continue;
				}
				Diet.Info dietInfo = diet.GetDietInfo(value.tag);
				if (dietInfo == null || (tag != Tag.Invalid && tag != dietInfo.producedElement))
				{
					continue;
				}
				float num4 = ((maxPoopSizeInKG < 0f) ? float.MaxValue : maxPoopSizeInKG);
				float b = Mathf.Clamp(num4 - num, 0f, num4);
				float num5 = Mathf.Min(dietInfo.ConvertConsumptionMassToProducedMass(dietInfo.ConvertCaloriesToConsumptionMass(value.calories)), b);
				num += num5;
				tag = dietInfo.producedElement;
				if (dietInfo.diseaseIdx != byte.MaxValue)
				{
					disease_idx = dietInfo.diseaseIdx;
					if (!storePoop && dietInfo.emmitDiseaseOnCell)
					{
						num3 += (int)(dietInfo.diseasePerKgProduced * num5);
					}
					else
					{
						num2 += (int)(dietInfo.diseasePerKgProduced * num5);
					}
				}
				value.calories = Mathf.Clamp(value.calories - dietInfo.ConvertConsumptionMassToCalories(dietInfo.ConvertProducedMassToConsumptionMass(num5)), 0f, float.MaxValue);
				caloriesConsumed[i] = value;
				flag = flag || dietInfo.produceSolidTile;
			}
			if (num <= 0f || tag == Tag.Invalid)
			{
				shouldContinuingPooping = false;
				return;
			}
			string text = null;
			Element element = ElementLoader.GetElement(tag);
			if (element != null)
			{
				text = element.name;
			}
			int num6 = Grid.PosToCell(owner.transform.GetPosition());
			float temperature = owner.GetComponent<PrimaryElement>().Temperature;
			DebugUtil.DevAssert(!(storePoop && flag), "Stomach cannot both store poop & create a solid tile.");
			if (storePoop)
			{
				Storage component = owner.GetComponent<Storage>();
				if (element == null)
				{
					GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab(tag), Grid.CellToPos(num6, CellAlignment.Top, Grid.SceneLayer.Ore), Grid.SceneLayer.Ore);
					PrimaryElement component2 = gameObject.GetComponent<PrimaryElement>();
					component2.Mass = num;
					component2.AddDisease(disease_idx, num2, "CreatureCalorieMonitor.Poop");
					component2.Temperature = temperature;
					gameObject.SetActive(value: true);
					component.Store(gameObject, hide_popups: true);
					text = gameObject.GetProperName();
				}
				else if (element.IsLiquid)
				{
					component.AddLiquid(element.id, num, temperature, disease_idx, num2);
				}
				else if (element.IsGas)
				{
					component.AddGasChunk(element.id, num, temperature, disease_idx, num2, keep_zero_mass: false);
				}
				else
				{
					component.AddOre(element.id, num, temperature, disease_idx, num2);
				}
			}
			else
			{
				if (element == null)
				{
					GameObject gameObject2 = GameUtil.KInstantiate(Assets.GetPrefab(tag), Grid.CellToPos(num6, CellAlignment.Top, Grid.SceneLayer.Ore), Grid.SceneLayer.Ore);
					PrimaryElement component3 = gameObject2.GetComponent<PrimaryElement>();
					component3.Mass = num;
					component3.AddDisease(disease_idx, num2, "CreatureCalorieMonitor.Poop");
					component3.Temperature = temperature;
					gameObject2.SetActive(value: true);
					text = gameObject2.GetProperName();
				}
				else if (element.IsLiquid)
				{
					FallingWater.instance.AddParticle(num6, element.idx, num, temperature, disease_idx, num2, skip_sound: true);
				}
				else if (element.IsGas)
				{
					SimMessages.AddRemoveSubstance(num6, element.idx, CellEventLogger.Instance.ElementConsumerSimUpdate, num, temperature, disease_idx, num2);
				}
				else if (flag)
				{
					int num7 = owner.GetComponent<Facing>().GetFrontCell();
					if (!Grid.IsValidCell(num7))
					{
						Debug.LogWarningFormat("{0} attemping to Poop {1} on invalid cell {2} from cell {3}", owner, element.name, num7, num6);
						num7 = num6;
					}
					SimMessages.AddRemoveSubstance(num7, element.idx, CellEventLogger.Instance.ElementConsumerSimUpdate, num, temperature, disease_idx, num2);
				}
				else
				{
					element.substance.SpawnResource(Grid.CellToPosCCC(num6, Grid.SceneLayer.Ore), num, temperature, disease_idx, num2);
				}
				if (num3 > 0)
				{
					SimMessages.ModifyDiseaseOnCell(num6, disease_idx, num3);
				}
			}
			if (GetTotalConsumedCalories() <= 0f)
			{
				shouldContinuingPooping = false;
			}
			KPrefabID component4 = owner.GetComponent<KPrefabID>();
			if (!Game.Instance.savedInfo.creaturePoopAmount.ContainsKey(component4.PrefabTag))
			{
				Game.Instance.savedInfo.creaturePoopAmount.Add(component4.PrefabTag, 0f);
			}
			Game.Instance.savedInfo.creaturePoopAmount[component4.PrefabTag] += num;
			PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, text, owner.transform);
			owner.Trigger(-1844238272);
		}

		public List<CaloriesConsumedEntry> GetCalorieEntries()
		{
			return caloriesConsumed;
		}

		public float GetTotalConsumedCalories()
		{
			float num = 0f;
			foreach (CaloriesConsumedEntry item in caloriesConsumed)
			{
				if (!(item.calories <= 0f))
				{
					Diet.Info dietInfo = diet.GetDietInfo(item.tag);
					if (dietInfo != null && !(dietInfo.producedElement == Tag.Invalid))
					{
						num += item.calories;
					}
				}
			}
			return num;
		}

		public float GetFullness()
		{
			return GetTotalConsumedCalories() / minConsumedCaloriesBeforePooping;
		}

		public bool IsReadyToPoop()
		{
			float totalConsumedCalories = GetTotalConsumedCalories();
			if (totalConsumedCalories > 0f)
			{
				if (!shouldContinuingPooping)
				{
					return totalConsumedCalories >= minConsumedCaloriesBeforePooping;
				}
				return true;
			}
			return false;
		}

		public void Consume(Tag tag, float calories)
		{
			for (int i = 0; i < caloriesConsumed.Count; i++)
			{
				CaloriesConsumedEntry value = caloriesConsumed[i];
				if (value.tag == tag)
				{
					value.calories += calories;
					caloriesConsumed[i] = value;
					caloriesConsumed[i] = value;
					return;
				}
			}
			CaloriesConsumedEntry item = default(CaloriesConsumedEntry);
			item.tag = tag;
			item.calories = calories;
			caloriesConsumed.Add(item);
		}

		public Tag GetNextPoopEntry()
		{
			for (int i = 0; i < caloriesConsumed.Count; i++)
			{
				CaloriesConsumedEntry caloriesConsumedEntry = caloriesConsumed[i];
				if (!(caloriesConsumedEntry.calories <= 0f))
				{
					Diet.Info dietInfo = diet.GetDietInfo(caloriesConsumedEntry.tag);
					if (dietInfo != null && !(dietInfo.producedElement == Tag.Invalid))
					{
						return dietInfo.producedElement;
					}
				}
			}
			return Tag.Invalid;
		}
	}

	public new class Instance : GameInstance
	{
		public const float HUNGRY_RATIO = 0.9f;

		public AmountInstance calories;

		[Serialize]
		public Stomach stomach;

		public float lastMealOrPoopTime;

		public AttributeInstance metabolism;

		public AttributeModifier deltaCalorieMetabolismModifier;

		public KPrefabID prefabID;

		public Instance(IStateMachineTarget master, Def def)
			: base(master, def)
		{
			calories = Db.Get().Amounts.Calories.Lookup(base.gameObject);
			calories.value = calories.GetMax() * 0.9f;
			stomach = new Stomach(master.gameObject, def.minConsumedCaloriesBeforePooping, def.maxPoopSizeKG, def.storePoop);
			metabolism = base.gameObject.GetAttributes().Add(Db.Get().CritterAttributes.Metabolism);
			deltaCalorieMetabolismModifier = new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, 1f, DUPLICANTS.MODIFIERS.METABOLISM_CALORIE_MODIFIER.NAME, is_multiplier: true, uiOnly: false, is_readonly: false);
			calories.deltaAttribute.Add(deltaCalorieMetabolismModifier);
		}

		public override void StartSM()
		{
			prefabID = base.gameObject.GetComponent<KPrefabID>();
			base.StartSM();
		}

		public void OnCaloriesConsumed(object data)
		{
			CaloriesConsumedEvent caloriesConsumedEvent = (CaloriesConsumedEvent)data;
			calories.value += caloriesConsumedEvent.calories;
			stomach.Consume(caloriesConsumedEvent.tag, caloriesConsumedEvent.calories);
			lastMealOrPoopTime = Time.time;
		}

		public float GetDeathTimeRemaining()
		{
			return base.smi.def.deathTimer - (GameClock.Instance.GetTime() - base.sm.starvationStartTime.Get(base.smi));
		}

		public void Poop()
		{
			lastMealOrPoopTime = Time.time;
			stomach.Poop();
		}

		public float GetCalories0to1()
		{
			return calories.value / calories.GetMax();
		}

		public bool IsHungry()
		{
			return GetCalories0to1() < 0.9f;
		}

		public bool IsOutOfCalories()
		{
			return GetCalories0to1() <= 0f;
		}
	}

	public State normal;

	public PauseStates pause;

	private HungryStates hungry;

	private Effect outOfCaloriesTame;

	public FloatParameter starvationStartTime;

	public override void InitializeStates(out BaseState default_state)
	{
		default_state = normal;
		base.serializable = SerializeType.Both_DEPRECATED;
		root.EventHandler(GameHashes.CaloriesConsumed, delegate(Instance smi, object data)
		{
			smi.OnCaloriesConsumed(data);
		}).ToggleBehaviour(GameTags.Creatures.Poop, ReadyToPoop, delegate(Instance smi)
		{
			smi.Poop();
		}).Update(UpdateMetabolismCalorieModifier);
		normal.TagTransition(GameTags.Creatures.PausedHunger, pause.commonPause).Transition(hungry, (Instance smi) => smi.IsHungry(), UpdateRate.SIM_1000ms);
		hungry.DefaultState(hungry.hungry).ToggleTag(GameTags.Creatures.Hungry).EventTransition(GameHashes.CaloriesConsumed, normal, (Instance smi) => !smi.IsHungry());
		hungry.hungry.TagTransition(GameTags.Creatures.PausedHunger, pause.commonPause).Transition(normal, (Instance smi) => !smi.IsHungry(), UpdateRate.SIM_1000ms).Transition(hungry.outofcalories, (Instance smi) => smi.IsOutOfCalories(), UpdateRate.SIM_1000ms)
			.ToggleStatusItem(Db.Get().CreatureStatusItems.Hungry);
		hungry.outofcalories.DefaultState(hungry.outofcalories.wild).Transition(hungry.hungry, (Instance smi) => !smi.IsOutOfCalories(), UpdateRate.SIM_1000ms);
		hungry.outofcalories.wild.TagTransition(GameTags.Creatures.PausedHunger, pause.commonPause).TagTransition(GameTags.Creatures.Wild, hungry.outofcalories.tame, on_remove: true).ToggleStatusItem(Db.Get().CreatureStatusItems.Hungry);
		hungry.outofcalories.tame.Enter("StarvationStartTime", StarvationStartTime).Exit("ClearStarvationTime", delegate(Instance smi)
		{
			starvationStartTime.Set(Mathf.Min(0f - (GameClock.Instance.GetTime() - starvationStartTime.Get(smi)), 0f), smi);
		}).Transition(hungry.outofcalories.starvedtodeath, (Instance smi) => smi.GetDeathTimeRemaining() <= 0f, UpdateRate.SIM_1000ms)
			.TagTransition(GameTags.Creatures.PausedHunger, pause.starvingPause)
			.TagTransition(GameTags.Creatures.Wild, hungry.outofcalories.wild)
			.ToggleStatusItem(CREATURES.STATUSITEMS.STARVING.NAME, CREATURES.STATUSITEMS.STARVING.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.BadMinor, allow_multiples: false, default(HashedString), 129022, (string str, Instance smi) => str.Replace("{TimeUntilDeath}", GameUtil.GetFormattedCycles(smi.GetDeathTimeRemaining())))
			.ToggleNotification((Instance smi) => new Notification(CREATURES.STATUSITEMS.STARVING.NOTIFICATION_NAME, NotificationType.BadMinor, (List<Notification> notifications, object data) => string.Concat(CREATURES.STATUSITEMS.STARVING.NOTIFICATION_TOOLTIP, notifications.ReduceMessages(countNames: false))))
			.ToggleEffect((Instance smi) => outOfCaloriesTame);
		hungry.outofcalories.starvedtodeath.Enter(delegate(Instance smi)
		{
			smi.GetSMI<DeathMonitor.Instance>().Kill(Db.Get().Deaths.Starvation);
		});
		pause.commonPause.TagTransition(GameTags.Creatures.PausedHunger, normal, on_remove: true);
		pause.starvingPause.Exit("Recalculate StarvationStartTime", RecalculateStartTimeOnUnpause).TagTransition(GameTags.Creatures.PausedHunger, hungry.outofcalories.tame, on_remove: true);
		outOfCaloriesTame = new Effect("OutOfCaloriesTame", CREATURES.MODIFIERS.OUT_OF_CALORIES.NAME, CREATURES.MODIFIERS.OUT_OF_CALORIES.TOOLTIP, 0f, show_in_ui: false, trigger_floating_text: false, is_bad: false);
		outOfCaloriesTame.Add(new AttributeModifier(Db.Get().CritterAttributes.Happiness.Id, -10f, CREATURES.MODIFIERS.OUT_OF_CALORIES.NAME));
	}

	private static bool ReadyToPoop(Instance smi)
	{
		if (!smi.stomach.IsReadyToPoop())
		{
			return false;
		}
		if (Time.time - smi.lastMealOrPoopTime < smi.def.minimumTimeBeforePooping)
		{
			return false;
		}
		if (smi.IsInsideState(smi.sm.pause))
		{
			return false;
		}
		return true;
	}

	private static void UpdateMetabolismCalorieModifier(Instance smi, float dt)
	{
		if (!smi.IsInsideState(smi.sm.pause))
		{
			smi.deltaCalorieMetabolismModifier.SetValue(1f - smi.metabolism.GetTotalValue() / 100f);
		}
	}

	private static void StarvationStartTime(Instance smi)
	{
		if (smi.sm.starvationStartTime.Get(smi) <= 0f)
		{
			smi.sm.starvationStartTime.Set(GameClock.Instance.GetTime(), smi);
		}
	}

	private static void RecalculateStartTimeOnUnpause(Instance smi)
	{
		float num = smi.sm.starvationStartTime.Get(smi);
		if (num < 0f)
		{
			float value = GameClock.Instance.GetTime() - Mathf.Abs(num);
			smi.sm.starvationStartTime.Set(value, smi);
		}
	}
}
