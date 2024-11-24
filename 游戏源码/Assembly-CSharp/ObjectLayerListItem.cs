using System;
using UnityEngine;

// Token: 0x02001674 RID: 5748
public class ObjectLayerListItem
{
	// Token: 0x1700077A RID: 1914
	// (get) Token: 0x060076AF RID: 30383 RVA: 0x000EDFC9 File Offset: 0x000EC1C9
	// (set) Token: 0x060076B0 RID: 30384 RVA: 0x000EDFD1 File Offset: 0x000EC1D1
	public ObjectLayerListItem previousItem { get; private set; }

	// Token: 0x1700077B RID: 1915
	// (get) Token: 0x060076B1 RID: 30385 RVA: 0x000EDFDA File Offset: 0x000EC1DA
	// (set) Token: 0x060076B2 RID: 30386 RVA: 0x000EDFE2 File Offset: 0x000EC1E2
	public ObjectLayerListItem nextItem { get; private set; }

	// Token: 0x1700077C RID: 1916
	// (get) Token: 0x060076B3 RID: 30387 RVA: 0x000EDFEB File Offset: 0x000EC1EB
	// (set) Token: 0x060076B4 RID: 30388 RVA: 0x000EDFF3 File Offset: 0x000EC1F3
	public GameObject gameObject { get; private set; }

	// Token: 0x060076B5 RID: 30389 RVA: 0x000EDFFC File Offset: 0x000EC1FC
	public ObjectLayerListItem(GameObject gameObject, ObjectLayer layer, int new_cell)
	{
		this.gameObject = gameObject;
		this.layer = layer;
		this.Refresh(new_cell);
	}

	// Token: 0x060076B6 RID: 30390 RVA: 0x000EE025 File Offset: 0x000EC225
	public void Clear()
	{
		this.Refresh(Grid.InvalidCell);
	}

	// Token: 0x060076B7 RID: 30391 RVA: 0x0030A04C File Offset: 0x0030824C
	public bool Refresh(int new_cell)
	{
		if (this.cell != new_cell)
		{
			if (this.cell != Grid.InvalidCell && Grid.Objects[this.cell, (int)this.layer] == this.gameObject)
			{
				GameObject value = null;
				if (this.nextItem != null && this.nextItem.gameObject != null)
				{
					value = this.nextItem.gameObject;
				}
				Grid.Objects[this.cell, (int)this.layer] = value;
			}
			if (this.previousItem != null)
			{
				this.previousItem.nextItem = this.nextItem;
			}
			if (this.nextItem != null)
			{
				this.nextItem.previousItem = this.previousItem;
			}
			this.previousItem = null;
			this.nextItem = null;
			this.cell = new_cell;
			if (this.cell != Grid.InvalidCell)
			{
				GameObject gameObject = Grid.Objects[this.cell, (int)this.layer];
				if (gameObject != null && gameObject != this.gameObject)
				{
					ObjectLayerListItem objectLayerListItem = gameObject.GetComponent<Pickupable>().objectLayerListItem;
					this.nextItem = objectLayerListItem;
					objectLayerListItem.previousItem = this;
				}
				Grid.Objects[this.cell, (int)this.layer] = this.gameObject;
			}
			return true;
		}
		return false;
	}

	// Token: 0x060076B8 RID: 30392 RVA: 0x000EE033 File Offset: 0x000EC233
	public bool Update(int cell)
	{
		return this.Refresh(cell);
	}

	// Token: 0x040058D0 RID: 22736
	private int cell = Grid.InvalidCell;

	// Token: 0x040058D1 RID: 22737
	private ObjectLayer layer;
}
