using System;

// Token: 0x02000AA0 RID: 2720
public static class NavigationTactics
{
	// Token: 0x040021E7 RID: 8679
	public static NavTactic ReduceTravelDistance = new NavTactic(0, 0, 1, 4);

	// Token: 0x040021E8 RID: 8680
	public static NavTactic Range_2_AvoidOverlaps = new NavTactic(2, 6, 12, 1);

	// Token: 0x040021E9 RID: 8681
	public static NavTactic Range_3_ProhibitOverlap = new NavTactic(3, 6, 9999, 1);
}
