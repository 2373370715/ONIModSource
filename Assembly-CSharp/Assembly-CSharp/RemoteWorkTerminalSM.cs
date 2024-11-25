public class RemoteWorkTerminalSM : StateMachineComponent<RemoteWorkTerminalSM.StatesInstance> {
    [MyCmpGet]
    private Operational operational;

    [MyCmpGet]
    private RemoteWorkTerminal terminal;

    protected override void OnSpawn() {
        base.OnSpawn();
        smi.StartSM();
    }

    public class StatesInstance : GameStateMachine<States, StatesInstance, RemoteWorkTerminalSM, object>.GameInstance {
        public StatesInstance(RemoteWorkTerminalSM master) : base(master) { }
    }

    public class States : GameStateMachine<States, StatesInstance, RemoteWorkTerminalSM> {
        public OfflineStates offline;
        public State         online;

        public override void InitializeStates(out BaseState default_state) {
            default_state = offline;
            offline.Transition(online, And(HasAssignedDock, IsOperational))
                   .Transition(offline.no_dock, Not(HasAssignedDock));

            offline.no_dock.Transition(offline, HasAssignedDock)
                   .ToggleStatusItem(Db.Get().BuildingStatusItems.RemoteWorkTerminalNoDock, null);

            online.ToggleRecurringChore(CreateChore).Transition(offline, Not(And(HasAssignedDock, IsOperational)));
        }

        public static bool  IsOperational(StatesInstance   smi) { return smi.master.operational.IsOperational; }
        public static bool  HasAssignedDock(StatesInstance smi) { return smi.master.terminal.CurrentDock != null; }
        public static Chore CreateChore(StatesInstance     smi) { return new RemoteChore(smi.master.terminal); }

        public class OfflineStates : State {
            public State no_dock;
        }
    }
}