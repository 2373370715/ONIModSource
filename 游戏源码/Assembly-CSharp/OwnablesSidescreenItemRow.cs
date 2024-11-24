using System;
using STRINGS;
using UnityEngine;

// Token: 0x02001FA0 RID: 8096
public class OwnablesSidescreenItemRow : KMonoBehaviour
{
	// Token: 0x17000AED RID: 2797
	// (get) Token: 0x0600AAF8 RID: 43768 RVA: 0x0010F145 File Offset: 0x0010D345
	// (set) Token: 0x0600AAF7 RID: 43767 RVA: 0x0010F13C File Offset: 0x0010D33C
	public bool IsLocked { get; private set; }

	// Token: 0x17000AEE RID: 2798
	// (get) Token: 0x0600AAF9 RID: 43769 RVA: 0x0010F14D File Offset: 0x0010D34D
	public bool SlotIsAssigned
	{
		get
		{
			return this.Slot != null && this.SlotInstance != null && !this.SlotInstance.IsUnassigning() && this.SlotInstance.IsAssigned();
		}
	}

	// Token: 0x17000AEF RID: 2799
	// (get) Token: 0x0600AAFB RID: 43771 RVA: 0x0010F182 File Offset: 0x0010D382
	// (set) Token: 0x0600AAFA RID: 43770 RVA: 0x0010F179 File Offset: 0x0010D379
	public AssignableSlotInstance SlotInstance { get; private set; }

	// Token: 0x17000AF0 RID: 2800
	// (get) Token: 0x0600AAFD RID: 43773 RVA: 0x0010F193 File Offset: 0x0010D393
	// (set) Token: 0x0600AAFC RID: 43772 RVA: 0x0010F18A File Offset: 0x0010D38A
	public AssignableSlot Slot { get; private set; }

	// Token: 0x17000AF1 RID: 2801
	// (get) Token: 0x0600AAFF RID: 43775 RVA: 0x0010F1A4 File Offset: 0x0010D3A4
	// (set) Token: 0x0600AAFE RID: 43774 RVA: 0x0010F19B File Offset: 0x0010D39B
	public Assignables Owner { get; private set; }

	// Token: 0x0600AB00 RID: 43776 RVA: 0x0010F1AC File Offset: 0x0010D3AC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		MultiToggle multiToggle = this.toggle;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(this.OnRowClicked));
		this.SetSelectedVisualState(false);
	}

	// Token: 0x0600AB01 RID: 43777 RVA: 0x0010F1E2 File Offset: 0x0010D3E2
	private void OnRowClicked()
	{
		Action<OwnablesSidescreenItemRow> onSlotRowClicked = this.OnSlotRowClicked;
		if (onSlotRowClicked == null)
		{
			return;
		}
		onSlotRowClicked(this);
	}

	// Token: 0x0600AB02 RID: 43778 RVA: 0x0010F1F5 File Offset: 0x0010D3F5
	public void SetLockState(bool locked)
	{
		this.IsLocked = locked;
		this.Refresh();
	}

	// Token: 0x0600AB03 RID: 43779 RVA: 0x00407B20 File Offset: 0x00405D20
	public void SetData(Assignables owner, AssignableSlot slot, bool IsLocked)
	{
		if (this.Owner != null)
		{
			this.ClearData();
		}
		this.Owner = owner;
		this.Slot = slot;
		this.SlotInstance = owner.GetSlot(slot);
		this.subscribe_IDX = this.Owner.Subscribe(-1585839766, delegate(object o)
		{
			this.Refresh();
		});
		this.SetLockState(IsLocked);
		if (!IsLocked)
		{
			this.Refresh();
		}
	}

	// Token: 0x0600AB04 RID: 43780 RVA: 0x00407B90 File Offset: 0x00405D90
	public void ClearData()
	{
		if (this.Owner != null && this.subscribe_IDX != -1)
		{
			this.Owner.Unsubscribe(this.subscribe_IDX);
		}
		this.Owner = null;
		this.Slot = null;
		this.SlotInstance = null;
		this.IsLocked = false;
		this.subscribe_IDX = -1;
		this.DisplayAsEmpty();
	}

	// Token: 0x0600AB05 RID: 43781 RVA: 0x0010F204 File Offset: 0x0010D404
	private void Refresh()
	{
		if (this.IsNullOrDestroyed())
		{
			return;
		}
		if (this.IsLocked)
		{
			this.DisplayAsLocked();
			return;
		}
		if (!this.SlotIsAssigned)
		{
			this.DisplayAsEmpty();
			return;
		}
		this.DisplayAsOccupied();
	}

	// Token: 0x0600AB06 RID: 43782 RVA: 0x00407BF0 File Offset: 0x00405DF0
	public void SetSelectedVisualState(bool shouldDisplayAsSelected)
	{
		int new_state_index = shouldDisplayAsSelected ? 1 : 0;
		this.toggle.ChangeState(new_state_index);
	}

	// Token: 0x0600AB07 RID: 43783 RVA: 0x00407C14 File Offset: 0x00405E14
	private void DisplayAsOccupied()
	{
		Assignable assignable = this.SlotInstance.assignable;
		string properName = assignable.GetProperName();
		string text = this.Slot.Name + ": " + properName;
		this.textLabel.SetText(text);
		this.itemIcon.sprite = Def.GetUISprite(assignable.gameObject, "ui", false).first;
		this.itemIcon.gameObject.SetActive(true);
		this.lockedIcon.gameObject.SetActive(false);
		InfoDescription component = assignable.gameObject.GetComponent<InfoDescription>();
		string simpleTooltip = string.Format(UI.UISIDESCREENS.OWNABLESSIDESCREEN.TOOLTIPS.ITEM_ASSIGNED_GENERIC, properName);
		if (component != null && !string.IsNullOrEmpty(component.description))
		{
			simpleTooltip = string.Format(UI.UISIDESCREENS.OWNABLESSIDESCREEN.TOOLTIPS.ITEM_ASSIGNED, properName, component.description);
		}
		this.tooltip.SetSimpleTooltip(simpleTooltip);
	}

	// Token: 0x0600AB08 RID: 43784 RVA: 0x00407CF4 File Offset: 0x00405EF4
	private void DisplayAsEmpty()
	{
		this.textLabel.SetText(((this.Slot != null) ? (this.Slot.Name + ": ") : "") + OwnablesSidescreenItemRow.EMPTY_TEXT);
		this.lockedIcon.gameObject.SetActive(false);
		this.itemIcon.sprite = null;
		this.itemIcon.gameObject.SetActive(false);
		this.tooltip.SetSimpleTooltip((this.Slot != null) ? string.Format(UI.UISIDESCREENS.OWNABLESSIDESCREEN.TOOLTIPS.NO_ITEM_ASSIGNED, this.Slot.Name) : null);
	}

	// Token: 0x0600AB09 RID: 43785 RVA: 0x00407D98 File Offset: 0x00405F98
	private void DisplayAsLocked()
	{
		this.lockedIcon.gameObject.SetActive(true);
		this.itemIcon.sprite = null;
		this.itemIcon.gameObject.SetActive(false);
		this.textLabel.SetText(string.Format(UI.UISIDESCREENS.OWNABLESSIDESCREEN.NO_APPLICABLE, this.Slot.Name));
		this.tooltip.SetSimpleTooltip(string.Format(UI.UISIDESCREENS.OWNABLESSIDESCREEN.TOOLTIPS.NO_APPLICABLE, this.Slot.Name));
	}

	// Token: 0x0600AB0A RID: 43786 RVA: 0x0010F233 File Offset: 0x0010D433
	protected override void OnCleanUp()
	{
		this.ClearData();
	}

	// Token: 0x0400865D RID: 34397
	private static string EMPTY_TEXT = UI.UISIDESCREENS.OWNABLESSIDESCREEN.NO_ITEM_ASSIGNED;

	// Token: 0x0400865E RID: 34398
	public KImage lockedIcon;

	// Token: 0x0400865F RID: 34399
	public KImage itemIcon;

	// Token: 0x04008660 RID: 34400
	public LocText textLabel;

	// Token: 0x04008661 RID: 34401
	public ToolTip tooltip;

	// Token: 0x04008662 RID: 34402
	[Header("Icon settings")]
	public KImage frameOuterBorder;

	// Token: 0x04008663 RID: 34403
	public Action<OwnablesSidescreenItemRow> OnSlotRowClicked;

	// Token: 0x04008668 RID: 34408
	public MultiToggle toggle;

	// Token: 0x04008669 RID: 34409
	private int subscribe_IDX = -1;
}
