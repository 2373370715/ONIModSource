using System;

// Token: 0x02000FBF RID: 4031
public class StorageLockerSmart : StorageLocker
{
	// Token: 0x0600519E RID: 20894 RVA: 0x000D52D5 File Offset: 0x000D34D5
	protected override void OnPrefabInit()
	{
		base.Initialize(true);
	}

	// Token: 0x0600519F RID: 20895 RVA: 0x002725A8 File Offset: 0x002707A8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.ports = base.gameObject.GetComponent<LogicPorts>();
		base.Subscribe<StorageLockerSmart>(-1697596308, StorageLockerSmart.UpdateLogicCircuitCBDelegate);
		base.Subscribe<StorageLockerSmart>(-592767678, StorageLockerSmart.UpdateLogicCircuitCBDelegate);
		this.UpdateLogicAndActiveState();
	}

	// Token: 0x060051A0 RID: 20896 RVA: 0x000D52DE File Offset: 0x000D34DE
	private void UpdateLogicCircuitCB(object data)
	{
		this.UpdateLogicAndActiveState();
	}

	// Token: 0x060051A1 RID: 20897 RVA: 0x002725F4 File Offset: 0x002707F4
	private void UpdateLogicAndActiveState()
	{
		bool flag = this.filteredStorage.IsFull();
		bool isOperational = this.operational.IsOperational;
		bool flag2 = flag && isOperational;
		this.ports.SendSignal(FilteredStorage.FULL_PORT_ID, flag2 ? 1 : 0);
		this.filteredStorage.SetLogicMeter(flag2);
		this.operational.SetActive(isOperational, false);
	}

	// Token: 0x1700049A RID: 1178
	// (get) Token: 0x060051A2 RID: 20898 RVA: 0x000D52E6 File Offset: 0x000D34E6
	// (set) Token: 0x060051A3 RID: 20899 RVA: 0x000D52EE File Offset: 0x000D34EE
	public override float UserMaxCapacity
	{
		get
		{
			return base.UserMaxCapacity;
		}
		set
		{
			base.UserMaxCapacity = value;
			this.UpdateLogicAndActiveState();
		}
	}

	// Token: 0x0400391C RID: 14620
	[MyCmpGet]
	private LogicPorts ports;

	// Token: 0x0400391D RID: 14621
	[MyCmpGet]
	private Operational operational;

	// Token: 0x0400391E RID: 14622
	private static readonly EventSystem.IntraObjectHandler<StorageLockerSmart> UpdateLogicCircuitCBDelegate = new EventSystem.IntraObjectHandler<StorageLockerSmart>(delegate(StorageLockerSmart component, object data)
	{
		component.UpdateLogicCircuitCB(data);
	});
}
