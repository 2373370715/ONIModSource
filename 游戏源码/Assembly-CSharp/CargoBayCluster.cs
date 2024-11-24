using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020018BB RID: 6331
public class CargoBayCluster : KMonoBehaviour, IUserControlledCapacity
{
	// Token: 0x17000867 RID: 2151
	// (get) Token: 0x06008329 RID: 33577 RVA: 0x000F6313 File Offset: 0x000F4513
	// (set) Token: 0x0600832A RID: 33578 RVA: 0x000F631B File Offset: 0x000F451B
	public float UserMaxCapacity
	{
		get
		{
			return this.userMaxCapacity;
		}
		set
		{
			this.userMaxCapacity = value;
			base.Trigger(-945020481, this);
		}
	}

	// Token: 0x17000868 RID: 2152
	// (get) Token: 0x0600832B RID: 33579 RVA: 0x000BCEBF File Offset: 0x000BB0BF
	public float MinCapacity
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000869 RID: 2153
	// (get) Token: 0x0600832C RID: 33580 RVA: 0x000F6330 File Offset: 0x000F4530
	public float MaxCapacity
	{
		get
		{
			return this.storage.capacityKg;
		}
	}

	// Token: 0x1700086A RID: 2154
	// (get) Token: 0x0600832D RID: 33581 RVA: 0x000F633D File Offset: 0x000F453D
	public float AmountStored
	{
		get
		{
			return this.storage.MassStored();
		}
	}

	// Token: 0x1700086B RID: 2155
	// (get) Token: 0x0600832E RID: 33582 RVA: 0x000AD2F7 File Offset: 0x000AB4F7
	public bool WholeValues
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700086C RID: 2156
	// (get) Token: 0x0600832F RID: 33583 RVA: 0x000C8D02 File Offset: 0x000C6F02
	public LocString CapacityUnits
	{
		get
		{
			return GameUtil.GetCurrentMassUnit(false);
		}
	}

	// Token: 0x1700086D RID: 2157
	// (get) Token: 0x06008330 RID: 33584 RVA: 0x000F634A File Offset: 0x000F454A
	public float RemainingCapacity
	{
		get
		{
			return this.userMaxCapacity - this.storage.MassStored();
		}
	}

	// Token: 0x06008331 RID: 33585 RVA: 0x000F635E File Offset: 0x000F455E
	protected override void OnPrefabInit()
	{
		this.userMaxCapacity = this.storage.capacityKg;
	}

	// Token: 0x06008332 RID: 33586 RVA: 0x0033F290 File Offset: 0x0033D490
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.GetComponent<KBatchedAnimController>().Play("grounded", KAnim.PlayMode.Loop, 1f, 0f);
		base.Subscribe<CargoBayCluster>(493375141, CargoBayCluster.OnRefreshUserMenuDelegate);
		this.meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_target",
			"meter_fill",
			"meter_frame",
			"meter_OL"
		});
		KBatchedAnimTracker component = this.meter.gameObject.GetComponent<KBatchedAnimTracker>();
		component.matchParentOffset = true;
		component.forceAlwaysAlive = true;
		this.OnStorageChange(null);
		base.Subscribe<CargoBayCluster>(-1697596308, CargoBayCluster.OnStorageChangeDelegate);
	}

	// Token: 0x06008333 RID: 33587 RVA: 0x0033F350 File Offset: 0x0033D550
	private void OnRefreshUserMenu(object data)
	{
		KIconButtonMenu.ButtonInfo button = new KIconButtonMenu.ButtonInfo("action_empty_contents", UI.USERMENUACTIONS.EMPTYSTORAGE.NAME, delegate()
		{
			this.storage.DropAll(false, false, default(Vector3), true, null);
		}, global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.EMPTYSTORAGE.TOOLTIP, true);
		Game.Instance.userMenu.AddButton(base.gameObject, button, 1f);
	}

	// Token: 0x06008334 RID: 33588 RVA: 0x000F6371 File Offset: 0x000F4571
	private void OnStorageChange(object data)
	{
		this.meter.SetPositionPercent(this.storage.MassStored() / this.storage.Capacity());
		this.UpdateCargoStatusItem();
	}

	// Token: 0x06008335 RID: 33589 RVA: 0x0033F3AC File Offset: 0x0033D5AC
	private void UpdateCargoStatusItem()
	{
		RocketModuleCluster component = base.GetComponent<RocketModuleCluster>();
		if (component == null)
		{
			return;
		}
		CraftModuleInterface craftInterface = component.CraftInterface;
		if (craftInterface == null)
		{
			return;
		}
		Clustercraft component2 = craftInterface.GetComponent<Clustercraft>();
		if (component2 == null)
		{
			return;
		}
		component2.UpdateStatusItem();
	}

	// Token: 0x0400637D RID: 25469
	private MeterController meter;

	// Token: 0x0400637E RID: 25470
	[SerializeField]
	public Storage storage;

	// Token: 0x0400637F RID: 25471
	[SerializeField]
	public CargoBay.CargoType storageType;

	// Token: 0x04006380 RID: 25472
	[Serialize]
	private float userMaxCapacity;

	// Token: 0x04006381 RID: 25473
	private static readonly EventSystem.IntraObjectHandler<CargoBayCluster> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<CargoBayCluster>(delegate(CargoBayCluster component, object data)
	{
		component.OnRefreshUserMenu(data);
	});

	// Token: 0x04006382 RID: 25474
	private static readonly EventSystem.IntraObjectHandler<CargoBayCluster> OnStorageChangeDelegate = new EventSystem.IntraObjectHandler<CargoBayCluster>(delegate(CargoBayCluster component, object data)
	{
		component.OnStorageChange(data);
	});
}
