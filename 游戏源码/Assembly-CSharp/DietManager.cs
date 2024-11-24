using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200123A RID: 4666
[AddComponentMenu("KMonoBehaviour/scripts/DietManager")]
public class DietManager : KMonoBehaviour
{
	// Token: 0x06005F7D RID: 24445 RVA: 0x000DE459 File Offset: 0x000DC659
	public static void DestroyInstance()
	{
		DietManager.Instance = null;
	}

	// Token: 0x06005F7E RID: 24446 RVA: 0x000DE461 File Offset: 0x000DC661
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.diets = DietManager.CollectSaveDiets(null);
		DietManager.Instance = this;
	}

	// Token: 0x06005F7F RID: 24447 RVA: 0x002AA300 File Offset: 0x002A8500
	protected override void OnSpawn()
	{
		base.OnSpawn();
		foreach (Tag tag in DiscoveredResources.Instance.GetDiscovered())
		{
			this.Discover(tag);
		}
		foreach (KeyValuePair<Tag, Diet> keyValuePair in this.diets)
		{
			Diet.Info[] infos = keyValuePair.Value.infos;
			for (int i = 0; i < infos.Length; i++)
			{
				foreach (Tag tag2 in infos[i].consumedTags)
				{
					if (Assets.GetPrefab(tag2) == null)
					{
						global::Debug.LogError(string.Format("Could not find prefab {0}, required by diet for {1}", tag2, keyValuePair.Key));
					}
				}
			}
		}
		DiscoveredResources.Instance.OnDiscover += this.OnWorldInventoryDiscover;
	}

	// Token: 0x06005F80 RID: 24448 RVA: 0x002AA448 File Offset: 0x002A8648
	private void Discover(Tag tag)
	{
		foreach (KeyValuePair<Tag, Diet> keyValuePair in this.diets)
		{
			if (keyValuePair.Value.GetDietInfo(tag) != null)
			{
				DiscoveredResources.Instance.Discover(tag, keyValuePair.Key);
			}
		}
	}

	// Token: 0x06005F81 RID: 24449 RVA: 0x000DE47B File Offset: 0x000DC67B
	private void OnWorldInventoryDiscover(Tag category_tag, Tag tag)
	{
		this.Discover(tag);
	}

	// Token: 0x06005F82 RID: 24450 RVA: 0x002AA4B8 File Offset: 0x002A86B8
	public static Dictionary<Tag, Diet> CollectDiets(Tag[] target_species)
	{
		Dictionary<Tag, Diet> dictionary = new Dictionary<Tag, Diet>();
		foreach (KPrefabID kprefabID in Assets.Prefabs)
		{
			CreatureCalorieMonitor.Def def = kprefabID.GetDef<CreatureCalorieMonitor.Def>();
			BeehiveCalorieMonitor.Def def2 = kprefabID.GetDef<BeehiveCalorieMonitor.Def>();
			Diet diet = null;
			if (def != null)
			{
				diet = def.diet;
			}
			else if (def2 != null)
			{
				diet = def2.diet;
			}
			if (diet != null && (target_species == null || Array.IndexOf<Tag>(target_species, kprefabID.GetComponent<CreatureBrain>().species) >= 0))
			{
				dictionary[kprefabID.PrefabTag] = diet;
			}
		}
		return dictionary;
	}

	// Token: 0x06005F83 RID: 24451 RVA: 0x002AA560 File Offset: 0x002A8760
	public static Dictionary<Tag, Diet> CollectSaveDiets(Tag[] target_species)
	{
		Dictionary<Tag, Diet> dictionary = new Dictionary<Tag, Diet>();
		foreach (KPrefabID kprefabID in Assets.Prefabs)
		{
			CreatureCalorieMonitor.Def def = kprefabID.GetDef<CreatureCalorieMonitor.Def>();
			BeehiveCalorieMonitor.Def def2 = kprefabID.GetDef<BeehiveCalorieMonitor.Def>();
			Diet diet = null;
			if (def != null)
			{
				diet = def.diet;
			}
			else if (def2 != null)
			{
				diet = def2.diet;
			}
			if (diet != null && (target_species == null || Array.IndexOf<Tag>(target_species, kprefabID.GetComponent<CreatureBrain>().species) >= 0))
			{
				dictionary[kprefabID.PrefabTag] = new Diet(diet);
				dictionary[kprefabID.PrefabTag].FilterDLC();
			}
		}
		return dictionary;
	}

	// Token: 0x06005F84 RID: 24452 RVA: 0x002AA620 File Offset: 0x002A8820
	public Diet GetPrefabDiet(GameObject owner)
	{
		Diet result;
		if (this.diets.TryGetValue(owner.GetComponent<KPrefabID>().PrefabTag, out result))
		{
			return result;
		}
		return null;
	}

	// Token: 0x040043C5 RID: 17349
	private Dictionary<Tag, Diet> diets;

	// Token: 0x040043C6 RID: 17350
	public static DietManager Instance;
}
