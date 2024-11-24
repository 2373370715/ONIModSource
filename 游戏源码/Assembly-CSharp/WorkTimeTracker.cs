using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B3F RID: 2879
public class WorkTimeTracker : WorldTracker
{
	// Token: 0x060036B2 RID: 14002 RVA: 0x000C395C File Offset: 0x000C1B5C
	public WorkTimeTracker(int worldID, ChoreGroup group) : base(worldID)
	{
		this.choreGroup = group;
	}

	// Token: 0x060036B3 RID: 14003 RVA: 0x002146D8 File Offset: 0x002128D8
	public override void UpdateData()
	{
		float num = 0f;
		List<MinionIdentity> worldItems = Components.LiveMinionIdentities.GetWorldItems(base.WorldID, false);
		Chore chore;
		Predicate<ChoreType> <>9__0;
		foreach (MinionIdentity minionIdentity in worldItems)
		{
			chore = minionIdentity.GetComponent<ChoreConsumer>().choreDriver.GetCurrentChore();
			if (chore != null)
			{
				List<ChoreType> choreTypes = this.choreGroup.choreTypes;
				Predicate<ChoreType> match2;
				if ((match2 = <>9__0) == null)
				{
					match2 = (<>9__0 = ((ChoreType match) => match == chore.choreType));
				}
				if (choreTypes.Find(match2) != null)
				{
					num += 1f;
				}
			}
		}
		base.AddPoint(num / (float)worldItems.Count * 100f);
	}

	// Token: 0x060036B4 RID: 14004 RVA: 0x000C396C File Offset: 0x000C1B6C
	public override string FormatValueString(float value)
	{
		return GameUtil.GetFormattedPercent(Mathf.Round(value), GameUtil.TimeSlice.None).ToString();
	}

	// Token: 0x0400252D RID: 9517
	public ChoreGroup choreGroup;
}
