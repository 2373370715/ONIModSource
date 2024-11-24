using System.Collections.Generic;
using UnityEngine;

public class QuestCriteria
{
	public enum BehaviorFlags
	{
		None = 0,
		TrackArea = 1,
		AllowsRegression = 2,
		TrackValues = 4,
		TrackItems = 8,
		UniqueItems = 24
	}

	public const int MAX_VALUES = 32;

	public const int INVALID_VALUE = -1;

	public readonly Tag CriteriaId;

	public readonly BehaviorFlags EvaluationBehaviors;

	public readonly float[] TargetValues;

	public readonly int RequiredCount = 1;

	public readonly HashSet<Tag> AcceptedTags;

	public string Text { get; private set; }

	public string Tooltip { get; private set; }

	public QuestCriteria(Tag id, float[] targetValues = null, int requiredCount = 1, HashSet<Tag> acceptedTags = null, BehaviorFlags flags = BehaviorFlags.None)
	{
		Debug.Assert(targetValues == null || (targetValues.Length != 0 && targetValues.Length <= 32));
		CriteriaId = id;
		EvaluationBehaviors = flags;
		TargetValues = targetValues;
		AcceptedTags = acceptedTags;
		RequiredCount = requiredCount;
	}

	public bool ValueSatisfies(float value, int valueHandle)
	{
		if (float.IsNaN(value))
		{
			return false;
		}
		float target = ((TargetValues == null) ? 0f : TargetValues[valueHandle]);
		return ValueSatisfies_Internal(value, target);
	}

	protected virtual bool ValueSatisfies_Internal(float current, float target)
	{
		return true;
	}

	public bool IsSatisfied(uint satisfactionState, uint satisfactionMask)
	{
		return (satisfactionState & satisfactionMask) == satisfactionMask;
	}

	public void PopulateStrings(string prefix)
	{
		string text = CriteriaId.Name.ToUpperInvariant();
		if (Strings.TryGet(prefix + "CRITERIA." + text + ".NAME", out var result))
		{
			Text = result.String;
		}
		if (Strings.TryGet(prefix + "CRITERIA." + text + ".TOOLTIP", out result))
		{
			Tooltip = result.String;
		}
	}

	public uint GetSatisfactionMask()
	{
		if (TargetValues == null)
		{
			return 1u;
		}
		return (uint)Mathf.Pow(2f, TargetValues.Length - 1);
	}

	public uint GetValueMask(int valueHandle)
	{
		if (TargetValues == null)
		{
			return 1u;
		}
		if (!HasBehavior(EvaluationBehaviors, BehaviorFlags.TrackArea))
		{
			valueHandle %= TargetValues.Length;
		}
		return (uint)(1 << valueHandle);
	}

	public static bool HasBehavior(BehaviorFlags flags, BehaviorFlags behavior)
	{
		return (flags & behavior) == behavior;
	}
}
