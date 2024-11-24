using System;

// Token: 0x020007F1 RID: 2033
public class PathFinderQuery
{
	// Token: 0x06002454 RID: 9300 RVA: 0x000A65EC File Offset: 0x000A47EC
	public virtual bool IsMatch(int cell, int parent_cell, int cost)
	{
		return true;
	}

	// Token: 0x06002455 RID: 9301 RVA: 0x000B79F0 File Offset: 0x000B5BF0
	public void SetResult(int cell, int cost, NavType nav_type)
	{
		this.resultCell = cell;
		this.resultNavType = nav_type;
	}

	// Token: 0x06002456 RID: 9302 RVA: 0x000B7A00 File Offset: 0x000B5C00
	public void ClearResult()
	{
		this.resultCell = -1;
	}

	// Token: 0x06002457 RID: 9303 RVA: 0x000B7A09 File Offset: 0x000B5C09
	public virtual int GetResultCell()
	{
		return this.resultCell;
	}

	// Token: 0x06002458 RID: 9304 RVA: 0x000B7A11 File Offset: 0x000B5C11
	public NavType GetResultNavType()
	{
		return this.resultNavType;
	}

	// Token: 0x0400186A RID: 6250
	protected int resultCell;

	// Token: 0x0400186B RID: 6251
	private NavType resultNavType;
}
