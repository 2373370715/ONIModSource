using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000D2C RID: 3372
[SerializationConfig(MemberSerialization.OptIn)]
public class DevHEPSpawner : StateMachineComponent<DevHEPSpawner.StatesInstance>, IHighEnergyParticleDirection, ISingleSliderControl, ISliderControl
{
	// Token: 0x1700033A RID: 826
	// (get) Token: 0x060041E3 RID: 16867 RVA: 0x000CA97C File Offset: 0x000C8B7C
	// (set) Token: 0x060041E4 RID: 16868 RVA: 0x0023FA50 File Offset: 0x0023DC50
	public EightDirection Direction
	{
		get
		{
			return this._direction;
		}
		set
		{
			this._direction = value;
			if (this.directionController != null)
			{
				this.directionController.SetRotation((float)(45 * EightDirectionUtil.GetDirectionIndex(this._direction)));
				this.directionController.controller.enabled = false;
				this.directionController.controller.enabled = true;
			}
		}
	}

	// Token: 0x060041E5 RID: 16869 RVA: 0x0023FAA8 File Offset: 0x0023DCA8
	private void OnCopySettings(object data)
	{
		DevHEPSpawner component = ((GameObject)data).GetComponent<DevHEPSpawner>();
		if (component != null)
		{
			this.Direction = component.Direction;
			this.boltAmount = component.boltAmount;
		}
	}

	// Token: 0x060041E6 RID: 16870 RVA: 0x000CA984 File Offset: 0x000C8B84
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<DevHEPSpawner>(-905833192, DevHEPSpawner.OnCopySettingsDelegate);
	}

	// Token: 0x060041E7 RID: 16871 RVA: 0x0023FAE4 File Offset: 0x0023DCE4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
		this.directionController = new EightDirectionController(base.GetComponent<KBatchedAnimController>(), "redirector_target", "redirect", EightDirectionController.Offset.Infront);
		this.Direction = this.Direction;
		this.particleController = new MeterController(base.GetComponent<KBatchedAnimController>(), "orb_target", "orb_off", Meter.Offset.NoChange, Grid.SceneLayer.NoLayer, Array.Empty<string>());
		this.particleController.gameObject.AddOrGet<LoopingSounds>();
		this.progressMeterController = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Array.Empty<string>());
	}

	// Token: 0x060041E8 RID: 16872 RVA: 0x0023FB84 File Offset: 0x0023DD84
	public void LauncherUpdate(float dt)
	{
		if (this.boltAmount <= 0f)
		{
			return;
		}
		this.launcherTimer += dt;
		this.progressMeterController.SetPositionPercent(this.launcherTimer / 5f);
		if (this.launcherTimer > 5f)
		{
			this.launcherTimer -= 5f;
			int highEnergyParticleOutputCell = base.GetComponent<Building>().GetHighEnergyParticleOutputCell();
			GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab("HighEnergyParticle"), Grid.CellToPosCCC(highEnergyParticleOutputCell, Grid.SceneLayer.FXFront2), Grid.SceneLayer.FXFront2, null, 0);
			gameObject.SetActive(true);
			if (gameObject != null)
			{
				HighEnergyParticle component = gameObject.GetComponent<HighEnergyParticle>();
				component.payload = this.boltAmount;
				component.SetDirection(this.Direction);
				this.directionController.PlayAnim("redirect_send", KAnim.PlayMode.Once);
				this.directionController.controller.Queue("redirect", KAnim.PlayMode.Once, 1f, 0f);
				this.particleController.meterController.Play("orb_send", KAnim.PlayMode.Once, 1f, 0f);
				this.particleController.meterController.Queue("orb_off", KAnim.PlayMode.Once, 1f, 0f);
			}
		}
	}

	// Token: 0x1700033B RID: 827
	// (get) Token: 0x060041E9 RID: 16873 RVA: 0x000CA99D File Offset: 0x000C8B9D
	public string SliderTitleKey
	{
		get
		{
			return "";
		}
	}

	// Token: 0x1700033C RID: 828
	// (get) Token: 0x060041EA RID: 16874 RVA: 0x000CA9A4 File Offset: 0x000C8BA4
	public string SliderUnits
	{
		get
		{
			return UI.UNITSUFFIXES.HIGHENERGYPARTICLES.PARTRICLES;
		}
	}

	// Token: 0x060041EB RID: 16875 RVA: 0x000AD2F7 File Offset: 0x000AB4F7
	public int SliderDecimalPlaces(int index)
	{
		return 0;
	}

	// Token: 0x060041EC RID: 16876 RVA: 0x000BCEBF File Offset: 0x000BB0BF
	public float GetSliderMin(int index)
	{
		return 0f;
	}

	// Token: 0x060041ED RID: 16877 RVA: 0x000CA9B0 File Offset: 0x000C8BB0
	public float GetSliderMax(int index)
	{
		return 500f;
	}

	// Token: 0x060041EE RID: 16878 RVA: 0x000CA9B7 File Offset: 0x000C8BB7
	public float GetSliderValue(int index)
	{
		return this.boltAmount;
	}

	// Token: 0x060041EF RID: 16879 RVA: 0x000CA9BF File Offset: 0x000C8BBF
	public void SetSliderValue(float value, int index)
	{
		this.boltAmount = value;
	}

	// Token: 0x060041F0 RID: 16880 RVA: 0x000CA99D File Offset: 0x000C8B9D
	public string GetSliderTooltipKey(int index)
	{
		return "";
	}

	// Token: 0x060041F1 RID: 16881 RVA: 0x000CA99D File Offset: 0x000C8B9D
	string ISliderControl.GetSliderTooltip(int index)
	{
		return "";
	}

	// Token: 0x04002CF3 RID: 11507
	[MyCmpGet]
	private Operational operational;

	// Token: 0x04002CF4 RID: 11508
	[Serialize]
	private EightDirection _direction;

	// Token: 0x04002CF5 RID: 11509
	public float boltAmount;

	// Token: 0x04002CF6 RID: 11510
	private EightDirectionController directionController;

	// Token: 0x04002CF7 RID: 11511
	private float launcherTimer;

	// Token: 0x04002CF8 RID: 11512
	private MeterController particleController;

	// Token: 0x04002CF9 RID: 11513
	private MeterController progressMeterController;

	// Token: 0x04002CFA RID: 11514
	[Serialize]
	public Ref<HighEnergyParticlePort> capturedByRef = new Ref<HighEnergyParticlePort>();

	// Token: 0x04002CFB RID: 11515
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04002CFC RID: 11516
	private static readonly EventSystem.IntraObjectHandler<DevHEPSpawner> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<DevHEPSpawner>(delegate(DevHEPSpawner component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x02000D2D RID: 3373
	public class StatesInstance : GameStateMachine<DevHEPSpawner.States, DevHEPSpawner.StatesInstance, DevHEPSpawner, object>.GameInstance
	{
		// Token: 0x060041F4 RID: 16884 RVA: 0x000CA9F7 File Offset: 0x000C8BF7
		public StatesInstance(DevHEPSpawner smi) : base(smi)
		{
		}
	}

	// Token: 0x02000D2E RID: 3374
	public class States : GameStateMachine<DevHEPSpawner.States, DevHEPSpawner.StatesInstance, DevHEPSpawner>
	{
		// Token: 0x060041F5 RID: 16885 RVA: 0x0023FCC4 File Offset: 0x0023DEC4
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.inoperational;
			this.inoperational.PlayAnim("off").TagTransition(GameTags.Operational, this.ready, false);
			this.ready.PlayAnim("on").TagTransition(GameTags.Operational, this.inoperational, true).Update(delegate(DevHEPSpawner.StatesInstance smi, float dt)
			{
				smi.master.LauncherUpdate(dt);
			}, UpdateRate.SIM_EVERY_TICK, false);
		}

		// Token: 0x04002CFD RID: 11517
		public StateMachine<DevHEPSpawner.States, DevHEPSpawner.StatesInstance, DevHEPSpawner, object>.BoolParameter isAbsorbingRadiation;

		// Token: 0x04002CFE RID: 11518
		public GameStateMachine<DevHEPSpawner.States, DevHEPSpawner.StatesInstance, DevHEPSpawner, object>.State ready;

		// Token: 0x04002CFF RID: 11519
		public GameStateMachine<DevHEPSpawner.States, DevHEPSpawner.StatesInstance, DevHEPSpawner, object>.State inoperational;
	}
}
