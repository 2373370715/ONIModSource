using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000CED RID: 3309
public class Compost : StateMachineComponent<Compost.StatesInstance>, IGameObjectEffectDescriptor
{
	// Token: 0x0600407C RID: 16508 RVA: 0x000C9CA3 File Offset: 0x000C7EA3
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<Compost>(-1697596308, Compost.OnStorageChangedDelegate);
	}

	// Token: 0x0600407D RID: 16509 RVA: 0x0023BA48 File Offset: 0x00239C48
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.GetComponent<ManualDeliveryKG>().ShowStatusItem = false;
		this.temperatureAdjuster = new SimulatedTemperatureAdjuster(this.simulatedInternalTemperature, this.simulatedInternalHeatCapacity, this.simulatedThermalConductivity, base.GetComponent<Storage>());
		base.smi.StartSM();
	}

	// Token: 0x0600407E RID: 16510 RVA: 0x000C9CBC File Offset: 0x000C7EBC
	protected override void OnCleanUp()
	{
		this.temperatureAdjuster.CleanUp();
	}

	// Token: 0x0600407F RID: 16511 RVA: 0x000C9CC9 File Offset: 0x000C7EC9
	private void OnStorageChanged(object data)
	{
		(GameObject)data == null;
	}

	// Token: 0x06004080 RID: 16512 RVA: 0x000C9CD8 File Offset: 0x000C7ED8
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		return SimulatedTemperatureAdjuster.GetDescriptors(this.simulatedInternalTemperature);
	}

	// Token: 0x04002C1F RID: 11295
	[MyCmpGet]
	private Operational operational;

	// Token: 0x04002C20 RID: 11296
	[MyCmpGet]
	private Storage storage;

	// Token: 0x04002C21 RID: 11297
	[SerializeField]
	public float flipInterval = 600f;

	// Token: 0x04002C22 RID: 11298
	[SerializeField]
	public float simulatedInternalTemperature = 323.15f;

	// Token: 0x04002C23 RID: 11299
	[SerializeField]
	public float simulatedInternalHeatCapacity = 400f;

	// Token: 0x04002C24 RID: 11300
	[SerializeField]
	public float simulatedThermalConductivity = 1000f;

	// Token: 0x04002C25 RID: 11301
	private SimulatedTemperatureAdjuster temperatureAdjuster;

	// Token: 0x04002C26 RID: 11302
	private static readonly EventSystem.IntraObjectHandler<Compost> OnStorageChangedDelegate = new EventSystem.IntraObjectHandler<Compost>(delegate(Compost component, object data)
	{
		component.OnStorageChanged(data);
	});

	// Token: 0x02000CEE RID: 3310
	public class StatesInstance : GameStateMachine<Compost.States, Compost.StatesInstance, Compost, object>.GameInstance
	{
		// Token: 0x06004083 RID: 16515 RVA: 0x000C9D35 File Offset: 0x000C7F35
		public StatesInstance(Compost master) : base(master)
		{
		}

		// Token: 0x06004084 RID: 16516 RVA: 0x000C9D3E File Offset: 0x000C7F3E
		public bool CanStartConverting()
		{
			return base.master.GetComponent<ElementConverter>().HasEnoughMassToStartConverting(false);
		}

		// Token: 0x06004085 RID: 16517 RVA: 0x000C9D51 File Offset: 0x000C7F51
		public bool CanContinueConverting()
		{
			return base.master.GetComponent<ElementConverter>().CanConvertAtAll();
		}

		// Token: 0x06004086 RID: 16518 RVA: 0x000C9D63 File Offset: 0x000C7F63
		public bool IsEmpty()
		{
			return base.master.storage.IsEmpty();
		}

		// Token: 0x06004087 RID: 16519 RVA: 0x000C9D75 File Offset: 0x000C7F75
		public void ResetWorkable()
		{
			CompostWorkable component = base.master.GetComponent<CompostWorkable>();
			component.ShowProgressBar(false);
			component.WorkTimeRemaining = component.GetWorkTime();
		}
	}

	// Token: 0x02000CEF RID: 3311
	public class States : GameStateMachine<Compost.States, Compost.StatesInstance, Compost>
	{
		// Token: 0x06004088 RID: 16520 RVA: 0x0023BA98 File Offset: 0x00239C98
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.empty;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.empty.Enter("empty", delegate(Compost.StatesInstance smi)
			{
				smi.ResetWorkable();
			}).EventTransition(GameHashes.OnStorageChange, this.insufficientMass, (Compost.StatesInstance smi) => !smi.IsEmpty()).EventTransition(GameHashes.OperationalChanged, this.disabledEmpty, (Compost.StatesInstance smi) => !smi.GetComponent<Operational>().IsOperational).ToggleStatusItem(Db.Get().BuildingStatusItems.AwaitingWaste, null).PlayAnim("off");
			this.insufficientMass.Enter("empty", delegate(Compost.StatesInstance smi)
			{
				smi.ResetWorkable();
			}).EventTransition(GameHashes.OnStorageChange, this.empty, (Compost.StatesInstance smi) => smi.IsEmpty()).EventTransition(GameHashes.OnStorageChange, this.inert, (Compost.StatesInstance smi) => smi.CanStartConverting()).ToggleStatusItem(Db.Get().BuildingStatusItems.AwaitingWaste, null).PlayAnim("idle_half");
			this.inert.EventTransition(GameHashes.OperationalChanged, this.disabled, (Compost.StatesInstance smi) => !smi.GetComponent<Operational>().IsOperational).PlayAnim("on").ToggleStatusItem(Db.Get().BuildingStatusItems.AwaitingCompostFlip, null).ToggleChore(new Func<Compost.StatesInstance, Chore>(this.CreateFlipChore), this.composting);
			this.composting.Enter("Composting", delegate(Compost.StatesInstance smi)
			{
				smi.master.operational.SetActive(true, false);
			}).EventTransition(GameHashes.OnStorageChange, this.empty, (Compost.StatesInstance smi) => !smi.CanContinueConverting()).EventTransition(GameHashes.OperationalChanged, this.disabled, (Compost.StatesInstance smi) => !smi.GetComponent<Operational>().IsOperational).ScheduleGoTo((Compost.StatesInstance smi) => smi.master.flipInterval, this.inert).Exit(delegate(Compost.StatesInstance smi)
			{
				smi.master.operational.SetActive(false, false);
			});
			this.disabled.Enter("disabledEmpty", delegate(Compost.StatesInstance smi)
			{
				smi.ResetWorkable();
			}).PlayAnim("on").EventTransition(GameHashes.OperationalChanged, this.inert, (Compost.StatesInstance smi) => smi.GetComponent<Operational>().IsOperational);
			this.disabledEmpty.Enter("disabledEmpty", delegate(Compost.StatesInstance smi)
			{
				smi.ResetWorkable();
			}).PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.empty, (Compost.StatesInstance smi) => smi.GetComponent<Operational>().IsOperational);
		}

		// Token: 0x06004089 RID: 16521 RVA: 0x0023BE28 File Offset: 0x0023A028
		private Chore CreateFlipChore(Compost.StatesInstance smi)
		{
			return new WorkChore<CompostWorkable>(Db.Get().ChoreTypes.FlipCompost, smi.master, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
		}

		// Token: 0x04002C27 RID: 11303
		public GameStateMachine<Compost.States, Compost.StatesInstance, Compost, object>.State empty;

		// Token: 0x04002C28 RID: 11304
		public GameStateMachine<Compost.States, Compost.StatesInstance, Compost, object>.State insufficientMass;

		// Token: 0x04002C29 RID: 11305
		public GameStateMachine<Compost.States, Compost.StatesInstance, Compost, object>.State disabled;

		// Token: 0x04002C2A RID: 11306
		public GameStateMachine<Compost.States, Compost.StatesInstance, Compost, object>.State disabledEmpty;

		// Token: 0x04002C2B RID: 11307
		public GameStateMachine<Compost.States, Compost.StatesInstance, Compost, object>.State inert;

		// Token: 0x04002C2C RID: 11308
		public GameStateMachine<Compost.States, Compost.StatesInstance, Compost, object>.State composting;
	}
}
