public class Campfire : GameStateMachine<Campfire, Campfire.Instance, IStateMachineTarget, Campfire.Def>
{
	public class Def : BaseDef
	{
		public Tag fuelTag;

		public float initialFuelMass;
	}

	public class OperationalStates : State
	{
		public State needsFuel;

		public State working;
	}

	public new class Instance : GameInstance
	{
		[MyCmpGet]
		public Operational operational;

		[MyCmpGet]
		public Storage storage;

		[MyCmpGet]
		public RangeVisualizer rangeVisualizer;

		[MyCmpGet]
		public Light2D light;

		[MyCmpGet]
		public DirectVolumeHeater heater;

		[MyCmpGet]
		public DecorProvider decorProvider;

		public bool HasFuel => storage.MassStored() > 0f;

		public bool IsAuraEnabled => base.sm.WarmAuraEnabled.Get(this);

		public Instance(IStateMachineTarget master, Def def)
			: base(master, def)
		{
		}

		public void EnableHeatEmission()
		{
			operational.SetActive(value: true);
			light.enabled = true;
			heater.EnableEmission = true;
			decorProvider.SetValues(CampfireConfig.DECOR_ON);
			decorProvider.Refresh();
		}

		public void DisableHeatEmission()
		{
			operational.SetActive(value: false);
			light.enabled = false;
			heater.EnableEmission = false;
			decorProvider.SetValues(CampfireConfig.DECOR_OFF);
			decorProvider.Refresh();
		}
	}

	public const string LIT_ANIM_NAME = "on";

	public const string UNLIT_ANIM_NAME = "off";

	public State noOperational;

	public OperationalStates operational;

	public BoolParameter WarmAuraEnabled;

	public override void InitializeStates(out BaseState default_state)
	{
		base.serializable = SerializeType.ParamsOnly;
		default_state = noOperational;
		noOperational.Enter(DisableHeatEmission).TagTransition(GameTags.Operational, operational).PlayAnim("off", KAnim.PlayMode.Once);
		operational.TagTransition(GameTags.Operational, noOperational, on_remove: true).DefaultState(operational.needsFuel);
		operational.needsFuel.Enter(DisableHeatEmission).EventTransition(GameHashes.OnStorageChange, operational.working, HasFuel).PlayAnim("off", KAnim.PlayMode.Once);
		operational.working.Enter(EnableHeatEmission).EventTransition(GameHashes.OnStorageChange, operational.needsFuel, GameStateMachine<Campfire, Instance, IStateMachineTarget, Def>.Not(HasFuel)).PlayAnim("on", KAnim.PlayMode.Loop)
			.Exit(DisableHeatEmission);
	}

	public static bool HasFuel(Instance smi)
	{
		return smi.HasFuel;
	}

	public static void EnableHeatEmission(Instance smi)
	{
		smi.EnableHeatEmission();
	}

	public static void DisableHeatEmission(Instance smi)
	{
		smi.DisableHeatEmission();
	}
}
