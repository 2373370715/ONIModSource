using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000D56 RID: 3414
public class ElectrobankDischarger : Generator
{
	// Token: 0x1700034E RID: 846
	// (get) Token: 0x060042DD RID: 17117 RVA: 0x00242C50 File Offset: 0x00240E50
	public float ElectrobankJoulesStored
	{
		get
		{
			float num = 0f;
			foreach (Electrobank electrobank in this.storedCells)
			{
				num += electrobank.Charge;
			}
			return num;
		}
	}

	// Token: 0x060042DE RID: 17118 RVA: 0x00242CAC File Offset: 0x00240EAC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.smi = new ElectrobankDischarger.StatesInstance(this);
		this.smi.StartSM();
		base.Subscribe(-1697596308, new Action<object>(this.OnStorageChange));
		base.Subscribe(-592767678, new Action<object>(this.RefreshOperationalActive));
		this.RefreshCells(null);
		this.RefreshOperationalActive(null);
		this.filteredStorage = new FilteredStorage(this, null, null, false, Db.Get().ChoreTypes.PowerFetch);
		this.filteredStorage.SetHasMeter(false);
		this.filteredStorage.FilterChanged();
		Storage storage = this.storage;
		storage.onDestroyItemsDropped = (Action<List<GameObject>>)Delegate.Combine(storage.onDestroyItemsDropped, new Action<List<GameObject>>(this.OnBatteriesDroppedFromDeconstruction));
	}

	// Token: 0x060042DF RID: 17119 RVA: 0x00242D70 File Offset: 0x00240F70
	private void OnBatteriesDroppedFromDeconstruction(List<GameObject> items)
	{
		if (items != null)
		{
			for (int i = 0; i < items.Count; i++)
			{
				Electrobank component = items[i].GetComponent<Electrobank>();
				if (component != null && component.HasTag(GameTags.ChargedPortableBattery) && !component.IsFullyCharged)
				{
					component.RemovePower(component.Charge, true);
				}
			}
		}
	}

	// Token: 0x060042E0 RID: 17120 RVA: 0x000CB36D File Offset: 0x000C956D
	protected override void OnCleanUp()
	{
		this.filteredStorage.CleanUp();
		base.OnCleanUp();
	}

	// Token: 0x060042E1 RID: 17121 RVA: 0x000CB380 File Offset: 0x000C9580
	private void OnStorageChange(object data = null)
	{
		this.RefreshCells(null);
		this.RefreshOperationalActive(null);
	}

	// Token: 0x060042E2 RID: 17122 RVA: 0x00242DCC File Offset: 0x00240FCC
	public void UpdateMeter()
	{
		if (this.meterController == null)
		{
			this.meterController = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Array.Empty<string>());
		}
		this.meterController.SetPositionPercent(this.smi.master.ElectrobankJoulesStored / 120000f);
	}

	// Token: 0x060042E3 RID: 17123 RVA: 0x000CB390 File Offset: 0x000C9590
	private void RefreshOperationalActive(object data = null)
	{
		if (this.operational.IsOperational)
		{
			if (this.storedCells.Count > 0)
			{
				this.operational.SetActive(true, false);
				return;
			}
			this.operational.SetActive(false, false);
		}
	}

	// Token: 0x060042E4 RID: 17124 RVA: 0x00242E28 File Offset: 0x00241028
	public override void EnergySim200ms(float dt)
	{
		base.EnergySim200ms(dt);
		ushort circuitID = base.CircuitID;
		this.operational.SetFlag(Generator.wireConnectedFlag, circuitID != ushort.MaxValue);
		if (!this.operational.IsActive)
		{
			return;
		}
		float num = 0f;
		float num2 = Mathf.Min(this.wattageRating * dt, this.Capacity - this.JoulesAvailable);
		for (int i = this.storedCells.Count - 1; i >= 0; i--)
		{
			num += this.storedCells[i].RemovePower(num2 - num, true);
			if (num >= num2)
			{
				break;
			}
		}
		if (num > 0f)
		{
			base.GenerateJoules(num, false);
		}
	}

	// Token: 0x060042E5 RID: 17125 RVA: 0x00242ED4 File Offset: 0x002410D4
	private void RefreshCells(object data = null)
	{
		this.storedCells.Clear();
		foreach (GameObject gameObject in this.storage.GetItems())
		{
			Electrobank component = gameObject.GetComponent<Electrobank>();
			if (component != null)
			{
				this.storedCells.Add(component);
			}
		}
	}

	// Token: 0x04002DB5 RID: 11701
	public float wattageRating;

	// Token: 0x04002DB6 RID: 11702
	[MyCmpReq]
	private Storage storage;

	// Token: 0x04002DB7 RID: 11703
	private ElectrobankDischarger.StatesInstance smi;

	// Token: 0x04002DB8 RID: 11704
	private List<Electrobank> storedCells = new List<Electrobank>();

	// Token: 0x04002DB9 RID: 11705
	private MeterController meterController;

	// Token: 0x04002DBA RID: 11706
	protected FilteredStorage filteredStorage;

	// Token: 0x02000D57 RID: 3415
	public class StatesInstance : GameStateMachine<ElectrobankDischarger.States, ElectrobankDischarger.StatesInstance, ElectrobankDischarger, object>.GameInstance
	{
		// Token: 0x060042E7 RID: 17127 RVA: 0x000CB3DB File Offset: 0x000C95DB
		public StatesInstance(ElectrobankDischarger master) : base(master)
		{
		}
	}

	// Token: 0x02000D58 RID: 3416
	public class States : GameStateMachine<ElectrobankDischarger.States, ElectrobankDischarger.StatesInstance, ElectrobankDischarger>
	{
		// Token: 0x060042E8 RID: 17128 RVA: 0x00242F4C File Offset: 0x0024114C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.noBattery;
			this.root.EventTransition(GameHashes.ActiveChanged, this.discharging, (ElectrobankDischarger.StatesInstance smi) => smi.GetComponent<Operational>().IsActive);
			this.noBattery.PlayAnim("off").Enter(delegate(ElectrobankDischarger.StatesInstance smi)
			{
				smi.master.UpdateMeter();
			});
			this.inoperational.PlayAnim("on").Enter(delegate(ElectrobankDischarger.StatesInstance smi)
			{
				smi.master.UpdateMeter();
			}).EnterTransition(this.noBattery, (ElectrobankDischarger.StatesInstance smi) => smi.master.storage.items.Count == 0);
			this.discharging.Enter(delegate(ElectrobankDischarger.StatesInstance smi)
			{
				smi.master.UpdateMeter();
			}).EventTransition(GameHashes.ActiveChanged, this.inoperational, (ElectrobankDischarger.StatesInstance smi) => !smi.GetComponent<Operational>().IsActive).QueueAnim("working_pre", false, null).QueueAnim("working_loop", true, null).Update(delegate(ElectrobankDischarger.StatesInstance smi, float dt)
			{
				smi.master.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, Db.Get().BuildingStatusItems.ElectrobankJoulesAvailable, smi.master);
				smi.master.UpdateMeter();
			}, UpdateRate.SIM_200ms, false);
			this.discharging_pst.Enter(delegate(ElectrobankDischarger.StatesInstance smi)
			{
				smi.master.UpdateMeter();
			}).PlayAnim("working_pst");
		}

		// Token: 0x04002DBB RID: 11707
		public GameStateMachine<ElectrobankDischarger.States, ElectrobankDischarger.StatesInstance, ElectrobankDischarger, object>.State noBattery;

		// Token: 0x04002DBC RID: 11708
		public GameStateMachine<ElectrobankDischarger.States, ElectrobankDischarger.StatesInstance, ElectrobankDischarger, object>.State inoperational;

		// Token: 0x04002DBD RID: 11709
		public GameStateMachine<ElectrobankDischarger.States, ElectrobankDischarger.StatesInstance, ElectrobankDischarger, object>.State discharging;

		// Token: 0x04002DBE RID: 11710
		public GameStateMachine<ElectrobankDischarger.States, ElectrobankDischarger.StatesInstance, ElectrobankDischarger, object>.State discharging_pst;
	}
}
