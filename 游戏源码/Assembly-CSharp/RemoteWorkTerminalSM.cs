using System;

// Token: 0x02001745 RID: 5957
public class RemoteWorkTerminalSM : StateMachineComponent<RemoteWorkTerminalSM.StatesInstance>
{
	// Token: 0x06007AB9 RID: 31417 RVA: 0x000F0A2E File Offset: 0x000EEC2E
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x04005C0F RID: 23567
	[MyCmpGet]
	private RemoteWorkTerminal terminal;

	// Token: 0x04005C10 RID: 23568
	[MyCmpGet]
	private Operational operational;

	// Token: 0x02001746 RID: 5958
	public class StatesInstance : GameStateMachine<RemoteWorkTerminalSM.States, RemoteWorkTerminalSM.StatesInstance, RemoteWorkTerminalSM, object>.GameInstance
	{
		// Token: 0x06007ABB RID: 31419 RVA: 0x000F0A49 File Offset: 0x000EEC49
		public StatesInstance(RemoteWorkTerminalSM master) : base(master)
		{
		}
	}

	// Token: 0x02001747 RID: 5959
	public class States : GameStateMachine<RemoteWorkTerminalSM.States, RemoteWorkTerminalSM.StatesInstance, RemoteWorkTerminalSM>
	{
		// Token: 0x06007ABC RID: 31420 RVA: 0x00319B48 File Offset: 0x00317D48
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.offline;
			this.offline.Transition(this.online, GameStateMachine<RemoteWorkTerminalSM.States, RemoteWorkTerminalSM.StatesInstance, RemoteWorkTerminalSM, object>.And(new StateMachine<RemoteWorkTerminalSM.States, RemoteWorkTerminalSM.StatesInstance, RemoteWorkTerminalSM, object>.Transition.ConditionCallback(RemoteWorkTerminalSM.States.HasAssignedDock), new StateMachine<RemoteWorkTerminalSM.States, RemoteWorkTerminalSM.StatesInstance, RemoteWorkTerminalSM, object>.Transition.ConditionCallback(RemoteWorkTerminalSM.States.IsOperational)), UpdateRate.SIM_200ms).Transition(this.offline.no_dock, GameStateMachine<RemoteWorkTerminalSM.States, RemoteWorkTerminalSM.StatesInstance, RemoteWorkTerminalSM, object>.Not(new StateMachine<RemoteWorkTerminalSM.States, RemoteWorkTerminalSM.StatesInstance, RemoteWorkTerminalSM, object>.Transition.ConditionCallback(RemoteWorkTerminalSM.States.HasAssignedDock)), UpdateRate.SIM_200ms);
			this.offline.no_dock.Transition(this.offline, new StateMachine<RemoteWorkTerminalSM.States, RemoteWorkTerminalSM.StatesInstance, RemoteWorkTerminalSM, object>.Transition.ConditionCallback(RemoteWorkTerminalSM.States.HasAssignedDock), UpdateRate.SIM_200ms).ToggleStatusItem(Db.Get().BuildingStatusItems.RemoteWorkTerminalNoDock, null);
			this.online.ToggleRecurringChore(new Func<RemoteWorkTerminalSM.StatesInstance, Chore>(RemoteWorkTerminalSM.States.CreateChore), null).Transition(this.offline, GameStateMachine<RemoteWorkTerminalSM.States, RemoteWorkTerminalSM.StatesInstance, RemoteWorkTerminalSM, object>.Not(GameStateMachine<RemoteWorkTerminalSM.States, RemoteWorkTerminalSM.StatesInstance, RemoteWorkTerminalSM, object>.And(new StateMachine<RemoteWorkTerminalSM.States, RemoteWorkTerminalSM.StatesInstance, RemoteWorkTerminalSM, object>.Transition.ConditionCallback(RemoteWorkTerminalSM.States.HasAssignedDock), new StateMachine<RemoteWorkTerminalSM.States, RemoteWorkTerminalSM.StatesInstance, RemoteWorkTerminalSM, object>.Transition.ConditionCallback(RemoteWorkTerminalSM.States.IsOperational))), UpdateRate.SIM_200ms);
		}

		// Token: 0x06007ABD RID: 31421 RVA: 0x000F0A52 File Offset: 0x000EEC52
		public static bool IsOperational(RemoteWorkTerminalSM.StatesInstance smi)
		{
			return smi.master.operational.IsOperational;
		}

		// Token: 0x06007ABE RID: 31422 RVA: 0x000F0A64 File Offset: 0x000EEC64
		public static bool HasAssignedDock(RemoteWorkTerminalSM.StatesInstance smi)
		{
			return smi.master.terminal.CurrentDock != null;
		}

		// Token: 0x06007ABF RID: 31423 RVA: 0x000F0A7C File Offset: 0x000EEC7C
		public static Chore CreateChore(RemoteWorkTerminalSM.StatesInstance smi)
		{
			return new RemoteChore(smi.master.terminal);
		}

		// Token: 0x04005C11 RID: 23569
		public GameStateMachine<RemoteWorkTerminalSM.States, RemoteWorkTerminalSM.StatesInstance, RemoteWorkTerminalSM, object>.State online;

		// Token: 0x04005C12 RID: 23570
		public RemoteWorkTerminalSM.States.OfflineStates offline;

		// Token: 0x02001748 RID: 5960
		public class OfflineStates : GameStateMachine<RemoteWorkTerminalSM.States, RemoteWorkTerminalSM.StatesInstance, RemoteWorkTerminalSM, object>.State
		{
			// Token: 0x04005C13 RID: 23571
			public GameStateMachine<RemoteWorkTerminalSM.States, RemoteWorkTerminalSM.StatesInstance, RemoteWorkTerminalSM, object>.State no_dock;
		}
	}
}
