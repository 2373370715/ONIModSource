using System;

// Token: 0x020007FD RID: 2045
public class IdleCellQuery : PathFinderQuery
{
	// Token: 0x0600248B RID: 9355 RVA: 0x000B7CA1 File Offset: 0x000B5EA1
	public IdleCellQuery Reset(MinionBrain brain, int max_cost)
	{
		this.brain = brain;
		this.maxCost = max_cost;
		this.targetCell = Grid.InvalidCell;
		return this;
	}

	// Token: 0x0600248C RID: 9356 RVA: 0x001C9D78 File Offset: 0x001C7F78
	public override bool IsMatch(int cell, int parent_cell, int cost)
	{
		SafeCellQuery.SafeFlags flags = SafeCellQuery.GetFlags(cell, this.brain, false);
		if ((flags & SafeCellQuery.SafeFlags.IsClear) != (SafeCellQuery.SafeFlags)0 && (flags & SafeCellQuery.SafeFlags.IsNotLadder) != (SafeCellQuery.SafeFlags)0 && (flags & SafeCellQuery.SafeFlags.IsNotTube) != (SafeCellQuery.SafeFlags)0 && (flags & SafeCellQuery.SafeFlags.IsBreathable) != (SafeCellQuery.SafeFlags)0 && (flags & SafeCellQuery.SafeFlags.IsNotLiquid) != (SafeCellQuery.SafeFlags)0)
		{
			this.targetCell = cell;
		}
		return cost > this.maxCost;
	}

	// Token: 0x0600248D RID: 9357 RVA: 0x000B7CBD File Offset: 0x000B5EBD
	public override int GetResultCell()
	{
		return this.targetCell;
	}

	// Token: 0x04001894 RID: 6292
	private MinionBrain brain;

	// Token: 0x04001895 RID: 6293
	private int targetCell;

	// Token: 0x04001896 RID: 6294
	private int maxCost;
}
