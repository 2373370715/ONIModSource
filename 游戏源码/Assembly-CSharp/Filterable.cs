using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02000D69 RID: 3433
[AddComponentMenu("KMonoBehaviour/scripts/Filterable")]
public class Filterable : KMonoBehaviour
{
	// Token: 0x14000012 RID: 18
	// (add) Token: 0x06004338 RID: 17208 RVA: 0x002441E0 File Offset: 0x002423E0
	// (remove) Token: 0x06004339 RID: 17209 RVA: 0x00244218 File Offset: 0x00242418
	public event Action<Tag> onFilterChanged;

	// Token: 0x17000350 RID: 848
	// (get) Token: 0x0600433A RID: 17210 RVA: 0x000CB705 File Offset: 0x000C9905
	// (set) Token: 0x0600433B RID: 17211 RVA: 0x000CB70D File Offset: 0x000C990D
	public Tag SelectedTag
	{
		get
		{
			return this.selectedTag;
		}
		set
		{
			this.selectedTag = value;
			this.OnFilterChanged();
		}
	}

	// Token: 0x0600433C RID: 17212 RVA: 0x00244250 File Offset: 0x00242450
	public Dictionary<Tag, HashSet<Tag>> GetTagOptions()
	{
		Dictionary<Tag, HashSet<Tag>> dictionary = new Dictionary<Tag, HashSet<Tag>>();
		if (this.filterElementState == Filterable.ElementState.Solid)
		{
			dictionary = DiscoveredResources.Instance.GetDiscoveredResourcesFromTagSet(Filterable.filterableCategories);
		}
		else
		{
			foreach (Element element in ElementLoader.elements)
			{
				if (!element.disabled && ((element.IsGas && this.filterElementState == Filterable.ElementState.Gas) || (element.IsLiquid && this.filterElementState == Filterable.ElementState.Liquid)))
				{
					Tag materialCategoryTag = element.GetMaterialCategoryTag();
					if (!dictionary.ContainsKey(materialCategoryTag))
					{
						dictionary[materialCategoryTag] = new HashSet<Tag>();
					}
					Tag item = GameTagExtensions.Create(element.id);
					dictionary[materialCategoryTag].Add(item);
				}
			}
		}
		dictionary.Add(GameTags.Void, new HashSet<Tag>
		{
			GameTags.Void
		});
		return dictionary;
	}

	// Token: 0x0600433D RID: 17213 RVA: 0x000CB71C File Offset: 0x000C991C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<Filterable>(-905833192, Filterable.OnCopySettingsDelegate);
	}

	// Token: 0x0600433E RID: 17214 RVA: 0x00244340 File Offset: 0x00242540
	private void OnCopySettings(object data)
	{
		Filterable component = ((GameObject)data).GetComponent<Filterable>();
		if (component != null)
		{
			this.SelectedTag = component.SelectedTag;
		}
	}

	// Token: 0x0600433F RID: 17215 RVA: 0x000CB735 File Offset: 0x000C9935
	protected override void OnSpawn()
	{
		this.OnFilterChanged();
	}

	// Token: 0x06004340 RID: 17216 RVA: 0x00244370 File Offset: 0x00242570
	private void OnFilterChanged()
	{
		if (this.onFilterChanged != null)
		{
			this.onFilterChanged(this.selectedTag);
		}
		Operational component = base.GetComponent<Operational>();
		if (component != null)
		{
			component.SetFlag(Filterable.filterSelected, this.selectedTag.IsValid);
		}
	}

	// Token: 0x04002E05 RID: 11781
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04002E06 RID: 11782
	[Serialize]
	public Filterable.ElementState filterElementState;

	// Token: 0x04002E07 RID: 11783
	[Serialize]
	private Tag selectedTag = GameTags.Void;

	// Token: 0x04002E09 RID: 11785
	private static TagSet filterableCategories = new TagSet(new TagSet[]
	{
		GameTags.CalorieCategories,
		GameTags.UnitCategories,
		GameTags.MaterialCategories,
		GameTags.MaterialBuildingElements
	});

	// Token: 0x04002E0A RID: 11786
	private static readonly Operational.Flag filterSelected = new Operational.Flag("filterSelected", Operational.Flag.Type.Requirement);

	// Token: 0x04002E0B RID: 11787
	private static readonly EventSystem.IntraObjectHandler<Filterable> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<Filterable>(delegate(Filterable component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x02000D6A RID: 3434
	public enum ElementState
	{
		// Token: 0x04002E0D RID: 11789
		None,
		// Token: 0x04002E0E RID: 11790
		Solid,
		// Token: 0x04002E0F RID: 11791
		Liquid,
		// Token: 0x04002E10 RID: 11792
		Gas
	}
}
