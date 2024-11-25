using Klei.AI;

public class
    ChilledBones : GameStateMachine<ChilledBones, ChilledBones.Instance, IStateMachineTarget, ChilledBones.Def> {
    public const string EFFECT_NAME = "ChilledBones";
    public       State  chilled;
    public       State  normal;

    public override void InitializeStates(out BaseState default_state) {
        serializable  = SerializeType.ParamsOnly;
        default_state = normal;
        normal.UpdateTransition(chilled, IsChilling);
        chilled.ToggleEffect("ChilledBones").UpdateTransition(normal, IsNotChilling);
    }

    public bool IsNotChilling(Instance smi, float dt) { return !IsChilling(smi, dt); }
    public bool IsChilling(Instance    smi, float dt) { return smi.IsChilled; }

    public class Def : BaseDef {
        public float THRESHOLD = -1f;
    }

    public new class Instance : GameInstance {
        public Attribute bodyTemperatureTransferAttribute;

        [MyCmpGet]
        public MinionModifiers minionModifiers;

        public Instance(IStateMachineTarget master, Def def) : base(master, def) {
            bodyTemperatureTransferAttribute = Db.Get().Attributes.TryGet("TemperatureDelta");
        }

        public float TemperatureTransferAttribute =>
            minionModifiers.GetAttributes().GetValue(bodyTemperatureTransferAttribute.Id) * 600f;

        public bool IsChilled => TemperatureTransferAttribute < def.THRESHOLD;
    }
}