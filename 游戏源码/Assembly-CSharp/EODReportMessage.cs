using System;
using KSerialization;

// Token: 0x02001DF5 RID: 7669
public class EODReportMessage : Message
{
	// Token: 0x0600A06C RID: 41068 RVA: 0x0010849C File Offset: 0x0010669C
	public EODReportMessage(string title, string tooltip)
	{
		this.day = GameUtil.GetCurrentCycle();
		this.title = title;
		this.tooltip = tooltip;
	}

	// Token: 0x0600A06D RID: 41069 RVA: 0x001082E7 File Offset: 0x001064E7
	public EODReportMessage()
	{
	}

	// Token: 0x0600A06E RID: 41070 RVA: 0x000AD332 File Offset: 0x000AB532
	public override string GetSound()
	{
		return null;
	}

	// Token: 0x0600A06F RID: 41071 RVA: 0x000CA99D File Offset: 0x000C8B9D
	public override string GetMessageBody()
	{
		return "";
	}

	// Token: 0x0600A070 RID: 41072 RVA: 0x001084BD File Offset: 0x001066BD
	public override string GetTooltip()
	{
		return this.tooltip;
	}

	// Token: 0x0600A071 RID: 41073 RVA: 0x001084C5 File Offset: 0x001066C5
	public override string GetTitle()
	{
		return this.title;
	}

	// Token: 0x0600A072 RID: 41074 RVA: 0x001084CD File Offset: 0x001066CD
	public void OpenReport()
	{
		ManagementMenu.Instance.OpenReports(this.day);
	}

	// Token: 0x0600A073 RID: 41075 RVA: 0x000AD2F7 File Offset: 0x000AB4F7
	public override bool ShowDialog()
	{
		return false;
	}

	// Token: 0x0600A074 RID: 41076 RVA: 0x001084DF File Offset: 0x001066DF
	public override void OnClick()
	{
		this.OpenReport();
	}

	// Token: 0x04007D73 RID: 32115
	[Serialize]
	private int day;

	// Token: 0x04007D74 RID: 32116
	[Serialize]
	private string title;

	// Token: 0x04007D75 RID: 32117
	[Serialize]
	private string tooltip;
}
