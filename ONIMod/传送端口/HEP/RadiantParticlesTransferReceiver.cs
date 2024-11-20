using KSerialization;



public class RadiantParticlesTransferReceiver
    : StateMachineComponent<RadiantParticlesTransferReceiver.StatesInstance>, IMyHighEnergyParticleDirection {
    [Serialize]
    private EightDirection _direction;

    [MyCmpGet]
    private KBatchedAnimController anim;

    private EightDirectionController directionController;
    public  float                    directorDelay = 0.5f;

    [MyCmpReq]
    private Operational operational;

    [MyCmpGet]
    private HighEnergyParticlePort port;

    // public static HashedString PORT_ID = "RadiantParticlesTransferReceiver";

    [MyCmpReq]
    private KSelectable selectable;

    [MyCmpReq]
    private HighEnergyParticleStorage storage;

    /// <summary>
    /// 是否需要发射
    /// </summary>
    // private bool needLaunch = false;

    /// <summary>
    ///     是否可用传送
    /// </summary>
    public bool Transmissible => operational != null && operational.IsOperational;

    public EightDirection Direction {
        get => _direction;
        set {
            _direction = value;

            directionController.SetRotation(45 * EightDirectionUtil.GetDirectionIndex(_direction));
            directionController.controller.enabled = false;
            directionController.controller.enabled = true;
        }
    }

    protected override void OnPrefabInit() { base.OnPrefabInit(); }

    protected override void OnSpawn() {
        base.OnSpawn();

        // Subscribe((int) GameHashes.OperationalChanged, OnOperationalChanged);
        directionController = new EightDirectionController(GetComponent<KBatchedAnimController>(),
                                                           "redirector_target",
                                                           "redirector_off",
                                                           EightDirectionController.Offset.Infront);

        Direction = Direction;
        smi.StartSM();
    }

    protected override void OnCleanUp() { base.OnCleanUp(); }
    public int GetInputSignal() { return GetComponent<LogicPorts>().GetInputValue(LogicOperationalController.PORT_ID); }

    public float StoreAndLaunch(float amount) {
        if (amount <= 0) return 0;

        return storage.Store(amount);
    }

    private void LaunchParticle() {
        if (storage.Particles < 0.100000001490116)
            storage.ConsumeAll();
        else {
            var particleOutputCell = GetComponent<Building>().GetHighEnergyParticleOutputCell();
            var gameObject = GameUtil.KInstantiate(global::Assets.GetPrefab((Tag)"HighEnergyParticle"),
                                                   Grid.CellToPosCCC(particleOutputCell, Grid.SceneLayer.FXFront2),
                                                   Grid.SceneLayer.FXFront2);

            gameObject.SetActive(true);
            if (!(gameObject != null)) return;

            var component = gameObject.GetComponent<HighEnergyParticle>();
            component.payload    =  storage.ConsumeAll();
            component.payload    -= 0.1f;
            component.capturedBy =  port;
            component.SetDirection(Direction);
            directionController.PlayAnim("redirector_send");
        }
    }

    public class StatesInstance
        : GameStateMachine<States, StatesInstance, RadiantParticlesTransferReceiver, object>.GameInstance {
        public StatesInstance(RadiantParticlesTransferReceiver smi) : base(smi) { }
    }

    public class States : GameStateMachine<States, StatesInstance, RadiantParticlesTransferReceiver> {
        // public State launch_pre;
        public State launch;
        public State off;
        public State ready;

        public override void InitializeStates(out BaseState default_state) {
            default_state = off;
            off.PlayAnim("off")
               .Enter(smi => smi.master.directionController.PlayAnim("redirector_off"))
               .TagTransition(GameTags.Operational, ready);

            ready.PlayAnim("on")
                 .Enter(smi => smi.master.directionController.PlayAnim("redirector_on", KAnim.PlayMode.Loop))
                 .TagTransition(GameTags.Operational, off, true)
                 .UpdateTransition(launch, (smi, dt) => !smi.master.storage.IsEmpty());

            // .EventTransition(GameHashes.OnParticleStorageChanged, launch);

            // launch_pre.PlayAnim("on").ScheduleGoTo(smi => smi.master.directorDelay, launch);
            launch.PlayAnim("on")
                  .Enter(smi => smi.master.LaunchParticle())
                  .ScheduleGoTo(smi => smi.master.directorDelay, ready);
        }
    }
}