using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02001176 RID: 4470
public class ElementGrowthMonitor : GameStateMachine<ElementGrowthMonitor, ElementGrowthMonitor.Instance, IStateMachineTarget, ElementGrowthMonitor.Def>
{
	// Token: 0x06005B32 RID: 23346 RVA: 0x00296ABC File Offset: 0x00294CBC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.growing;
		this.root.Enter(delegate(ElementGrowthMonitor.Instance smi)
		{
			ElementGrowthMonitor.UpdateGrowth(smi, 0f);
		}).Update(new Action<ElementGrowthMonitor.Instance, float>(ElementGrowthMonitor.UpdateGrowth), UpdateRate.SIM_1000ms, false).EventHandler(GameHashes.EatSolidComplete, delegate(ElementGrowthMonitor.Instance smi, object data)
		{
			smi.OnEatSolidComplete(data);
		});
		this.growing.DefaultState(this.growing.growing).Transition(this.fullyGrown, new StateMachine<ElementGrowthMonitor, ElementGrowthMonitor.Instance, IStateMachineTarget, ElementGrowthMonitor.Def>.Transition.ConditionCallback(ElementGrowthMonitor.IsFullyGrown), UpdateRate.SIM_1000ms).TagTransition(this.HungryTags, this.halted, false);
		this.growing.growing.Transition(this.growing.stunted, GameStateMachine<ElementGrowthMonitor, ElementGrowthMonitor.Instance, IStateMachineTarget, ElementGrowthMonitor.Def>.Not(new StateMachine<ElementGrowthMonitor, ElementGrowthMonitor.Instance, IStateMachineTarget, ElementGrowthMonitor.Def>.Transition.ConditionCallback(ElementGrowthMonitor.IsConsumedInTemperatureRange)), UpdateRate.SIM_1000ms).ToggleStatusItem(Db.Get().CreatureStatusItems.ElementGrowthGrowing, null).Enter(new StateMachine<ElementGrowthMonitor, ElementGrowthMonitor.Instance, IStateMachineTarget, ElementGrowthMonitor.Def>.State.Callback(ElementGrowthMonitor.ApplyModifier)).Exit(new StateMachine<ElementGrowthMonitor, ElementGrowthMonitor.Instance, IStateMachineTarget, ElementGrowthMonitor.Def>.State.Callback(ElementGrowthMonitor.RemoveModifier));
		this.growing.stunted.Transition(this.growing.growing, new StateMachine<ElementGrowthMonitor, ElementGrowthMonitor.Instance, IStateMachineTarget, ElementGrowthMonitor.Def>.Transition.ConditionCallback(ElementGrowthMonitor.IsConsumedInTemperatureRange), UpdateRate.SIM_1000ms).ToggleStatusItem(Db.Get().CreatureStatusItems.ElementGrowthStunted, null).Enter(new StateMachine<ElementGrowthMonitor, ElementGrowthMonitor.Instance, IStateMachineTarget, ElementGrowthMonitor.Def>.State.Callback(ElementGrowthMonitor.ApplyModifier)).Exit(new StateMachine<ElementGrowthMonitor, ElementGrowthMonitor.Instance, IStateMachineTarget, ElementGrowthMonitor.Def>.State.Callback(ElementGrowthMonitor.RemoveModifier));
		this.halted.TagTransition(this.HungryTags, this.growing, true).ToggleStatusItem(Db.Get().CreatureStatusItems.ElementGrowthHalted, null);
		this.fullyGrown.ToggleStatusItem(Db.Get().CreatureStatusItems.ElementGrowthComplete, null).ToggleBehaviour(GameTags.Creatures.ScalesGrown, (ElementGrowthMonitor.Instance smi) => smi.HasTag(GameTags.Creatures.CanMolt), null).Transition(this.growing, GameStateMachine<ElementGrowthMonitor, ElementGrowthMonitor.Instance, IStateMachineTarget, ElementGrowthMonitor.Def>.Not(new StateMachine<ElementGrowthMonitor, ElementGrowthMonitor.Instance, IStateMachineTarget, ElementGrowthMonitor.Def>.Transition.ConditionCallback(ElementGrowthMonitor.IsFullyGrown)), UpdateRate.SIM_1000ms);
	}

	// Token: 0x06005B33 RID: 23347 RVA: 0x000DB732 File Offset: 0x000D9932
	private static bool IsConsumedInTemperatureRange(ElementGrowthMonitor.Instance smi)
	{
		return smi.lastConsumedTemperature == 0f || (smi.lastConsumedTemperature >= smi.def.minTemperature && smi.lastConsumedTemperature <= smi.def.maxTemperature);
	}

	// Token: 0x06005B34 RID: 23348 RVA: 0x000DB76E File Offset: 0x000D996E
	private static bool IsFullyGrown(ElementGrowthMonitor.Instance smi)
	{
		return smi.elementGrowth.value >= smi.elementGrowth.GetMax();
	}

	// Token: 0x06005B35 RID: 23349 RVA: 0x00296CD0 File Offset: 0x00294ED0
	private static void ApplyModifier(ElementGrowthMonitor.Instance smi)
	{
		if (smi.IsInsideState(smi.sm.growing.growing))
		{
			smi.elementGrowth.deltaAttribute.Add(smi.growingGrowthModifier);
			return;
		}
		if (smi.IsInsideState(smi.sm.growing.stunted))
		{
			smi.elementGrowth.deltaAttribute.Add(smi.stuntedGrowthModifier);
		}
	}

	// Token: 0x06005B36 RID: 23350 RVA: 0x000DB78B File Offset: 0x000D998B
	private static void RemoveModifier(ElementGrowthMonitor.Instance smi)
	{
		smi.elementGrowth.deltaAttribute.Remove(smi.growingGrowthModifier);
		smi.elementGrowth.deltaAttribute.Remove(smi.stuntedGrowthModifier);
	}

	// Token: 0x06005B37 RID: 23351 RVA: 0x00296D3C File Offset: 0x00294F3C
	private static void UpdateGrowth(ElementGrowthMonitor.Instance smi, float dt)
	{
		int num = (int)((float)smi.def.levelCount * smi.elementGrowth.value / 100f);
		if (smi.currentGrowthLevel != num)
		{
			KBatchedAnimController component = smi.GetComponent<KBatchedAnimController>();
			for (int i = 0; i < ElementGrowthMonitor.GROWTH_SYMBOL_NAMES.Length; i++)
			{
				bool is_visible = i == num - 1;
				component.SetSymbolVisiblity(ElementGrowthMonitor.GROWTH_SYMBOL_NAMES[i], is_visible);
			}
			smi.currentGrowthLevel = num;
		}
	}

	// Token: 0x0400405F RID: 16479
	public Tag[] HungryTags = new Tag[]
	{
		GameTags.Creatures.Hungry
	};

	// Token: 0x04004060 RID: 16480
	public GameStateMachine<ElementGrowthMonitor, ElementGrowthMonitor.Instance, IStateMachineTarget, ElementGrowthMonitor.Def>.State halted;

	// Token: 0x04004061 RID: 16481
	public ElementGrowthMonitor.GrowingState growing;

	// Token: 0x04004062 RID: 16482
	public GameStateMachine<ElementGrowthMonitor, ElementGrowthMonitor.Instance, IStateMachineTarget, ElementGrowthMonitor.Def>.State fullyGrown;

	// Token: 0x04004063 RID: 16483
	private static HashedString[] GROWTH_SYMBOL_NAMES = new HashedString[]
	{
		"del_ginger1",
		"del_ginger2",
		"del_ginger3",
		"del_ginger4",
		"del_ginger5"
	};

	// Token: 0x02001177 RID: 4471
	public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
	{
		// Token: 0x06005B3A RID: 23354 RVA: 0x000DB7D9 File Offset: 0x000D99D9
		public override void Configure(GameObject prefab)
		{
			prefab.GetComponent<Modifiers>().initialAmounts.Add(Db.Get().Amounts.ElementGrowth.Id);
		}

		// Token: 0x06005B3B RID: 23355 RVA: 0x00296E20 File Offset: 0x00295020
		public List<Descriptor> GetDescriptors(GameObject obj)
		{
			return new List<Descriptor>
			{
				new Descriptor(UI.BUILDINGEFFECTS.SCALE_GROWTH_TEMP.Replace("{Item}", this.itemDroppedOnShear.ProperName()).Replace("{Amount}", GameUtil.GetFormattedMass(this.dropMass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")).Replace("{Time}", GameUtil.GetFormattedCycles(1f / this.defaultGrowthRate, "F1", false)).Replace("{TempMin}", GameUtil.GetFormattedTemperature(this.minTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)).Replace("{TempMax}", GameUtil.GetFormattedTemperature(this.maxTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), UI.BUILDINGEFFECTS.TOOLTIPS.SCALE_GROWTH_TEMP.Replace("{Item}", this.itemDroppedOnShear.ProperName()).Replace("{Amount}", GameUtil.GetFormattedMass(this.dropMass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")).Replace("{Time}", GameUtil.GetFormattedCycles(1f / this.defaultGrowthRate, "F1", false)).Replace("{TempMin}", GameUtil.GetFormattedTemperature(this.minTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)).Replace("{TempMax}", GameUtil.GetFormattedTemperature(this.maxTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), Descriptor.DescriptorType.Effect, false)
			};
		}

		// Token: 0x04004064 RID: 16484
		public int levelCount;

		// Token: 0x04004065 RID: 16485
		public float defaultGrowthRate;

		// Token: 0x04004066 RID: 16486
		public Tag itemDroppedOnShear;

		// Token: 0x04004067 RID: 16487
		public float dropMass;

		// Token: 0x04004068 RID: 16488
		public float minTemperature;

		// Token: 0x04004069 RID: 16489
		public float maxTemperature;
	}

	// Token: 0x02001178 RID: 4472
	public class GrowingState : GameStateMachine<ElementGrowthMonitor, ElementGrowthMonitor.Instance, IStateMachineTarget, ElementGrowthMonitor.Def>.State
	{
		// Token: 0x0400406A RID: 16490
		public GameStateMachine<ElementGrowthMonitor, ElementGrowthMonitor.Instance, IStateMachineTarget, ElementGrowthMonitor.Def>.State growing;

		// Token: 0x0400406B RID: 16491
		public GameStateMachine<ElementGrowthMonitor, ElementGrowthMonitor.Instance, IStateMachineTarget, ElementGrowthMonitor.Def>.State stunted;
	}

	// Token: 0x02001179 RID: 4473
	public new class Instance : GameStateMachine<ElementGrowthMonitor, ElementGrowthMonitor.Instance, IStateMachineTarget, ElementGrowthMonitor.Def>.GameInstance, IShearable
	{
		// Token: 0x06005B3E RID: 23358 RVA: 0x00296F54 File Offset: 0x00295154
		public Instance(IStateMachineTarget master, ElementGrowthMonitor.Def def) : base(master, def)
		{
			this.elementGrowth = Db.Get().Amounts.ElementGrowth.Lookup(base.gameObject);
			this.elementGrowth.value = this.elementGrowth.GetMax();
			this.growingGrowthModifier = new AttributeModifier(this.elementGrowth.amount.deltaAttribute.Id, def.defaultGrowthRate * 100f, CREATURES.MODIFIERS.ELEMENT_GROWTH_RATE.NAME, false, false, true);
			this.stuntedGrowthModifier = new AttributeModifier(this.elementGrowth.amount.deltaAttribute.Id, def.defaultGrowthRate * 20f, CREATURES.MODIFIERS.ELEMENT_GROWTH_RATE.NAME, false, false, true);
		}

		// Token: 0x06005B3F RID: 23359 RVA: 0x00297018 File Offset: 0x00295218
		public void OnEatSolidComplete(object data)
		{
			KPrefabID kprefabID = (KPrefabID)data;
			if (kprefabID == null)
			{
				return;
			}
			PrimaryElement component = kprefabID.GetComponent<PrimaryElement>();
			this.lastConsumedElement = component.ElementID;
			this.lastConsumedTemperature = component.Temperature;
		}

		// Token: 0x06005B40 RID: 23360 RVA: 0x000DB807 File Offset: 0x000D9A07
		public bool IsFullyGrown()
		{
			return this.currentGrowthLevel == base.def.levelCount;
		}

		// Token: 0x06005B41 RID: 23361 RVA: 0x00297058 File Offset: 0x00295258
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
			this.elementGrowth.value = 0f;
			ElementGrowthMonitor.UpdateGrowth(this, 0f);
		}

		// Token: 0x0400406C RID: 16492
		public AmountInstance elementGrowth;

		// Token: 0x0400406D RID: 16493
		public AttributeModifier growingGrowthModifier;

		// Token: 0x0400406E RID: 16494
		public AttributeModifier stuntedGrowthModifier;

		// Token: 0x0400406F RID: 16495
		public int currentGrowthLevel = -1;

		// Token: 0x04004070 RID: 16496
		[Serialize]
		public SimHashes lastConsumedElement;

		// Token: 0x04004071 RID: 16497
		[Serialize]
		public float lastConsumedTemperature;
	}
}
