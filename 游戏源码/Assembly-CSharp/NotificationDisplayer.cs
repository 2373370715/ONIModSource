using System;
using System.Collections.Generic;

// Token: 0x02001E5C RID: 7772
public abstract class NotificationDisplayer : KMonoBehaviour
{
	// Token: 0x0600A2EB RID: 41707 RVA: 0x00109A81 File Offset: 0x00107C81
	protected override void OnSpawn()
	{
		this.displayedNotifications = new List<Notification>();
		NotificationManager.Instance.notificationAdded += this.NotificationAdded;
		NotificationManager.Instance.notificationRemoved += this.NotificationRemoved;
	}

	// Token: 0x0600A2EC RID: 41708 RVA: 0x00109ABA File Offset: 0x00107CBA
	public void NotificationAdded(Notification notification)
	{
		if (this.ShouldDisplayNotification(notification))
		{
			this.displayedNotifications.Add(notification);
			this.OnNotificationAdded(notification);
		}
	}

	// Token: 0x0600A2ED RID: 41709
	protected abstract void OnNotificationAdded(Notification notification);

	// Token: 0x0600A2EE RID: 41710 RVA: 0x00109AD8 File Offset: 0x00107CD8
	public void NotificationRemoved(Notification notification)
	{
		if (this.displayedNotifications.Contains(notification))
		{
			this.displayedNotifications.Remove(notification);
			this.OnNotificationRemoved(notification);
		}
	}

	// Token: 0x0600A2EF RID: 41711
	protected abstract void OnNotificationRemoved(Notification notification);

	// Token: 0x0600A2F0 RID: 41712
	protected abstract bool ShouldDisplayNotification(Notification notification);

	// Token: 0x04007F31 RID: 32561
	protected List<Notification> displayedNotifications;
}
