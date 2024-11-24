using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Klei;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200117F RID: 4479
public class FertilityMonitor : GameStateMachine<FertilityMonitor, FertilityMonitor.Instance, IStateMachineTarget, FertilityMonitor.Def>
{
	// Token: 0x06005B66 RID: 23398 RVA: 0x0029756C File Offset: 0x0029576C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.fertile;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.root.DefaultState(this.fertile);
		this.fertile.ToggleBehaviour(GameTags.Creatures.Fertile, (FertilityMonitor.Instance smi) => smi.IsReadyToLayEgg(), null).ToggleEffect((FertilityMonitor.Instance smi) => smi.fertileEffect).Transition(this.infertile, GameStateMachine<FertilityMonitor, FertilityMonitor.Instance, IStateMachineTarget, FertilityMonitor.Def>.Not(new StateMachine<FertilityMonitor, FertilityMonitor.Instance, IStateMachineTarget, FertilityMonitor.Def>.Transition.ConditionCallback(FertilityMonitor.IsFertile)), UpdateRate.SIM_1000ms);
		this.infertile.Transition(this.fertile, new StateMachine<FertilityMonitor, FertilityMonitor.Instance, IStateMachineTarget, FertilityMonitor.Def>.Transition.ConditionCallback(FertilityMonitor.IsFertile), UpdateRate.SIM_1000ms);
	}

	// Token: 0x06005B67 RID: 23399 RVA: 0x000DB9FD File Offset: 0x000D9BFD
	public static bool IsFertile(FertilityMonitor.Instance smi)
	{
		return !smi.HasTag(GameTags.Creatures.PausedReproduction) && !smi.HasTag(GameTags.Creatures.Confined) && !smi.HasTag(GameTags.Creatures.Expecting);
	}

	// Token: 0x06005B68 RID: 23400 RVA: 0x0029762C File Offset: 0x0029582C
	public static Tag EggBreedingRoll(List<FertilityMonitor.BreedingChance> breedingChances, bool excludeOriginalCreature = false)
	{
		float num = UnityEngine.Random.value;
		if (excludeOriginalCreature)
		{
			num *= 1f - breedingChances[0].weight;
		}
		foreach (FertilityMonitor.BreedingChance breedingChance in breedingChances)
		{
			if (excludeOriginalCreature)
			{
				excludeOriginalCreature = false;
			}
			else
			{
				num -= breedingChance.weight;
				if (num <= 0f)
				{
					return breedingChance.egg;
				}
			}
		}
		return Tag.Invalid;
	}

	// Token: 0x0400408A RID: 16522
	private GameStateMachine<FertilityMonitor, FertilityMonitor.Instance, IStateMachineTarget, FertilityMonitor.Def>.State fertile;

	// Token: 0x0400408B RID: 16523
	private GameStateMachine<FertilityMonitor, FertilityMonitor.Instance, IStateMachineTarget, FertilityMonitor.Def>.State infertile;

	// Token: 0x02001180 RID: 4480
	[Serializable]
	public class BreedingChance
	{
		// Token: 0x0400408C RID: 16524
		public Tag egg;

		// Token: 0x0400408D RID: 16525
		public float weight;
	}

	// Token: 0x02001181 RID: 4481
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x06005B6B RID: 23403 RVA: 0x000DBA35 File Offset: 0x000D9C35
		public override void Configure(GameObject prefab)
		{
			prefab.AddOrGet<Modifiers>().initialAmounts.Add(Db.Get().Amounts.Fertility.Id);
		}

		// Token: 0x0400408E RID: 16526
		public Tag eggPrefab;

		// Token: 0x0400408F RID: 16527
		public List<FertilityMonitor.BreedingChance> initialBreedingWeights;

		// Token: 0x04004090 RID: 16528
		public float baseFertileCycles;
	}

	// Token: 0x02001182 RID: 4482
	public new class Instance : GameStateMachine<FertilityMonitor, FertilityMonitor.Instance, IStateMachineTarget, FertilityMonitor.Def>.GameInstance
	{
		// Token: 0x06005B6D RID: 23405 RVA: 0x002976BC File Offset: 0x002958BC
		public Instance(IStateMachineTarget master, FertilityMonitor.Def def) : base(master, def)
		{
			this.fertility = Db.Get().Amounts.Fertility.Lookup(base.gameObject);
			if (GenericGameSettings.instance.acceleratedLifecycle)
			{
				this.fertility.deltaAttribute.Add(new AttributeModifier(this.fertility.deltaAttribute.Id, 33.333332f, "Accelerated Lifecycle", false, false, true));
			}
			float value = 100f / (def.baseFertileCycles * 600f);
			this.fertileEffect = new Effect("Fertile", CREATURES.MODIFIERS.BASE_FERTILITY.NAME, CREATURES.MODIFIERS.BASE_FERTILITY.TOOLTIP, 0f, false, false, false, null, -1f, 0f, null, "");
			this.fertileEffect.Add(new AttributeModifier(Db.Get().Amounts.Fertility.deltaAttribute.Id, value, CREATURES.MODIFIERS.BASE_FERTILITY.NAME, false, false, true));
			this.InitializeBreedingChances();
		}

		// Token: 0x06005B6E RID: 23406 RVA: 0x002977BC File Offset: 0x002959BC
		[OnDeserialized]
		private void OnDeserialized()
		{
			int num = (base.def.initialBreedingWeights != null) ? base.def.initialBreedingWeights.Count : 0;
			if (this.breedingChances.Count != num)
			{
				this.InitializeBreedingChances();
			}
		}

		// Token: 0x06005B6F RID: 23407 RVA: 0x00297800 File Offset: 0x00295A00
		private void InitializeBreedingChances()
		{
			this.breedingChances = new List<FertilityMonitor.BreedingChance>();
			if (base.def.initialBreedingWeights != null)
			{
				foreach (FertilityMonitor.BreedingChance breedingChance in base.def.initialBreedingWeights)
				{
					this.breedingChances.Add(new FertilityMonitor.BreedingChance
					{
						egg = breedingChance.egg,
						weight = breedingChance.weight
					});
					foreach (FertilityModifier fertilityModifier in Db.Get().FertilityModifiers.GetForTag(breedingChance.egg))
					{
						fertilityModifier.ApplyFunction(this, breedingChance.egg);
					}
				}
				this.NormalizeBreedingChances();
			}
		}

		// Token: 0x06005B70 RID: 23408 RVA: 0x002978F8 File Offset: 0x00295AF8
		public void ShowEgg()
		{
			if (this.egg != null)
			{
				bool flag;
				Vector3 vector = base.GetComponent<KBatchedAnimController>().GetSymbolTransform(FertilityMonitor.Instance.targetEggSymbol, out flag).MultiplyPoint3x4(Vector3.zero);
				if (flag)
				{
					vector.z = Grid.GetLayerZ(Grid.SceneLayer.Ore);
					int num = Grid.PosToCell(vector);
					if (Grid.IsValidCell(num) && !Grid.Solid[num])
					{
						this.egg.transform.SetPosition(vector);
					}
				}
				this.egg.SetActive(true);
				Db.Get().Amounts.Wildness.Copy(this.egg, base.gameObject);
				this.egg = null;
			}
		}

		// Token: 0x06005B71 RID: 23409 RVA: 0x002979A8 File Offset: 0x00295BA8
		public void LayEgg()
		{
			this.fertility.value = 0f;
			Vector3 position = base.smi.transform.GetPosition();
			position.z = Grid.GetLayerZ(Grid.SceneLayer.Ore);
			Tag tag = FertilityMonitor.EggBreedingRoll(this.breedingChances, false);
			if (GenericGameSettings.instance.acceleratedLifecycle)
			{
				float num = 0f;
				foreach (FertilityMonitor.BreedingChance breedingChance in this.breedingChances)
				{
					if (breedingChance.weight > num)
					{
						num = breedingChance.weight;
						tag = breedingChance.egg;
					}
				}
			}
			global::Debug.Assert(tag != Tag.Invalid, "Didn't pick an egg to lay. Weights weren't normalized?");
			GameObject prefab = Assets.GetPrefab(tag);
			GameObject gameObject = Util.KInstantiate(prefab, position);
			this.egg = gameObject;
			SymbolOverrideController component = base.GetComponent<SymbolOverrideController>();
			string str = "egg01";
			CreatureBrain component2 = Assets.GetPrefab(prefab.GetDef<IncubationMonitor.Def>().spawnedCreature).GetComponent<CreatureBrain>();
			if (!string.IsNullOrEmpty(component2.symbolPrefix))
			{
				str = component2.symbolPrefix + "egg01";
			}
			KAnim.Build.Symbol symbol = this.egg.GetComponent<KBatchedAnimController>().AnimFiles[0].GetData().build.GetSymbol(str);
			if (symbol != null)
			{
				component.AddSymbolOverride(FertilityMonitor.Instance.targetEggSymbol, symbol, 0);
			}
			base.Trigger(1193600993, this.egg);
		}

		// Token: 0x06005B72 RID: 23410 RVA: 0x000DBA5B File Offset: 0x000D9C5B
		public bool IsReadyToLayEgg()
		{
			return base.smi.fertility.value >= base.smi.fertility.GetMax();
		}

		// Token: 0x06005B73 RID: 23411 RVA: 0x00297B20 File Offset: 0x00295D20
		public void AddBreedingChance(Tag type, float addedPercentChance)
		{
			foreach (FertilityMonitor.BreedingChance breedingChance in this.breedingChances)
			{
				if (breedingChance.egg == type)
				{
					float num = Mathf.Min(1f - breedingChance.weight, Mathf.Max(0f - breedingChance.weight, addedPercentChance));
					breedingChance.weight += num;
				}
			}
			this.NormalizeBreedingChances();
			base.master.Trigger(1059811075, this.breedingChances);
		}

		// Token: 0x06005B74 RID: 23412 RVA: 0x00297BC8 File Offset: 0x00295DC8
		public float GetBreedingChance(Tag type)
		{
			foreach (FertilityMonitor.BreedingChance breedingChance in this.breedingChances)
			{
				if (breedingChance.egg == type)
				{
					return breedingChance.weight;
				}
			}
			return -1f;
		}

		// Token: 0x06005B75 RID: 23413 RVA: 0x00297C34 File Offset: 0x00295E34
		public void NormalizeBreedingChances()
		{
			float num = 0f;
			foreach (FertilityMonitor.BreedingChance breedingChance in this.breedingChances)
			{
				num += breedingChance.weight;
			}
			foreach (FertilityMonitor.BreedingChance breedingChance2 in this.breedingChances)
			{
				breedingChance2.weight /= num;
			}
		}

		// Token: 0x06005B76 RID: 23414 RVA: 0x000DBA82 File Offset: 0x000D9C82
		protected override void OnCleanUp()
		{
			base.OnCleanUp();
			if (this.egg != null)
			{
				UnityEngine.Object.Destroy(this.egg);
				this.egg = null;
			}
		}

		// Token: 0x04004091 RID: 16529
		public AmountInstance fertility;

		// Token: 0x04004092 RID: 16530
		private GameObject egg;

		// Token: 0x04004093 RID: 16531
		[Serialize]
		public List<FertilityMonitor.BreedingChance> breedingChances;

		// Token: 0x04004094 RID: 16532
		public Effect fertileEffect;

		// Token: 0x04004095 RID: 16533
		private static HashedString targetEggSymbol = "snapto_egg";
	}
}
