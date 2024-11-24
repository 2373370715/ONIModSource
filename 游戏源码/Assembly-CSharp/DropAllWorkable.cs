using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200124C RID: 4684
[AddComponentMenu("KMonoBehaviour/Workable/DropAllWorkable")]
public class DropAllWorkable : Workable
{
	// Token: 0x170005C3 RID: 1475
	// (get) Token: 0x06005FF4 RID: 24564 RVA: 0x000DE86F File Offset: 0x000DCA6F
	// (set) Token: 0x06005FF5 RID: 24565 RVA: 0x000DE877 File Offset: 0x000DCA77
	private Chore Chore
	{
		get
		{
			return this._chore;
		}
		set
		{
			this._chore = value;
			this.markedForDrop = (this._chore != null);
		}
	}

	// Token: 0x06005FF6 RID: 24566 RVA: 0x000DE88F File Offset: 0x000DCA8F
	protected DropAllWorkable()
	{
		base.SetOffsetTable(OffsetGroups.InvertedStandardTable);
	}

	// Token: 0x06005FF7 RID: 24567 RVA: 0x002AC314 File Offset: 0x002AA514
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<DropAllWorkable>(493375141, DropAllWorkable.OnRefreshUserMenuDelegate);
		base.Subscribe<DropAllWorkable>(-1697596308, DropAllWorkable.OnStorageChangeDelegate);
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Emptying;
		this.synchronizeAnims = false;
		base.SetWorkTime(this.dropWorkTime);
		Prioritizable.AddRef(base.gameObject);
	}

	// Token: 0x06005FF8 RID: 24568 RVA: 0x000DE8AD File Offset: 0x000DCAAD
	private Storage[] GetStorages()
	{
		if (this.storages == null)
		{
			this.storages = base.GetComponents<Storage>();
		}
		return this.storages;
	}

	// Token: 0x06005FF9 RID: 24569 RVA: 0x000DE8C9 File Offset: 0x000DCAC9
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.showCmd = this.GetNewShowCmd();
		if (this.markedForDrop)
		{
			this.DropAll();
		}
	}

	// Token: 0x06005FFA RID: 24570 RVA: 0x002AC37C File Offset: 0x002AA57C
	public void DropAll()
	{
		if (DebugHandler.InstantBuildMode)
		{
			this.OnCompleteWork(null);
		}
		else if (this.Chore == null)
		{
			ChoreType chore_type = (!string.IsNullOrEmpty(this.choreTypeID)) ? Db.Get().ChoreTypes.Get(this.choreTypeID) : Db.Get().ChoreTypes.EmptyStorage;
			this.Chore = new WorkChore<DropAllWorkable>(chore_type, this, null, true, null, null, null, true, null, false, false, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
		}
		else
		{
			this.Chore.Cancel("Cancelled emptying");
			this.Chore = null;
			base.GetComponent<KSelectable>().RemoveStatusItem(this.workerStatusItem, false);
			base.ShowProgressBar(false);
		}
		this.RefreshStatusItem();
	}

	// Token: 0x06005FFB RID: 24571 RVA: 0x002AC430 File Offset: 0x002AA630
	protected override void OnCompleteWork(WorkerBase worker)
	{
		Storage[] array = this.GetStorages();
		for (int i = 0; i < array.Length; i++)
		{
			List<GameObject> list = new List<GameObject>(array[i].items);
			for (int j = 0; j < list.Count; j++)
			{
				GameObject gameObject = array[i].Drop(list[j], true);
				if (gameObject != null)
				{
					foreach (Tag tag in this.removeTags)
					{
						gameObject.RemoveTag(tag);
					}
					gameObject.Trigger(580035959, worker);
					if (this.resetTargetWorkableOnCompleteWork)
					{
						Pickupable component = gameObject.GetComponent<Pickupable>();
						component.targetWorkable = component;
						component.SetOffsetTable(OffsetGroups.InvertedStandardTable);
					}
				}
			}
		}
		this.Chore = null;
		this.RefreshStatusItem();
		base.Trigger(-1957399615, null);
	}

	// Token: 0x06005FFC RID: 24572 RVA: 0x002AC52C File Offset: 0x002AA72C
	private void OnRefreshUserMenu(object data)
	{
		if (this.showCmd)
		{
			KIconButtonMenu.ButtonInfo button = (this.Chore == null) ? new KIconButtonMenu.ButtonInfo("action_empty_contents", UI.USERMENUACTIONS.EMPTYSTORAGE.NAME, new System.Action(this.DropAll), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.EMPTYSTORAGE.TOOLTIP, true) : new KIconButtonMenu.ButtonInfo("action_empty_contents", UI.USERMENUACTIONS.EMPTYSTORAGE.NAME_OFF, new System.Action(this.DropAll), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.EMPTYSTORAGE.TOOLTIP_OFF, true);
			Game.Instance.userMenu.AddButton(base.gameObject, button, 1f);
		}
	}

	// Token: 0x06005FFD RID: 24573 RVA: 0x002AC5D0 File Offset: 0x002AA7D0
	private bool GetNewShowCmd()
	{
		bool flag = false;
		Storage[] array = this.GetStorages();
		for (int i = 0; i < array.Length; i++)
		{
			flag = (flag || !array[i].IsEmpty());
		}
		return flag;
	}

	// Token: 0x06005FFE RID: 24574 RVA: 0x002AC608 File Offset: 0x002AA808
	private void OnStorageChange(object data)
	{
		bool newShowCmd = this.GetNewShowCmd();
		if (newShowCmd != this.showCmd)
		{
			this.showCmd = newShowCmd;
			Game.Instance.userMenu.Refresh(base.gameObject);
		}
	}

	// Token: 0x06005FFF RID: 24575 RVA: 0x002AC644 File Offset: 0x002AA844
	private void RefreshStatusItem()
	{
		if (this.Chore != null && this.statusItem == Guid.Empty)
		{
			KSelectable component = base.GetComponent<KSelectable>();
			this.statusItem = component.AddStatusItem(Db.Get().BuildingStatusItems.AwaitingEmptyBuilding, null);
			return;
		}
		if (this.Chore == null && this.statusItem != Guid.Empty)
		{
			KSelectable component2 = base.GetComponent<KSelectable>();
			this.statusItem = component2.RemoveStatusItem(this.statusItem, false);
		}
	}

	// Token: 0x04004413 RID: 17427
	[Serialize]
	private bool markedForDrop;

	// Token: 0x04004414 RID: 17428
	private Chore _chore;

	// Token: 0x04004415 RID: 17429
	private bool showCmd;

	// Token: 0x04004416 RID: 17430
	private Storage[] storages;

	// Token: 0x04004417 RID: 17431
	public float dropWorkTime = 0.1f;

	// Token: 0x04004418 RID: 17432
	public string choreTypeID;

	// Token: 0x04004419 RID: 17433
	[MyCmpAdd]
	private Prioritizable _prioritizable;

	// Token: 0x0400441A RID: 17434
	public List<Tag> removeTags;

	// Token: 0x0400441B RID: 17435
	public bool resetTargetWorkableOnCompleteWork;

	// Token: 0x0400441C RID: 17436
	private static readonly EventSystem.IntraObjectHandler<DropAllWorkable> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<DropAllWorkable>(delegate(DropAllWorkable component, object data)
	{
		component.OnRefreshUserMenu(data);
	});

	// Token: 0x0400441D RID: 17437
	private static readonly EventSystem.IntraObjectHandler<DropAllWorkable> OnStorageChangeDelegate = new EventSystem.IntraObjectHandler<DropAllWorkable>(delegate(DropAllWorkable component, object data)
	{
		component.OnStorageChange(data);
	});

	// Token: 0x0400441E RID: 17438
	private Guid statusItem;
}
