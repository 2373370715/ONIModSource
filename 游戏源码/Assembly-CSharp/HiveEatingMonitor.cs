using System;

// Token: 0x020001A6 RID: 422
public class HiveEatingMonitor : GameStateMachine<HiveEatingMonitor, HiveEatingMonitor.Instance, IStateMachineTarget, HiveEatingMonitor.Def>
{
	// Token: 0x060005DC RID: 1500 RVA: 0x000A88F1 File Offset: 0x000A6AF1
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.ToggleBehaviour(GameTags.Creatures.WantsToEat, new StateMachine<HiveEatingMonitor, HiveEatingMonitor.Instance, IStateMachineTarget, HiveEatingMonitor.Def>.Transition.ConditionCallback(HiveEatingMonitor.ShouldEat), null);
	}

	// Token: 0x060005DD RID: 1501 RVA: 0x000A8919 File Offset: 0x000A6B19
	public static bool ShouldEat(HiveEatingMonitor.Instance smi)
	{
		return smi.storage.FindFirst(smi.def.consumedOre) != null;
	}

	// Token: 0x020001A7 RID: 423
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04000447 RID: 1095
		public Tag consumedOre;
	}

	// Token: 0x020001A8 RID: 424
	public new class Instance : GameStateMachine<HiveEatingMonitor, HiveEatingMonitor.Instance, IStateMachineTarget, HiveEatingMonitor.Def>.GameInstance
	{
		// Token: 0x060005E0 RID: 1504 RVA: 0x000A893F File Offset: 0x000A6B3F
		public Instance(IStateMachineTarget master, HiveEatingMonitor.Def def) : base(master, def)
		{
		}

		// Token: 0x04000448 RID: 1096
		[MyCmpReq]
		public Storage storage;
	}
}
