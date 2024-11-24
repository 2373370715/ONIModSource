using System.Collections.Generic;
using System.Diagnostics;
using FoodRehydrator;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/FetchManager")]
public class FetchManager : KMonoBehaviour, ISim1000ms
{
	public struct Fetchable
	{
		public Pickupable pickupable;

		public int tagBitsHash;

		public int masterPriority;

		public int freshness;

		public int foodQuality;
	}

	[DebuggerDisplay("{pickupable.name}")]
	public struct Pickup
	{
		public Pickupable pickupable;

		public int tagBitsHash;

		public ushort PathCost;

		public int masterPriority;

		public int freshness;

		public int foodQuality;
	}

	private class PickupComparerIncludingPriority : IComparer<Pickup>
	{
		public int Compare(Pickup a, Pickup b)
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

	private class PickupComparerNoPriority : IComparer<Pickup>
	{
		public int Compare(Pickup a, Pickup b)
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

	public class FetchablesByPrefabId
	{
		public KCompactedVector<Fetchable> fetchables;

		public List<Pickup> finalPickups = new List<Pickup>();

		private Dictionary<HandleVector<int>.Handle, Rottable.Instance> rotUpdaters;

		private List<Pickup> pickupsWhichCanBePickedUp = new List<Pickup>();

		private Dictionary<int, int> cellCosts = new Dictionary<int, int>();

		public Tag prefabId { get; private set; }

		public FetchablesByPrefabId(Tag prefab_id)
		{
			prefabId = prefab_id;
			fetchables = new KCompactedVector<Fetchable>();
			rotUpdaters = new Dictionary<HandleVector<int>.Handle, Rottable.Instance>();
			finalPickups = new List<Pickup>();
		}

		public HandleVector<int>.Handle AddPickupable(Pickupable pickupable)
		{
			int foodQuality = 5;
			Edible component = pickupable.GetComponent<Edible>();
			if (component != null)
			{
				foodQuality = component.GetQuality();
			}
			int masterPriority = 0;
			Prioritizable prioritizable = null;
			if (pickupable.storage != null)
			{
				prioritizable = pickupable.storage.prioritizable;
				if (prioritizable != null)
				{
					masterPriority = prioritizable.GetMasterPriority().priority_value;
				}
			}
			Rottable.Instance sMI = pickupable.GetSMI<Rottable.Instance>();
			int freshness = 0;
			if (!sMI.IsNullOrStopped())
			{
				freshness = QuantizeRotValue(sMI.RotValue);
			}
			KPrefabID kPrefabID = pickupable.KPrefabID;
			HandleVector<int>.Handle handle = fetchables.Allocate(new Fetchable
			{
				pickupable = pickupable,
				foodQuality = foodQuality,
				freshness = freshness,
				masterPriority = masterPriority,
				tagBitsHash = kPrefabID.GetTagsHash()
			});
			if (!sMI.IsNullOrStopped())
			{
				rotUpdaters[handle] = sMI;
			}
			return handle;
		}

		public void RemovePickupable(HandleVector<int>.Handle fetchable_handle)
		{
			fetchables.Free(fetchable_handle);
			rotUpdaters.Remove(fetchable_handle);
		}

		public void UpdatePickups(PathProber path_prober, Navigator worker_navigator, GameObject worker_go)
		{
			GatherPickupablesWhichCanBePickedUp(worker_go);
			GatherReachablePickups(worker_navigator);
			finalPickups.Sort(ComparerIncludingPriority);
			if (finalPickups.Count <= 0)
			{
				return;
			}
			Pickup pickup = finalPickups[0];
			int num = pickup.tagBitsHash;
			int num2 = finalPickups.Count;
			int num3 = 0;
			for (int i = 1; i < finalPickups.Count; i++)
			{
				bool flag = false;
				Pickup pickup2 = finalPickups[i];
				int tagBitsHash = pickup2.tagBitsHash;
				if (pickup.masterPriority == pickup2.masterPriority && tagBitsHash == num)
				{
					flag = true;
				}
				if (flag)
				{
					num2--;
					continue;
				}
				num3++;
				pickup = pickup2;
				num = tagBitsHash;
				if (i > num3)
				{
					finalPickups[num3] = pickup2;
				}
			}
			finalPickups.RemoveRange(num2, finalPickups.Count - num2);
		}

		private void GatherPickupablesWhichCanBePickedUp(GameObject worker_go)
		{
			pickupsWhichCanBePickedUp.Clear();
			foreach (Fetchable data in fetchables.GetDataList())
			{
				Pickupable pickupable = data.pickupable;
				if (pickupable.CouldBePickedUpByMinion(worker_go))
				{
					pickupsWhichCanBePickedUp.Add(new Pickup
					{
						pickupable = pickupable,
						tagBitsHash = data.tagBitsHash,
						PathCost = ushort.MaxValue,
						masterPriority = data.masterPriority,
						freshness = data.freshness,
						foodQuality = data.foodQuality
					});
				}
			}
		}

		public void UpdateOffsetTables()
		{
			foreach (Fetchable data in fetchables.GetDataList())
			{
				data.pickupable.GetOffsets(data.pickupable.cachedCell);
			}
		}

		private void GatherReachablePickups(Navigator navigator)
		{
			cellCosts.Clear();
			finalPickups.Clear();
			foreach (Pickup item in pickupsWhichCanBePickedUp)
			{
				Pickupable pickupable = item.pickupable;
				int value = -1;
				if (!cellCosts.TryGetValue(pickupable.cachedCell, out value))
				{
					value = pickupable.GetNavigationCost(navigator, pickupable.cachedCell);
					cellCosts[pickupable.cachedCell] = value;
				}
				if (value != -1)
				{
					finalPickups.Add(new Pickup
					{
						pickupable = pickupable,
						tagBitsHash = item.tagBitsHash,
						PathCost = (ushort)value,
						masterPriority = item.masterPriority,
						freshness = item.freshness,
						foodQuality = item.foodQuality
					});
				}
			}
		}

		public void UpdateStorage(HandleVector<int>.Handle fetchable_handle, Storage storage)
		{
			Fetchable data = fetchables.GetData(fetchable_handle);
			int masterPriority = 0;
			Prioritizable prioritizable = null;
			Pickupable pickupable = data.pickupable;
			if (pickupable.storage != null)
			{
				prioritizable = pickupable.storage.prioritizable;
				if (prioritizable != null)
				{
					masterPriority = prioritizable.GetMasterPriority().priority_value;
				}
			}
			data.masterPriority = masterPriority;
			fetchables.SetData(fetchable_handle, data);
		}

		public void UpdateTags(HandleVector<int>.Handle fetchable_handle)
		{
			Fetchable data = fetchables.GetData(fetchable_handle);
			data.tagBitsHash = data.pickupable.KPrefabID.GetTagsHash();
			fetchables.SetData(fetchable_handle, data);
		}

		public void Sim1000ms(float dt)
		{
			foreach (KeyValuePair<HandleVector<int>.Handle, Rottable.Instance> rotUpdater in rotUpdaters)
			{
				HandleVector<int>.Handle key = rotUpdater.Key;
				Rottable.Instance value = rotUpdater.Value;
				Fetchable data = fetchables.GetData(key);
				data.freshness = QuantizeRotValue(value.RotValue);
				fetchables.SetData(key, data);
			}
		}
	}

	private struct UpdatePickupWorkItem : IWorkItem<object>
	{
		public FetchablesByPrefabId fetchablesByPrefabId;

		public PathProber pathProber;

		public Navigator navigator;

		public GameObject worker;

		public void Run(object shared_data)
		{
			fetchablesByPrefabId.UpdatePickups(pathProber, navigator, worker);
		}
	}

	private static readonly PickupComparerIncludingPriority ComparerIncludingPriority = new PickupComparerIncludingPriority();

	private static readonly PickupComparerNoPriority ComparerNoPriority = new PickupComparerNoPriority();

	private List<Pickup> pickups = new List<Pickup>();

	public Dictionary<Tag, FetchablesByPrefabId> prefabIdToFetchables = new Dictionary<Tag, FetchablesByPrefabId>();

	private WorkItemCollection<UpdatePickupWorkItem, object> updatePickupsWorkItems = new WorkItemCollection<UpdatePickupWorkItem, object>();

	private static int QuantizeRotValue(float rot_value)
	{
		return (int)(4f * rot_value);
	}

	[Conditional("ENABLE_FETCH_PROFILING")]
	private static void BeginDetailedSample(string region_name)
	{
	}

	[Conditional("ENABLE_FETCH_PROFILING")]
	private static void BeginDetailedSample(string region_name, int count)
	{
	}

	[Conditional("ENABLE_FETCH_PROFILING")]
	private static void EndDetailedSample(string region_name)
	{
	}

	[Conditional("ENABLE_FETCH_PROFILING")]
	private static void EndDetailedSample(string region_name, int count)
	{
	}

	public HandleVector<int>.Handle Add(Pickupable pickupable)
	{
		Tag tag = pickupable.KPrefabID.PrefabID();
		FetchablesByPrefabId value = null;
		if (!prefabIdToFetchables.TryGetValue(tag, out value))
		{
			value = new FetchablesByPrefabId(tag);
			prefabIdToFetchables[tag] = value;
		}
		return value.AddPickupable(pickupable);
	}

	public void Remove(Tag prefab_tag, HandleVector<int>.Handle fetchable_handle)
	{
		if (prefabIdToFetchables.TryGetValue(prefab_tag, out var value))
		{
			value.RemovePickupable(fetchable_handle);
		}
	}

	public void UpdateStorage(Tag prefab_tag, HandleVector<int>.Handle fetchable_handle, Storage storage)
	{
		if (prefabIdToFetchables.TryGetValue(prefab_tag, out var value))
		{
			value.UpdateStorage(fetchable_handle, storage);
		}
	}

	public void UpdateTags(Tag prefab_tag, HandleVector<int>.Handle fetchable_handle)
	{
		prefabIdToFetchables[prefab_tag].UpdateTags(fetchable_handle);
	}

	public void Sim1000ms(float dt)
	{
		foreach (KeyValuePair<Tag, FetchablesByPrefabId> prefabIdToFetchable in prefabIdToFetchables)
		{
			prefabIdToFetchable.Value.Sim1000ms(dt);
		}
	}

	public void UpdatePickups(PathProber path_prober, Worker worker)
	{
		Navigator component = worker.GetComponent<Navigator>();
		updatePickupsWorkItems.Reset(null);
		foreach (KeyValuePair<Tag, FetchablesByPrefabId> prefabIdToFetchable in prefabIdToFetchables)
		{
			FetchablesByPrefabId value = prefabIdToFetchable.Value;
			value.UpdateOffsetTables();
			updatePickupsWorkItems.Add(new UpdatePickupWorkItem
			{
				fetchablesByPrefabId = value,
				pathProber = path_prober,
				navigator = component,
				worker = worker.gameObject
			});
		}
		OffsetTracker.isExecutingWithinJob = true;
		GlobalJobManager.Run(updatePickupsWorkItems);
		OffsetTracker.isExecutingWithinJob = false;
		pickups.Clear();
		foreach (KeyValuePair<Tag, FetchablesByPrefabId> prefabIdToFetchable2 in prefabIdToFetchables)
		{
			pickups.AddRange(prefabIdToFetchable2.Value.finalPickups);
		}
		pickups.Sort(ComparerNoPriority);
	}

	public static bool IsFetchablePickup(Pickupable pickup, FetchChore chore, Storage destination)
	{
		KPrefabID kPrefabID = pickup.KPrefabID;
		Storage storage = pickup.storage;
		if (pickup.UnreservedAmount <= 0f)
		{
			return false;
		}
		if (kPrefabID == null)
		{
			return false;
		}
		if (!pickup.isChoreAllowedToPickup(chore.choreType))
		{
			return false;
		}
		if (chore.criteria == FetchChore.MatchCriteria.MatchID && !chore.tags.Contains(kPrefabID.PrefabTag))
		{
			return false;
		}
		if (chore.criteria == FetchChore.MatchCriteria.MatchTags && !kPrefabID.HasTag(chore.tagsFirst))
		{
			return false;
		}
		if (chore.requiredTag.IsValid && !kPrefabID.HasTag(chore.requiredTag))
		{
			return false;
		}
		if (kPrefabID.HasAnyTags(chore.forbiddenTags))
		{
			return false;
		}
		if (kPrefabID.HasTag(GameTags.MarkedForMove))
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

	public static Pickupable FindFetchTarget(List<Pickupable> pickupables, Storage destination, FetchChore chore)
	{
		foreach (Pickupable pickupable in pickupables)
		{
			if (IsFetchablePickup(pickupable, chore, destination))
			{
				return pickupable;
			}
		}
		return null;
	}

	public Pickupable FindFetchTarget(Storage destination, FetchChore chore)
	{
		foreach (Pickup pickup in pickups)
		{
			if (IsFetchablePickup(pickup.pickupable, chore, destination))
			{
				return pickup.pickupable;
			}
		}
		return null;
	}

	public static bool IsFetchablePickup_Exclude(KPrefabID pickup_id, Storage source, float pickup_unreserved_amount, HashSet<Tag> exclude_tags, Tag required_tag, Storage destination)
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
		if (!pickup_id.HasTag(required_tag))
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

	public Pickupable FindEdibleFetchTarget(Storage destination, HashSet<Tag> exclude_tags, Tag required_tag)
	{
		Pickup pickup = default(Pickup);
		pickup.PathCost = ushort.MaxValue;
		pickup.foodQuality = int.MinValue;
		Pickup pickup2 = pickup;
		int num = int.MaxValue;
		foreach (Pickup pickup3 in pickups)
		{
			Pickupable pickupable = pickup3.pickupable;
			if (IsFetchablePickup_Exclude(pickupable.KPrefabID, pickupable.storage, pickupable.UnreservedAmount, exclude_tags, required_tag, destination))
			{
				int num2 = pickup3.PathCost + (5 - pickup3.foodQuality) * 50;
				if (num2 < num)
				{
					pickup2 = pickup3;
					num = num2;
				}
			}
		}
		Navigator component = destination.GetComponent<Navigator>();
		if (component != null)
		{
			foreach (GameObject foodRehydrator in Components.FoodRehydrators)
			{
				int cell = Grid.PosToCell(foodRehydrator);
				int cost = component.PathProber.GetCost(cell);
				if (cost == -1 || num <= cost + 50 + 5)
				{
					continue;
				}
				AccessabilityManager accessabilityManager = ((foodRehydrator != null) ? foodRehydrator.GetComponent<AccessabilityManager>() : null);
				if (!(accessabilityManager != null) || !accessabilityManager.CanAccess(destination.gameObject))
				{
					continue;
				}
				foreach (GameObject item in foodRehydrator.GetComponent<Storage>().items)
				{
					Storage storage = ((item != null) ? item.GetComponent<Storage>() : null);
					if (!(storage != null) || storage.IsEmpty())
					{
						continue;
					}
					Edible component2 = storage.items[0].GetComponent<Edible>();
					Pickupable component3 = component2.GetComponent<Pickupable>();
					if (IsFetchablePickup_Exclude(component3.KPrefabID, component3.storage, component3.UnreservedAmount, exclude_tags, required_tag, destination))
					{
						int num3 = cost + (5 - component2.FoodInfo.Quality + 1) * 50 + 5;
						if (num3 < num)
						{
							pickup2.pickupable = component3;
							pickup2.foodQuality = component2.FoodInfo.Quality;
							pickup2.tagBitsHash = component2.PrefabID().GetHashCode();
							num = num3;
						}
					}
				}
			}
		}
		return pickup2.pickupable;
	}
}
