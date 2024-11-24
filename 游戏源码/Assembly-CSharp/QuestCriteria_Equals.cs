using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001722 RID: 5922
public class QuestCriteria_Equals : QuestCriteria
{
	// Token: 0x06007A0D RID: 31245 RVA: 0x000F02F3 File Offset: 0x000EE4F3
	public QuestCriteria_Equals(Tag id, float[] targetValues, int requiredCount = 1, HashSet<Tag> acceptedTags = null, QuestCriteria.BehaviorFlags flags = QuestCriteria.BehaviorFlags.TrackValues) : base(id, targetValues, requiredCount, acceptedTags, flags)
	{
	}

	// Token: 0x06007A0E RID: 31246 RVA: 0x000F0302 File Offset: 0x000EE502
	protected override bool ValueSatisfies_Internal(float current, float target)
	{
		return Mathf.Abs(target - current) <= Mathf.Epsilon;
	}
}
