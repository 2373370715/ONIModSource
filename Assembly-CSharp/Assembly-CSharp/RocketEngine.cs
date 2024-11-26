using KSerialization;
using STRINGS;

[SerializationConfig(MemberSerialization.OptIn)]
public class RocketEngine : StateMachineComponent<RocketEngine.StatesInstance> {
    public float         efficiency         = 1f;
    public SimHashes     exhaustElement     = SimHashes.CarbonDioxide;
    public float         exhaustEmitRate    = 50f;
    public float         exhaustTemperature = 1500f;
    public SpawnFXHashes explosionEffectHash;
    public Tag           fuelTag;
    public bool          mainEngine      = true;
    public bool          requireOxidizer = true;

    protected override void OnSpawn() {
        base.OnSpawn();
        smi.StartSM();
        if (mainEngine)
            GetComponent<RocketModule>()
                .AddModuleCondition(ProcessCondition.ProcessConditionType.RocketPrep,
                                    new RequireAttachedComponent(gameObject.GetComponent<AttachableBuilding>(),
                                                                 typeof(FuelTank),
                                                                 UI.STARMAP.COMPONENT.FUEL_TANK));
    }

    public class StatesInstance : GameStateMachine<States, StatesInstance, RocketEngine, object>.GameInstance {
        public StatesInstance(RocketEngine smi) : base(smi) { }
    }

    public class States : GameStateMachine<States, StatesInstance, RocketEngine> {
        public State burnComplete;
        public State burning;
        public State idle;

        public override void InitializeStates(out BaseState default_state) {
            default_state = idle;
            idle.PlayAnim("grounded", KAnim.PlayMode.Loop).EventTransition(GameHashes.IgniteEngine, burning);
            burning.EventTransition(GameHashes.RocketLanded, burnComplete)
                   .PlayAnim("launch_pre")
                   .QueueAnim("launch_loop", true)
                   .Update(delegate(StatesInstance smi, float dt) {
                               var num = Grid.PosToCell(smi.master.gameObject.transform.GetPosition() +
                                                        smi.master.GetComponent<KBatchedAnimController>().Offset);

                               if (Grid.IsValidCell(num))
                                   SimMessages.EmitMass(num,
                                                        ElementLoader.GetElementIndex(smi.master.exhaustElement),
                                                        dt * smi.master.exhaustEmitRate,
                                                        smi.master.exhaustTemperature,
                                                        0,
                                                        0);

                               var num2 = 10;
                               for (var i = 1; i < num2; i++) {
                                   var num3 = Grid.OffsetCell(num, -1, -i);
                                   var num4 = Grid.OffsetCell(num, 0,  -i);
                                   var num5 = Grid.OffsetCell(num, 1,  -i);
                                   if (Grid.IsValidCell(num3))
                                       SimMessages.ModifyEnergy(num3,
                                                                smi.master.exhaustTemperature / (i + 1),
                                                                3200f,
                                                                SimMessages.EnergySourceID.Burner);

                                   if (Grid.IsValidCell(num4))
                                       SimMessages.ModifyEnergy(num4,
                                                                smi.master.exhaustTemperature / i,
                                                                3200f,
                                                                SimMessages.EnergySourceID.Burner);

                                   if (Grid.IsValidCell(num5))
                                       SimMessages.ModifyEnergy(num5,
                                                                smi.master.exhaustTemperature / (i + 1),
                                                                3200f,
                                                                SimMessages.EnergySourceID.Burner);
                               }
                           });

            burnComplete.PlayAnim("grounded", KAnim.PlayMode.Loop).EventTransition(GameHashes.IgniteEngine, burning);
        }
    }
}