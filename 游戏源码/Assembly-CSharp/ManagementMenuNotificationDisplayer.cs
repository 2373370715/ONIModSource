using System;
using System.Collections.Generic;

// Token: 0x02001E59 RID: 7769
public class ManagementMenuNotificationDisplayer : NotificationDisplayer
{
	// Token: 0x17000A78 RID: 2680
	// (get) Token: 0x0600A2D7 RID: 41687 RVA: 0x001099BC File Offset: 0x00107BBC
	// (set) Token: 0x0600A2D8 RID: 41688 RVA: 0x001099C4 File Offset: 0x00107BC4
	public List<ManagementMenuNotification> displayedManagementMenuNotifications { get; private set; }

	// Token: 0x14000031 RID: 49
	// (add) Token: 0x0600A2D9 RID: 41689 RVA: 0x003DF8F0 File Offset: 0x003DDAF0
	// (remove) Token: 0x0600A2DA RID: 41690 RVA: 0x003DF928 File Offset: 0x003DDB28
	public event System.Action onNotificationsChanged;

	// Token: 0x0600A2DB RID: 41691 RVA: 0x001099CD File Offset: 0x00107BCD
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.displayedManagementMenuNotifications = new List<ManagementMenuNotification>();
	}

	// Token: 0x0600A2DC RID: 41692 RVA: 0x001099E0 File Offset: 0x00107BE0
	public void NotificationWasViewed(ManagementMenuNotification notification)
	{
		this.onNotificationsChanged();
	}

	// Token: 0x0600A2DD RID: 41693 RVA: 0x001099ED File Offset: 0x00107BED
	protected override void OnNotificationAdded(Notification notification)
	{
		this.displayedManagementMenuNotifications.Add(notification as ManagementMenuNotification);
		this.onNotificationsChanged();
	}

	// Token: 0x0600A2DE RID: 41694 RVA: 0x00109A0B File Offset: 0x00107C0B
	protected override void OnNotificationRemoved(Notification notification)
	{
		this.displayedManagementMenuNotifications.Remove(notification as ManagementMenuNotification);
		this.onNotificationsChanged();
	}

	// Token: 0x0600A2DF RID: 41695 RVA: 0x00109A2A File Offset: 0x00107C2A
	protected override bool ShouldDisplayNotification(Notification notification)
	{
		return notification is ManagementMenuNotification;
	}

	// Token: 0x0600A2E0 RID: 41696 RVA: 0x003DF960 File Offset: 0x003DDB60
	public List<ManagementMenuNotification> GetNotificationsForAction(global::Action hotKey)
	{
		List<ManagementMenuNotification> list = new List<ManagementMenuNotification>();
		foreach (ManagementMenuNotification managementMenuNotification in this.displayedManagementMenuNotifications)
		{
			if (managementMenuNotification.targetMenu == hotKey)
			{
				list.Add(managementMenuNotification);
			}
		}
		return list;
	}
}
