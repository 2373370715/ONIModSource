using System;
using System.Collections.Generic;

// Token: 0x02001724 RID: 5924
public class QuestCriteria_LessThan : QuestCriteria
{
	// Token: 0x06007A11 RID: 31249 RVA: 0x000F02F3 File Offset: 0x000EE4F3
	public QuestCriteria_LessThan(Tag id, float[] targetValues, int requiredCount = 1, HashSet<Tag> acceptedTags = null, QuestCriteria.BehaviorFlags flags = QuestCriteria.BehaviorFlags.TrackValues) : base(id, targetValues, requiredCount, acceptedTags, flags)
	{
	}

	// Token: 0x06007A12 RID: 31250 RVA: 0x000F031C File Offset: 0x000EE51C
	protected override bool ValueSatisfies_Internal(float current, float target)
	{
		return current < target;
	}
}
