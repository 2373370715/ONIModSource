using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020012EC RID: 4844
[AddComponentMenu("KMonoBehaviour/scripts/FetchListStatusItemUpdater")]
public class FetchListStatusItemUpdater : KMonoBehaviour, IRender200ms
{
	// Token: 0x0600636A RID: 25450 RVA: 0x000E0E49 File Offset: 0x000DF049
	public static void DestroyInstance()
	{
		FetchListStatusItemUpdater.instance = null;
	}

	// Token: 0x0600636B RID: 25451 RVA: 0x000E0E51 File Offset: 0x000DF051
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		FetchListStatusItemUpdater.instance = this;
	}

	// Token: 0x0600636C RID: 25452 RVA: 0x000E0E5F File Offset: 0x000DF05F
	public void AddFetchList(FetchList2 fetch_list)
	{
		this.fetchLists.Add(fetch_list);
	}

	// Token: 0x0600636D RID: 25453 RVA: 0x000E0E6D File Offset: 0x000DF06D
	public void RemoveFetchList(FetchList2 fetch_list)
	{
		this.fetchLists.Remove(fetch_list);
	}

	// Token: 0x0600636E RID: 25454 RVA: 0x002BAD78 File Offset: 0x002B8F78
	public void Render200ms(float dt)
	{
		foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
		{
			int id = worldContainer.id;
			DictionaryPool<int, ListPool<FetchList2, FetchListStatusItemUpdater>.PooledList, FetchListStatusItemUpdater>.PooledDictionary pooledDictionary = DictionaryPool<int, ListPool<FetchList2, FetchListStatusItemUpdater>.PooledList, FetchListStatusItemUpdater>.Allocate();
			int num = Math.Min(this.maxIteratingCount, this.fetchLists.Count - this.currentIterationIndex[id]);
			for (int i = 0; i < num; i++)
			{
				FetchList2 fetchList = this.fetchLists[i + this.currentIterationIndex[id]];
				if (!(fetchList.Destination == null) && fetchList.Destination.gameObject.GetMyWorldId() == id)
				{
					ListPool<FetchList2, FetchListStatusItemUpdater>.PooledList pooledList = null;
					int instanceID = fetchList.Destination.GetInstanceID();
					if (!pooledDictionary.TryGetValue(instanceID, out pooledList))
					{
						pooledList = ListPool<FetchList2, FetchListStatusItemUpdater>.Allocate();
						pooledDictionary[instanceID] = pooledList;
					}
					pooledList.Add(fetchList);
				}
			}
			this.currentIterationIndex[id] += num;
			if (this.currentIterationIndex[id] >= this.fetchLists.Count)
			{
				this.currentIterationIndex[id] = 0;
			}
			DictionaryPool<Tag, float, FetchListStatusItemUpdater>.PooledDictionary pooledDictionary2 = DictionaryPool<Tag, float, FetchListStatusItemUpdater>.Allocate();
			DictionaryPool<Tag, float, FetchListStatusItemUpdater>.PooledDictionary pooledDictionary3 = DictionaryPool<Tag, float, FetchListStatusItemUpdater>.Allocate();
			foreach (KeyValuePair<int, ListPool<FetchList2, FetchListStatusItemUpdater>.PooledList> keyValuePair in pooledDictionary)
			{
				if (!(keyValuePair.Value[0].Destination.GetMyWorld() == null))
				{
					ListPool<Tag, FetchListStatusItemUpdater>.PooledList pooledList2 = ListPool<Tag, FetchListStatusItemUpdater>.Allocate();
					Storage destination = keyValuePair.Value[0].Destination;
					foreach (FetchList2 fetchList2 in keyValuePair.Value)
					{
						fetchList2.UpdateRemaining();
						foreach (KeyValuePair<Tag, float> keyValuePair2 in fetchList2.GetRemaining())
						{
							if (!pooledList2.Contains(keyValuePair2.Key))
							{
								pooledList2.Add(keyValuePair2.Key);
							}
						}
					}
					ListPool<Pickupable, FetchListStatusItemUpdater>.PooledList pooledList3 = ListPool<Pickupable, FetchListStatusItemUpdater>.Allocate();
					foreach (GameObject gameObject in destination.items)
					{
						if (!(gameObject == null))
						{
							Pickupable component = gameObject.GetComponent<Pickupable>();
							if (!(component == null))
							{
								pooledList3.Add(component);
							}
						}
					}
					DictionaryPool<Tag, float, FetchListStatusItemUpdater>.PooledDictionary pooledDictionary4 = DictionaryPool<Tag, float, FetchListStatusItemUpdater>.Allocate();
					foreach (Tag tag in pooledList2)
					{
						float num2 = 0f;
						foreach (Pickupable pickupable in pooledList3)
						{
							if (pickupable.KPrefabID.HasTag(tag))
							{
								num2 += pickupable.TotalAmount;
							}
						}
						pooledDictionary4[tag] = num2;
					}
					foreach (Tag tag2 in pooledList2)
					{
						if (!pooledDictionary2.ContainsKey(tag2))
						{
							pooledDictionary2[tag2] = destination.GetMyWorld().worldInventory.GetTotalAmount(tag2, true);
						}
						if (!pooledDictionary3.ContainsKey(tag2))
						{
							pooledDictionary3[tag2] = destination.GetMyWorld().worldInventory.GetAmount(tag2, true);
						}
					}
					foreach (FetchList2 fetchList3 in keyValuePair.Value)
					{
						bool should_add = false;
						bool should_add2 = true;
						bool should_add3 = false;
						foreach (KeyValuePair<Tag, float> keyValuePair3 in fetchList3.GetRemaining())
						{
							Tag key = keyValuePair3.Key;
							float value = keyValuePair3.Value;
							float num3 = pooledDictionary4[key];
							float b = pooledDictionary2[key];
							float num4 = pooledDictionary3[key];
							float num5 = Mathf.Min(value, b);
							float num6 = num4 + num5;
							float minimumAmount = fetchList3.GetMinimumAmount(key);
							if (num3 + num6 < minimumAmount)
							{
								should_add = true;
							}
							if (num6 < value)
							{
								should_add2 = false;
							}
							if (num3 + num6 > value && value > num6)
							{
								should_add3 = true;
							}
						}
						fetchList3.UpdateStatusItem(Db.Get().BuildingStatusItems.WaitingForMaterials, ref fetchList3.waitingForMaterialsHandle, should_add2);
						fetchList3.UpdateStatusItem(Db.Get().BuildingStatusItems.MaterialsUnavailable, ref fetchList3.materialsUnavailableHandle, should_add);
						fetchList3.UpdateStatusItem(Db.Get().BuildingStatusItems.MaterialsUnavailableForRefill, ref fetchList3.materialsUnavailableForRefillHandle, should_add3);
					}
					pooledDictionary4.Recycle();
					pooledList3.Recycle();
					pooledList2.Recycle();
					keyValuePair.Value.Recycle();
				}
			}
			pooledDictionary3.Recycle();
			pooledDictionary2.Recycle();
			pooledDictionary.Recycle();
		}
	}

	// Token: 0x04004710 RID: 18192
	public static FetchListStatusItemUpdater instance;

	// Token: 0x04004711 RID: 18193
	private List<FetchList2> fetchLists = new List<FetchList2>();

	// Token: 0x04004712 RID: 18194
	private int[] currentIterationIndex = new int[255];

	// Token: 0x04004713 RID: 18195
	private int maxIteratingCount = 100;
}
