using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02000792 RID: 1938
[SerializationConfig(MemberSerialization.OptIn)]
public class GameplayEventInstance : ISaveLoadable
{
	// Token: 0x170000F6 RID: 246
	// (get) Token: 0x060022E1 RID: 8929 RVA: 0x000B6B10 File Offset: 0x000B4D10
	// (set) Token: 0x060022E2 RID: 8930 RVA: 0x000B6B18 File Offset: 0x000B4D18
	public StateMachine.Instance smi { get; private set; }

	// Token: 0x170000F7 RID: 247
	// (get) Token: 0x060022E3 RID: 8931 RVA: 0x000B6B21 File Offset: 0x000B4D21
	// (set) Token: 0x060022E4 RID: 8932 RVA: 0x000B6B29 File Offset: 0x000B4D29
	public bool seenNotification
	{
		get
		{
			return this._seenNotification;
		}
		set
		{
			this._seenNotification = value;
			this.monitorCallbackObjects.ForEach(delegate(GameObject x)
			{
				x.Trigger(-1122598290, this);
			});
		}
	}

	// Token: 0x170000F8 RID: 248
	// (get) Token: 0x060022E5 RID: 8933 RVA: 0x000B6B49 File Offset: 0x000B4D49
	public GameplayEvent gameplayEvent
	{
		get
		{
			if (this._gameplayEvent == null)
			{
				this._gameplayEvent = Db.Get().GameplayEvents.TryGet(this.eventID);
			}
			return this._gameplayEvent;
		}
	}

	// Token: 0x060022E6 RID: 8934 RVA: 0x000B6B74 File Offset: 0x000B4D74
	public GameplayEventInstance(GameplayEvent gameplayEvent, int worldId)
	{
		this.eventID = gameplayEvent.Id;
		this.tags = new List<Tag>();
		this.eventStartTime = GameUtil.GetCurrentTimeInCycles();
		this.worldId = worldId;
	}

	// Token: 0x060022E7 RID: 8935 RVA: 0x000B6BAA File Offset: 0x000B4DAA
	public StateMachine.Instance PrepareEvent(GameplayEventManager manager)
	{
		this.smi = this.gameplayEvent.GetSMI(manager, this);
		return this.smi;
	}

	// Token: 0x060022E8 RID: 8936 RVA: 0x001C4510 File Offset: 0x001C2710
	public void StartEvent()
	{
		GameplayEventManager.Instance.Trigger(1491341646, this);
		StateMachine.Instance smi = this.smi;
		smi.OnStop = (Action<string, StateMachine.Status>)Delegate.Combine(smi.OnStop, new Action<string, StateMachine.Status>(this.OnStop));
		this.smi.StartSM();
	}

	// Token: 0x060022E9 RID: 8937 RVA: 0x000B6BC5 File Offset: 0x000B4DC5
	public void RegisterMonitorCallback(GameObject go)
	{
		if (this.monitorCallbackObjects == null)
		{
			this.monitorCallbackObjects = new List<GameObject>();
		}
		if (!this.monitorCallbackObjects.Contains(go))
		{
			this.monitorCallbackObjects.Add(go);
		}
	}

	// Token: 0x060022EA RID: 8938 RVA: 0x000B6BF4 File Offset: 0x000B4DF4
	public void UnregisterMonitorCallback(GameObject go)
	{
		if (this.monitorCallbackObjects == null)
		{
			this.monitorCallbackObjects = new List<GameObject>();
		}
		this.monitorCallbackObjects.Remove(go);
	}

	// Token: 0x060022EB RID: 8939 RVA: 0x001C4560 File Offset: 0x001C2760
	public void OnStop(string reason, StateMachine.Status status)
	{
		GameplayEventManager.Instance.Trigger(1287635015, this);
		if (this.monitorCallbackObjects != null)
		{
			this.monitorCallbackObjects.ForEach(delegate(GameObject x)
			{
				x.Trigger(1287635015, this);
			});
		}
		if (status == StateMachine.Status.Success)
		{
			using (List<HashedString>.Enumerator enumerator = this.gameplayEvent.successEvents.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					HashedString hashedString = enumerator.Current;
					GameplayEvent gameplayEvent = Db.Get().GameplayEvents.TryGet(hashedString);
					DebugUtil.DevAssert(gameplayEvent != null, string.Format("GameplayEvent {0} is null", hashedString), null);
					if (gameplayEvent != null && gameplayEvent.IsAllowed())
					{
						GameplayEventManager.Instance.StartNewEvent(gameplayEvent, -1, null);
					}
				}
				return;
			}
		}
		if (status == StateMachine.Status.Failed)
		{
			foreach (HashedString hashedString2 in this.gameplayEvent.failureEvents)
			{
				GameplayEvent gameplayEvent2 = Db.Get().GameplayEvents.TryGet(hashedString2);
				DebugUtil.DevAssert(gameplayEvent2 != null, string.Format("GameplayEvent {0} is null", hashedString2), null);
				if (gameplayEvent2 != null && gameplayEvent2.IsAllowed())
				{
					GameplayEventManager.Instance.StartNewEvent(gameplayEvent2, -1, null);
				}
			}
		}
	}

	// Token: 0x060022EC RID: 8940 RVA: 0x000B6C16 File Offset: 0x000B4E16
	public float AgeInCycles()
	{
		return GameUtil.GetCurrentTimeInCycles() - this.eventStartTime;
	}

	// Token: 0x0400170E RID: 5902
	[Serialize]
	public readonly HashedString eventID;

	// Token: 0x0400170F RID: 5903
	[Serialize]
	public List<Tag> tags;

	// Token: 0x04001710 RID: 5904
	[Serialize]
	public float eventStartTime;

	// Token: 0x04001711 RID: 5905
	[Serialize]
	public readonly int worldId;

	// Token: 0x04001712 RID: 5906
	[Serialize]
	private bool _seenNotification;

	// Token: 0x04001713 RID: 5907
	public List<GameObject> monitorCallbackObjects;

	// Token: 0x04001714 RID: 5908
	public GameplayEventInstance.GameplayEventPopupDataCallback GetEventPopupData;

	// Token: 0x04001715 RID: 5909
	private GameplayEvent _gameplayEvent;

	// Token: 0x02000793 RID: 1939
	// (Invoke) Token: 0x060022F0 RID: 8944
	public delegate EventInfoData GameplayEventPopupDataCallback();
}
