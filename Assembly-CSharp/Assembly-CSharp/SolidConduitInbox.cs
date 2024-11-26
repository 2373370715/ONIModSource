using KSerialization;

[SerializationConfig(MemberSerialization.OptIn)]
public class SolidConduitInbox : StateMachineComponent<SolidConduitInbox.SMInstance>, ISim1000ms {
    [MyCmpReq]
    private SolidConduitDispenser dispenser;

    private FilteredStorage filteredStorage;

    [MyCmpReq]
    private Operational operational;

    [MyCmpAdd]
    private Storage storage;

    public void Sim1000ms(float dt) {
        if (operational.IsOperational && dispenser.IsDispensing) {
            operational.SetActive(true);
            return;
        }

        operational.SetActive(false);
    }

    protected override void OnPrefabInit() {
        base.OnPrefabInit();
        filteredStorage = new FilteredStorage(this, null, null, false, Db.Get().ChoreTypes.StorageFetch);
    }

    protected override void OnSpawn() {
        base.OnSpawn();
        filteredStorage.FilterChanged();
        smi.StartSM();
    }

    protected override void OnCleanUp() { base.OnCleanUp(); }

    public class SMInstance : GameStateMachine<States, SMInstance, SolidConduitInbox, object>.GameInstance {
        public SMInstance(SolidConduitInbox master) : base(master) { }
    }

    public class States : GameStateMachine<States, SMInstance, SolidConduitInbox> {
        public State       off;
        public ReadyStates on;

        public override void InitializeStates(out BaseState default_state) {
            default_state = off;
            root.DoNothing();
            off.PlayAnim("off")
               .EventTransition(GameHashes.OperationalChanged,
                                on,
                                smi => smi.GetComponent<Operational>().IsOperational);

            on.DefaultState(on.idle)
              .EventTransition(GameHashes.OperationalChanged,
                               off,
                               smi => !smi.GetComponent<Operational>().IsOperational);

            on.idle.PlayAnim("on")
              .EventTransition(GameHashes.ActiveChanged, on.working, smi => smi.GetComponent<Operational>().IsActive);

            on.working.PlayAnim("working_pre")
              .QueueAnim("working_loop", true)
              .EventTransition(GameHashes.ActiveChanged, on.post, smi => !smi.GetComponent<Operational>().IsActive);

            on.post.PlayAnim("working_pst").OnAnimQueueComplete(on);
        }

        public class ReadyStates : State {
            public State idle;
            public State post;
            public State working;
        }
    }
}