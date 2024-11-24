using System;
using System.Collections.Generic;
using Database;
using KSerialization;

// Token: 0x020019B1 RID: 6577
[SerializationConfig(MemberSerialization.OptIn)]
public class StoryInstance : ISaveLoadable
{
	// Token: 0x170008FC RID: 2300
	// (get) Token: 0x060088EE RID: 35054 RVA: 0x000F99A0 File Offset: 0x000F7BA0
	// (set) Token: 0x060088EF RID: 35055 RVA: 0x00355A30 File Offset: 0x00353C30
	public StoryInstance.State CurrentState
	{
		get
		{
			return this.state;
		}
		set
		{
			if (this.state == value)
			{
				return;
			}
			this.state = value;
			this.Telemetry.LogStateChange(this.state, GameClock.Instance.GetTimeInCycles());
			Action<StoryInstance.State> storyStateChanged = this.StoryStateChanged;
			if (storyStateChanged == null)
			{
				return;
			}
			storyStateChanged(this.state);
		}
	}

	// Token: 0x170008FD RID: 2301
	// (get) Token: 0x060088F0 RID: 35056 RVA: 0x000F99A8 File Offset: 0x000F7BA8
	public StoryManager.StoryTelemetry Telemetry
	{
		get
		{
			if (this.telemetry == null)
			{
				this.telemetry = new StoryManager.StoryTelemetry();
			}
			return this.telemetry;
		}
	}

	// Token: 0x170008FE RID: 2302
	// (get) Token: 0x060088F1 RID: 35057 RVA: 0x000F99C3 File Offset: 0x000F7BC3
	// (set) Token: 0x060088F2 RID: 35058 RVA: 0x000F99CB File Offset: 0x000F7BCB
	public EventInfoData EventInfo { get; private set; }

	// Token: 0x170008FF RID: 2303
	// (get) Token: 0x060088F3 RID: 35059 RVA: 0x000F99D4 File Offset: 0x000F7BD4
	// (set) Token: 0x060088F4 RID: 35060 RVA: 0x000F99DC File Offset: 0x000F7BDC
	public Notification Notification { get; private set; }

	// Token: 0x17000900 RID: 2304
	// (get) Token: 0x060088F5 RID: 35061 RVA: 0x000F99E5 File Offset: 0x000F7BE5
	// (set) Token: 0x060088F6 RID: 35062 RVA: 0x000F99ED File Offset: 0x000F7BED
	public EventInfoDataHelper.PopupType PendingType { get; private set; } = EventInfoDataHelper.PopupType.NONE;

	// Token: 0x060088F7 RID: 35063 RVA: 0x000F99F6 File Offset: 0x000F7BF6
	public Story GetStory()
	{
		if (this._story == null)
		{
			this._story = Db.Get().Stories.Get(this.storyId);
		}
		return this._story;
	}

	// Token: 0x060088F8 RID: 35064 RVA: 0x000F9A21 File Offset: 0x000F7C21
	public StoryInstance()
	{
	}

	// Token: 0x060088F9 RID: 35065 RVA: 0x000F9A3B File Offset: 0x000F7C3B
	public StoryInstance(Story story, int worldId)
	{
		this._story = story;
		this.storyId = story.Id;
		this.worldId = worldId;
	}

	// Token: 0x060088FA RID: 35066 RVA: 0x000F9A6F File Offset: 0x000F7C6F
	public bool HasDisplayedPopup(EventInfoDataHelper.PopupType type)
	{
		return this.popupDisplayedStates != null && this.popupDisplayedStates.Contains(type);
	}

	// Token: 0x060088FB RID: 35067 RVA: 0x00355A80 File Offset: 0x00353C80
	public void SetPopupData(StoryManager.PopupInfo info, EventInfoData eventInfo, Notification notification = null)
	{
		this.EventInfo = eventInfo;
		this.Notification = notification;
		this.PendingType = info.PopupType;
		eventInfo.showCallback = (System.Action)Delegate.Combine(eventInfo.showCallback, new System.Action(this.OnPopupDisplayed));
		if (info.DisplayImmediate)
		{
			EventInfoScreen.ShowPopup(eventInfo);
		}
	}

	// Token: 0x060088FC RID: 35068 RVA: 0x000F9A87 File Offset: 0x000F7C87
	private void OnPopupDisplayed()
	{
		if (this.popupDisplayedStates == null)
		{
			this.popupDisplayedStates = new HashSet<EventInfoDataHelper.PopupType>();
		}
		this.popupDisplayedStates.Add(this.PendingType);
		this.EventInfo = null;
		this.Notification = null;
		this.PendingType = EventInfoDataHelper.PopupType.NONE;
	}

	// Token: 0x04006711 RID: 26385
	public Action<StoryInstance.State> StoryStateChanged;

	// Token: 0x04006712 RID: 26386
	[Serialize]
	public readonly string storyId;

	// Token: 0x04006713 RID: 26387
	[Serialize]
	public int worldId;

	// Token: 0x04006714 RID: 26388
	[Serialize]
	private StoryInstance.State state;

	// Token: 0x04006715 RID: 26389
	[Serialize]
	private StoryManager.StoryTelemetry telemetry;

	// Token: 0x04006716 RID: 26390
	[Serialize]
	private HashSet<EventInfoDataHelper.PopupType> popupDisplayedStates = new HashSet<EventInfoDataHelper.PopupType>();

	// Token: 0x0400671A RID: 26394
	private Story _story;

	// Token: 0x020019B2 RID: 6578
	public enum State
	{
		// Token: 0x0400671C RID: 26396
		RETROFITTED = -1,
		// Token: 0x0400671D RID: 26397
		NOT_STARTED,
		// Token: 0x0400671E RID: 26398
		DISCOVERED,
		// Token: 0x0400671F RID: 26399
		IN_PROGRESS,
		// Token: 0x04006720 RID: 26400
		COMPLETE
	}
}
