using UnityEngine;

#if 原油精炼器
public class AutomaticOilRefinery : StateMachineComponent<AutomaticOilRefinery.StatesInstance> {
    private static readonly EventSystem.IntraObjectHandler<AutomaticOilRefinery> OnStorageChangedDelegate
        = new EventSystem.IntraObjectHandler<AutomaticOilRefinery>(delegate(AutomaticOilRefinery component,
                                                                            object               data) {
                                                                       component.OnStorageChanged(data);
                                                                   });

    private float           cellCount;
    private float           envPressure;
    private float           maxSrcMass;
    private MeterController meter;

    [MyCmpReq]
    private Operational operational = null;

    [SerializeField]
    public float overpressureMass = 5f;

    [SerializeField]
    public float overpressureWarningMass = 4.5f;

    [MyCmpGet]
    private Storage storage = null;

    protected override void OnSpawn() {
        Subscribe(-1697596308, OnStorageChangedDelegate);
        var component = GetComponent<KBatchedAnimController>();
        meter = new MeterController(component,
                                    "meter_target",
                                    "meter",
                                    Meter.Offset.Infront,
                                    Grid.SceneLayer.NoLayer,
                                    Vector3.zero,
                                    null);

        smi.StartSM();
        maxSrcMass = GetComponent<ConduitConsumer>().capacityKG;
    }

    private void OnStorageChanged(object data) {
        var positionPercent = Mathf.Clamp01(storage.GetMassAvailable(SimHashes.CrudeOil) / maxSrcMass);

        meter.SetPositionPercent(positionPercent);
    }

    private static bool UpdateStateCb(int cell, object data) {
        var automaticOilRefinery = data as AutomaticOilRefinery;
        var isGas                = Grid.Element[cell].IsGas;
        var flag                 = isGas;
        var flag2                = flag;
        if (flag2) {
            automaticOilRefinery.cellCount   += 1f;
            automaticOilRefinery.envPressure += Grid.Mass[cell];
        }

        return true;
    }

    public class StatesInstance : GameStateMachine<States, StatesInstance, AutomaticOilRefinery, object>.GameInstance {
        public StatesInstance(AutomaticOilRefinery smi) : base(smi) { }
    }

    public class States : GameStateMachine<States, StatesInstance, AutomaticOilRefinery> {
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
#endif