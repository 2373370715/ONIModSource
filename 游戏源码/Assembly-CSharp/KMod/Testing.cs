using System;

namespace KMod
{
	// Token: 0x020021C5 RID: 8645
	public static class Testing
	{
		// Token: 0x04009603 RID: 38403
		public static Testing.DLLLoading dll_loading;

		// Token: 0x04009604 RID: 38404
		public const Testing.SaveLoad SAVE_LOAD = Testing.SaveLoad.NoTesting;

		// Token: 0x04009605 RID: 38405
		public const Testing.Install INSTALL = Testing.Install.NoTesting;

		// Token: 0x04009606 RID: 38406
		public const Testing.Boot BOOT = Testing.Boot.NoTesting;

		// Token: 0x020021C6 RID: 8646
		public enum DLLLoading
		{
			// Token: 0x04009608 RID: 38408
			NoTesting,
			// Token: 0x04009609 RID: 38409
			Fail,
			// Token: 0x0400960A RID: 38410
			UseModLoaderDLLExclusively
		}

		// Token: 0x020021C7 RID: 8647
		public enum SaveLoad
		{
			// Token: 0x0400960C RID: 38412
			NoTesting,
			// Token: 0x0400960D RID: 38413
			FailSave,
			// Token: 0x0400960E RID: 38414
			FailLoad
		}

		// Token: 0x020021C8 RID: 8648
		public enum Install
		{
			// Token: 0x04009610 RID: 38416
			NoTesting,
			// Token: 0x04009611 RID: 38417
			ForceUninstall,
			// Token: 0x04009612 RID: 38418
			ForceReinstall,
			// Token: 0x04009613 RID: 38419
			ForceUpdate
		}

		// Token: 0x020021C9 RID: 8649
		public enum Boot
		{
			// Token: 0x04009615 RID: 38421
			NoTesting,
			// Token: 0x04009616 RID: 38422
			Crash
		}
	}
}
