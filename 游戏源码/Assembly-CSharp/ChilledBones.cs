using Klei.AI;

public class ChilledBones : GameStateMachine<ChilledBones, ChilledBones.Instance, IStateMachineTarget, ChilledBones.Def>
{
	public class Def : BaseDef
	{
		public float THRESHOLD = -1f;
	}

	public new class Instance : GameInstance
	{
		[MyCmpGet]
		public MinionModifiers minionModifiers;

		public Attribute bodyTemperatureTransferAttribute;

		public float TemperatureTransferAttribute => minionModifiers.GetAttributes().GetValue(bodyTemperatureTransferAttribute.Id) * 600f;

		public bool IsChilled => TemperatureTransferAttribute < base.def.THRESHOLD;

		public Instance(IStateMachineTarget master, Def def)
			: base(master, def)
		{
			bodyTemperatureTransferAttribute = Db.Get().Attributes.TryGet("TemperatureDelta");
		}
	}

	public const string EFFECT_NAME = "ChilledBones";

	public State normal;

	public State chilled;

	public override void InitializeStates(out BaseState default_state)
	{
		base.serializable = SerializeType.ParamsOnly;
		default_state = normal;
		normal.UpdateTransition(chilled, IsChilling);
		chilled.ToggleEffect("ChilledBones").UpdateTransition(normal, IsNotChilling);
	}

	public bool IsNotChilling(Instance smi, float dt)
	{
		return !IsChilling(smi, dt);
	}

	public bool IsChilling(Instance smi, float dt)
	{
		return smi.IsChilled;
	}
}
