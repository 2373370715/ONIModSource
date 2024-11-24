using System;
using KSerialization;
using UnityEngine;

// Token: 0x02001408 RID: 5128
[SerializationConfig(MemberSerialization.OptIn)]
public class IceMachine : StateMachineComponent<IceMachine.StatesInstance>, FewOptionSideScreen.IFewOptionSideScreen
{
	// Token: 0x06006951 RID: 26961 RVA: 0x000E4FDC File Offset: 0x000E31DC
	public void SetStorages(Storage waterStorage, Storage iceStorage)
	{
		this.waterStorage = waterStorage;
		this.iceStorage = iceStorage;
	}

	// Token: 0x06006952 RID: 26962 RVA: 0x002D996C File Offset: 0x002D7B6C
	private bool CanMakeIce()
	{
		bool flag = this.waterStorage != null && this.waterStorage.GetMassAvailable(SimHashes.Water) >= 0.1f;
		bool flag2 = this.iceStorage != null && this.iceStorage.IsFull();
		return flag && !flag2;
	}

	// Token: 0x06006953 RID: 26963 RVA: 0x002D99CC File Offset: 0x002D7BCC
	private void MakeIce(IceMachine.StatesInstance smi, float dt)
	{
		float num = this.heatRemovalRate * dt / (float)this.waterStorage.items.Count;
		foreach (GameObject gameObject in this.waterStorage.items)
		{
			GameUtil.DeltaThermalEnergy(gameObject.GetComponent<PrimaryElement>(), -num, smi.master.targetTemperature);
		}
		for (int i = this.waterStorage.items.Count; i > 0; i--)
		{
			GameObject gameObject2 = this.waterStorage.items[i - 1];
			if (gameObject2 && gameObject2.GetComponent<PrimaryElement>().Temperature < gameObject2.GetComponent<PrimaryElement>().Element.lowTemp)
			{
				PrimaryElement component = gameObject2.GetComponent<PrimaryElement>();
				this.waterStorage.AddOre(this.targetProductionElement, component.Mass, component.Temperature, component.DiseaseIdx, component.DiseaseCount, false, true);
				this.waterStorage.ConsumeIgnoringDisease(gameObject2);
			}
		}
		smi.UpdateIceState();
	}

	// Token: 0x06006954 RID: 26964 RVA: 0x000E4FEC File Offset: 0x000E31EC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x06006955 RID: 26965 RVA: 0x002D9AF4 File Offset: 0x002D7CF4
	public FewOptionSideScreen.IFewOptionSideScreen.Option[] GetOptions()
	{
		FewOptionSideScreen.IFewOptionSideScreen.Option[] array = new FewOptionSideScreen.IFewOptionSideScreen.Option[IceMachineConfig.ELEMENT_OPTIONS.Length];
		for (int i = 0; i < array.Length; i++)
		{
			string tooltipText = Strings.Get("STRINGS.BUILDINGS.PREFABS.ICEMACHINE.OPTION_TOOLTIPS." + IceMachineConfig.ELEMENT_OPTIONS[i].ToString().ToUpper());
			array[i] = new FewOptionSideScreen.IFewOptionSideScreen.Option(IceMachineConfig.ELEMENT_OPTIONS[i], ElementLoader.GetElement(IceMachineConfig.ELEMENT_OPTIONS[i]).name, Def.GetUISprite(IceMachineConfig.ELEMENT_OPTIONS[i], "ui", false), tooltipText);
		}
		return array;
	}

	// Token: 0x06006956 RID: 26966 RVA: 0x000E4FFF File Offset: 0x000E31FF
	public void OnOptionSelected(FewOptionSideScreen.IFewOptionSideScreen.Option option)
	{
		this.targetProductionElement = ElementLoader.GetElementID(option.tag);
	}

	// Token: 0x06006957 RID: 26967 RVA: 0x000E5012 File Offset: 0x000E3212
	public Tag GetSelectedOption()
	{
		return this.targetProductionElement.CreateTag();
	}

	// Token: 0x04004F72 RID: 20338
	[MyCmpGet]
	private Operational operational;

	// Token: 0x04004F73 RID: 20339
	public Storage waterStorage;

	// Token: 0x04004F74 RID: 20340
	public Storage iceStorage;

	// Token: 0x04004F75 RID: 20341
	public float targetTemperature;

	// Token: 0x04004F76 RID: 20342
	public float heatRemovalRate;

	// Token: 0x04004F77 RID: 20343
	private static StatusItem iceStorageFullStatusItem;

	// Token: 0x04004F78 RID: 20344
	[Serialize]
	public SimHashes targetProductionElement = SimHashes.Ice;

	// Token: 0x02001409 RID: 5129
	public class StatesInstance : GameStateMachine<IceMachine.States, IceMachine.StatesInstance, IceMachine, object>.GameInstance
	{
		// Token: 0x06006959 RID: 26969 RVA: 0x002D9B98 File Offset: 0x002D7D98
		public StatesInstance(IceMachine smi) : base(smi)
		{
			this.meter = new MeterController(base.gameObject.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
			{
				"meter_OL",
				"meter_frame",
				"meter_fill"
			});
			this.UpdateMeter();
			base.Subscribe(-1697596308, new Action<object>(this.OnStorageChange));
		}

		// Token: 0x0600695A RID: 26970 RVA: 0x000E5032 File Offset: 0x000E3232
		private void OnStorageChange(object data)
		{
			this.UpdateMeter();
		}

		// Token: 0x0600695B RID: 26971 RVA: 0x000E503A File Offset: 0x000E323A
		public void UpdateMeter()
		{
			this.meter.SetPositionPercent(Mathf.Clamp01(base.smi.master.iceStorage.MassStored() / base.smi.master.iceStorage.Capacity()));
		}

		// Token: 0x0600695C RID: 26972 RVA: 0x002D9C0C File Offset: 0x002D7E0C
		public void UpdateIceState()
		{
			bool value = false;
			for (int i = base.smi.master.waterStorage.items.Count; i > 0; i--)
			{
				GameObject gameObject = base.smi.master.waterStorage.items[i - 1];
				if (gameObject && gameObject.GetComponent<PrimaryElement>().Temperature <= base.smi.master.targetTemperature)
				{
					value = true;
				}
			}
			base.sm.doneFreezingIce.Set(value, this, false);
		}

		// Token: 0x04004F79 RID: 20345
		private MeterController meter;

		// Token: 0x04004F7A RID: 20346
		public Chore emptyChore;
	}

	// Token: 0x0200140A RID: 5130
	public class States : GameStateMachine<IceMachine.States, IceMachine.StatesInstance, IceMachine>
	{
		// Token: 0x0600695D RID: 26973 RVA: 0x002D9C9C File Offset: 0x002D7E9C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.off;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.off.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.on, (IceMachine.StatesInstance smi) => smi.master.operational.IsOperational);
			this.on.PlayAnim("on").EventTransition(GameHashes.OperationalChanged, this.off, (IceMachine.StatesInstance smi) => !smi.master.operational.IsOperational).DefaultState(this.on.waiting);
			this.on.waiting.EventTransition(GameHashes.OnStorageChange, this.on.working_pre, (IceMachine.StatesInstance smi) => smi.master.CanMakeIce());
			this.on.working_pre.Enter(delegate(IceMachine.StatesInstance smi)
			{
				smi.UpdateIceState();
			}).PlayAnim("working_pre").OnAnimQueueComplete(this.on.working);
			this.on.working.QueueAnim("working_loop", true, null).Update("UpdateWorking", delegate(IceMachine.StatesInstance smi, float dt)
			{
				smi.master.MakeIce(smi, dt);
			}, UpdateRate.SIM_200ms, false).ParamTransition<bool>(this.doneFreezingIce, this.on.working_pst, GameStateMachine<IceMachine.States, IceMachine.StatesInstance, IceMachine, object>.IsTrue).Enter(delegate(IceMachine.StatesInstance smi)
			{
				smi.master.operational.SetActive(true, false);
				smi.master.gameObject.GetComponent<ManualDeliveryKG>().Pause(true, "Working");
			}).Exit(delegate(IceMachine.StatesInstance smi)
			{
				smi.master.operational.SetActive(false, false);
				smi.master.gameObject.GetComponent<ManualDeliveryKG>().Pause(false, "Done Working");
			}).ToggleStatusItem(Db.Get().BuildingStatusItems.CoolingWater, null);
			this.on.working_pst.Exit(new StateMachine<IceMachine.States, IceMachine.StatesInstance, IceMachine, object>.State.Callback(this.DoTransfer)).PlayAnim("working_pst").OnAnimQueueComplete(this.on);
		}

		// Token: 0x0600695E RID: 26974 RVA: 0x002D9EC0 File Offset: 0x002D80C0
		private void DoTransfer(IceMachine.StatesInstance smi)
		{
			for (int i = smi.master.waterStorage.items.Count - 1; i >= 0; i--)
			{
				GameObject gameObject = smi.master.waterStorage.items[i];
				if (gameObject && gameObject.GetComponent<PrimaryElement>().Temperature <= smi.master.targetTemperature)
				{
					smi.master.waterStorage.Transfer(gameObject, smi.master.iceStorage, false, true);
				}
			}
			smi.UpdateMeter();
		}

		// Token: 0x04004F7B RID: 20347
		public StateMachine<IceMachine.States, IceMachine.StatesInstance, IceMachine, object>.BoolParameter doneFreezingIce;

		// Token: 0x04004F7C RID: 20348
		public GameStateMachine<IceMachine.States, IceMachine.StatesInstance, IceMachine, object>.State off;

		// Token: 0x04004F7D RID: 20349
		public IceMachine.States.OnStates on;

		// Token: 0x0200140B RID: 5131
		public class OnStates : GameStateMachine<IceMachine.States, IceMachine.StatesInstance, IceMachine, object>.State
		{
			// Token: 0x04004F7E RID: 20350
			public GameStateMachine<IceMachine.States, IceMachine.StatesInstance, IceMachine, object>.State waiting;

			// Token: 0x04004F7F RID: 20351
			public GameStateMachine<IceMachine.States, IceMachine.StatesInstance, IceMachine, object>.State working_pre;

			// Token: 0x04004F80 RID: 20352
			public GameStateMachine<IceMachine.States, IceMachine.StatesInstance, IceMachine, object>.State working;

			// Token: 0x04004F81 RID: 20353
			public GameStateMachine<IceMachine.States, IceMachine.StatesInstance, IceMachine, object>.State working_pst;
		}
	}
}
