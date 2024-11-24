using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using KSerialization;
using STRINGS;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/TreeFilterable")]
public class TreeFilterable : KMonoBehaviour, ISaveLoadable
{
	public enum UISideScreenHeight
	{
		Short,
		Tall
	}

	[MyCmpReq]
	private Storage storage;

	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	public static readonly Color32 FILTER_TINT = Color.white;

	public static readonly Color32 NO_FILTER_TINT = new Color(0.5019608f, 0.5019608f, 0.5019608f, 1f);

	public Color32 filterTint = FILTER_TINT;

	public Color32 noFilterTint = NO_FILTER_TINT;

	[SerializeField]
	public bool dropIncorrectOnFilterChange = true;

	[SerializeField]
	public bool autoSelectStoredOnLoad = true;

	public bool showUserMenu = true;

	public bool copySettingsEnabled = true;

	public bool preventAutoAddOnDiscovery;

	public string allResourceFilterLabelString = UI.UISIDESCREENS.TREEFILTERABLESIDESCREEN.ALLBUTTON;

	public bool filterAllStoragesOnBuilding;

	public UISideScreenHeight uiHeight = UISideScreenHeight.Tall;

	public bool filterByStorageCategoriesOnSpawn = true;

	[SerializeField]
	[Serialize]
	[Obsolete("Deprecated, use acceptedTagSet")]
	private List<Tag> acceptedTags = new List<Tag>();

	[SerializeField]
	[Serialize]
	private HashSet<Tag> acceptedTagSet = new HashSet<Tag>();

	public Action<HashSet<Tag>> OnFilterChanged;

	private static readonly EventSystem.IntraObjectHandler<TreeFilterable> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<TreeFilterable>(delegate(TreeFilterable component, object data)
	{
		component.OnCopySettings(data);
	});

	public HashSet<Tag> AcceptedTags => acceptedTagSet;

	[OnDeserialized]
	[Obsolete]
	private void OnDeserialized()
	{
		if (SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 20))
		{
			filterByStorageCategoriesOnSpawn = false;
		}
		if (SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 29))
		{
			acceptedTagSet.UnionWith(acceptedTags);
			acceptedTags = null;
		}
	}

	private void OnDiscover(Tag category_tag, Tag tag)
	{
		if (preventAutoAddOnDiscovery || !storage.storageFilters.Contains(category_tag))
		{
			return;
		}
		bool flag = false;
		if (DiscoveredResources.Instance.GetDiscoveredResourcesFromTag(category_tag).Count <= 1)
		{
			foreach (Tag storageFilter in storage.storageFilters)
			{
				if (storageFilter == category_tag || !DiscoveredResources.Instance.IsDiscovered(storageFilter))
				{
					continue;
				}
				flag = true;
				foreach (Tag item in DiscoveredResources.Instance.GetDiscoveredResourcesFromTag(storageFilter))
				{
					if (!acceptedTagSet.Contains(item))
					{
						return;
					}
				}
			}
			if (!flag)
			{
				return;
			}
		}
		foreach (Tag item2 in DiscoveredResources.Instance.GetDiscoveredResourcesFromTag(category_tag))
		{
			if (!(item2 == tag) && !acceptedTagSet.Contains(item2))
			{
				return;
			}
		}
		AddTagToFilter(tag);
	}

	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Subscribe(-905833192, OnCopySettingsDelegate);
	}

	protected override void OnSpawn()
	{
		DiscoveredResources.Instance.OnDiscover += OnDiscover;
		if (autoSelectStoredOnLoad && storage != null)
		{
			HashSet<Tag> hashSet = new HashSet<Tag>(acceptedTagSet);
			hashSet.UnionWith(storage.GetAllIDsInStorage());
			UpdateFilters(hashSet);
		}
		if (OnFilterChanged != null)
		{
			OnFilterChanged(acceptedTagSet);
		}
		RefreshTint();
		if (filterByStorageCategoriesOnSpawn)
		{
			RemoveIncorrectAcceptedTags();
		}
	}

	private void RemoveIncorrectAcceptedTags()
	{
		List<Tag> list = new List<Tag>();
		foreach (Tag item in acceptedTagSet)
		{
			bool flag = false;
			foreach (Tag storageFilter in storage.storageFilters)
			{
				if (DiscoveredResources.Instance.GetDiscoveredResourcesFromTag(storageFilter).Contains(item))
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
		foreach (Tag item2 in list)
		{
			RemoveTagFromFilter(item2);
		}
	}

	protected override void OnCleanUp()
	{
		DiscoveredResources.Instance.OnDiscover -= OnDiscover;
		base.OnCleanUp();
	}

	private void OnCopySettings(object data)
	{
		if (copySettingsEnabled)
		{
			TreeFilterable component = ((GameObject)data).GetComponent<TreeFilterable>();
			if (component != null)
			{
				UpdateFilters(component.GetTags());
			}
		}
	}

	public HashSet<Tag> GetTags()
	{
		return acceptedTagSet;
	}

	public bool ContainsTag(Tag t)
	{
		return acceptedTagSet.Contains(t);
	}

	public void AddTagToFilter(Tag t)
	{
		if (!ContainsTag(t))
		{
			HashSet<Tag> hashSet = new HashSet<Tag>(acceptedTagSet);
			hashSet.Add(t);
			UpdateFilters(hashSet);
		}
	}

	public void RemoveTagFromFilter(Tag t)
	{
		if (ContainsTag(t))
		{
			HashSet<Tag> hashSet = new HashSet<Tag>(acceptedTagSet);
			hashSet.Remove(t);
			UpdateFilters(hashSet);
		}
	}

	public void UpdateFilters(HashSet<Tag> filters)
	{
		acceptedTagSet.Clear();
		acceptedTagSet.UnionWith(filters);
		if (OnFilterChanged != null)
		{
			OnFilterChanged(acceptedTagSet);
		}
		RefreshTint();
		if (!dropIncorrectOnFilterChange || storage == null || storage.items == null)
		{
			return;
		}
		if (!filterAllStoragesOnBuilding)
		{
			DropFilteredItemsFromTargetStorage(storage);
			return;
		}
		Storage[] components = GetComponents<Storage>();
		foreach (Storage targetStorage in components)
		{
			DropFilteredItemsFromTargetStorage(targetStorage);
		}
	}

	private void DropFilteredItemsFromTargetStorage(Storage targetStorage)
	{
		for (int num = targetStorage.items.Count - 1; num >= 0; num--)
		{
			GameObject gameObject = targetStorage.items[num];
			if (!(gameObject == null))
			{
				KPrefabID component = gameObject.GetComponent<KPrefabID>();
				if (!acceptedTagSet.Contains(component.PrefabTag))
				{
					targetStorage.Drop(gameObject);
				}
			}
		}
	}

	public string GetTagsAsStatus(int maxDisplays = 6)
	{
		string text = "Tags:\n";
		List<Tag> list = new List<Tag>(storage.storageFilters);
		list.Intersect(acceptedTagSet);
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

	private void RefreshTint()
	{
		bool flag = acceptedTagSet != null && acceptedTagSet.Count != 0;
		GetComponent<KBatchedAnimController>().TintColour = (flag ? filterTint : noFilterTint);
		GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.NoStorageFilterSet, !flag, this);
	}
}
