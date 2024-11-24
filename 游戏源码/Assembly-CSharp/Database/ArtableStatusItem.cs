using System;

namespace Database
{
	// Token: 0x0200210E RID: 8462
	public class ArtableStatusItem : StatusItem
	{
		// Token: 0x0600B3E4 RID: 46052 RVA: 0x0043C1A8 File Offset: 0x0043A3A8
		public ArtableStatusItem(string id, ArtableStatuses.ArtableStatusType statusType) : base(id, "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null)
		{
			this.StatusType = statusType;
		}

		// Token: 0x04008E1D RID: 36381
		public ArtableStatuses.ArtableStatusType StatusType;
	}
}
