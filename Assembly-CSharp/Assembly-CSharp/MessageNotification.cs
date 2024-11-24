﻿using System;
using System.Collections.Generic;

public class MessageNotification : Notification
{
	private string OnToolTip(List<Notification> notifications, string tooltipText)
	{
		return tooltipText;
	}

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

	public Message message;
}
