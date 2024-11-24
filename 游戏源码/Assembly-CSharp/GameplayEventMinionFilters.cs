using System;
using Database;

// Token: 0x02000796 RID: 1942
public class GameplayEventMinionFilters
{
	// Token: 0x170000F9 RID: 249
	// (get) Token: 0x060022F8 RID: 8952 RVA: 0x000B6C40 File Offset: 0x000B4E40
	public static GameplayEventMinionFilters Instance
	{
		get
		{
			if (GameplayEventMinionFilters._instance == null)
			{
				GameplayEventMinionFilters._instance = new GameplayEventMinionFilters();
			}
			return GameplayEventMinionFilters._instance;
		}
	}

	// Token: 0x060022F9 RID: 8953 RVA: 0x001C46B8 File Offset: 0x001C28B8
	public GameplayEventMinionFilter HasMasteredSkill(Skill skill)
	{
		return new GameplayEventMinionFilter
		{
			filter = ((MinionIdentity minion) => minion.GetComponent<MinionResume>().HasMasteredSkill(skill.Id)),
			id = "HasMasteredSkill"
		};
	}

	// Token: 0x060022FA RID: 8954 RVA: 0x001C46F4 File Offset: 0x001C28F4
	public GameplayEventMinionFilter HasSkillAptitude(Skill skill)
	{
		return new GameplayEventMinionFilter
		{
			filter = ((MinionIdentity minion) => minion.GetComponent<MinionResume>().HasSkillAptitude(skill)),
			id = "HasSkillAptitude"
		};
	}

	// Token: 0x060022FB RID: 8955 RVA: 0x001C4730 File Offset: 0x001C2930
	public GameplayEventMinionFilter HasChoreGroupPriorityOrHigher(ChoreGroup choreGroup, int priority)
	{
		return new GameplayEventMinionFilter
		{
			filter = delegate(MinionIdentity minion)
			{
				ChoreConsumer component = minion.GetComponent<ChoreConsumer>();
				return !component.IsChoreGroupDisabled(choreGroup) && component.GetPersonalPriority(choreGroup) >= priority;
			},
			id = "HasChoreGroupPriorityOrHigher"
		};
	}

	// Token: 0x060022FC RID: 8956 RVA: 0x001C4774 File Offset: 0x001C2974
	public GameplayEventMinionFilter AgeRange(float min = 0f, float max = float.PositiveInfinity)
	{
		return new GameplayEventMinionFilter
		{
			filter = ((MinionIdentity minion) => minion.arrivalTime >= min && minion.arrivalTime <= max),
			id = "AgeRange"
		};
	}

	// Token: 0x060022FD RID: 8957 RVA: 0x000B6C58 File Offset: 0x000B4E58
	public GameplayEventMinionFilter PriorityIn()
	{
		GameplayEventMinionFilter gameplayEventMinionFilter = new GameplayEventMinionFilter();
		gameplayEventMinionFilter.filter = ((MinionIdentity minion) => true);
		gameplayEventMinionFilter.id = "PriorityIn";
		return gameplayEventMinionFilter;
	}

	// Token: 0x060022FE RID: 8958 RVA: 0x001C47B8 File Offset: 0x001C29B8
	public GameplayEventMinionFilter Not(GameplayEventMinionFilter filter)
	{
		return new GameplayEventMinionFilter
		{
			filter = ((MinionIdentity minion) => !filter.filter(minion)),
			id = "Not[" + filter.id + "]"
		};
	}

	// Token: 0x060022FF RID: 8959 RVA: 0x001C480C File Offset: 0x001C2A0C
	public GameplayEventMinionFilter Or(GameplayEventMinionFilter precondition1, GameplayEventMinionFilter precondition2)
	{
		return new GameplayEventMinionFilter
		{
			filter = ((MinionIdentity minion) => precondition1.filter(minion) || precondition2.filter(minion)),
			id = string.Concat(new string[]
			{
				"[",
				precondition1.id,
				"]-OR-[",
				precondition2.id,
				"]"
			})
		};
	}

	// Token: 0x04001718 RID: 5912
	private static GameplayEventMinionFilters _instance;
}
