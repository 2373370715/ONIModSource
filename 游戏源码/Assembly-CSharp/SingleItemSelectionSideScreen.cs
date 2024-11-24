using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001FD0 RID: 8144
public class SingleItemSelectionSideScreen : SingleItemSelectionSideScreenBase
{
	// Token: 0x0600AC72 RID: 44146 RVA: 0x001101EE File Offset: 0x0010E3EE
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetSMI<StorageTile.Instance>() != null && target.GetComponent<TreeFilterable>() != null;
	}

	// Token: 0x0600AC73 RID: 44147 RVA: 0x00110206 File Offset: 0x0010E406
	private Tag GetTargetCurrentSelectedTag()
	{
		if (this.CurrentTarget != null)
		{
			return this.CurrentTarget.TargetTag;
		}
		return this.INVALID_OPTION_TAG;
	}

	// Token: 0x0600AC74 RID: 44148 RVA: 0x0040E744 File Offset: 0x0040C944
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		this.CurrentTarget = target.GetSMI<StorageTile.Instance>();
		if (this.CurrentTarget != null)
		{
			Dictionary<Tag, HashSet<Tag>> dictionary = new Dictionary<Tag, HashSet<Tag>>();
			foreach (Tag tag in new HashSet<Tag>(this.CurrentTarget.GetComponent<Storage>().storageFilters))
			{
				HashSet<Tag> discoveredResourcesFromTag = DiscoveredResources.Instance.GetDiscoveredResourcesFromTag(tag);
				if (discoveredResourcesFromTag != null && discoveredResourcesFromTag.Count > 0)
				{
					dictionary.Add(tag, discoveredResourcesFromTag);
				}
			}
			this.SetData(dictionary);
			SingleItemSelectionSideScreenBase.Category category = null;
			if (!this.categories.TryGetValue(this.INVALID_OPTION_TAG, out category))
			{
				category = base.GetCategoryWithItem(this.INVALID_OPTION_TAG, false);
			}
			if (category != null)
			{
				category.SetProihibedState(true);
			}
			this.CreateNoneOption();
			Tag targetCurrentSelectedTag = this.GetTargetCurrentSelectedTag();
			if (targetCurrentSelectedTag != this.INVALID_OPTION_TAG)
			{
				this.SetSelectedItem(targetCurrentSelectedTag);
				base.GetCategoryWithItem(targetCurrentSelectedTag, false).SetUnfoldedState(SingleItemSelectionSideScreenBase.Category.UnfoldedStates.Unfolded);
			}
			else
			{
				this.SetSelectedItem(this.noneOptionRow);
			}
			this.selectedItemLabel.SetItem(targetCurrentSelectedTag);
		}
	}

	// Token: 0x0600AC75 RID: 44149 RVA: 0x00110222 File Offset: 0x0010E422
	private void CreateNoneOption()
	{
		if (this.noneOptionRow == null)
		{
			this.noneOptionRow = this.GetOrCreateItemRow(this.INVALID_OPTION_TAG);
		}
		this.noneOptionRow.transform.SetAsFirstSibling();
	}

	// Token: 0x0600AC76 RID: 44150 RVA: 0x0040E86C File Offset: 0x0040CA6C
	public override void ItemRowClicked(SingleItemSelectionRow rowClicked)
	{
		base.ItemRowClicked(rowClicked);
		this.selectedItemLabel.SetItem(rowClicked.tag);
		Tag targetCurrentSelectedTag = this.GetTargetCurrentSelectedTag();
		if (this.CurrentTarget != null && targetCurrentSelectedTag != rowClicked.tag)
		{
			this.CurrentTarget.SetTargetItem(rowClicked.tag);
		}
	}

	// Token: 0x0400877A RID: 34682
	[SerializeField]
	private SingleItemSelectionSideScreen_SelectedItemSection selectedItemLabel;

	// Token: 0x0400877B RID: 34683
	private StorageTile.Instance CurrentTarget;

	// Token: 0x0400877C RID: 34684
	private SingleItemSelectionRow noneOptionRow;

	// Token: 0x0400877D RID: 34685
	private Tag INVALID_OPTION_TAG = GameTags.Void;
}
