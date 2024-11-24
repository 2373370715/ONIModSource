using System;

// Token: 0x020007F8 RID: 2040
public class CellCostQuery : PathFinderQuery
{
	// Token: 0x17000107 RID: 263
	// (get) Token: 0x0600247A RID: 9338 RVA: 0x000B7BCF File Offset: 0x000B5DCF
	// (set) Token: 0x0600247B RID: 9339 RVA: 0x000B7BD7 File Offset: 0x000B5DD7
	public int resultCost { get; private set; }

	// Token: 0x0600247C RID: 9340 RVA: 0x000B7BE0 File Offset: 0x000B5DE0
	public void Reset(int target_cell, int max_cost)
	{
		this.targetCell = target_cell;
		this.maxCost = max_cost;
		this.resultCost = -1;
	}

	// Token: 0x0600247D RID: 9341 RVA: 0x000B7BF7 File Offset: 0x000B5DF7
	public override bool IsMatch(int cell, int parent_cell, int cost)
	{
		if (cost > this.maxCost)
		{
			return true;
		}
		if (cell == this.targetCell)
		{
			this.resultCost = cost;
			return true;
		}
		return false;
	}

	// Token: 0x0400188D RID: 6285
	private int targetCell;

	// Token: 0x0400188E RID: 6286
	private int maxCost;
}
