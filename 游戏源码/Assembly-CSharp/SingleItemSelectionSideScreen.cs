using System.Collections.Generic;
using UnityEngine;

public class SingleItemSelectionSideScreen : SingleItemSelectionSideScreenBase
{
	[SerializeField]
	private SingleItemSelectionSideScreen_SelectedItemSection selectedItemLabel;

	private StorageTile.Instance CurrentTarget;

	private SingleItemSelectionRow noneOptionRow;

	private Tag INVALID_OPTION_TAG = GameTags.Void;

	public override bool IsValidForTarget(GameObject target)
	{
		if (target.GetSMI<StorageTile.Instance>() != null)
		{
			return target.GetComponent<TreeFilterable>() != null;
		}
		return false;
	}

	private Tag GetTargetCurrentSelectedTag()
	{
		if (CurrentTarget != null)
		{
			return CurrentTarget.TargetTag;
		}
		return INVALID_OPTION_TAG;
	}

	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		CurrentTarget = target.GetSMI<StorageTile.Instance>();
		if (CurrentTarget == null)
		{
			return;
		}
		Dictionary<Tag, HashSet<Tag>> dictionary = new Dictionary<Tag, HashSet<Tag>>();
		foreach (Tag item in new HashSet<Tag>(CurrentTarget.GetComponent<Storage>().storageFilters))
		{
			HashSet<Tag> discoveredResourcesFromTag = DiscoveredResources.Instance.GetDiscoveredResourcesFromTag(item);
			if (discoveredResourcesFromTag != null && discoveredResourcesFromTag.Count > 0)
			{
				dictionary.Add(item, discoveredResourcesFromTag);
			}
		}
		SetData(dictionary);
		Category value = null;
		if (!categories.TryGetValue(INVALID_OPTION_TAG, out value))
		{
			value = GetCategoryWithItem(INVALID_OPTION_TAG);
		}
		value?.SetProihibedState(isPohibited: true);
		CreateNoneOption();
		Tag targetCurrentSelectedTag = GetTargetCurrentSelectedTag();
		if (targetCurrentSelectedTag != INVALID_OPTION_TAG)
		{
			SetSelectedItem(targetCurrentSelectedTag);
			GetCategoryWithItem(targetCurrentSelectedTag).SetUnfoldedState(Category.UnfoldedStates.Unfolded);
		}
		else
		{
			SetSelectedItem(noneOptionRow);
		}
		selectedItemLabel.SetItem(targetCurrentSelectedTag);
	}

	private void CreateNoneOption()
	{
		if (noneOptionRow == null)
		{
			noneOptionRow = GetOrCreateItemRow(INVALID_OPTION_TAG);
		}
		noneOptionRow.transform.SetAsFirstSibling();
	}

	public override void ItemRowClicked(SingleItemSelectionRow rowClicked)
	{
		base.ItemRowClicked(rowClicked);
		selectedItemLabel.SetItem(rowClicked.tag);
		Tag targetCurrentSelectedTag = GetTargetCurrentSelectedTag();
		if (CurrentTarget != null && targetCurrentSelectedTag != rowClicked.tag)
		{
			CurrentTarget.SetTargetItem(rowClicked.tag);
		}
	}
}
