using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02001842 RID: 6210
[SerializationConfig(MemberSerialization.OptIn)]
public class Schedule : ISaveLoadable, IListableOption
{
	// Token: 0x17000835 RID: 2101
	// (get) Token: 0x0600805E RID: 32862 RVA: 0x000F4841 File Offset: 0x000F2A41
	// (set) Token: 0x0600805F RID: 32863 RVA: 0x000F4849 File Offset: 0x000F2A49
	public int ProgressTimetableIdx
	{
		get
		{
			return this.progressTimetableIdx;
		}
		set
		{
			this.progressTimetableIdx = value;
		}
	}

	// Token: 0x06008060 RID: 32864 RVA: 0x000F4852 File Offset: 0x000F2A52
	public ScheduleBlock GetCurrentScheduleBlock()
	{
		return this.GetBlock(this.GetCurrentBlockIdx());
	}

	// Token: 0x06008061 RID: 32865 RVA: 0x000F4860 File Offset: 0x000F2A60
	public int GetCurrentBlockIdx()
	{
		return Math.Min((int)(GameClock.Instance.GetCurrentCycleAsPercentage() * 24f), 23) + this.progressTimetableIdx * 24;
	}

	// Token: 0x06008062 RID: 32866 RVA: 0x000F4884 File Offset: 0x000F2A84
	public ScheduleBlock GetPreviousScheduleBlock()
	{
		return this.GetBlock(this.GetPreviousBlockIdx());
	}

	// Token: 0x06008063 RID: 32867 RVA: 0x00334504 File Offset: 0x00332704
	public int GetPreviousBlockIdx()
	{
		int num = this.GetCurrentBlockIdx() - 1;
		if (num == -1)
		{
			num = this.blocks.Count - 1;
		}
		return num;
	}

	// Token: 0x06008064 RID: 32868 RVA: 0x000F4892 File Offset: 0x000F2A92
	public void ClearNullReferences()
	{
		this.assigned.RemoveAll((Ref<Schedulable> x) => x.Get() == null);
	}

	// Token: 0x06008065 RID: 32869 RVA: 0x00334530 File Offset: 0x00332730
	public Schedule(string name, List<ScheduleGroup> defaultGroups, bool alarmActivated)
	{
		this.name = name;
		this.alarmActivated = alarmActivated;
		this.blocks = new List<ScheduleBlock>(defaultGroups.Count);
		this.assigned = new List<Ref<Schedulable>>();
		this.tones = this.GenerateTones();
		this.SetBlocksToGroupDefaults(defaultGroups);
	}

	// Token: 0x06008066 RID: 32870 RVA: 0x00334588 File Offset: 0x00332788
	public Schedule(string name, List<ScheduleBlock> sourceBlocks, bool alarmActivated)
	{
		this.name = name;
		this.alarmActivated = alarmActivated;
		this.blocks = new List<ScheduleBlock>();
		for (int i = 0; i < sourceBlocks.Count; i++)
		{
			this.blocks.Add(new ScheduleBlock(sourceBlocks[i].name, sourceBlocks[i].GroupId));
		}
		this.assigned = new List<Ref<Schedulable>>();
		this.tones = this.GenerateTones();
		this.Changed();
	}

	// Token: 0x06008067 RID: 32871 RVA: 0x000F48BF File Offset: 0x000F2ABF
	public void SetBlocksToGroupDefaults(List<ScheduleGroup> defaultGroups)
	{
		this.blocks = Schedule.GetScheduleBlocksFromGroupDefaults(defaultGroups);
		global::Debug.Assert(this.blocks.Count == 24);
		this.Changed();
	}

	// Token: 0x06008068 RID: 32872 RVA: 0x00334614 File Offset: 0x00332814
	public static List<ScheduleBlock> GetScheduleBlocksFromGroupDefaults(List<ScheduleGroup> defaultGroups)
	{
		List<ScheduleBlock> list = new List<ScheduleBlock>();
		for (int i = 0; i < defaultGroups.Count; i++)
		{
			ScheduleGroup scheduleGroup = defaultGroups[i];
			for (int j = 0; j < scheduleGroup.defaultSegments; j++)
			{
				list.Add(new ScheduleBlock(scheduleGroup.Name, scheduleGroup.Id));
			}
		}
		return list;
	}

	// Token: 0x06008069 RID: 32873 RVA: 0x0033466C File Offset: 0x0033286C
	public void Tick()
	{
		ScheduleBlock currentScheduleBlock = this.GetCurrentScheduleBlock();
		ScheduleBlock block = this.GetBlock(this.GetPreviousBlockIdx());
		global::Debug.Assert(block != currentScheduleBlock);
		if (this.GetCurrentBlockIdx() % 24 == 0)
		{
			this.progressTimetableIdx++;
			if (this.progressTimetableIdx >= this.blocks.Count / 24)
			{
				this.progressTimetableIdx = 0;
			}
			if (ScheduleScreen.Instance != null)
			{
				ScheduleScreen.Instance.OnChangeCurrentTimetable();
			}
		}
		if (!Schedule.AreScheduleTypesIdentical(currentScheduleBlock.allowed_types, block.allowed_types))
		{
			ScheduleGroup scheduleGroup = Db.Get().ScheduleGroups.FindGroupForScheduleTypes(currentScheduleBlock.allowed_types);
			ScheduleGroup scheduleGroup2 = Db.Get().ScheduleGroups.FindGroupForScheduleTypes(block.allowed_types);
			if (this.alarmActivated && scheduleGroup2.alarm != scheduleGroup.alarm)
			{
				ScheduleManager.Instance.PlayScheduleAlarm(this, currentScheduleBlock, scheduleGroup.alarm);
			}
			foreach (Ref<Schedulable> @ref in this.GetAssigned())
			{
				@ref.Get().OnScheduleBlocksChanged(this);
			}
		}
		foreach (Ref<Schedulable> ref2 in this.GetAssigned())
		{
			ref2.Get().OnScheduleBlocksTick(this);
		}
	}

	// Token: 0x0600806A RID: 32874 RVA: 0x000F48E7 File Offset: 0x000F2AE7
	string IListableOption.GetProperName()
	{
		return this.name;
	}

	// Token: 0x0600806B RID: 32875 RVA: 0x003347E0 File Offset: 0x003329E0
	public int[] GenerateTones()
	{
		int minToneIndex = TuningData<ScheduleManager.Tuning>.Get().minToneIndex;
		int maxToneIndex = TuningData<ScheduleManager.Tuning>.Get().maxToneIndex;
		int firstLastToneSpacing = TuningData<ScheduleManager.Tuning>.Get().firstLastToneSpacing;
		int[] array = new int[4];
		array[0] = UnityEngine.Random.Range(minToneIndex, maxToneIndex - firstLastToneSpacing + 1);
		array[1] = UnityEngine.Random.Range(minToneIndex, maxToneIndex + 1);
		array[2] = UnityEngine.Random.Range(minToneIndex, maxToneIndex + 1);
		array[3] = UnityEngine.Random.Range(array[0] + firstLastToneSpacing, maxToneIndex + 1);
		return array;
	}

	// Token: 0x0600806C RID: 32876 RVA: 0x000F48EF File Offset: 0x000F2AEF
	public List<Ref<Schedulable>> GetAssigned()
	{
		if (this.assigned == null)
		{
			this.assigned = new List<Ref<Schedulable>>();
		}
		return this.assigned;
	}

	// Token: 0x0600806D RID: 32877 RVA: 0x000F490A File Offset: 0x000F2B0A
	public int[] GetTones()
	{
		if (this.tones == null)
		{
			this.tones = this.GenerateTones();
		}
		return this.tones;
	}

	// Token: 0x0600806E RID: 32878 RVA: 0x000F4926 File Offset: 0x000F2B26
	public void SetBlockGroup(int idx, ScheduleGroup group)
	{
		if (0 <= idx && idx < this.blocks.Count)
		{
			this.blocks[idx] = new ScheduleBlock(group.Name, group.Id);
			this.Changed();
		}
	}

	// Token: 0x0600806F RID: 32879 RVA: 0x0033484C File Offset: 0x00332A4C
	private void Changed()
	{
		foreach (Ref<Schedulable> @ref in this.GetAssigned())
		{
			@ref.Get().OnScheduleChanged(this);
		}
		if (this.onChanged != null)
		{
			this.onChanged(this);
		}
	}

	// Token: 0x06008070 RID: 32880 RVA: 0x000F495D File Offset: 0x000F2B5D
	public List<ScheduleBlock> GetBlocks()
	{
		return this.blocks;
	}

	// Token: 0x06008071 RID: 32881 RVA: 0x000F4965 File Offset: 0x000F2B65
	public ScheduleBlock GetBlock(int idx)
	{
		return this.blocks[idx];
	}

	// Token: 0x06008072 RID: 32882 RVA: 0x000F4973 File Offset: 0x000F2B73
	public void InsertTimetable(int timetableIdx, List<ScheduleBlock> newBlocks)
	{
		this.blocks.InsertRange(timetableIdx * 24, newBlocks);
		if (timetableIdx <= this.progressTimetableIdx)
		{
			this.progressTimetableIdx++;
		}
	}

	// Token: 0x06008073 RID: 32883 RVA: 0x000F499C File Offset: 0x000F2B9C
	public void AddTimetable(List<ScheduleBlock> newBlocks)
	{
		this.blocks.AddRange(newBlocks);
	}

	// Token: 0x06008074 RID: 32884 RVA: 0x003348B8 File Offset: 0x00332AB8
	public void RemoveTimetable(int TimetableToRemoveIdx)
	{
		int index = TimetableToRemoveIdx * 24;
		int num = this.blocks.Count / 24;
		this.blocks.RemoveRange(index, 24);
		bool flag = TimetableToRemoveIdx == this.progressTimetableIdx;
		bool flag2 = this.progressTimetableIdx == num - 1;
		if (TimetableToRemoveIdx < this.progressTimetableIdx || (flag && flag2))
		{
			this.progressTimetableIdx--;
		}
		ScheduleScreen.Instance.OnChangeCurrentTimetable();
	}

	// Token: 0x06008075 RID: 32885 RVA: 0x000F49AA File Offset: 0x000F2BAA
	public void Assign(Schedulable schedulable)
	{
		if (!this.IsAssigned(schedulable))
		{
			this.GetAssigned().Add(new Ref<Schedulable>(schedulable));
		}
		this.Changed();
	}

	// Token: 0x06008076 RID: 32886 RVA: 0x00334924 File Offset: 0x00332B24
	public void Unassign(Schedulable schedulable)
	{
		for (int i = 0; i < this.GetAssigned().Count; i++)
		{
			if (this.GetAssigned()[i].Get() == schedulable)
			{
				this.GetAssigned().RemoveAt(i);
				break;
			}
		}
		this.Changed();
	}

	// Token: 0x06008077 RID: 32887 RVA: 0x00334974 File Offset: 0x00332B74
	public bool IsAssigned(Schedulable schedulable)
	{
		using (List<Ref<Schedulable>>.Enumerator enumerator = this.GetAssigned().GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.Get() == schedulable)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06008078 RID: 32888 RVA: 0x003349D4 File Offset: 0x00332BD4
	public static bool AreScheduleTypesIdentical(List<ScheduleBlockType> a, List<ScheduleBlockType> b)
	{
		if (a.Count != b.Count)
		{
			return false;
		}
		foreach (ScheduleBlockType scheduleBlockType in a)
		{
			bool flag = false;
			foreach (ScheduleBlockType scheduleBlockType2 in b)
			{
				if (scheduleBlockType.IdHash == scheduleBlockType2.IdHash)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06008079 RID: 32889 RVA: 0x00334A88 File Offset: 0x00332C88
	public bool ShiftTimetable(bool up, int timetableToShiftIdx = 0)
	{
		if (timetableToShiftIdx == 0 && up)
		{
			return false;
		}
		if (timetableToShiftIdx == this.blocks.Count / 24 - 1 && !up)
		{
			return false;
		}
		int num = timetableToShiftIdx * 24;
		List<ScheduleBlock> collection = new List<ScheduleBlock>();
		List<ScheduleBlock> collection2 = new List<ScheduleBlock>();
		if (up)
		{
			collection = this.blocks.GetRange(num, 24);
			collection2 = this.blocks.GetRange(num - 24, 24);
			this.blocks.RemoveRange(num - 24, 48);
			this.blocks.InsertRange(num - 24, collection2);
			this.blocks.InsertRange(num - 24, collection);
		}
		else
		{
			collection = this.blocks.GetRange(num, 24);
			collection2 = this.blocks.GetRange(num + 24, 24);
			this.blocks.RemoveRange(num, 48);
			this.blocks.InsertRange(num, collection);
			this.blocks.InsertRange(num, collection2);
		}
		this.Changed();
		return true;
	}

	// Token: 0x0600807A RID: 32890 RVA: 0x00334B70 File Offset: 0x00332D70
	public void RotateBlocks(bool directionLeft, int timetableToRotateIdx = 0)
	{
		List<ScheduleBlock> list = new List<ScheduleBlock>();
		int index = timetableToRotateIdx * 24;
		list = this.blocks.GetRange(index, 24);
		if (!directionLeft)
		{
			ScheduleGroup scheduleGroup = Db.Get().ScheduleGroups.Get(list[list.Count - 1].GroupId);
			for (int i = list.Count - 1; i >= 1; i--)
			{
				ScheduleGroup scheduleGroup2 = Db.Get().ScheduleGroups.Get(list[i - 1].GroupId);
				list[i].GroupId = scheduleGroup2.Id;
			}
			list[0].GroupId = scheduleGroup.Id;
		}
		else
		{
			ScheduleGroup scheduleGroup3 = Db.Get().ScheduleGroups.Get(list[0].GroupId);
			for (int j = 0; j < list.Count - 1; j++)
			{
				ScheduleGroup scheduleGroup4 = Db.Get().ScheduleGroups.Get(list[j + 1].GroupId);
				list[j].GroupId = scheduleGroup4.Id;
			}
			list[list.Count - 1].GroupId = scheduleGroup3.Id;
		}
		this.blocks.RemoveRange(index, 24);
		this.blocks.InsertRange(index, list);
		this.Changed();
	}

	// Token: 0x04006152 RID: 24914
	[Serialize]
	private List<ScheduleBlock> blocks;

	// Token: 0x04006153 RID: 24915
	[Serialize]
	private List<Ref<Schedulable>> assigned;

	// Token: 0x04006154 RID: 24916
	[Serialize]
	public string name;

	// Token: 0x04006155 RID: 24917
	[Serialize]
	public bool alarmActivated = true;

	// Token: 0x04006156 RID: 24918
	[Serialize]
	private int[] tones;

	// Token: 0x04006157 RID: 24919
	[Serialize]
	public bool isDefaultForBionics;

	// Token: 0x04006158 RID: 24920
	[Serialize]
	private int progressTimetableIdx;

	// Token: 0x04006159 RID: 24921
	public Action<Schedule> onChanged;
}
