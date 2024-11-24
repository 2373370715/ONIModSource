using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001F0E RID: 7950
[AddComponentMenu("KMonoBehaviour/scripts/ScheduleScreenEntry")]
public class ScheduleScreenEntry : KMonoBehaviour
{
	// Token: 0x17000AB3 RID: 2739
	// (get) Token: 0x0600A7A9 RID: 42921 RVA: 0x0010CC2C File Offset: 0x0010AE2C
	// (set) Token: 0x0600A7AA RID: 42922 RVA: 0x0010CC34 File Offset: 0x0010AE34
	public Schedule schedule { get; private set; }

	// Token: 0x0600A7AB RID: 42923 RVA: 0x003F984C File Offset: 0x003F7A4C
	public void Setup(Schedule schedule)
	{
		this.schedule = schedule;
		base.gameObject.name = "Schedule_" + schedule.name;
		this.title.SetTitle(schedule.name);
		this.title.OnNameChanged += this.OnNameChanged;
		this.duplicateScheduleButton.onClick += this.DuplicateSchedule;
		this.deleteScheduleButton.onClick += this.DeleteSchedule;
		this.timetableRows = new List<GameObject>();
		this.blockButtonsByTimetableRow = new Dictionary<GameObject, List<ScheduleBlockButton>>();
		int num = Mathf.CeilToInt((float)(schedule.GetBlocks().Count / 24));
		for (int i = 0; i < num; i++)
		{
			this.AddTimetableRow(i * 24);
		}
		this.minionWidgets = new List<ScheduleMinionWidget>();
		this.blankMinionWidget = Util.KInstantiateUI<ScheduleMinionWidget>(this.minionWidgetPrefab.gameObject, this.minionWidgetContainer, false);
		this.blankMinionWidget.SetupBlank(schedule);
		this.RebuildMinionWidgets();
		this.RefreshStatus();
		this.RefreshAlarmButton();
		MultiToggle multiToggle = this.alarmButton;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(this.OnAlarmClicked));
		schedule.onChanged = (Action<Schedule>)Delegate.Combine(schedule.onChanged, new Action<Schedule>(this.OnScheduleChanged));
		this.ConfigPaintButton(this.PaintButtonBathtime, Db.Get().ScheduleGroups.Hygene, Def.GetUISprite(Assets.GetPrefab(ShowerConfig.ID), "ui", false).first);
		this.ConfigPaintButton(this.PaintButtonWorktime, Db.Get().ScheduleGroups.Worktime, Def.GetUISprite(Assets.GetPrefab("ManualGenerator"), "ui", false).first);
		this.ConfigPaintButton(this.PaintButtonRecreation, Db.Get().ScheduleGroups.Recreation, Def.GetUISprite(Assets.GetPrefab("WaterCooler"), "ui", false).first);
		this.ConfigPaintButton(this.PaintButtonSleep, Db.Get().ScheduleGroups.Sleep, Def.GetUISprite(Assets.GetPrefab("Bed"), "ui", false).first);
		this.RefreshPaintButtons();
		this.RefreshTimeOfDayPositioner();
	}

	// Token: 0x0600A7AC RID: 42924 RVA: 0x0010CC3D File Offset: 0x0010AE3D
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		if (this.schedule != null)
		{
			Schedule schedule = this.schedule;
			schedule.onChanged = (Action<Schedule>)Delegate.Remove(schedule.onChanged, new Action<Schedule>(this.OnScheduleChanged));
		}
	}

	// Token: 0x0600A7AD RID: 42925 RVA: 0x0010CC74 File Offset: 0x0010AE74
	private void DuplicateSchedule()
	{
		ScheduleManager.Instance.DuplicateSchedule(this.schedule);
	}

	// Token: 0x0600A7AE RID: 42926 RVA: 0x0010CC87 File Offset: 0x0010AE87
	private void DeleteSchedule()
	{
		ScheduleManager.Instance.DeleteSchedule(this.schedule);
	}

	// Token: 0x0600A7AF RID: 42927 RVA: 0x003F9A98 File Offset: 0x003F7C98
	public void RefreshTimeOfDayPositioner()
	{
		GameObject targetTimetable = this.timetableRows[this.schedule.ProgressTimetableIdx];
		this.timeOfDayPositioner.SetTargetTimetable(targetTimetable);
	}

	// Token: 0x0600A7B0 RID: 42928 RVA: 0x003F9AC8 File Offset: 0x003F7CC8
	private void DuplicateTimetableRow(int sourceTimetableIdx)
	{
		List<ScheduleBlock> range = this.schedule.GetBlocks().GetRange(sourceTimetableIdx * 24, 24);
		List<ScheduleBlock> list = new List<ScheduleBlock>();
		for (int i = 0; i < range.Count; i++)
		{
			list.Add(new ScheduleBlock(range[i].name, range[i].GroupId));
		}
		int num = sourceTimetableIdx + 1;
		this.schedule.InsertTimetable(num, list);
		this.AddTimetableRow(num * 24);
	}

	// Token: 0x0600A7B1 RID: 42929 RVA: 0x003F9B44 File Offset: 0x003F7D44
	private void AddTimetableRow(int startingBlockIdx)
	{
		GameObject row = Util.KInstantiateUI(this.timetableRowPrefab, this.timetableRowContainer, true);
		int num = startingBlockIdx / 24;
		this.timetableRows.Insert(num, row);
		row.transform.SetSiblingIndex(num);
		HierarchyReferences component = row.GetComponent<HierarchyReferences>();
		List<ScheduleBlockButton> list = new List<ScheduleBlockButton>();
		for (int i = startingBlockIdx; i < startingBlockIdx + 24; i++)
		{
			GameObject gameObject = component.GetReference<RectTransform>("BlockContainer").gameObject;
			ScheduleBlockButton scheduleBlockButton = Util.KInstantiateUI<ScheduleBlockButton>(this.blockButtonPrefab.gameObject, gameObject, true);
			scheduleBlockButton.Setup(i - startingBlockIdx);
			scheduleBlockButton.SetBlockTypes(this.schedule.GetBlock(i).allowed_types);
			list.Add(scheduleBlockButton);
		}
		this.blockButtonsByTimetableRow.Add(row, list);
		component.GetReference<ScheduleBlockPainter>("BlockPainter").SetEntry(this);
		component.GetReference<KButton>("DuplicateButton").onClick += delegate()
		{
			this.DuplicateTimetableRow(this.timetableRows.IndexOf(row));
		};
		component.GetReference<KButton>("DeleteButton").onClick += delegate()
		{
			this.RemoveTimetableRow(row);
		};
		component.GetReference<KButton>("RotateLeftButton").onClick += delegate()
		{
			this.schedule.RotateBlocks(true, this.timetableRows.IndexOf(row));
		};
		component.GetReference<KButton>("RotateRightButton").onClick += delegate()
		{
			this.schedule.RotateBlocks(false, this.timetableRows.IndexOf(row));
		};
		KButton rotateUpButton = component.GetReference<KButton>("ShiftUpButton");
		rotateUpButton.onClick += delegate()
		{
			int timetableToShiftIdx = this.timetableRows.IndexOf(row);
			this.schedule.ShiftTimetable(true, timetableToShiftIdx);
			if (rotateUpButton.soundPlayer.button_widget_sound_events[0].OverrideAssetName == "ScheduleMenu_Shift_up")
			{
				rotateUpButton.soundPlayer.button_widget_sound_events[0].OverrideAssetName = "ScheduleMenu_Shift_up_reset";
				return;
			}
			rotateUpButton.soundPlayer.button_widget_sound_events[0].OverrideAssetName = "ScheduleMenu_Shift_up";
		};
		KButton rotateDownButton = component.GetReference<KButton>("ShiftDownButton");
		rotateDownButton.onClick += delegate()
		{
			int timetableToShiftIdx = this.timetableRows.IndexOf(row);
			this.schedule.ShiftTimetable(false, timetableToShiftIdx);
			if (rotateDownButton.soundPlayer.button_widget_sound_events[0].OverrideAssetName == "ScheduleMenu_Shift_down")
			{
				rotateDownButton.soundPlayer.button_widget_sound_events[0].OverrideAssetName = "ScheduleMenu_Shift_down_reset";
				return;
			}
			rotateDownButton.soundPlayer.button_widget_sound_events[0].OverrideAssetName = "ScheduleMenu_Shift_down";
		};
	}

	// Token: 0x0600A7B2 RID: 42930 RVA: 0x003F9CFC File Offset: 0x003F7EFC
	private void RemoveTimetableRow(GameObject row)
	{
		if (this.timetableRows.Count == 1)
		{
			return;
		}
		this.timeOfDayPositioner.SetTargetTimetable(null);
		int timetableToRemoveIdx = this.timetableRows.IndexOf(row);
		this.timetableRows.Remove(row);
		this.blockButtonsByTimetableRow.Remove(row);
		UnityEngine.Object.Destroy(row);
		this.schedule.RemoveTimetable(timetableToRemoveIdx);
	}

	// Token: 0x0600A7B3 RID: 42931 RVA: 0x0010CC99 File Offset: 0x0010AE99
	public GameObject GetNameInputField()
	{
		return this.title.inputField.gameObject;
	}

	// Token: 0x0600A7B4 RID: 42932 RVA: 0x003F9D60 File Offset: 0x003F7F60
	private void RebuildMinionWidgets()
	{
		if (!this.MinionWidgetsNeedRebuild())
		{
			return;
		}
		foreach (ScheduleMinionWidget original in this.minionWidgets)
		{
			Util.KDestroyGameObject(original);
		}
		this.minionWidgets.Clear();
		foreach (Ref<Schedulable> @ref in this.schedule.GetAssigned())
		{
			ScheduleMinionWidget scheduleMinionWidget = Util.KInstantiateUI<ScheduleMinionWidget>(this.minionWidgetPrefab.gameObject, this.minionWidgetContainer, true);
			scheduleMinionWidget.Setup(@ref.Get());
			this.minionWidgets.Add(scheduleMinionWidget);
		}
		if (Components.LiveMinionIdentities.Count > this.schedule.GetAssigned().Count)
		{
			this.blankMinionWidget.transform.SetAsLastSibling();
			this.blankMinionWidget.gameObject.SetActive(true);
			return;
		}
		this.blankMinionWidget.gameObject.SetActive(false);
	}

	// Token: 0x0600A7B5 RID: 42933 RVA: 0x003F9E84 File Offset: 0x003F8084
	private bool MinionWidgetsNeedRebuild()
	{
		List<Ref<Schedulable>> assigned = this.schedule.GetAssigned();
		if (assigned.Count != this.minionWidgets.Count)
		{
			return true;
		}
		if (assigned.Count != Components.LiveMinionIdentities.Count != this.blankMinionWidget.gameObject.activeSelf)
		{
			return true;
		}
		for (int i = 0; i < assigned.Count; i++)
		{
			if (assigned[i].Get() != this.minionWidgets[i].schedulable)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600A7B6 RID: 42934 RVA: 0x003F9F14 File Offset: 0x003F8114
	public void RefreshWidgetWorldData()
	{
		foreach (ScheduleMinionWidget scheduleMinionWidget in this.minionWidgets)
		{
			if (!scheduleMinionWidget.IsNullOrDestroyed())
			{
				scheduleMinionWidget.RefreshWidgetWorldData();
			}
		}
	}

	// Token: 0x0600A7B7 RID: 42935 RVA: 0x0010CCAB File Offset: 0x0010AEAB
	private void OnNameChanged(string newName)
	{
		this.schedule.name = newName;
		base.gameObject.name = "Schedule_" + this.schedule.name;
	}

	// Token: 0x0600A7B8 RID: 42936 RVA: 0x0010CCD9 File Offset: 0x0010AED9
	private void OnAlarmClicked()
	{
		this.schedule.alarmActivated = !this.schedule.alarmActivated;
		this.RefreshAlarmButton();
	}

	// Token: 0x0600A7B9 RID: 42937 RVA: 0x003F9F70 File Offset: 0x003F8170
	private void RefreshAlarmButton()
	{
		this.alarmButton.ChangeState(this.schedule.alarmActivated ? 1 : 0);
		ToolTip component = this.alarmButton.GetComponent<ToolTip>();
		component.SetSimpleTooltip(this.schedule.alarmActivated ? UI.SCHEDULESCREEN.ALARM_BUTTON_ON_TOOLTIP : UI.SCHEDULESCREEN.ALARM_BUTTON_OFF_TOOLTIP);
		ToolTipScreen.Instance.MarkTooltipDirty(component);
		this.alarmField.text = (this.schedule.alarmActivated ? UI.SCHEDULESCREEN.ALARM_TITLE_ENABLED : UI.SCHEDULESCREEN.ALARM_TITLE_DISABLED);
	}

	// Token: 0x0600A7BA RID: 42938 RVA: 0x0010CCFA File Offset: 0x0010AEFA
	private void OnResetClicked()
	{
		this.schedule.SetBlocksToGroupDefaults(Db.Get().ScheduleGroups.allGroups);
	}

	// Token: 0x0600A7BB RID: 42939 RVA: 0x0010CC87 File Offset: 0x0010AE87
	private void OnDeleteClicked()
	{
		ScheduleManager.Instance.DeleteSchedule(this.schedule);
	}

	// Token: 0x0600A7BC RID: 42940 RVA: 0x003FA000 File Offset: 0x003F8200
	private void OnScheduleChanged(Schedule changedSchedule)
	{
		foreach (KeyValuePair<GameObject, List<ScheduleBlockButton>> keyValuePair in this.blockButtonsByTimetableRow)
		{
			GameObject key = keyValuePair.Key;
			int num = this.timetableRows.IndexOf(key);
			List<ScheduleBlockButton> value = keyValuePair.Value;
			for (int i = 0; i < value.Count; i++)
			{
				int idx = num * 24 + i;
				value[i].SetBlockTypes(changedSchedule.GetBlock(idx).allowed_types);
			}
		}
		this.RefreshStatus();
		this.RebuildMinionWidgets();
	}

	// Token: 0x0600A7BD RID: 42941 RVA: 0x003FA0B0 File Offset: 0x003F82B0
	private void RefreshStatus()
	{
		this.blockTypeCounts.Clear();
		foreach (ScheduleBlockType scheduleBlockType in Db.Get().ScheduleBlockTypes.resources)
		{
			this.blockTypeCounts[scheduleBlockType.Id] = 0;
		}
		foreach (ScheduleBlock scheduleBlock in this.schedule.GetBlocks())
		{
			foreach (ScheduleBlockType scheduleBlockType2 in scheduleBlock.allowed_types)
			{
				Dictionary<string, int> dictionary = this.blockTypeCounts;
				string id = scheduleBlockType2.Id;
				int num = dictionary[id];
				dictionary[id] = num + 1;
			}
		}
		if (this.noteEntryRight == null)
		{
			return;
		}
		int num2 = 0;
		ToolTip component = this.noteEntryRight.GetComponent<ToolTip>();
		component.ClearMultiStringTooltip();
		foreach (KeyValuePair<string, int> keyValuePair in this.blockTypeCounts)
		{
			if (keyValuePair.Value == 0)
			{
				num2++;
				component.AddMultiStringTooltip(string.Format(UI.SCHEDULEGROUPS.NOTIME, Db.Get().ScheduleBlockTypes.Get(keyValuePair.Key).Name), null);
			}
		}
		if (num2 > 0)
		{
			this.noteEntryRight.text = string.Format(UI.SCHEDULEGROUPS.MISSINGBLOCKS, num2);
			return;
		}
		this.noteEntryRight.text = "";
	}

	// Token: 0x0600A7BE RID: 42942 RVA: 0x003FA294 File Offset: 0x003F8494
	private void ConfigPaintButton(GameObject button, ScheduleGroup group, Sprite iconSprite)
	{
		string groupID = group.Id;
		button.GetComponent<MultiToggle>().onClick = delegate()
		{
			ScheduleScreen.Instance.SelectedPaint = groupID;
			ScheduleScreen.Instance.RefreshAllPaintButtons();
		};
		this.paintButtons.Add(group.Id, button);
		HierarchyReferences component = button.GetComponent<HierarchyReferences>();
		component.GetReference<Image>("Icon").sprite = iconSprite;
		component.GetReference<LocText>("Label").text = group.Name;
	}

	// Token: 0x0600A7BF RID: 42943 RVA: 0x003FA308 File Offset: 0x003F8508
	public void RefreshPaintButtons()
	{
		foreach (KeyValuePair<string, GameObject> keyValuePair in this.paintButtons)
		{
			keyValuePair.Value.GetComponent<MultiToggle>().ChangeState((keyValuePair.Key == ScheduleScreen.Instance.SelectedPaint) ? 1 : 0);
		}
	}

	// Token: 0x0600A7C0 RID: 42944 RVA: 0x003FA384 File Offset: 0x003F8584
	public bool PaintBlock(ScheduleBlockButton blockButton)
	{
		foreach (KeyValuePair<GameObject, List<ScheduleBlockButton>> keyValuePair in this.blockButtonsByTimetableRow)
		{
			GameObject key = keyValuePair.Key;
			int i = 0;
			while (i < keyValuePair.Value.Count)
			{
				if (keyValuePair.Value[i] == blockButton)
				{
					int idx = this.timetableRows.IndexOf(key) * 24 + i;
					ScheduleGroup scheduleGroup = Db.Get().ScheduleGroups.Get(ScheduleScreen.Instance.SelectedPaint);
					if (this.schedule.GetBlock(idx).GroupId != scheduleGroup.Id)
					{
						this.schedule.SetBlockGroup(idx, scheduleGroup);
						return true;
					}
					return false;
				}
				else
				{
					i++;
				}
			}
		}
		return false;
	}

	// Token: 0x040083C6 RID: 33734
	[SerializeField]
	private ScheduleBlockButton blockButtonPrefab;

	// Token: 0x040083C7 RID: 33735
	[SerializeField]
	private ScheduleMinionWidget minionWidgetPrefab;

	// Token: 0x040083C8 RID: 33736
	[SerializeField]
	private GameObject minionWidgetContainer;

	// Token: 0x040083C9 RID: 33737
	private ScheduleMinionWidget blankMinionWidget;

	// Token: 0x040083CA RID: 33738
	[SerializeField]
	private KButton duplicateScheduleButton;

	// Token: 0x040083CB RID: 33739
	[SerializeField]
	private KButton deleteScheduleButton;

	// Token: 0x040083CC RID: 33740
	[SerializeField]
	private EditableTitleBar title;

	// Token: 0x040083CD RID: 33741
	[SerializeField]
	private LocText alarmField;

	// Token: 0x040083CE RID: 33742
	[SerializeField]
	private KButton optionsButton;

	// Token: 0x040083CF RID: 33743
	[SerializeField]
	private LocText noteEntryLeft;

	// Token: 0x040083D0 RID: 33744
	[SerializeField]
	private LocText noteEntryRight;

	// Token: 0x040083D1 RID: 33745
	[SerializeField]
	private MultiToggle alarmButton;

	// Token: 0x040083D2 RID: 33746
	private List<GameObject> timetableRows;

	// Token: 0x040083D3 RID: 33747
	private Dictionary<GameObject, List<ScheduleBlockButton>> blockButtonsByTimetableRow;

	// Token: 0x040083D4 RID: 33748
	private List<ScheduleMinionWidget> minionWidgets;

	// Token: 0x040083D5 RID: 33749
	[SerializeField]
	private GameObject timetableRowPrefab;

	// Token: 0x040083D6 RID: 33750
	[SerializeField]
	private GameObject timetableRowContainer;

	// Token: 0x040083D7 RID: 33751
	private Dictionary<string, GameObject> paintButtons = new Dictionary<string, GameObject>();

	// Token: 0x040083D8 RID: 33752
	[SerializeField]
	private GameObject PaintButtonBathtime;

	// Token: 0x040083D9 RID: 33753
	[SerializeField]
	private GameObject PaintButtonWorktime;

	// Token: 0x040083DA RID: 33754
	[SerializeField]
	private GameObject PaintButtonRecreation;

	// Token: 0x040083DB RID: 33755
	[SerializeField]
	private GameObject PaintButtonSleep;

	// Token: 0x040083DC RID: 33756
	[SerializeField]
	private TimeOfDayPositioner timeOfDayPositioner;

	// Token: 0x040083DE RID: 33758
	private Dictionary<string, int> blockTypeCounts = new Dictionary<string, int>();
}
