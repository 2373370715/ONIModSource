using System;
using System.Collections.Generic;

// Token: 0x02001726 RID: 5926
public class QuestCriteria_LessOrEqual : QuestCriteria
{
	// Token: 0x06007A15 RID: 31253 RVA: 0x000F02F3 File Offset: 0x000EE4F3
	public QuestCriteria_LessOrEqual(Tag id, float[] targetValues, int requiredCount = 1, HashSet<Tag> acceptedTags = null, QuestCriteria.BehaviorFlags flags = QuestCriteria.BehaviorFlags.TrackValues) : base(id, targetValues, requiredCount, acceptedTags, flags)
	{
	}

	// Token: 0x06007A16 RID: 31254 RVA: 0x000F032B File Offset: 0x000EE52B
	protected override bool ValueSatisfies_Internal(float current, float target)
	{
		return current <= target;
	}
}
