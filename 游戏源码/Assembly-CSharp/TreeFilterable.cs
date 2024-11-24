using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200101D RID: 4125
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/TreeFilterable")]
public class TreeFilterable : KMonoBehaviour, ISaveLoadable
{
	// Token: 0x170004DA RID: 1242
	// (get) Token: 0x06005422 RID: 21538 RVA: 0x000D6D43 File Offset: 0x000D4F43
	public HashSet<Tag> AcceptedTags
	{
		get
		{
			return this.acceptedTagSet;
		}
	}

	// Token: 0x06005423 RID: 21539 RVA: 0x00279B50 File Offset: 0x00277D50
	[OnDeserialized]
	[Obsolete]
	private void OnDeserialized()
	{
		if (SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 20))
		{
			this.filterByStorageCategoriesOnSpawn = false;
		}
		if (SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 29))
		{
			this.acceptedTagSet.UnionWith(this.acceptedTags);
			this.acceptedTags = null;
		}
	}

	// Token: 0x06005424 RID: 21540 RVA: 0x00279BAC File Offset: 0x00277DAC
	private void OnDiscover(Tag category_tag, Tag tag)
	{
		if (this.preventAutoAddOnDiscovery)
		{
			return;
		}
		if (this.storage.storageFilters.Contains(category_tag))
		{
			bool flag = false;
			if (DiscoveredResources.Instance.GetDiscoveredResourcesFromTag(category_tag).Count <= 1)
			{
				foreach (Tag tag2 in this.storage.storageFilters)
				{
					if (!(tag2 == category_tag) && DiscoveredResources.Instance.IsDiscovered(tag2))
					{
						flag = true;
						foreach (Tag item in DiscoveredResources.Instance.GetDiscoveredResourcesFromTag(tag2))
						{
							if (!this.acceptedTagSet.Contains(item))
							{
								return;
							}
						}
					}
				}
				if (!flag)
				{
					return;
				}
			}
			foreach (Tag tag3 in DiscoveredResources.Instance.GetDiscoveredResourcesFromTag(category_tag))
			{
				if (!(tag3 == tag) && !this.acceptedTagSet.Contains(tag3))
				{
					return;
				}
			}
			this.AddTagToFilter(tag);
		}
	}

	// Token: 0x06005425 RID: 21541 RVA: 0x000D6D4B File Offset: 0x000D4F4B
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<TreeFilterable>(-905833192, TreeFilterable.OnCopySettingsDelegate);
	}

	// Token: 0x06005426 RID: 21542 RVA: 0x00279D08 File Offset: 0x00277F08
	protected override void OnSpawn()
	{
		DiscoveredResources.Instance.OnDiscover += this.OnDiscover;
		if (this.autoSelectStoredOnLoad && this.storage != null)
		{
			HashSet<Tag> hashSet = new HashSet<Tag>(this.acceptedTagSet);
			hashSet.UnionWith(this.storage.GetAllIDsInStorage());
			this.UpdateFilters(hashSet);
		}
		if (this.OnFilterChanged != null)
		{
			this.OnFilterChanged(this.acceptedTagSet);
		}
		this.RefreshTint();
		if (this.filterByStorageCategoriesOnSpawn)
		{
			this.RemoveIncorrectAcceptedTags();
		}
	}

	// Token: 0x06005427 RID: 21543 RVA: 0x00279D94 File Offset: 0x00277F94
	private void RemoveIncorrectAcceptedTags()
	{
		List<Tag> list = new List<Tag>();
		foreach (Tag item in this.acceptedTagSet)
		{
			bool flag = false;
			foreach (Tag tag in this.storage.storageFilters)
			{
				if (DiscoveredResources.Instance.GetDiscoveredResourcesFromTag(tag).Contains(item))
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				list.Add(item);
			}
		}
		foreach (Tag t in list)
		{
			this.RemoveTagFromFilter(t);
		}
	}

	// Token: 0x06005428 RID: 21544 RVA: 0x000D6D64 File Offset: 0x000D4F64
	protected override void OnCleanUp()
	{
		DiscoveredResources.Instance.OnDiscover -= this.OnDiscover;
		base.OnCleanUp();
	}

	// Token: 0x06005429 RID: 21545 RVA: 0x00279E8C File Offset: 0x0027808C
	private void OnCopySettings(object data)
	{
		if (this.copySettingsEnabled)
		{
			TreeFilterable component = ((GameObject)data).GetComponent<TreeFilterable>();
			if (component != null)
			{
				this.UpdateFilters(component.GetTags());
			}
		}
	}

	// Token: 0x0600542A RID: 21546 RVA: 0x000D6D43 File Offset: 0x000D4F43
	public HashSet<Tag> GetTags()
	{
		return this.acceptedTagSet;
	}

	// Token: 0x0600542B RID: 21547 RVA: 0x000D6D82 File Offset: 0x000D4F82
	public bool ContainsTag(Tag t)
	{
		return this.acceptedTagSet.Contains(t);
	}

	// Token: 0x0600542C RID: 21548 RVA: 0x00279EC4 File Offset: 0x002780C4
	public void AddTagToFilter(Tag t)
	{
		if (this.ContainsTag(t))
		{
			return;
		}
		this.UpdateFilters(new HashSet<Tag>(this.acceptedTagSet)
		{
			t
		});
	}

	// Token: 0x0600542D RID: 21549 RVA: 0x00279EF8 File Offset: 0x002780F8
	public void RemoveTagFromFilter(Tag t)
	{
		if (!this.ContainsTag(t))
		{
			return;
		}
		HashSet<Tag> hashSet = new HashSet<Tag>(this.acceptedTagSet);
		hashSet.Remove(t);
		this.UpdateFilters(hashSet);
	}

	// Token: 0x0600542E RID: 21550 RVA: 0x00279F2C File Offset: 0x0027812C
	public void UpdateFilters(HashSet<Tag> filters)
	{
		this.acceptedTagSet.Clear();
		this.acceptedTagSet.UnionWith(filters);
		if (this.OnFilterChanged != null)
		{
			this.OnFilterChanged(this.acceptedTagSet);
		}
		this.RefreshTint();
		if (!this.dropIncorrectOnFilterChange || this.storage == null || this.storage.items == null)
		{
			return;
		}
		if (!this.filterAllStoragesOnBuilding)
		{
			this.DropFilteredItemsFromTargetStorage(this.storage);
			return;
		}
		foreach (Storage targetStorage in base.GetComponents<Storage>())
		{
			this.DropFilteredItemsFromTargetStorage(targetStorage);
		}
	}

	// Token: 0x0600542F RID: 21551 RVA: 0x00279FC8 File Offset: 0x002781C8
	private void DropFilteredItemsFromTargetStorage(Storage targetStorage)
	{
		for (int i = targetStorage.items.Count - 1; i >= 0; i--)
		{
			GameObject gameObject = targetStorage.items[i];
			if (!(gameObject == null))
			{
				KPrefabID component = gameObject.GetComponent<KPrefabID>();
				if (!this.acceptedTagSet.Contains(component.PrefabTag))
				{
					targetStorage.Drop(gameObject, true);
				}
			}
		}
	}

	// Token: 0x06005430 RID: 21552 RVA: 0x0027A028 File Offset: 0x00278228
	public string GetTagsAsStatus(int maxDisplays = 6)
	{
		string text = "Tags:\n";
		List<Tag> list = new List<Tag>(this.storage.storageFilters);
		list.Intersect(this.acceptedTagSet);
		for (int i = 0; i < Mathf.Min(list.Count, maxDisplays); i++)
		{
			text += list[i].ProperName();
			if (i < Mathf.Min(list.Count, maxDisplays) - 1)
			{
				text += "\n";
			}
			if (i == maxDisplays - 1 && list.Count > maxDisplays)
			{
				text += "\n...";
				break;
			}
		}
		if (base.tag.Length == 0)
		{
			text = "No tags selected";
		}
		return text;
	}

	// Token: 0x06005431 RID: 21553 RVA: 0x0027A0D4 File Offset: 0x002782D4
	private void RefreshTint()
	{
		bool flag = this.acceptedTagSet != null && this.acceptedTagSet.Count != 0;
		base.GetComponent<KBatchedAnimController>().TintColour = (flag ? this.filterTint : this.noFilterTint);
		base.GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.NoStorageFilterSet, !flag, this);
	}

	// Token: 0x04003AC7 RID: 15047
	[MyCmpReq]
	private Storage storage;

	// Token: 0x04003AC8 RID: 15048
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04003AC9 RID: 15049
	public static readonly Color32 FILTER_TINT = Color.white;

	// Token: 0x04003ACA RID: 15050
	public static readonly Color32 NO_FILTER_TINT = new Color(0.5019608f, 0.5019608f, 0.5019608f, 1f);

	// Token: 0x04003ACB RID: 15051
	public Color32 filterTint = TreeFilterable.FILTER_TINT;

	// Token: 0x04003ACC RID: 15052
	public Color32 noFilterTint = TreeFilterable.NO_FILTER_TINT;

	// Token: 0x04003ACD RID: 15053
	[SerializeField]
	public bool dropIncorrectOnFilterChange = true;

	// Token: 0x04003ACE RID: 15054
	[SerializeField]
	public bool autoSelectStoredOnLoad = true;

	// Token: 0x04003ACF RID: 15055
	public bool showUserMenu = true;

	// Token: 0x04003AD0 RID: 15056
	public bool copySettingsEnabled = true;

	// Token: 0x04003AD1 RID: 15057
	public bool preventAutoAddOnDiscovery;

	// Token: 0x04003AD2 RID: 15058
	public string allResourceFilterLabelString = UI.UISIDESCREENS.TREEFILTERABLESIDESCREEN.ALLBUTTON;

	// Token: 0x04003AD3 RID: 15059
	public bool filterAllStoragesOnBuilding;

	// Token: 0x04003AD4 RID: 15060
	public TreeFilterable.UISideScreenHeight uiHeight = TreeFilterable.UISideScreenHeight.Tall;

	// Token: 0x04003AD5 RID: 15061
	public bool filterByStorageCategoriesOnSpawn = true;

	// Token: 0x04003AD6 RID: 15062
	[SerializeField]
	[Serialize]
	[Obsolete("Deprecated, use acceptedTagSet")]
	private List<Tag> acceptedTags = new List<Tag>();

	// Token: 0x04003AD7 RID: 15063
	[SerializeField]
	[Serialize]
	private HashSet<Tag> acceptedTagSet = new HashSet<Tag>();

	// Token: 0x04003AD8 RID: 15064
	public Action<HashSet<Tag>> OnFilterChanged;

	// Token: 0x04003AD9 RID: 15065
	private static readonly EventSystem.IntraObjectHandler<TreeFilterable> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<TreeFilterable>(delegate(TreeFilterable component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x0200101E RID: 4126
	public enum UISideScreenHeight
	{
		// Token: 0x04003ADB RID: 15067
		Short,
		// Token: 0x04003ADC RID: 15068
		Tall
	}
}
