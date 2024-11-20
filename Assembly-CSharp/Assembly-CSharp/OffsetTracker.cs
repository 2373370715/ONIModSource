using System;
using UnityEngine;

public class OffsetTracker
{
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

	public void ForceRefresh()
	{
		int cell = this.previousCell;
		this.previousCell = Grid.InvalidCell;
		this.Refresh(cell);
	}

	public void Refresh(int cell)
	{
		this.GetOffsets(cell);
	}

	protected virtual void UpdateCell(int previous_cell, int current_cell)
	{
	}

	protected virtual void UpdateOffsets(int current_cell)
	{
	}

	public virtual void Clear()
	{
	}

	public virtual void DebugDrawExtents()
	{
	}

	public virtual void DebugDrawEditor()
	{
	}

	public virtual void DebugDrawOffsets(int cell)
	{
		foreach (CellOffset offset in this.GetOffsets(cell))
		{
			int cell2 = Grid.OffsetCell(cell, offset);
			Gizmos.color = new Color(0f, 1f, 0f, 0.25f);
			Gizmos.DrawWireCube(Grid.CellToPosCCC(cell2, Grid.SceneLayer.Move), new Vector3(0.95f, 0.95f, 0.95f));
		}
	}

	public static bool isExecutingWithinJob;

	protected CellOffset[] offsets;

	protected int previousCell = Grid.InvalidCell;
}
