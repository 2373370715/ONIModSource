using System;

// Token: 0x020007FE RID: 2046
public class IdleSuitMarkerCellQuery : PathFinderQuery
{
	// Token: 0x0600248F RID: 9359 RVA: 0x000B7CC5 File Offset: 0x000B5EC5
	public IdleSuitMarkerCellQuery(bool is_rotated, int marker_x)
	{
		this.targetCell = Grid.InvalidCell;
		this.isRotated = is_rotated;
		this.markerX = marker_x;
	}

	// Token: 0x06002490 RID: 9360 RVA: 0x001C9DC4 File Offset: 0x001C7FC4
	public override bool IsMatch(int cell, int parent_cell, int cost)
	{
		if (!Grid.PreventIdleTraversal[cell] && Grid.CellToXY(cell).x < this.markerX != this.isRotated)
		{
			this.targetCell = cell;
		}
		return this.targetCell != Grid.InvalidCell;
	}

	// Token: 0x06002491 RID: 9361 RVA: 0x000B7CE6 File Offset: 0x000B5EE6
	public override int GetResultCell()
	{
		return this.targetCell;
	}

	// Token: 0x04001897 RID: 6295
	private int targetCell;

	// Token: 0x04001898 RID: 6296
	private bool isRotated;

	// Token: 0x04001899 RID: 6297
	private int markerX;
}
