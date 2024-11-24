using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

public class FilterSideScreen : SingleItemSelectionSideScreenBase
{
	public HierarchyReferences categoryFoldoutPrefab;

	public RectTransform elementEntryContainer;

	public Image outputIcon;

	public Image everythingElseIcon;

	public LocText outputElementHeaderLabel;

	public LocText everythingElseHeaderLabel;

	public LocText selectElementHeaderLabel;

	public LocText currentSelectionLabel;

	private SingleItemSelectionRow voidRow;

	public bool isLogicFilter;

	private Filterable targetFilterable;

	public override bool IsValidForTarget(GameObject target)
	{
		bool flag = false;
		if ((!isLogicFilter) ? (target.GetComponent<ElementFilter>() != null || target.GetComponent<RocketConduitStorageAccess>() != null || target.GetComponent<DevPump>() != null) : (target.GetComponent<ConduitElementSensor>() != null || target.GetComponent<LogicElementSensor>() != null))
		{
			return target.GetComponent<Filterable>() != null;
		}
		return false;
	}

	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		targetFilterable = target.GetComponent<Filterable>();
		if (!(targetFilterable == null))
		{
			switch (targetFilterable.filterElementState)
			{
			case Filterable.ElementState.Solid:
				everythingElseHeaderLabel.text = UI.UISIDESCREENS.FILTERSIDESCREEN.UNFILTEREDELEMENTS.SOLID;
				break;
			case Filterable.ElementState.Gas:
				everythingElseHeaderLabel.text = UI.UISIDESCREENS.FILTERSIDESCREEN.UNFILTEREDELEMENTS.GAS;
				break;
			default:
				everythingElseHeaderLabel.text = UI.UISIDESCREENS.FILTERSIDESCREEN.UNFILTEREDELEMENTS.LIQUID;
				break;
			}
			Configure(targetFilterable);
			SetFilterTag(targetFilterable.SelectedTag);
		}
	}

	public override void ItemRowClicked(SingleItemSelectionRow rowClicked)
	{
		SetFilterTag(rowClicked.tag);
		base.ItemRowClicked(rowClicked);
	}

	private void Configure(Filterable filterable)
	{
		Dictionary<Tag, HashSet<Tag>> tagOptions = filterable.GetTagOptions();
		Tag tag = GameTags.Void;
		foreach (Tag key in tagOptions.Keys)
		{
			foreach (Tag item in tagOptions[key])
			{
				if (item == filterable.SelectedTag)
				{
					tag = key;
					break;
				}
			}
		}
		SetData(tagOptions);
		Category value = null;
		if (categories.TryGetValue(GameTags.Void, out value))
		{
			value.SetProihibedState(isPohibited: true);
		}
		if (tag != GameTags.Void)
		{
			categories[tag].SetUnfoldedState(Category.UnfoldedStates.Unfolded);
		}
		if (voidRow == null)
		{
			voidRow = GetOrCreateItemRow(GameTags.Void);
		}
		voidRow.transform.SetAsFirstSibling();
		if (filterable.SelectedTag != GameTags.Void)
		{
			SetSelectedItem(filterable.SelectedTag);
		}
		else
		{
			SetSelectedItem(voidRow);
		}
		RefreshUI();
	}

	private void SetFilterTag(Tag tag)
	{
		if (!(targetFilterable == null))
		{
			if (tag.IsValid)
			{
				targetFilterable.SelectedTag = tag;
			}
			RefreshUI();
		}
	}

	private void RefreshUI()
	{
		LocString locString = targetFilterable.filterElementState switch
		{
			Filterable.ElementState.Solid => UI.UISIDESCREENS.FILTERSIDESCREEN.FILTEREDELEMENT.SOLID, 
			Filterable.ElementState.Gas => UI.UISIDESCREENS.FILTERSIDESCREEN.FILTEREDELEMENT.GAS, 
			_ => UI.UISIDESCREENS.FILTERSIDESCREEN.FILTEREDELEMENT.LIQUID, 
		};
		currentSelectionLabel.text = string.Format(locString, UI.UISIDESCREENS.FILTERSIDESCREEN.NOELEMENTSELECTED);
		if (base.CurrentSelectedItem == null || base.CurrentSelectedItem.tag != targetFilterable.SelectedTag)
		{
			SetSelectedItem(targetFilterable.SelectedTag);
		}
		if (targetFilterable.SelectedTag != GameTags.Void)
		{
			currentSelectionLabel.text = string.Format(locString, targetFilterable.SelectedTag.ProperName());
		}
		else
		{
			currentSelectionLabel.text = UI.UISIDESCREENS.FILTERSIDESCREEN.NO_SELECTION;
		}
	}
}
