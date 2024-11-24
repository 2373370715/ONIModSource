using System;
using System.Collections.Generic;
using Database;
using KSerialization;

[SerializationConfig(MemberSerialization.OptIn)]
public class StoryInstance : ISaveLoadable
{
	public enum State
	{
		RETROFITTED = -1,
		NOT_STARTED,
		DISCOVERED,
		IN_PROGRESS,
		COMPLETE
	}

	public Action<State> StoryStateChanged;

	[Serialize]
	public readonly string storyId;

	[Serialize]
	public int worldId;

	[Serialize]
	private State state;

	[Serialize]
	private StoryManager.StoryTelemetry telemetry;

	[Serialize]
	private HashSet<EventInfoDataHelper.PopupType> popupDisplayedStates = new HashSet<EventInfoDataHelper.PopupType>();

	private Story _story;

	public State CurrentState
	{
		get
		{
			return state;
		}
		set
		{
			if (state != value)
			{
				state = value;
				Telemetry.LogStateChange(state, GameClock.Instance.GetTimeInCycles());
				StoryStateChanged?.Invoke(state);
			}
		}
	}

	public StoryManager.StoryTelemetry Telemetry
	{
		get
		{
			if (telemetry == null)
			{
				telemetry = new StoryManager.StoryTelemetry();
			}
			return telemetry;
		}
	}

	public EventInfoData EventInfo { get; private set; }

	public Notification Notification { get; private set; }

	public EventInfoDataHelper.PopupType PendingType { get; private set; } = EventInfoDataHelper.PopupType.NONE;


	public Story GetStory()
	{
		if (_story == null)
		{
			_story = Db.Get().Stories.Get(storyId);
		}
		return _story;
	}

	public StoryInstance()
	{
	}

	public StoryInstance(Story story, int worldId)
	{
		_story = story;
		storyId = story.Id;
		this.worldId = worldId;
	}

	public bool HasDisplayedPopup(EventInfoDataHelper.PopupType type)
	{
		if (popupDisplayedStates == null)
		{
			return false;
		}
		return popupDisplayedStates.Contains(type);
	}

	public void SetPopupData(StoryManager.PopupInfo info, EventInfoData eventInfo, Notification notification = null)
	{
		EventInfo = eventInfo;
		Notification = notification;
		PendingType = info.PopupType;
		eventInfo.showCallback = (System.Action)Delegate.Combine(eventInfo.showCallback, new System.Action(OnPopupDisplayed));
		if (info.DisplayImmediate)
		{
			EventInfoScreen.ShowPopup(eventInfo);
		}
	}

	private void OnPopupDisplayed()
	{
		if (popupDisplayedStates == null)
		{
			popupDisplayedStates = new HashSet<EventInfoDataHelper.PopupType>();
		}
		popupDisplayedStates.Add(PendingType);
		EventInfo = null;
		Notification = null;
		PendingType = EventInfoDataHelper.PopupType.NONE;
	}
}
