using KSerialization;

[SerializationConfig(MemberSerialization.OptIn)]
public class WaterPurifier : StateMachineComponent<WaterPurifier.StatesInstance> {
    private static readonly EventSystem.IntraObjectHandler<WaterPurifier> OnConduitConnectionChangedDelegate
        = new EventSystem.IntraObjectHandler<WaterPurifier>(delegate(WaterPurifier component, object data) {
                                                                component.OnConduitConnectionChanged(data);
                                                            });

    private ManualDeliveryKG[] deliveryComponents;

    [MyCmpGet]
    private Operational operational;

    protected override void OnSpawn() {
        base.OnSpawn();
        deliveryComponents = GetComponents<ManualDeliveryKG>();
        OnConduitConnectionChanged(GetComponent<ConduitConsumer>().IsConnected);
        Subscribe(-2094018600, OnConduitConnectionChangedDelegate);
        smi.StartSM();
    }

    private void OnConduitConnectionChanged(object data) {
        var pause = (bool)data;
        foreach (var manualDeliveryKG in deliveryComponents) {
            var element = ElementLoader.GetElement(manualDeliveryKG.RequestedItemTag);
            if (element != null && element.IsLiquid) manualDeliveryKG.Pause(pause, "pipe connected");
        }
    }

    public class StatesInstance : GameStateMachine<States, StatesInstance, WaterPurifier, object>.GameInstance {
        public StatesInstance(WaterPurifier smi) : base(smi) { }
    }

    public class States : GameStateMachine<States, StatesInstance, WaterPurifier> {
        public State    off;
        public OnStates on;

        public override void InitializeStates(out BaseState default_state) {
            default_state = off;
            off.PlayAnim("off")
               .EventTransition(GameHashes.OperationalChanged, on, smi => smi.master.operational.IsOperational);

            on.PlayAnim("on")
              .EventTransition(GameHashes.OperationalChanged, off, smi => !smi.master.operational.IsOperational)
              .DefaultState(on.waiting);

            on.waiting.EventTransition(GameHashes.OnStorageChange,
                                       on.working_pre,
                                       smi => smi.master.GetComponent<ElementConverter>()
                                                 .HasEnoughMassToStartConverting());

            on.working_pre.PlayAnim("working_pre").OnAnimQueueComplete(on.working);
            on.working.Enter(delegate(StatesInstance smi) { smi.master.operational.SetActive(true); })
              .QueueAnim("working_loop", true)
              .EventTransition(GameHashes.OnStorageChange,
                               on.working_pst,
                               smi => !smi.master.GetComponent<ElementConverter>().CanConvertAtAll())
              .Exit(delegate(StatesInstance smi) { smi.master.operational.SetActive(false); });

            on.working_pst.PlayAnim("working_pst").OnAnimQueueComplete(on.waiting);
        }

        public class OnStates : State {
            public State waiting;
            public State working;
            public State working_pre;
            public State working_pst;
        }
    }
}