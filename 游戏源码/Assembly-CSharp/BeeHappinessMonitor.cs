using System;
using Klei.AI;
using STRINGS;

// Token: 0x0200112E RID: 4398
public class BeeHappinessMonitor : GameStateMachine<BeeHappinessMonitor, BeeHappinessMonitor.Instance, IStateMachineTarget, BeeHappinessMonitor.Def>
{
	// Token: 0x06005A07 RID: 23047 RVA: 0x002939B4 File Offset: 0x00291BB4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		this.satisfied.TriggerOnEnter(GameHashes.Satisfied, null).Transition(this.happy, new StateMachine<BeeHappinessMonitor, BeeHappinessMonitor.Instance, IStateMachineTarget, BeeHappinessMonitor.Def>.Transition.ConditionCallback(BeeHappinessMonitor.IsHappy), UpdateRate.SIM_1000ms).Transition(this.unhappy, new StateMachine<BeeHappinessMonitor, BeeHappinessMonitor.Instance, IStateMachineTarget, BeeHappinessMonitor.Def>.Transition.ConditionCallback(BeeHappinessMonitor.IsUnhappy), UpdateRate.SIM_1000ms).ToggleEffect((BeeHappinessMonitor.Instance smi) => this.neutralEffect);
		this.happy.TriggerOnEnter(GameHashes.Happy, null).ToggleEffect((BeeHappinessMonitor.Instance smi) => this.happyEffect).Transition(this.satisfied, GameStateMachine<BeeHappinessMonitor, BeeHappinessMonitor.Instance, IStateMachineTarget, BeeHappinessMonitor.Def>.Not(new StateMachine<BeeHappinessMonitor, BeeHappinessMonitor.Instance, IStateMachineTarget, BeeHappinessMonitor.Def>.Transition.ConditionCallback(BeeHappinessMonitor.IsHappy)), UpdateRate.SIM_1000ms);
		this.unhappy.TriggerOnEnter(GameHashes.Unhappy, null).Transition(this.satisfied, GameStateMachine<BeeHappinessMonitor, BeeHappinessMonitor.Instance, IStateMachineTarget, BeeHappinessMonitor.Def>.Not(new StateMachine<BeeHappinessMonitor, BeeHappinessMonitor.Instance, IStateMachineTarget, BeeHappinessMonitor.Def>.Transition.ConditionCallback(BeeHappinessMonitor.IsUnhappy)), UpdateRate.SIM_1000ms).ToggleEffect((BeeHappinessMonitor.Instance smi) => this.unhappyEffect);
		this.happyEffect = new Effect("Happy", CREATURES.MODIFIERS.HAPPY_WILD.NAME, CREATURES.MODIFIERS.HAPPY_WILD.TOOLTIP, 0f, true, false, false, null, -1f, 0f, null, "");
		this.neutralEffect = new Effect("Neutral", CREATURES.MODIFIERS.NEUTRAL.NAME, CREATURES.MODIFIERS.NEUTRAL.TOOLTIP, 0f, true, false, false, null, -1f, 0f, null, "");
		this.unhappyEffect = new Effect("Unhappy", CREATURES.MODIFIERS.GLUM.NAME, CREATURES.MODIFIERS.GLUM.TOOLTIP, 0f, true, false, true, null, -1f, 0f, null, "");
	}

	// Token: 0x06005A08 RID: 23048 RVA: 0x000DAA35 File Offset: 0x000D8C35
	private static bool IsHappy(BeeHappinessMonitor.Instance smi)
	{
		return smi.happiness.GetTotalValue() >= smi.def.happyThreshold;
	}

	// Token: 0x06005A09 RID: 23049 RVA: 0x000DAA52 File Offset: 0x000D8C52
	private static bool IsUnhappy(BeeHappinessMonitor.Instance smi)
	{
		return smi.happiness.GetTotalValue() <= smi.def.unhappyThreshold;
	}

	// Token: 0x04003F86 RID: 16262
	private GameStateMachine<BeeHappinessMonitor, BeeHappinessMonitor.Instance, IStateMachineTarget, BeeHappinessMonitor.Def>.State satisfied;

	// Token: 0x04003F87 RID: 16263
	private GameStateMachine<BeeHappinessMonitor, BeeHappinessMonitor.Instance, IStateMachineTarget, BeeHappinessMonitor.Def>.State happy;

	// Token: 0x04003F88 RID: 16264
	private GameStateMachine<BeeHappinessMonitor, BeeHappinessMonitor.Instance, IStateMachineTarget, BeeHappinessMonitor.Def>.State unhappy;

	// Token: 0x04003F89 RID: 16265
	private Effect happyEffect;

	// Token: 0x04003F8A RID: 16266
	private Effect neutralEffect;

	// Token: 0x04003F8B RID: 16267
	private Effect unhappyEffect;

	// Token: 0x0200112F RID: 4399
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04003F8C RID: 16268
		public float happyThreshold = 4f;

		// Token: 0x04003F8D RID: 16269
		public float unhappyThreshold = -1f;
	}

	// Token: 0x02001130 RID: 4400
	public new class Instance : GameStateMachine<BeeHappinessMonitor, BeeHappinessMonitor.Instance, IStateMachineTarget, BeeHappinessMonitor.Def>.GameInstance
	{
		// Token: 0x06005A0F RID: 23055 RVA: 0x000DAAAD File Offset: 0x000D8CAD
		public Instance(IStateMachineTarget master, BeeHappinessMonitor.Def def) : base(master, def)
		{
			this.happiness = base.gameObject.GetAttributes().Add(Db.Get().CritterAttributes.Happiness);
		}

		// Token: 0x04003F8E RID: 16270
		public AttributeInstance happiness;
	}
}
