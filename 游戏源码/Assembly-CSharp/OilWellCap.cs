using System;
using Klei;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000EF2 RID: 3826
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/Workable/OilWellCap")]
public class OilWellCap : Workable, ISingleSliderControl, ISliderControl, IElementEmitter
{
	// Token: 0x17000444 RID: 1092
	// (get) Token: 0x06004D29 RID: 19753 RVA: 0x000D20D4 File Offset: 0x000D02D4
	public SimHashes Element
	{
		get
		{
			return this.gasElement;
		}
	}

	// Token: 0x17000445 RID: 1093
	// (get) Token: 0x06004D2A RID: 19754 RVA: 0x000D20DC File Offset: 0x000D02DC
	public float AverageEmitRate
	{
		get
		{
			return Game.Instance.accumulators.GetAverageRate(this.accumulator);
		}
	}

	// Token: 0x17000446 RID: 1094
	// (get) Token: 0x06004D2B RID: 19755 RVA: 0x000D20F3 File Offset: 0x000D02F3
	public string SliderTitleKey
	{
		get
		{
			return "STRINGS.UI.UISIDESCREENS.OIL_WELL_CAP_SIDE_SCREEN.TITLE";
		}
	}

	// Token: 0x17000447 RID: 1095
	// (get) Token: 0x06004D2C RID: 19756 RVA: 0x000CABAC File Offset: 0x000C8DAC
	public string SliderUnits
	{
		get
		{
			return UI.UNITSUFFIXES.PERCENT;
		}
	}

	// Token: 0x06004D2D RID: 19757 RVA: 0x000AD2F7 File Offset: 0x000AB4F7
	public int SliderDecimalPlaces(int index)
	{
		return 0;
	}

	// Token: 0x06004D2E RID: 19758 RVA: 0x000BCEBF File Offset: 0x000BB0BF
	public float GetSliderMin(int index)
	{
		return 0f;
	}

	// Token: 0x06004D2F RID: 19759 RVA: 0x000C8A64 File Offset: 0x000C6C64
	public float GetSliderMax(int index)
	{
		return 100f;
	}

	// Token: 0x06004D30 RID: 19760 RVA: 0x000D20FA File Offset: 0x000D02FA
	public float GetSliderValue(int index)
	{
		return this.depressurizePercent * 100f;
	}

	// Token: 0x06004D31 RID: 19761 RVA: 0x000D2108 File Offset: 0x000D0308
	public void SetSliderValue(float value, int index)
	{
		this.depressurizePercent = value / 100f;
	}

	// Token: 0x06004D32 RID: 19762 RVA: 0x000D2117 File Offset: 0x000D0317
	public string GetSliderTooltipKey(int index)
	{
		return "STRINGS.UI.UISIDESCREENS.OIL_WELL_CAP_SIDE_SCREEN.TOOLTIP";
	}

	// Token: 0x06004D33 RID: 19763 RVA: 0x000D211E File Offset: 0x000D031E
	string ISliderControl.GetSliderTooltip(int index)
	{
		return string.Format(Strings.Get("STRINGS.UI.UISIDESCREENS.OIL_WELL_CAP_SIDE_SCREEN.TOOLTIP"), this.depressurizePercent * 100f);
	}

	// Token: 0x06004D34 RID: 19764 RVA: 0x000D2145 File Offset: 0x000D0345
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<OilWellCap>(-905833192, OilWellCap.OnCopySettingsDelegate);
	}

	// Token: 0x06004D35 RID: 19765 RVA: 0x00264650 File Offset: 0x00262850
	private void OnCopySettings(object data)
	{
		OilWellCap component = ((GameObject)data).GetComponent<OilWellCap>();
		if (component != null)
		{
			this.depressurizePercent = component.depressurizePercent;
		}
	}

	// Token: 0x06004D36 RID: 19766 RVA: 0x00264680 File Offset: 0x00262880
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Prioritizable.AddRef(base.gameObject);
		this.accumulator = Game.Instance.accumulators.Add("pressuregas", this);
		this.showProgressBar = false;
		base.SetWorkTime(float.PositiveInfinity);
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_oil_cap_kanim")
		};
		this.workingStatusItem = Db.Get().BuildingStatusItems.ReleasingPressure;
		this.attributeConverter = Db.Get().AttributeConverters.MachinerySpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Technicals.Id;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		this.pressureMeter = new MeterController(component, "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new Vector3(0f, 0f, 0f), null);
		this.smi = new OilWellCap.StatesInstance(this);
		this.smi.StartSM();
		this.UpdatePressurePercent();
	}

	// Token: 0x06004D37 RID: 19767 RVA: 0x000D215E File Offset: 0x000D035E
	protected override void OnCleanUp()
	{
		Game.Instance.accumulators.Remove(this.accumulator);
		Prioritizable.RemoveRef(base.gameObject);
		base.OnCleanUp();
	}

	// Token: 0x06004D38 RID: 19768 RVA: 0x000D2187 File Offset: 0x000D0387
	public void AddGasPressure(float dt)
	{
		this.storage.AddGasChunk(this.gasElement, this.addGasRate * dt, this.gasTemperature, 0, 0, true, true);
		this.UpdatePressurePercent();
	}

	// Token: 0x06004D39 RID: 19769 RVA: 0x00264798 File Offset: 0x00262998
	public void ReleaseGasPressure(float dt)
	{
		PrimaryElement primaryElement = this.storage.FindPrimaryElement(this.gasElement);
		if (primaryElement != null && primaryElement.Mass > 0f)
		{
			float num = this.releaseGasRate * dt;
			if (base.worker != null)
			{
				num *= this.GetEfficiencyMultiplier(base.worker);
			}
			num = Mathf.Min(num, primaryElement.Mass);
			SimUtil.DiseaseInfo percentOfDisease = SimUtil.GetPercentOfDisease(primaryElement, num / primaryElement.Mass);
			primaryElement.Mass -= num;
			Game.Instance.accumulators.Accumulate(this.accumulator, num);
			SimMessages.AddRemoveSubstance(Grid.PosToCell(this), ElementLoader.GetElementIndex(this.gasElement), null, num, primaryElement.Temperature, percentOfDisease.idx, percentOfDisease.count, true, -1);
		}
		this.UpdatePressurePercent();
	}

	// Token: 0x06004D3A RID: 19770 RVA: 0x0026486C File Offset: 0x00262A6C
	private void UpdatePressurePercent()
	{
		float num = this.storage.GetMassAvailable(this.gasElement) / this.maxGasPressure;
		num = Mathf.Clamp01(num);
		this.smi.sm.pressurePercent.Set(num, this.smi, false);
		this.pressureMeter.SetPositionPercent(num);
	}

	// Token: 0x06004D3B RID: 19771 RVA: 0x000D21B3 File Offset: 0x000D03B3
	public bool NeedsDepressurizing()
	{
		return this.smi.GetPressurePercent() >= this.depressurizePercent;
	}

	// Token: 0x06004D3C RID: 19772 RVA: 0x002648C4 File Offset: 0x00262AC4
	private WorkChore<OilWellCap> CreateWorkChore()
	{
		this.DepressurizeChore = new WorkChore<OilWellCap>(Db.Get().ChoreTypes.Depressurize, this, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
		this.DepressurizeChore.AddPrecondition(OilWellCap.AllowedToDepressurize, this);
		return this.DepressurizeChore;
	}

	// Token: 0x06004D3D RID: 19773 RVA: 0x000D21CB File Offset: 0x000D03CB
	private void CancelChore(string reason)
	{
		if (this.DepressurizeChore != null)
		{
			this.DepressurizeChore.Cancel(reason);
		}
	}

	// Token: 0x06004D3E RID: 19774 RVA: 0x000D21E1 File Offset: 0x000D03E1
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		this.smi.sm.working.Set(true, this.smi, false);
	}

	// Token: 0x06004D3F RID: 19775 RVA: 0x000D2208 File Offset: 0x000D0408
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
		this.smi.sm.working.Set(false, this.smi, false);
		this.DepressurizeChore = null;
	}

	// Token: 0x06004D40 RID: 19776 RVA: 0x000D2236 File Offset: 0x000D0436
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		return this.smi.GetPressurePercent() <= 0f;
	}

	// Token: 0x06004D41 RID: 19777 RVA: 0x000D224D File Offset: 0x000D044D
	public override bool InstantlyFinish(WorkerBase worker)
	{
		this.ReleaseGasPressure(60f);
		return true;
	}

	// Token: 0x040035A2 RID: 13730
	private OilWellCap.StatesInstance smi;

	// Token: 0x040035A3 RID: 13731
	[MyCmpReq]
	private Operational operational;

	// Token: 0x040035A4 RID: 13732
	[MyCmpReq]
	private Storage storage;

	// Token: 0x040035A5 RID: 13733
	public SimHashes gasElement;

	// Token: 0x040035A6 RID: 13734
	public float gasTemperature;

	// Token: 0x040035A7 RID: 13735
	public float addGasRate = 1f;

	// Token: 0x040035A8 RID: 13736
	public float maxGasPressure = 10f;

	// Token: 0x040035A9 RID: 13737
	public float releaseGasRate = 10f;

	// Token: 0x040035AA RID: 13738
	[Serialize]
	private float depressurizePercent = 0.75f;

	// Token: 0x040035AB RID: 13739
	private HandleVector<int>.Handle accumulator = HandleVector<int>.InvalidHandle;

	// Token: 0x040035AC RID: 13740
	private MeterController pressureMeter;

	// Token: 0x040035AD RID: 13741
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x040035AE RID: 13742
	private static readonly EventSystem.IntraObjectHandler<OilWellCap> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<OilWellCap>(delegate(OilWellCap component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x040035AF RID: 13743
	private WorkChore<OilWellCap> DepressurizeChore;

	// Token: 0x040035B0 RID: 13744
	private static readonly Chore.Precondition AllowedToDepressurize = new Chore.Precondition
	{
		id = "AllowedToDepressurize",
		description = DUPLICANTS.CHORES.PRECONDITIONS.ALLOWED_TO_DEPRESSURIZE,
		fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			return ((OilWellCap)data).NeedsDepressurizing();
		}
	};

	// Token: 0x02000EF3 RID: 3827
	public class StatesInstance : GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.GameInstance
	{
		// Token: 0x06004D44 RID: 19780 RVA: 0x000D229A File Offset: 0x000D049A
		public StatesInstance(OilWellCap master) : base(master)
		{
		}

		// Token: 0x06004D45 RID: 19781 RVA: 0x000D22A3 File Offset: 0x000D04A3
		public float GetPressurePercent()
		{
			return base.sm.pressurePercent.Get(base.smi);
		}
	}

	// Token: 0x02000EF4 RID: 3828
	public class States : GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap>
	{
		// Token: 0x06004D46 RID: 19782 RVA: 0x00264980 File Offset: 0x00262B80
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.inoperational;
			this.inoperational.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.operational, new StateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.Transition.ConditionCallback(this.IsOperational));
			this.operational.DefaultState(this.operational.idle).ToggleRecurringChore((OilWellCap.StatesInstance smi) => smi.master.CreateWorkChore(), null).EventHandler(GameHashes.WorkChoreDisabled, delegate(OilWellCap.StatesInstance smi)
			{
				smi.master.CancelChore("WorkChoreDisabled");
			});
			this.operational.idle.PlayAnim("off").ToggleStatusItem(Db.Get().BuildingStatusItems.WellPressurizing, null).ParamTransition<float>(this.pressurePercent, this.operational.overpressure, GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.IsGTEOne).ParamTransition<bool>(this.working, this.operational.releasing_pressure, GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.IsTrue).EventTransition(GameHashes.OperationalChanged, this.inoperational, GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.Not(new StateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.Transition.ConditionCallback(this.IsOperational))).EventTransition(GameHashes.OnStorageChange, this.operational.active, new StateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.Transition.ConditionCallback(this.IsAbleToPump));
			this.operational.active.DefaultState(this.operational.active.pre).ToggleStatusItem(Db.Get().BuildingStatusItems.WellPressurizing, null).Enter(delegate(OilWellCap.StatesInstance smi)
			{
				smi.master.operational.SetActive(true, false);
			}).Exit(delegate(OilWellCap.StatesInstance smi)
			{
				smi.master.operational.SetActive(false, false);
			}).Update(delegate(OilWellCap.StatesInstance smi, float dt)
			{
				smi.master.AddGasPressure(dt);
			}, UpdateRate.SIM_200ms, false);
			this.operational.active.pre.PlayAnim("working_pre").ParamTransition<float>(this.pressurePercent, this.operational.overpressure, GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.IsGTEOne).ParamTransition<bool>(this.working, this.operational.releasing_pressure, GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.IsTrue).OnAnimQueueComplete(this.operational.active.loop);
			this.operational.active.loop.PlayAnim("working_loop", KAnim.PlayMode.Loop).ParamTransition<float>(this.pressurePercent, this.operational.active.pst, GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.IsGTEOne).ParamTransition<bool>(this.working, this.operational.active.pst, GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.IsTrue).EventTransition(GameHashes.OperationalChanged, this.operational.active.pst, new StateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.Transition.ConditionCallback(this.MustStopPumping)).EventTransition(GameHashes.OnStorageChange, this.operational.active.pst, new StateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.Transition.ConditionCallback(this.MustStopPumping));
			this.operational.active.pst.PlayAnim("working_pst").OnAnimQueueComplete(this.operational.idle);
			this.operational.overpressure.PlayAnim("over_pressured_pre", KAnim.PlayMode.Once).QueueAnim("over_pressured_loop", true, null).ToggleStatusItem(Db.Get().BuildingStatusItems.WellOverpressure, null).ParamTransition<float>(this.pressurePercent, this.operational.idle, (OilWellCap.StatesInstance smi, float p) => p <= 0f).ParamTransition<bool>(this.working, this.operational.releasing_pressure, GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.IsTrue);
			this.operational.releasing_pressure.DefaultState(this.operational.releasing_pressure.pre).ToggleStatusItem(Db.Get().BuildingStatusItems.EmittingElement, (OilWellCap.StatesInstance smi) => smi.master);
			this.operational.releasing_pressure.pre.PlayAnim("steam_out_pre").OnAnimQueueComplete(this.operational.releasing_pressure.loop);
			this.operational.releasing_pressure.loop.PlayAnim("steam_out_loop", KAnim.PlayMode.Loop).EventTransition(GameHashes.OperationalChanged, this.operational.releasing_pressure.pst, GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.Not(new StateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.Transition.ConditionCallback(this.IsOperational))).ParamTransition<bool>(this.working, this.operational.releasing_pressure.pst, GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.IsFalse).Update(delegate(OilWellCap.StatesInstance smi, float dt)
			{
				smi.master.ReleaseGasPressure(dt);
			}, UpdateRate.SIM_200ms, false);
			this.operational.releasing_pressure.pst.PlayAnim("steam_out_pst").OnAnimQueueComplete(this.operational.idle);
		}

		// Token: 0x06004D47 RID: 19783 RVA: 0x000D22BB File Offset: 0x000D04BB
		private bool IsOperational(OilWellCap.StatesInstance smi)
		{
			return smi.master.operational.IsOperational;
		}

		// Token: 0x06004D48 RID: 19784 RVA: 0x000D22CD File Offset: 0x000D04CD
		private bool IsAbleToPump(OilWellCap.StatesInstance smi)
		{
			return smi.master.operational.IsOperational && smi.GetComponent<ElementConverter>().HasEnoughMassToStartConverting(false);
		}

		// Token: 0x06004D49 RID: 19785 RVA: 0x000D22EF File Offset: 0x000D04EF
		private bool MustStopPumping(OilWellCap.StatesInstance smi)
		{
			return !smi.master.operational.IsOperational || !smi.GetComponent<ElementConverter>().CanConvertAtAll();
		}

		// Token: 0x040035B1 RID: 13745
		public StateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.FloatParameter pressurePercent;

		// Token: 0x040035B2 RID: 13746
		public StateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.BoolParameter working;

		// Token: 0x040035B3 RID: 13747
		public GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.State inoperational;

		// Token: 0x040035B4 RID: 13748
		public OilWellCap.States.OperationalStates operational;

		// Token: 0x02000EF5 RID: 3829
		public class OperationalStates : GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.State
		{
			// Token: 0x040035B5 RID: 13749
			public GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.State idle;

			// Token: 0x040035B6 RID: 13750
			public GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.PreLoopPostState active;

			// Token: 0x040035B7 RID: 13751
			public GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.State overpressure;

			// Token: 0x040035B8 RID: 13752
			public GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.PreLoopPostState releasing_pressure;
		}
	}
}
