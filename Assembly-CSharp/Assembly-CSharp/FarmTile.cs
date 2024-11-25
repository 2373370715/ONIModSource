public class FarmTile : StateMachineComponent<FarmTile.SMInstance> {
    [MyCmpReq]
    private PlantablePlot plantablePlot;

    [MyCmpReq]
    private Storage storage;

    protected override void OnSpawn() {
        base.OnSpawn();
        smi.StartSM();
    }

    public class SMInstance : GameStateMachine<States, SMInstance, FarmTile, object>.GameInstance {
        public SMInstance(FarmTile master) : base(master) { }

        public bool HasWater() {
            var primaryElement = master.storage.FindPrimaryElement(SimHashes.Water);
            return primaryElement != null && primaryElement.Mass > 0f;
        }
    }

    public class States : GameStateMachine<States, SMInstance, FarmTile> {
        public FarmStates empty;
        public FarmStates full;

        public override void InitializeStates(out BaseState default_state) {
            default_state = empty;
            empty.EventTransition(GameHashes.OccupantChanged, full, smi => smi.master.plantablePlot.Occupant != null);
            empty.wet.EventTransition(GameHashes.OnStorageChange, empty.dry, smi => !smi.HasWater());
            empty.dry.EventTransition(GameHashes.OnStorageChange, empty.wet, smi => !smi.HasWater());
            full.EventTransition(GameHashes.OccupantChanged, empty, smi => smi.master.plantablePlot.Occupant == null);
            full.wet.EventTransition(GameHashes.OnStorageChange, full.dry, smi => !smi.HasWater());
            full.dry.EventTransition(GameHashes.OnStorageChange, full.wet, smi => !smi.HasWater());
        }

        public class FarmStates : State {
            public State dry;
            public State wet;
        }
    }
}