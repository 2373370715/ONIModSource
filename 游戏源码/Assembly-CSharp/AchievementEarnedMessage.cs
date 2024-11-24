using System;
using STRINGS;

// Token: 0x02001DEF RID: 7663
public class AchievementEarnedMessage : Message
{
	// Token: 0x0600A042 RID: 41026 RVA: 0x000AD2F7 File Offset: 0x000AB4F7
	public override bool ShowDialog()
	{
		return false;
	}

	// Token: 0x0600A043 RID: 41027 RVA: 0x001082A2 File Offset: 0x001064A2
	public override string GetSound()
	{
		return "AI_Notification_ResearchComplete";
	}

	// Token: 0x0600A044 RID: 41028 RVA: 0x000CA99D File Offset: 0x000C8B9D
	public override string GetMessageBody()
	{
		return "";
	}

	// Token: 0x0600A045 RID: 41029 RVA: 0x001082A9 File Offset: 0x001064A9
	public override string GetTitle()
	{
		return MISC.NOTIFICATIONS.COLONY_ACHIEVEMENT_EARNED.NAME;
	}

	// Token: 0x0600A046 RID: 41030 RVA: 0x001082B5 File Offset: 0x001064B5
	public override string GetTooltip()
	{
		return MISC.NOTIFICATIONS.COLONY_ACHIEVEMENT_EARNED.TOOLTIP;
	}

	// Token: 0x0600A047 RID: 41031 RVA: 0x000A65EC File Offset: 0x000A47EC
	public override bool IsValid()
	{
		return true;
	}

	// Token: 0x0600A048 RID: 41032 RVA: 0x001082C1 File Offset: 0x001064C1
	public override void OnClick()
	{
		RetireColonyUtility.SaveColonySummaryData();
		MainMenu.ActivateRetiredColoniesScreenFromData(PauseScreen.Instance.transform.parent.gameObject, RetireColonyUtility.GetCurrentColonyRetiredColonyData());
	}
}
