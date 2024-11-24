using KSerialization;
using STRINGS;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class DevHEPSpawner : StateMachineComponent<DevHEPSpawner.StatesInstance>, IHighEnergyParticleDirection, ISingleSliderControl, ISliderControl
{
	public class StatesInstance : GameStateMachine<States, StatesInstance, DevHEPSpawner, object>.GameInstance
	{
		public StatesInstance(DevHEPSpawner smi)
			: base(smi)
		{
		}
	}

	public class States : GameStateMachine<States, StatesInstance, DevHEPSpawner>
	{
		public BoolParameter isAbsorbingRadiation;

		public State ready;

		public State inoperational;

		public override void InitializeStates(out BaseState default_state)
		{
			default_state = inoperational;
			inoperational.PlayAnim("off").TagTransition(GameTags.Operational, ready);
			ready.PlayAnim("on").TagTransition(GameTags.Operational, inoperational, on_remove: true).Update(delegate(StatesInstance smi, float dt)
			{
				smi.master.LauncherUpdate(dt);
			}, UpdateRate.SIM_EVERY_TICK);
		}
	}

	[MyCmpGet]
	private Operational operational;

	[Serialize]
	private EightDirection _direction;

	public float boltAmount;

	private EightDirectionController directionController;

	private float launcherTimer;

	private MeterController particleController;

	private MeterController progressMeterController;

	[Serialize]
	public Ref<HighEnergyParticlePort> capturedByRef = new Ref<HighEnergyParticlePort>();

	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	private static readonly EventSystem.IntraObjectHandler<DevHEPSpawner> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<DevHEPSpawner>(delegate(DevHEPSpawner component, object data)
	{
		component.OnCopySettings(data);
	});

	public EightDirection Direction
	{
		get
		{
			return _direction;
		}
		set
		{
			_direction = value;
			if (directionController != null)
			{
				directionController.SetRotation(45 * EightDirectionUtil.GetDirectionIndex(_direction));
				directionController.controller.enabled = false;
				directionController.controller.enabled = true;
			}
		}
	}

	public string SliderTitleKey => "";

	public string SliderUnits => UI.UNITSUFFIXES.HIGHENERGYPARTICLES.PARTRICLES;

	private void OnCopySettings(object data)
	{
		DevHEPSpawner component = ((GameObject)data).GetComponent<DevHEPSpawner>();
		if (component != null)
		{
			Direction = component.Direction;
			boltAmount = component.boltAmount;
		}
	}

	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Subscribe(-905833192, OnCopySettingsDelegate);
	}

	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
		directionController = new EightDirectionController(GetComponent<KBatchedAnimController>(), "redirector_target", "redirect", EightDirectionController.Offset.Infront);
		Direction = Direction;
		particleController = new MeterController(GetComponent<KBatchedAnimController>(), "orb_target", "orb_off", Meter.Offset.NoChange, Grid.SceneLayer.NoLayer);
		particleController.gameObject.AddOrGet<LoopingSounds>();
		progressMeterController = new MeterController(GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer);
	}

	public void LauncherUpdate(float dt)
	{
		if (boltAmount <= 0f)
		{
			return;
		}
		launcherTimer += dt;
		progressMeterController.SetPositionPercent(launcherTimer / 5f);
		if (launcherTimer > 5f)
		{
			launcherTimer -= 5f;
			int highEnergyParticleOutputCell = GetComponent<Building>().GetHighEnergyParticleOutputCell();
			GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab("HighEnergyParticle"), Grid.CellToPosCCC(highEnergyParticleOutputCell, Grid.SceneLayer.FXFront2), Grid.SceneLayer.FXFront2);
			gameObject.SetActive(value: true);
			if (gameObject != null)
			{
				HighEnergyParticle component = gameObject.GetComponent<HighEnergyParticle>();
				component.payload = boltAmount;
				component.SetDirection(Direction);
				directionController.PlayAnim("redirect_send");
				directionController.controller.Queue("redirect");
				particleController.meterController.Play("orb_send");
				particleController.meterController.Queue("orb_off");
			}
		}
	}

	public int SliderDecimalPlaces(int index)
	{
		return 0;
	}

	public float GetSliderMin(int index)
	{
		return 0f;
	}

	public float GetSliderMax(int index)
	{
		return 500f;
	}

	public float GetSliderValue(int index)
	{
		return boltAmount;
	}

	public void SetSliderValue(float value, int index)
	{
		boltAmount = value;
	}

	public string GetSliderTooltipKey(int index)
	{
		return "";
	}

	string ISliderControl.GetSliderTooltip(int index)
	{
		return "";
	}
}
