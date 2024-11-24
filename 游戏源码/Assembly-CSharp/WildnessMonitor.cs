using System;
using Klei.AI;
using UnityEngine;

// Token: 0x020011F2 RID: 4594
public class WildnessMonitor : GameStateMachine<WildnessMonitor, WildnessMonitor.Instance, IStateMachineTarget, WildnessMonitor.Def>
{
	// Token: 0x06005D83 RID: 23939 RVA: 0x0029E6AC File Offset: 0x0029C8AC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.tame;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.wild.Enter(new StateMachine<WildnessMonitor, WildnessMonitor.Instance, IStateMachineTarget, WildnessMonitor.Def>.State.Callback(WildnessMonitor.RefreshAmounts)).Enter(new StateMachine<WildnessMonitor, WildnessMonitor.Instance, IStateMachineTarget, WildnessMonitor.Def>.State.Callback(WildnessMonitor.HideDomesticationSymbol)).Transition(this.tame, (WildnessMonitor.Instance smi) => !WildnessMonitor.IsWild(smi), UpdateRate.SIM_1000ms).ToggleEffect((WildnessMonitor.Instance smi) => smi.def.wildEffect).ToggleTag(GameTags.Creatures.Wild);
		this.tame.Enter(new StateMachine<WildnessMonitor, WildnessMonitor.Instance, IStateMachineTarget, WildnessMonitor.Def>.State.Callback(WildnessMonitor.RefreshAmounts)).Enter(new StateMachine<WildnessMonitor, WildnessMonitor.Instance, IStateMachineTarget, WildnessMonitor.Def>.State.Callback(WildnessMonitor.ShowDomesticationSymbol)).Transition(this.wild, new StateMachine<WildnessMonitor, WildnessMonitor.Instance, IStateMachineTarget, WildnessMonitor.Def>.Transition.ConditionCallback(WildnessMonitor.IsWild), UpdateRate.SIM_1000ms).ToggleEffect((WildnessMonitor.Instance smi) => smi.def.tameEffect).Enter(delegate(WildnessMonitor.Instance smi)
		{
			SaveGame.Instance.ColonyAchievementTracker.LogCritterTamed(smi.PrefabID());
		});
	}

	// Token: 0x06005D84 RID: 23940 RVA: 0x0029E7D4 File Offset: 0x0029C9D4
	private static void HideDomesticationSymbol(WildnessMonitor.Instance smi)
	{
		foreach (KAnimHashedString symbol in WildnessMonitor.DOMESTICATION_SYMBOLS)
		{
			smi.GetComponent<KBatchedAnimController>().SetSymbolVisiblity(symbol, false);
		}
	}

	// Token: 0x06005D85 RID: 23941 RVA: 0x0029E80C File Offset: 0x0029CA0C
	private static void ShowDomesticationSymbol(WildnessMonitor.Instance smi)
	{
		foreach (KAnimHashedString symbol in WildnessMonitor.DOMESTICATION_SYMBOLS)
		{
			smi.GetComponent<KBatchedAnimController>().SetSymbolVisiblity(symbol, true);
		}
	}

	// Token: 0x06005D86 RID: 23942 RVA: 0x000DD1D0 File Offset: 0x000DB3D0
	private static bool IsWild(WildnessMonitor.Instance smi)
	{
		return smi.wildness.value > 0f;
	}

	// Token: 0x06005D87 RID: 23943 RVA: 0x0029E844 File Offset: 0x0029CA44
	private static void RefreshAmounts(WildnessMonitor.Instance smi)
	{
		bool flag = WildnessMonitor.IsWild(smi);
		smi.wildness.hide = !flag;
		AttributeInstance attributeInstance = Db.Get().CritterAttributes.Happiness.Lookup(smi.gameObject);
		if (attributeInstance != null)
		{
			attributeInstance.hide = flag;
		}
		AttributeInstance attributeInstance2 = Db.Get().CritterAttributes.Metabolism.Lookup(smi.gameObject);
		if (attributeInstance2 != null)
		{
			attributeInstance2.hide = flag;
		}
		AmountInstance amountInstance = Db.Get().Amounts.Calories.Lookup(smi.gameObject);
		if (amountInstance != null)
		{
			amountInstance.hide = flag;
		}
		AmountInstance amountInstance2 = Db.Get().Amounts.Temperature.Lookup(smi.gameObject);
		if (amountInstance2 != null)
		{
			amountInstance2.hide = flag;
		}
		AmountInstance amountInstance3 = Db.Get().Amounts.Fertility.Lookup(smi.gameObject);
		if (amountInstance3 != null)
		{
			amountInstance3.hide = flag;
		}
		AmountInstance amountInstance4 = Db.Get().Amounts.MilkProduction.Lookup(smi.gameObject);
		if (amountInstance4 != null)
		{
			amountInstance4.hide = flag;
		}
		AmountInstance amountInstance5 = Db.Get().Amounts.Beckoning.Lookup(smi.gameObject);
		if (amountInstance5 != null)
		{
			amountInstance5.hide = flag;
		}
	}

	// Token: 0x0400423E RID: 16958
	public GameStateMachine<WildnessMonitor, WildnessMonitor.Instance, IStateMachineTarget, WildnessMonitor.Def>.State wild;

	// Token: 0x0400423F RID: 16959
	public GameStateMachine<WildnessMonitor, WildnessMonitor.Instance, IStateMachineTarget, WildnessMonitor.Def>.State tame;

	// Token: 0x04004240 RID: 16960
	private static readonly KAnimHashedString[] DOMESTICATION_SYMBOLS = new KAnimHashedString[]
	{
		"tag",
		"snapto_tag"
	};

	// Token: 0x020011F3 RID: 4595
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x06005D8A RID: 23946 RVA: 0x000DD21B File Offset: 0x000DB41B
		public override void Configure(GameObject prefab)
		{
			prefab.GetComponent<Modifiers>().initialAmounts.Add(Db.Get().Amounts.Wildness.Id);
		}

		// Token: 0x04004241 RID: 16961
		public Effect wildEffect;

		// Token: 0x04004242 RID: 16962
		public Effect tameEffect;
	}

	// Token: 0x020011F4 RID: 4596
	public new class Instance : GameStateMachine<WildnessMonitor, WildnessMonitor.Instance, IStateMachineTarget, WildnessMonitor.Def>.GameInstance
	{
		// Token: 0x06005D8C RID: 23948 RVA: 0x000DD241 File Offset: 0x000DB441
		public Instance(IStateMachineTarget master, WildnessMonitor.Def def) : base(master, def)
		{
			this.wildness = Db.Get().Amounts.Wildness.Lookup(base.gameObject);
			this.wildness.value = this.wildness.GetMax();
		}

		// Token: 0x04004243 RID: 16963
		public AmountInstance wildness;
	}
}
