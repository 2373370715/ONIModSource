using System;
using KSerialization;
using STRINGS;

// Token: 0x02001DFF RID: 7679
public class ResearchCompleteMessage : Message
{
	// Token: 0x0600A0BF RID: 41151 RVA: 0x0010881C File Offset: 0x00106A1C
	public ResearchCompleteMessage()
	{
	}

	// Token: 0x0600A0C0 RID: 41152 RVA: 0x0010882F File Offset: 0x00106A2F
	public ResearchCompleteMessage(Tech tech)
	{
		this.tech.Set(tech);
	}

	// Token: 0x0600A0C1 RID: 41153 RVA: 0x001082A2 File Offset: 0x001064A2
	public override string GetSound()
	{
		return "AI_Notification_ResearchComplete";
	}

	// Token: 0x0600A0C2 RID: 41154 RVA: 0x003D60A4 File Offset: 0x003D42A4
	public override string GetMessageBody()
	{
		Tech tech = this.tech.Get();
		string text = "";
		for (int i = 0; i < tech.unlockedItems.Count; i++)
		{
			if (i != 0)
			{
				text += ", ";
			}
			text += tech.unlockedItems[i].Name;
		}
		return string.Format(MISC.NOTIFICATIONS.RESEARCHCOMPLETE.MESSAGEBODY, tech.Name, text);
	}

	// Token: 0x0600A0C3 RID: 41155 RVA: 0x0010884E File Offset: 0x00106A4E
	public override string GetTitle()
	{
		return MISC.NOTIFICATIONS.RESEARCHCOMPLETE.NAME;
	}

	// Token: 0x0600A0C4 RID: 41156 RVA: 0x003D6118 File Offset: 0x003D4318
	public override string GetTooltip()
	{
		Tech tech = this.tech.Get();
		return string.Format(MISC.NOTIFICATIONS.RESEARCHCOMPLETE.TOOLTIP, tech.Name);
	}

	// Token: 0x0600A0C5 RID: 41157 RVA: 0x0010885A File Offset: 0x00106A5A
	public override bool IsValid()
	{
		return this.tech.Get() != null;
	}

	// Token: 0x04007D93 RID: 32147
	[Serialize]
	private ResourceRef<Tech> tech = new ResourceRef<Tech>();
}
