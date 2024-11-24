using System;
using System.Collections.Generic;
using System.Linq;
using KSerialization;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class DiscoveredResources : KMonoBehaviour, ISaveLoadable, ISim4000ms
{
	public static DiscoveredResources Instance;

	[Serialize]
	private HashSet<Tag> Discovered = new HashSet<Tag>();

	[Serialize]
	private Dictionary<Tag, HashSet<Tag>> DiscoveredCategories = new Dictionary<Tag, HashSet<Tag>>();

	[Serialize]
	public Dictionary<Tag, float> newDiscoveries = new Dictionary<Tag, float>();

	public event Action<Tag, Tag> OnDiscover;

	public static void DestroyInstance()
	{
		Instance = null;
	}

	public void Discover(Tag tag, Tag categoryTag)
	{
		bool num = Discovered.Add(tag);
		DiscoverCategory(categoryTag, tag);
		if (num)
		{
			if (this.OnDiscover != null)
			{
				this.OnDiscover(categoryTag, tag);
			}
			if (!newDiscoveries.ContainsKey(tag))
			{
				newDiscoveries.Add(tag, (float)GameClock.Instance.GetCycle() + GameClock.Instance.GetCurrentCycleAsPercentage());
			}
		}
	}

	public void Discover(Tag tag)
	{
		Discover(tag, GetCategoryForEntity(Assets.GetPrefab(tag).GetComponent<KPrefabID>()));
	}

	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Instance = this;
	}

	protected override void OnSpawn()
	{
		base.OnSpawn();
		FilterDisabledContent();
	}

	private void FilterDisabledContent()
	{
		HashSet<Tag> hashSet = new HashSet<Tag>();
		foreach (Tag item in Discovered)
		{
			Element element = ElementLoader.GetElement(item);
			if (element != null && element.disabled)
			{
				hashSet.Add(item);
				continue;
			}
			GameObject gameObject = Assets.TryGetPrefab(item);
			if (gameObject != null && gameObject.HasTag(GameTags.DeprecatedContent))
			{
				hashSet.Add(item);
			}
			else if (gameObject == null)
			{
				hashSet.Add(item);
			}
		}
		foreach (Tag item2 in hashSet)
		{
			Discovered.Remove(item2);
		}
		foreach (KeyValuePair<Tag, HashSet<Tag>> discoveredCategory in DiscoveredCategories)
		{
			foreach (Tag item3 in hashSet)
			{
				if (discoveredCategory.Value.Contains(item3))
				{
					discoveredCategory.Value.Remove(item3);
				}
			}
		}
		foreach (string item4 in new List<string> { "Pacu", "PacuCleaner", "PacuTropical", "PacuBaby", "PacuCleanerBaby", "PacuTropicalBaby" })
		{
			if (!DiscoveredCategories.ContainsKey(item4))
			{
				continue;
			}
			List<Tag> list = DiscoveredCategories[item4].ToList();
			SolidConsumerMonitor.Def def = Assets.GetPrefab(item4).GetDef<SolidConsumerMonitor.Def>();
			foreach (Tag item5 in list)
			{
				if (def.diet.GetDietInfo(item5) == null)
				{
					DiscoveredCategories[item4].Remove(item5);
				}
			}
		}
	}

	public bool CheckAllDiscoveredAreNew()
	{
		foreach (Tag item in Discovered)
		{
			if (!newDiscoveries.ContainsKey(item))
			{
				return false;
			}
		}
		return true;
	}

	private void DiscoverCategory(Tag category_tag, Tag item_tag)
	{
		if (!DiscoveredCategories.TryGetValue(category_tag, out var value))
		{
			value = new HashSet<Tag>();
			DiscoveredCategories[category_tag] = value;
		}
		value.Add(item_tag);
	}

	public HashSet<Tag> GetDiscovered()
	{
		return Discovered;
	}

	public bool IsDiscovered(Tag tag)
	{
		if (!Discovered.Contains(tag))
		{
			return DiscoveredCategories.ContainsKey(tag);
		}
		return true;
	}

	public bool AnyDiscovered(ICollection<Tag> tags)
	{
		foreach (Tag tag in tags)
		{
			if (IsDiscovered(tag))
			{
				return true;
			}
		}
		return false;
	}

	public bool TryGetDiscoveredResourcesFromTag(Tag tag, out HashSet<Tag> resources)
	{
		return DiscoveredCategories.TryGetValue(tag, out resources);
	}

	public HashSet<Tag> GetDiscoveredResourcesFromTag(Tag tag)
	{
		if (DiscoveredCategories.TryGetValue(tag, out var value))
		{
			return value;
		}
		return new HashSet<Tag>();
	}

	public Dictionary<Tag, HashSet<Tag>> GetDiscoveredResourcesFromTagSet(TagSet tagSet)
	{
		Dictionary<Tag, HashSet<Tag>> dictionary = new Dictionary<Tag, HashSet<Tag>>();
		foreach (Tag item in tagSet)
		{
			if (DiscoveredCategories.TryGetValue(item, out var value))
			{
				dictionary[item] = value;
			}
		}
		return dictionary;
	}

	public static Tag GetCategoryForTags(HashSet<Tag> tags)
	{
		Tag result = Tag.Invalid;
		foreach (Tag tag in tags)
		{
			if (GameTags.AllCategories.Contains(tag) || GameTags.IgnoredMaterialCategories.Contains(tag))
			{
				result = tag;
				break;
			}
		}
		return result;
	}

	public static Tag GetCategoryForEntity(KPrefabID entity)
	{
		ElementChunk component = entity.GetComponent<ElementChunk>();
		if (component != null)
		{
			return component.GetComponent<PrimaryElement>().Element.materialCategory;
		}
		return GetCategoryForTags(entity.Tags);
	}

	public void Sim4000ms(float dt)
	{
		float num = GameClock.Instance.GetTimeInCycles() + GameClock.Instance.GetCurrentCycleAsPercentage();
		List<Tag> list = new List<Tag>();
		foreach (KeyValuePair<Tag, float> newDiscovery in newDiscoveries)
		{
			if (num - newDiscovery.Value > 3f)
			{
				list.Add(newDiscovery.Key);
			}
		}
		foreach (Tag item in list)
		{
			newDiscoveries.Remove(item);
		}
	}
}
