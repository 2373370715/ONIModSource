using System;

namespace KMod
{
	// Token: 0x020021EF RID: 8687
	public enum EventType
	{
		// Token: 0x04009698 RID: 38552
		LoadError,
		// Token: 0x04009699 RID: 38553
		NotFound,
		// Token: 0x0400969A RID: 38554
		InstallInfoInaccessible,
		// Token: 0x0400969B RID: 38555
		OutOfOrder,
		// Token: 0x0400969C RID: 38556
		ExpectedActive,
		// Token: 0x0400969D RID: 38557
		ExpectedInactive,
		// Token: 0x0400969E RID: 38558
		ActiveDuringCrash,
		// Token: 0x0400969F RID: 38559
		InstallFailed,
		// Token: 0x040096A0 RID: 38560
		Installed,
		// Token: 0x040096A1 RID: 38561
		Uninstalled,
		// Token: 0x040096A2 RID: 38562
		VersionUpdate,
		// Token: 0x040096A3 RID: 38563
		AvailableContentChanged,
		// Token: 0x040096A4 RID: 38564
		RestartRequested,
		// Token: 0x040096A5 RID: 38565
		BadWorldGen,
		// Token: 0x040096A6 RID: 38566
		Deactivated,
		// Token: 0x040096A7 RID: 38567
		DisabledEarlyAccess,
		// Token: 0x040096A8 RID: 38568
		DownloadFailed
	}
}
