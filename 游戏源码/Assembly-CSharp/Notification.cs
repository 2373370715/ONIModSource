using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

// Token: 0x02000AAD RID: 2733
public class Notification
{
	// Token: 0x17000211 RID: 529
	// (get) Token: 0x060032DD RID: 13021 RVA: 0x000C12D5 File Offset: 0x000BF4D5
	// (set) Token: 0x060032DE RID: 13022 RVA: 0x000C12DD File Offset: 0x000BF4DD
	public NotificationType Type { get; set; }

	// Token: 0x17000212 RID: 530
	// (get) Token: 0x060032DF RID: 13023 RVA: 0x000C12E6 File Offset: 0x000BF4E6
	// (set) Token: 0x060032E0 RID: 13024 RVA: 0x000C12EE File Offset: 0x000BF4EE
	public Notifier Notifier { get; set; }

	// Token: 0x17000213 RID: 531
	// (get) Token: 0x060032E1 RID: 13025 RVA: 0x000C12F7 File Offset: 0x000BF4F7
	// (set) Token: 0x060032E2 RID: 13026 RVA: 0x000C12FF File Offset: 0x000BF4FF
	public Transform clickFocus { get; set; }

	// Token: 0x17000214 RID: 532
	// (get) Token: 0x060032E3 RID: 13027 RVA: 0x000C1308 File Offset: 0x000BF508
	// (set) Token: 0x060032E4 RID: 13028 RVA: 0x000C1310 File Offset: 0x000BF510
	public float Time { get; set; }

	// Token: 0x17000215 RID: 533
	// (get) Token: 0x060032E5 RID: 13029 RVA: 0x000C1319 File Offset: 0x000BF519
	// (set) Token: 0x060032E6 RID: 13030 RVA: 0x000C1321 File Offset: 0x000BF521
	public float GameTime { get; set; }

	// Token: 0x17000216 RID: 534
	// (get) Token: 0x060032E7 RID: 13031 RVA: 0x000C132A File Offset: 0x000BF52A
	// (set) Token: 0x060032E8 RID: 13032 RVA: 0x000C1332 File Offset: 0x000BF532
	public float Delay { get; set; }

	// Token: 0x17000217 RID: 535
	// (get) Token: 0x060032E9 RID: 13033 RVA: 0x000C133B File Offset: 0x000BF53B
	// (set) Token: 0x060032EA RID: 13034 RVA: 0x000C1343 File Offset: 0x000BF543
	public int Idx { get; set; }

	// Token: 0x17000218 RID: 536
	// (get) Token: 0x060032EB RID: 13035 RVA: 0x000C134C File Offset: 0x000BF54C
	// (set) Token: 0x060032EC RID: 13036 RVA: 0x000C1354 File Offset: 0x000BF554
	public Func<List<Notification>, object, string> ToolTip { get; set; }

	// Token: 0x060032ED RID: 13037 RVA: 0x000C135D File Offset: 0x000BF55D
	public bool IsReady()
	{
		return UnityEngine.Time.time >= this.GameTime + this.Delay;
	}

	// Token: 0x17000219 RID: 537
	// (get) Token: 0x060032EE RID: 13038 RVA: 0x000C1376 File Offset: 0x000BF576
	// (set) Token: 0x060032EF RID: 13039 RVA: 0x000C137E File Offset: 0x000BF57E
	public string titleText { get; private set; }

	// Token: 0x1700021A RID: 538
	// (get) Token: 0x060032F0 RID: 13040 RVA: 0x000C1387 File Offset: 0x000BF587
	// (set) Token: 0x060032F1 RID: 13041 RVA: 0x000C138F File Offset: 0x000BF58F
	public string NotifierName
	{
		get
		{
			return this.notifierName;
		}
		set
		{
			this.notifierName = value;
			this.titleText = this.ReplaceTags(this.titleText);
		}
	}

	// Token: 0x060032F2 RID: 13042 RVA: 0x002049D8 File Offset: 0x00202BD8
	public Notification(string title, NotificationType type, Func<List<Notification>, object, string> tooltip = null, object tooltip_data = null, bool expires = true, float delay = 0f, Notification.ClickCallback custom_click_callback = null, object custom_click_data = null, Transform click_focus = null, bool volume_attenuation = true, bool clear_on_click = false, bool show_dismiss_button = false)
	{
		this.titleText = title;
		this.Type = type;
		this.ToolTip = tooltip;
		this.tooltipData = tooltip_data;
		this.expires = expires;
		this.Delay = delay;
		this.customClickCallback = custom_click_callback;
		this.customClickData = custom_click_data;
		this.clickFocus = click_focus;
		this.volume_attenuation = volume_attenuation;
		this.clearOnClick = clear_on_click;
		this.showDismissButton = show_dismiss_button;
		int num = this.notificationIncrement;
		this.notificationIncrement = num + 1;
		this.Idx = num;
	}

	// Token: 0x060032F3 RID: 13043 RVA: 0x000C13AA File Offset: 0x000BF5AA
	public void Clear()
	{
		if (this.Notifier != null)
		{
			this.Notifier.Remove(this);
			return;
		}
		NotificationManager.Instance.RemoveNotification(this);
	}

	// Token: 0x060032F4 RID: 13044 RVA: 0x00204A74 File Offset: 0x00202C74
	private string ReplaceTags(string text)
	{
		DebugUtil.Assert(text != null);
		int num = text.IndexOf('{');
		int num2 = text.IndexOf('}');
		if (0 <= num && num < num2)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int num3 = 0;
			while (0 <= num)
			{
				string value = text.Substring(num3, num - num3);
				stringBuilder.Append(value);
				num2 = text.IndexOf('}', num);
				if (num >= num2)
				{
					break;
				}
				string tag = text.Substring(num + 1, num2 - num - 1);
				string tagDescription = this.GetTagDescription(tag);
				stringBuilder.Append(tagDescription);
				num3 = num2 + 1;
				num = text.IndexOf('{', num2);
			}
			stringBuilder.Append(text.Substring(num3, text.Length - num3));
			return stringBuilder.ToString();
		}
		return text;
	}

	// Token: 0x060032F5 RID: 13045 RVA: 0x00204B28 File Offset: 0x00202D28
	private string GetTagDescription(string tag)
	{
		string result;
		if (tag == "NotifierName")
		{
			result = this.notifierName;
		}
		else
		{
			result = "UNKNOWN TAG: " + tag;
		}
		return result;
	}

	// Token: 0x04002244 RID: 8772
	public object tooltipData;

	// Token: 0x04002245 RID: 8773
	public bool expires = true;

	// Token: 0x04002246 RID: 8774
	public bool playSound = true;

	// Token: 0x04002247 RID: 8775
	public bool volume_attenuation = true;

	// Token: 0x04002248 RID: 8776
	public Notification.ClickCallback customClickCallback;

	// Token: 0x04002249 RID: 8777
	public bool clearOnClick;

	// Token: 0x0400224A RID: 8778
	public bool showDismissButton;

	// Token: 0x0400224B RID: 8779
	public object customClickData;

	// Token: 0x0400224C RID: 8780
	private int notificationIncrement;

	// Token: 0x0400224E RID: 8782
	private string notifierName;

	// Token: 0x02000AAE RID: 2734
	// (Invoke) Token: 0x060032F7 RID: 13047
	public delegate void ClickCallback(object data);
}
