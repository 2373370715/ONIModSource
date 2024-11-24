using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

// Token: 0x0200078F RID: 1935
[DebuggerDisplay("{base.Id}")]
public abstract class GameplayEvent : Resource, IComparable<GameplayEvent>
{
	// Token: 0x170000F5 RID: 245
	// (get) Token: 0x060022C8 RID: 8904 RVA: 0x000B6A16 File Offset: 0x000B4C16
	// (set) Token: 0x060022C9 RID: 8905 RVA: 0x000B6A1E File Offset: 0x000B4C1E
	public int importance { get; private set; }

	// Token: 0x060022CA RID: 8906 RVA: 0x001C4260 File Offset: 0x001C2460
	public virtual bool IsAllowed()
	{
		if (this.WillNeverRunAgain())
		{
			return false;
		}
		if (!this.allowMultipleEventInstances && GameplayEventManager.Instance.IsGameplayEventActive(this))
		{
			return false;
		}
		foreach (GameplayEventPrecondition gameplayEventPrecondition in this.preconditions)
		{
			if (gameplayEventPrecondition.required && !gameplayEventPrecondition.condition())
			{
				return false;
			}
		}
		float sleepTimer = GameplayEventManager.Instance.GetSleepTimer(this);
		return GameUtil.GetCurrentTimeInCycles() >= sleepTimer;
	}

	// Token: 0x060022CB RID: 8907 RVA: 0x000B6A27 File Offset: 0x000B4C27
	public void SetSleepTimer(float timeToSleepUntil)
	{
		GameplayEventManager.Instance.SetSleepTimerForEvent(this, timeToSleepUntil);
	}

	// Token: 0x060022CC RID: 8908 RVA: 0x000B6A35 File Offset: 0x000B4C35
	public virtual bool WillNeverRunAgain()
	{
		return this.numTimesAllowed != -1 && GameplayEventManager.Instance.NumberOfPastEvents(this.Id) >= this.numTimesAllowed;
	}

	// Token: 0x060022CD RID: 8909 RVA: 0x000B6A62 File Offset: 0x000B4C62
	public int GetCashedPriority()
	{
		return this.calculatedPriority;
	}

	// Token: 0x060022CE RID: 8910 RVA: 0x000B6A6A File Offset: 0x000B4C6A
	public virtual int CalculatePriority()
	{
		this.calculatedPriority = this.basePriority + this.CalculateBoost();
		return this.calculatedPriority;
	}

	// Token: 0x060022CF RID: 8911 RVA: 0x001C4300 File Offset: 0x001C2500
	public int CalculateBoost()
	{
		int num = 0;
		foreach (GameplayEventPrecondition gameplayEventPrecondition in this.preconditions)
		{
			if (!gameplayEventPrecondition.required && gameplayEventPrecondition.condition())
			{
				num += gameplayEventPrecondition.priorityModifier;
			}
		}
		return num;
	}

	// Token: 0x060022D0 RID: 8912 RVA: 0x000B6A85 File Offset: 0x000B4C85
	public GameplayEvent AddPrecondition(GameplayEventPrecondition precondition)
	{
		precondition.required = true;
		this.preconditions.Add(precondition);
		return this;
	}

	// Token: 0x060022D1 RID: 8913 RVA: 0x000B6A9B File Offset: 0x000B4C9B
	public GameplayEvent AddPriorityBoost(GameplayEventPrecondition precondition, int priorityBoost)
	{
		precondition.required = false;
		precondition.priorityModifier = priorityBoost;
		this.preconditions.Add(precondition);
		return this;
	}

	// Token: 0x060022D2 RID: 8914 RVA: 0x000B6AB8 File Offset: 0x000B4CB8
	public GameplayEvent AddMinionFilter(GameplayEventMinionFilter filter)
	{
		this.minionFilters.Add(filter);
		return this;
	}

	// Token: 0x060022D3 RID: 8915 RVA: 0x000B6AC7 File Offset: 0x000B4CC7
	public GameplayEvent TrySpawnEventOnSuccess(HashedString evt)
	{
		this.successEvents.Add(evt);
		return this;
	}

	// Token: 0x060022D4 RID: 8916 RVA: 0x000B6AD6 File Offset: 0x000B4CD6
	public GameplayEvent TrySpawnEventOnFailure(HashedString evt)
	{
		this.failureEvents.Add(evt);
		return this;
	}

	// Token: 0x060022D5 RID: 8917 RVA: 0x000B6AE5 File Offset: 0x000B4CE5
	public GameplayEvent SetVisuals(HashedString animFileName)
	{
		this.animFileName = animFileName;
		return this;
	}

	// Token: 0x060022D6 RID: 8918 RVA: 0x000AD332 File Offset: 0x000AB532
	public virtual Sprite GetDisplaySprite()
	{
		return null;
	}

	// Token: 0x060022D7 RID: 8919 RVA: 0x000AD332 File Offset: 0x000AB532
	public virtual string GetDisplayString()
	{
		return null;
	}

	// Token: 0x060022D8 RID: 8920 RVA: 0x001C4370 File Offset: 0x001C2570
	public MinionIdentity GetRandomFilteredMinion()
	{
		List<MinionIdentity> list = new List<MinionIdentity>(Components.LiveMinionIdentities.Items);
		using (List<GameplayEventMinionFilter>.Enumerator enumerator = this.minionFilters.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				GameplayEventMinionFilter filter = enumerator.Current;
				list.RemoveAll((MinionIdentity x) => !filter.filter(x));
			}
		}
		if (list.Count != 0)
		{
			return list[UnityEngine.Random.Range(0, list.Count)];
		}
		return null;
	}

	// Token: 0x060022D9 RID: 8921 RVA: 0x001C4408 File Offset: 0x001C2608
	public MinionIdentity GetRandomMinionPrioritizeFiltered()
	{
		MinionIdentity randomFilteredMinion = this.GetRandomFilteredMinion();
		if (!(randomFilteredMinion == null))
		{
			return randomFilteredMinion;
		}
		return Components.LiveMinionIdentities.Items[UnityEngine.Random.Range(0, Components.LiveMinionIdentities.Items.Count)];
	}

	// Token: 0x060022DA RID: 8922 RVA: 0x001C444C File Offset: 0x001C264C
	public int CompareTo(GameplayEvent other)
	{
		return -this.GetCashedPriority().CompareTo(other.GetCashedPriority());
	}

	// Token: 0x060022DB RID: 8923 RVA: 0x001C4470 File Offset: 0x001C2670
	public GameplayEvent(string id, int priority, int importance) : base(id, null, null)
	{
		this.tags = new List<Tag>();
		this.basePriority = priority;
		this.preconditions = new List<GameplayEventPrecondition>();
		this.minionFilters = new List<GameplayEventMinionFilter>();
		this.successEvents = new List<HashedString>();
		this.failureEvents = new List<HashedString>();
		this.importance = importance;
		this.animFileName = id;
	}

	// Token: 0x060022DC RID: 8924
	public abstract StateMachine.Instance GetSMI(GameplayEventManager manager, GameplayEventInstance eventInstance);

	// Token: 0x060022DD RID: 8925 RVA: 0x001C44E0 File Offset: 0x001C26E0
	public GameplayEventInstance CreateInstance(int worldId)
	{
		GameplayEventInstance gameplayEventInstance = new GameplayEventInstance(this, worldId);
		if (this.tags != null)
		{
			gameplayEventInstance.tags.AddRange(this.tags);
		}
		return gameplayEventInstance;
	}

	// Token: 0x040016FE RID: 5886
	public const int INFINITE = -1;

	// Token: 0x040016FF RID: 5887
	public int numTimesAllowed = -1;

	// Token: 0x04001700 RID: 5888
	public bool allowMultipleEventInstances;

	// Token: 0x04001701 RID: 5889
	protected int basePriority;

	// Token: 0x04001702 RID: 5890
	protected int calculatedPriority;

	// Token: 0x04001704 RID: 5892
	public List<GameplayEventPrecondition> preconditions;

	// Token: 0x04001705 RID: 5893
	public List<GameplayEventMinionFilter> minionFilters;

	// Token: 0x04001706 RID: 5894
	public List<HashedString> successEvents;

	// Token: 0x04001707 RID: 5895
	public List<HashedString> failureEvents;

	// Token: 0x04001708 RID: 5896
	public string title;

	// Token: 0x04001709 RID: 5897
	public string description;

	// Token: 0x0400170A RID: 5898
	public HashedString animFileName;

	// Token: 0x0400170B RID: 5899
	public List<Tag> tags;
}
