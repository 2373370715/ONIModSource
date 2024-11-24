using System;
using System.Collections.Generic;

// Token: 0x020007FF RID: 2047
public class MineableCellQuery : PathFinderQuery
{
	// Token: 0x06002492 RID: 9362 RVA: 0x000B7CEE File Offset: 0x000B5EEE
	public MineableCellQuery Reset(Tag element, int max_results)
	{
		this.element = element;
		this.max_results = max_results;
		this.result_cells.Clear();
		return this;
	}

	// Token: 0x06002493 RID: 9363 RVA: 0x001C9E10 File Offset: 0x001C8010
	public override bool IsMatch(int cell, int parent_cell, int cost)
	{
		if (!this.result_cells.Contains(cell) && this.CheckValidMineCell(this.element, cell))
		{
			this.result_cells.Add(cell);
		}
		return this.result_cells.Count >= this.max_results;
	}

	// Token: 0x06002494 RID: 9364 RVA: 0x001C9E5C File Offset: 0x001C805C
	private bool CheckValidMineCell(Tag element, int testCell)
	{
		if (!Grid.IsValidCell(testCell))
		{
			return false;
		}
		foreach (Direction d in MineableCellQuery.DIRECTION_CHECKS)
		{
			int cellInDirection = Grid.GetCellInDirection(testCell, d);
			if (Grid.IsValidCell(cellInDirection) && Grid.IsSolidCell(cellInDirection) && !Grid.Foundation[cellInDirection] && Grid.Element[cellInDirection].tag == element)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0400189A RID: 6298
	public List<int> result_cells = new List<int>();

	// Token: 0x0400189B RID: 6299
	private Tag element;

	// Token: 0x0400189C RID: 6300
	private int max_results;

	// Token: 0x0400189D RID: 6301
	public static List<Direction> DIRECTION_CHECKS = new List<Direction>
	{
		Direction.Down,
		Direction.Right,
		Direction.Left,
		Direction.Up
	};
}
