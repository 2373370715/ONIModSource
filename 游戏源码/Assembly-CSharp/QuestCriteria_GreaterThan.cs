using System;
using System.Collections.Generic;

// Token: 0x02001723 RID: 5923
public class QuestCriteria_GreaterThan : QuestCriteria
{
	// Token: 0x06007A0F RID: 31247 RVA: 0x000F02F3 File Offset: 0x000EE4F3
	public QuestCriteria_GreaterThan(Tag id, float[] targetValues, int requiredCount = 1, HashSet<Tag> acceptedTags = null, QuestCriteria.BehaviorFlags flags = QuestCriteria.BehaviorFlags.TrackValues) : base(id, targetValues, requiredCount, acceptedTags, flags)
	{
	}

	// Token: 0x06007A10 RID: 31248 RVA: 0x000F0316 File Offset: 0x000EE516
	protected override bool ValueSatisfies_Internal(float current, float target)
	{
		return current > target;
	}
}
