using System;
using UnityEngine;

// Token: 0x02001A16 RID: 6678
public abstract class UtilityNetworkLink : KMonoBehaviour
{
	// Token: 0x06008B19 RID: 35609 RVA: 0x000FAFD4 File Offset: 0x000F91D4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<UtilityNetworkLink>(774203113, UtilityNetworkLink.OnBuildingBrokenDelegate);
		base.Subscribe<UtilityNetworkLink>(-1735440190, UtilityNetworkLink.OnBuildingFullyRepairedDelegate);
		this.Connect();
	}

	// Token: 0x06008B1A RID: 35610 RVA: 0x000FB004 File Offset: 0x000F9204
	protected override void OnCleanUp()
	{
		base.Unsubscribe<UtilityNetworkLink>(774203113, UtilityNetworkLink.OnBuildingBrokenDelegate, false);
		base.Unsubscribe<UtilityNetworkLink>(-1735440190, UtilityNetworkLink.OnBuildingFullyRepairedDelegate, false);
		this.Disconnect();
		base.OnCleanUp();
	}

	// Token: 0x06008B1B RID: 35611 RVA: 0x0035E568 File Offset: 0x0035C768
	protected void Connect()
	{
		if (!this.visualizeOnly && !this.connected)
		{
			this.connected = true;
			int cell;
			int cell2;
			this.GetCells(out cell, out cell2);
			this.OnConnect(cell, cell2);
		}
	}

	// Token: 0x06008B1C RID: 35612 RVA: 0x000A5E40 File Offset: 0x000A4040
	protected virtual void OnConnect(int cell1, int cell2)
	{
	}

	// Token: 0x06008B1D RID: 35613 RVA: 0x0035E5A0 File Offset: 0x0035C7A0
	protected void Disconnect()
	{
		if (!this.visualizeOnly && this.connected)
		{
			this.connected = false;
			int cell;
			int cell2;
			this.GetCells(out cell, out cell2);
			this.OnDisconnect(cell, cell2);
		}
	}

	// Token: 0x06008B1E RID: 35614 RVA: 0x000A5E40 File Offset: 0x000A4040
	protected virtual void OnDisconnect(int cell1, int cell2)
	{
	}

	// Token: 0x06008B1F RID: 35615 RVA: 0x0035E5D8 File Offset: 0x0035C7D8
	public void GetCells(out int linked_cell1, out int linked_cell2)
	{
		Building component = base.GetComponent<Building>();
		if (component != null)
		{
			Orientation orientation = component.Orientation;
			int cell = Grid.PosToCell(base.transform.GetPosition());
			this.GetCells(cell, orientation, out linked_cell1, out linked_cell2);
			return;
		}
		linked_cell1 = -1;
		linked_cell2 = -1;
	}

	// Token: 0x06008B20 RID: 35616 RVA: 0x0035E620 File Offset: 0x0035C820
	public void GetCells(int cell, Orientation orientation, out int linked_cell1, out int linked_cell2)
	{
		CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(this.link1, orientation);
		CellOffset rotatedCellOffset2 = Rotatable.GetRotatedCellOffset(this.link2, orientation);
		linked_cell1 = Grid.OffsetCell(cell, rotatedCellOffset);
		linked_cell2 = Grid.OffsetCell(cell, rotatedCellOffset2);
	}

	// Token: 0x06008B21 RID: 35617 RVA: 0x0035E65C File Offset: 0x0035C85C
	public bool AreCellsValid(int cell, Orientation orientation)
	{
		CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(this.link1, orientation);
		CellOffset rotatedCellOffset2 = Rotatable.GetRotatedCellOffset(this.link2, orientation);
		return Grid.IsCellOffsetValid(cell, rotatedCellOffset) && Grid.IsCellOffsetValid(cell, rotatedCellOffset2);
	}

	// Token: 0x06008B22 RID: 35618 RVA: 0x000FB034 File Offset: 0x000F9234
	private void OnBuildingBroken(object data)
	{
		this.Disconnect();
	}

	// Token: 0x06008B23 RID: 35619 RVA: 0x000FB03C File Offset: 0x000F923C
	private void OnBuildingFullyRepaired(object data)
	{
		this.Connect();
	}

	// Token: 0x06008B24 RID: 35620 RVA: 0x0035E698 File Offset: 0x0035C898
	public int GetNetworkCell()
	{
		int result;
		int num;
		this.GetCells(out result, out num);
		return result;
	}

	// Token: 0x040068B5 RID: 26805
	[MyCmpGet]
	private Rotatable rotatable;

	// Token: 0x040068B6 RID: 26806
	[SerializeField]
	public CellOffset link1;

	// Token: 0x040068B7 RID: 26807
	[SerializeField]
	public CellOffset link2;

	// Token: 0x040068B8 RID: 26808
	[SerializeField]
	public bool visualizeOnly;

	// Token: 0x040068B9 RID: 26809
	private bool connected;

	// Token: 0x040068BA RID: 26810
	private static readonly EventSystem.IntraObjectHandler<UtilityNetworkLink> OnBuildingBrokenDelegate = new EventSystem.IntraObjectHandler<UtilityNetworkLink>(delegate(UtilityNetworkLink component, object data)
	{
		component.OnBuildingBroken(data);
	});

	// Token: 0x040068BB RID: 26811
	private static readonly EventSystem.IntraObjectHandler<UtilityNetworkLink> OnBuildingFullyRepairedDelegate = new EventSystem.IntraObjectHandler<UtilityNetworkLink>(delegate(UtilityNetworkLink component, object data)
	{
		component.OnBuildingFullyRepaired(data);
	});
}
