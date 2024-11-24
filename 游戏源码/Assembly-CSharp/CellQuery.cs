using System;

// Token: 0x020007FA RID: 2042
public class CellQuery : PathFinderQuery
{
	// Token: 0x06002481 RID: 9345 RVA: 0x000B7C1F File Offset: 0x000B5E1F
	public CellQuery Reset(int target_cell)
	{
		this.targetCell = target_cell;
		return this;
	}

	// Token: 0x06002482 RID: 9346 RVA: 0x000B7C29 File Offset: 0x000B5E29
	public override bool IsMatch(int cell, int parent_cell, int cost)
	{
		return cell == this.targetCell;
	}

	// Token: 0x04001890 RID: 6288
	private int targetCell;
}
