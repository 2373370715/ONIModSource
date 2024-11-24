using System;
using System.Collections.Generic;
using System.Diagnostics;
using FoodRehydrator;
using UnityEngine;

// Token: 0x020012ED RID: 4845
[AddComponentMenu("KMonoBehaviour/scripts/FetchManager")]
public class FetchManager : KMonoBehaviour, ISim1000ms
{
	// Token: 0x06006370 RID: 25456 RVA: 0x000E0EA7 File Offset: 0x000DF0A7
	private static int QuantizeRotValue(float rot_value)
	{
		return (int)(4f * rot_value);
	}

	// Token: 0x06006371 RID: 25457 RVA: 0x000A5E40 File Offset: 0x000A4040
	[Conditional("ENABLE_FETCH_PROFILING")]
	private static void BeginDetailedSample(string region_name)
	{
	}

	// Token: 0x06006372 RID: 25458 RVA: 0x000A5E40 File Offset: 0x000A4040
	[Conditional("ENABLE_FETCH_PROFILING")]
	private static void BeginDetailedSample(string region_name, int count)
	{
	}

	// Token: 0x06006373 RID: 25459 RVA: 0x000A5E40 File Offset: 0x000A4040
	[Conditional("ENABLE_FETCH_PROFILING")]
	private static void EndDetailedSample(string region_name)
	{
	}

	// Token: 0x06006374 RID: 25460 RVA: 0x000A5E40 File Offset: 0x000A4040
	[Conditional("ENABLE_FETCH_PROFILING")]
	private static void EndDetailedSample(string region_name, int count)
	{
	}

	// Token: 0x06006375 RID: 25461 RVA: 0x002BB368 File Offset: 0x002B9568
	public HandleVector<int>.Handle Add(Pickupable pickupable)
	{
		Tag tag = pickupable.KPrefabID.PrefabID();
		FetchManager.FetchablesByPrefabId fetchablesByPrefabId = null;
		if (!this.prefabIdToFetchables.TryGetValue(tag, out fetchablesByPrefabId))
		{
			fetchablesByPrefabId = new FetchManager.FetchablesByPrefabId(tag);
			this.prefabIdToFetchables[tag] = fetchablesByPrefabId;
		}
		return fetchablesByPrefabId.AddPickupable(pickupable);
	}

	// Token: 0x06006376 RID: 25462 RVA: 0x002BB3B0 File Offset: 0x002B95B0
	public void Remove(Tag prefab_tag, HandleVector<int>.Handle fetchable_handle)
	{
		FetchManager.FetchablesByPrefabId fetchablesByPrefabId;
		if (this.prefabIdToFetchables.TryGetValue(prefab_tag, out fetchablesByPrefabId))
		{
			fetchablesByPrefabId.RemovePickupable(fetchable_handle);
		}
	}

	// Token: 0x06006377 RID: 25463 RVA: 0x002BB3D4 File Offset: 0x002B95D4
	public void UpdateStorage(Tag prefab_tag, HandleVector<int>.Handle fetchable_handle, Storage storage)
	{
		FetchManager.FetchablesByPrefabId fetchablesByPrefabId;
		if (this.prefabIdToFetchables.TryGetValue(prefab_tag, out fetchablesByPrefabId))
		{
			fetchablesByPrefabId.UpdateStorage(fetchable_handle, storage);
		}
	}

	// Token: 0x06006378 RID: 25464 RVA: 0x000E0EB1 File Offset: 0x000DF0B1
	public void UpdateTags(Tag prefab_tag, HandleVector<int>.Handle fetchable_handle)
	{
		this.prefabIdToFetchables[prefab_tag].UpdateTags(fetchable_handle);
	}

	// Token: 0x06006379 RID: 25465 RVA: 0x002BB3FC File Offset: 0x002B95FC
	public void Sim1000ms(float dt)
	{
		foreach (KeyValuePair<Tag, FetchManager.FetchablesByPrefabId> keyValuePair in this.prefabIdToFetchables)
		{
			keyValuePair.Value.Sim1000ms(dt);
		}
	}

	// Token: 0x0600637A RID: 25466 RVA: 0x002BB458 File Offset: 0x002B9658
	public void UpdatePickups(PathProber path_prober, WorkerBase worker)
	{
		Navigator component = worker.GetComponent<Navigator>();
		this.updateOffsetTables.Reset(null);
		this.updatePickupsWorkItems.Reset(null);
		foreach (KeyValuePair<Tag, FetchManager.FetchablesByPrefabId> keyValuePair in this.prefabIdToFetchables)
		{
			FetchManager.FetchablesByPrefabId value = keyValuePair.Value;
			this.updateOffsetTables.Add(new FetchManager.UpdateOffsetTables(value));
			this.updatePickupsWorkItems.Add(new FetchManager.UpdatePickupWorkItem
			{
				fetchablesByPrefabId = value,
				pathProber = path_prober,
				navigator = component,
				worker = worker.gameObject
			});
		}
		GlobalJobManager.Run(this.updateOffsetTables);
		for (int i = 0; i < this.updateOffsetTables.Count; i++)
		{
			this.updateOffsetTables.GetWorkItem(i).Finish();
		}
		OffsetTracker.isExecutingWithinJob = true;
		GlobalJobManager.Run(this.updatePickupsWorkItems);
		OffsetTracker.isExecutingWithinJob = false;
		this.pickups.Clear();
		foreach (KeyValuePair<Tag, FetchManager.FetchablesByPrefabId> keyValuePair2 in this.prefabIdToFetchables)
		{
			this.pickups.AddRange(keyValuePair2.Value.finalPickups);
		}
		this.pickups.Sort(FetchManager.ComparerNoPriority);
	}

	// Token: 0x0600637B RID: 25467 RVA: 0x002BB5D8 File Offset: 0x002B97D8
	public static bool IsFetchablePickup(Pickupable pickup, FetchChore chore, Storage destination)
	{
		KPrefabID kprefabID = pickup.KPrefabID;
		Storage storage = pickup.storage;
		if (pickup.UnreservedAmount <= 0f)
		{
			return false;
		}
		if (kprefabID == null)
		{
			return false;
		}
		if (!pickup.isChoreAllowedToPickup(chore.choreType))
		{
			return false;
		}
		if (chore.criteria == FetchChore.MatchCriteria.MatchID && !chore.tags.Contains(kprefabID.PrefabTag))
		{
			return false;
		}
		if (chore.criteria == FetchChore.MatchCriteria.MatchTags && !kprefabID.HasTag(chore.tagsFirst))
		{
			return false;
		}
		if (chore.requiredTag.IsValid && !kprefabID.HasTag(chore.requiredTag))
		{
			return false;
		}
		if (kprefabID.HasAnyTags(chore.forbiddenTags))
		{
			return false;
		}
		if (kprefabID.HasTag(GameTags.MarkedForMove))
		{
			return false;
		}
		if (storage != null)
		{
			if (!storage.ignoreSourcePriority && destination.ShouldOnlyTransferFromLowerPriority && destination.masterPriority <= storage.masterPriority)
			{
				return false;
			}
			if (destination.storageNetworkID != -1 && destination.storageNetworkID == storage.storageNetworkID)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x0600637C RID: 25468 RVA: 0x002BB6D8 File Offset: 0x002B98D8
	public static Pickupable FindFetchTarget(List<Pickupable> pickupables, Storage destination, FetchChore chore)
	{
		foreach (Pickupable pickupable in pickupables)
		{
			if (FetchManager.IsFetchablePickup(pickupable, chore, destination))
			{
				return pickupable;
			}
		}
		return null;
	}

	// Token: 0x0600637D RID: 25469 RVA: 0x002BB730 File Offset: 0x002B9930
	public Pickupable FindFetchTarget(Storage destination, FetchChore chore)
	{
		foreach (FetchManager.Pickup pickup in this.pickups)
		{
			if (FetchManager.IsFetchablePickup(pickup.pickupable, chore, destination))
			{
				return pickup.pickupable;
			}
		}
		return null;
	}

	// Token: 0x0600637E RID: 25470 RVA: 0x000E0EC5 File Offset: 0x000DF0C5
	public static bool IsFetchablePickup_Exclude(KPrefabID pickup_id, Storage source, float pickup_unreserved_amount, HashSet<Tag> exclude_tags, Tag required_tag, Storage destination)
	{
		return FetchManager.IsFetchablePickup_Exclude(pickup_id, source, pickup_unreserved_amount, exclude_tags, new Tag[]
		{
			required_tag
		}, destination);
	}

	// Token: 0x0600637F RID: 25471 RVA: 0x002BB798 File Offset: 0x002B9998
	public static bool IsFetchablePickup_Exclude(KPrefabID pickup_id, Storage source, float pickup_unreserved_amount, HashSet<Tag> exclude_tags, Tag[] required_tags, Storage destination)
	{
		if (pickup_unreserved_amount <= 0f)
		{
			return false;
		}
		if (pickup_id == null)
		{
			return false;
		}
		if (exclude_tags.Contains(pickup_id.PrefabTag))
		{
			return false;
		}
		if (!pickup_id.HasAllTags(required_tags))
		{
			return false;
		}
		if (source != null)
		{
			if (!source.ignoreSourcePriority && destination.ShouldOnlyTransferFromLowerPriority && destination.masterPriority <= source.masterPriority)
			{
				return false;
			}
			if (destination.storageNetworkID != -1 && destination.storageNetworkID == source.storageNetworkID)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06006380 RID: 25472 RVA: 0x000E0EE1 File Offset: 0x000DF0E1
	public Pickupable FindEdibleFetchTarget(Storage destination, HashSet<Tag> exclude_tags, Tag required_tag)
	{
		return this.FindEdibleFetchTarget(destination, exclude_tags, new Tag[]
		{
			required_tag
		});
	}

	// Token: 0x06006381 RID: 25473 RVA: 0x002BB824 File Offset: 0x002B9A24
	public Pickupable FindEdibleFetchTarget(Storage destination, HashSet<Tag> exclude_tags, Tag[] required_tags)
	{
		FetchManager.Pickup pickup = new FetchManager.Pickup
		{
			PathCost = ushort.MaxValue,
			foodQuality = int.MinValue
		};
		int num = int.MaxValue;
		foreach (FetchManager.Pickup pickup2 in this.pickups)
		{
			Pickupable pickupable = pickup2.pickupable;
			if (FetchManager.IsFetchablePickup_Exclude(pickupable.KPrefabID, pickupable.storage, pickupable.UnreservedAmount, exclude_tags, required_tags, destination))
			{
				int num2 = (int)pickup2.PathCost + (5 - pickup2.foodQuality) * 50;
				if (num2 < num)
				{
					pickup = pickup2;
					num = num2;
				}
			}
		}
		Navigator component = destination.GetComponent<Navigator>();
		if (component != null)
		{
			foreach (object obj in Components.FoodRehydrators)
			{
				GameObject gameObject = (GameObject)obj;
				int cell = Grid.PosToCell(gameObject);
				int cost = component.PathProber.GetCost(cell);
				if (cost != -1 && num > cost + 50 + 5)
				{
					AccessabilityManager accessabilityManager = (gameObject != null) ? gameObject.GetComponent<AccessabilityManager>() : null;
					if (accessabilityManager != null && accessabilityManager.CanAccess(destination.gameObject))
					{
						foreach (GameObject gameObject2 in gameObject.GetComponent<Storage>().items)
						{
							Storage storage = (gameObject2 != null) ? gameObject2.GetComponent<Storage>() : null;
							if (storage != null && !storage.IsEmpty())
							{
								Edible component2 = storage.items[0].GetComponent<Edible>();
								Pickupable component3 = component2.GetComponent<Pickupable>();
								if (FetchManager.IsFetchablePickup_Exclude(component3.KPrefabID, component3.storage, component3.UnreservedAmount, exclude_tags, required_tags, destination))
								{
									int num3 = cost + (5 - component2.FoodInfo.Quality + 1) * 50 + 5;
									if (num3 < num)
									{
										pickup.pickupable = component3;
										pickup.foodQuality = component2.FoodInfo.Quality;
										pickup.tagBitsHash = component2.PrefabID().GetHashCode();
										num = num3;
									}
								}
							}
						}
					}
				}
			}
		}
		return pickup.pickupable;
	}

	// Token: 0x04004714 RID: 18196
	private static readonly FetchManager.PickupComparerIncludingPriority ComparerIncludingPriority = new FetchManager.PickupComparerIncludingPriority();

	// Token: 0x04004715 RID: 18197
	private static readonly FetchManager.PickupComparerNoPriority ComparerNoPriority = new FetchManager.PickupComparerNoPriority();

	// Token: 0x04004716 RID: 18198
	private List<FetchManager.Pickup> pickups = new List<FetchManager.Pickup>();

	// Token: 0x04004717 RID: 18199
	public Dictionary<Tag, FetchManager.FetchablesByPrefabId> prefabIdToFetchables = new Dictionary<Tag, FetchManager.FetchablesByPrefabId>();

	// Token: 0x04004718 RID: 18200
	private WorkItemCollection<FetchManager.UpdateOffsetTables, object> updateOffsetTables = new WorkItemCollection<FetchManager.UpdateOffsetTables, object>();

	// Token: 0x04004719 RID: 18201
	private WorkItemCollection<FetchManager.UpdatePickupWorkItem, object> updatePickupsWorkItems = new WorkItemCollection<FetchManager.UpdatePickupWorkItem, object>();

	// Token: 0x020012EE RID: 4846
	public struct Fetchable
	{
		// Token: 0x0400471A RID: 18202
		public Pickupable pickupable;

		// Token: 0x0400471B RID: 18203
		public int tagBitsHash;

		// Token: 0x0400471C RID: 18204
		public int masterPriority;

		// Token: 0x0400471D RID: 18205
		public int freshness;

		// Token: 0x0400471E RID: 18206
		public int foodQuality;
	}

	// Token: 0x020012EF RID: 4847
	[DebuggerDisplay("{pickupable.name}")]
	public struct Pickup
	{
		// Token: 0x0400471F RID: 18207
		public Pickupable pickupable;

		// Token: 0x04004720 RID: 18208
		public int tagBitsHash;

		// Token: 0x04004721 RID: 18209
		public ushort PathCost;

		// Token: 0x04004722 RID: 18210
		public int masterPriority;

		// Token: 0x04004723 RID: 18211
		public int freshness;

		// Token: 0x04004724 RID: 18212
		public int foodQuality;
	}

	// Token: 0x020012F0 RID: 4848
	private class PickupComparerIncludingPriority : IComparer<FetchManager.Pickup>
	{
		// Token: 0x06006384 RID: 25476 RVA: 0x002BBAE0 File Offset: 0x002B9CE0
		public int Compare(FetchManager.Pickup a, FetchManager.Pickup b)
		{
			int num = a.tagBitsHash.CompareTo(b.tagBitsHash);
			if (num != 0)
			{
				return num;
			}
			num = b.masterPriority.CompareTo(a.masterPriority);
			if (num != 0)
			{
				return num;
			}
			num = a.PathCost.CompareTo(b.PathCost);
			if (num != 0)
			{
				return num;
			}
			num = b.foodQuality.CompareTo(a.foodQuality);
			if (num != 0)
			{
				return num;
			}
			return b.freshness.CompareTo(a.freshness);
		}
	}

	// Token: 0x020012F1 RID: 4849
	private class PickupComparerNoPriority : IComparer<FetchManager.Pickup>
	{
		// Token: 0x06006386 RID: 25478 RVA: 0x002BBB64 File Offset: 0x002B9D64
		public int Compare(FetchManager.Pickup a, FetchManager.Pickup b)
		{
			int num = a.PathCost.CompareTo(b.PathCost);
			if (num != 0)
			{
				return num;
			}
			num = b.foodQuality.CompareTo(a.foodQuality);
			if (num != 0)
			{
				return num;
			}
			return b.freshness.CompareTo(a.freshness);
		}
	}

	// Token: 0x020012F2 RID: 4850
	public class FetchablesByPrefabId
	{
		// Token: 0x17000637 RID: 1591
		// (get) Token: 0x06006388 RID: 25480 RVA: 0x000E0F43 File Offset: 0x000DF143
		// (set) Token: 0x06006389 RID: 25481 RVA: 0x000E0F4B File Offset: 0x000DF14B
		public Tag prefabId { get; private set; }

		// Token: 0x0600638A RID: 25482 RVA: 0x002BBBB8 File Offset: 0x002B9DB8
		public FetchablesByPrefabId(Tag prefab_id)
		{
			this.prefabId = prefab_id;
			this.fetchables = new KCompactedVector<FetchManager.Fetchable>(0);
			this.rotUpdaters = new Dictionary<HandleVector<int>.Handle, Rottable.Instance>();
			this.finalPickups = new List<FetchManager.Pickup>();
		}

		// Token: 0x0600638B RID: 25483 RVA: 0x002BBC18 File Offset: 0x002B9E18
		public HandleVector<int>.Handle AddPickupable(Pickupable pickupable)
		{
			int foodQuality = 5;
			Edible component = pickupable.GetComponent<Edible>();
			if (component != null)
			{
				foodQuality = component.GetQuality();
			}
			int masterPriority = 0;
			if (pickupable.storage != null)
			{
				Prioritizable prioritizable = pickupable.storage.prioritizable;
				if (prioritizable != null)
				{
					masterPriority = prioritizable.GetMasterPriority().priority_value;
				}
			}
			Rottable.Instance smi = pickupable.GetSMI<Rottable.Instance>();
			int freshness = 0;
			if (!smi.IsNullOrStopped())
			{
				freshness = FetchManager.QuantizeRotValue(smi.RotValue);
			}
			KPrefabID kprefabID = pickupable.KPrefabID;
			HandleVector<int>.Handle handle = this.fetchables.Allocate(new FetchManager.Fetchable
			{
				pickupable = pickupable,
				foodQuality = foodQuality,
				freshness = freshness,
				masterPriority = masterPriority,
				tagBitsHash = kprefabID.GetTagsHash()
			});
			if (!smi.IsNullOrStopped())
			{
				this.rotUpdaters[handle] = smi;
			}
			return handle;
		}

		// Token: 0x0600638C RID: 25484 RVA: 0x000E0F54 File Offset: 0x000DF154
		public void RemovePickupable(HandleVector<int>.Handle fetchable_handle)
		{
			this.fetchables.Free(fetchable_handle);
			this.rotUpdaters.Remove(fetchable_handle);
		}

		// Token: 0x0600638D RID: 25485 RVA: 0x002BBCFC File Offset: 0x002B9EFC
		public void UpdatePickups(PathProber path_prober, Navigator worker_navigator, GameObject worker_go)
		{
			this.GatherPickupablesWhichCanBePickedUp(worker_go);
			this.GatherReachablePickups(worker_navigator);
			this.finalPickups.Sort(FetchManager.ComparerIncludingPriority);
			if (this.finalPickups.Count > 0)
			{
				FetchManager.Pickup pickup = this.finalPickups[0];
				int num = pickup.tagBitsHash;
				int num2 = this.finalPickups.Count;
				int num3 = 0;
				for (int i = 1; i < this.finalPickups.Count; i++)
				{
					bool flag = false;
					FetchManager.Pickup pickup2 = this.finalPickups[i];
					int tagBitsHash = pickup2.tagBitsHash;
					if (pickup.masterPriority == pickup2.masterPriority && tagBitsHash == num)
					{
						flag = true;
					}
					if (flag)
					{
						num2--;
					}
					else
					{
						num3++;
						pickup = pickup2;
						num = tagBitsHash;
						if (i > num3)
						{
							this.finalPickups[num3] = pickup2;
						}
					}
				}
				this.finalPickups.RemoveRange(num2, this.finalPickups.Count - num2);
			}
		}

		// Token: 0x0600638E RID: 25486 RVA: 0x002BBDE8 File Offset: 0x002B9FE8
		private void GatherPickupablesWhichCanBePickedUp(GameObject worker_go)
		{
			this.pickupsWhichCanBePickedUp.Clear();
			foreach (FetchManager.Fetchable fetchable in this.fetchables.GetDataList())
			{
				Pickupable pickupable = fetchable.pickupable;
				if (pickupable.CouldBePickedUpByMinion(worker_go))
				{
					this.pickupsWhichCanBePickedUp.Add(new FetchManager.Pickup
					{
						pickupable = pickupable,
						tagBitsHash = fetchable.tagBitsHash,
						PathCost = ushort.MaxValue,
						masterPriority = fetchable.masterPriority,
						freshness = fetchable.freshness,
						foodQuality = fetchable.foodQuality
					});
				}
			}
		}

		// Token: 0x0600638F RID: 25487 RVA: 0x002BBEB0 File Offset: 0x002BA0B0
		public void UpdateOffsetTables()
		{
			foreach (FetchManager.Fetchable fetchable in this.fetchables.GetDataList())
			{
				fetchable.pickupable.GetOffsets(fetchable.pickupable.cachedCell);
			}
		}

		// Token: 0x06006390 RID: 25488 RVA: 0x002BBF18 File Offset: 0x002BA118
		private void GatherReachablePickups(Navigator navigator)
		{
			this.cellCosts.Clear();
			this.finalPickups.Clear();
			foreach (FetchManager.Pickup pickup in this.pickupsWhichCanBePickedUp)
			{
				Pickupable pickupable = pickup.pickupable;
				int num = -1;
				if (!this.cellCosts.TryGetValue(pickupable.cachedCell, out num))
				{
					num = pickupable.GetNavigationCost(navigator, pickupable.cachedCell);
					this.cellCosts[pickupable.cachedCell] = num;
				}
				if (num != -1)
				{
					this.finalPickups.Add(new FetchManager.Pickup
					{
						pickupable = pickupable,
						tagBitsHash = pickup.tagBitsHash,
						PathCost = (ushort)num,
						masterPriority = pickup.masterPriority,
						freshness = pickup.freshness,
						foodQuality = pickup.foodQuality
					});
				}
			}
		}

		// Token: 0x06006391 RID: 25489 RVA: 0x002BC01C File Offset: 0x002BA21C
		public void UpdateStorage(HandleVector<int>.Handle fetchable_handle, Storage storage)
		{
			FetchManager.Fetchable data = this.fetchables.GetData(fetchable_handle);
			int masterPriority = 0;
			Pickupable pickupable = data.pickupable;
			if (pickupable.storage != null)
			{
				Prioritizable prioritizable = pickupable.storage.prioritizable;
				if (prioritizable != null)
				{
					masterPriority = prioritizable.GetMasterPriority().priority_value;
				}
			}
			data.masterPriority = masterPriority;
			this.fetchables.SetData(fetchable_handle, data);
		}

		// Token: 0x06006392 RID: 25490 RVA: 0x002BC088 File Offset: 0x002BA288
		public void UpdateTags(HandleVector<int>.Handle fetchable_handle)
		{
			FetchManager.Fetchable data = this.fetchables.GetData(fetchable_handle);
			data.tagBitsHash = data.pickupable.KPrefabID.GetTagsHash();
			this.fetchables.SetData(fetchable_handle, data);
		}

		// Token: 0x06006393 RID: 25491 RVA: 0x002BC0C8 File Offset: 0x002BA2C8
		public void Sim1000ms(float dt)
		{
			foreach (KeyValuePair<HandleVector<int>.Handle, Rottable.Instance> keyValuePair in this.rotUpdaters)
			{
				HandleVector<int>.Handle key = keyValuePair.Key;
				Rottable.Instance value = keyValuePair.Value;
				FetchManager.Fetchable data = this.fetchables.GetData(key);
				data.freshness = FetchManager.QuantizeRotValue(value.RotValue);
				this.fetchables.SetData(key, data);
			}
		}

		// Token: 0x04004725 RID: 18213
		public KCompactedVector<FetchManager.Fetchable> fetchables;

		// Token: 0x04004726 RID: 18214
		public List<FetchManager.Pickup> finalPickups = new List<FetchManager.Pickup>();

		// Token: 0x04004727 RID: 18215
		private Dictionary<HandleVector<int>.Handle, Rottable.Instance> rotUpdaters;

		// Token: 0x04004728 RID: 18216
		private List<FetchManager.Pickup> pickupsWhichCanBePickedUp = new List<FetchManager.Pickup>();

		// Token: 0x04004729 RID: 18217
		private Dictionary<int, int> cellCosts = new Dictionary<int, int>();
	}

	// Token: 0x020012F3 RID: 4851
	private struct UpdateOffsetTables : IWorkItem<object>
	{
		// Token: 0x06006394 RID: 25492 RVA: 0x000E0F70 File Offset: 0x000DF170
		public UpdateOffsetTables(FetchManager.FetchablesByPrefabId fetchables)
		{
			this.data = fetchables;
			this.failed = ListPool<Pickupable, FetchManager.UpdateOffsetTables>.Allocate();
		}

		// Token: 0x06006395 RID: 25493 RVA: 0x002BC154 File Offset: 0x002BA354
		public void Run(object _)
		{
			if (Game.IsOnMainThread())
			{
				this.data.UpdateOffsetTables();
				return;
			}
			foreach (FetchManager.Fetchable fetchable in this.data.fetchables.GetDataList())
			{
				if (!fetchable.pickupable.ValidateOffsets(fetchable.pickupable.cachedCell))
				{
					this.failed.Add(fetchable.pickupable);
				}
			}
		}

		// Token: 0x06006396 RID: 25494 RVA: 0x002BC1E8 File Offset: 0x002BA3E8
		public void Finish()
		{
			foreach (Pickupable pickupable in this.failed)
			{
				pickupable.GetOffsets(pickupable.cachedCell);
			}
			this.failed.Recycle();
		}

		// Token: 0x0400472B RID: 18219
		public FetchManager.FetchablesByPrefabId data;

		// Token: 0x0400472C RID: 18220
		private ListPool<Pickupable, FetchManager.UpdateOffsetTables>.PooledList failed;
	}

	// Token: 0x020012F4 RID: 4852
	private struct UpdatePickupWorkItem : IWorkItem<object>
	{
		// Token: 0x06006397 RID: 25495 RVA: 0x000E0F84 File Offset: 0x000DF184
		public void Run(object shared_data)
		{
			this.fetchablesByPrefabId.UpdatePickups(this.pathProber, this.navigator, this.worker);
		}

		// Token: 0x0400472D RID: 18221
		public FetchManager.FetchablesByPrefabId fetchablesByPrefabId;

		// Token: 0x0400472E RID: 18222
		public PathProber pathProber;

		// Token: 0x0400472F RID: 18223
		public Navigator navigator;

		// Token: 0x04004730 RID: 18224
		public GameObject worker;
	}
}
