using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using FMOD.Studio;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02001844 RID: 6212
[AddComponentMenu("KMonoBehaviour/scripts/ScheduleManager")]
public class ScheduleManager : KMonoBehaviour, ISim33ms
{
	// Token: 0x14000023 RID: 35
	// (add) Token: 0x0600807E RID: 32894 RVA: 0x00334CBC File Offset: 0x00332EBC
	// (remove) Token: 0x0600807F RID: 32895 RVA: 0x00334CF4 File Offset: 0x00332EF4
	public event Action<List<Schedule>> onSchedulesChanged;

	// Token: 0x06008080 RID: 32896 RVA: 0x000F49E6 File Offset: 0x000F2BE6
	public static void DestroyInstance()
	{
		ScheduleManager.Instance = null;
	}

	// Token: 0x06008081 RID: 32897 RVA: 0x000F49EE File Offset: 0x000F2BEE
	public Schedule GetDefaultBionicSchedule()
	{
		return this.schedules.Find((Schedule match) => match.isDefaultForBionics);
	}

	// Token: 0x06008082 RID: 32898 RVA: 0x000F4A1A File Offset: 0x000F2C1A
	[OnDeserialized]
	private void OnDeserialized()
	{
		if (this.schedules.Count == 0)
		{
			this.AddDefaultSchedule(true);
		}
	}

	// Token: 0x06008083 RID: 32899 RVA: 0x000F4A30 File Offset: 0x000F2C30
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.schedules = new List<Schedule>();
		ScheduleManager.Instance = this;
	}

	// Token: 0x06008084 RID: 32900 RVA: 0x00334D2C File Offset: 0x00332F2C
	protected override void OnSpawn()
	{
		if (this.schedules.Count == 0)
		{
			this.AddDefaultSchedule(true);
		}
		foreach (Schedule schedule in this.schedules)
		{
			schedule.ClearNullReferences();
		}
		List<ScheduleBlock> scheduleBlocksFromGroupDefaults = Schedule.GetScheduleBlocksFromGroupDefaults(Db.Get().ScheduleGroups.allGroups);
		foreach (Schedule schedule2 in this.schedules)
		{
			List<ScheduleBlock> blocks = schedule2.GetBlocks();
			for (int i = 0; i < blocks.Count; i++)
			{
				ScheduleBlock scheduleBlock = blocks[i];
				if (Db.Get().ScheduleGroups.FindGroupForScheduleTypes(scheduleBlock.allowed_types) == null)
				{
					ScheduleGroup group = Db.Get().ScheduleGroups.FindGroupForScheduleTypes(scheduleBlocksFromGroupDefaults[i].allowed_types);
					schedule2.SetBlockGroup(i, group);
				}
			}
		}
		foreach (MinionIdentity minionIdentity in Components.LiveMinionIdentities.Items)
		{
			Schedulable component = minionIdentity.GetComponent<Schedulable>();
			if (this.GetSchedule(component) == null)
			{
				this.schedules[0].Assign(component);
			}
		}
		Components.LiveMinionIdentities.OnAdd += this.OnAddDupe;
		Components.LiveMinionIdentities.OnRemove += this.OnRemoveDupe;
	}

	// Token: 0x06008085 RID: 32901 RVA: 0x00334ED8 File Offset: 0x003330D8
	private void OnAddDupe(MinionIdentity minion)
	{
		Schedulable component = minion.GetComponent<Schedulable>();
		Schedule schedule = this.schedules[0];
		if (minion.model == GameTags.Minions.Models.Bionic)
		{
			if (this.GetDefaultBionicSchedule() == null)
			{
				if (!this.hasDeletedDefaultBionicSchedule)
				{
					Schedule schedule2 = this.AddSchedule(Db.Get().ScheduleGroups.allGroups, "_Bionics Default", false);
					schedule2.AddTimetable(Schedule.GetScheduleBlocksFromGroupDefaults(Db.Get().ScheduleGroups.allGroups));
					schedule2.AddTimetable(Schedule.GetScheduleBlocksFromGroupDefaults(Db.Get().ScheduleGroups.allGroups));
					for (int i = 0; i < schedule2.GetBlocks().Count; i++)
					{
						schedule2.SetBlockGroup(i, Db.Get().ScheduleGroups.Worktime);
					}
					for (int j = 1; j <= 6; j++)
					{
						schedule2.SetBlockGroup(schedule2.GetBlocks().Count - j, Db.Get().ScheduleGroups.Sleep);
					}
					for (int k = 7; k <= 10; k++)
					{
						schedule2.SetBlockGroup(schedule2.GetBlocks().Count - k, Db.Get().ScheduleGroups.Recreation);
					}
					for (int l = 11; l <= 11; l++)
					{
						schedule2.SetBlockGroup(schedule2.GetBlocks().Count - l, Db.Get().ScheduleGroups.Hygene);
					}
					schedule = schedule2;
					schedule2.isDefaultForBionics = true;
				}
			}
			else
			{
				schedule = this.GetDefaultBionicSchedule();
			}
		}
		else if (this.GetSchedule(component) != null)
		{
			schedule = this.GetSchedule(component);
		}
		schedule.Assign(component);
	}

	// Token: 0x06008086 RID: 32902 RVA: 0x00335068 File Offset: 0x00333268
	private void OnRemoveDupe(MinionIdentity minion)
	{
		Schedulable component = minion.GetComponent<Schedulable>();
		Schedule schedule = this.GetSchedule(component);
		if (schedule != null)
		{
			schedule.Unassign(component);
		}
	}

	// Token: 0x06008087 RID: 32903 RVA: 0x00335090 File Offset: 0x00333290
	public void OnStoredDupeDestroyed(StoredMinionIdentity dupe)
	{
		foreach (Schedule schedule in this.schedules)
		{
			schedule.Unassign(dupe.gameObject.GetComponent<Schedulable>());
		}
	}

	// Token: 0x06008088 RID: 32904 RVA: 0x003350EC File Offset: 0x003332EC
	public void AddDefaultSchedule(bool alarmOn)
	{
		Schedule schedule = this.AddSchedule(Db.Get().ScheduleGroups.allGroups, UI.SCHEDULESCREEN.SCHEDULE_NAME_DEFAULT, alarmOn);
		if (Game.Instance.FastWorkersModeActive)
		{
			for (int i = 0; i < 21; i++)
			{
				schedule.SetBlockGroup(i, Db.Get().ScheduleGroups.Worktime);
			}
			schedule.SetBlockGroup(21, Db.Get().ScheduleGroups.Recreation);
			schedule.SetBlockGroup(22, Db.Get().ScheduleGroups.Recreation);
			schedule.SetBlockGroup(23, Db.Get().ScheduleGroups.Sleep);
		}
	}

	// Token: 0x06008089 RID: 32905 RVA: 0x00335190 File Offset: 0x00333390
	public Schedule AddSchedule(List<ScheduleGroup> groups, string name = null, bool alarmOn = false)
	{
		if (name == null)
		{
			this.scheduleNameIncrementor++;
			name = string.Format(UI.SCHEDULESCREEN.SCHEDULE_NAME_FORMAT, this.scheduleNameIncrementor.ToString());
		}
		Schedule schedule = new Schedule(name, groups, alarmOn);
		this.schedules.Add(schedule);
		if (this.onSchedulesChanged != null)
		{
			this.onSchedulesChanged(this.schedules);
		}
		return schedule;
	}

	// Token: 0x0600808A RID: 32906 RVA: 0x003351FC File Offset: 0x003333FC
	public Schedule DuplicateSchedule(Schedule source)
	{
		if (base.name == null)
		{
			this.scheduleNameIncrementor++;
			base.name = string.Format(UI.SCHEDULESCREEN.SCHEDULE_NAME_FORMAT, this.scheduleNameIncrementor.ToString());
		}
		Schedule schedule = new Schedule("copy of " + source.name, source.GetBlocks(), source.alarmActivated);
		schedule.ProgressTimetableIdx = source.ProgressTimetableIdx;
		this.schedules.Add(schedule);
		if (this.onSchedulesChanged != null)
		{
			this.onSchedulesChanged(this.schedules);
		}
		return schedule;
	}

	// Token: 0x0600808B RID: 32907 RVA: 0x00335294 File Offset: 0x00333494
	public void DeleteSchedule(Schedule schedule)
	{
		if (this.schedules.Count == 1)
		{
			return;
		}
		List<Ref<Schedulable>> assigned = schedule.GetAssigned();
		if (schedule.isDefaultForBionics)
		{
			this.hasDeletedDefaultBionicSchedule = true;
		}
		this.schedules.Remove(schedule);
		foreach (Ref<Schedulable> @ref in assigned)
		{
			this.schedules[0].Assign(@ref.Get());
		}
		if (this.onSchedulesChanged != null)
		{
			this.onSchedulesChanged(this.schedules);
		}
	}

	// Token: 0x0600808C RID: 32908 RVA: 0x0033533C File Offset: 0x0033353C
	public Schedule GetSchedule(Schedulable schedulable)
	{
		foreach (Schedule schedule in this.schedules)
		{
			if (schedule.IsAssigned(schedulable))
			{
				return schedule;
			}
		}
		return null;
	}

	// Token: 0x0600808D RID: 32909 RVA: 0x000F4A49 File Offset: 0x000F2C49
	public List<Schedule> GetSchedules()
	{
		return this.schedules;
	}

	// Token: 0x0600808E RID: 32910 RVA: 0x00335398 File Offset: 0x00333598
	public bool IsAllowed(Schedulable schedulable, ScheduleBlockType schedule_block_type)
	{
		Schedule schedule = this.GetSchedule(schedulable);
		return schedule != null && schedule.GetCurrentScheduleBlock().IsAllowed(schedule_block_type);
	}

	// Token: 0x0600808F RID: 32911 RVA: 0x000F4A51 File Offset: 0x000F2C51
	public static int GetCurrentHour()
	{
		return Math.Min((int)(GameClock.Instance.GetCurrentCycleAsPercentage() * 24f), 23);
	}

	// Token: 0x06008090 RID: 32912 RVA: 0x003353C0 File Offset: 0x003335C0
	public void Sim33ms(float dt)
	{
		int currentHour = ScheduleManager.GetCurrentHour();
		if (ScheduleManager.GetCurrentHour() != this.lastHour)
		{
			foreach (Schedule schedule in this.schedules)
			{
				schedule.Tick();
			}
			this.lastHour = currentHour;
		}
	}

	// Token: 0x06008091 RID: 32913 RVA: 0x0033542C File Offset: 0x0033362C
	public void PlayScheduleAlarm(Schedule schedule, ScheduleBlock block, bool forwards)
	{
		Notification notification = new Notification(string.Format(MISC.NOTIFICATIONS.SCHEDULE_CHANGED.NAME, schedule.name, block.name), NotificationType.Good, (List<Notification> notificationList, object data) => MISC.NOTIFICATIONS.SCHEDULE_CHANGED.TOOLTIP.Replace("{0}", schedule.name).Replace("{1}", block.name).Replace("{2}", Db.Get().ScheduleGroups.Get(block.GroupId).notificationTooltip), null, true, 0f, null, null, null, true, false, false);
		base.GetComponent<Notifier>().Add(notification, "");
		base.StartCoroutine(this.PlayScheduleTone(schedule, forwards));
	}

	// Token: 0x06008092 RID: 32914 RVA: 0x000F4A6B File Offset: 0x000F2C6B
	private IEnumerator PlayScheduleTone(Schedule schedule, bool forwards)
	{
		int[] tones = schedule.GetTones();
		int num2;
		for (int i = 0; i < tones.Length; i = num2 + 1)
		{
			int num = forwards ? i : (tones.Length - 1 - i);
			this.PlayTone(tones[num], forwards);
			yield return SequenceUtil.WaitForSeconds(TuningData<ScheduleManager.Tuning>.Get().toneSpacingSeconds);
			num2 = i;
		}
		yield break;
	}

	// Token: 0x06008093 RID: 32915 RVA: 0x003354B8 File Offset: 0x003336B8
	private void PlayTone(int pitch, bool forwards)
	{
		EventInstance instance = KFMOD.BeginOneShot(GlobalAssets.GetSound("WorkChime_tone", false), Vector3.zero, 1f);
		instance.setParameterByName("WorkChime_pitch", (float)pitch, false);
		instance.setParameterByName("WorkChime_start", (float)(forwards ? 1 : 0), false);
		KFMOD.EndOneShot(instance);
	}

	// Token: 0x0400615C RID: 24924
	[Serialize]
	private List<Schedule> schedules;

	// Token: 0x0400615D RID: 24925
	[Serialize]
	private int lastHour;

	// Token: 0x0400615E RID: 24926
	[Serialize]
	private int scheduleNameIncrementor;

	// Token: 0x04006160 RID: 24928
	public static ScheduleManager Instance;

	// Token: 0x04006161 RID: 24929
	[Serialize]
	private bool hasDeletedDefaultBionicSchedule;

	// Token: 0x02001845 RID: 6213
	public class Tuning : TuningData<ScheduleManager.Tuning>
	{
		// Token: 0x04006162 RID: 24930
		public float toneSpacingSeconds;

		// Token: 0x04006163 RID: 24931
		public int minToneIndex;

		// Token: 0x04006164 RID: 24932
		public int maxToneIndex;

		// Token: 0x04006165 RID: 24933
		public int firstLastToneSpacing;
	}
}
