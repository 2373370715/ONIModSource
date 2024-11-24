using System;
using System.Collections.Generic;
using Klei.AI;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001D9F RID: 7583
public class CrewRationsScreen : CrewListScreen<CrewRationsEntry>
{
	// Token: 0x06009E8D RID: 40589 RVA: 0x00107424 File Offset: 0x00105624
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.closebutton.onClick += delegate()
		{
			ManagementMenu.Instance.CloseAll();
		};
	}

	// Token: 0x06009E8E RID: 40590 RVA: 0x00107456 File Offset: 0x00105656
	protected override void OnCmpEnable()
	{
		base.OnCmpEnable();
		base.RefreshCrewPortraitContent();
		this.SortByPreviousSelected();
	}

	// Token: 0x06009E8F RID: 40591 RVA: 0x003CC3D4 File Offset: 0x003CA5D4
	private void SortByPreviousSelected()
	{
		if (this.sortToggleGroup == null)
		{
			return;
		}
		if (this.lastSortToggle == null)
		{
			return;
		}
		for (int i = 0; i < this.ColumnTitlesContainer.childCount; i++)
		{
			OverviewColumnIdentity component = this.ColumnTitlesContainer.GetChild(i).GetComponent<OverviewColumnIdentity>();
			if (this.ColumnTitlesContainer.GetChild(i).GetComponent<Toggle>() == this.lastSortToggle)
			{
				if (component.columnID == "name")
				{
					base.SortByName(this.lastSortReversed);
				}
				if (component.columnID == "health")
				{
					this.SortByAmount("HitPoints", this.lastSortReversed);
				}
				if (component.columnID == "stress")
				{
					this.SortByAmount("Stress", this.lastSortReversed);
				}
				if (component.columnID == "calories")
				{
					this.SortByAmount("Calories", this.lastSortReversed);
				}
			}
		}
	}

	// Token: 0x06009E90 RID: 40592 RVA: 0x003CC4D8 File Offset: 0x003CA6D8
	protected override void PositionColumnTitles()
	{
		base.PositionColumnTitles();
		for (int i = 0; i < this.ColumnTitlesContainer.childCount; i++)
		{
			OverviewColumnIdentity component = this.ColumnTitlesContainer.GetChild(i).GetComponent<OverviewColumnIdentity>();
			if (component.Sortable)
			{
				Toggle toggle = this.ColumnTitlesContainer.GetChild(i).GetComponent<Toggle>();
				toggle.group = this.sortToggleGroup;
				ImageToggleState toggleImage = toggle.GetComponentInChildren<ImageToggleState>(true);
				if (component.columnID == "name")
				{
					toggle.onValueChanged.AddListener(delegate(bool value)
					{
						this.SortByName(!toggle.isOn);
						this.lastSortToggle = toggle;
						this.lastSortReversed = !toggle.isOn;
						this.ResetSortToggles(toggle);
						if (toggle.isOn)
						{
							toggleImage.SetActive();
							return;
						}
						toggleImage.SetInactive();
					});
				}
				if (component.columnID == "health")
				{
					toggle.onValueChanged.AddListener(delegate(bool value)
					{
						this.SortByAmount("HitPoints", !toggle.isOn);
						this.lastSortToggle = toggle;
						this.lastSortReversed = !toggle.isOn;
						this.ResetSortToggles(toggle);
						if (toggle.isOn)
						{
							toggleImage.SetActive();
							return;
						}
						toggleImage.SetInactive();
					});
				}
				if (component.columnID == "stress")
				{
					toggle.onValueChanged.AddListener(delegate(bool value)
					{
						this.SortByAmount("Stress", !toggle.isOn);
						this.lastSortToggle = toggle;
						this.lastSortReversed = !toggle.isOn;
						this.ResetSortToggles(toggle);
						if (toggle.isOn)
						{
							toggleImage.SetActive();
							return;
						}
						toggleImage.SetInactive();
					});
				}
				if (component.columnID == "calories")
				{
					toggle.onValueChanged.AddListener(delegate(bool value)
					{
						this.SortByAmount("Calories", !toggle.isOn);
						this.lastSortToggle = toggle;
						this.lastSortReversed = !toggle.isOn;
						this.ResetSortToggles(toggle);
						if (toggle.isOn)
						{
							toggleImage.SetActive();
							return;
						}
						toggleImage.SetInactive();
					});
				}
			}
		}
	}

	// Token: 0x06009E91 RID: 40593 RVA: 0x003CC624 File Offset: 0x003CA824
	protected override void SpawnEntries()
	{
		base.SpawnEntries();
		foreach (MinionIdentity identity in Components.LiveMinionIdentities.Items)
		{
			CrewRationsEntry component = Util.KInstantiateUI(this.Prefab_CrewEntry, this.EntriesPanelTransform.gameObject, false).GetComponent<CrewRationsEntry>();
			component.Populate(identity);
			this.EntryObjects.Add(component);
		}
		this.SortByPreviousSelected();
	}

	// Token: 0x06009E92 RID: 40594 RVA: 0x003CC6B0 File Offset: 0x003CA8B0
	public override void ScreenUpdate(bool topLevel)
	{
		base.ScreenUpdate(topLevel);
		foreach (CrewRationsEntry crewRationsEntry in this.EntryObjects)
		{
			crewRationsEntry.Refresh();
		}
	}

	// Token: 0x06009E93 RID: 40595 RVA: 0x003CC708 File Offset: 0x003CA908
	private void SortByAmount(string amount_id, bool reverse)
	{
		List<CrewRationsEntry> list = new List<CrewRationsEntry>(this.EntryObjects);
		list.Sort(delegate(CrewRationsEntry a, CrewRationsEntry b)
		{
			float value = a.Identity.GetAmounts().GetValue(amount_id);
			float value2 = b.Identity.GetAmounts().GetValue(amount_id);
			return value.CompareTo(value2);
		});
		base.ReorderEntries(list, reverse);
	}

	// Token: 0x06009E94 RID: 40596 RVA: 0x003CC748 File Offset: 0x003CA948
	private void ResetSortToggles(Toggle exceptToggle)
	{
		for (int i = 0; i < this.ColumnTitlesContainer.childCount; i++)
		{
			Toggle component = this.ColumnTitlesContainer.GetChild(i).GetComponent<Toggle>();
			ImageToggleState componentInChildren = component.GetComponentInChildren<ImageToggleState>(true);
			if (component != exceptToggle)
			{
				componentInChildren.SetDisabled();
			}
		}
	}

	// Token: 0x04007C52 RID: 31826
	[SerializeField]
	private KButton closebutton;
}
