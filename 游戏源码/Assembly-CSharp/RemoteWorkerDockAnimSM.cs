using System;

// Token: 0x02001769 RID: 5993
internal class RemoteWorkerDockAnimSM : StateMachineComponent<RemoteWorkerDockAnimSM.StatesInstance>
{
	// Token: 0x06007B61 RID: 31585 RVA: 0x000F1213 File Offset: 0x000EF413
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x04005C84 RID: 23684
	[MyCmpAdd]
	private RemoteWorkerDock dock;

	// Token: 0x04005C85 RID: 23685
	[MyCmpGet]
	private Operational operational;

	// Token: 0x0200176A RID: 5994
	public class StatesInstance : GameStateMachine<RemoteWorkerDockAnimSM.States, RemoteWorkerDockAnimSM.StatesInstance, RemoteWorkerDockAnimSM, object>.GameInstance
	{
		// Token: 0x06007B63 RID: 31587 RVA: 0x000F122E File Offset: 0x000EF42E
		public StatesInstance(RemoteWorkerDockAnimSM master) : base(master)
		{
		}
	}

	// Token: 0x0200176B RID: 5995
	public class States : GameStateMachine<RemoteWorkerDockAnimSM.States, RemoteWorkerDockAnimSM.StatesInstance, RemoteWorkerDockAnimSM>
	{
		// Token: 0x06007B64 RID: 31588 RVA: 0x0031B380 File Offset: 0x00319580
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.off;
			this.off.EnterTransition(this.off.full, new StateMachine<RemoteWorkerDockAnimSM.States, RemoteWorkerDockAnimSM.StatesInstance, RemoteWorkerDockAnimSM, object>.Transition.ConditionCallback(RemoteWorkerDockAnimSM.States.HasWorkerStored)).EnterTransition(this.off.empty, GameStateMachine<RemoteWorkerDockAnimSM.States, RemoteWorkerDockAnimSM.StatesInstance, RemoteWorkerDockAnimSM, object>.Not(new StateMachine<RemoteWorkerDockAnimSM.States, RemoteWorkerDockAnimSM.StatesInstance, RemoteWorkerDockAnimSM, object>.Transition.ConditionCallback(RemoteWorkerDockAnimSM.States.HasWorkerStored))).Transition(this.on, new StateMachine<RemoteWorkerDockAnimSM.States, RemoteWorkerDockAnimSM.StatesInstance, RemoteWorkerDockAnimSM, object>.Transition.ConditionCallback(RemoteWorkerDockAnimSM.States.IsOnline), UpdateRate.SIM_200ms);
			this.off.full.QueueAnim("off_full", false, null).Transition(this.off.empty, GameStateMachine<RemoteWorkerDockAnimSM.States, RemoteWorkerDockAnimSM.StatesInstance, RemoteWorkerDockAnimSM, object>.Not(new StateMachine<RemoteWorkerDockAnimSM.States, RemoteWorkerDockAnimSM.StatesInstance, RemoteWorkerDockAnimSM, object>.Transition.ConditionCallback(RemoteWorkerDockAnimSM.States.HasWorkerStored)), UpdateRate.SIM_200ms);
			this.off.empty.QueueAnim("off_empty", false, null).Transition(this.off.full, new StateMachine<RemoteWorkerDockAnimSM.States, RemoteWorkerDockAnimSM.StatesInstance, RemoteWorkerDockAnimSM, object>.Transition.ConditionCallback(RemoteWorkerDockAnimSM.States.HasWorkerStored), UpdateRate.SIM_200ms);
			this.on.EnterTransition(this.on.full, new StateMachine<RemoteWorkerDockAnimSM.States, RemoteWorkerDockAnimSM.StatesInstance, RemoteWorkerDockAnimSM, object>.Transition.ConditionCallback(RemoteWorkerDockAnimSM.States.HasWorkerStored)).EnterTransition(this.on.empty, GameStateMachine<RemoteWorkerDockAnimSM.States, RemoteWorkerDockAnimSM.StatesInstance, RemoteWorkerDockAnimSM, object>.Not(new StateMachine<RemoteWorkerDockAnimSM.States, RemoteWorkerDockAnimSM.StatesInstance, RemoteWorkerDockAnimSM, object>.Transition.ConditionCallback(RemoteWorkerDockAnimSM.States.HasWorkerStored))).Transition(this.off, GameStateMachine<RemoteWorkerDockAnimSM.States, RemoteWorkerDockAnimSM.StatesInstance, RemoteWorkerDockAnimSM, object>.Not(new StateMachine<RemoteWorkerDockAnimSM.States, RemoteWorkerDockAnimSM.StatesInstance, RemoteWorkerDockAnimSM, object>.Transition.ConditionCallback(RemoteWorkerDockAnimSM.States.IsOnline)), UpdateRate.SIM_200ms);
			this.on.full.QueueAnim("on_full", false, null).Transition(this.off.empty, GameStateMachine<RemoteWorkerDockAnimSM.States, RemoteWorkerDockAnimSM.StatesInstance, RemoteWorkerDockAnimSM, object>.Not(new StateMachine<RemoteWorkerDockAnimSM.States, RemoteWorkerDockAnimSM.StatesInstance, RemoteWorkerDockAnimSM, object>.Transition.ConditionCallback(RemoteWorkerDockAnimSM.States.HasWorkerStored)), UpdateRate.SIM_200ms);
			this.on.empty.QueueAnim("on_empty", false, null).Transition(this.on.full, new StateMachine<RemoteWorkerDockAnimSM.States, RemoteWorkerDockAnimSM.StatesInstance, RemoteWorkerDockAnimSM, object>.Transition.ConditionCallback(RemoteWorkerDockAnimSM.States.HasWorkerStored), UpdateRate.SIM_200ms);
		}

		// Token: 0x06007B65 RID: 31589 RVA: 0x000F1237 File Offset: 0x000EF437
		public static bool IsOnline(RemoteWorkerDockAnimSM.StatesInstance smi)
		{
			return smi.master.operational.IsOperational && smi.master.dock.RemoteWorker != null;
		}

		// Token: 0x06007B66 RID: 31590 RVA: 0x000F1263 File Offset: 0x000EF463
		public static bool HasWorkerStored(RemoteWorkerDockAnimSM.StatesInstance smi)
		{
			return smi.master.dock.RemoteWorker != null && smi.master.dock.RemoteWorker.Docked;
		}

		// Token: 0x04005C86 RID: 23686
		public RemoteWorkerDockAnimSM.States.FullOrEmptyState on;

		// Token: 0x04005C87 RID: 23687
		public RemoteWorkerDockAnimSM.States.FullOrEmptyState off;

		// Token: 0x0200176C RID: 5996
		public class FullOrEmptyState : GameStateMachine<RemoteWorkerDockAnimSM.States, RemoteWorkerDockAnimSM.StatesInstance, RemoteWorkerDockAnimSM, object>.State
		{
			// Token: 0x04005C88 RID: 23688
			public GameStateMachine<RemoteWorkerDockAnimSM.States, RemoteWorkerDockAnimSM.StatesInstance, RemoteWorkerDockAnimSM, object>.State full;

			// Token: 0x04005C89 RID: 23689
			public GameStateMachine<RemoteWorkerDockAnimSM.States, RemoteWorkerDockAnimSM.StatesInstance, RemoteWorkerDockAnimSM, object>.State empty;
		}
	}
}
