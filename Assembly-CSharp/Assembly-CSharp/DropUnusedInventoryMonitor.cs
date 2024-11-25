public class DropUnusedInventoryMonitor
    : GameStateMachine<DropUnusedInventoryMonitor, DropUnusedInventoryMonitor.Instance> {
    public State hasinventory;
    public State satisfied;

    public override void InitializeStates(out BaseState default_state) {
        default_state = satisfied;
        satisfied.EventTransition(GameHashes.OnStorageChange,
                                  hasinventory,
                                  smi => smi.GetComponent<Storage>().Count > 0);

        hasinventory
            .EventTransition(GameHashes.OnStorageChange, hasinventory, smi => smi.GetComponent<Storage>().Count == 0)
            .ToggleChore(smi => new DropUnusedInventoryChore(Db.Get().ChoreTypes.DropUnusedInventory, smi.master),
                         satisfied);
    }

    public new class Instance : GameInstance {
        public Instance(IStateMachineTarget master) : base(master) { }
    }
}