using System;
using System.Diagnostics;

namespace Database
{
	// Token: 0x0200216C RID: 8556
	public class StatusItems : ResourceSet<StatusItem>
	{
		// Token: 0x0600B605 RID: 46597 RVA: 0x001154AC File Offset: 0x001136AC
		public StatusItems(string id, ResourceSet parent) : base(id, parent)
		{
		}

		// Token: 0x0200216D RID: 8557
		[DebuggerDisplay("{Id}")]
		public class StatusItemInfo : Resource
		{
			// Token: 0x04009474 RID: 38004
			public string Type;

			// Token: 0x04009475 RID: 38005
			public string Tooltip;

			// Token: 0x04009476 RID: 38006
			public bool IsIconTinted;

			// Token: 0x04009477 RID: 38007
			public StatusItem.IconType IconType;

			// Token: 0x04009478 RID: 38008
			public string Icon;

			// Token: 0x04009479 RID: 38009
			public string SoundPath;

			// Token: 0x0400947A RID: 38010
			public bool ShouldNotify;

			// Token: 0x0400947B RID: 38011
			public float NotificationDelay;

			// Token: 0x0400947C RID: 38012
			public NotificationType NotificationType;

			// Token: 0x0400947D RID: 38013
			public bool AllowMultiples;

			// Token: 0x0400947E RID: 38014
			public string Effect;

			// Token: 0x0400947F RID: 38015
			public HashedString Overlay;

			// Token: 0x04009480 RID: 38016
			public HashedString SecondOverlay;
		}
	}
}
