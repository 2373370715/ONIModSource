using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x020011EE RID: 4590
public class WellFedShearable : GameStateMachine<WellFedShearable, WellFedShearable.Instance, IStateMachineTarget, WellFedShearable.Def>
{
	// Token: 0x06005D6F RID: 23919 RVA: 0x0029E130 File Offset: 0x0029C330
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.growing;
		this.root.Enter(delegate(WellFedShearable.Instance smi)
		{
			WellFedShearable.UpdateScales(smi, 0f);
		}).Enter(delegate(WellFedShearable.Instance smi)
		{
			if (smi.def.hideSymbols != null)
			{
				foreach (KAnimHashedString symbol in smi.def.hideSymbols)
				{
					smi.animController.SetSymbolVisiblity(symbol, false);
				}
			}
		}).Update(new Action<WellFedShearable.Instance, float>(WellFedShearable.UpdateScales), UpdateRate.SIM_1000ms, false).EventHandler(GameHashes.CaloriesConsumed, delegate(WellFedShearable.Instance smi, object data)
		{
			smi.OnCaloriesConsumed(data);
		});
		this.growing.Enter(delegate(WellFedShearable.Instance smi)
		{
			WellFedShearable.UpdateScales(smi, 0f);
		}).Transition(this.fullyGrown, new StateMachine<WellFedShearable, WellFedShearable.Instance, IStateMachineTarget, WellFedShearable.Def>.Transition.ConditionCallback(WellFedShearable.AreScalesFullyGrown), UpdateRate.SIM_1000ms);
		this.fullyGrown.Enter(delegate(WellFedShearable.Instance smi)
		{
			WellFedShearable.UpdateScales(smi, 0f);
		}).ToggleBehaviour(GameTags.Creatures.ScalesGrown, (WellFedShearable.Instance smi) => smi.HasTag(GameTags.Creatures.CanMolt), null).EventTransition(GameHashes.Molt, this.growing, GameStateMachine<WellFedShearable, WellFedShearable.Instance, IStateMachineTarget, WellFedShearable.Def>.Not(new StateMachine<WellFedShearable, WellFedShearable.Instance, IStateMachineTarget, WellFedShearable.Def>.Transition.ConditionCallback(WellFedShearable.AreScalesFullyGrown))).Transition(this.growing, GameStateMachine<WellFedShearable, WellFedShearable.Instance, IStateMachineTarget, WellFedShearable.Def>.Not(new StateMachine<WellFedShearable, WellFedShearable.Instance, IStateMachineTarget, WellFedShearable.Def>.Transition.ConditionCallback(WellFedShearable.AreScalesFullyGrown)), UpdateRate.SIM_1000ms);
	}

	// Token: 0x06005D70 RID: 23920 RVA: 0x000DD155 File Offset: 0x000DB355
	private static bool AreScalesFullyGrown(WellFedShearable.Instance smi)
	{
		return smi.scaleGrowth.value >= smi.scaleGrowth.GetMax();
	}

	// Token: 0x06005D71 RID: 23921 RVA: 0x0029E2A8 File Offset: 0x0029C4A8
	private static void UpdateScales(WellFedShearable.Instance smi, float dt)
	{
		int num = (int)((float)smi.def.levelCount * smi.scaleGrowth.value / 100f);
		if (smi.currentScaleLevel != num)
		{
			for (int i = 0; i < smi.def.scaleGrowthSymbols.Length; i++)
			{
				bool is_visible = i <= num - 1;
				smi.animController.SetSymbolVisiblity(smi.def.scaleGrowthSymbols[i], is_visible);
			}
			smi.currentScaleLevel = num;
		}
	}

	// Token: 0x04004227 RID: 16935
	public GameStateMachine<WellFedShearable, WellFedShearable.Instance, IStateMachineTarget, WellFedShearable.Def>.State growing;

	// Token: 0x04004228 RID: 16936
	public GameStateMachine<WellFedShearable, WellFedShearable.Instance, IStateMachineTarget, WellFedShearable.Def>.State fullyGrown;

	// Token: 0x020011EF RID: 4591
	public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
	{
		// Token: 0x06005D73 RID: 23923 RVA: 0x000DCB73 File Offset: 0x000DAD73
		public override void Configure(GameObject prefab)
		{
			prefab.GetComponent<Modifiers>().initialAmounts.Add(Db.Get().Amounts.ScaleGrowth.Id);
		}

		// Token: 0x06005D74 RID: 23924 RVA: 0x0029E324 File Offset: 0x0029C524
		public List<Descriptor> GetDescriptors(GameObject obj)
		{
			return new List<Descriptor>
			{
				new Descriptor(UI.BUILDINGEFFECTS.SCALE_GROWTH.Replace("{Item}", this.itemDroppedOnShear.ProperName()).Replace("{Amount}", GameUtil.GetFormattedMass(this.dropMass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")).Replace("{Time}", GameUtil.GetFormattedCycles(this.growthDurationCycles * 600f, "F1", false)), UI.BUILDINGEFFECTS.TOOLTIPS.SCALE_GROWTH_FED.Replace("{Item}", this.itemDroppedOnShear.ProperName()).Replace("{Amount}", GameUtil.GetFormattedMass(this.dropMass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")).Replace("{Time}", GameUtil.GetFormattedCycles(this.growthDurationCycles * 600f, "F1", false)), Descriptor.DescriptorType.Effect, false)
			};
		}

		// Token: 0x04004229 RID: 16937
		public string effectId;

		// Token: 0x0400422A RID: 16938
		public float caloriesPerCycle;

		// Token: 0x0400422B RID: 16939
		public float growthDurationCycles;

		// Token: 0x0400422C RID: 16940
		public int levelCount;

		// Token: 0x0400422D RID: 16941
		public Tag itemDroppedOnShear;

		// Token: 0x0400422E RID: 16942
		public float dropMass;

		// Token: 0x0400422F RID: 16943
		public Tag requiredDiet = null;

		// Token: 0x04004230 RID: 16944
		public KAnimHashedString[] scaleGrowthSymbols = WellFedShearable.Def.SCALE_SYMBOL_NAMES;

		// Token: 0x04004231 RID: 16945
		public KAnimHashedString[] hideSymbols;

		// Token: 0x04004232 RID: 16946
		public static KAnimHashedString[] SCALE_SYMBOL_NAMES = new KAnimHashedString[]
		{
			"scale_0",
			"scale_1",
			"scale_2",
			"scale_3",
			"scale_4"
		};
	}

	// Token: 0x020011F0 RID: 4592
	public new class Instance : GameStateMachine<WellFedShearable, WellFedShearable.Instance, IStateMachineTarget, WellFedShearable.Def>.GameInstance, IShearable
	{
		// Token: 0x06005D77 RID: 23927 RVA: 0x0029E464 File Offset: 0x0029C664
		public Instance(IStateMachineTarget master, WellFedShearable.Def def) : base(master, def)
		{
			this.scaleGrowth = Db.Get().Amounts.ScaleGrowth.Lookup(base.gameObject);
			this.scaleGrowth.value = this.scaleGrowth.GetMax();
		}

		// Token: 0x06005D78 RID: 23928 RVA: 0x000DD199 File Offset: 0x000DB399
		public bool IsFullyGrown()
		{
			return this.currentScaleLevel == base.def.levelCount;
		}

		// Token: 0x06005D79 RID: 23929 RVA: 0x0029E4B8 File Offset: 0x0029C6B8
		public void OnCaloriesConsumed(object data)
		{
			CreatureCalorieMonitor.CaloriesConsumedEvent caloriesConsumedEvent = (CreatureCalorieMonitor.CaloriesConsumedEvent)data;
			if (base.def.requiredDiet != null && caloriesConsumedEvent.tag != base.def.requiredDiet)
			{
				return;
			}
			EffectInstance effectInstance = this.effects.Get(base.smi.def.effectId);
			if (effectInstance == null)
			{
				effectInstance = this.effects.Add(base.smi.def.effectId, true);
			}
			effectInstance.timeRemaining += caloriesConsumedEvent.calories / base.smi.def.caloriesPerCycle * 600f;
		}

		// Token: 0x06005D7A RID: 23930 RVA: 0x0029E564 File Offset: 0x0029C764
		public void Shear()
		{
			PrimaryElement component = base.smi.GetComponent<PrimaryElement>();
			GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(base.def.itemDroppedOnShear), null, null);
			gameObject.transform.SetPosition(Grid.CellToPosCCC(Grid.CellLeft(Grid.PosToCell(this)), Grid.SceneLayer.Ore));
			PrimaryElement component2 = gameObject.GetComponent<PrimaryElement>();
			component2.Temperature = component.Temperature;
			component2.Mass = base.def.dropMass;
			component2.AddDisease(component.DiseaseIdx, component.DiseaseCount, "Shearing");
			gameObject.SetActive(true);
			Vector2 initial_velocity = new Vector2(UnityEngine.Random.Range(-1f, 1f) * 1f, UnityEngine.Random.value * 2f + 2f);
			if (GameComps.Fallers.Has(gameObject))
			{
				GameComps.Fallers.Remove(gameObject);
			}
			GameComps.Fallers.Add(gameObject, initial_velocity);
			this.scaleGrowth.value = 0f;
			WellFedShearable.UpdateScales(this, 0f);
		}

		// Token: 0x04004233 RID: 16947
		[MyCmpGet]
		private Effects effects;

		// Token: 0x04004234 RID: 16948
		[MyCmpGet]
		public KBatchedAnimController animController;

		// Token: 0x04004235 RID: 16949
		public AmountInstance scaleGrowth;

		// Token: 0x04004236 RID: 16950
		public int currentScaleLevel = -1;
	}
}
