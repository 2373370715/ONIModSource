﻿using System;
using Klei.AI;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/Workable/ManualGenerator")]
public class ManualGenerator : RemoteWorkable, ISingleSliderControl, ISliderControl
{
			public string SliderTitleKey
	{
		get
		{
			return "STRINGS.UI.UISIDESCREENS.MANUALGENERATORSIDESCREEN.TITLE";
		}
	}

			public string SliderUnits
	{
		get
		{
			return UI.UNITSUFFIXES.PERCENT;
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
		return 100f;
	}

		public float GetSliderValue(int index)
	{
		return this.batteryRefillPercent * 100f;
	}

		public void SetSliderValue(float value, int index)
	{
		this.batteryRefillPercent = value / 100f;
	}

		public string GetSliderTooltipKey(int index)
	{
		return "STRINGS.UI.UISIDESCREENS.MANUALGENERATORSIDESCREEN.TOOLTIP";
	}

		string ISliderControl.GetSliderTooltip(int index)
	{
		return string.Format(Strings.Get("STRINGS.UI.UISIDESCREENS.MANUALGENERATORSIDESCREEN.TOOLTIP"), this.batteryRefillPercent * 100f);
	}

			public bool IsPowered
	{
		get
		{
			return this.operational.IsActive;
		}
	}

			public override Chore RemoteDockChore
	{
		get
		{
			return this.chore;
		}
	}

		private ManualGenerator()
	{
		this.showProgressBar = false;
	}

		protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<ManualGenerator>(-592767678, ManualGenerator.OnOperationalChangedDelegate);
		base.Subscribe<ManualGenerator>(824508782, ManualGenerator.OnActiveChangedDelegate);
		base.Subscribe<ManualGenerator>(-905833192, ManualGenerator.OnCopySettingsDelegate);
		this.workerStatusItem = Db.Get().DuplicantStatusItems.GeneratingPower;
		this.attributeConverter = Db.Get().AttributeConverters.MachinerySpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Technicals.Id;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		EnergyGenerator.EnsureStatusItemAvailable();
	}

		protected override void OnSpawn()
	{
		base.OnSpawn();
		base.SetWorkTime(float.PositiveInfinity);
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		foreach (KAnimHashedString symbol in ManualGenerator.symbol_names)
		{
			component.SetSymbolVisiblity(symbol, false);
		}
		Building component2 = base.GetComponent<Building>();
		this.powerCell = component2.GetPowerOutputCell();
		this.OnActiveChanged(null);
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_generatormanual_kanim")
		};
		this.smi = new ManualGenerator.GeneratePowerSM.Instance(this);
		this.smi.StartSM();
		Game.Instance.energySim.AddManualGenerator(this);
	}

		protected override void OnCleanUp()
	{
		Game.Instance.energySim.RemoveManualGenerator(this);
		this.smi.StopSM("cleanup");
		base.OnCleanUp();
	}

		protected void OnActiveChanged(object is_active)
	{
		if (this.operational.IsActive)
		{
			base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, Db.Get().BuildingStatusItems.ManualGeneratorChargingUp, null);
		}
	}

		private void OnCopySettings(object data)
	{
		GameObject gameObject = data as GameObject;
		if (gameObject != null)
		{
			ManualGenerator component = gameObject.GetComponent<ManualGenerator>();
			if (component != null)
			{
				this.batteryRefillPercent = component.batteryRefillPercent;
			}
		}
	}

		public void EnergySim200ms(float dt)
	{
		KSelectable component = base.GetComponent<KSelectable>();
		if (this.operational.IsActive)
		{
			this.generator.GenerateJoules(this.generator.WattageRating * dt, false);
			component.SetStatusItem(Db.Get().StatusItemCategories.Power, Db.Get().BuildingStatusItems.Wattage, this.generator);
			return;
		}
		this.generator.ResetJoules();
		component.SetStatusItem(Db.Get().StatusItemCategories.Power, Db.Get().BuildingStatusItems.GeneratorOffline, null);
		if (this.operational.IsOperational)
		{
			CircuitManager circuitManager = Game.Instance.circuitManager;
			if (circuitManager == null)
			{
				return;
			}
			ushort circuitID = circuitManager.GetCircuitID(this.generator);
			bool flag = circuitManager.HasBatteries(circuitID);
			bool flag2 = false;
			if (!flag && circuitManager.HasConsumers(circuitID))
			{
				flag2 = true;
			}
			else if (flag)
			{
				if (this.batteryRefillPercent <= 0f && circuitManager.GetMinBatteryPercentFullOnCircuit(circuitID) <= 0f)
				{
					flag2 = true;
				}
				else if (circuitManager.GetMinBatteryPercentFullOnCircuit(circuitID) < this.batteryRefillPercent)
				{
					flag2 = true;
				}
			}
			if (flag2)
			{
				if (this.chore == null && this.smi.GetCurrentState() == this.smi.sm.on)
				{
					this.chore = new WorkChore<ManualGenerator>(Db.Get().ChoreTypes.GeneratePower, this, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
				}
			}
			else if (this.chore != null)
			{
				this.chore.Cancel("No refill needed");
				this.chore = null;
			}
			component.ToggleStatusItem(EnergyGenerator.BatteriesSufficientlyFull, !flag2, null);
		}
	}

		protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		this.operational.SetActive(true, false);
	}

		protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		CircuitManager circuitManager = Game.Instance.circuitManager;
		bool flag = false;
		if (circuitManager != null)
		{
			ushort circuitID = circuitManager.GetCircuitID(this.generator);
			bool flag2 = circuitManager.HasBatteries(circuitID);
			flag = ((flag2 && circuitManager.GetMinBatteryPercentFullOnCircuit(circuitID) < 1f) || (!flag2 && circuitManager.HasConsumers(circuitID)));
		}
		AttributeLevels component = worker.GetComponent<AttributeLevels>();
		if (component != null)
		{
			component.AddExperience(Db.Get().Attributes.Athletics.Id, dt, DUPLICANTSTATS.ATTRIBUTE_LEVELING.ALL_DAY_EXPERIENCE);
		}
		return !flag;
	}

		protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
		this.operational.SetActive(false, false);
	}

		protected override void OnCompleteWork(WorkerBase worker)
	{
		this.operational.SetActive(false, false);
		if (this.chore != null)
		{
			this.chore.Cancel("complete");
			this.chore = null;
		}
	}

		public override bool InstantlyFinish(WorkerBase worker)
	{
		return false;
	}

		private void OnOperationalChanged(object data)
	{
		if (!this.buildingEnabledButton.IsEnabled)
		{
			this.generator.ResetJoules();
		}
	}

		[Serialize]
	[SerializeField]
	private float batteryRefillPercent = 0.5f;

		private const float batteryStopRunningPercent = 1f;

		[MyCmpReq]
	private Generator generator;

		[MyCmpReq]
	private Operational operational;

		[MyCmpGet]
	private BuildingEnabledButton buildingEnabledButton;

		[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

		private Chore chore;

		private int powerCell;

		private ManualGenerator.GeneratePowerSM.Instance smi;

		private static readonly KAnimHashedString[] symbol_names = new KAnimHashedString[]
	{
		"meter",
		"meter_target",
		"meter_fill",
		"meter_frame",
		"meter_light",
		"meter_tubing"
	};

		private static readonly EventSystem.IntraObjectHandler<ManualGenerator> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<ManualGenerator>(delegate(ManualGenerator component, object data)
	{
		component.OnOperationalChanged(data);
	});

		private static readonly EventSystem.IntraObjectHandler<ManualGenerator> OnActiveChangedDelegate = new EventSystem.IntraObjectHandler<ManualGenerator>(delegate(ManualGenerator component, object data)
	{
		component.OnActiveChanged(data);
	});

		private static readonly EventSystem.IntraObjectHandler<ManualGenerator> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<ManualGenerator>(delegate(ManualGenerator component, object data)
	{
		component.OnCopySettings(data);
	});

		public class GeneratePowerSM : GameStateMachine<ManualGenerator.GeneratePowerSM, ManualGenerator.GeneratePowerSM.Instance>
	{
				public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.off;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.off.EventTransition(GameHashes.OperationalChanged, this.on, (ManualGenerator.GeneratePowerSM.Instance smi) => smi.master.GetComponent<Operational>().IsOperational).PlayAnim("off");
			this.on.EventTransition(GameHashes.OperationalChanged, this.off, (ManualGenerator.GeneratePowerSM.Instance smi) => !smi.master.GetComponent<Operational>().IsOperational).EventTransition(GameHashes.ActiveChanged, this.working.pre, (ManualGenerator.GeneratePowerSM.Instance smi) => smi.master.GetComponent<Operational>().IsActive).PlayAnim("on");
			this.working.DefaultState(this.working.pre);
			this.working.pre.PlayAnim("working_pre").OnAnimQueueComplete(this.working.loop);
			this.working.loop.PlayAnim("working_loop", KAnim.PlayMode.Loop).EventTransition(GameHashes.ActiveChanged, this.off, (ManualGenerator.GeneratePowerSM.Instance smi) => this.masterTarget.Get(smi) != null && !smi.master.GetComponent<Operational>().IsActive);
		}

				public GameStateMachine<ManualGenerator.GeneratePowerSM, ManualGenerator.GeneratePowerSM.Instance, IStateMachineTarget, object>.State off;

				public GameStateMachine<ManualGenerator.GeneratePowerSM, ManualGenerator.GeneratePowerSM.Instance, IStateMachineTarget, object>.State on;

				public ManualGenerator.GeneratePowerSM.WorkingStates working;

				public class WorkingStates : GameStateMachine<ManualGenerator.GeneratePowerSM, ManualGenerator.GeneratePowerSM.Instance, IStateMachineTarget, object>.State
		{
						public GameStateMachine<ManualGenerator.GeneratePowerSM, ManualGenerator.GeneratePowerSM.Instance, IStateMachineTarget, object>.State pre;

						public GameStateMachine<ManualGenerator.GeneratePowerSM, ManualGenerator.GeneratePowerSM.Instance, IStateMachineTarget, object>.State loop;

						public GameStateMachine<ManualGenerator.GeneratePowerSM, ManualGenerator.GeneratePowerSM.Instance, IStateMachineTarget, object>.State pst;
		}

				public new class Instance : GameStateMachine<ManualGenerator.GeneratePowerSM, ManualGenerator.GeneratePowerSM.Instance, IStateMachineTarget, object>.GameInstance
		{
						public Instance(IStateMachineTarget master) : base(master)
			{
			}
		}
	}
}
