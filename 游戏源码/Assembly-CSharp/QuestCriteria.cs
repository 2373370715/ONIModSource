using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001720 RID: 5920
public class QuestCriteria
{
	// Token: 0x170007A9 RID: 1961
	// (get) Token: 0x06007A01 RID: 31233 RVA: 0x000F0273 File Offset: 0x000EE473
	// (set) Token: 0x06007A02 RID: 31234 RVA: 0x000F027B File Offset: 0x000EE47B
	public string Text { get; private set; }

	// Token: 0x170007AA RID: 1962
	// (get) Token: 0x06007A03 RID: 31235 RVA: 0x000F0284 File Offset: 0x000EE484
	// (set) Token: 0x06007A04 RID: 31236 RVA: 0x000F028C File Offset: 0x000EE48C
	public string Tooltip { get; private set; }

	// Token: 0x06007A05 RID: 31237 RVA: 0x003178D4 File Offset: 0x00315AD4
	public QuestCriteria(Tag id, float[] targetValues = null, int requiredCount = 1, HashSet<Tag> acceptedTags = null, QuestCriteria.BehaviorFlags flags = QuestCriteria.BehaviorFlags.None)
	{
		global::Debug.Assert(targetValues == null || (targetValues.Length != 0 && targetValues.Length <= 32));
		this.CriteriaId = id;
		this.EvaluationBehaviors = flags;
		this.TargetValues = targetValues;
		this.AcceptedTags = acceptedTags;
		this.RequiredCount = requiredCount;
	}

	// Token: 0x06007A06 RID: 31238 RVA: 0x00317930 File Offset: 0x00315B30
	public bool ValueSatisfies(float value, int valueHandle)
	{
		if (float.IsNaN(value))
		{
			return false;
		}
		float target = (this.TargetValues == null) ? 0f : this.TargetValues[valueHandle];
		return this.ValueSatisfies_Internal(value, target);
	}

	// Token: 0x06007A07 RID: 31239 RVA: 0x000A65EC File Offset: 0x000A47EC
	protected virtual bool ValueSatisfies_Internal(float current, float target)
	{
		return true;
	}

	// Token: 0x06007A08 RID: 31240 RVA: 0x000F0295 File Offset: 0x000EE495
	public bool IsSatisfied(uint satisfactionState, uint satisfactionMask)
	{
		return (satisfactionState & satisfactionMask) == satisfactionMask;
	}

	// Token: 0x06007A09 RID: 31241 RVA: 0x00317968 File Offset: 0x00315B68
	public void PopulateStrings(string prefix)
	{
		string str = this.CriteriaId.Name.ToUpperInvariant();
		StringEntry stringEntry;
		if (Strings.TryGet(prefix + "CRITERIA." + str + ".NAME", out stringEntry))
		{
			this.Text = stringEntry.String;
		}
		if (Strings.TryGet(prefix + "CRITERIA." + str + ".TOOLTIP", out stringEntry))
		{
			this.Tooltip = stringEntry.String;
		}
	}

	// Token: 0x06007A0A RID: 31242 RVA: 0x000F029D File Offset: 0x000EE49D
	public uint GetSatisfactionMask()
	{
		if (this.TargetValues == null)
		{
			return 1U;
		}
		return (uint)Mathf.Pow(2f, (float)(this.TargetValues.Length - 1));
	}

	// Token: 0x06007A0B RID: 31243 RVA: 0x000F02BF File Offset: 0x000EE4BF
	public uint GetValueMask(int valueHandle)
	{
		if (this.TargetValues == null)
		{
			return 1U;
		}
		if (!QuestCriteria.HasBehavior(this.EvaluationBehaviors, QuestCriteria.BehaviorFlags.TrackArea))
		{
			valueHandle %= this.TargetValues.Length;
		}
		return 1U << valueHandle;
	}

	// Token: 0x06007A0C RID: 31244 RVA: 0x000F02EB File Offset: 0x000EE4EB
	public static bool HasBehavior(QuestCriteria.BehaviorFlags flags, QuestCriteria.BehaviorFlags behavior)
	{
		return (flags & behavior) == behavior;
	}

	// Token: 0x04005B8D RID: 23437
	public const int MAX_VALUES = 32;

	// Token: 0x04005B8E RID: 23438
	public const int INVALID_VALUE = -1;

	// Token: 0x04005B8F RID: 23439
	public readonly Tag CriteriaId;

	// Token: 0x04005B90 RID: 23440
	public readonly QuestCriteria.BehaviorFlags EvaluationBehaviors;

	// Token: 0x04005B91 RID: 23441
	public readonly float[] TargetValues;

	// Token: 0x04005B92 RID: 23442
	public readonly int RequiredCount = 1;

	// Token: 0x04005B93 RID: 23443
	public readonly HashSet<Tag> AcceptedTags;

	// Token: 0x02001721 RID: 5921
	public enum BehaviorFlags
	{
		// Token: 0x04005B97 RID: 23447
		None,
		// Token: 0x04005B98 RID: 23448
		TrackArea,
		// Token: 0x04005B99 RID: 23449
		AllowsRegression,
		// Token: 0x04005B9A RID: 23450
		TrackValues = 4,
		// Token: 0x04005B9B RID: 23451
		TrackItems = 8,
		// Token: 0x04005B9C RID: 23452
		UniqueItems = 24
	}
}
