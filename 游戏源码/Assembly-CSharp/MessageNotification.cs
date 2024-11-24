using System;
using System.Collections.Generic;

// Token: 0x02000A91 RID: 2705
public class MessageNotification : Notification
{
	// Token: 0x060031FD RID: 12797 RVA: 0x000A8AEE File Offset: 0x000A6CEE
	private string OnToolTip(List<Notification> notifications, string tooltipText)
	{
		return tooltipText;
	}

	// Token: 0x060031FE RID: 12798 RVA: 0x00201950 File Offset: 0x001FFB50
	public MessageNotification(Message m) : base(m.GetTitle(), NotificationType.Messages, null, null, false, 0f, null, null, null, true, false, true)
	{
		MessageNotification <>4__this = this;
		this.message = m;
		base.Type = m.GetMessageType();
		this.showDismissButton = m.ShowDismissButton();
		if (!this.message.PlayNotificationSound())
		{
			this.playSound = false;
		}
		base.ToolTip = ((List<Notification> notifications, object data) => <>4__this.OnToolTip(notifications, m.GetTooltip()));
		base.clickFocus = null;
	}

	// Token: 0x04002198 RID: 8600
	public Message message;
}
