using System;

namespace Database
{
	// Token: 0x0200210C RID: 8460
	public class ArtableStatuses : ResourceSet<ArtableStatusItem>
	{
		// Token: 0x0600B3E2 RID: 46050 RVA: 0x0043C120 File Offset: 0x0043A320
		public ArtableStatuses(ResourceSet parent) : base("ArtableStatuses", parent)
		{
			this.AwaitingArting = this.Add("AwaitingArting", ArtableStatuses.ArtableStatusType.AwaitingArting);
			this.LookingUgly = this.Add("LookingUgly", ArtableStatuses.ArtableStatusType.LookingUgly);
			this.LookingOkay = this.Add("LookingOkay", ArtableStatuses.ArtableStatusType.LookingOkay);
			this.LookingGreat = this.Add("LookingGreat", ArtableStatuses.ArtableStatusType.LookingGreat);
		}

		// Token: 0x0600B3E3 RID: 46051 RVA: 0x0043C184 File Offset: 0x0043A384
		public ArtableStatusItem Add(string id, ArtableStatuses.ArtableStatusType statusType)
		{
			ArtableStatusItem artableStatusItem = new ArtableStatusItem(id, statusType);
			this.resources.Add(artableStatusItem);
			return artableStatusItem;
		}

		// Token: 0x04008E14 RID: 36372
		public ArtableStatusItem AwaitingArting;

		// Token: 0x04008E15 RID: 36373
		public ArtableStatusItem LookingUgly;

		// Token: 0x04008E16 RID: 36374
		public ArtableStatusItem LookingOkay;

		// Token: 0x04008E17 RID: 36375
		public ArtableStatusItem LookingGreat;

		// Token: 0x0200210D RID: 8461
		public enum ArtableStatusType
		{
			// Token: 0x04008E19 RID: 36377
			AwaitingArting,
			// Token: 0x04008E1A RID: 36378
			LookingUgly,
			// Token: 0x04008E1B RID: 36379
			LookingOkay,
			// Token: 0x04008E1C RID: 36380
			LookingGreat
		}
	}
}
