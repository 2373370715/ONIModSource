using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;
using UnityEngine.UI;

public class NotificationScreen : KScreen
{
	private class Entry
	{
		public string message;

		public int clickIdx;

		public GameObject label;

		public List<Notification> notifications = new List<Notification>();

		public Notification NextClickedNotification => notifications[clickIdx++ % notifications.Count];

		public Entry(GameObject label)
		{
			this.label = label;
		}

		public void Add(Notification notification)
		{
			notifications.Add(notification);
			UpdateMessage(notification);
		}

		public void Remove(Notification notification)
		{
			notifications.Remove(notification);
			UpdateMessage(notification, playSound: false);
		}

		public void UpdateMessage(Notification notification, bool playSound = true)
		{
			if (Game.IsQuitting())
			{
				return;
			}
			message = notification.titleText;
			if (notifications.Count > 1)
			{
				if (playSound && (notification.Type == NotificationType.Bad || notification.Type == NotificationType.DuplicantThreatening))
				{
					Instance.PlayDingSound(notification, notifications.Count);
				}
				message = message + " (" + notifications.Count + ")";
			}
			if (label != null)
			{
				label.GetComponent<HierarchyReferences>().GetReference<LocText>("Text").text = message;
			}
		}
	}

	public float lifetime;

	public bool dirty;

	public GameObject LabelPrefab;

	public GameObject LabelsFolder;

	public GameObject MessagesPrefab;

	public GameObject MessagesFolder;

	private MessageDialogFrame messageDialog;

	private float initTime;

	[MyCmpAdd]
	private Notifier notifier;

	[SerializeField]
	private List<MessageDialog> dialogPrefabs = new List<MessageDialog>();

	[SerializeField]
	private Color badColorBG;

	[SerializeField]
	private Color badColor = Color.red;

	[SerializeField]
	private Color normalColorBG;

	[SerializeField]
	private Color normalColor = Color.white;

	[SerializeField]
	private Color warningColorBG;

	[SerializeField]
	private Color warningColor;

	[SerializeField]
	private Color messageColorBG;

	[SerializeField]
	private Color messageColor;

	[SerializeField]
	private Color messageImportantColorBG;

	[SerializeField]
	private Color messageImportantColor;

	[SerializeField]
	private Color eventColorBG;

	[SerializeField]
	private Color eventColor;

	public Sprite icon_normal;

	public Sprite icon_warning;

	public Sprite icon_bad;

	public Sprite icon_threatening;

	public Sprite icon_message;

	public Sprite icon_message_important;

	public Sprite icon_video;

	public Sprite icon_event;

	private List<Notification> pendingNotifications = new List<Notification>();

	private List<Notification> notifications = new List<Notification>();

	public TextStyleSetting TooltipTextStyle;

	private Dictionary<NotificationType, string> notificationSounds = new Dictionary<NotificationType, string>();

	private Dictionary<string, float> timeOfLastNotification = new Dictionary<string, float>();

	private float soundDecayTime = 10f;

	private List<Entry> entries = new List<Entry>();

	private Dictionary<string, Entry> entriesByMessage = new Dictionary<string, Entry>();

	public static NotificationScreen Instance { get; private set; }

	public static void DestroyInstance()
	{
		Instance = null;
	}

	public void AddPendingNotification(Notification notification)
	{
		pendingNotifications.Add(notification);
	}

	public void RemovePendingNotification(Notification notification)
	{
		dirty = true;
		pendingNotifications.Remove(notification);
		RemoveNotification(notification);
	}

	public void RemoveNotification(Notification notification)
	{
		Entry value = null;
		entriesByMessage.TryGetValue(notification.titleText, out value);
		if (value != null)
		{
			notifications.Remove(notification);
			value.Remove(notification);
			if (value.notifications.Count == 0)
			{
				Object.Destroy(value.label);
				entriesByMessage[notification.titleText] = null;
				entries.Remove(value);
			}
		}
	}

	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Instance = this;
		MessagesPrefab.gameObject.SetActive(value: false);
		LabelPrefab.gameObject.SetActive(value: false);
		InitNotificationSounds();
	}

	private void OnNewMessage(object data)
	{
		Message m = (Message)data;
		notifier.Add(new MessageNotification(m));
	}

	private void ShowMessage(MessageNotification mn)
	{
		mn.message.OnClick();
		if (mn.message.ShowDialog())
		{
			for (int i = 0; i < dialogPrefabs.Count; i++)
			{
				if (dialogPrefabs[i].CanDisplay(mn.message))
				{
					if (messageDialog != null)
					{
						Object.Destroy(messageDialog.gameObject);
						messageDialog = null;
					}
					messageDialog = Util.KInstantiateUI<MessageDialogFrame>(ScreenPrefabs.Instance.MessageDialogFrame.gameObject, GameScreenManager.Instance.ssOverlayCanvas.gameObject);
					MessageDialog dialog = Util.KInstantiateUI<MessageDialog>(dialogPrefabs[i].gameObject, GameScreenManager.Instance.ssOverlayCanvas.gameObject);
					messageDialog.SetMessage(dialog, mn.message);
					messageDialog.Show();
					break;
				}
			}
		}
		Messenger.Instance.RemoveMessage(mn.message);
		mn.Clear();
	}

	public void OnClickNextMessage()
	{
		Notification notification2 = notifications.Find((Notification notification) => notification.Type == NotificationType.Messages);
		ShowMessage((MessageNotification)notification2);
	}

	protected override void OnSpawn()
	{
		base.OnSpawn();
		initTime = KTime.Instance.UnscaledGameTime;
		LocText[] componentsInChildren = LabelPrefab.GetComponentsInChildren<LocText>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].color = GlobalAssets.Instance.colorSet.NotificationNormal;
		}
		componentsInChildren = MessagesPrefab.GetComponentsInChildren<LocText>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].color = GlobalAssets.Instance.colorSet.NotificationNormal;
		}
		Subscribe(Messenger.Instance.gameObject, 1558809273, OnNewMessage);
		foreach (Message message in Messenger.Instance.Messages)
		{
			Notification notification = new MessageNotification(message);
			notification.playSound = false;
			notifier.Add(notification);
		}
	}

	protected override void OnActivate()
	{
		base.OnActivate();
		dirty = true;
	}

	public void AddNotification(Notification notification)
	{
		if (DebugHandler.NotificationsDisabled)
		{
			return;
		}
		notifications.Add(notification);
		entriesByMessage.TryGetValue(notification.titleText, out var entry);
		if (entry == null)
		{
			HierarchyReferences hierarchyReferences = ((notification.Type != NotificationType.Messages) ? Util.KInstantiateUI<HierarchyReferences>(LabelPrefab, LabelsFolder) : Util.KInstantiateUI<HierarchyReferences>(MessagesPrefab, MessagesFolder));
			Button reference = hierarchyReferences.GetReference<Button>("DismissButton");
			reference.gameObject.SetActive(notification.showDismissButton);
			if (notification.showDismissButton)
			{
				reference.onClick.AddListener(delegate
				{
					if (entriesByMessage.TryGetValue(notification.titleText, out var value))
					{
						for (int num = value.notifications.Count - 1; num >= 0; num--)
						{
							Notification notification2 = value.notifications[num];
							if (notification2 is MessageNotification messageNotification2)
							{
								Messenger.Instance.RemoveMessage(messageNotification2.message);
							}
							notification2.Clear();
						}
					}
				});
			}
			hierarchyReferences.GetReference<NotificationAnimator>("Animator").Begin();
			hierarchyReferences.gameObject.SetActive(value: true);
			if (notification.ToolTip != null)
			{
				ToolTip tooltip = hierarchyReferences.GetReference<ToolTip>("ToolTip");
				tooltip.OnToolTip = delegate
				{
					tooltip.ClearMultiStringTooltip();
					tooltip.AddMultiStringTooltip(notification.ToolTip(entry.notifications, notification.tooltipData), TooltipTextStyle);
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
				reference2.sprite = ((notification.Type == NotificationType.Bad) ? icon_bad : icon_threatening);
				break;
			case NotificationType.Tutorial:
				colors.normalColor = GlobalAssets.Instance.colorSet.NotificationTutorialBG;
				reference3.color = GlobalAssets.Instance.colorSet.NotificationTutorial;
				reference2.color = GlobalAssets.Instance.colorSet.NotificationTutorial;
				reference2.sprite = icon_warning;
				break;
			case NotificationType.Messages:
				colors.normalColor = GlobalAssets.Instance.colorSet.NotificationMessageBG;
				reference3.color = GlobalAssets.Instance.colorSet.NotificationMessage;
				reference2.color = GlobalAssets.Instance.colorSet.NotificationMessage;
				reference2.sprite = icon_message;
				if (notification is MessageNotification { message: TutorialMessage message } && !string.IsNullOrEmpty(message.videoClipId))
				{
					reference2.sprite = icon_video;
				}
				break;
			case NotificationType.MessageImportant:
				colors.normalColor = GlobalAssets.Instance.colorSet.NotificationMessageImportantBG;
				reference3.color = GlobalAssets.Instance.colorSet.NotificationMessageImportant;
				reference2.color = GlobalAssets.Instance.colorSet.NotificationMessageImportant;
				reference2.sprite = icon_message_important;
				break;
			case NotificationType.Event:
				colors.normalColor = GlobalAssets.Instance.colorSet.NotificationEventBG;
				reference3.color = GlobalAssets.Instance.colorSet.NotificationEvent;
				reference2.color = GlobalAssets.Instance.colorSet.NotificationEvent;
				reference2.sprite = icon_event;
				break;
			default:
				colors.normalColor = GlobalAssets.Instance.colorSet.NotificationNormalBG;
				reference3.color = GlobalAssets.Instance.colorSet.NotificationNormal;
				reference2.color = GlobalAssets.Instance.colorSet.NotificationNormal;
				reference2.sprite = icon_normal;
				break;
			}
			reference4.colors = colors;
			reference4.onClick.AddListener(delegate
			{
				OnClick(entry);
			});
			string text = "";
			if (KTime.Instance.UnscaledGameTime - initTime > 5f && notification.playSound)
			{
				PlayDingSound(notification, 0);
			}
			else
			{
				text = "too early";
			}
			if (AudioDebug.Get().debugNotificationSounds)
			{
				Debug.Log("Notification(" + notification.titleText + "):" + text);
			}
			entry = new Entry(hierarchyReferences.gameObject);
			entriesByMessage[notification.titleText] = entry;
			entries.Add(entry);
		}
		entry.Add(notification);
		dirty = true;
		SortNotifications();
	}

	private void SortNotifications()
	{
		notifications.Sort((Notification n1, Notification n2) => (n1.Type == n2.Type) ? (n1.Idx - n2.Idx) : (n1.Type - n2.Type));
		foreach (Notification notification in notifications)
		{
			Entry value = null;
			entriesByMessage.TryGetValue(notification.titleText, out value);
			value?.label.GetComponent<RectTransform>().SetAsLastSibling();
		}
	}

	private void PlayDingSound(Notification notification, int count)
	{
		if (!notificationSounds.TryGetValue(notification.Type, out var value))
		{
			value = "Notification";
		}
		if (!timeOfLastNotification.TryGetValue(value, out var value2))
		{
			value2 = 0f;
		}
		float value3 = (notification.volume_attenuation ? ((Time.time - value2) / soundDecayTime) : 1f);
		timeOfLastNotification[value] = Time.time;
		string sound;
		if (count > 1)
		{
			sound = GlobalAssets.GetSound(value + "_AddCount", force_no_warning: true);
			if (sound == null)
			{
				sound = GlobalAssets.GetSound(value);
			}
		}
		else
		{
			sound = GlobalAssets.GetSound(value);
		}
		if (notification.playSound)
		{
			EventInstance instance = KFMOD.BeginOneShot(sound, Vector3.zero);
			instance.setParameterByName("timeSinceLast", value3);
			KFMOD.EndOneShot(instance);
		}
	}

	private void Update()
	{
		int num = 0;
		while (num < pendingNotifications.Count)
		{
			if (pendingNotifications[num].IsReady())
			{
				AddNotification(pendingNotifications[num]);
				pendingNotifications.RemoveAt(num);
			}
			else
			{
				num++;
			}
		}
		int num2 = 0;
		int num3 = 0;
		for (int i = 0; i < notifications.Count; i++)
		{
			Notification notification = notifications[i];
			if (notification.Type == NotificationType.Messages)
			{
				num3++;
			}
			else
			{
				num2++;
			}
			if (notification.expires && KTime.Instance.UnscaledGameTime - notification.Time > lifetime)
			{
				dirty = true;
				if (notification.Notifier == null)
				{
					RemovePendingNotification(notification);
				}
				else
				{
					notification.Clear();
				}
			}
		}
	}

	private void OnClick(Entry entry)
	{
		Notification nextClickedNotification = entry.NextClickedNotification;
		PlaySound3D(GlobalAssets.GetSound("HUD_Click_Open"));
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
					CameraController.Instance.ActiveWorldStarWipe(myWorldId, position);
				}
				else if (DlcManager.FeatureClusterSpaceEnabled() && component != null && component.IsVisible)
				{
					ManagementMenu.Instance.OpenClusterMap();
					ClusterMapScreen.Instance.SetTargetFocusPosition(component.Location);
				}
				if (component2 != null)
				{
					if (DlcManager.FeatureClusterSpaceEnabled() && component != null && component.IsVisible)
					{
						ClusterMapSelectTool.Instance.Select(component2);
					}
					else
					{
						SelectTool.Instance.Select(component2);
					}
				}
			}
			else if (nextClickedNotification.Notifier != null)
			{
				SelectTool.Instance.Select(nextClickedNotification.Notifier.GetComponent<KSelectable>());
			}
			if (nextClickedNotification.Type == NotificationType.Messages)
			{
				ShowMessage((MessageNotification)nextClickedNotification);
			}
		}
		if (nextClickedNotification.clearOnClick)
		{
			nextClickedNotification.Clear();
		}
	}

	private void PositionLocatorIcon()
	{
	}

	private void InitNotificationSounds()
	{
		notificationSounds[NotificationType.Good] = "Notification";
		notificationSounds[NotificationType.BadMinor] = "Notification";
		notificationSounds[NotificationType.Bad] = "Warning";
		notificationSounds[NotificationType.Neutral] = "Notification";
		notificationSounds[NotificationType.Tutorial] = "Notification";
		notificationSounds[NotificationType.Messages] = "Message";
		notificationSounds[NotificationType.DuplicantThreatening] = "Warning_DupeThreatening";
		notificationSounds[NotificationType.Event] = "Message";
		notificationSounds[NotificationType.MessageImportant] = "Message_Important";
	}

	public Sprite GetNotificationIcon(NotificationType type)
	{
		return type switch
		{
			NotificationType.Bad => icon_bad, 
			NotificationType.DuplicantThreatening => icon_threatening, 
			NotificationType.Tutorial => icon_warning, 
			NotificationType.Messages => icon_message, 
			NotificationType.Event => icon_event, 
			NotificationType.MessageImportant => icon_message_important, 
			_ => icon_normal, 
		};
	}

	public Color GetNotificationColour(NotificationType type)
	{
		return type switch
		{
			NotificationType.Bad => GlobalAssets.Instance.colorSet.NotificationBad, 
			NotificationType.DuplicantThreatening => GlobalAssets.Instance.colorSet.NotificationBad, 
			NotificationType.Tutorial => GlobalAssets.Instance.colorSet.NotificationTutorial, 
			NotificationType.Messages => GlobalAssets.Instance.colorSet.NotificationMessage, 
			NotificationType.Event => GlobalAssets.Instance.colorSet.NotificationEvent, 
			NotificationType.MessageImportant => GlobalAssets.Instance.colorSet.NotificationMessageImportant, 
			_ => GlobalAssets.Instance.colorSet.NotificationNormal, 
		};
	}

	public Color GetNotificationBGColour(NotificationType type)
	{
		return type switch
		{
			NotificationType.Bad => GlobalAssets.Instance.colorSet.NotificationBadBG, 
			NotificationType.DuplicantThreatening => GlobalAssets.Instance.colorSet.NotificationBadBG, 
			NotificationType.Tutorial => GlobalAssets.Instance.colorSet.NotificationTutorialBG, 
			NotificationType.Messages => GlobalAssets.Instance.colorSet.NotificationMessageBG, 
			NotificationType.Event => GlobalAssets.Instance.colorSet.NotificationEventBG, 
			NotificationType.MessageImportant => GlobalAssets.Instance.colorSet.NotificationMessageImportantBG, 
			_ => GlobalAssets.Instance.colorSet.NotificationNormalBG, 
		};
	}

	public string GetNotificationSound(NotificationType type)
	{
		return notificationSounds[type];
	}
}
