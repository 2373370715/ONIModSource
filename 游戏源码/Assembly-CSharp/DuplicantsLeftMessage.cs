using System;
using STRINGS;

// Token: 0x02001DF4 RID: 7668
public class DuplicantsLeftMessage : Message
{
	// Token: 0x0600A067 RID: 41063 RVA: 0x000CA99D File Offset: 0x000C8B9D
	public override string GetSound()
	{
		return "";
	}

	// Token: 0x0600A068 RID: 41064 RVA: 0x00108478 File Offset: 0x00106678
	public override string GetTitle()
	{
		return MISC.NOTIFICATIONS.DUPLICANTABSORBED.NAME;
	}

	// Token: 0x0600A069 RID: 41065 RVA: 0x00108484 File Offset: 0x00106684
	public override string GetMessageBody()
	{
		return MISC.NOTIFICATIONS.DUPLICANTABSORBED.MESSAGEBODY;
	}

	// Token: 0x0600A06A RID: 41066 RVA: 0x00108490 File Offset: 0x00106690
	public override string GetTooltip()
	{
		return MISC.NOTIFICATIONS.DUPLICANTABSORBED.TOOLTIP;
	}
}
