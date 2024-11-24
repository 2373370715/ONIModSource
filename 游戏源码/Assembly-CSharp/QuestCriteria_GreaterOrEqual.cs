using System;
using System.Collections.Generic;

// Token: 0x02001725 RID: 5925
public class QuestCriteria_GreaterOrEqual : QuestCriteria
{
	// Token: 0x06007A13 RID: 31251 RVA: 0x000F02F3 File Offset: 0x000EE4F3
	public QuestCriteria_GreaterOrEqual(Tag id, float[] targetValues, int requiredCount = 1, HashSet<Tag> acceptedTags = null, QuestCriteria.BehaviorFlags flags = QuestCriteria.BehaviorFlags.TrackValues) : base(id, targetValues, requiredCount, acceptedTags, flags)
	{
	}

	// Token: 0x06007A14 RID: 31252 RVA: 0x000F0322 File Offset: 0x000EE522
	protected override bool ValueSatisfies_Internal(float current, float target)
	{
		return current >= target;
	}
}
