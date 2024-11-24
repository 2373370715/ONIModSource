using System;
using System.Collections.Generic;
using System.Linq;
using Database;
using Klei.AI;

// Token: 0x020007A0 RID: 1952
public class GameplayEventPreconditions
{
	// Token: 0x170000FA RID: 250
	// (get) Token: 0x06002315 RID: 8981 RVA: 0x000B6D27 File Offset: 0x000B4F27
	public static GameplayEventPreconditions Instance
	{
		get
		{
			if (GameplayEventPreconditions._instance == null)
			{
				GameplayEventPreconditions._instance = new GameplayEventPreconditions();
			}
			return GameplayEventPreconditions._instance;
		}
	}

	// Token: 0x06002316 RID: 8982 RVA: 0x001C48C8 File Offset: 0x001C2AC8
	public GameplayEventPrecondition LiveMinions(int count = 1)
	{
		return new GameplayEventPrecondition
		{
			condition = (() => Components.LiveMinionIdentities.Count >= count),
			description = string.Format("At least {0} dupes alive", count)
		};
	}

	// Token: 0x06002317 RID: 8983 RVA: 0x001C4914 File Offset: 0x001C2B14
	public GameplayEventPrecondition BuildingExists(string buildingId, int count = 1)
	{
		return new GameplayEventPrecondition
		{
			condition = (() => BuildingInventory.Instance.BuildingCount(new Tag(buildingId)) >= count),
			description = string.Format("{0} {1} has been built", count, buildingId)
		};
	}

	// Token: 0x06002318 RID: 8984 RVA: 0x001C4970 File Offset: 0x001C2B70
	public GameplayEventPrecondition ResearchCompleted(string techName)
	{
		return new GameplayEventPrecondition
		{
			condition = (() => Research.Instance.Get(Db.Get().Techs.Get(techName)).IsComplete()),
			description = "Has researched " + techName + "."
		};
	}

	// Token: 0x06002319 RID: 8985 RVA: 0x001C49BC File Offset: 0x001C2BBC
	public GameplayEventPrecondition AchievementUnlocked(ColonyAchievement achievement)
	{
		return new GameplayEventPrecondition
		{
			condition = (() => SaveGame.Instance.ColonyAchievementTracker.IsAchievementUnlocked(achievement)),
			description = "Unlocked the " + achievement.Id + " achievement"
		};
	}

	// Token: 0x0600231A RID: 8986 RVA: 0x001C4A10 File Offset: 0x001C2C10
	public GameplayEventPrecondition RoomBuilt(RoomType roomType)
	{
		Predicate<Room> <>9__1;
		return new GameplayEventPrecondition
		{
			condition = delegate()
			{
				List<Room> rooms = Game.Instance.roomProber.rooms;
				Predicate<Room> match2;
				if ((match2 = <>9__1) == null)
				{
					match2 = (<>9__1 = ((Room match) => match.roomType == roomType));
				}
				return rooms.Exists(match2);
			},
			description = "Built a " + roomType.Id + " room"
		};
	}

	// Token: 0x0600231B RID: 8987 RVA: 0x001C4A64 File Offset: 0x001C2C64
	public GameplayEventPrecondition CycleRestriction(float min = 0f, float max = float.PositiveInfinity)
	{
		return new GameplayEventPrecondition
		{
			condition = (() => GameUtil.GetCurrentTimeInCycles() >= min && GameUtil.GetCurrentTimeInCycles() <= max),
			description = string.Format("After cycle {0} and before cycle {1}", min, max)
		};
	}

	// Token: 0x0600231C RID: 8988 RVA: 0x001C4AC4 File Offset: 0x001C2CC4
	public GameplayEventPrecondition MinionsWithEffect(string effectId, int count = 1)
	{
		Func<MinionIdentity, bool> <>9__1;
		return new GameplayEventPrecondition
		{
			condition = delegate()
			{
				IEnumerable<MinionIdentity> items = Components.LiveMinionIdentities.Items;
				Func<MinionIdentity, bool> predicate;
				if ((predicate = <>9__1) == null)
				{
					predicate = (<>9__1 = ((MinionIdentity minion) => minion.GetComponent<Effects>().Get(effectId) != null));
				}
				return items.Count(predicate) >= count;
			},
			description = string.Format("At least {0} dupes have the {1} effect applied", count, effectId)
		};
	}

	// Token: 0x0600231D RID: 8989 RVA: 0x001C4B20 File Offset: 0x001C2D20
	public GameplayEventPrecondition MinionsWithStatusItem(StatusItem statusItem, int count = 1)
	{
		Func<MinionIdentity, bool> <>9__1;
		return new GameplayEventPrecondition
		{
			condition = delegate()
			{
				IEnumerable<MinionIdentity> items = Components.LiveMinionIdentities.Items;
				Func<MinionIdentity, bool> predicate;
				if ((predicate = <>9__1) == null)
				{
					predicate = (<>9__1 = ((MinionIdentity minion) => minion.GetComponent<KSelectable>().HasStatusItem(statusItem)));
				}
				return items.Count(predicate) >= count;
			},
			description = string.Format("At least {0} dupes have the {1} status item", count, statusItem)
		};
	}

	// Token: 0x0600231E RID: 8990 RVA: 0x001C4B7C File Offset: 0x001C2D7C
	public GameplayEventPrecondition MinionsWithChoreGroupPriorityOrGreater(ChoreGroup choreGroup, int count, int priority)
	{
		Func<MinionIdentity, bool> <>9__1;
		return new GameplayEventPrecondition
		{
			condition = delegate()
			{
				IEnumerable<MinionIdentity> items = Components.LiveMinionIdentities.Items;
				Func<MinionIdentity, bool> predicate;
				if ((predicate = <>9__1) == null)
				{
					predicate = (<>9__1 = delegate(MinionIdentity minion)
					{
						ChoreConsumer component = minion.GetComponent<ChoreConsumer>();
						return !component.IsChoreGroupDisabled(choreGroup) && component.GetPersonalPriority(choreGroup) >= priority;
					});
				}
				return items.Count(predicate) >= count;
			},
			description = string.Format("At least {0} dupes have their {1} set to {2} or higher.", count, choreGroup.Name, priority)
		};
	}

	// Token: 0x0600231F RID: 8991 RVA: 0x001C4BEC File Offset: 0x001C2DEC
	public GameplayEventPrecondition PastEventCount(string evtId, int count = 1)
	{
		return new GameplayEventPrecondition
		{
			condition = (() => GameplayEventManager.Instance.NumberOfPastEvents(evtId) >= count),
			description = string.Format("The {0} event has triggered {1} times.", evtId, count)
		};
	}

	// Token: 0x06002320 RID: 8992 RVA: 0x001C4C48 File Offset: 0x001C2E48
	public GameplayEventPrecondition PastEventCountAndNotActive(GameplayEvent evt, int count = 1)
	{
		return new GameplayEventPrecondition
		{
			condition = (() => GameplayEventManager.Instance.NumberOfPastEvents(evt.IdHash) >= count && !GameplayEventManager.Instance.IsGameplayEventActive(evt)),
			description = string.Format("The {0} event has triggered {1} times and is not active.", evt.Id, count)
		};
	}

	// Token: 0x06002321 RID: 8993 RVA: 0x001C4CA8 File Offset: 0x001C2EA8
	public GameplayEventPrecondition Not(GameplayEventPrecondition precondition)
	{
		return new GameplayEventPrecondition
		{
			condition = (() => !precondition.condition()),
			description = "Not[" + precondition.description + "]"
		};
	}

	// Token: 0x06002322 RID: 8994 RVA: 0x001C4CFC File Offset: 0x001C2EFC
	public GameplayEventPrecondition Or(GameplayEventPrecondition precondition1, GameplayEventPrecondition precondition2)
	{
		return new GameplayEventPrecondition
		{
			condition = (() => precondition1.condition() || precondition2.condition()),
			description = string.Concat(new string[]
			{
				"[",
				precondition1.description,
				"]-OR-[",
				precondition2.description,
				"]"
			})
		};
	}

	// Token: 0x04001728 RID: 5928
	private static GameplayEventPreconditions _instance;
}
