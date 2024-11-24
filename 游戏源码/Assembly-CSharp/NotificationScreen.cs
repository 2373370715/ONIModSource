using System;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001E54 RID: 7764
public class NotificationScreen : KScreen
{
	// Token: 0x17000A76 RID: 2678
	// (get) Token: 0x0600A2B1 RID: 41649 RVA: 0x0010988B File Offset: 0x00107A8B
	// (set) Token: 0x0600A2B2 RID: 41650 RVA: 0x00109892 File Offset: 0x00107A92
	public static NotificationScreen Instance { get; private set; }

	// Token: 0x0600A2B3 RID: 41651 RVA: 0x0010989A File Offset: 0x00107A9A
	public static void DestroyInstance()
	{
		NotificationScreen.Instance = null;
	}

	// Token: 0x0600A2B4 RID: 41652 RVA: 0x001098A2 File Offset: 0x00107AA2
	public void AddPendingNotification(Notification notification)
	{
		this.pendingNotifications.Add(notification);
	}

	// Token: 0x0600A2B5 RID: 41653 RVA: 0x001098B0 File Offset: 0x00107AB0
	public void RemovePendingNotification(Notification notification)
	{
		this.dirty = true;
		this.pendingNotifications.Remove(notification);
		this.RemoveNotification(notification);
	}

	// Token: 0x0600A2B6 RID: 41654 RVA: 0x003DE800 File Offset: 0x003DCA00
	public void RemoveNotification(Notification notification)
	{
		NotificationScreen.Entry entry = null;
		this.entriesByMessage.TryGetValue(notification.titleText, out entry);
		if (entry == null)
		{
			return;
		}
		this.notifications.Remove(notification);
		entry.Remove(notification);
		if (entry.notifications.Count == 0)
		{
			UnityEngine.Object.Destroy(entry.label);
			this.entriesByMessage[notification.titleText] = null;
			this.entries.Remove(entry);
		}
	}

	// Token: 0x0600A2B7 RID: 41655 RVA: 0x001098CD File Offset: 0x00107ACD
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		NotificationScreen.Instance = this;
		this.MessagesPrefab.gameObject.SetActive(false);
		this.LabelPrefab.gameObject.SetActive(false);
		this.InitNotificationSounds();
	}

	// Token: 0x0600A2B8 RID: 41656 RVA: 0x003DE874 File Offset: 0x003DCA74
	private void OnNewMessage(object data)
	{
		Message m = (Message)data;
		this.notifier.Add(new MessageNotification(m), "");
	}

	// Token: 0x0600A2B9 RID: 41657 RVA: 0x003DE8A0 File Offset: 0x003DCAA0
	private void ShowMessage(MessageNotification mn)
	{
		mn.message.OnClick();
		if (mn.message.ShowDialog())
		{
			for (int i = 0; i < this.dialogPrefabs.Count; i++)
			{
				if (this.dialogPrefabs[i].CanDisplay(mn.message))
				{
					if (this.messageDialog != null)
					{
						UnityEngine.Object.Destroy(this.messageDialog.gameObject);
						this.messageDialog = null;
					}
					this.messageDialog = global::Util.KInstantiateUI<MessageDialogFrame>(ScreenPrefabs.Instance.MessageDialogFrame.gameObject, GameScreenManager.Instance.ssOverlayCanvas.gameObject, false);
					MessageDialog dialog = global::Util.KInstantiateUI<MessageDialog>(this.dialogPrefabs[i].gameObject, GameScreenManager.Instance.ssOverlayCanvas.gameObject, false);
					this.messageDialog.SetMessage(dialog, mn.message);
					this.messageDialog.Show(true);
					break;
				}
			}
		}
		Messenger.Instance.RemoveMessage(mn.message);
		mn.Clear();
	}

	// Token: 0x0600A2BA RID: 41658 RVA: 0x003DE9AC File Offset: 0x003DCBAC
	public void OnClickNextMessage()
	{
		Notification notification2 = this.notifications.Find((Notification notification) => notification.Type == NotificationType.Messages);
		this.ShowMessage((MessageNotification)notification2);
	}

	// Token: 0x0600A2BB RID: 41659 RVA: 0x003DE9F0 File Offset: 0x003DCBF0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.initTime = KTime.Instance.UnscaledGameTime;
		LocText[] componentsInChildren = this.LabelPrefab.GetComponentsInChildren<LocText>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].color = GlobalAssets.Instance.colorSet.NotificationNormal;
		}
		componentsInChildren = this.MessagesPrefab.GetComponentsInChildren<LocText>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].color = GlobalAssets.Instance.colorSet.NotificationNormal;
		}
		base.Subscribe(Messenger.Instance.gameObject, 1558809273, new Action<object>(this.OnNewMessage));
		foreach (Message m in Messenger.Instance.Messages)
		{
			Notification notification = new MessageNotification(m);
			notification.playSound = false;
			this.notifier.Add(notification, "");
		}
	}

	// Token: 0x0600A2BC RID: 41660 RVA: 0x00109903 File Offset: 0x00107B03
	protected override void OnActivate()
	{
		base.OnActivate();
		this.dirty = true;
	}

	// Token: 0x0600A2BD RID: 41661 RVA: 0x003DEAFC File Offset: 0x003DCCFC
	public void AddNotification(Notification notification)
	{
		if (DebugHandler.NotificationsDisabled)
		{
			return;
		}
		this.notifications.Add(notification);
		NotificationScreen.Entry entry;
		this.entriesByMessage.TryGetValue(notification.titleText, out entry);
		if (entry == null)
		{
			HierarchyReferences hierarchyReferences;
			if (notification.Type == NotificationType.Messages)
			{
				hierarchyReferences = global::Util.KInstantiateUI<HierarchyReferences>(this.MessagesPrefab, this.MessagesFolder, false);
			}
			else
			{
				hierarchyReferences = global::Util.KInstantiateUI<HierarchyReferences>(this.LabelPrefab, this.LabelsFolder, false);
			}
			Button reference = hierarchyReferences.GetReference<Button>("DismissButton");
			reference.gameObject.SetActive(notification.showDismissButton);
			if (notification.showDismissButton)
			{
				reference.onClick.AddListener(delegate()
				{
					NotificationScreen.Entry entry;
					if (!this.entriesByMessage.TryGetValue(notification.titleText, out entry))
					{
						return;
					}
					for (int i = entry.notifications.Count - 1; i >= 0; i--)
					{
						Notification notification2 = entry.notifications[i];
						MessageNotification messageNotification2 = notification2 as MessageNotification;
						if (messageNotification2 != null)
						{
							Messenger.Instance.RemoveMessage(messageNotification2.message);
						}
						notification2.Clear();
					}
				});
			}
			hierarchyReferences.GetReference<NotificationAnimator>("Animator").Begin(true);
			hierarchyReferences.gameObject.SetActive(true);
			if (notification.ToolTip != null)
			{
				ToolTip tooltip = hierarchyReferences.GetReference<ToolTip>("ToolTip");
				tooltip.OnToolTip = delegate()
				{
					tooltip.ClearMultiStringTooltip();
					tooltip.AddMultiStringTooltip(notification.ToolTip(entry.notifications, notification.tooltipData), this.TooltipTextStyle);
					return "";
				};
			}
			KImage reference2 = hierarchyReferences.GetReference<KImage>("Icon");
			LocText reference3 = hierarchyReferences.GetReference<LocText>("Text");
			Button reference4 = hierarchyReferences.GetReference<Button>("MainButton");
			ColorBlock colors = reference4.colors;
			switch (notification.Type)
			{
			case NotificationType.Bad:
			case NotificationType.DuplicantThreatening:
				colors.normalColor = GlobalAssets.Instance.colorSet.NotificationBadBG;
				reference3.color = GlobalAssets.Instance.colorSet.NotificationBad;
				reference2.color = GlobalAssets.Instance.colorSet.NotificationBad;
				reference2.sprite = ((notification.Type == NotificationType.Bad) ? this.icon_bad : this.icon_threatening);
				goto IL_43D;
			case NotificationType.Tutorial:
				colors.normalColor = GlobalAssets.Instance.colorSet.NotificationTutorialBG;
				reference3.color = GlobalAssets.Instance.colorSet.NotificationTutorial;
				reference2.color = GlobalAssets.Instance.colorSet.NotificationTutorial;
				reference2.sprite = this.icon_warning;
				goto IL_43D;
			case NotificationType.Messages:
			{
				colors.normalColor = GlobalAssets.Instance.colorSet.NotificationMessageBG;
				reference3.color = GlobalAssets.Instance.colorSet.NotificationMessage;
				reference2.color = GlobalAssets.Instance.colorSet.NotificationMessage;
				reference2.sprite = this.icon_message;
				MessageNotification messageNotification = notification as MessageNotification;
				if (messageNotification == null)
				{
					goto IL_43D;
				}
				TutorialMessage tutorialMessage = messageNotification.message as TutorialMessage;
				if (tutorialMessage != null && !string.IsNullOrEmpty(tutorialMessage.videoClipId))
				{
					reference2.sprite = this.icon_video;
					goto IL_43D;
				}
				goto IL_43D;
			}
			case NotificationType.Event:
				colors.normalColor = GlobalAssets.Instance.colorSet.NotificationEventBG;
				reference3.color = GlobalAssets.Instance.colorSet.NotificationEvent;
				reference2.color = GlobalAssets.Instance.colorSet.NotificationEvent;
				reference2.sprite = this.icon_event;
				goto IL_43D;
			case NotificationType.MessageImportant:
				colors.normalColor = GlobalAssets.Instance.colorSet.NotificationMessageImportantBG;
				reference3.color = GlobalAssets.Instance.colorSet.NotificationMessageImportant;
				reference2.color = GlobalAssets.Instance.colorSet.NotificationMessageImportant;
				reference2.sprite = this.icon_message_important;
				goto IL_43D;
			}
			colors.normalColor = GlobalAssets.Instance.colorSet.NotificationNormalBG;
			reference3.color = GlobalAssets.Instance.colorSet.NotificationNormal;
			reference2.color = GlobalAssets.Instance.colorSet.NotificationNormal;
			reference2.sprite = this.icon_normal;
			IL_43D:
			reference4.colors = colors;
			reference4.onClick.AddListener(delegate()
			{
				this.OnClick(entry);
			});
			string str = "";
			if (KTime.Instance.UnscaledGameTime - this.initTime > 5f && notification.playSound)
			{
				this.PlayDingSound(notification, 0);
			}
			else
			{
				str = "too early";
			}
			if (AudioDebug.Get().debugNotificationSounds)
			{
				global::Debug.Log("Notification(" + notification.titleText + "):" + str);
			}
			entry = new NotificationScreen.Entry(hierarchyReferences.gameObject);
			this.entriesByMessage[notification.titleText] = entry;
			this.entries.Add(entry);
		}
		entry.Add(notification);
		this.dirty = true;
		this.SortNotifications();
	}

	// Token: 0x0600A2BE RID: 41662 RVA: 0x003DF034 File Offset: 0x003DD234
	private void SortNotifications()
	{
		this.notifications.Sort(delegate(Notification n1, Notification n2)
		{
			if (n1.Type == n2.Type)
			{
				return n1.Idx - n2.Idx;
			}
			return n1.Type - n2.Type;
		});
		foreach (Notification notification in this.notifications)
		{
			NotificationScreen.Entry entry = null;
			this.entriesByMessage.TryGetValue(notification.titleText, out entry);
			if (entry != null)
			{
				entry.label.GetComponent<RectTransform>().SetAsLastSibling();
			}
		}
	}

	// Token: 0x0600A2BF RID: 41663 RVA: 0x003DF0D4 File Offset: 0x003DD2D4
	private void PlayDingSound(Notification notification, int count)
	{
		string text;
		if (!this.notificationSounds.TryGetValue(notification.Type, out text))
		{
			text = "Notification";
		}
		float num;
		if (!this.timeOfLastNotification.TryGetValue(text, out num))
		{
			num = 0f;
		}
		float value = notification.volume_attenuation ? ((Time.time - num) / this.soundDecayTime) : 1f;
		this.timeOfLastNotification[text] = Time.time;
		string sound;
		if (count > 1)
		{
			sound = GlobalAssets.GetSound(text + "_AddCount", true);
			if (sound == null)
			{
				sound = GlobalAssets.GetSound(text, false);
			}
		}
		else
		{
			sound = GlobalAssets.GetSound(text, false);
		}
		if (notification.playSound)
		{
			EventInstance instance = KFMOD.BeginOneShot(sound, Vector3.zero, 1f);
			instance.setParameterByName("timeSinceLast", value, false);
			KFMOD.EndOneShot(instance);
		}
	}

	// Token: 0x0600A2C0 RID: 41664 RVA: 0x003DF1A0 File Offset: 0x003DD3A0
	private void Update()
	{
		int i = 0;
		while (i < this.pendingNotifications.Count)
		{
			if (this.pendingNotifications[i].IsReady())
			{
				this.AddNotification(this.pendingNotifications[i]);
				this.pendingNotifications.RemoveAt(i);
			}
			else
			{
				i++;
			}
		}
		int num = 0;
		int num2 = 0;
		for (int j = 0; j < this.notifications.Count; j++)
		{
			Notification notification = this.notifications[j];
			if (notification.Type == NotificationType.Messages)
			{
				num2++;
			}
			else
			{
				num++;
			}
			if (notification.expires && KTime.Instance.UnscaledGameTime - notification.Time > this.lifetime)
			{
				this.dirty = true;
				if (notification.Notifier == null)
				{
					this.RemovePendingNotification(notification);
				}
				else
				{
					notification.Clear();
				}
			}
		}
	}

	// Token: 0x0600A2C1 RID: 41665 RVA: 0x003DF27C File Offset: 0x003DD47C
	private void OnClick(NotificationScreen.Entry entry)
	{
		Notification nextClickedNotification = entry.NextClickedNotification;
		base.PlaySound3D(GlobalAssets.GetSound("HUD_Click_Open", false));
		if (nextClickedNotification.customClickCallback != null)
		{
			nextClickedNotification.customClickCallback(nextClickedNotification.customClickData);
		}
		else
		{
			if (nextClickedNotification.clickFocus != null)
			{
				Vector3 position = nextClickedNotification.clickFocus.GetPosition();
				position.z = -40f;
				ClusterGridEntity component = nextClickedNotification.clickFocus.GetComponent<ClusterGridEntity>();
				KSelectable component2 = nextClickedNotification.clickFocus.GetComponent<KSelectable>();
				int myWorldId = nextClickedNotification.clickFocus.gameObject.GetMyWorldId();
				if (myWorldId != -1)
				{
					CameraController.Instance.ActiveWorldStarWipe(myWorldId, position, 10f, null);
				}
				else if (DlcManager.FeatureClusterSpaceEnabled() && component != null && component.IsVisible)
				{
					ManagementMenu.Instance.OpenClusterMap();
					ClusterMapScreen.Instance.SetTargetFocusPosition(component.Location, 0.5f);
				}
				if (component2 != null)
				{
					if (DlcManager.FeatureClusterSpaceEnabled() && component != null && component.IsVisible)
					{
						ClusterMapSelectTool.Instance.Select(component2, false);
					}
					else
					{
						SelectTool.Instance.Select(component2, false);
					}
				}
			}
			else if (nextClickedNotification.Notifier != null)
			{
				SelectTool.Instance.Select(nextClickedNotification.Notifier.GetComponent<KSelectable>(), false);
			}
			if (nextClickedNotification.Type == NotificationType.Messages)
			{
				this.ShowMessage((MessageNotification)nextClickedNotification);
			}
		}
		if (nextClickedNotification.clearOnClick)
		{
			nextClickedNotification.Clear();
		}
	}

	// Token: 0x0600A2C2 RID: 41666 RVA: 0x000A5E40 File Offset: 0x000A4040
	private void PositionLocatorIcon()
	{
	}

	// Token: 0x0600A2C3 RID: 41667 RVA: 0x003DF3E8 File Offset: 0x003DD5E8
	private void InitNotificationSounds()
	{
		this.notificationSounds[NotificationType.Good] = "Notification";
		this.notificationSounds[NotificationType.BadMinor] = "Notification";
		this.notificationSounds[NotificationType.Bad] = "Warning";
		this.notificationSounds[NotificationType.Neutral] = "Notification";
		this.notificationSounds[NotificationType.Tutorial] = "Notification";
		this.notificationSounds[NotificationType.Messages] = "Message";
		this.notificationSounds[NotificationType.DuplicantThreatening] = "Warning_DupeThreatening";
		this.notificationSounds[NotificationType.Event] = "Message";
		this.notificationSounds[NotificationType.MessageImportant] = "Message_Important";
	}

	// Token: 0x0600A2C4 RID: 41668 RVA: 0x003DF490 File Offset: 0x003DD690
	public Sprite GetNotificationIcon(NotificationType type)
	{
		switch (type)
		{
		case NotificationType.Bad:
			return this.icon_bad;
		case NotificationType.Tutorial:
			return this.icon_warning;
		case NotificationType.Messages:
			return this.icon_message;
		case NotificationType.DuplicantThreatening:
			return this.icon_threatening;
		case NotificationType.Event:
			return this.icon_event;
		case NotificationType.MessageImportant:
			return this.icon_message_important;
		}
		return this.icon_normal;
	}

	// Token: 0x0600A2C5 RID: 41669 RVA: 0x003DF4FC File Offset: 0x003DD6FC
	public Color GetNotificationColour(NotificationType type)
	{
		switch (type)
		{
		case NotificationType.Bad:
			return GlobalAssets.Instance.colorSet.NotificationBad;
		case NotificationType.Tutorial:
			return GlobalAssets.Instance.colorSet.NotificationTutorial;
		case NotificationType.Messages:
			return GlobalAssets.Instance.colorSet.NotificationMessage;
		case NotificationType.DuplicantThreatening:
			return GlobalAssets.Instance.colorSet.NotificationBad;
		case NotificationType.Event:
			return GlobalAssets.Instance.colorSet.NotificationEvent;
		case NotificationType.MessageImportant:
			return GlobalAssets.Instance.colorSet.NotificationMessageImportant;
		}
		return GlobalAssets.Instance.colorSet.NotificationNormal;
	}

	// Token: 0x0600A2C6 RID: 41670 RVA: 0x003DF5CC File Offset: 0x003DD7CC
	public Color GetNotificationBGColour(NotificationType type)
	{
		switch (type)
		{
		case NotificationType.Bad:
			return GlobalAssets.Instance.colorSet.NotificationBadBG;
		case NotificationType.Tutorial:
			return GlobalAssets.Instance.colorSet.NotificationTutorialBG;
		case NotificationType.Messages:
			return GlobalAssets.Instance.colorSet.NotificationMessageBG;
		case NotificationType.DuplicantThreatening:
			return GlobalAssets.Instance.colorSet.NotificationBadBG;
		case NotificationType.Event:
			return GlobalAssets.Instance.colorSet.NotificationEventBG;
		case NotificationType.MessageImportant:
			return GlobalAssets.Instance.colorSet.NotificationMessageImportantBG;
		}
		return GlobalAssets.Instance.colorSet.NotificationNormalBG;
	}

	// Token: 0x0600A2C7 RID: 41671 RVA: 0x00109912 File Offset: 0x00107B12
	public string GetNotificationSound(NotificationType type)
	{
		return this.notificationSounds[type];
	}

	// Token: 0x04007EF2 RID: 32498
	public float lifetime;

	// Token: 0x04007EF3 RID: 32499
	public bool dirty;

	// Token: 0x04007EF4 RID: 32500
	public GameObject LabelPrefab;

	// Token: 0x04007EF5 RID: 32501
	public GameObject LabelsFolder;

	// Token: 0x04007EF6 RID: 32502
	public GameObject MessagesPrefab;

	// Token: 0x04007EF7 RID: 32503
	public GameObject MessagesFolder;

	// Token: 0x04007EF8 RID: 32504
	private MessageDialogFrame messageDialog;

	// Token: 0x04007EF9 RID: 32505
	private float initTime;

	// Token: 0x04007EFA RID: 32506
	[MyCmpAdd]
	private Notifier notifier;

	// Token: 0x04007EFB RID: 32507
	[SerializeField]
	private List<MessageDialog> dialogPrefabs = new List<MessageDialog>();

	// Token: 0x04007EFC RID: 32508
	[SerializeField]
	private Color badColorBG;

	// Token: 0x04007EFD RID: 32509
	[SerializeField]
	private Color badColor = Color.red;

	// Token: 0x04007EFE RID: 32510
	[SerializeField]
	private Color normalColorBG;

	// Token: 0x04007EFF RID: 32511
	[SerializeField]
	private Color normalColor = Color.white;

	// Token: 0x04007F00 RID: 32512
	[SerializeField]
	private Color warningColorBG;

	// Token: 0x04007F01 RID: 32513
	[SerializeField]
	private Color warningColor;

	// Token: 0x04007F02 RID: 32514
	[SerializeField]
	private Color messageColorBG;

	// Token: 0x04007F03 RID: 32515
	[SerializeField]
	private Color messageColor;

	// Token: 0x04007F04 RID: 32516
	[SerializeField]
	private Color messageImportantColorBG;

	// Token: 0x04007F05 RID: 32517
	[SerializeField]
	private Color messageImportantColor;

	// Token: 0x04007F06 RID: 32518
	[SerializeField]
	private Color eventColorBG;

	// Token: 0x04007F07 RID: 32519
	[SerializeField]
	private Color eventColor;

	// Token: 0x04007F08 RID: 32520
	public Sprite icon_normal;

	// Token: 0x04007F09 RID: 32521
	public Sprite icon_warning;

	// Token: 0x04007F0A RID: 32522
	public Sprite icon_bad;

	// Token: 0x04007F0B RID: 32523
	public Sprite icon_threatening;

	// Token: 0x04007F0C RID: 32524
	public Sprite icon_message;

	// Token: 0x04007F0D RID: 32525
	public Sprite icon_message_important;

	// Token: 0x04007F0E RID: 32526
	public Sprite icon_video;

	// Token: 0x04007F0F RID: 32527
	public Sprite icon_event;

	// Token: 0x04007F10 RID: 32528
	private List<Notification> pendingNotifications = new List<Notification>();

	// Token: 0x04007F11 RID: 32529
	private List<Notification> notifications = new List<Notification>();

	// Token: 0x04007F12 RID: 32530
	public TextStyleSetting TooltipTextStyle;

	// Token: 0x04007F13 RID: 32531
	private Dictionary<NotificationType, string> notificationSounds = new Dictionary<NotificationType, string>();

	// Token: 0x04007F14 RID: 32532
	private Dictionary<string, float> timeOfLastNotification = new Dictionary<string, float>();

	// Token: 0x04007F15 RID: 32533
	private float soundDecayTime = 10f;

	// Token: 0x04007F16 RID: 32534
	private List<NotificationScreen.Entry> entries = new List<NotificationScreen.Entry>();

	// Token: 0x04007F17 RID: 32535
	private Dictionary<string, NotificationScreen.Entry> entriesByMessage = new Dictionary<string, NotificationScreen.Entry>();

	// Token: 0x02001E55 RID: 7765
	private class Entry
	{
		// Token: 0x0600A2C9 RID: 41673 RVA: 0x00109920 File Offset: 0x00107B20
		public Entry(GameObject label)
		{
			this.label = label;
		}

		// Token: 0x0600A2CA RID: 41674 RVA: 0x0010993A File Offset: 0x00107B3A
		public void Add(Notification notification)
		{
			this.notifications.Add(notification);
			this.UpdateMessage(notification, true);
		}

		// Token: 0x0600A2CB RID: 41675 RVA: 0x00109950 File Offset: 0x00107B50
		public void Remove(Notification notification)
		{
			this.notifications.Remove(notification);
			this.UpdateMessage(notification, false);
		}

		// Token: 0x0600A2CC RID: 41676 RVA: 0x003DF720 File Offset: 0x003DD920
		public void UpdateMessage(Notification notification, bool playSound = true)
		{
			if (Game.IsQuitting())
			{
				return;
			}
			this.message = notification.titleText;
			if (this.notifications.Count > 1)
			{
				if (playSound && (notification.Type == NotificationType.Bad || notification.Type == NotificationType.DuplicantThreatening))
				{
					NotificationScreen.Instance.PlayDingSound(notification, this.notifications.Count);
				}
				this.message = this.message + " (" + this.notifications.Count.ToString() + ")";
			}
			if (this.label != null)
			{
				this.label.GetComponent<HierarchyReferences>().GetReference<LocText>("Text").text = this.message;
			}
		}

		// Token: 0x17000A77 RID: 2679
		// (get) Token: 0x0600A2CD RID: 41677 RVA: 0x003DF7D8 File Offset: 0x003DD9D8
		public Notification NextClickedNotification
		{
			get
			{
				List<Notification> list = this.notifications;
				int num = this.clickIdx;
				this.clickIdx = num + 1;
				return list[num % this.notifications.Count];
			}
		}

		// Token: 0x04007F18 RID: 32536
		public string message;

		// Token: 0x04007F19 RID: 32537
		public int clickIdx;

		// Token: 0x04007F1A RID: 32538
		public GameObject label;

		// Token: 0x04007F1B RID: 32539
		public List<Notification> notifications = new List<Notification>();
	}
}
