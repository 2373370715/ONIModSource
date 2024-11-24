using System;

// Token: 0x02001593 RID: 5523
public class IdleMonitor : GameStateMachine<IdleMonitor, IdleMonitor.Instance>
{
	// Token: 0x060072BC RID: 29372 RVA: 0x000EB22C File Offset: 0x000E942C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.idle;
		this.idle.TagTransition(GameTags.Dying, this.stopped, false).ToggleRecurringChore(new Func<IdleMonitor.Instance, Chore>(this.CreateIdleChore), null);
		this.stopped.DoNothing();
	}

	// Token: 0x060072BD RID: 29373 RVA: 0x000EB26C File Offset: 0x000E946C
	private Chore CreateIdleChore(IdleMonitor.Instance smi)
	{
		return new IdleChore(smi.master);
	}

	// Token: 0x040055CB RID: 21963
	public GameStateMachine<IdleMonitor, IdleMonitor.Instance, IStateMachineTarget, object>.State idle;

	// Token: 0x040055CC RID: 21964
	public GameStateMachine<IdleMonitor, IdleMonitor.Instance, IStateMachineTarget, object>.State stopped;

	// Token: 0x02001594 RID: 5524
	public new class Instance : GameStateMachine<IdleMonitor, IdleMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x060072BF RID: 29375 RVA: 0x000EB281 File Offset: 0x000E9481
		public Instance(IStateMachineTarget master) : base(master)
		{
		}
	}
}
