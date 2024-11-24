using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class QuestInstance : ISaveLoadable
{
	private struct CriteriaState
	{
		public int Handle;

		public int CurrentCount;

		public uint SatisfactionState;

		public Tag[] SatisfyingItems;

		public float[] CurrentValues;

		public static bool ItemAlreadySatisfying(CriteriaState state, Tag item)
		{
			bool result = false;
			int num = 0;
			while (state.SatisfyingItems != null && num < state.SatisfyingItems.Length)
			{
				if (state.SatisfyingItems[num] == item)
				{
					result = true;
					break;
				}
				num++;
			}
			return result;
		}
	}

	public Action<QuestInstance, Quest.State, float> QuestProgressChanged;

	private Quest quest;

	[Serialize]
	private Dictionary<int, CriteriaState> criteriaStates;

	[Serialize]
	private Quest.State currentState;

	public HashedString Id => quest.IdHash;

	public int CriteriaCount => quest.Criteria.Length;

	public string Name => quest.Name;

	public string CompletionText => quest.CompletionText;

	public bool IsStarted => currentState != Quest.State.NotStarted;

	public bool IsComplete => currentState == Quest.State.Completed;

	public float CurrentProgress { get; private set; }

	public Quest.State CurrentState => currentState;

	public QuestInstance(Quest quest)
	{
		this.quest = quest;
		criteriaStates = new Dictionary<int, CriteriaState>(quest.Criteria.Length);
		for (int i = 0; i < quest.Criteria.Length; i++)
		{
			QuestCriteria questCriteria = quest.Criteria[i];
			CriteriaState value = new CriteriaState
			{
				Handle = i
			};
			if (questCriteria.TargetValues != null)
			{
				if ((questCriteria.EvaluationBehaviors & QuestCriteria.BehaviorFlags.TrackItems) == QuestCriteria.BehaviorFlags.TrackItems)
				{
					value.SatisfyingItems = new Tag[questCriteria.TargetValues.Length * questCriteria.RequiredCount];
				}
				if ((questCriteria.EvaluationBehaviors & QuestCriteria.BehaviorFlags.TrackValues) == QuestCriteria.BehaviorFlags.TrackValues)
				{
					value.CurrentValues = new float[questCriteria.TargetValues.Length * questCriteria.RequiredCount];
				}
			}
			criteriaStates[questCriteria.CriteriaId.GetHash()] = value;
		}
	}

	public void Initialize(Quest quest)
	{
		this.quest = quest;
		ValidateCriteriasOnLoad();
		UpdateQuestProgress();
	}

	public bool HasCriteria(HashedString criteriaId)
	{
		return criteriaStates.ContainsKey(criteriaId.HashValue);
	}

	public bool HasBehavior(QuestCriteria.BehaviorFlags behavior)
	{
		bool flag = false;
		int num = 0;
		while (!flag && num < quest.Criteria.Length)
		{
			flag = (quest.Criteria[num].EvaluationBehaviors & behavior) != 0;
			num++;
		}
		return flag;
	}

	public int GetTargetCount(HashedString criteriaId)
	{
		if (!criteriaStates.TryGetValue(criteriaId.HashValue, out var value))
		{
			return 0;
		}
		return quest.Criteria[value.Handle].RequiredCount;
	}

	public int GetCurrentCount(HashedString criteriaId)
	{
		if (!criteriaStates.TryGetValue(criteriaId.HashValue, out var value))
		{
			return 0;
		}
		return value.CurrentCount;
	}

	public float GetCurrentValue(HashedString criteriaId, int valueHandle = 0)
	{
		if (!criteriaStates.TryGetValue(criteriaId.HashValue, out var value) || value.CurrentValues == null)
		{
			return float.NaN;
		}
		return value.CurrentValues[valueHandle];
	}

	public float GetTargetValue(HashedString criteriaId, int valueHandle = 0)
	{
		if (!criteriaStates.TryGetValue(criteriaId.HashValue, out var value))
		{
			return float.NaN;
		}
		if (quest.Criteria[value.Handle].TargetValues == null)
		{
			return float.NaN;
		}
		return quest.Criteria[value.Handle].TargetValues[valueHandle];
	}

	public Tag GetSatisfyingItem(HashedString criteriaId, int valueHandle = 0)
	{
		if (!criteriaStates.TryGetValue(criteriaId.HashValue, out var value) || value.SatisfyingItems == null)
		{
			return default(Tag);
		}
		return value.SatisfyingItems[valueHandle];
	}

	public float GetAreaAverage(HashedString criteriaId)
	{
		if (!criteriaStates.TryGetValue(criteriaId.HashValue, out var value))
		{
			return float.NaN;
		}
		if (!QuestCriteria.HasBehavior(quest.Criteria[value.Handle].EvaluationBehaviors, (QuestCriteria.BehaviorFlags)5))
		{
			return float.NaN;
		}
		float num = 0f;
		for (int i = 0; i < value.CurrentValues.Length; i++)
		{
			num += value.CurrentValues[i];
		}
		return num / (float)value.CurrentValues.Length;
	}

	public bool IsItemRedundant(HashedString criteriaId, Tag item)
	{
		if (!criteriaStates.TryGetValue(criteriaId.HashValue, out var value) || value.SatisfyingItems == null)
		{
			return false;
		}
		bool flag = false;
		int num = 0;
		while (!flag && num < value.SatisfyingItems.Length)
		{
			flag = value.SatisfyingItems[num] == item;
			num++;
		}
		return flag;
	}

	public bool IsCriteriaSatisfied(HashedString id)
	{
		if (criteriaStates.TryGetValue(id.HashValue, out var value))
		{
			return quest.Criteria[value.Handle].IsSatisfied(value.SatisfactionState, GetSatisfactionMask(value));
		}
		return false;
	}

	public bool IsCriteriaSatisfied(Tag id)
	{
		if (criteriaStates.TryGetValue(id.GetHash(), out var value))
		{
			return quest.Criteria[value.Handle].IsSatisfied(value.SatisfactionState, GetSatisfactionMask(value));
		}
		return false;
	}

	public void TrackAreaForCriteria(HashedString criteriaId, Extents area)
	{
		if (criteriaStates.TryGetValue(criteriaId.HashValue, out var value))
		{
			int num = area.width * area.height;
			QuestCriteria obj = quest.Criteria[value.Handle];
			Debug.Assert(num <= 32);
			if (QuestCriteria.HasBehavior(obj.EvaluationBehaviors, QuestCriteria.BehaviorFlags.TrackValues))
			{
				value.CurrentValues = new float[num];
			}
			if (QuestCriteria.HasBehavior(obj.EvaluationBehaviors, QuestCriteria.BehaviorFlags.TrackItems))
			{
				value.SatisfyingItems = new Tag[num];
			}
			criteriaStates[criteriaId.HashValue] = value;
		}
	}

	private uint GetSatisfactionMask(CriteriaState state)
	{
		QuestCriteria questCriteria = quest.Criteria[state.Handle];
		if (QuestCriteria.HasBehavior(questCriteria.EvaluationBehaviors, QuestCriteria.BehaviorFlags.TrackArea))
		{
			int num = 0;
			if (state.SatisfyingItems != null)
			{
				num = state.SatisfyingItems.Length;
			}
			else if (state.CurrentValues != null)
			{
				num = state.CurrentValues.Length;
			}
			return (uint)(Mathf.Pow(2f, num) - 1f);
		}
		return questCriteria.GetSatisfactionMask();
	}

	public int TrackProgress(Quest.ItemData data, out bool dataSatisfies, out bool itemIsRedundant)
	{
		dataSatisfies = false;
		itemIsRedundant = false;
		if (!criteriaStates.TryGetValue(data.CriteriaId.HashValue, out var value))
		{
			return -1;
		}
		int valueHandle = data.ValueHandle;
		QuestCriteria questCriteria = quest.Criteria[value.Handle];
		dataSatisfies = DataSatisfiesCriteria(data, ref valueHandle);
		if (valueHandle == -1)
		{
			return valueHandle;
		}
		bool flag = QuestCriteria.HasBehavior(questCriteria.EvaluationBehaviors, QuestCriteria.BehaviorFlags.AllowsRegression);
		bool flag2 = QuestCriteria.HasBehavior(questCriteria.EvaluationBehaviors, QuestCriteria.BehaviorFlags.TrackItems);
		Tag tag = (flag2 ? value.SatisfyingItems[valueHandle] : default(Tag));
		if (dataSatisfies)
		{
			itemIsRedundant = QuestCriteria.HasBehavior(questCriteria.EvaluationBehaviors, QuestCriteria.BehaviorFlags.UniqueItems) && IsItemRedundant(data.CriteriaId, data.SatisfyingItem);
			if (itemIsRedundant)
			{
				return valueHandle;
			}
			tag = data.SatisfyingItem;
			value.SatisfactionState |= questCriteria.GetValueMask(valueHandle);
		}
		else if (flag)
		{
			value.SatisfactionState &= ~questCriteria.GetValueMask(valueHandle);
		}
		if (QuestCriteria.HasBehavior(questCriteria.EvaluationBehaviors, QuestCriteria.BehaviorFlags.TrackValues))
		{
			value.CurrentValues[valueHandle] = data.CurrentValue;
		}
		if (flag2)
		{
			value.SatisfyingItems[valueHandle] = tag;
		}
		bool flag3 = IsCriteriaSatisfied(data.CriteriaId);
		bool flag4 = questCriteria.IsSatisfied(value.SatisfactionState, GetSatisfactionMask(value));
		if (flag3 != flag4)
		{
			value.CurrentCount += ((!flag3) ? 1 : (-1));
			if (flag4 && value.CurrentCount < questCriteria.RequiredCount)
			{
				value.SatisfactionState = 0u;
			}
		}
		criteriaStates[data.CriteriaId.HashValue] = value;
		UpdateQuestProgress(startQuest: true);
		return valueHandle;
	}

	public bool DataSatisfiesCriteria(Quest.ItemData data, ref int valueHandle)
	{
		if (!criteriaStates.TryGetValue(data.CriteriaId.HashValue, out var value))
		{
			return false;
		}
		QuestCriteria questCriteria = quest.Criteria[value.Handle];
		bool flag = questCriteria.AcceptedTags == null || (data.QualifyingTag.IsValid && questCriteria.AcceptedTags.Contains(data.QualifyingTag));
		if (flag && questCriteria.TargetValues == null)
		{
			valueHandle = 0;
		}
		if (!flag || valueHandle != -1)
		{
			if (flag)
			{
				return questCriteria.ValueSatisfies(data.CurrentValue, valueHandle);
			}
			return false;
		}
		if (QuestCriteria.HasBehavior(questCriteria.EvaluationBehaviors, QuestCriteria.BehaviorFlags.TrackArea))
		{
			valueHandle = data.LocalCellId;
		}
		int num = -1;
		bool flag2 = QuestCriteria.HasBehavior(questCriteria.EvaluationBehaviors, QuestCriteria.BehaviorFlags.TrackValues);
		bool flag3 = false;
		int num2 = 0;
		while (!flag3 && num2 < questCriteria.TargetValues.Length)
		{
			if (questCriteria.ValueSatisfies(data.CurrentValue, num2))
			{
				flag3 = true;
				num = num2;
				break;
			}
			if (flag2 && (num == -1 || value.CurrentValues[num] > value.CurrentValues[num2]))
			{
				num = num2;
			}
			num2++;
		}
		if (valueHandle == -1 && num != -1)
		{
			valueHandle = questCriteria.RequiredCount * num + Mathf.Min(value.CurrentCount, questCriteria.RequiredCount - 1);
		}
		return flag3;
	}

	private void UpdateQuestProgress(bool startQuest = false)
	{
		if (!IsStarted && !startQuest)
		{
			return;
		}
		float currentProgress = CurrentProgress;
		Quest.State state = currentState;
		currentState = Quest.State.InProgress;
		CurrentProgress = 0f;
		float num = 0f;
		for (int i = 0; i < quest.Criteria.Length; i++)
		{
			QuestCriteria questCriteria = quest.Criteria[i];
			CriteriaState criteriaState = criteriaStates[questCriteria.CriteriaId.GetHash()];
			float num2 = ((questCriteria.TargetValues == null) ? 1 : questCriteria.TargetValues.Length);
			num += (float)questCriteria.RequiredCount;
			CurrentProgress += criteriaState.CurrentCount;
			if (IsCriteriaSatisfied(questCriteria.CriteriaId))
			{
				continue;
			}
			float num3 = 0f;
			int num4 = 0;
			while (questCriteria.TargetValues != null && (float)num4 < num2)
			{
				if ((criteriaState.SatisfactionState & questCriteria.GetValueMask(num4)) == 0)
				{
					if (QuestCriteria.HasBehavior(questCriteria.EvaluationBehaviors, QuestCriteria.BehaviorFlags.TrackValues))
					{
						int num5 = questCriteria.RequiredCount * num4 + Mathf.Min(criteriaState.CurrentCount, questCriteria.RequiredCount - 1);
						num3 += Mathf.Max(0f, criteriaState.CurrentValues[num5] / questCriteria.TargetValues[num4]);
					}
				}
				else
				{
					num3 += 1f;
				}
				num4++;
			}
			CurrentProgress += num3 / num2;
		}
		CurrentProgress = Mathf.Clamp01(CurrentProgress / num);
		if (CurrentProgress == 1f)
		{
			currentState = Quest.State.Completed;
		}
		float num6 = CurrentProgress - currentProgress;
		if (state != currentState || Mathf.Abs(num6) > Mathf.Epsilon)
		{
			QuestProgressChanged?.Invoke(this, state, num6);
		}
	}

	public ICheckboxListGroupControl.CheckboxItem[] GetCheckBoxData(Func<int, string, QuestInstance, string> resolveToolTip = null)
	{
		ICheckboxListGroupControl.CheckboxItem[] array = new ICheckboxListGroupControl.CheckboxItem[quest.Criteria.Length];
		for (int i = 0; i < quest.Criteria.Length; i++)
		{
			QuestCriteria c = quest.Criteria[i];
			array[i] = new ICheckboxListGroupControl.CheckboxItem
			{
				text = c.Text,
				isOn = IsCriteriaSatisfied(c.CriteriaId),
				tooltip = c.Tooltip
			};
			if (resolveToolTip != null)
			{
				array[i].resolveTooltipCallback = (string tooltip, object owner) => resolveToolTip(c.CriteriaId.GetHash(), c.Tooltip, this);
			}
		}
		return array;
	}

	public void ValidateCriteriasOnLoad()
	{
		if (criteriaStates.Count == quest.Criteria.Length)
		{
			return;
		}
		Dictionary<int, CriteriaState> dictionary = new Dictionary<int, CriteriaState>(quest.Criteria.Length);
		for (int i = 0; i < quest.Criteria.Length; i++)
		{
			QuestCriteria questCriteria = quest.Criteria[i];
			int hash = questCriteria.CriteriaId.GetHash();
			if (criteriaStates.ContainsKey(hash))
			{
				dictionary[hash] = criteriaStates[hash];
				continue;
			}
			CriteriaState criteriaState = default(CriteriaState);
			criteriaState.Handle = i;
			CriteriaState value = criteriaState;
			if (questCriteria.TargetValues != null)
			{
				if ((questCriteria.EvaluationBehaviors & QuestCriteria.BehaviorFlags.TrackItems) == QuestCriteria.BehaviorFlags.TrackItems)
				{
					value.SatisfyingItems = new Tag[questCriteria.TargetValues.Length * questCriteria.RequiredCount];
				}
				if ((questCriteria.EvaluationBehaviors & QuestCriteria.BehaviorFlags.TrackValues) == QuestCriteria.BehaviorFlags.TrackValues)
				{
					value.CurrentValues = new float[questCriteria.TargetValues.Length * questCriteria.RequiredCount];
				}
			}
			dictionary[hash] = value;
		}
		criteriaStates = dictionary;
	}
}
