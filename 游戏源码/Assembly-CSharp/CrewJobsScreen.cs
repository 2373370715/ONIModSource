using System;
using System.Collections.Generic;
using System.Linq;
using Klei.AI;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001D91 RID: 7569
public class CrewJobsScreen : CrewListScreen<CrewJobsEntry>
{
	// Token: 0x06009E36 RID: 40502 RVA: 0x003CA84C File Offset: 0x003C8A4C
	protected override void OnActivate()
	{
		CrewJobsScreen.Instance = this;
		foreach (ChoreGroup item in Db.Get().ChoreGroups.resources)
		{
			this.choreGroups.Add(item);
		}
		base.OnActivate();
	}

	// Token: 0x06009E37 RID: 40503 RVA: 0x0010708D File Offset: 0x0010528D
	protected override void OnCmpEnable()
	{
		base.OnCmpEnable();
		base.RefreshCrewPortraitContent();
		this.SortByPreviousSelected();
	}

	// Token: 0x06009E38 RID: 40504 RVA: 0x001070A1 File Offset: 0x001052A1
	protected override void OnForcedCleanUp()
	{
		CrewJobsScreen.Instance = null;
		base.OnForcedCleanUp();
	}

	// Token: 0x06009E39 RID: 40505 RVA: 0x003CA8BC File Offset: 0x003C8ABC
	protected override void SpawnEntries()
	{
		base.SpawnEntries();
		foreach (MinionIdentity identity in Components.LiveMinionIdentities.Items)
		{
			CrewJobsEntry component = Util.KInstantiateUI(this.Prefab_CrewEntry, this.EntriesPanelTransform.gameObject, false).GetComponent<CrewJobsEntry>();
			component.Populate(identity);
			this.EntryObjects.Add(component);
		}
		this.SortEveryoneToggle.group = this.sortToggleGroup;
		ImageToggleState toggleImage = this.SortEveryoneToggle.GetComponentInChildren<ImageToggleState>(true);
		this.SortEveryoneToggle.onValueChanged.AddListener(delegate(bool value)
		{
			this.SortByName(!this.SortEveryoneToggle.isOn);
			this.lastSortToggle = this.SortEveryoneToggle;
			this.lastSortReversed = !this.SortEveryoneToggle.isOn;
			this.ResetSortToggles(this.SortEveryoneToggle);
			if (this.SortEveryoneToggle.isOn)
			{
				toggleImage.SetActive();
				return;
			}
			toggleImage.SetInactive();
		});
		this.SortByPreviousSelected();
		this.dirty = true;
	}

	// Token: 0x06009E3A RID: 40506 RVA: 0x003CA99C File Offset: 0x003C8B9C
	private void SortByPreviousSelected()
	{
		if (this.sortToggleGroup == null || this.lastSortToggle == null)
		{
			return;
		}
		int childCount = this.ColumnTitlesContainer.childCount;
		for (int i = 0; i < childCount; i++)
		{
			if (i < this.choreGroups.Count && this.ColumnTitlesContainer.GetChild(i).Find("Title").GetComponentInChildren<Toggle>() == this.lastSortToggle)
			{
				this.SortByEffectiveness(this.choreGroups[i], this.lastSortReversed, false);
				return;
			}
		}
		if (this.SortEveryoneToggle == this.lastSortToggle)
		{
			base.SortByName(this.lastSortReversed);
		}
	}

	// Token: 0x06009E3B RID: 40507 RVA: 0x003CAA50 File Offset: 0x003C8C50
	protected override void PositionColumnTitles()
	{
		base.PositionColumnTitles();
		int childCount = this.ColumnTitlesContainer.childCount;
		for (int i = 0; i < childCount; i++)
		{
			if (i < this.choreGroups.Count)
			{
				Toggle sortToggle = this.ColumnTitlesContainer.GetChild(i).Find("Title").GetComponentInChildren<Toggle>();
				this.ColumnTitlesContainer.GetChild(i).rectTransform().localScale = Vector3.one;
				ChoreGroup chore_group = this.choreGroups[i];
				ImageToggleState toggleImage = sortToggle.GetComponentInChildren<ImageToggleState>(true);
				sortToggle.group = this.sortToggleGroup;
				sortToggle.onValueChanged.AddListener(delegate(bool value)
				{
					bool playSound = false;
					if (this.lastSortToggle == sortToggle)
					{
						playSound = true;
					}
					this.SortByEffectiveness(chore_group, !sortToggle.isOn, playSound);
					this.lastSortToggle = sortToggle;
					this.lastSortReversed = !sortToggle.isOn;
					this.ResetSortToggles(sortToggle);
					if (sortToggle.isOn)
					{
						toggleImage.SetActive();
						return;
					}
					toggleImage.SetInactive();
				});
			}
			ToolTip JobTooltip = this.ColumnTitlesContainer.GetChild(i).GetComponent<ToolTip>();
			ToolTip jobTooltip = JobTooltip;
			jobTooltip.OnToolTip = (Func<string>)Delegate.Combine(jobTooltip.OnToolTip, new Func<string>(() => this.GetJobTooltip(JobTooltip.gameObject)));
			Button componentInChildren = this.ColumnTitlesContainer.GetChild(i).GetComponentInChildren<Button>();
			this.EveryoneToggles.Add(componentInChildren, CrewJobsScreen.everyoneToggleState.on);
		}
		for (int j = 0; j < this.choreGroups.Count; j++)
		{
			ChoreGroup chore_group = this.choreGroups[j];
			Button b = this.EveryoneToggles.Keys.ElementAt(j);
			this.EveryoneToggles.Keys.ElementAt(j).onClick.AddListener(delegate()
			{
				this.ToggleJobEveryone(b, chore_group);
			});
		}
		Button key = this.EveryoneToggles.ElementAt(this.EveryoneToggles.Count - 1).Key;
		key.transform.parent.Find("Title").gameObject.GetComponentInChildren<Toggle>().gameObject.SetActive(false);
		key.onClick.AddListener(delegate()
		{
			this.ToggleAllTasksEveryone();
		});
		this.EveryoneAllTaskToggle = new KeyValuePair<Button, CrewJobsScreen.everyoneToggleState>(key, this.EveryoneAllTaskToggle.Value);
	}

	// Token: 0x06009E3C RID: 40508 RVA: 0x003CAC9C File Offset: 0x003C8E9C
	private string GetJobTooltip(GameObject go)
	{
		ToolTip component = go.GetComponent<ToolTip>();
		component.ClearMultiStringTooltip();
		OverviewColumnIdentity component2 = go.GetComponent<OverviewColumnIdentity>();
		if (component2.columnID != "AllTasks")
		{
			ChoreGroup choreGroup = Db.Get().ChoreGroups.Get(component2.columnID);
			component.AddMultiStringTooltip(component2.Column_DisplayName, this.TextStyle_JobTooltip_Title);
			component.AddMultiStringTooltip(choreGroup.description, this.TextStyle_JobTooltip_Description);
			component.AddMultiStringTooltip("\n", this.TextStyle_JobTooltip_Description);
			component.AddMultiStringTooltip(UI.TOOLTIPS.JOBSSCREEN_ATTRIBUTES, this.TextStyle_JobTooltip_Description);
			component.AddMultiStringTooltip("•  " + choreGroup.attribute.Name, this.TextStyle_JobTooltip_RelevantAttributes);
		}
		return "";
	}

	// Token: 0x06009E3D RID: 40509 RVA: 0x003CAD5C File Offset: 0x003C8F5C
	private void ToggleAllTasksEveryone()
	{
		string name = "HUD_Click_Deselect";
		if (this.EveryoneAllTaskToggle.Value != CrewJobsScreen.everyoneToggleState.on)
		{
			name = "HUD_Click";
		}
		KMonoBehaviour.PlaySound(GlobalAssets.GetSound(name, false));
		for (int i = 0; i < this.choreGroups.Count; i++)
		{
			this.SetJobEveryone(this.EveryoneAllTaskToggle.Value != CrewJobsScreen.everyoneToggleState.on, this.choreGroups[i]);
		}
	}

	// Token: 0x06009E3E RID: 40510 RVA: 0x001070AF File Offset: 0x001052AF
	private void SetJobEveryone(Button button, ChoreGroup chore_group)
	{
		this.SetJobEveryone(this.EveryoneToggles[button] != CrewJobsScreen.everyoneToggleState.on, chore_group);
	}

	// Token: 0x06009E3F RID: 40511 RVA: 0x003CADC8 File Offset: 0x003C8FC8
	private void SetJobEveryone(bool state, ChoreGroup chore_group)
	{
		foreach (CrewJobsEntry crewJobsEntry in this.EntryObjects)
		{
			crewJobsEntry.consumer.SetPermittedByUser(chore_group, state);
		}
	}

	// Token: 0x06009E40 RID: 40512 RVA: 0x003CAE20 File Offset: 0x003C9020
	private void ToggleJobEveryone(Button button, ChoreGroup chore_group)
	{
		string name = "HUD_Click_Deselect";
		if (this.EveryoneToggles[button] != CrewJobsScreen.everyoneToggleState.on)
		{
			name = "HUD_Click";
		}
		KMonoBehaviour.PlaySound(GlobalAssets.GetSound(name, false));
		foreach (CrewJobsEntry crewJobsEntry in this.EntryObjects)
		{
			crewJobsEntry.consumer.SetPermittedByUser(chore_group, this.EveryoneToggles[button] != CrewJobsScreen.everyoneToggleState.on);
		}
	}

	// Token: 0x06009E41 RID: 40513 RVA: 0x003CAEB0 File Offset: 0x003C90B0
	private void SortByEffectiveness(ChoreGroup chore_group, bool reverse, bool playSound)
	{
		if (playSound)
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click", false));
		}
		List<CrewJobsEntry> list = new List<CrewJobsEntry>(this.EntryObjects);
		list.Sort(delegate(CrewJobsEntry a, CrewJobsEntry b)
		{
			float value = a.Identity.GetAttributes().GetValue(chore_group.attribute.Id);
			float value2 = b.Identity.GetAttributes().GetValue(chore_group.attribute.Id);
			return value.CompareTo(value2);
		});
		base.ReorderEntries(list, reverse);
	}

	// Token: 0x06009E42 RID: 40514 RVA: 0x003CAF04 File Offset: 0x003C9104
	private void ResetSortToggles(Toggle exceptToggle)
	{
		for (int i = 0; i < this.ColumnTitlesContainer.childCount; i++)
		{
			Toggle componentInChildren = this.ColumnTitlesContainer.GetChild(i).Find("Title").GetComponentInChildren<Toggle>();
			if (!(componentInChildren == null))
			{
				ImageToggleState componentInChildren2 = componentInChildren.GetComponentInChildren<ImageToggleState>(true);
				if (componentInChildren != exceptToggle)
				{
					componentInChildren2.SetDisabled();
				}
			}
		}
		ImageToggleState componentInChildren3 = this.SortEveryoneToggle.GetComponentInChildren<ImageToggleState>(true);
		if (this.SortEveryoneToggle != exceptToggle)
		{
			componentInChildren3.SetDisabled();
		}
	}

	// Token: 0x06009E43 RID: 40515 RVA: 0x003CAF84 File Offset: 0x003C9184
	private void Refresh()
	{
		if (this.dirty)
		{
			int childCount = this.ColumnTitlesContainer.childCount;
			bool flag = false;
			bool flag2 = false;
			for (int i = 0; i < childCount; i++)
			{
				bool flag3 = false;
				bool flag4 = false;
				if (this.choreGroups.Count - 1 >= i)
				{
					ChoreGroup chore_group = this.choreGroups[i];
					for (int j = 0; j < this.EntryObjects.Count; j++)
					{
						ChoreConsumer consumer = this.EntryObjects[j].GetComponent<CrewJobsEntry>().consumer;
						if (consumer.IsPermittedByTraits(chore_group))
						{
							if (consumer.IsPermittedByUser(chore_group))
							{
								flag3 = true;
								flag = true;
							}
							else
							{
								flag4 = true;
								flag2 = true;
							}
						}
					}
					if (flag3 && flag4)
					{
						this.EveryoneToggles[this.EveryoneToggles.ElementAt(i).Key] = CrewJobsScreen.everyoneToggleState.mixed;
					}
					else if (flag3)
					{
						this.EveryoneToggles[this.EveryoneToggles.ElementAt(i).Key] = CrewJobsScreen.everyoneToggleState.on;
					}
					else
					{
						this.EveryoneToggles[this.EveryoneToggles.ElementAt(i).Key] = CrewJobsScreen.everyoneToggleState.off;
					}
					Button componentInChildren = this.ColumnTitlesContainer.GetChild(i).GetComponentInChildren<Button>();
					ImageToggleState component = componentInChildren.GetComponentsInChildren<Image>(true)[1].GetComponent<ImageToggleState>();
					switch (this.EveryoneToggles[componentInChildren])
					{
					case CrewJobsScreen.everyoneToggleState.off:
						component.SetDisabled();
						break;
					case CrewJobsScreen.everyoneToggleState.mixed:
						component.SetInactive();
						break;
					case CrewJobsScreen.everyoneToggleState.on:
						component.SetActive();
						break;
					}
				}
			}
			if (flag && flag2)
			{
				this.EveryoneAllTaskToggle = new KeyValuePair<Button, CrewJobsScreen.everyoneToggleState>(this.EveryoneAllTaskToggle.Key, CrewJobsScreen.everyoneToggleState.mixed);
			}
			else if (flag)
			{
				this.EveryoneAllTaskToggle = new KeyValuePair<Button, CrewJobsScreen.everyoneToggleState>(this.EveryoneAllTaskToggle.Key, CrewJobsScreen.everyoneToggleState.on);
			}
			else if (flag2)
			{
				this.EveryoneAllTaskToggle = new KeyValuePair<Button, CrewJobsScreen.everyoneToggleState>(this.EveryoneAllTaskToggle.Key, CrewJobsScreen.everyoneToggleState.off);
			}
			ImageToggleState component2 = this.EveryoneAllTaskToggle.Key.GetComponentsInChildren<Image>(true)[1].GetComponent<ImageToggleState>();
			switch (this.EveryoneAllTaskToggle.Value)
			{
			case CrewJobsScreen.everyoneToggleState.off:
				component2.SetDisabled();
				break;
			case CrewJobsScreen.everyoneToggleState.mixed:
				component2.SetInactive();
				break;
			case CrewJobsScreen.everyoneToggleState.on:
				component2.SetActive();
				break;
			}
			this.screenWidth = this.EntriesPanelTransform.rectTransform().sizeDelta.x;
			this.ScrollRectTransform.GetComponent<LayoutElement>().minWidth = this.screenWidth;
			float num = 31f;
			base.GetComponent<LayoutElement>().minWidth = this.screenWidth + num;
			this.dirty = false;
		}
	}

	// Token: 0x06009E44 RID: 40516 RVA: 0x001070CA File Offset: 0x001052CA
	private void Update()
	{
		this.Refresh();
	}

	// Token: 0x06009E45 RID: 40517 RVA: 0x001070D2 File Offset: 0x001052D2
	public void Dirty(object data = null)
	{
		this.dirty = true;
	}

	// Token: 0x04007C05 RID: 31749
	public static CrewJobsScreen Instance;

	// Token: 0x04007C06 RID: 31750
	private Dictionary<Button, CrewJobsScreen.everyoneToggleState> EveryoneToggles = new Dictionary<Button, CrewJobsScreen.everyoneToggleState>();

	// Token: 0x04007C07 RID: 31751
	private KeyValuePair<Button, CrewJobsScreen.everyoneToggleState> EveryoneAllTaskToggle;

	// Token: 0x04007C08 RID: 31752
	public TextStyleSetting TextStyle_JobTooltip_Title;

	// Token: 0x04007C09 RID: 31753
	public TextStyleSetting TextStyle_JobTooltip_Description;

	// Token: 0x04007C0A RID: 31754
	public TextStyleSetting TextStyle_JobTooltip_RelevantAttributes;

	// Token: 0x04007C0B RID: 31755
	public Toggle SortEveryoneToggle;

	// Token: 0x04007C0C RID: 31756
	private List<ChoreGroup> choreGroups = new List<ChoreGroup>();

	// Token: 0x04007C0D RID: 31757
	private bool dirty;

	// Token: 0x04007C0E RID: 31758
	private float screenWidth;

	// Token: 0x02001D92 RID: 7570
	public enum everyoneToggleState
	{
		// Token: 0x04007C10 RID: 31760
		off,
		// Token: 0x04007C11 RID: 31761
		mixed,
		// Token: 0x04007C12 RID: 31762
		on
	}
}
