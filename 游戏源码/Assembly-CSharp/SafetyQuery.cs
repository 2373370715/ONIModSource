using System;

// Token: 0x02000807 RID: 2055
public class SafetyQuery : PathFinderQuery
{
	// Token: 0x060024B7 RID: 9399 RVA: 0x000B7E92 File Offset: 0x000B6092
	public SafetyQuery(SafetyChecker checker, KMonoBehaviour cmp, int max_cost)
	{
		this.checker = checker;
		this.cmp = cmp;
		this.maxCost = max_cost;
	}

	// Token: 0x060024B8 RID: 9400 RVA: 0x000B7EAF File Offset: 0x000B60AF
	public void Reset()
	{
		this.targetCell = PathFinder.InvalidCell;
		this.targetCost = int.MaxValue;
		this.targetConditions = 0;
		this.context = new SafetyChecker.Context(this.cmp);
	}

	// Token: 0x060024B9 RID: 9401 RVA: 0x001CA54C File Offset: 0x001C874C
	public override bool IsMatch(int cell, int parent_cell, int cost)
	{
		bool flag = false;
		int safetyConditions = this.checker.GetSafetyConditions(cell, cost, this.context, out flag);
		if (safetyConditions != 0 && (safetyConditions > this.targetConditions || (safetyConditions == this.targetConditions && cost < this.targetCost)))
		{
			this.targetCell = cell;
			this.targetConditions = safetyConditions;
			this.targetCost = cost;
			if (flag)
			{
				return true;
			}
		}
		return cost >= this.maxCost;
	}

	// Token: 0x060024BA RID: 9402 RVA: 0x000B7EDF File Offset: 0x000B60DF
	public override int GetResultCell()
	{
		return this.targetCell;
	}

	// Token: 0x040018C9 RID: 6345
	private int targetCell;

	// Token: 0x040018CA RID: 6346
	private int targetCost;

	// Token: 0x040018CB RID: 6347
	private int targetConditions;

	// Token: 0x040018CC RID: 6348
	private int maxCost;

	// Token: 0x040018CD RID: 6349
	private SafetyChecker checker;

	// Token: 0x040018CE RID: 6350
	private KMonoBehaviour cmp;

	// Token: 0x040018CF RID: 6351
	private SafetyChecker.Context context;
}
