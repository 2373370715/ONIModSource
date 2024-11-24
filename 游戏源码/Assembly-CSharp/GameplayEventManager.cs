using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;

public class GameplayEventManager : KMonoBehaviour
{
	public static GameplayEventManager Instance;

	public Notifier notifier;

	[Serialize]
	private List<GameplayEventInstance> activeEvents = new List<GameplayEventInstance>();

	[Serialize]
	private Dictionary<HashedString, int> pastEvents = new Dictionary<HashedString, int>();

	[Serialize]
	private Dictionary<HashedString, float> sleepTimers = new Dictionary<HashedString, float>();

	public static void DestroyInstance()
	{
		Instance = null;
	}

	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Instance = this;
		notifier = GetComponent<Notifier>();
	}

	protected override void OnSpawn()
	{
		base.OnSpawn();
		RestoreEvents();
	}

	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Instance = null;
	}

	private void RestoreEvents()
	{
		activeEvents.RemoveAll((GameplayEventInstance x) => Db.Get().GameplayEvents.TryGet(x.eventID) == null);
		for (int num = activeEvents.Count - 1; num >= 0; num--)
		{
			GameplayEventInstance gameplayEventInstance = activeEvents[num];
			if (gameplayEventInstance.smi == null)
			{
				StartEventInstance(gameplayEventInstance);
			}
		}
	}

	public void SetSleepTimerForEvent(GameplayEvent eventType, float time)
	{
		sleepTimers[eventType.IdHash] = time;
	}

	public float GetSleepTimer(GameplayEvent eventType)
	{
		float value = 0f;
		sleepTimers.TryGetValue(eventType.IdHash, out value);
		sleepTimers[eventType.IdHash] = value;
		return value;
	}

	public bool IsGameplayEventActive(GameplayEvent eventType)
	{
		return activeEvents.Find((GameplayEventInstance e) => e.eventID == eventType.IdHash) != null;
	}

	public bool IsGameplayEventRunningWithTag(Tag tag)
	{
		foreach (GameplayEventInstance activeEvent in activeEvents)
		{
			if (activeEvent.tags.Contains(tag))
			{
				return true;
			}
		}
		return false;
	}

	public void GetActiveEventsOfType<T>(int worldID, ref List<GameplayEventInstance> results) where T : GameplayEvent
	{
		foreach (GameplayEventInstance activeEvent in activeEvents)
		{
			if (activeEvent.worldId == worldID && activeEvent.gameplayEvent as T != null)
			{
				results.Add(activeEvent);
			}
		}
	}

	public void GetActiveEventsOfType<T>(ref List<GameplayEventInstance> results) where T : GameplayEvent
	{
		foreach (GameplayEventInstance activeEvent in activeEvents)
		{
			if (activeEvent.gameplayEvent as T != null)
			{
				results.Add(activeEvent);
			}
		}
	}

	private GameplayEventInstance CreateGameplayEvent(GameplayEvent gameplayEvent, int worldId)
	{
		return gameplayEvent.CreateInstance(worldId);
	}

	public GameplayEventInstance GetGameplayEventInstance(HashedString eventID, int worldId = -1)
	{
		return activeEvents.Find((GameplayEventInstance e) => e.eventID == eventID && (worldId == -1 || e.worldId == worldId));
	}

	public GameplayEventInstance CreateOrGetEventInstance(GameplayEvent eventType, int worldId = -1)
	{
		GameplayEventInstance gameplayEventInstance = GetGameplayEventInstance(eventType.Id, worldId);
		if (gameplayEventInstance == null)
		{
			gameplayEventInstance = StartNewEvent(eventType, worldId);
		}
		return gameplayEventInstance;
	}

	public void RemoveActiveEvent(GameplayEventInstance eventInstance, string reason = "RemoveActiveEvent() called")
	{
		GameplayEventInstance gameplayEventInstance = activeEvents.Find((GameplayEventInstance x) => x == eventInstance);
		if (gameplayEventInstance != null)
		{
			if (gameplayEventInstance.smi != null)
			{
				gameplayEventInstance.smi.StopSM(reason);
			}
			else
			{
				activeEvents.Remove(gameplayEventInstance);
			}
		}
	}

	public GameplayEventInstance StartNewEvent(GameplayEvent eventType, int worldId = -1, Action<StateMachine.Instance> setupActionsBeforeStart = null)
	{
		GameplayEventInstance gameplayEventInstance = CreateGameplayEvent(eventType, worldId);
		StartEventInstance(gameplayEventInstance, setupActionsBeforeStart);
		activeEvents.Add(gameplayEventInstance);
		pastEvents.TryGetValue(gameplayEventInstance.eventID, out var value);
		pastEvents[gameplayEventInstance.eventID] = value + 1;
		return gameplayEventInstance;
	}

	private void StartEventInstance(GameplayEventInstance gameplayEventInstance, Action<StateMachine.Instance> setupActionsBeforeStart = null)
	{
		StateMachine.Instance instance = gameplayEventInstance.PrepareEvent(this);
		instance.OnStop = (Action<string, StateMachine.Status>)Delegate.Combine(instance.OnStop, (Action<string, StateMachine.Status>)delegate
		{
			activeEvents.Remove(gameplayEventInstance);
		});
		setupActionsBeforeStart?.Invoke(instance);
		gameplayEventInstance.StartEvent();
	}

	public int NumberOfPastEvents(HashedString eventID)
	{
		pastEvents.TryGetValue(eventID, out var value);
		return value;
	}

	public static Notification CreateStandardCancelledNotification(EventInfoData eventInfoData)
	{
		if (eventInfoData == null)
		{
			DebugUtil.LogWarningArgs("eventPopup is null in CreateStandardCancelledNotification");
			return null;
		}
		eventInfoData.FinalizeText();
		return new Notification(string.Format(GAMEPLAY_EVENTS.CANCELED, eventInfoData.title), NotificationType.Event, (List<Notification> list, object data) => string.Format(GAMEPLAY_EVENTS.CANCELED_TOOLTIP, eventInfoData.title));
	}
}
