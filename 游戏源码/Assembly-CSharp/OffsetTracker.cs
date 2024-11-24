using System;
using UnityEngine;

// Token: 0x0200167C RID: 5756
public class OffsetTracker
{
	// Token: 0x060076EE RID: 30446 RVA: 0x0030C38C File Offset: 0x0030A58C
	public virtual CellOffset[] GetOffsets(int current_cell)
	{
		if (current_cell != this.previousCell)
		{
			global::Debug.Assert(!OffsetTracker.isExecutingWithinJob, "OffsetTracker.GetOffsets() is making a mutating call but is currently executing within a job");
			this.UpdateCell(this.previousCell, current_cell);
			this.previousCell = current_cell;
		}
		if (this.offsets == null)
		{
			global::Debug.Assert(!OffsetTracker.isExecutingWithinJob, "OffsetTracker.GetOffsets() is making a mutating call but is currently executing within a job");
			this.UpdateOffsets(this.previousCell);
		}
		return this.offsets;
	}

	// Token: 0x060076EF RID: 30447 RVA: 0x000EE227 File Offset: 0x000EC427
	public virtual bool ValidateOffsets(int current_cell)
	{
		return current_cell == this.previousCell && this.offsets != null;
	}

	// Token: 0x060076F0 RID: 30448 RVA: 0x0030C3F4 File Offset: 0x0030A5F4
	public void ForceRefresh()
	{
		int cell = this.previousCell;
		this.previousCell = Grid.InvalidCell;
		this.Refresh(cell);
	}

	// Token: 0x060076F1 RID: 30449 RVA: 0x000EE23D File Offset: 0x000EC43D
	public void Refresh(int cell)
	{
		this.GetOffsets(cell);
	}

	// Token: 0x060076F2 RID: 30450 RVA: 0x000A5E40 File Offset: 0x000A4040
	protected virtual void UpdateCell(int previous_cell, int current_cell)
	{
	}

	// Token: 0x060076F3 RID: 30451 RVA: 0x000A5E40 File Offset: 0x000A4040
	protected virtual void UpdateOffsets(int current_cell)
	{
	}

	// Token: 0x060076F4 RID: 30452 RVA: 0x000A5E40 File Offset: 0x000A4040
	public virtual void Clear()
	{
	}

	// Token: 0x060076F5 RID: 30453 RVA: 0x000A5E40 File Offset: 0x000A4040
	public virtual void DebugDrawExtents()
	{
	}

	// Token: 0x060076F6 RID: 30454 RVA: 0x000A5E40 File Offset: 0x000A4040
	public virtual void DebugDrawEditor()
	{
	}

	// Token: 0x060076F7 RID: 30455 RVA: 0x0030C41C File Offset: 0x0030A61C
	public virtual void DebugDrawOffsets(int cell)
	{
		foreach (CellOffset offset in this.GetOffsets(cell))
		{
			int cell2 = Grid.OffsetCell(cell, offset);
			Gizmos.color = new Color(0f, 1f, 0f, 0.25f);
			Gizmos.DrawWireCube(Grid.CellToPosCCC(cell2, Grid.SceneLayer.Move), new Vector3(0.95f, 0.95f, 0.95f));
		}
	}

	// Token: 0x040058F7 RID: 22775
	public static bool isExecutingWithinJob;

	// Token: 0x040058F8 RID: 22776
	protected CellOffset[] offsets;

	// Token: 0x040058F9 RID: 22777
	protected int previousCell = Grid.InvalidCell;
}
