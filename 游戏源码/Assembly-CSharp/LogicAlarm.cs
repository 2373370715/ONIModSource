using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000E31 RID: 3633
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/LogicAlarm")]
public class LogicAlarm : KMonoBehaviour, ISaveLoadable
{
	// Token: 0x06004794 RID: 18324 RVA: 0x000CE6E7 File Offset: 0x000CC8E7
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<LogicAlarm>(-905833192, LogicAlarm.OnCopySettingsDelegate);
	}

	// Token: 0x06004795 RID: 18325 RVA: 0x00253270 File Offset: 0x00251470
	private void OnCopySettings(object data)
	{
		LogicAlarm component = ((GameObject)data).GetComponent<LogicAlarm>();
		if (component != null)
		{
			this.notificationName = component.notificationName;
			this.notificationType = component.notificationType;
			this.pauseOnNotify = component.pauseOnNotify;
			this.zoomOnNotify = component.zoomOnNotify;
			this.cooldown = component.cooldown;
			this.notificationTooltip = component.notificationTooltip;
		}
	}

	// Token: 0x06004796 RID: 18326 RVA: 0x002532DC File Offset: 0x002514DC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.notifier = base.gameObject.AddComponent<Notifier>();
		base.Subscribe<LogicAlarm>(-801688580, LogicAlarm.OnLogicValueChangedDelegate);
		if (string.IsNullOrEmpty(this.notificationName))
		{
			this.notificationName = UI.UISIDESCREENS.LOGICALARMSIDESCREEN.NAME_DEFAULT;
		}
		if (string.IsNullOrEmpty(this.notificationTooltip))
		{
			this.notificationTooltip = UI.UISIDESCREENS.LOGICALARMSIDESCREEN.TOOLTIP_DEFAULT;
		}
		this.UpdateVisualState();
		this.UpdateNotification(false);
	}

	// Token: 0x06004797 RID: 18327 RVA: 0x000CE700 File Offset: 0x000CC900
	private void UpdateVisualState()
	{
		base.GetComponent<KBatchedAnimController>().Play(this.wasOn ? LogicAlarm.ON_ANIMS : LogicAlarm.OFF_ANIMS, KAnim.PlayMode.Once);
	}

	// Token: 0x06004798 RID: 18328 RVA: 0x00253358 File Offset: 0x00251558
	public void OnLogicValueChanged(object data)
	{
		LogicValueChanged logicValueChanged = (LogicValueChanged)data;
		if (logicValueChanged.portID != LogicAlarm.INPUT_PORT_ID)
		{
			return;
		}
		int newValue = logicValueChanged.newValue;
		if (LogicCircuitNetwork.IsBitActive(0, newValue))
		{
			if (!this.wasOn)
			{
				this.PushNotification();
				this.wasOn = true;
				if (this.pauseOnNotify && !SpeedControlScreen.Instance.IsPaused)
				{
					SpeedControlScreen.Instance.Pause(false, false);
				}
				if (this.zoomOnNotify)
				{
					CameraController.Instance.ActiveWorldStarWipe(base.gameObject.GetMyWorldId(), base.transform.GetPosition(), 8f, null);
				}
				this.UpdateVisualState();
				return;
			}
		}
		else if (this.wasOn)
		{
			this.wasOn = false;
			this.UpdateVisualState();
		}
	}

	// Token: 0x06004799 RID: 18329 RVA: 0x000CE722 File Offset: 0x000CC922
	private void PushNotification()
	{
		this.notification.Clear();
		this.notifier.Add(this.notification, "");
	}

	// Token: 0x0600479A RID: 18330 RVA: 0x00253410 File Offset: 0x00251610
	public void UpdateNotification(bool clear)
	{
		if (this.notification != null && clear)
		{
			this.notification.Clear();
			this.lastNotificationCreated = null;
		}
		if (this.notification != this.lastNotificationCreated || this.lastNotificationCreated == null)
		{
			this.notification = this.CreateNotification();
		}
	}

	// Token: 0x0600479B RID: 18331 RVA: 0x00253460 File Offset: 0x00251660
	public Notification CreateNotification()
	{
		base.GetComponent<KSelectable>();
		Notification result = new Notification(this.notificationName, this.notificationType, (List<Notification> n, object d) => this.notificationTooltip, null, true, 0f, null, null, null, false, false, false);
		this.lastNotificationCreated = result;
		return result;
	}

	// Token: 0x040031B8 RID: 12728
	[Serialize]
	public string notificationName;

	// Token: 0x040031B9 RID: 12729
	[Serialize]
	public string notificationTooltip;

	// Token: 0x040031BA RID: 12730
	[Serialize]
	public NotificationType notificationType;

	// Token: 0x040031BB RID: 12731
	[Serialize]
	public bool pauseOnNotify;

	// Token: 0x040031BC RID: 12732
	[Serialize]
	public bool zoomOnNotify;

	// Token: 0x040031BD RID: 12733
	[Serialize]
	public float cooldown;

	// Token: 0x040031BE RID: 12734
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x040031BF RID: 12735
	private bool wasOn;

	// Token: 0x040031C0 RID: 12736
	private Notifier notifier;

	// Token: 0x040031C1 RID: 12737
	private Notification notification;

	// Token: 0x040031C2 RID: 12738
	private Notification lastNotificationCreated;

	// Token: 0x040031C3 RID: 12739
	private static readonly EventSystem.IntraObjectHandler<LogicAlarm> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicAlarm>(delegate(LogicAlarm component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x040031C4 RID: 12740
	private static readonly EventSystem.IntraObjectHandler<LogicAlarm> OnLogicValueChangedDelegate = new EventSystem.IntraObjectHandler<LogicAlarm>(delegate(LogicAlarm component, object data)
	{
		component.OnLogicValueChanged(data);
	});

	// Token: 0x040031C5 RID: 12741
	public static readonly HashedString INPUT_PORT_ID = new HashedString("LogicAlarmInput");

	// Token: 0x040031C6 RID: 12742
	protected static readonly HashedString[] ON_ANIMS = new HashedString[]
	{
		"on_pre",
		"on_loop"
	};

	// Token: 0x040031C7 RID: 12743
	protected static readonly HashedString[] OFF_ANIMS = new HashedString[]
	{
		"on_pst",
		"off"
	};
}
