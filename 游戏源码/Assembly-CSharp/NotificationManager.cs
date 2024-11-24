using System;
using System.Collections.Generic;

public class NotificationManager : KMonoBehaviour
{
	private List<Notification> pendingNotifications = new List<Notification>();

	private List<Notification> notifications = new List<Notification>();

	public static NotificationManager Instance { get; private set; }

	public event Action<Notification> notificationAdded;

	public event Action<Notification> notificationRemoved;

	protected override void OnPrefabInit()
	{
		Debug.Assert(Instance == null);
		Instance = this;
	}

	protected override void OnForcedCleanUp()
	{
		Instance = null;
	}

	public void AddNotification(Notification notification)
	{
		pendingNotifications.Add(notification);
		if (NotificationScreen.Instance != null)
		{
			NotificationScreen.Instance.AddPendingNotification(notification);
		}
	}

	public void RemoveNotification(Notification notification)
	{
		pendingNotifications.Remove(notification);
		if (NotificationScreen.Instance != null)
		{
			NotificationScreen.Instance.RemovePendingNotification(notification);
		}
		if (notifications.Remove(notification))
		{
			this.notificationRemoved(notification);
		}
	}

	private void Update()
	{
		int num = 0;
		while (num < pendingNotifications.Count)
		{
			if (pendingNotifications[num].IsReady())
			{
				DoAddNotification(pendingNotifications[num]);
				pendingNotifications.RemoveAt(num);
			}
			else
			{
				num++;
			}
		}
	}

	private void DoAddNotification(Notification notification)
	{
		notifications.Add(notification);
		if (this.notificationAdded != null)
		{
			this.notificationAdded(notification);
		}
	}
}
