using System.Collections.Generic;

internal class ClearableManager
{
	private struct MarkedClearable
	{
		public Clearable clearable;

		public Pickupable pickupable;

		public Prioritizable prioritizable;
	}

	private struct SortedClearable
	{
		public class Comparer : IComparer<SortedClearable>
		{
			public int Compare(SortedClearable a, SortedClearable b)
			{
				int num = b.masterPriority.priority_value - a.masterPriority.priority_value;
				if (num == 0)
				{
					return a.cost - b.cost;
				}
				return num;
			}
		}

		public Pickupable pickupable;

		public PrioritySetting masterPriority;

		public int cost;

		public static Comparer comparer = new Comparer();
	}

	private KCompactedVector<MarkedClearable> markedClearables = new KCompactedVector<MarkedClearable>();

	private List<SortedClearable> sortedClearables = new List<SortedClearable>();

	public HandleVector<int>.Handle RegisterClearable(Clearable clearable)
	{
		return markedClearables.Allocate(new MarkedClearable
		{
			clearable = clearable,
			pickupable = clearable.GetComponent<Pickupable>(),
			prioritizable = clearable.GetComponent<Prioritizable>()
		});
	}

	public void UnregisterClearable(HandleVector<int>.Handle handle)
	{
		markedClearables.Free(handle);
	}

	public void CollectAndSortClearables(Navigator navigator)
	{
		sortedClearables.Clear();
		foreach (MarkedClearable data in markedClearables.GetDataList())
		{
			int navigationCost = data.pickupable.GetNavigationCost(navigator, data.pickupable.cachedCell);
			if (navigationCost != -1)
			{
				sortedClearables.Add(new SortedClearable
				{
					pickupable = data.pickupable,
					masterPriority = data.prioritizable.GetMasterPriority(),
					cost = navigationCost
				});
			}
		}
		sortedClearables.Sort(SortedClearable.comparer);
	}

	public void CollectChores(List<GlobalChoreProvider.Fetch> fetches, ChoreConsumerState consumer_state, List<Chore.Precondition.Context> succeeded, List<Chore.Precondition.Context> failed_contexts)
	{
		ChoreType transport = Db.Get().ChoreTypes.Transport;
		int personalPriority = consumer_state.consumer.GetPersonalPriority(transport);
		int priority = (Game.Instance.advancedPersonalPriorities ? transport.explicitPriority : transport.priority);
		bool flag = false;
		for (int i = 0; i < sortedClearables.Count; i++)
		{
			SortedClearable sortedClearable = sortedClearables[i];
			Pickupable pickupable = sortedClearable.pickupable;
			PrioritySetting masterPriority = sortedClearable.masterPriority;
			Chore.Precondition.Context item = default(Chore.Precondition.Context);
			item.personalPriority = personalPriority;
			KPrefabID kPrefabID = pickupable.KPrefabID;
			int num = 0;
			while (fetches != null && num < fetches.Count)
			{
				GlobalChoreProvider.Fetch fetch = fetches[num];
				if ((fetch.chore.criteria == FetchChore.MatchCriteria.MatchID && fetch.chore.tags.Contains(kPrefabID.PrefabTag)) || (fetch.chore.criteria == FetchChore.MatchCriteria.MatchTags && kPrefabID.HasTag(fetch.chore.tagsFirst)))
				{
					item.Set(fetch.chore, consumer_state, is_attempting_override: false, pickupable);
					item.choreTypeForPermission = transport;
					item.RunPreconditions();
					if (item.IsSuccess())
					{
						item.masterPriority = masterPriority;
						item.priority = priority;
						item.interruptPriority = transport.interruptPriority;
						succeeded.Add(item);
						flag = true;
						break;
					}
				}
				num++;
			}
			if (flag)
			{
				break;
			}
		}
	}
}
