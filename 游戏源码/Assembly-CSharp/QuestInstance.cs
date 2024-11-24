using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x0200171C RID: 5916
[SerializationConfig(MemberSerialization.OptIn)]
public class QuestInstance : ISaveLoadable
{
	// Token: 0x170007A1 RID: 1953
	// (get) Token: 0x060079E0 RID: 31200 RVA: 0x000F01E4 File Offset: 0x000EE3E4
	public HashedString Id
	{
		get
		{
			return this.quest.IdHash;
		}
	}

	// Token: 0x170007A2 RID: 1954
	// (get) Token: 0x060079E1 RID: 31201 RVA: 0x000F01F1 File Offset: 0x000EE3F1
	public int CriteriaCount
	{
		get
		{
			return this.quest.Criteria.Length;
		}
	}

	// Token: 0x170007A3 RID: 1955
	// (get) Token: 0x060079E2 RID: 31202 RVA: 0x000F0200 File Offset: 0x000EE400
	public string Name
	{
		get
		{
			return this.quest.Name;
		}
	}

	// Token: 0x170007A4 RID: 1956
	// (get) Token: 0x060079E3 RID: 31203 RVA: 0x000F020D File Offset: 0x000EE40D
	public string CompletionText
	{
		get
		{
			return this.quest.CompletionText;
		}
	}

	// Token: 0x170007A5 RID: 1957
	// (get) Token: 0x060079E4 RID: 31204 RVA: 0x000F021A File Offset: 0x000EE41A
	public bool IsStarted
	{
		get
		{
			return this.currentState > Quest.State.NotStarted;
		}
	}

	// Token: 0x170007A6 RID: 1958
	// (get) Token: 0x060079E5 RID: 31205 RVA: 0x000F0225 File Offset: 0x000EE425
	public bool IsComplete
	{
		get
		{
			return this.currentState == Quest.State.Completed;
		}
	}

	// Token: 0x170007A7 RID: 1959
	// (get) Token: 0x060079E6 RID: 31206 RVA: 0x000F0230 File Offset: 0x000EE430
	// (set) Token: 0x060079E7 RID: 31207 RVA: 0x000F0238 File Offset: 0x000EE438
	public float CurrentProgress { get; private set; }

	// Token: 0x170007A8 RID: 1960
	// (get) Token: 0x060079E8 RID: 31208 RVA: 0x000F0241 File Offset: 0x000EE441
	public Quest.State CurrentState
	{
		get
		{
			return this.currentState;
		}
	}

	// Token: 0x060079E9 RID: 31209 RVA: 0x00316CBC File Offset: 0x00314EBC
	public QuestInstance(Quest quest)
	{
		this.quest = quest;
		this.criteriaStates = new Dictionary<int, QuestInstance.CriteriaState>(quest.Criteria.Length);
		for (int i = 0; i < quest.Criteria.Length; i++)
		{
			QuestCriteria questCriteria = quest.Criteria[i];
			QuestInstance.CriteriaState value = new QuestInstance.CriteriaState
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
			this.criteriaStates[questCriteria.CriteriaId.GetHash()] = value;
		}
	}

	// Token: 0x060079EA RID: 31210 RVA: 0x000F0249 File Offset: 0x000EE449
	public void Initialize(Quest quest)
	{
		this.quest = quest;
		this.ValidateCriteriasOnLoad();
		this.UpdateQuestProgress(false);
	}

	// Token: 0x060079EB RID: 31211 RVA: 0x000F025F File Offset: 0x000EE45F
	public bool HasCriteria(HashedString criteriaId)
	{
		return this.criteriaStates.ContainsKey(criteriaId.HashValue);
	}

	// Token: 0x060079EC RID: 31212 RVA: 0x00316D8C File Offset: 0x00314F8C
	public bool HasBehavior(QuestCriteria.BehaviorFlags behavior)
	{
		bool flag = false;
		int num = 0;
		while (!flag && num < this.quest.Criteria.Length)
		{
			flag = ((this.quest.Criteria[num].EvaluationBehaviors & behavior) > QuestCriteria.BehaviorFlags.None);
			num++;
		}
		return flag;
	}

	// Token: 0x060079ED RID: 31213 RVA: 0x00316DD0 File Offset: 0x00314FD0
	public int GetTargetCount(HashedString criteriaId)
	{
		QuestInstance.CriteriaState criteriaState;
		if (!this.criteriaStates.TryGetValue(criteriaId.HashValue, out criteriaState))
		{
			return 0;
		}
		return this.quest.Criteria[criteriaState.Handle].RequiredCount;
	}

	// Token: 0x060079EE RID: 31214 RVA: 0x00316E0C File Offset: 0x0031500C
	public int GetCurrentCount(HashedString criteriaId)
	{
		QuestInstance.CriteriaState criteriaState;
		if (!this.criteriaStates.TryGetValue(criteriaId.HashValue, out criteriaState))
		{
			return 0;
		}
		return criteriaState.CurrentCount;
	}

	// Token: 0x060079EF RID: 31215 RVA: 0x00316E38 File Offset: 0x00315038
	public float GetCurrentValue(HashedString criteriaId, int valueHandle = 0)
	{
		QuestInstance.CriteriaState criteriaState;
		if (!this.criteriaStates.TryGetValue(criteriaId.HashValue, out criteriaState) || criteriaState.CurrentValues == null)
		{
			return float.NaN;
		}
		return criteriaState.CurrentValues[valueHandle];
	}

	// Token: 0x060079F0 RID: 31216 RVA: 0x00316E74 File Offset: 0x00315074
	public float GetTargetValue(HashedString criteriaId, int valueHandle = 0)
	{
		QuestInstance.CriteriaState criteriaState;
		if (!this.criteriaStates.TryGetValue(criteriaId.HashValue, out criteriaState))
		{
			return float.NaN;
		}
		if (this.quest.Criteria[criteriaState.Handle].TargetValues == null)
		{
			return float.NaN;
		}
		return this.quest.Criteria[criteriaState.Handle].TargetValues[valueHandle];
	}

	// Token: 0x060079F1 RID: 31217 RVA: 0x00316ED8 File Offset: 0x003150D8
	public Tag GetSatisfyingItem(HashedString criteriaId, int valueHandle = 0)
	{
		QuestInstance.CriteriaState criteriaState;
		if (!this.criteriaStates.TryGetValue(criteriaId.HashValue, out criteriaState) || criteriaState.SatisfyingItems == null)
		{
			return default(Tag);
		}
		return criteriaState.SatisfyingItems[valueHandle];
	}

	// Token: 0x060079F2 RID: 31218 RVA: 0x00316F1C File Offset: 0x0031511C
	public float GetAreaAverage(HashedString criteriaId)
	{
		QuestInstance.CriteriaState criteriaState;
		if (!this.criteriaStates.TryGetValue(criteriaId.HashValue, out criteriaState))
		{
			return float.NaN;
		}
		if (!QuestCriteria.HasBehavior(this.quest.Criteria[criteriaState.Handle].EvaluationBehaviors, (QuestCriteria.BehaviorFlags)5))
		{
			return float.NaN;
		}
		float num = 0f;
		for (int i = 0; i < criteriaState.CurrentValues.Length; i++)
		{
			num += criteriaState.CurrentValues[i];
		}
		return num / (float)criteriaState.CurrentValues.Length;
	}

	// Token: 0x060079F3 RID: 31219 RVA: 0x00316F9C File Offset: 0x0031519C
	public bool IsItemRedundant(HashedString criteriaId, Tag item)
	{
		QuestInstance.CriteriaState criteriaState;
		if (!this.criteriaStates.TryGetValue(criteriaId.HashValue, out criteriaState) || criteriaState.SatisfyingItems == null)
		{
			return false;
		}
		bool flag = false;
		int num = 0;
		while (!flag && num < criteriaState.SatisfyingItems.Length)
		{
			flag = (criteriaState.SatisfyingItems[num] == item);
			num++;
		}
		return flag;
	}

	// Token: 0x060079F4 RID: 31220 RVA: 0x00316FF8 File Offset: 0x003151F8
	public bool IsCriteriaSatisfied(HashedString id)
	{
		QuestInstance.CriteriaState criteriaState;
		return this.criteriaStates.TryGetValue(id.HashValue, out criteriaState) && this.quest.Criteria[criteriaState.Handle].IsSatisfied(criteriaState.SatisfactionState, this.GetSatisfactionMask(criteriaState));
	}

	// Token: 0x060079F5 RID: 31221 RVA: 0x00317044 File Offset: 0x00315244
	public bool IsCriteriaSatisfied(Tag id)
	{
		QuestInstance.CriteriaState criteriaState;
		return this.criteriaStates.TryGetValue(id.GetHash(), out criteriaState) && this.quest.Criteria[criteriaState.Handle].IsSatisfied(criteriaState.SatisfactionState, this.GetSatisfactionMask(criteriaState));
	}

	// Token: 0x060079F6 RID: 31222 RVA: 0x00317090 File Offset: 0x00315290
	public void TrackAreaForCriteria(HashedString criteriaId, Extents area)
	{
		QuestInstance.CriteriaState criteriaState;
		if (!this.criteriaStates.TryGetValue(criteriaId.HashValue, out criteriaState))
		{
			return;
		}
		int num = area.width * area.height;
		QuestCriteria questCriteria = this.quest.Criteria[criteriaState.Handle];
		global::Debug.Assert(num <= 32);
		if (QuestCriteria.HasBehavior(questCriteria.EvaluationBehaviors, QuestCriteria.BehaviorFlags.TrackValues))
		{
			criteriaState.CurrentValues = new float[num];
		}
		if (QuestCriteria.HasBehavior(questCriteria.EvaluationBehaviors, QuestCriteria.BehaviorFlags.TrackItems))
		{
			criteriaState.SatisfyingItems = new Tag[num];
		}
		this.criteriaStates[criteriaId.HashValue] = criteriaState;
	}

	// Token: 0x060079F7 RID: 31223 RVA: 0x0031712C File Offset: 0x0031532C
	private uint GetSatisfactionMask(QuestInstance.CriteriaState state)
	{
		QuestCriteria questCriteria = this.quest.Criteria[state.Handle];
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
			return (uint)(Mathf.Pow(2f, (float)num) - 1f);
		}
		return questCriteria.GetSatisfactionMask();
	}

	// Token: 0x060079F8 RID: 31224 RVA: 0x0031719C File Offset: 0x0031539C
	public int TrackProgress(Quest.ItemData data, out bool dataSatisfies, out bool itemIsRedundant)
	{
		dataSatisfies = false;
		itemIsRedundant = false;
		QuestInstance.CriteriaState criteriaState;
		if (!this.criteriaStates.TryGetValue(data.CriteriaId.HashValue, out criteriaState))
		{
			return -1;
		}
		int valueHandle = data.ValueHandle;
		QuestCriteria questCriteria = this.quest.Criteria[criteriaState.Handle];
		dataSatisfies = this.DataSatisfiesCriteria(data, ref valueHandle);
		if (valueHandle == -1)
		{
			return valueHandle;
		}
		bool flag = QuestCriteria.HasBehavior(questCriteria.EvaluationBehaviors, QuestCriteria.BehaviorFlags.AllowsRegression);
		bool flag2 = QuestCriteria.HasBehavior(questCriteria.EvaluationBehaviors, QuestCriteria.BehaviorFlags.TrackItems);
		Tag tag = flag2 ? criteriaState.SatisfyingItems[valueHandle] : default(Tag);
		if (dataSatisfies)
		{
			itemIsRedundant = (QuestCriteria.HasBehavior(questCriteria.EvaluationBehaviors, QuestCriteria.BehaviorFlags.UniqueItems) && this.IsItemRedundant(data.CriteriaId, data.SatisfyingItem));
			if (itemIsRedundant)
			{
				return valueHandle;
			}
			tag = data.SatisfyingItem;
			criteriaState.SatisfactionState |= questCriteria.GetValueMask(valueHandle);
		}
		else if (flag)
		{
			criteriaState.SatisfactionState &= ~questCriteria.GetValueMask(valueHandle);
		}
		if (QuestCriteria.HasBehavior(questCriteria.EvaluationBehaviors, QuestCriteria.BehaviorFlags.TrackValues))
		{
			criteriaState.CurrentValues[valueHandle] = data.CurrentValue;
		}
		if (flag2)
		{
			criteriaState.SatisfyingItems[valueHandle] = tag;
		}
		bool flag3 = this.IsCriteriaSatisfied(data.CriteriaId);
		bool flag4 = questCriteria.IsSatisfied(criteriaState.SatisfactionState, this.GetSatisfactionMask(criteriaState));
		if (flag3 != flag4)
		{
			criteriaState.CurrentCount += (flag3 ? -1 : 1);
			if (flag4 && criteriaState.CurrentCount < questCriteria.RequiredCount)
			{
				criteriaState.SatisfactionState = 0U;
			}
		}
		this.criteriaStates[data.CriteriaId.HashValue] = criteriaState;
		this.UpdateQuestProgress(true);
		return valueHandle;
	}

	// Token: 0x060079F9 RID: 31225 RVA: 0x00317338 File Offset: 0x00315538
	public bool DataSatisfiesCriteria(Quest.ItemData data, ref int valueHandle)
	{
		QuestInstance.CriteriaState criteriaState;
		if (!this.criteriaStates.TryGetValue(data.CriteriaId.HashValue, out criteriaState))
		{
			return false;
		}
		QuestCriteria questCriteria = this.quest.Criteria[criteriaState.Handle];
		bool flag = questCriteria.AcceptedTags == null || (data.QualifyingTag.IsValid && questCriteria.AcceptedTags.Contains(data.QualifyingTag));
		if (flag && questCriteria.TargetValues == null)
		{
			valueHandle = 0;
		}
		if (!flag || valueHandle != -1)
		{
			return flag && questCriteria.ValueSatisfies(data.CurrentValue, valueHandle);
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
			if (flag2 && (num == -1 || criteriaState.CurrentValues[num] > criteriaState.CurrentValues[num2]))
			{
				num = num2;
			}
			num2++;
		}
		if (valueHandle == -1 && num != -1)
		{
			valueHandle = questCriteria.RequiredCount * num + Mathf.Min(criteriaState.CurrentCount, questCriteria.RequiredCount - 1);
		}
		return flag3;
	}

	// Token: 0x060079FA RID: 31226 RVA: 0x00317470 File Offset: 0x00315670
	private void UpdateQuestProgress(bool startQuest = false)
	{
		if (!this.IsStarted && !startQuest)
		{
			return;
		}
		float currentProgress = this.CurrentProgress;
		Quest.State state = this.currentState;
		this.currentState = Quest.State.InProgress;
		this.CurrentProgress = 0f;
		float num = 0f;
		for (int i = 0; i < this.quest.Criteria.Length; i++)
		{
			QuestCriteria questCriteria = this.quest.Criteria[i];
			QuestInstance.CriteriaState criteriaState = this.criteriaStates[questCriteria.CriteriaId.GetHash()];
			float num2 = (float)((questCriteria.TargetValues != null) ? questCriteria.TargetValues.Length : 1);
			num += (float)questCriteria.RequiredCount;
			this.CurrentProgress += (float)criteriaState.CurrentCount;
			if (!this.IsCriteriaSatisfied(questCriteria.CriteriaId))
			{
				float num3 = 0f;
				int num4 = 0;
				while (questCriteria.TargetValues != null && (float)num4 < num2)
				{
					if ((criteriaState.SatisfactionState & questCriteria.GetValueMask(num4)) == 0U)
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
				this.CurrentProgress += num3 / num2;
			}
		}
		this.CurrentProgress = Mathf.Clamp01(this.CurrentProgress / num);
		if (this.CurrentProgress == 1f)
		{
			this.currentState = Quest.State.Completed;
		}
		float num6 = this.CurrentProgress - currentProgress;
		if (state != this.currentState || Mathf.Abs(num6) > Mathf.Epsilon)
		{
			Action<QuestInstance, Quest.State, float> questProgressChanged = this.QuestProgressChanged;
			if (questProgressChanged == null)
			{
				return;
			}
			questProgressChanged(this, state, num6);
		}
	}

	// Token: 0x060079FB RID: 31227 RVA: 0x0031764C File Offset: 0x0031584C
	public ICheckboxListGroupControl.CheckboxItem[] GetCheckBoxData(Func<int, string, QuestInstance, string> resolveToolTip = null)
	{
		ICheckboxListGroupControl.CheckboxItem[] array = new ICheckboxListGroupControl.CheckboxItem[this.quest.Criteria.Length];
		for (int i = 0; i < this.quest.Criteria.Length; i++)
		{
			QuestCriteria c = this.quest.Criteria[i];
			array[i] = new ICheckboxListGroupControl.CheckboxItem
			{
				text = c.Text,
				isOn = this.IsCriteriaSatisfied(c.CriteriaId),
				tooltip = c.Tooltip
			};
			if (resolveToolTip != null)
			{
				array[i].resolveTooltipCallback = ((string tooltip, object owner) => resolveToolTip(c.CriteriaId.GetHash(), c.Tooltip, this));
			}
		}
		return array;
	}

	// Token: 0x060079FC RID: 31228 RVA: 0x00317734 File Offset: 0x00315934
	public void ValidateCriteriasOnLoad()
	{
		if (this.criteriaStates.Count != this.quest.Criteria.Length)
		{
			Dictionary<int, QuestInstance.CriteriaState> dictionary = new Dictionary<int, QuestInstance.CriteriaState>(this.quest.Criteria.Length);
			for (int i = 0; i < this.quest.Criteria.Length; i++)
			{
				QuestCriteria questCriteria = this.quest.Criteria[i];
				int hash = questCriteria.CriteriaId.GetHash();
				if (this.criteriaStates.ContainsKey(hash))
				{
					dictionary[hash] = this.criteriaStates[hash];
				}
				else
				{
					QuestInstance.CriteriaState value = new QuestInstance.CriteriaState
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
					dictionary[hash] = value;
				}
			}
			this.criteriaStates = dictionary;
		}
	}

	// Token: 0x04005B7F RID: 23423
	public Action<QuestInstance, Quest.State, float> QuestProgressChanged;

	// Token: 0x04005B81 RID: 23425
	private Quest quest;

	// Token: 0x04005B82 RID: 23426
	[Serialize]
	private Dictionary<int, QuestInstance.CriteriaState> criteriaStates;

	// Token: 0x04005B83 RID: 23427
	[Serialize]
	private Quest.State currentState;

	// Token: 0x0200171D RID: 5917
	private struct CriteriaState
	{
		// Token: 0x060079FD RID: 31229 RVA: 0x00317848 File Offset: 0x00315A48
		public static bool ItemAlreadySatisfying(QuestInstance.CriteriaState state, Tag item)
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

		// Token: 0x04005B84 RID: 23428
		public int Handle;

		// Token: 0x04005B85 RID: 23429
		public int CurrentCount;

		// Token: 0x04005B86 RID: 23430
		public uint SatisfactionState;

		// Token: 0x04005B87 RID: 23431
		public Tag[] SatisfyingItems;

		// Token: 0x04005B88 RID: 23432
		public float[] CurrentValues;
	}
}
