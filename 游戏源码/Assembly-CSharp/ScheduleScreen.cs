using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02001F0A RID: 7946
public class ScheduleScreen : KScreen
{
	// Token: 0x17000AB2 RID: 2738
	// (get) Token: 0x0600A791 RID: 42897 RVA: 0x0010CB6C File Offset: 0x0010AD6C
	// (set) Token: 0x0600A792 RID: 42898 RVA: 0x0010CB74 File Offset: 0x0010AD74
	public string SelectedPaint { get; set; }

	// Token: 0x0600A793 RID: 42899 RVA: 0x000FD501 File Offset: 0x000FB701
	public override float GetSortKey()
	{
		return 50f;
	}

	// Token: 0x0600A794 RID: 42900 RVA: 0x0010CB7D File Offset: 0x0010AD7D
	protected override void OnPrefabInit()
	{
		base.ConsumeMouseScroll = true;
		this.scheduleEntries = new List<ScheduleScreenEntry>();
		ScheduleScreen.Instance = this;
	}

	// Token: 0x0600A795 RID: 42901 RVA: 0x003F953C File Offset: 0x003F773C
	protected override void OnSpawn()
	{
		foreach (Schedule schedule in ScheduleManager.Instance.GetSchedules())
		{
			this.AddScheduleEntry(schedule);
		}
		this.addScheduleButton.onClick += this.OnAddScheduleClick;
		this.closeButton.onClick += delegate()
		{
			ManagementMenu.Instance.CloseAll();
		};
		ScheduleManager.Instance.onSchedulesChanged += this.OnSchedulesChanged;
		Game.Instance.Subscribe(1983128072, new Action<object>(this.RefreshWidgetWorldData));
	}

	// Token: 0x0600A796 RID: 42902 RVA: 0x0010CB97 File Offset: 0x0010AD97
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		ScheduleManager.Instance.onSchedulesChanged -= this.OnSchedulesChanged;
		ScheduleScreen.Instance = null;
	}

	// Token: 0x0600A797 RID: 42903 RVA: 0x0010CBBB File Offset: 0x0010ADBB
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		if (show)
		{
			base.Activate();
		}
	}

	// Token: 0x0600A798 RID: 42904 RVA: 0x003F9608 File Offset: 0x003F7808
	public void RefreshAllPaintButtons()
	{
		foreach (ScheduleScreenEntry scheduleScreenEntry in this.scheduleEntries)
		{
			scheduleScreenEntry.RefreshPaintButtons();
		}
	}

	// Token: 0x0600A799 RID: 42905 RVA: 0x0010CBCD File Offset: 0x0010ADCD
	private void OnAddScheduleClick()
	{
		ScheduleManager.Instance.AddDefaultSchedule(false);
	}

	// Token: 0x0600A79A RID: 42906 RVA: 0x003F9658 File Offset: 0x003F7858
	private void AddScheduleEntry(Schedule schedule)
	{
		ScheduleScreenEntry scheduleScreenEntry = Util.KInstantiateUI<ScheduleScreenEntry>(this.scheduleEntryPrefab.gameObject, this.scheduleEntryContainer, true);
		scheduleScreenEntry.Setup(schedule);
		this.scheduleEntries.Add(scheduleScreenEntry);
	}

	// Token: 0x0600A79B RID: 42907 RVA: 0x003F9690 File Offset: 0x003F7890
	private void OnSchedulesChanged(List<Schedule> schedules)
	{
		foreach (ScheduleScreenEntry original in this.scheduleEntries)
		{
			Util.KDestroyGameObject(original);
		}
		this.scheduleEntries.Clear();
		foreach (Schedule schedule in schedules)
		{
			this.AddScheduleEntry(schedule);
		}
	}

	// Token: 0x0600A79C RID: 42908 RVA: 0x003F9728 File Offset: 0x003F7928
	private void RefreshWidgetWorldData(object data = null)
	{
		foreach (ScheduleScreenEntry scheduleScreenEntry in this.scheduleEntries)
		{
			scheduleScreenEntry.RefreshWidgetWorldData();
		}
	}

	// Token: 0x0600A79D RID: 42909 RVA: 0x003F9778 File Offset: 0x003F7978
	public void OnChangeCurrentTimetable()
	{
		foreach (ScheduleScreenEntry scheduleScreenEntry in this.scheduleEntries)
		{
			scheduleScreenEntry.RefreshTimeOfDayPositioner();
		}
	}

	// Token: 0x0600A79E RID: 42910 RVA: 0x0010CBDA File Offset: 0x0010ADDA
	public override void OnKeyDown(KButtonEvent e)
	{
		if (this.CheckBlockedInput())
		{
			if (!e.Consumed)
			{
				e.Consumed = true;
				return;
			}
		}
		else
		{
			base.OnKeyDown(e);
		}
	}

	// Token: 0x0600A79F RID: 42911 RVA: 0x003F97C8 File Offset: 0x003F79C8
	private bool CheckBlockedInput()
	{
		bool result = false;
		if (UnityEngine.EventSystems.EventSystem.current != null)
		{
			GameObject currentSelectedGameObject = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
			if (currentSelectedGameObject != null)
			{
				foreach (ScheduleScreenEntry scheduleScreenEntry in this.scheduleEntries)
				{
					if (currentSelectedGameObject == scheduleScreenEntry.GetNameInputField())
					{
						result = true;
						break;
					}
				}
			}
		}
		return result;
	}

	// Token: 0x040083BA RID: 33722
	public static ScheduleScreen Instance;

	// Token: 0x040083BC RID: 33724
	[SerializeField]
	private ScheduleScreenEntry scheduleEntryPrefab;

	// Token: 0x040083BD RID: 33725
	[SerializeField]
	private GameObject scheduleEntryContainer;

	// Token: 0x040083BE RID: 33726
	[SerializeField]
	private KButton addScheduleButton;

	// Token: 0x040083BF RID: 33727
	[SerializeField]
	private KButton closeButton;

	// Token: 0x040083C0 RID: 33728
	private List<ScheduleScreenEntry> scheduleEntries;
}
