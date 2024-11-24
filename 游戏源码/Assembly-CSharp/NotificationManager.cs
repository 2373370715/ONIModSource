using System;
using System.Collections.Generic;

// Token: 0x02001E5F RID: 7775
public class NotificationManager : KMonoBehaviour
{
	// Token: 0x17000A79 RID: 2681
	// (get) Token: 0x0600A301 RID: 41729 RVA: 0x00109C07 File Offset: 0x00107E07
	// (set) Token: 0x0600A302 RID: 41730 RVA: 0x00109C0E File Offset: 0x00107E0E
	public static NotificationManager Instance { get; private set; }

	// Token: 0x14000032 RID: 50
	// (add) Token: 0x0600A303 RID: 41731 RVA: 0x003DFDA8 File Offset: 0x003DDFA8
	// (remove) Token: 0x0600A304 RID: 41732 RVA: 0x003DFDE0 File Offset: 0x003DDFE0
	public event Action<Notification> notificationAdded;

	// Token: 0x14000033 RID: 51
	// (add) Token: 0x0600A305 RID: 41733 RVA: 0x003DFE18 File Offset: 0x003DE018
	// (remove) Token: 0x0600A306 RID: 41734 RVA: 0x003DFE50 File Offset: 0x003DE050
	public event Action<Notification> notificationRemoved;

	// Token: 0x0600A307 RID: 41735 RVA: 0x00109C16 File Offset: 0x00107E16
	protected override void OnPrefabInit()
	{
		Debug.Assert(NotificationManager.Instance == null);
		NotificationManager.Instance = this;
	}

	// Token: 0x0600A308 RID: 41736 RVA: 0x00109C2E File Offset: 0x00107E2E
	protected override void OnForcedCleanUp()
	{
		NotificationManager.Instance = null;
	}

	// Token: 0x0600A309 RID: 41737 RVA: 0x00109C36 File Offset: 0x00107E36
	public void AddNotification(Notification notification)
	{
		this.pendingNotifications.Add(notification);
		if (NotificationScreen.Instance != null)
		{
			NotificationScreen.Instance.AddPendingNotification(notification);
		}
	}

	// Token: 0x0600A30A RID: 41738 RVA: 0x003DFE88 File Offset: 0x003DE088
	public void RemoveNotification(Notification notification)
	{
		this.pendingNotifications.Remove(notification);
		if (NotificationScreen.Instance != null)
		{
			NotificationScreen.Instance.RemovePendingNotification(notification);
		}
		if (this.notifications.Remove(notification))
		{
			this.notificationRemoved(notification);
		}
	}

	// Token: 0x0600A30B RID: 41739 RVA: 0x003DFED4 File Offset: 0x003DE0D4
	private void Update()
	{
		int i = 0;
		while (i < this.pendingNotifications.Count)
		{
			if (this.pendingNotifications[i].IsReady())
			{
				this.DoAddNotification(this.pendingNotifications[i]);
				this.pendingNotifications.RemoveAt(i);
			}
			else
			{
				i++;
			}
		}
	}

	// Token: 0x0600A30C RID: 41740 RVA: 0x00109C5C File Offset: 0x00107E5C
	private void DoAddNotification(Notification notification)
	{
		this.notifications.Add(notification);
		if (this.notificationAdded != null)
		{
			this.notificationAdded(notification);
		}
	}

	// Token: 0x04007F3B RID: 32571
	private List<Notification> pendingNotifications = new List<Notification>();

	// Token: 0x04007F3C RID: 32572
	private List<Notification> notifications = new List<Notification>();
}
