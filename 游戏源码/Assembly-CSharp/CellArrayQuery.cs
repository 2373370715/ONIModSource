using System;

// Token: 0x020007F7 RID: 2039
public class CellArrayQuery : PathFinderQuery
{
	// Token: 0x06002477 RID: 9335 RVA: 0x000B7BC5 File Offset: 0x000B5DC5
	public CellArrayQuery Reset(int[] target_cells)
	{
		this.targetCells = target_cells;
		return this;
	}

	// Token: 0x06002478 RID: 9336 RVA: 0x001C9BF8 File Offset: 0x001C7DF8
	public override bool IsMatch(int cell, int parent_cell, int cost)
	{
		for (int i = 0; i < this.targetCells.Length; i++)
		{
			if (this.targetCells[i] == cell)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0400188C RID: 6284
	private int[] targetCells;
}
