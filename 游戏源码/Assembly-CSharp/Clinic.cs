using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02001074 RID: 4212
[AddComponentMenu("KMonoBehaviour/Workable/Clinic")]
public class Clinic : Workable, IGameObjectEffectDescriptor, ISingleSliderControl, ISliderControl
{
	// Token: 0x060055FD RID: 22013 RVA: 0x00281098 File Offset: 0x0027F298
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.showProgressBar = false;
		this.assignable.subSlots = new AssignableSlot[]
		{
			Db.Get().AssignableSlots.MedicalBed
		};
		this.assignable.AddAutoassignPrecondition(new Func<MinionAssignablesProxy, bool>(this.CanAutoAssignTo));
		this.assignable.AddAssignPrecondition(new Func<MinionAssignablesProxy, bool>(this.CanManuallyAssignTo));
	}

	// Token: 0x060055FE RID: 22014 RVA: 0x000D81DD File Offset: 0x000D63DD
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Prioritizable.AddRef(base.gameObject);
		Components.Clinics.Add(this);
		base.SetWorkTime(float.PositiveInfinity);
		this.clinicSMI = new Clinic.ClinicSM.Instance(this);
		this.clinicSMI.StartSM();
	}

	// Token: 0x060055FF RID: 22015 RVA: 0x000D821D File Offset: 0x000D641D
	protected override void OnCleanUp()
	{
		Prioritizable.RemoveRef(base.gameObject);
		Components.Clinics.Remove(this);
		base.OnCleanUp();
	}

	// Token: 0x06005600 RID: 22016 RVA: 0x00281104 File Offset: 0x0027F304
	private KAnimFile[] GetAppropriateOverrideAnims(WorkerBase worker)
	{
		KAnimFile[] result = null;
		if (!worker.GetSMI<WoundMonitor.Instance>().ShouldExitInfirmary())
		{
			result = this.workerInjuredAnims;
		}
		else if (this.workerDiseasedAnims != null && this.IsValidEffect(this.diseaseEffect) && worker.GetSMI<SicknessMonitor.Instance>().IsSick())
		{
			result = this.workerDiseasedAnims;
		}
		return result;
	}

	// Token: 0x06005601 RID: 22017 RVA: 0x000D823B File Offset: 0x000D643B
	public override Workable.AnimInfo GetAnim(WorkerBase worker)
	{
		this.overrideAnims = this.GetAppropriateOverrideAnims(worker);
		return base.GetAnim(worker);
	}

	// Token: 0x06005602 RID: 22018 RVA: 0x000D8251 File Offset: 0x000D6451
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		worker.GetComponent<Effects>().Add("Sleep", false);
	}

	// Token: 0x06005603 RID: 22019 RVA: 0x00281154 File Offset: 0x0027F354
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		KAnimFile[] appropriateOverrideAnims = this.GetAppropriateOverrideAnims(worker);
		if (appropriateOverrideAnims == null || appropriateOverrideAnims != this.overrideAnims)
		{
			return true;
		}
		base.OnWorkTick(worker, dt);
		return false;
	}

	// Token: 0x06005604 RID: 22020 RVA: 0x000D826C File Offset: 0x000D646C
	protected override void OnStopWork(WorkerBase worker)
	{
		worker.GetComponent<Effects>().Remove("Sleep");
		base.OnStopWork(worker);
	}

	// Token: 0x06005605 RID: 22021 RVA: 0x00281184 File Offset: 0x0027F384
	protected override void OnCompleteWork(WorkerBase worker)
	{
		this.assignable.Unassign();
		base.OnCompleteWork(worker);
		Effects component = worker.GetComponent<Effects>();
		for (int i = 0; i < Clinic.EffectsRemoved.Length; i++)
		{
			string effect_id = Clinic.EffectsRemoved[i];
			component.Remove(effect_id);
		}
	}

	// Token: 0x06005606 RID: 22022 RVA: 0x000AD2F7 File Offset: 0x000AB4F7
	public override bool InstantlyFinish(WorkerBase worker)
	{
		return false;
	}

	// Token: 0x06005607 RID: 22023 RVA: 0x002811CC File Offset: 0x0027F3CC
	private Chore CreateWorkChore(ChoreType chore_type, bool allow_prioritization, bool allow_in_red_alert, PriorityScreen.PriorityClass priority_class, bool ignore_schedule_block = false)
	{
		return new WorkChore<Clinic>(chore_type, this, null, true, null, null, null, allow_in_red_alert, null, ignore_schedule_block, true, null, false, true, allow_prioritization, priority_class, 5, false, false);
	}

	// Token: 0x06005608 RID: 22024 RVA: 0x002811F8 File Offset: 0x0027F3F8
	private bool CanAutoAssignTo(MinionAssignablesProxy worker)
	{
		bool flag = false;
		MinionIdentity minionIdentity = worker.target as MinionIdentity;
		if (minionIdentity != null)
		{
			if (this.IsValidEffect(this.healthEffect))
			{
				Health component = minionIdentity.GetComponent<Health>();
				if (component != null && component.hitPoints < component.maxHitPoints)
				{
					flag = true;
				}
			}
			if (!flag && this.IsValidEffect(this.diseaseEffect))
			{
				flag = (minionIdentity.GetComponent<MinionModifiers>().sicknesses.Count > 0);
			}
		}
		return flag;
	}

	// Token: 0x06005609 RID: 22025 RVA: 0x00281270 File Offset: 0x0027F470
	private bool CanManuallyAssignTo(MinionAssignablesProxy worker)
	{
		bool result = false;
		MinionIdentity minionIdentity = worker.target as MinionIdentity;
		if (minionIdentity != null)
		{
			result = this.IsHealthBelowThreshold(minionIdentity.gameObject);
		}
		return result;
	}

	// Token: 0x0600560A RID: 22026 RVA: 0x002812A4 File Offset: 0x0027F4A4
	private bool IsHealthBelowThreshold(GameObject minion)
	{
		Health health = (minion != null) ? minion.GetComponent<Health>() : null;
		if (health != null)
		{
			float num = health.hitPoints / health.maxHitPoints;
			if (health != null)
			{
				return num < this.MedicalAttentionMinimum;
			}
		}
		return false;
	}

	// Token: 0x0600560B RID: 22027 RVA: 0x000D8285 File Offset: 0x000D6485
	private bool IsValidEffect(string effect)
	{
		return effect != null && effect != "";
	}

	// Token: 0x0600560C RID: 22028 RVA: 0x000D8297 File Offset: 0x000D6497
	private bool AllowDoctoring()
	{
		return this.IsValidEffect(this.doctoredDiseaseEffect) || this.IsValidEffect(this.doctoredHealthEffect);
	}

	// Token: 0x0600560D RID: 22029 RVA: 0x002812F0 File Offset: 0x0027F4F0
	public override List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> descriptors = base.GetDescriptors(go);
		if (this.IsValidEffect(this.healthEffect))
		{
			Effect.AddModifierDescriptions(base.gameObject, descriptors, this.healthEffect, false);
		}
		if (this.diseaseEffect != this.healthEffect && this.IsValidEffect(this.diseaseEffect))
		{
			Effect.AddModifierDescriptions(base.gameObject, descriptors, this.diseaseEffect, false);
		}
		if (this.AllowDoctoring())
		{
			Descriptor item = default(Descriptor);
			item.SetupDescriptor(UI.BUILDINGEFFECTS.DOCTORING, UI.BUILDINGEFFECTS.TOOLTIPS.DOCTORING, Descriptor.DescriptorType.Effect);
			descriptors.Add(item);
			if (this.IsValidEffect(this.doctoredHealthEffect))
			{
				Effect.AddModifierDescriptions(base.gameObject, descriptors, this.doctoredHealthEffect, true);
			}
			if (this.doctoredDiseaseEffect != this.doctoredHealthEffect && this.IsValidEffect(this.doctoredDiseaseEffect))
			{
				Effect.AddModifierDescriptions(base.gameObject, descriptors, this.doctoredDiseaseEffect, true);
			}
		}
		return descriptors;
	}

	// Token: 0x170004F5 RID: 1269
	// (get) Token: 0x0600560E RID: 22030 RVA: 0x000D82B5 File Offset: 0x000D64B5
	public float MedicalAttentionMinimum
	{
		get
		{
			return this.sicknessSliderValue / 100f;
		}
	}

	// Token: 0x170004F6 RID: 1270
	// (get) Token: 0x0600560F RID: 22031 RVA: 0x000D82C3 File Offset: 0x000D64C3
	string ISliderControl.SliderTitleKey
	{
		get
		{
			return "STRINGS.UI.UISIDESCREENS.MEDICALCOTSIDESCREEN.TITLE";
		}
	}

	// Token: 0x170004F7 RID: 1271
	// (get) Token: 0x06005610 RID: 22032 RVA: 0x000CABAC File Offset: 0x000C8DAC
	string ISliderControl.SliderUnits
	{
		get
		{
			return UI.UNITSUFFIXES.PERCENT;
		}
	}

	// Token: 0x06005611 RID: 22033 RVA: 0x000AD2F7 File Offset: 0x000AB4F7
	int ISliderControl.SliderDecimalPlaces(int index)
	{
		return 0;
	}

	// Token: 0x06005612 RID: 22034 RVA: 0x000BCEBF File Offset: 0x000BB0BF
	float ISliderControl.GetSliderMin(int index)
	{
		return 0f;
	}

	// Token: 0x06005613 RID: 22035 RVA: 0x000C8A64 File Offset: 0x000C6C64
	float ISliderControl.GetSliderMax(int index)
	{
		return 100f;
	}

	// Token: 0x06005614 RID: 22036 RVA: 0x000D82CA File Offset: 0x000D64CA
	float ISliderControl.GetSliderValue(int index)
	{
		return this.sicknessSliderValue;
	}

	// Token: 0x06005615 RID: 22037 RVA: 0x000D82D2 File Offset: 0x000D64D2
	void ISliderControl.SetSliderValue(float percent, int index)
	{
		if (percent != this.sicknessSliderValue)
		{
			this.sicknessSliderValue = (float)Mathf.RoundToInt(percent);
			Game.Instance.Trigger(875045922, null);
		}
	}

	// Token: 0x06005616 RID: 22038 RVA: 0x000D82FA File Offset: 0x000D64FA
	string ISliderControl.GetSliderTooltip(int index)
	{
		return string.Format(Strings.Get("STRINGS.UI.UISIDESCREENS.MEDICALCOTSIDESCREEN.TOOLTIP"), this.sicknessSliderValue);
	}

	// Token: 0x06005617 RID: 22039 RVA: 0x000D831B File Offset: 0x000D651B
	string ISliderControl.GetSliderTooltipKey(int index)
	{
		return "STRINGS.UI.UISIDESCREENS.MEDICALCOTSIDESCREEN.TOOLTIP";
	}

	// Token: 0x04003C59 RID: 15449
	[MyCmpReq]
	private Assignable assignable;

	// Token: 0x04003C5A RID: 15450
	private static readonly string[] EffectsRemoved = new string[]
	{
		"SoreBack"
	};

	// Token: 0x04003C5B RID: 15451
	private const int MAX_RANGE = 10;

	// Token: 0x04003C5C RID: 15452
	private const float CHECK_RANGE_INTERVAL = 10f;

	// Token: 0x04003C5D RID: 15453
	public float doctorVisitInterval = 300f;

	// Token: 0x04003C5E RID: 15454
	public KAnimFile[] workerInjuredAnims;

	// Token: 0x04003C5F RID: 15455
	public KAnimFile[] workerDiseasedAnims;

	// Token: 0x04003C60 RID: 15456
	public string diseaseEffect;

	// Token: 0x04003C61 RID: 15457
	public string healthEffect;

	// Token: 0x04003C62 RID: 15458
	public string doctoredDiseaseEffect;

	// Token: 0x04003C63 RID: 15459
	public string doctoredHealthEffect;

	// Token: 0x04003C64 RID: 15460
	public string doctoredPlaceholderEffect;

	// Token: 0x04003C65 RID: 15461
	private Clinic.ClinicSM.Instance clinicSMI;

	// Token: 0x04003C66 RID: 15462
	public static readonly Chore.Precondition IsOverSicknessThreshold = new Chore.Precondition
	{
		id = "IsOverSicknessThreshold",
		description = DUPLICANTS.CHORES.PRECONDITIONS.IS_NOT_BEING_ATTACKED,
		fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			return ((Clinic)data).IsHealthBelowThreshold(context.consumerState.gameObject);
		}
	};

	// Token: 0x04003C67 RID: 15463
	[Serialize]
	private float sicknessSliderValue = 70f;

	// Token: 0x02001075 RID: 4213
	public class ClinicSM : GameStateMachine<Clinic.ClinicSM, Clinic.ClinicSM.Instance, Clinic>
	{
		// Token: 0x0600561A RID: 22042 RVA: 0x0028144C File Offset: 0x0027F64C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			base.serializable = StateMachine.SerializeType.Never;
			default_state = this.unoperational;
			this.unoperational.EventTransition(GameHashes.OperationalChanged, this.operational, (Clinic.ClinicSM.Instance smi) => smi.GetComponent<Operational>().IsOperational).Enter(delegate(Clinic.ClinicSM.Instance smi)
			{
				smi.master.GetComponent<Assignable>().Unassign();
			});
			this.operational.DefaultState(this.operational.idle).EventTransition(GameHashes.OperationalChanged, this.unoperational, (Clinic.ClinicSM.Instance smi) => !smi.master.GetComponent<Operational>().IsOperational).EventTransition(GameHashes.AssigneeChanged, this.unoperational, null).ToggleRecurringChore((Clinic.ClinicSM.Instance smi) => smi.master.CreateWorkChore(Db.Get().ChoreTypes.Heal, false, true, PriorityScreen.PriorityClass.personalNeeds, false), (Clinic.ClinicSM.Instance smi) => !string.IsNullOrEmpty(smi.master.healthEffect)).ToggleRecurringChore((Clinic.ClinicSM.Instance smi) => smi.master.CreateWorkChore(Db.Get().ChoreTypes.HealCritical, false, true, PriorityScreen.PriorityClass.personalNeeds, false), (Clinic.ClinicSM.Instance smi) => !string.IsNullOrEmpty(smi.master.healthEffect)).ToggleRecurringChore((Clinic.ClinicSM.Instance smi) => smi.master.CreateWorkChore(Db.Get().ChoreTypes.RestDueToDisease, false, true, PriorityScreen.PriorityClass.personalNeeds, true), (Clinic.ClinicSM.Instance smi) => !string.IsNullOrEmpty(smi.master.diseaseEffect)).ToggleRecurringChore((Clinic.ClinicSM.Instance smi) => smi.master.CreateWorkChore(Db.Get().ChoreTypes.SleepDueToDisease, false, true, PriorityScreen.PriorityClass.personalNeeds, true), (Clinic.ClinicSM.Instance smi) => !string.IsNullOrEmpty(smi.master.diseaseEffect));
			this.operational.idle.WorkableStartTransition((Clinic.ClinicSM.Instance smi) => smi.master, this.operational.healing);
			this.operational.healing.DefaultState(this.operational.healing.undoctored).WorkableStopTransition((Clinic.ClinicSM.Instance smi) => smi.GetComponent<Clinic>(), this.operational.idle).Enter(delegate(Clinic.ClinicSM.Instance smi)
			{
				smi.master.GetComponent<Operational>().SetActive(true, false);
			}).Exit(delegate(Clinic.ClinicSM.Instance smi)
			{
				smi.master.GetComponent<Operational>().SetActive(false, false);
			});
			this.operational.healing.undoctored.Enter(delegate(Clinic.ClinicSM.Instance smi)
			{
				smi.StartEffect(smi.master.healthEffect, false);
				smi.StartEffect(smi.master.diseaseEffect, false);
				bool flag = false;
				if (smi.master.worker != null)
				{
					flag = (smi.HasEffect(smi.master.doctoredHealthEffect) || smi.HasEffect(smi.master.doctoredDiseaseEffect) || smi.HasEffect(smi.master.doctoredPlaceholderEffect));
				}
				if (smi.master.AllowDoctoring())
				{
					if (flag)
					{
						smi.GoTo(this.operational.healing.doctored);
						return;
					}
					smi.StartDoctorChore();
				}
			}).Exit(delegate(Clinic.ClinicSM.Instance smi)
			{
				smi.StopEffect(smi.master.healthEffect);
				smi.StopEffect(smi.master.diseaseEffect);
				smi.StopDoctorChore();
			});
			this.operational.healing.newlyDoctored.Enter(delegate(Clinic.ClinicSM.Instance smi)
			{
				smi.StartEffect(smi.master.doctoredDiseaseEffect, true);
				smi.StartEffect(smi.master.doctoredHealthEffect, true);
				smi.GoTo(this.operational.healing.doctored);
			});
			this.operational.healing.doctored.Enter(delegate(Clinic.ClinicSM.Instance smi)
			{
				Effects component = smi.master.worker.GetComponent<Effects>();
				if (smi.HasEffect(smi.master.doctoredPlaceholderEffect))
				{
					EffectInstance effectInstance = component.Get(smi.master.doctoredPlaceholderEffect);
					EffectInstance effectInstance2 = smi.StartEffect(smi.master.doctoredDiseaseEffect, true);
					if (effectInstance2 != null)
					{
						float num = effectInstance.effect.duration - effectInstance.timeRemaining;
						effectInstance2.timeRemaining = effectInstance2.effect.duration - num;
					}
					EffectInstance effectInstance3 = smi.StartEffect(smi.master.doctoredHealthEffect, true);
					if (effectInstance3 != null)
					{
						float num2 = effectInstance.effect.duration - effectInstance.timeRemaining;
						effectInstance3.timeRemaining = effectInstance3.effect.duration - num2;
					}
					component.Remove(smi.master.doctoredPlaceholderEffect);
				}
			}).ScheduleGoTo(delegate(Clinic.ClinicSM.Instance smi)
			{
				Effects component = smi.master.worker.GetComponent<Effects>();
				float num = smi.master.doctorVisitInterval;
				if (smi.HasEffect(smi.master.doctoredHealthEffect))
				{
					EffectInstance effectInstance = component.Get(smi.master.doctoredHealthEffect);
					num = Mathf.Min(num, effectInstance.GetTimeRemaining());
				}
				if (smi.HasEffect(smi.master.doctoredDiseaseEffect))
				{
					EffectInstance effectInstance = component.Get(smi.master.doctoredDiseaseEffect);
					num = Mathf.Min(num, effectInstance.GetTimeRemaining());
				}
				return num;
			}, this.operational.healing.undoctored).Exit(delegate(Clinic.ClinicSM.Instance smi)
			{
				Effects component = smi.master.worker.GetComponent<Effects>();
				if (smi.HasEffect(smi.master.doctoredDiseaseEffect) || smi.HasEffect(smi.master.doctoredHealthEffect))
				{
					EffectInstance effectInstance = component.Get(smi.master.doctoredDiseaseEffect);
					if (effectInstance == null)
					{
						effectInstance = component.Get(smi.master.doctoredHealthEffect);
					}
					EffectInstance effectInstance2 = smi.StartEffect(smi.master.doctoredPlaceholderEffect, true);
					effectInstance2.timeRemaining = effectInstance2.effect.duration - (effectInstance.effect.duration - effectInstance.timeRemaining);
					component.Remove(smi.master.doctoredDiseaseEffect);
					component.Remove(smi.master.doctoredHealthEffect);
				}
			});
		}

		// Token: 0x04003C68 RID: 15464
		public GameStateMachine<Clinic.ClinicSM, Clinic.ClinicSM.Instance, Clinic, object>.State unoperational;

		// Token: 0x04003C69 RID: 15465
		public Clinic.ClinicSM.OperationalStates operational;

		// Token: 0x02001076 RID: 4214
		public class OperationalStates : GameStateMachine<Clinic.ClinicSM, Clinic.ClinicSM.Instance, Clinic, object>.State
		{
			// Token: 0x04003C6A RID: 15466
			public GameStateMachine<Clinic.ClinicSM, Clinic.ClinicSM.Instance, Clinic, object>.State idle;

			// Token: 0x04003C6B RID: 15467
			public Clinic.ClinicSM.HealingStates healing;
		}

		// Token: 0x02001077 RID: 4215
		public class HealingStates : GameStateMachine<Clinic.ClinicSM, Clinic.ClinicSM.Instance, Clinic, object>.State
		{
			// Token: 0x04003C6C RID: 15468
			public GameStateMachine<Clinic.ClinicSM, Clinic.ClinicSM.Instance, Clinic, object>.State undoctored;

			// Token: 0x04003C6D RID: 15469
			public GameStateMachine<Clinic.ClinicSM, Clinic.ClinicSM.Instance, Clinic, object>.State doctored;

			// Token: 0x04003C6E RID: 15470
			public GameStateMachine<Clinic.ClinicSM, Clinic.ClinicSM.Instance, Clinic, object>.State newlyDoctored;
		}

		// Token: 0x02001078 RID: 4216
		public new class Instance : GameStateMachine<Clinic.ClinicSM, Clinic.ClinicSM.Instance, Clinic, object>.GameInstance
		{
			// Token: 0x06005620 RID: 22048 RVA: 0x000D838E File Offset: 0x000D658E
			public Instance(Clinic master) : base(master)
			{
			}

			// Token: 0x06005621 RID: 22049 RVA: 0x002818A0 File Offset: 0x0027FAA0
			public void StartDoctorChore()
			{
				if (base.master.IsValidEffect(base.master.doctoredHealthEffect) || base.master.IsValidEffect(base.master.doctoredDiseaseEffect))
				{
					this.doctorChore = new WorkChore<DoctorChoreWorkable>(Db.Get().ChoreTypes.Doctor, base.smi.master, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, true, true);
					WorkChore<DoctorChoreWorkable> workChore = this.doctorChore;
					workChore.onComplete = (Action<Chore>)Delegate.Combine(workChore.onComplete, new Action<Chore>(delegate(Chore chore)
					{
						base.smi.GoTo(base.smi.sm.operational.healing.newlyDoctored);
					}));
				}
			}

			// Token: 0x06005622 RID: 22050 RVA: 0x000D8397 File Offset: 0x000D6597
			public void StopDoctorChore()
			{
				if (this.doctorChore != null)
				{
					this.doctorChore.Cancel("StopDoctorChore");
					this.doctorChore = null;
				}
			}

			// Token: 0x06005623 RID: 22051 RVA: 0x0028193C File Offset: 0x0027FB3C
			public bool HasEffect(string effect)
			{
				bool result = false;
				if (base.master.IsValidEffect(effect))
				{
					result = base.smi.master.worker.GetComponent<Effects>().HasEffect(effect);
				}
				return result;
			}

			// Token: 0x06005624 RID: 22052 RVA: 0x00281978 File Offset: 0x0027FB78
			public EffectInstance StartEffect(string effect, bool should_save)
			{
				if (base.master.IsValidEffect(effect))
				{
					WorkerBase worker = base.smi.master.worker;
					if (worker != null)
					{
						Effects component = worker.GetComponent<Effects>();
						if (!component.HasEffect(effect))
						{
							return component.Add(effect, should_save);
						}
					}
				}
				return null;
			}

			// Token: 0x06005625 RID: 22053 RVA: 0x002819C8 File Offset: 0x0027FBC8
			public void StopEffect(string effect)
			{
				if (base.master.IsValidEffect(effect))
				{
					WorkerBase worker = base.smi.master.worker;
					if (worker != null)
					{
						Effects component = worker.GetComponent<Effects>();
						if (component.HasEffect(effect))
						{
							component.Remove(effect);
						}
					}
				}
			}

			// Token: 0x04003C6F RID: 15471
			private WorkChore<DoctorChoreWorkable> doctorChore;
		}
	}
}
