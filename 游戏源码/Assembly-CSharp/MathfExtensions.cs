using System;

// Token: 0x020004AE RID: 1198
public static class MathfExtensions
{
	// Token: 0x06001530 RID: 5424 RVA: 0x000AF694 File Offset: 0x000AD894
	public static long Max(this long a, long b)
	{
		if (a < b)
		{
			return b;
		}
		return a;
	}

	// Token: 0x06001531 RID: 5425 RVA: 0x000AF69D File Offset: 0x000AD89D
	public static long Min(this long a, long b)
	{
		if (a > b)
		{
			return b;
		}
		return a;
	}
}
