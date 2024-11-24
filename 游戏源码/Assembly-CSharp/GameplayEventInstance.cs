using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class GameplayEventInstance : ISaveLoadable
{
	public delegate EventInfoData GameplayEventPopupDataCallback();

	[Serialize]
	public readonly HashedString eventID;

	[Serialize]
	public List<Tag> tags;

	[Serialize]
	public float eventStartTime;

	[Serialize]
	public readonly int worldId;

	[Serialize]
	private bool _seenNotification;

	public List<GameObject> monitorCallbackObjects;

	public GameplayEventPopupDataCallback GetEventPopupData;

	private GameplayEvent _gameplayEvent;

	public StateMachine.Instance smi { get; private set; }

	public bool seenNotification
	{
		get
		{
			return _seenNotification;
		}
		set
		{
			_seenNotification = value;
			monitorCallbackObjects.ForEach(delegate(GameObject x)
			{
				x.Trigger(-1122598290, this);
			});
		}
	}

	public GameplayEvent gameplayEvent
	{
		get
		{
			if (_gameplayEvent == null)
			{
				_gameplayEvent = Db.Get().GameplayEvents.TryGet(eventID);
			}
			return _gameplayEvent;
		}
	}

	public GameplayEventInstance(GameplayEvent gameplayEvent, int worldId)
	{
		eventID = gameplayEvent.Id;
		tags = new List<Tag>();
		eventStartTime = GameUtil.GetCurrentTimeInCycles();
		this.worldId = worldId;
	}

	public StateMachine.Instance PrepareEvent(GameplayEventManager manager)
	{
		smi = gameplayEvent.GetSMI(manager, this);
		return smi;
	}

	public void StartEvent()
	{
		GameplayEventManager.Instance.Trigger(1491341646, this);
		StateMachine.Instance instance = smi;
		instance.OnStop = (Action<string, StateMachine.Status>)Delegate.Combine(instance.OnStop, new Action<string, StateMachine.Status>(OnStop));
		smi.StartSM();
	}

	public void RegisterMonitorCallback(GameObject go)
	{
		if (monitorCallbackObjects == null)
		{
			monitorCallbackObjects = new List<GameObject>();
		}
		if (!monitorCallbackObjects.Contains(go))
		{
			monitorCallbackObjects.Add(go);
		}
	}

	public void UnregisterMonitorCallback(GameObject go)
	{
		if (monitorCallbackObjects == null)
		{
			monitorCallbackObjects = new List<GameObject>();
		}
		monitorCallbackObjects.Remove(go);
	}

	public void OnStop(string reason, StateMachine.Status status)
	{
		GameplayEventManager.Instance.Trigger(1287635015, this);
		if (monitorCallbackObjects != null)
		{
			monitorCallbackObjects.ForEach(delegate(GameObject x)
			{
				x.Trigger(1287635015, this);
			});
		}
		switch (status)
		{
		case StateMachine.Status.Success:
		{
			foreach (HashedString successEvent in this.gameplayEvent.successEvents)
			{
				GameplayEvent gameplayEvent2 = Db.Get().GameplayEvents.TryGet(successEvent);
				DebugUtil.DevAssert(gameplayEvent2 != null, $"GameplayEvent {successEvent} is null");
				if (gameplayEvent2 != null && gameplayEvent2.IsAllowed())
				{
					GameplayEventManager.Instance.StartNewEvent(gameplayEvent2);
				}
			}
			break;
		}
		case StateMachine.Status.Failed:
		{
			foreach (HashedString failureEvent in this.gameplayEvent.failureEvents)
			{
				GameplayEvent gameplayEvent = Db.Get().GameplayEvents.TryGet(failureEvent);
				DebugUtil.DevAssert(gameplayEvent != null, $"GameplayEvent {failureEvent} is null");
				if (gameplayEvent != null && gameplayEvent.IsAllowed())
				{
					GameplayEventManager.Instance.StartNewEvent(gameplayEvent);
				}
			}
			break;
		}
		}
	}

	public float AgeInCycles()
	{
		return GameUtil.GetCurrentTimeInCycles() - eventStartTime;
	}
}
