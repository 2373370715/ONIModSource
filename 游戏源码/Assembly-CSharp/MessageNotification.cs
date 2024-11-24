using System.Collections.Generic;

public class MessageNotification : Notification
{
	public Message message;

	private string OnToolTip(List<Notification> notifications, string tooltipText)
	{
		return tooltipText;
	}

	public MessageNotification(Message m)
		: base(m.GetTitle(), NotificationType.Messages, null, null, expires: false, 0f, null, null, null, volume_attenuation: true, clear_on_click: false, show_dismiss_button: true)
	{
		MessageNotification messageNotification = this;
		message = m;
		base.Type = m.GetMessageType();
		showDismissButton = m.ShowDismissButton();
		if (!message.PlayNotificationSound())
		{
			playSound = false;
		}
		base.ToolTip = (List<Notification> notifications, object data) => messageNotification.OnToolTip(notifications, m.GetTooltip());
		base.clickFocus = null;
	}
}
