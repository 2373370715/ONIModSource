using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000AAC RID: 2732
public class ManagementMenuNotification : Notification
{
	// Token: 0x1700020F RID: 527
	// (get) Token: 0x060032D7 RID: 13015 RVA: 0x000C129A File Offset: 0x000BF49A
	// (set) Token: 0x060032D8 RID: 13016 RVA: 0x000C12A2 File Offset: 0x000BF4A2
	public bool hasBeenViewed { get; private set; }

	// Token: 0x17000210 RID: 528
	// (get) Token: 0x060032D9 RID: 13017 RVA: 0x000C12AB File Offset: 0x000BF4AB
	// (set) Token: 0x060032DA RID: 13018 RVA: 0x000C12B3 File Offset: 0x000BF4B3
	public string highlightTarget { get; set; }

	// Token: 0x060032DB RID: 13019 RVA: 0x00204998 File Offset: 0x00202B98
	public ManagementMenuNotification(global::Action targetMenu, NotificationValence valence, string highlightTarget, string title, NotificationType type, Func<List<Notification>, object, string> tooltip = null, object tooltip_data = null, bool expires = true, float delay = 0f, Notification.ClickCallback custom_click_callback = null, object custom_click_data = null, Transform click_focus = null, bool volume_attenuation = true) : base(title, type, tooltip, tooltip_data, expires, delay, custom_click_callback, custom_click_data, click_focus, volume_attenuation, false, false)
	{
		this.targetMenu = targetMenu;
		this.valence = valence;
		this.highlightTarget = highlightTarget;
	}

	// Token: 0x060032DC RID: 13020 RVA: 0x000C12BC File Offset: 0x000BF4BC
	public void View()
	{
		this.hasBeenViewed = true;
		ManagementMenu.Instance.notificationDisplayer.NotificationWasViewed(this);
	}

	// Token: 0x04002238 RID: 8760
	public global::Action targetMenu;

	// Token: 0x04002239 RID: 8761
	public NotificationValence valence;
}
