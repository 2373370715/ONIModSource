using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001F6A RID: 8042
public class FilterSideScreen : SingleItemSelectionSideScreenBase
{
	// Token: 0x0600A9B5 RID: 43445 RVA: 0x004023B4 File Offset: 0x004005B4
	public override bool IsValidForTarget(GameObject target)
	{
		bool flag;
		if (this.isLogicFilter)
		{
			flag = (target.GetComponent<ConduitElementSensor>() != null || target.GetComponent<LogicElementSensor>() != null);
		}
		else
		{
			flag = (target.GetComponent<ElementFilter>() != null || target.GetComponent<RocketConduitStorageAccess>() != null || target.GetComponent<DevPump>() != null);
		}
		return flag && target.GetComponent<Filterable>() != null;
	}

	// Token: 0x0600A9B6 RID: 43446 RVA: 0x00402428 File Offset: 0x00400628
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		this.targetFilterable = target.GetComponent<Filterable>();
		if (this.targetFilterable == null)
		{
			return;
		}
		switch (this.targetFilterable.filterElementState)
		{
		case Filterable.ElementState.Solid:
			this.everythingElseHeaderLabel.text = UI.UISIDESCREENS.FILTERSIDESCREEN.UNFILTEREDELEMENTS.SOLID;
			goto IL_87;
		case Filterable.ElementState.Gas:
			this.everythingElseHeaderLabel.text = UI.UISIDESCREENS.FILTERSIDESCREEN.UNFILTEREDELEMENTS.GAS;
			goto IL_87;
		}
		this.everythingElseHeaderLabel.text = UI.UISIDESCREENS.FILTERSIDESCREEN.UNFILTEREDELEMENTS.LIQUID;
		IL_87:
		this.Configure(this.targetFilterable);
		this.SetFilterTag(this.targetFilterable.SelectedTag);
	}

	// Token: 0x0600A9B7 RID: 43447 RVA: 0x0010E3CC File Offset: 0x0010C5CC
	public override void ItemRowClicked(SingleItemSelectionRow rowClicked)
	{
		this.SetFilterTag(rowClicked.tag);
		base.ItemRowClicked(rowClicked);
	}

	// Token: 0x0600A9B8 RID: 43448 RVA: 0x004024DC File Offset: 0x004006DC
	private void Configure(Filterable filterable)
	{
		Dictionary<Tag, HashSet<Tag>> tagOptions = filterable.GetTagOptions();
		Tag tag = GameTags.Void;
		foreach (Tag tag2 in tagOptions.Keys)
		{
			using (HashSet<Tag>.Enumerator enumerator2 = tagOptions[tag2].GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					if (enumerator2.Current == filterable.SelectedTag)
					{
						tag = tag2;
						break;
					}
				}
			}
		}
		this.SetData(tagOptions);
		SingleItemSelectionSideScreenBase.Category category = null;
		if (this.categories.TryGetValue(GameTags.Void, out category))
		{
			category.SetProihibedState(true);
		}
		if (tag != GameTags.Void)
		{
			this.categories[tag].SetUnfoldedState(SingleItemSelectionSideScreenBase.Category.UnfoldedStates.Unfolded);
		}
		if (this.voidRow == null)
		{
			this.voidRow = this.GetOrCreateItemRow(GameTags.Void);
		}
		this.voidRow.transform.SetAsFirstSibling();
		if (filterable.SelectedTag != GameTags.Void)
		{
			this.SetSelectedItem(filterable.SelectedTag);
		}
		else
		{
			this.SetSelectedItem(this.voidRow);
		}
		this.RefreshUI();
	}

	// Token: 0x0600A9B9 RID: 43449 RVA: 0x0010E3E1 File Offset: 0x0010C5E1
	private void SetFilterTag(Tag tag)
	{
		if (this.targetFilterable == null)
		{
			return;
		}
		if (tag.IsValid)
		{
			this.targetFilterable.SelectedTag = tag;
		}
		this.RefreshUI();
	}

	// Token: 0x0600A9BA RID: 43450 RVA: 0x0040262C File Offset: 0x0040082C
	private void RefreshUI()
	{
		LocString loc_string;
		switch (this.targetFilterable.filterElementState)
		{
		case Filterable.ElementState.Solid:
			loc_string = UI.UISIDESCREENS.FILTERSIDESCREEN.FILTEREDELEMENT.SOLID;
			goto IL_38;
		case Filterable.ElementState.Gas:
			loc_string = UI.UISIDESCREENS.FILTERSIDESCREEN.FILTEREDELEMENT.GAS;
			goto IL_38;
		}
		loc_string = UI.UISIDESCREENS.FILTERSIDESCREEN.FILTEREDELEMENT.LIQUID;
		IL_38:
		this.currentSelectionLabel.text = string.Format(loc_string, UI.UISIDESCREENS.FILTERSIDESCREEN.NOELEMENTSELECTED);
		if (base.CurrentSelectedItem == null || base.CurrentSelectedItem.tag != this.targetFilterable.SelectedTag)
		{
			this.SetSelectedItem(this.targetFilterable.SelectedTag);
		}
		if (this.targetFilterable.SelectedTag != GameTags.Void)
		{
			this.currentSelectionLabel.text = string.Format(loc_string, this.targetFilterable.SelectedTag.ProperName());
			return;
		}
		this.currentSelectionLabel.text = UI.UISIDESCREENS.FILTERSIDESCREEN.NO_SELECTION;
	}

	// Token: 0x04008572 RID: 34162
	public HierarchyReferences categoryFoldoutPrefab;

	// Token: 0x04008573 RID: 34163
	public RectTransform elementEntryContainer;

	// Token: 0x04008574 RID: 34164
	public Image outputIcon;

	// Token: 0x04008575 RID: 34165
	public Image everythingElseIcon;

	// Token: 0x04008576 RID: 34166
	public LocText outputElementHeaderLabel;

	// Token: 0x04008577 RID: 34167
	public LocText everythingElseHeaderLabel;

	// Token: 0x04008578 RID: 34168
	public LocText selectElementHeaderLabel;

	// Token: 0x04008579 RID: 34169
	public LocText currentSelectionLabel;

	// Token: 0x0400857A RID: 34170
	private SingleItemSelectionRow voidRow;

	// Token: 0x0400857B RID: 34171
	public bool isLogicFilter;

	// Token: 0x0400857C RID: 34172
	private Filterable targetFilterable;
}
