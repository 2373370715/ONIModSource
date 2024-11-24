using System;
using System.Collections.Generic;
using Klei;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000F7C RID: 3964
[AddComponentMenu("KMonoBehaviour/Workable/Shower")]
public class Shower : Workable, IGameObjectEffectDescriptor
{
	// Token: 0x06005037 RID: 20535 RVA: 0x000AC786 File Offset: 0x000AA986
	private Shower()
	{
		base.SetReportType(ReportManager.ReportType.PersonalTime);
	}

	// Token: 0x06005038 RID: 20536 RVA: 0x000D44DC File Offset: 0x000D26DC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.resetProgressOnStop = true;
		this.smi = new Shower.ShowerSM.Instance(this);
		this.smi.StartSM();
	}

	// Token: 0x06005039 RID: 20537 RVA: 0x0026E050 File Offset: 0x0026C250
	protected override void OnStartWork(WorkerBase worker)
	{
		HygieneMonitor.Instance instance = worker.GetSMI<HygieneMonitor.Instance>();
		base.WorkTimeRemaining = this.workTime * instance.GetDirtiness();
		this.accumulatedDisease = SimUtil.DiseaseInfo.Invalid;
		this.smi.SetActive(true);
		base.OnStartWork(worker);
	}

	// Token: 0x0600503A RID: 20538 RVA: 0x000D4502 File Offset: 0x000D2702
	protected override void OnStopWork(WorkerBase worker)
	{
		this.smi.SetActive(false);
	}

	// Token: 0x0600503B RID: 20539 RVA: 0x0026E098 File Offset: 0x0026C298
	protected override void OnCompleteWork(WorkerBase worker)
	{
		base.OnCompleteWork(worker);
		Effects component = worker.GetComponent<Effects>();
		for (int i = 0; i < Shower.EffectsRemoved.Length; i++)
		{
			string effect_id = Shower.EffectsRemoved[i];
			component.Remove(effect_id);
		}
		if (!worker.HasTag(GameTags.HasSuitTank))
		{
			GasLiquidExposureMonitor.Instance instance = worker.GetSMI<GasLiquidExposureMonitor.Instance>();
			if (instance != null)
			{
				instance.ResetExposure();
			}
		}
		component.Add(Shower.SHOWER_EFFECT, true);
		HygieneMonitor.Instance instance2 = worker.GetSMI<HygieneMonitor.Instance>();
		if (instance2 != null)
		{
			instance2.SetDirtiness(0f);
		}
	}

	// Token: 0x0600503C RID: 20540 RVA: 0x0026E118 File Offset: 0x0026C318
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		PrimaryElement component = worker.GetComponent<PrimaryElement>();
		if (component.DiseaseCount > 0)
		{
			SimUtil.DiseaseInfo diseaseInfo = new SimUtil.DiseaseInfo
			{
				idx = component.DiseaseIdx,
				count = Mathf.CeilToInt((float)component.DiseaseCount * (1f - Mathf.Pow(this.fractionalDiseaseRemoval, dt)) - (float)this.absoluteDiseaseRemoval)
			};
			component.ModifyDiseaseCount(-diseaseInfo.count, "Shower.RemoveDisease");
			this.accumulatedDisease = SimUtil.CalculateFinalDiseaseInfo(this.accumulatedDisease, diseaseInfo);
			PrimaryElement primaryElement = base.GetComponent<Storage>().FindPrimaryElement(this.outputTargetElement);
			if (primaryElement != null)
			{
				primaryElement.GetComponent<PrimaryElement>().AddDisease(this.accumulatedDisease.idx, this.accumulatedDisease.count, "Shower.RemoveDisease");
				this.accumulatedDisease = SimUtil.DiseaseInfo.Invalid;
			}
		}
		return false;
	}

	// Token: 0x0600503D RID: 20541 RVA: 0x0026E1F0 File Offset: 0x0026C3F0
	protected override void OnAbortWork(WorkerBase worker)
	{
		base.OnAbortWork(worker);
		HygieneMonitor.Instance instance = worker.GetSMI<HygieneMonitor.Instance>();
		if (instance != null)
		{
			instance.SetDirtiness(1f - this.GetPercentComplete());
		}
	}

	// Token: 0x0600503E RID: 20542 RVA: 0x0026E220 File Offset: 0x0026C420
	public override List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> descriptors = base.GetDescriptors(go);
		if (Shower.EffectsRemoved.Length != 0)
		{
			Descriptor item = default(Descriptor);
			item.SetupDescriptor(UI.BUILDINGEFFECTS.REMOVESEFFECTSUBTITLE, UI.BUILDINGEFFECTS.TOOLTIPS.REMOVESEFFECTSUBTITLE, Descriptor.DescriptorType.Effect);
			descriptors.Add(item);
			for (int i = 0; i < Shower.EffectsRemoved.Length; i++)
			{
				string text = Shower.EffectsRemoved[i];
				string arg = Strings.Get("STRINGS.DUPLICANTS.MODIFIERS." + text.ToUpper() + ".NAME");
				string arg2 = Strings.Get("STRINGS.DUPLICANTS.MODIFIERS." + text.ToUpper() + ".CAUSE");
				Descriptor item2 = default(Descriptor);
				item2.IncreaseIndent();
				item2.SetupDescriptor("• " + string.Format(UI.BUILDINGEFFECTS.REMOVEDEFFECT, arg), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.REMOVEDEFFECT, arg2), Descriptor.DescriptorType.Effect);
				descriptors.Add(item2);
			}
		}
		Effect.AddModifierDescriptions(base.gameObject, descriptors, Shower.SHOWER_EFFECT, true);
		return descriptors;
	}

	// Token: 0x040037F2 RID: 14322
	private Shower.ShowerSM.Instance smi;

	// Token: 0x040037F3 RID: 14323
	public static string SHOWER_EFFECT = "Showered";

	// Token: 0x040037F4 RID: 14324
	public SimHashes outputTargetElement;

	// Token: 0x040037F5 RID: 14325
	public float fractionalDiseaseRemoval;

	// Token: 0x040037F6 RID: 14326
	public int absoluteDiseaseRemoval;

	// Token: 0x040037F7 RID: 14327
	private SimUtil.DiseaseInfo accumulatedDisease;

	// Token: 0x040037F8 RID: 14328
	public const float WATER_PER_USE = 5f;

	// Token: 0x040037F9 RID: 14329
	private static readonly string[] EffectsRemoved = new string[]
	{
		"SoakingWet",
		"WetFeet",
		"MinorIrritation",
		"MajorIrritation"
	};

	// Token: 0x02000F7D RID: 3965
	public class ShowerSM : GameStateMachine<Shower.ShowerSM, Shower.ShowerSM.Instance, Shower>
	{
		// Token: 0x06005040 RID: 20544 RVA: 0x0026E32C File Offset: 0x0026C52C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.unoperational;
			this.root.Update(new Action<Shower.ShowerSM.Instance, float>(this.UpdateStatusItems), UpdateRate.SIM_200ms, false);
			this.unoperational.EventTransition(GameHashes.OperationalChanged, this.operational, (Shower.ShowerSM.Instance smi) => smi.IsOperational).PlayAnim("off");
			this.operational.DefaultState(this.operational.not_ready).EventTransition(GameHashes.OperationalChanged, this.unoperational, (Shower.ShowerSM.Instance smi) => !smi.IsOperational);
			this.operational.not_ready.EventTransition(GameHashes.OnStorageChange, this.operational.ready, (Shower.ShowerSM.Instance smi) => smi.IsReady()).PlayAnim("off");
			this.operational.ready.ToggleChore(new Func<Shower.ShowerSM.Instance, Chore>(this.CreateShowerChore), this.operational.not_ready);
		}

		// Token: 0x06005041 RID: 20545 RVA: 0x0026E454 File Offset: 0x0026C654
		private Chore CreateShowerChore(Shower.ShowerSM.Instance smi)
		{
			WorkChore<Shower> workChore = new WorkChore<Shower>(Db.Get().ChoreTypes.Shower, smi.master, null, true, null, null, null, false, Db.Get().ScheduleBlockTypes.Hygiene, false, true, null, false, true, false, PriorityScreen.PriorityClass.high, 5, false, true);
			workChore.AddPrecondition(ChorePreconditions.instance.IsNotABionic, smi);
			return workChore;
		}

		// Token: 0x06005042 RID: 20546 RVA: 0x0026E4AC File Offset: 0x0026C6AC
		private void UpdateStatusItems(Shower.ShowerSM.Instance smi, float dt)
		{
			if (smi.OutputFull())
			{
				smi.master.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.OutputPipeFull, this);
				return;
			}
			smi.master.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.OutputPipeFull, false);
		}

		// Token: 0x040037FA RID: 14330
		public GameStateMachine<Shower.ShowerSM, Shower.ShowerSM.Instance, Shower, object>.State unoperational;

		// Token: 0x040037FB RID: 14331
		public Shower.ShowerSM.OperationalState operational;

		// Token: 0x02000F7E RID: 3966
		public class OperationalState : GameStateMachine<Shower.ShowerSM, Shower.ShowerSM.Instance, Shower, object>.State
		{
			// Token: 0x040037FC RID: 14332
			public GameStateMachine<Shower.ShowerSM, Shower.ShowerSM.Instance, Shower, object>.State not_ready;

			// Token: 0x040037FD RID: 14333
			public GameStateMachine<Shower.ShowerSM, Shower.ShowerSM.Instance, Shower, object>.State ready;
		}

		// Token: 0x02000F7F RID: 3967
		public new class Instance : GameStateMachine<Shower.ShowerSM, Shower.ShowerSM.Instance, Shower, object>.GameInstance
		{
			// Token: 0x06005045 RID: 20549 RVA: 0x000D4557 File Offset: 0x000D2757
			public Instance(Shower master) : base(master)
			{
				this.operational = master.GetComponent<Operational>();
				this.consumer = master.GetComponent<ConduitConsumer>();
				this.dispenser = master.GetComponent<ConduitDispenser>();
			}

			// Token: 0x1700047A RID: 1146
			// (get) Token: 0x06005046 RID: 20550 RVA: 0x000D4584 File Offset: 0x000D2784
			public bool IsOperational
			{
				get
				{
					return this.operational.IsOperational && this.consumer.IsConnected && this.dispenser.IsConnected;
				}
			}

			// Token: 0x06005047 RID: 20551 RVA: 0x000D45AD File Offset: 0x000D27AD
			public void SetActive(bool active)
			{
				this.operational.SetActive(active, false);
			}

			// Token: 0x06005048 RID: 20552 RVA: 0x0026E504 File Offset: 0x0026C704
			private bool HasSufficientMass()
			{
				bool result = false;
				PrimaryElement primaryElement = base.GetComponent<Storage>().FindPrimaryElement(SimHashes.Water);
				if (primaryElement != null)
				{
					result = (primaryElement.Mass >= 5f);
				}
				return result;
			}

			// Token: 0x06005049 RID: 20553 RVA: 0x0026E540 File Offset: 0x0026C740
			public bool OutputFull()
			{
				PrimaryElement primaryElement = base.GetComponent<Storage>().FindPrimaryElement(SimHashes.DirtyWater);
				return primaryElement != null && primaryElement.Mass >= 5f;
			}

			// Token: 0x0600504A RID: 20554 RVA: 0x000D45BC File Offset: 0x000D27BC
			public bool IsReady()
			{
				return this.HasSufficientMass() && !this.OutputFull();
			}

			// Token: 0x040037FE RID: 14334
			private Operational operational;

			// Token: 0x040037FF RID: 14335
			private ConduitConsumer consumer;

			// Token: 0x04003800 RID: 14336
			private ConduitDispenser dispenser;
		}
	}
}
