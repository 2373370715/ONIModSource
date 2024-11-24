using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001E5A RID: 7770
public class ManagementScreenNotificationOverlay : KMonoBehaviour
{
	// Token: 0x0600A2E2 RID: 41698 RVA: 0x000A5E40 File Offset: 0x000A4040
	protected void OnEnable()
	{
	}

	// Token: 0x0600A2E3 RID: 41699 RVA: 0x000A5E40 File Offset: 0x000A4040
	protected override void OnDisable()
	{
	}

	// Token: 0x0600A2E4 RID: 41700 RVA: 0x00109A3D File Offset: 0x00107C3D
	private NotificationAlertBar CreateAlertBar(ManagementMenuNotification notification)
	{
		NotificationAlertBar notificationAlertBar = Util.KInstantiateUI<NotificationAlertBar>(this.alertBarPrefab.gameObject, this.alertContainer.gameObject, false);
		notificationAlertBar.Init(notification);
		notificationAlertBar.gameObject.SetActive(true);
		return notificationAlertBar;
	}

	// Token: 0x0600A2E5 RID: 41701 RVA: 0x000A5E40 File Offset: 0x000A4040
	private void NotificationsChanged()
	{
	}

	// Token: 0x04007F26 RID: 32550
	public global::Action currentMenu;

	// Token: 0x04007F27 RID: 32551
	public NotificationAlertBar alertBarPrefab;

	// Token: 0x04007F28 RID: 32552
	public RectTransform alertContainer;

	// Token: 0x04007F29 RID: 32553
	private List<NotificationAlertBar> alertBars = new List<NotificationAlertBar>();
}
