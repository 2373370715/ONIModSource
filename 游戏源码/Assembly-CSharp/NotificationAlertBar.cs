using System;
using System.Collections.Generic;

// Token: 0x02001E5B RID: 7771
public class NotificationAlertBar : KMonoBehaviour
{
	// Token: 0x0600A2E7 RID: 41703 RVA: 0x003DF9C4 File Offset: 0x003DDBC4
	public void Init(ManagementMenuNotification notification)
	{
		this.notification = notification;
		this.thisButton.onClick += this.OnThisButtonClicked;
		this.background.colorStyleSetting = this.alertColorStyle[(int)notification.valence];
		this.background.ApplyColorStyleSetting();
		this.text.text = notification.titleText;
		this.tooltip.SetSimpleTooltip(notification.ToolTip(null, notification.tooltipData));
		this.muteButton.onClick += this.OnMuteButtonClicked;
	}

	// Token: 0x0600A2E8 RID: 41704 RVA: 0x003DFA5C File Offset: 0x003DDC5C
	private void OnThisButtonClicked()
	{
		NotificationHighlightController componentInParent = base.GetComponentInParent<NotificationHighlightController>();
		if (componentInParent != null)
		{
			componentInParent.SetActiveTarget(this.notification);
			return;
		}
		this.notification.View();
	}

	// Token: 0x0600A2E9 RID: 41705 RVA: 0x000A5E40 File Offset: 0x000A4040
	private void OnMuteButtonClicked()
	{
	}

	// Token: 0x04007F2A RID: 32554
	public ManagementMenuNotification notification;

	// Token: 0x04007F2B RID: 32555
	public KButton thisButton;

	// Token: 0x04007F2C RID: 32556
	public KImage background;

	// Token: 0x04007F2D RID: 32557
	public LocText text;

	// Token: 0x04007F2E RID: 32558
	public ToolTip tooltip;

	// Token: 0x04007F2F RID: 32559
	public KButton muteButton;

	// Token: 0x04007F30 RID: 32560
	public List<ColorStyleSetting> alertColorStyle;
}
