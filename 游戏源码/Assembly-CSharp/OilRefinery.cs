using System;
using KSerialization;
using TUNING;
using UnityEngine;

// Token: 0x02000EEC RID: 3820
[SerializationConfig(MemberSerialization.OptIn)]
public class OilRefinery : StateMachineComponent<OilRefinery.StatesInstance>
{
	// Token: 0x06004D0A RID: 19722 RVA: 0x00264208 File Offset: 0x00262408
	protected override void OnSpawn()
	{
		base.Subscribe<OilRefinery>(-1697596308, OilRefinery.OnStorageChangedDelegate);
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		this.meter = new MeterController(component, "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Vector3.zero, null);
		base.smi.StartSM();
		this.maxSrcMass = base.GetComponent<ConduitConsumer>().capacityKG;
	}

	// Token: 0x06004D0B RID: 19723 RVA: 0x00264268 File Offset: 0x00262468
	private void OnStorageChanged(object data)
	{
		float positionPercent = Mathf.Clamp01(this.storage.GetMassAvailable(SimHashes.CrudeOil) / this.maxSrcMass);
		this.meter.SetPositionPercent(positionPercent);
	}

	// Token: 0x06004D0C RID: 19724 RVA: 0x002642A0 File Offset: 0x002624A0
	private static bool UpdateStateCb(int cell, object data)
	{
		OilRefinery oilRefinery = data as OilRefinery;
		if (Grid.Element[cell].IsGas)
		{
			oilRefinery.cellCount += 1f;
			oilRefinery.envPressure += Grid.Mass[cell];
		}
		return true;
	}

	// Token: 0x06004D0D RID: 19725 RVA: 0x002642F0 File Offset: 0x002624F0
	private void TestAreaPressure()
	{
		this.envPressure = 0f;
		this.cellCount = 0f;
		if (this.occupyArea != null && base.gameObject != null)
		{
			this.occupyArea.TestArea(Grid.PosToCell(base.gameObject), this, new Func<int, object, bool>(OilRefinery.UpdateStateCb));
			this.envPressure /= this.cellCount;
		}
	}

	// Token: 0x06004D0E RID: 19726 RVA: 0x000D1FB9 File Offset: 0x000D01B9
	private bool IsOverPressure()
	{
		return this.envPressure >= this.overpressureMass;
	}

	// Token: 0x06004D0F RID: 19727 RVA: 0x000D1FCC File Offset: 0x000D01CC
	private bool IsOverWarningPressure()
	{
		return this.envPressure >= this.overpressureWarningMass;
	}

	// Token: 0x04003585 RID: 13701
	private bool wasOverPressure;

	// Token: 0x04003586 RID: 13702
	[SerializeField]
	public float overpressureWarningMass = 4.5f;

	// Token: 0x04003587 RID: 13703
	[SerializeField]
	public float overpressureMass = 5f;

	// Token: 0x04003588 RID: 13704
	private float maxSrcMass;

	// Token: 0x04003589 RID: 13705
	private float envPressure;

	// Token: 0x0400358A RID: 13706
	private float cellCount;

	// Token: 0x0400358B RID: 13707
	[MyCmpGet]
	private Storage storage;

	// Token: 0x0400358C RID: 13708
	[MyCmpReq]
	private Operational operational;

	// Token: 0x0400358D RID: 13709
	[MyCmpAdd]
	private OilRefinery.WorkableTarget workable;

	// Token: 0x0400358E RID: 13710
	[MyCmpReq]
	private OccupyArea occupyArea;

	// Token: 0x0400358F RID: 13711
	private const bool hasMeter = true;

	// Token: 0x04003590 RID: 13712
	private MeterController meter;

	// Token: 0x04003591 RID: 13713
	private static readonly EventSystem.IntraObjectHandler<OilRefinery> OnStorageChangedDelegate = new EventSystem.IntraObjectHandler<OilRefinery>(delegate(OilRefinery component, object data)
	{
		component.OnStorageChanged(data);
	});

	// Token: 0x02000EED RID: 3821
	public class StatesInstance : GameStateMachine<OilRefinery.States, OilRefinery.StatesInstance, OilRefinery, object>.GameInstance
	{
		// Token: 0x06004D12 RID: 19730 RVA: 0x000D2019 File Offset: 0x000D0219
		public StatesInstance(OilRefinery smi) : base(smi)
		{
		}

		// Token: 0x06004D13 RID: 19731 RVA: 0x00264368 File Offset: 0x00262568
		public void TestAreaPressure()
		{
			base.smi.master.TestAreaPressure();
			bool flag = base.smi.master.IsOverPressure();
			bool flag2 = base.smi.master.IsOverWarningPressure();
			if (flag)
			{
				base.smi.master.wasOverPressure = true;
				base.sm.isOverPressure.Set(true, this, false);
				return;
			}
			if (base.smi.master.wasOverPressure && !flag2)
			{
				base.sm.isOverPressure.Set(false, this, false);
			}
		}
	}

	// Token: 0x02000EEE RID: 3822
	public class States : GameStateMachine<OilRefinery.States, OilRefinery.StatesInstance, OilRefinery>
	{
		// Token: 0x06004D14 RID: 19732 RVA: 0x002643F8 File Offset: 0x002625F8
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.disabled;
			this.root.EventTransition(GameHashes.OperationalChanged, this.disabled, (OilRefinery.StatesInstance smi) => !smi.master.operational.IsOperational);
			this.disabled.EventTransition(GameHashes.OperationalChanged, this.needResources, (OilRefinery.StatesInstance smi) => smi.master.operational.IsOperational);
			this.needResources.EventTransition(GameHashes.OnStorageChange, this.ready, (OilRefinery.StatesInstance smi) => smi.master.GetComponent<ElementConverter>().HasEnoughMassToStartConverting(false));
			this.ready.Update("Test Pressure Update", delegate(OilRefinery.StatesInstance smi, float dt)
			{
				smi.TestAreaPressure();
			}, UpdateRate.SIM_1000ms, false).ParamTransition<bool>(this.isOverPressure, this.overpressure, GameStateMachine<OilRefinery.States, OilRefinery.StatesInstance, OilRefinery, object>.IsTrue).Transition(this.needResources, (OilRefinery.StatesInstance smi) => !smi.master.GetComponent<ElementConverter>().HasEnoughMassToStartConverting(false), UpdateRate.SIM_200ms).ToggleChore((OilRefinery.StatesInstance smi) => new WorkChore<OilRefinery.WorkableTarget>(Db.Get().ChoreTypes.Fabricate, smi.master.workable, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true), this.needResources);
			this.overpressure.Update("Test Pressure Update", delegate(OilRefinery.StatesInstance smi, float dt)
			{
				smi.TestAreaPressure();
			}, UpdateRate.SIM_1000ms, false).ParamTransition<bool>(this.isOverPressure, this.ready, GameStateMachine<OilRefinery.States, OilRefinery.StatesInstance, OilRefinery, object>.IsFalse).ToggleStatusItem(Db.Get().BuildingStatusItems.PressureOk, null);
		}

		// Token: 0x04003592 RID: 13714
		public StateMachine<OilRefinery.States, OilRefinery.StatesInstance, OilRefinery, object>.BoolParameter isOverPressure;

		// Token: 0x04003593 RID: 13715
		public StateMachine<OilRefinery.States, OilRefinery.StatesInstance, OilRefinery, object>.BoolParameter isOverPressureWarning;

		// Token: 0x04003594 RID: 13716
		public GameStateMachine<OilRefinery.States, OilRefinery.StatesInstance, OilRefinery, object>.State disabled;

		// Token: 0x04003595 RID: 13717
		public GameStateMachine<OilRefinery.States, OilRefinery.StatesInstance, OilRefinery, object>.State overpressure;

		// Token: 0x04003596 RID: 13718
		public GameStateMachine<OilRefinery.States, OilRefinery.StatesInstance, OilRefinery, object>.State needResources;

		// Token: 0x04003597 RID: 13719
		public GameStateMachine<OilRefinery.States, OilRefinery.StatesInstance, OilRefinery, object>.State ready;
	}

	// Token: 0x02000EF0 RID: 3824
	[AddComponentMenu("KMonoBehaviour/Workable/WorkableTarget")]
	public class WorkableTarget : Workable
	{
		// Token: 0x06004D1F RID: 19743 RVA: 0x002645EC File Offset: 0x002627EC
		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
			this.showProgressBar = false;
			this.workerStatusItem = null;
			this.skillExperienceSkillGroup = Db.Get().SkillGroups.Technicals.Id;
			this.skillExperienceMultiplier = SKILLS.MOST_DAY_EXPERIENCE;
			this.overrideAnims = new KAnimFile[]
			{
				Assets.GetAnim("anim_interacts_oilrefinery_kanim")
			};
		}

		// Token: 0x06004D20 RID: 19744 RVA: 0x000D208E File Offset: 0x000D028E
		protected override void OnSpawn()
		{
			base.OnSpawn();
			base.SetWorkTime(float.PositiveInfinity);
		}

		// Token: 0x06004D21 RID: 19745 RVA: 0x000D20A1 File Offset: 0x000D02A1
		protected override void OnStartWork(WorkerBase worker)
		{
			this.operational.SetActive(true, false);
		}

		// Token: 0x06004D22 RID: 19746 RVA: 0x000D20B0 File Offset: 0x000D02B0
		protected override void OnStopWork(WorkerBase worker)
		{
			this.operational.SetActive(false, false);
		}

		// Token: 0x06004D23 RID: 19747 RVA: 0x000D20B0 File Offset: 0x000D02B0
		protected override void OnCompleteWork(WorkerBase worker)
		{
			this.operational.SetActive(false, false);
		}

		// Token: 0x06004D24 RID: 19748 RVA: 0x000AD2F7 File Offset: 0x000AB4F7
		public override bool InstantlyFinish(WorkerBase worker)
		{
			return false;
		}

		// Token: 0x040035A0 RID: 13728
		[MyCmpGet]
		public Operational operational;
	}
}
