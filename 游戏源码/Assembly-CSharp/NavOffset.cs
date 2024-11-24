using System;

// Token: 0x020007D4 RID: 2004
public struct NavOffset
{
	// Token: 0x060023D2 RID: 9170 RVA: 0x000B7481 File Offset: 0x000B5681
	public NavOffset(NavType nav_type, int x, int y)
	{
		this.navType = nav_type;
		this.offset.x = x;
		this.offset.y = y;
	}

	// Token: 0x040017D7 RID: 6103
	public NavType navType;

	// Token: 0x040017D8 RID: 6104
	public CellOffset offset;
}
