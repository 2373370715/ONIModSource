using System;

// Token: 0x020001AE RID: 430
public class HiveGrowthMonitor : GameStateMachine<HiveGrowthMonitor, HiveGrowthMonitor.Instance, IStateMachineTarget, HiveGrowthMonitor.Def>
{
	// Token: 0x060005EC RID: 1516 RVA: 0x000A89E6 File Offset: 0x000A6BE6
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.ToggleBehaviour(GameTags.Creatures.Behaviours.GrowUpBehaviour, new StateMachine<HiveGrowthMonitor, HiveGrowthMonitor.Instance, IStateMachineTarget, HiveGrowthMonitor.Def>.Transition.ConditionCallback(HiveGrowthMonitor.IsGrowing), null);
	}

	// Token: 0x060005ED RID: 1517 RVA: 0x000A8A0E File Offset: 0x000A6C0E
	public static bool IsGrowing(HiveGrowthMonitor.Instance smi)
	{
		return !smi.GetSMI<BeeHive.StatesInstance>().IsFullyGrown();
	}

	// Token: 0x020001AF RID: 431
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x020001B0 RID: 432
	public new class Instance : GameStateMachine<HiveGrowthMonitor, HiveGrowthMonitor.Instance, IStateMachineTarget, HiveGrowthMonitor.Def>.GameInstance
	{
		// Token: 0x060005F0 RID: 1520 RVA: 0x000A8A26 File Offset: 0x000A6C26
		public Instance(IStateMachineTarget master, HiveGrowthMonitor.Def def) : base(master, def)
		{
		}
	}
}
