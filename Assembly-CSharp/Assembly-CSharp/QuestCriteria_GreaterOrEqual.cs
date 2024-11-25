﻿using System;
using System.Collections.Generic;

public class QuestCriteria_GreaterOrEqual : QuestCriteria
{
		public QuestCriteria_GreaterOrEqual(Tag id, float[] targetValues, int requiredCount = 1, HashSet<Tag> acceptedTags = null, QuestCriteria.BehaviorFlags flags = QuestCriteria.BehaviorFlags.TrackValues) : base(id, targetValues, requiredCount, acceptedTags, flags)
	{
	}

		protected override bool ValueSatisfies_Internal(float current, float target)
	{
		return current >= target;
	}
}
