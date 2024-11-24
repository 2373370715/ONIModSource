using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020007F6 RID: 2038
public class BuildingPlacementQuery : PathFinderQuery
{
	// Token: 0x06002473 RID: 9331 RVA: 0x000B7B4A File Offset: 0x000B5D4A
	public BuildingPlacementQuery Reset(int max_results, GameObject toPlace)
	{
		this.max_results = max_results;
		this.toPlace = toPlace;
		this.cellOffsets = toPlace.GetComponent<OccupyArea>().OccupiedCellsOffsets;
		this.result_cells.Clear();
		return this;
	}

	// Token: 0x06002474 RID: 9332 RVA: 0x000B7B77 File Offset: 0x000B5D77
	public override bool IsMatch(int cell, int parent_cell, int cost)
	{
		if (!this.result_cells.Contains(cell) && this.CheckValidPlaceCell(cell))
		{
			this.result_cells.Add(cell);
		}
		return this.result_cells.Count >= this.max_results;
	}

	// Token: 0x06002475 RID: 9333 RVA: 0x001C9B18 File Offset: 0x001C7D18
	private bool CheckValidPlaceCell(int testCell)
	{
		if (!Grid.IsValidCell(testCell) || Grid.IsSolidCell(testCell) || Grid.ObjectLayers[1].ContainsKey(testCell))
		{
			return false;
		}
		bool flag = true;
		int widthInCells = this.toPlace.GetComponent<OccupyArea>().GetWidthInCells();
		int cell = testCell;
		for (int i = 0; i < widthInCells; i++)
		{
			int cellInDirection = Grid.GetCellInDirection(cell, Direction.Down);
			if (!Grid.IsValidCell(cellInDirection) || !Grid.IsSolidCell(cellInDirection))
			{
				flag = false;
				break;
			}
			cell = Grid.GetCellInDirection(cell, Direction.Right);
		}
		if (flag)
		{
			for (int j = 0; j < this.cellOffsets.Length; j++)
			{
				CellOffset offset = this.cellOffsets[j];
				int num = Grid.OffsetCell(testCell, offset);
				if (!Grid.IsValidCell(num) || Grid.IsSolidCell(num) || !Grid.IsValidBuildingCell(num) || Grid.ObjectLayers[1].ContainsKey(num))
				{
					flag = false;
					break;
				}
			}
		}
		return flag;
	}

	// Token: 0x04001888 RID: 6280
	public List<int> result_cells = new List<int>();

	// Token: 0x04001889 RID: 6281
	private int max_results;

	// Token: 0x0400188A RID: 6282
	private GameObject toPlace;

	// Token: 0x0400188B RID: 6283
	private CellOffset[] cellOffsets;
}
