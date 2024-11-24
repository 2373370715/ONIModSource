using System;
using System.Collections.Generic;
using System.Linq;
using KSerialization;
using UnityEngine;

// Token: 0x0200123B RID: 4667
[SerializationConfig(MemberSerialization.OptIn)]
public class DiscoveredResources : KMonoBehaviour, ISaveLoadable, ISim4000ms
{
	// Token: 0x06005F86 RID: 24454 RVA: 0x000DE484 File Offset: 0x000DC684
	public static void DestroyInstance()
	{
		DiscoveredResources.Instance = null;
	}

	// Token: 0x1400001D RID: 29
	// (add) Token: 0x06005F87 RID: 24455 RVA: 0x002AA64C File Offset: 0x002A884C
	// (remove) Token: 0x06005F88 RID: 24456 RVA: 0x002AA684 File Offset: 0x002A8884
	public event Action<Tag, Tag> OnDiscover;

	// Token: 0x06005F89 RID: 24457 RVA: 0x002AA6BC File Offset: 0x002A88BC
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

	// Token: 0x06005F8A RID: 24458 RVA: 0x000DE48C File Offset: 0x000DC68C
	public void Discover(Tag tag)
	{
		this.Discover(tag, DiscoveredResources.GetCategoryForEntity(Assets.GetPrefab(tag).GetComponent<KPrefabID>()));
	}

	// Token: 0x06005F8B RID: 24459 RVA: 0x000DE4A5 File Offset: 0x000DC6A5
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		DiscoveredResources.Instance = this;
	}

	// Token: 0x06005F8C RID: 24460 RVA: 0x000DE4B3 File Offset: 0x000DC6B3
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.FilterDisabledContent();
	}

	// Token: 0x06005F8D RID: 24461 RVA: 0x002AA724 File Offset: 0x002A8924
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

	// Token: 0x06005F8E RID: 24462 RVA: 0x002AA9CC File Offset: 0x002A8BCC
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

	// Token: 0x06005F8F RID: 24463 RVA: 0x002AAA30 File Offset: 0x002A8C30
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

	// Token: 0x06005F90 RID: 24464 RVA: 0x000DE4C1 File Offset: 0x000DC6C1
	public HashSet<Tag> GetDiscovered()
	{
		return this.Discovered;
	}

	// Token: 0x06005F91 RID: 24465 RVA: 0x000DE4C9 File Offset: 0x000DC6C9
	public bool IsDiscovered(Tag tag)
	{
		return this.Discovered.Contains(tag) || this.DiscoveredCategories.ContainsKey(tag);
	}

	// Token: 0x06005F92 RID: 24466 RVA: 0x002AAA68 File Offset: 0x002A8C68
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

	// Token: 0x06005F93 RID: 24467 RVA: 0x000DE4E7 File Offset: 0x000DC6E7
	public bool TryGetDiscoveredResourcesFromTag(Tag tag, out HashSet<Tag> resources)
	{
		return this.DiscoveredCategories.TryGetValue(tag, out resources);
	}

	// Token: 0x06005F94 RID: 24468 RVA: 0x002AAABC File Offset: 0x002A8CBC
	public HashSet<Tag> GetDiscoveredResourcesFromTag(Tag tag)
	{
		HashSet<Tag> result;
		if (this.DiscoveredCategories.TryGetValue(tag, out result))
		{
			return result;
		}
		return new HashSet<Tag>();
	}

	// Token: 0x06005F95 RID: 24469 RVA: 0x002AAAE0 File Offset: 0x002A8CE0
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

	// Token: 0x06005F96 RID: 24470 RVA: 0x002AAB40 File Offset: 0x002A8D40
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

	// Token: 0x06005F97 RID: 24471 RVA: 0x002AABAC File Offset: 0x002A8DAC
	public static Tag GetCategoryForEntity(KPrefabID entity)
	{
		ElementChunk component = entity.GetComponent<ElementChunk>();
		if (component != null)
		{
			return component.GetComponent<PrimaryElement>().Element.materialCategory;
		}
		return DiscoveredResources.GetCategoryForTags(entity.Tags);
	}

	// Token: 0x06005F98 RID: 24472 RVA: 0x002AABE8 File Offset: 0x002A8DE8
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

	// Token: 0x040043C7 RID: 17351
	public static DiscoveredResources Instance;

	// Token: 0x040043C8 RID: 17352
	[Serialize]
	private HashSet<Tag> Discovered = new HashSet<Tag>();

	// Token: 0x040043C9 RID: 17353
	[Serialize]
	private Dictionary<Tag, HashSet<Tag>> DiscoveredCategories = new Dictionary<Tag, HashSet<Tag>>();

	// Token: 0x040043CB RID: 17355
	[Serialize]
	public Dictionary<Tag, float> newDiscoveries = new Dictionary<Tag, float>();
}
