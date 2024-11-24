using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000D5A RID: 3418
[SerializationConfig(MemberSerialization.OptIn)]
public class Electrolyzer : StateMachineComponent<Electrolyzer.StatesInstance>
{
	// Token: 0x060042F4 RID: 17140 RVA: 0x00243148 File Offset: 0x00241348
	protected override void OnSpawn()
	{
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		if (this.hasMeter)
		{
			this.meter = new MeterController(component, "U2H_meter_target", "meter", Meter.Offset.Behind, Grid.SceneLayer.NoLayer, new Vector3(-0.4f, 0.5f, -0.1f), new string[]
			{
				"U2H_meter_target",
				"U2H_meter_tank",
				"U2H_meter_waterbody",
				"U2H_meter_level"
			});
		}
		base.smi.StartSM();
		this.UpdateMeter();
		Tutorial.Instance.oxygenGenerators.Add(base.gameObject);
	}

	// Token: 0x060042F5 RID: 17141 RVA: 0x000CB41F File Offset: 0x000C961F
	protected override void OnCleanUp()
	{
		Tutorial.Instance.oxygenGenerators.Remove(base.gameObject);
		base.OnCleanUp();
	}

	// Token: 0x060042F6 RID: 17142 RVA: 0x002431E0 File Offset: 0x002413E0
	public void UpdateMeter()
	{
		if (this.hasMeter)
		{
			float positionPercent = Mathf.Clamp01(this.storage.MassStored() / this.storage.capacityKg);
			this.meter.SetPositionPercent(positionPercent);
		}
	}

	// Token: 0x1700034F RID: 847
	// (get) Token: 0x060042F7 RID: 17143 RVA: 0x00243220 File Offset: 0x00241420
	private bool RoomForPressure
	{
		get
		{
			int num = Grid.PosToCell(base.transform.GetPosition());
			num = Grid.OffsetCell(num, this.emissionOffset);
			return !GameUtil.FloodFillCheck<Electrolyzer>(new Func<int, Electrolyzer, bool>(Electrolyzer.OverPressure), this, num, 3, true, true);
		}
	}

	// Token: 0x060042F8 RID: 17144 RVA: 0x000CB43D File Offset: 0x000C963D
	private static bool OverPressure(int cell, Electrolyzer electrolyzer)
	{
		return Grid.Mass[cell] > electrolyzer.maxMass;
	}

	// Token: 0x04002DC8 RID: 11720
	[SerializeField]
	public float maxMass = 2.5f;

	// Token: 0x04002DC9 RID: 11721
	[SerializeField]
	public bool hasMeter = true;

	// Token: 0x04002DCA RID: 11722
	[SerializeField]
	public CellOffset emissionOffset = CellOffset.none;

	// Token: 0x04002DCB RID: 11723
	[MyCmpAdd]
	private Storage storage;

	// Token: 0x04002DCC RID: 11724
	[MyCmpGet]
	private ElementConverter emitter;

	// Token: 0x04002DCD RID: 11725
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04002DCE RID: 11726
	private MeterController meter;

	// Token: 0x02000D5B RID: 3419
	public class StatesInstance : GameStateMachine<Electrolyzer.States, Electrolyzer.StatesInstance, Electrolyzer, object>.GameInstance
	{
		// Token: 0x060042FA RID: 17146 RVA: 0x000CB477 File Offset: 0x000C9677
		public StatesInstance(Electrolyzer smi) : base(smi)
		{
		}
	}

	// Token: 0x02000D5C RID: 3420
	public class States : GameStateMachine<Electrolyzer.States, Electrolyzer.StatesInstance, Electrolyzer>
	{
		// Token: 0x060042FB RID: 17147 RVA: 0x00243264 File Offset: 0x00241464
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.disabled;
			this.root.EventTransition(GameHashes.OperationalChanged, this.disabled, (Electrolyzer.StatesInstance smi) => !smi.master.operational.IsOperational).EventHandler(GameHashes.OnStorageChange, delegate(Electrolyzer.StatesInstance smi)
			{
				smi.master.UpdateMeter();
			});
			this.disabled.EventTransition(GameHashes.OperationalChanged, this.waiting, (Electrolyzer.StatesInstance smi) => smi.master.operational.IsOperational);
			this.waiting.Enter("Waiting", delegate(Electrolyzer.StatesInstance smi)
			{
				smi.master.operational.SetActive(false, false);
			}).EventTransition(GameHashes.OnStorageChange, this.converting, (Electrolyzer.StatesInstance smi) => smi.master.GetComponent<ElementConverter>().HasEnoughMassToStartConverting(false));
			this.converting.Enter("Ready", delegate(Electrolyzer.StatesInstance smi)
			{
				smi.master.operational.SetActive(true, false);
			}).Transition(this.waiting, (Electrolyzer.StatesInstance smi) => !smi.master.GetComponent<ElementConverter>().CanConvertAtAll(), UpdateRate.SIM_200ms).Transition(this.overpressure, (Electrolyzer.StatesInstance smi) => !smi.master.RoomForPressure, UpdateRate.SIM_200ms);
			this.overpressure.Enter("OverPressure", delegate(Electrolyzer.StatesInstance smi)
			{
				smi.master.operational.SetActive(false, false);
			}).ToggleStatusItem(Db.Get().BuildingStatusItems.PressureOk, null).Transition(this.converting, (Electrolyzer.StatesInstance smi) => smi.master.RoomForPressure, UpdateRate.SIM_200ms);
		}

		// Token: 0x04002DCF RID: 11727
		public GameStateMachine<Electrolyzer.States, Electrolyzer.StatesInstance, Electrolyzer, object>.State disabled;

		// Token: 0x04002DD0 RID: 11728
		public GameStateMachine<Electrolyzer.States, Electrolyzer.StatesInstance, Electrolyzer, object>.State waiting;

		// Token: 0x04002DD1 RID: 11729
		public GameStateMachine<Electrolyzer.States, Electrolyzer.StatesInstance, Electrolyzer, object>.State converting;

		// Token: 0x04002DD2 RID: 11730
		public GameStateMachine<Electrolyzer.States, Electrolyzer.StatesInstance, Electrolyzer, object>.State overpressure;
	}
}
