using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001F96 RID: 8086
public class OwnablesSecondSideScreen : KScreen
{
	// Token: 0x17000AE5 RID: 2789
	// (get) Token: 0x0600AAAC RID: 43692 RVA: 0x0010ED51 File Offset: 0x0010CF51
	// (set) Token: 0x0600AAAB RID: 43691 RVA: 0x0010ED48 File Offset: 0x0010CF48
	public AssignableSlotInstance Slot { get; private set; }

	// Token: 0x17000AE6 RID: 2790
	// (get) Token: 0x0600AAAE RID: 43694 RVA: 0x0010ED62 File Offset: 0x0010CF62
	// (set) Token: 0x0600AAAD RID: 43693 RVA: 0x0010ED59 File Offset: 0x0010CF59
	public IAssignableIdentity OwnerIdentity { get; private set; }

	// Token: 0x17000AE7 RID: 2791
	// (get) Token: 0x0600AAAF RID: 43695 RVA: 0x0010ED6A File Offset: 0x0010CF6A
	public AssignableSlot SlotType
	{
		get
		{
			if (this.Slot != null)
			{
				return this.Slot.slot;
			}
			return null;
		}
	}

	// Token: 0x17000AE8 RID: 2792
	// (get) Token: 0x0600AAB0 RID: 43696 RVA: 0x0010ED81 File Offset: 0x0010CF81
	public Assignable CurrentSlotItem
	{
		get
		{
			if (!this.HasItem)
			{
				return null;
			}
			return this.Slot.assignable;
		}
	}

	// Token: 0x17000AE9 RID: 2793
	// (get) Token: 0x0600AAB1 RID: 43697 RVA: 0x0010ED98 File Offset: 0x0010CF98
	public bool HasItem
	{
		get
		{
			return this.Slot != null && this.Slot.IsAssigned();
		}
	}

	// Token: 0x0600AAB2 RID: 43698 RVA: 0x0010EDAF File Offset: 0x0010CFAF
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.originalRow.gameObject.SetActive(false);
		MultiToggle multiToggle = this.noneRow;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(this.OnNoneRowClicked));
	}

	// Token: 0x0600AAB3 RID: 43699 RVA: 0x0010EDEF File Offset: 0x0010CFEF
	private void OnNoneRowClicked()
	{
		this.UnassignCurrentItem();
		this.RefreshNoneRow();
	}

	// Token: 0x0600AAB4 RID: 43700 RVA: 0x0010EDFD File Offset: 0x0010CFFD
	protected override void OnCmpDisable()
	{
		this.SetSlot(null);
		base.OnCmpDisable();
	}

	// Token: 0x0600AAB5 RID: 43701 RVA: 0x00406C98 File Offset: 0x00404E98
	public void SetSlot(AssignableSlotInstance slot)
	{
		Components.AssignableItems.Unregister(new Action<Assignable>(this.OnNewItemAvailable), new Action<Assignable>(this.OnItemUnregistered));
		this.Slot = slot;
		this.OwnerIdentity = ((slot == null) ? null : slot.assignables.GetComponent<IAssignableIdentity>());
		if (this.Slot != null)
		{
			Components.AssignableItems.Register(new Action<Assignable>(this.OnNewItemAvailable), new Action<Assignable>(this.OnItemUnregistered));
		}
		this.RefreshItemListOptions();
	}

	// Token: 0x0600AAB6 RID: 43702 RVA: 0x00406D18 File Offset: 0x00404F18
	public void SortRows()
	{
		if (this.itemRows != null)
		{
			OwnablesSecondSideScreenRow ownablesSecondSideScreenRow = null;
			for (int i = 0; i < this.itemRows.Count; i++)
			{
				OwnablesSecondSideScreenRow ownablesSecondSideScreenRow2 = this.itemRows[i];
				if (ownablesSecondSideScreenRow2.item == null || ownablesSecondSideScreenRow2.item.IsAssigned())
				{
					if (ownablesSecondSideScreenRow == null && ownablesSecondSideScreenRow2 != null && ownablesSecondSideScreenRow2.item != null && ownablesSecondSideScreenRow2.item.IsAssigned() && ownablesSecondSideScreenRow2.item == this.CurrentSlotItem)
					{
						ownablesSecondSideScreenRow = ownablesSecondSideScreenRow2;
					}
					else
					{
						ownablesSecondSideScreenRow2.transform.SetAsLastSibling();
					}
				}
			}
			if (ownablesSecondSideScreenRow != null)
			{
				ownablesSecondSideScreenRow.transform.SetAsFirstSibling();
			}
		}
		this.noneRow.transform.SetAsFirstSibling();
	}

	// Token: 0x0600AAB7 RID: 43703 RVA: 0x00406DE8 File Offset: 0x00404FE8
	public void RefreshItemListOptions()
	{
		GameObject gameObject = (this.OwnerIdentity == null) ? null : this.OwnerIdentity.GetOwners()[0].GetComponent<MinionAssignablesProxy>().GetTargetGameObject();
		int worldID = (this.OwnerIdentity == null) ? 255 : gameObject.GetMyWorldId();
		List<Assignable> list = null;
		int b = 0;
		if (worldID != 255)
		{
			list = Components.AssignableItems.Items.FindAll(delegate(Assignable i)
			{
				bool flag = i.slotID == this.SlotType.Id && i.CanAssignTo(this.OwnerIdentity);
				if (flag && i is Equippable)
				{
					Equippable equippable = i as Equippable;
					GameObject gameObject2 = equippable.gameObject;
					if (equippable.isEquipped)
					{
						gameObject2 = equippable.assignee.GetOwners()[0].GetComponent<MinionAssignablesProxy>().GetTargetGameObject();
					}
					flag = (gameObject2.GetMyWorldId() == worldID);
				}
				return flag;
			});
			b = list.Count;
		}
		for (int j = 0; j < Mathf.Max(this.itemRows.Count, b); j++)
		{
			if (list != null && j < list.Count)
			{
				Assignable assignable = list[j];
				if (j >= this.itemRows.Count)
				{
					OwnablesSecondSideScreenRow item = this.CreateItemRow(assignable);
					this.itemRows.Add(item);
				}
				OwnablesSecondSideScreenRow ownablesSecondSideScreenRow = this.itemRows[j];
				ownablesSecondSideScreenRow.gameObject.SetActive(true);
				ownablesSecondSideScreenRow.SetData(this.Slot, assignable);
			}
			else
			{
				OwnablesSecondSideScreenRow ownablesSecondSideScreenRow2 = this.itemRows[j];
				ownablesSecondSideScreenRow2.ClearData();
				ownablesSecondSideScreenRow2.gameObject.SetActive(false);
			}
		}
		this.SortRows();
		this.RefreshNoneRow();
	}

	// Token: 0x0600AAB8 RID: 43704 RVA: 0x0010EE0C File Offset: 0x0010D00C
	private void RefreshNoneRow()
	{
		this.noneRow.ChangeState(this.HasItem ? 0 : 1);
	}

	// Token: 0x0600AAB9 RID: 43705 RVA: 0x00406F30 File Offset: 0x00405130
	private OwnablesSecondSideScreenRow CreateItemRow(Assignable item)
	{
		OwnablesSecondSideScreenRow component = Util.KInstantiateUI(this.originalRow.gameObject, this.originalRow.transform.parent.gameObject, false).GetComponent<OwnablesSecondSideScreenRow>();
		component.OnRowClicked = (Action<OwnablesSecondSideScreenRow>)Delegate.Combine(component.OnRowClicked, new Action<OwnablesSecondSideScreenRow>(this.OnItemRowClicked));
		component.OnRowItemAssigneeChanged = (Action<OwnablesSecondSideScreenRow>)Delegate.Combine(component.OnRowItemAssigneeChanged, new Action<OwnablesSecondSideScreenRow>(this.OnItemRowAsigneeChanged));
		component.OnRowItemDestroyed = (Action<OwnablesSecondSideScreenRow>)Delegate.Combine(component.OnRowItemDestroyed, new Action<OwnablesSecondSideScreenRow>(this.OnItemDestroyed));
		return component;
	}

	// Token: 0x0600AABA RID: 43706 RVA: 0x0010EE25 File Offset: 0x0010D025
	private void OnItemDestroyed(OwnablesSecondSideScreenRow correspondingItemRow)
	{
		correspondingItemRow.ClearData();
		correspondingItemRow.gameObject.SetActive(false);
	}

	// Token: 0x0600AABB RID: 43707 RVA: 0x0010EE39 File Offset: 0x0010D039
	private void OnItemRowAsigneeChanged(OwnablesSecondSideScreenRow correspondingItemRow)
	{
		correspondingItemRow.Refresh();
		this.SortRows();
		this.RefreshNoneRow();
	}

	// Token: 0x0600AABC RID: 43708 RVA: 0x00406FD0 File Offset: 0x004051D0
	private void OnItemRowClicked(OwnablesSecondSideScreenRow rowClicked)
	{
		Assignable item = rowClicked.item;
		bool flag = item.IsAssigned() && item.assignee is AssignmentGroup;
		bool flag2 = item.IsAssigned() && item.IsAssignedTo(this.OwnerIdentity) && !flag && this.Slot.IsAssigned() && this.Slot.assignable == item;
		if (item.IsAssigned())
		{
			item.Unassign();
		}
		if (!flag2)
		{
			item.Assign(this.OwnerIdentity, this.Slot);
		}
		rowClicked.Refresh();
		this.RefreshNoneRow();
	}

	// Token: 0x0600AABD RID: 43709 RVA: 0x0010EE4D File Offset: 0x0010D04D
	private void UnassignCurrentItem()
	{
		if (this.Slot != null)
		{
			this.Slot.Unassign(true);
			this.RefreshItemListOptions();
		}
	}

	// Token: 0x0600AABE RID: 43710 RVA: 0x0010EE69 File Offset: 0x0010D069
	private void OnNewItemAvailable(Assignable item)
	{
		if (this.Slot != null && item.slotID == this.SlotType.Id)
		{
			this.RefreshItemListOptions();
		}
	}

	// Token: 0x0600AABF RID: 43711 RVA: 0x0010EE69 File Offset: 0x0010D069
	private void OnItemUnregistered(Assignable item)
	{
		if (this.Slot != null && item.slotID == this.SlotType.Id)
		{
			this.RefreshItemListOptions();
		}
	}

	// Token: 0x04008625 RID: 34341
	public MultiToggle noneRow;

	// Token: 0x04008626 RID: 34342
	public OwnablesSecondSideScreenRow originalRow;

	// Token: 0x04008629 RID: 34345
	public System.Action OnScreenDeactivated;

	// Token: 0x0400862A RID: 34346
	private List<OwnablesSecondSideScreenRow> itemRows = new List<OwnablesSecondSideScreenRow>();
}
