using System;

// Token: 0x02000B52 RID: 2898
public struct DataPoint
{
	// Token: 0x060036F4 RID: 14068 RVA: 0x000C3ACE File Offset: 0x000C1CCE
	public DataPoint(float start, float end, float value)
	{
		this.periodStart = start;
		this.periodEnd = end;
		this.periodValue = value;
	}

	// Token: 0x04002538 RID: 9528
	public float periodStart;

	// Token: 0x04002539 RID: 9529
	public float periodEnd;

	// Token: 0x0400253A RID: 9530
	public float periodValue;
}
