using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000DF3 RID: 3571
[SerializationConfig(MemberSerialization.OptIn)]
public class HighEnergyParticleSpawner : StateMachineComponent<HighEnergyParticleSpawner.StatesInstance>, IHighEnergyParticleDirection, IProgressBarSideScreen, ISingleSliderControl, ISliderControl
{
	// Token: 0x17000363 RID: 867
	// (get) Token: 0x06004636 RID: 17974 RVA: 0x000CD7A5 File Offset: 0x000CB9A5
	public float PredictedPerCycleConsumptionRate
	{
		get
		{
			return (float)Mathf.FloorToInt(this.recentPerSecondConsumptionRate * 0.1f * 600f);
		}
	}

	// Token: 0x17000364 RID: 868
	// (get) Token: 0x06004637 RID: 17975 RVA: 0x000CD7BF File Offset: 0x000CB9BF
	// (set) Token: 0x06004638 RID: 17976 RVA: 0x0024E408 File Offset: 0x0024C608
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

	// Token: 0x06004639 RID: 17977 RVA: 0x0024E460 File Offset: 0x0024C660
	private void OnCopySettings(object data)
	{
		HighEnergyParticleSpawner component = ((GameObject)data).GetComponent<HighEnergyParticleSpawner>();
		if (component != null)
		{
			this.Direction = component.Direction;
			this.particleThreshold = component.particleThreshold;
		}
	}

	// Token: 0x0600463A RID: 17978 RVA: 0x000CD7C7 File Offset: 0x000CB9C7
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<HighEnergyParticleSpawner>(-905833192, HighEnergyParticleSpawner.OnCopySettingsDelegate);
	}

	// Token: 0x0600463B RID: 17979 RVA: 0x0024E49C File Offset: 0x0024C69C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
		this.directionController = new EightDirectionController(base.GetComponent<KBatchedAnimController>(), "redirector_target", "redirect", EightDirectionController.Offset.Infront);
		this.Direction = this.Direction;
		this.particleController = new MeterController(base.GetComponent<KBatchedAnimController>(), "orb_target", "orb_off", Meter.Offset.NoChange, Grid.SceneLayer.NoLayer, Array.Empty<string>());
		this.particleController.gameObject.AddOrGet<LoopingSounds>();
		this.progressMeterController = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Array.Empty<string>());
		Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Radiation, true);
	}

	// Token: 0x0600463C RID: 17980 RVA: 0x000CD7E0 File Offset: 0x000CB9E0
	public float GetProgressBarMaxValue()
	{
		return this.particleThreshold;
	}

	// Token: 0x0600463D RID: 17981 RVA: 0x000CD7E8 File Offset: 0x000CB9E8
	public float GetProgressBarFillPercentage()
	{
		return this.particleStorage.Particles / this.particleThreshold;
	}

	// Token: 0x0600463E RID: 17982 RVA: 0x000CD7FC File Offset: 0x000CB9FC
	public string GetProgressBarTitleLabel()
	{
		return UI.UISIDESCREENS.RADBOLTTHRESHOLDSIDESCREEN.PROGRESS_BAR_LABEL;
	}

	// Token: 0x0600463F RID: 17983 RVA: 0x0024E548 File Offset: 0x0024C748
	public string GetProgressBarLabel()
	{
		return Mathf.FloorToInt(this.particleStorage.Particles).ToString() + "/" + Mathf.FloorToInt(this.particleThreshold).ToString();
	}

	// Token: 0x06004640 RID: 17984 RVA: 0x000CD808 File Offset: 0x000CBA08
	public string GetProgressBarTooltip()
	{
		return UI.UISIDESCREENS.RADBOLTTHRESHOLDSIDESCREEN.PROGRESS_BAR_TOOLTIP;
	}

	// Token: 0x06004641 RID: 17985 RVA: 0x000CD814 File Offset: 0x000CBA14
	public void DoConsumeParticlesWhileDisabled(float dt)
	{
		this.particleStorage.ConsumeAndGet(dt * 1f);
		this.progressMeterController.SetPositionPercent(this.GetProgressBarFillPercentage());
	}

	// Token: 0x06004642 RID: 17986 RVA: 0x0024E58C File Offset: 0x0024C78C
	public void LauncherUpdate(float dt)
	{
		this.radiationSampleTimer += dt;
		if (this.radiationSampleTimer >= this.radiationSampleRate)
		{
			this.radiationSampleTimer -= this.radiationSampleRate;
			int i = Grid.PosToCell(this);
			float num = Grid.Radiation[i];
			if (num != 0f && this.particleStorage.RemainingCapacity() > 0f)
			{
				base.smi.sm.isAbsorbingRadiation.Set(true, base.smi, false);
				this.recentPerSecondConsumptionRate = num / 600f;
				this.particleStorage.Store(this.recentPerSecondConsumptionRate * this.radiationSampleRate * 0.1f);
			}
			else
			{
				this.recentPerSecondConsumptionRate = 0f;
				base.smi.sm.isAbsorbingRadiation.Set(false, base.smi, false);
			}
		}
		this.progressMeterController.SetPositionPercent(this.GetProgressBarFillPercentage());
		if (!this.particleVisualPlaying && this.particleStorage.Particles > this.particleThreshold / 2f)
		{
			this.particleController.meterController.Play("orb_pre", KAnim.PlayMode.Once, 1f, 0f);
			this.particleController.meterController.Queue("orb_idle", KAnim.PlayMode.Loop, 1f, 0f);
			this.particleVisualPlaying = true;
		}
		this.launcherTimer += dt;
		if (this.launcherTimer < this.minLaunchInterval)
		{
			return;
		}
		if (this.particleStorage.Particles >= this.particleThreshold)
		{
			this.launcherTimer = 0f;
			int highEnergyParticleOutputCell = base.GetComponent<Building>().GetHighEnergyParticleOutputCell();
			GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab("HighEnergyParticle"), Grid.CellToPosCCC(highEnergyParticleOutputCell, Grid.SceneLayer.FXFront2), Grid.SceneLayer.FXFront2, null, 0);
			gameObject.SetActive(true);
			if (gameObject != null)
			{
				HighEnergyParticle component = gameObject.GetComponent<HighEnergyParticle>();
				component.payload = this.particleStorage.ConsumeAndGet(this.particleThreshold);
				component.SetDirection(this.Direction);
				this.directionController.PlayAnim("redirect_send", KAnim.PlayMode.Once);
				this.directionController.controller.Queue("redirect", KAnim.PlayMode.Once, 1f, 0f);
				this.particleController.meterController.Play("orb_send", KAnim.PlayMode.Once, 1f, 0f);
				this.particleController.meterController.Queue("orb_off", KAnim.PlayMode.Once, 1f, 0f);
				this.particleVisualPlaying = false;
			}
		}
	}

	// Token: 0x17000365 RID: 869
	// (get) Token: 0x06004643 RID: 17987 RVA: 0x000CD83A File Offset: 0x000CBA3A
	public string SliderTitleKey
	{
		get
		{
			return "STRINGS.UI.UISIDESCREENS.RADBOLTTHRESHOLDSIDESCREEN.TITLE";
		}
	}

	// Token: 0x17000366 RID: 870
	// (get) Token: 0x06004644 RID: 17988 RVA: 0x000CA9A4 File Offset: 0x000C8BA4
	public string SliderUnits
	{
		get
		{
			return UI.UNITSUFFIXES.HIGHENERGYPARTICLES.PARTRICLES;
		}
	}

	// Token: 0x06004645 RID: 17989 RVA: 0x000AD2F7 File Offset: 0x000AB4F7
	public int SliderDecimalPlaces(int index)
	{
		return 0;
	}

	// Token: 0x06004646 RID: 17990 RVA: 0x000CD841 File Offset: 0x000CBA41
	public float GetSliderMin(int index)
	{
		return (float)this.minSlider;
	}

	// Token: 0x06004647 RID: 17991 RVA: 0x000CD84A File Offset: 0x000CBA4A
	public float GetSliderMax(int index)
	{
		return (float)this.maxSlider;
	}

	// Token: 0x06004648 RID: 17992 RVA: 0x000CD7E0 File Offset: 0x000CB9E0
	public float GetSliderValue(int index)
	{
		return this.particleThreshold;
	}

	// Token: 0x06004649 RID: 17993 RVA: 0x000CD853 File Offset: 0x000CBA53
	public void SetSliderValue(float value, int index)
	{
		this.particleThreshold = value;
	}

	// Token: 0x0600464A RID: 17994 RVA: 0x000CD85C File Offset: 0x000CBA5C
	public string GetSliderTooltipKey(int index)
	{
		return "STRINGS.UI.UISIDESCREENS.RADBOLTTHRESHOLDSIDESCREEN.TOOLTIP";
	}

	// Token: 0x0600464B RID: 17995 RVA: 0x000CD863 File Offset: 0x000CBA63
	string ISliderControl.GetSliderTooltip(int index)
	{
		return string.Format(Strings.Get("STRINGS.UI.UISIDESCREENS.RADBOLTTHRESHOLDSIDESCREEN.TOOLTIP"), this.particleThreshold);
	}

	// Token: 0x04003086 RID: 12422
	[MyCmpReq]
	private HighEnergyParticleStorage particleStorage;

	// Token: 0x04003087 RID: 12423
	[MyCmpGet]
	private Operational operational;

	// Token: 0x04003088 RID: 12424
	private float recentPerSecondConsumptionRate;

	// Token: 0x04003089 RID: 12425
	public int minSlider;

	// Token: 0x0400308A RID: 12426
	public int maxSlider;

	// Token: 0x0400308B RID: 12427
	[Serialize]
	private EightDirection _direction;

	// Token: 0x0400308C RID: 12428
	public float minLaunchInterval;

	// Token: 0x0400308D RID: 12429
	public float radiationSampleRate;

	// Token: 0x0400308E RID: 12430
	[Serialize]
	public float particleThreshold = 50f;

	// Token: 0x0400308F RID: 12431
	private EightDirectionController directionController;

	// Token: 0x04003090 RID: 12432
	private float launcherTimer;

	// Token: 0x04003091 RID: 12433
	private float radiationSampleTimer;

	// Token: 0x04003092 RID: 12434
	private MeterController particleController;

	// Token: 0x04003093 RID: 12435
	private bool particleVisualPlaying;

	// Token: 0x04003094 RID: 12436
	private MeterController progressMeterController;

	// Token: 0x04003095 RID: 12437
	[Serialize]
	public Ref<HighEnergyParticlePort> capturedByRef = new Ref<HighEnergyParticlePort>();

	// Token: 0x04003096 RID: 12438
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04003097 RID: 12439
	private static readonly EventSystem.IntraObjectHandler<HighEnergyParticleSpawner> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<HighEnergyParticleSpawner>(delegate(HighEnergyParticleSpawner component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x02000DF4 RID: 3572
	public class StatesInstance : GameStateMachine<HighEnergyParticleSpawner.States, HighEnergyParticleSpawner.StatesInstance, HighEnergyParticleSpawner, object>.GameInstance
	{
		// Token: 0x0600464E RID: 17998 RVA: 0x000CD8BE File Offset: 0x000CBABE
		public StatesInstance(HighEnergyParticleSpawner smi) : base(smi)
		{
		}
	}

	// Token: 0x02000DF5 RID: 3573
	public class States : GameStateMachine<HighEnergyParticleSpawner.States, HighEnergyParticleSpawner.StatesInstance, HighEnergyParticleSpawner>
	{
		// Token: 0x0600464F RID: 17999 RVA: 0x0024E820 File Offset: 0x0024CA20
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.inoperational;
			this.inoperational.PlayAnim("off").TagTransition(GameTags.Operational, this.ready, false).DefaultState(this.inoperational.empty);
			this.inoperational.empty.EventTransition(GameHashes.OnParticleStorageChanged, this.inoperational.losing, (HighEnergyParticleSpawner.StatesInstance smi) => !smi.GetComponent<HighEnergyParticleStorage>().IsEmpty());
			this.inoperational.losing.ToggleStatusItem(Db.Get().BuildingStatusItems.LosingRadbolts, null).Update(delegate(HighEnergyParticleSpawner.StatesInstance smi, float dt)
			{
				smi.master.DoConsumeParticlesWhileDisabled(dt);
			}, UpdateRate.SIM_1000ms, false).EventTransition(GameHashes.OnParticleStorageChanged, this.inoperational.empty, (HighEnergyParticleSpawner.StatesInstance smi) => smi.GetComponent<HighEnergyParticleStorage>().IsEmpty());
			this.ready.TagTransition(GameTags.Operational, this.inoperational, true).DefaultState(this.ready.idle).Update(delegate(HighEnergyParticleSpawner.StatesInstance smi, float dt)
			{
				smi.master.LauncherUpdate(dt);
			}, UpdateRate.SIM_EVERY_TICK, false);
			this.ready.idle.ParamTransition<bool>(this.isAbsorbingRadiation, this.ready.absorbing, GameStateMachine<HighEnergyParticleSpawner.States, HighEnergyParticleSpawner.StatesInstance, HighEnergyParticleSpawner, object>.IsTrue).PlayAnim("on");
			this.ready.absorbing.Enter("SetActive(true)", delegate(HighEnergyParticleSpawner.StatesInstance smi)
			{
				smi.master.operational.SetActive(true, false);
			}).Exit("SetActive(false)", delegate(HighEnergyParticleSpawner.StatesInstance smi)
			{
				smi.master.operational.SetActive(false, false);
			}).ParamTransition<bool>(this.isAbsorbingRadiation, this.ready.idle, GameStateMachine<HighEnergyParticleSpawner.States, HighEnergyParticleSpawner.StatesInstance, HighEnergyParticleSpawner, object>.IsFalse).ToggleStatusItem(Db.Get().BuildingStatusItems.CollectingHEP, (HighEnergyParticleSpawner.StatesInstance smi) => smi.master).PlayAnim("working_loop", KAnim.PlayMode.Loop);
		}

		// Token: 0x04003098 RID: 12440
		public StateMachine<HighEnergyParticleSpawner.States, HighEnergyParticleSpawner.StatesInstance, HighEnergyParticleSpawner, object>.BoolParameter isAbsorbingRadiation;

		// Token: 0x04003099 RID: 12441
		public HighEnergyParticleSpawner.States.ReadyStates ready;

		// Token: 0x0400309A RID: 12442
		public HighEnergyParticleSpawner.States.InoperationalStates inoperational;

		// Token: 0x02000DF6 RID: 3574
		public class InoperationalStates : GameStateMachine<HighEnergyParticleSpawner.States, HighEnergyParticleSpawner.StatesInstance, HighEnergyParticleSpawner, object>.State
		{
			// Token: 0x0400309B RID: 12443
			public GameStateMachine<HighEnergyParticleSpawner.States, HighEnergyParticleSpawner.StatesInstance, HighEnergyParticleSpawner, object>.State empty;

			// Token: 0x0400309C RID: 12444
			public GameStateMachine<HighEnergyParticleSpawner.States, HighEnergyParticleSpawner.StatesInstance, HighEnergyParticleSpawner, object>.State losing;
		}

		// Token: 0x02000DF7 RID: 3575
		public class ReadyStates : GameStateMachine<HighEnergyParticleSpawner.States, HighEnergyParticleSpawner.StatesInstance, HighEnergyParticleSpawner, object>.State
		{
			// Token: 0x0400309D RID: 12445
			public GameStateMachine<HighEnergyParticleSpawner.States, HighEnergyParticleSpawner.StatesInstance, HighEnergyParticleSpawner, object>.State idle;

			// Token: 0x0400309E RID: 12446
			public GameStateMachine<HighEnergyParticleSpawner.States, HighEnergyParticleSpawner.StatesInstance, HighEnergyParticleSpawner, object>.State absorbing;
		}
	}
}
