using System;

// Token: 0x02001F9D RID: 8093
public class OwnablesSidescreenCategoryRow : KMonoBehaviour
{
	// Token: 0x17000AEC RID: 2796
	// (get) Token: 0x0600AAEB RID: 43755 RVA: 0x0010F0CD File Offset: 0x0010D2CD
	private AssignableSlot[] slots
	{
		get
		{
			return this.data.slots;
		}
	}

	// Token: 0x0600AAEC RID: 43756 RVA: 0x0010F0DA File Offset: 0x0010D2DA
	public void SetCategoryData(OwnablesSidescreenCategoryRow.Data categoryData)
	{
		this.DeleteAllRows();
		this.data = categoryData;
		this.titleLabel.text = categoryData.name;
	}

	// Token: 0x0600AAED RID: 43757 RVA: 0x0010F0FA File Offset: 0x0010D2FA
	public void SetOwner(Assignables owner)
	{
		this.owner = owner;
		if (owner != null)
		{
			this.RecreateAllItemRows();
			return;
		}
		this.DeleteAllRows();
	}

	// Token: 0x0600AAEE RID: 43758 RVA: 0x004078FC File Offset: 0x00405AFC
	private void RecreateAllItemRows()
	{
		this.DeleteAllRows();
		this.itemRows = new OwnablesSidescreenItemRow[this.slots.Length];
		IAssignableIdentity component = this.owner.gameObject.GetComponent<IAssignableIdentity>();
		for (int i = 0; i < this.slots.Length; i++)
		{
			AssignableSlot slot = this.slots[i];
			this.itemRows[i] = this.CreateRow(slot, component);
		}
	}

	// Token: 0x0600AAEF RID: 43759 RVA: 0x00407960 File Offset: 0x00405B60
	private OwnablesSidescreenItemRow CreateRow(AssignableSlot slot, IAssignableIdentity ownerIdentity)
	{
		this.originalItemRow.gameObject.SetActive(false);
		OwnablesSidescreenItemRow component = Util.KInstantiateUI(this.originalItemRow.gameObject, this.originalItemRow.transform.parent.gameObject, false).GetComponent<OwnablesSidescreenItemRow>();
		component.OnSlotRowClicked = (Action<OwnablesSidescreenItemRow>)Delegate.Combine(component.OnSlotRowClicked, new Action<OwnablesSidescreenItemRow>(this.OnRowClicked));
		component.gameObject.SetActive(true);
		component.SetData(this.owner, slot, !this.data.IsSlotApplicable(ownerIdentity, slot));
		return component;
	}

	// Token: 0x0600AAF0 RID: 43760 RVA: 0x0010F119 File Offset: 0x0010D319
	private void OnRowClicked(OwnablesSidescreenItemRow row)
	{
		Action<OwnablesSidescreenItemRow> onSlotRowClicked = this.OnSlotRowClicked;
		if (onSlotRowClicked == null)
		{
			return;
		}
		onSlotRowClicked(row);
	}

	// Token: 0x0600AAF1 RID: 43761 RVA: 0x004079F4 File Offset: 0x00405BF4
	private void DeleteAllRows()
	{
		this.originalItemRow.gameObject.SetActive(false);
		if (this.itemRows != null)
		{
			for (int i = 0; i < this.itemRows.Length; i++)
			{
				this.itemRows[i].ClearData();
				this.itemRows[i].DeleteObject();
			}
			this.itemRows = null;
		}
	}

	// Token: 0x0600AAF2 RID: 43762 RVA: 0x00407A50 File Offset: 0x00405C50
	public void SetSelectedRow_VisualsOnly(AssignableSlotInstance slotInstance)
	{
		if (this.itemRows == null)
		{
			return;
		}
		for (int i = 0; i < this.itemRows.Length; i++)
		{
			OwnablesSidescreenItemRow ownablesSidescreenItemRow = this.itemRows[i];
			ownablesSidescreenItemRow.SetSelectedVisualState(ownablesSidescreenItemRow.SlotInstance == slotInstance);
		}
	}

	// Token: 0x04008652 RID: 34386
	public Action<OwnablesSidescreenItemRow> OnSlotRowClicked;

	// Token: 0x04008653 RID: 34387
	public LocText titleLabel;

	// Token: 0x04008654 RID: 34388
	public OwnablesSidescreenItemRow originalItemRow;

	// Token: 0x04008655 RID: 34389
	private Assignables owner;

	// Token: 0x04008656 RID: 34390
	private OwnablesSidescreenCategoryRow.Data data;

	// Token: 0x04008657 RID: 34391
	private OwnablesSidescreenItemRow[] itemRows;

	// Token: 0x02001F9E RID: 8094
	public struct AssignableSlotData
	{
		// Token: 0x0600AAF4 RID: 43764 RVA: 0x0010F12C File Offset: 0x0010D32C
		public AssignableSlotData(AssignableSlot slot, Func<IAssignableIdentity, bool> isApplicableCallback)
		{
			this.slot = slot;
			this.IsApplicableCallback = isApplicableCallback;
		}

		// Token: 0x04008658 RID: 34392
		public AssignableSlot slot;

		// Token: 0x04008659 RID: 34393
		public Func<IAssignableIdentity, bool> IsApplicableCallback;
	}

	// Token: 0x02001F9F RID: 8095
	public struct Data
	{
		// Token: 0x0600AAF5 RID: 43765 RVA: 0x00407A90 File Offset: 0x00405C90
		public Data(string name, OwnablesSidescreenCategoryRow.AssignableSlotData[] slotsData)
		{
			this.name = name;
			this.slotsData = slotsData;
			this.slots = new AssignableSlot[slotsData.Length];
			for (int i = 0; i < slotsData.Length; i++)
			{
				this.slots[i] = slotsData[i].slot;
			}
		}

		// Token: 0x0600AAF6 RID: 43766 RVA: 0x00407ADC File Offset: 0x00405CDC
		public bool IsSlotApplicable(IAssignableIdentity identity, AssignableSlot slot)
		{
			for (int i = 0; i < this.slotsData.Length; i++)
			{
				OwnablesSidescreenCategoryRow.AssignableSlotData assignableSlotData = this.slotsData[i];
				if (assignableSlotData.slot == slot)
				{
					return assignableSlotData.IsApplicableCallback(identity);
				}
			}
			return false;
		}

		// Token: 0x0400865A RID: 34394
		public string name;

		// Token: 0x0400865B RID: 34395
		public AssignableSlot[] slots;

		// Token: 0x0400865C RID: 34396
		private OwnablesSidescreenCategoryRow.AssignableSlotData[] slotsData;
	}
}
