using System;

// Token: 0x020015A9 RID: 5545
public class MingleMonitor : GameStateMachine<MingleMonitor, MingleMonitor.Instance>
{
	// Token: 0x06007315 RID: 29461 RVA: 0x000EB729 File Offset: 0x000E9929
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.mingle;
		base.serializable = StateMachine.SerializeType.Never;
		this.mingle.ToggleRecurringChore(new Func<MingleMonitor.Instance, Chore>(this.CreateMingleChore), null);
	}

	// Token: 0x06007316 RID: 29462 RVA: 0x000EB753 File Offset: 0x000E9953
	private Chore CreateMingleChore(MingleMonitor.Instance smi)
	{
		return new MingleChore(smi.master);
	}

	// Token: 0x0400561C RID: 22044
	public GameStateMachine<MingleMonitor, MingleMonitor.Instance, IStateMachineTarget, object>.State mingle;

	// Token: 0x020015AA RID: 5546
	public new class Instance : GameStateMachine<MingleMonitor, MingleMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06007318 RID: 29464 RVA: 0x000EB768 File Offset: 0x000E9968
		public Instance(IStateMachineTarget master) : base(master)
		{
		}
	}
}
