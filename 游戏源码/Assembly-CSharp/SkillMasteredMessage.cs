using System;
using KSerialization;
using STRINGS;

// Token: 0x02001E01 RID: 7681
public class SkillMasteredMessage : Message
{
	// Token: 0x0600A0D1 RID: 41169 RVA: 0x001082E7 File Offset: 0x001064E7
	public SkillMasteredMessage()
	{
	}

	// Token: 0x0600A0D2 RID: 41170 RVA: 0x001088F0 File Offset: 0x00106AF0
	public SkillMasteredMessage(MinionResume resume)
	{
		this.minionName = resume.GetProperName();
	}

	// Token: 0x0600A0D3 RID: 41171 RVA: 0x001082A2 File Offset: 0x001064A2
	public override string GetSound()
	{
		return "AI_Notification_ResearchComplete";
	}

	// Token: 0x0600A0D4 RID: 41172 RVA: 0x003D6378 File Offset: 0x003D4578
	public override string GetMessageBody()
	{
		Debug.Assert(this.minionName != null);
		string arg = string.Format(MISC.NOTIFICATIONS.SKILL_POINT_EARNED.LINE, this.minionName);
		return string.Format(MISC.NOTIFICATIONS.SKILL_POINT_EARNED.MESSAGEBODY, arg);
	}

	// Token: 0x0600A0D5 RID: 41173 RVA: 0x00108904 File Offset: 0x00106B04
	public override string GetTitle()
	{
		return MISC.NOTIFICATIONS.SKILL_POINT_EARNED.NAME.Replace("{Duplicant}", this.minionName);
	}

	// Token: 0x0600A0D6 RID: 41174 RVA: 0x0010891B File Offset: 0x00106B1B
	public override string GetTooltip()
	{
		return MISC.NOTIFICATIONS.SKILL_POINT_EARNED.TOOLTIP.Replace("{Duplicant}", this.minionName);
	}

	// Token: 0x0600A0D7 RID: 41175 RVA: 0x00108932 File Offset: 0x00106B32
	public override bool IsValid()
	{
		return this.minionName != null;
	}

	// Token: 0x04007D96 RID: 32150
	[Serialize]
	private string minionName;
}
