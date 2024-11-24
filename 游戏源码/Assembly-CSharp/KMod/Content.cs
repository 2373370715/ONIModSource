using System;

namespace KMod
{
	// Token: 0x020021DE RID: 8670
	[Flags]
	public enum Content : byte
	{
		// Token: 0x0400963F RID: 38463
		LayerableFiles = 1,
		// Token: 0x04009640 RID: 38464
		Strings = 2,
		// Token: 0x04009641 RID: 38465
		DLL = 4,
		// Token: 0x04009642 RID: 38466
		Translation = 8,
		// Token: 0x04009643 RID: 38467
		Animation = 16
	}
}
