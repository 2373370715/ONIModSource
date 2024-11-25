using System;
using System.Collections.Generic;
using System.Linq;
using KSerialization;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class DiscoveredResources : KMonoBehaviour, ISaveLoadable, ISim4000ms
{
		public static void DestroyInstance()
	{
		DiscoveredResources.Instance = null;
	}

				public event Action<Tag, Tag> OnDiscover;

		public void Discover(Tag tag, Tag categoryTag)
	{
		bool flag = this.Discovered.Add(tag);
		this.DiscoverCategory(categoryTag, tag);
		if (flag)
		{
			if (this.OnDiscover != null)
			{
				this.OnDiscover(categoryTag, tag);
			}
			if (!this.newDiscoveries.ContainsKey(tag))
			{
				this.newDiscoveries.Add(tag, (float)GameClock.Instance.GetCycle() + GameClock.Instance.GetCurrentCycleAsPercentage());
			}
		}
	}

		public void Discover(Tag tag)
	{
		this.Discover(tag, DiscoveredResources.GetCategoryForEntity(Assets.GetPrefab(tag).GetComponent<KPrefabID>()));
	}

		protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		DiscoveredResources.Instance = this;
	}

		protected override void OnSpawn()
	{
		base.OnSpawn();
		this.FilterDisabledContent();
	}

		private void FilterDisabledContent()
	{
		HashSet<Tag> hashSet = new HashSet<Tag>();
		foreach (Tag tag in this.Discovered)
		{
			Element element = ElementLoader.GetElement(tag);
			if (element != null && element.disabled)
			{
				hashSet.Add(tag);
			}
			else
			{
				GameObject gameObject = Assets.TryGetPrefab(tag);
				if (gameObject != null && gameObject.HasTag(GameTags.DeprecatedContent))
				{
					hashSet.Add(tag);
				}
				else if (gameObject == null)
				{
					hashSet.Add(tag);
				}
			}
		}
		foreach (Tag item in hashSet)
		{
			this.Discovered.Remove(item);
		}
		foreach (KeyValuePair<Tag, HashSet<Tag>> keyValuePair in this.DiscoveredCategories)
		{
			foreach (Tag item2 in hashSet)
			{
				if (keyValuePair.Value.Contains(item2))
				{
					keyValuePair.Value.Remove(item2);
				}
			}
		}
		foreach (string s in new List<string>
		{
			"Pacu",
			"PacuCleaner",
			"PacuTropical",
			"PacuBaby",
			"PacuCleanerBaby",
			"PacuTropicalBaby"
		})
		{
			if (this.DiscoveredCategories.ContainsKey(s))
			{
				List<Tag> list = this.DiscoveredCategories[s].ToList<Tag>();
				SolidConsumerMonitor.Def def = Assets.GetPrefab(s).GetDef<SolidConsumerMonitor.Def>();
				foreach (Tag tag2 in list)
				{
					if (def.diet.GetDietInfo(tag2) == null)
					{
						this.DiscoveredCategories[s].Remove(tag2);
					}
				}
			}
		}
	}

		public bool CheckAllDiscoveredAreNew()
	{
		foreach (Tag key in this.Discovered)
		{
			if (!this.newDiscoveries.ContainsKey(key))
			{
				return false;
			}
		}
		return true;
	}

		private void DiscoverCategory(Tag category_tag, Tag item_tag)
	{
		HashSet<Tag> hashSet;
		if (!this.DiscoveredCategories.TryGetValue(category_tag, out hashSet))
		{
			hashSet = new HashSet<Tag>();
			this.DiscoveredCategories[category_tag] = hashSet;
		}
		hashSet.Add(item_tag);
	}

		public HashSet<Tag> GetDiscovered()
	{
		return this.Discovered;
	}

		public bool IsDiscovered(Tag tag)
	{
		return this.Discovered.Contains(tag) || this.DiscoveredCategories.ContainsKey(tag);
	}

		public bool AnyDiscovered(ICollection<Tag> tags)
	{
		foreach (Tag tag in tags)
		{
			if (this.IsDiscovered(tag))
			{
				return true;
			}
		}
		return false;
	}

		public bool TryGetDiscoveredResourcesFromTag(Tag tag, out HashSet<Tag> resources)
	{
		return this.DiscoveredCategories.TryGetValue(tag, out resources);
	}

		public HashSet<Tag> GetDiscoveredResourcesFromTag(Tag tag)
	{
		HashSet<Tag> result;
		if (this.DiscoveredCategories.TryGetValue(tag, out result))
		{
			return result;
		}
		return new HashSet<Tag>();
	}

		public Dictionary<Tag, HashSet<Tag>> GetDiscoveredResourcesFromTagSet(TagSet tagSet)
	{
		Dictionary<Tag, HashSet<Tag>> dictionary = new Dictionary<Tag, HashSet<Tag>>();
		foreach (Tag key in tagSet)
		{
			HashSet<Tag> value;
			if (this.DiscoveredCategories.TryGetValue(key, out value))
			{
				dictionary[key] = value;
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
		return DiscoveredResources.GetCategoryForTags(entity.Tags);
	}

		public void Sim4000ms(float dt)
	{
		float num = GameClock.Instance.GetTimeInCycles() + GameClock.Instance.GetCurrentCycleAsPercentage();
		List<Tag> list = new List<Tag>();
		foreach (KeyValuePair<Tag, float> keyValuePair in this.newDiscoveries)
		{
			if (num - keyValuePair.Value > 3f)
			{
				list.Add(keyValuePair.Key);
			}
		}
		foreach (Tag key in list)
		{
			this.newDiscoveries.Remove(key);
		}
	}

		public static DiscoveredResources Instance;

		[Serialize]
	private HashSet<Tag> Discovered = new HashSet<Tag>();

		[Serialize]
	private Dictionary<Tag, HashSet<Tag>> DiscoveredCategories = new Dictionary<Tag, HashSet<Tag>>();

		[Serialize]
	public Dictionary<Tag, float> newDiscoveries = new Dictionary<Tag, float>();
}
