using System;
using System.Collections.Generic;

// Token: 0x0200077E RID: 1918
public class GlobalChoreProvider : ChoreProvider, IRender200ms
{
	// Token: 0x0600228C RID: 8844 RVA: 0x000B67B6 File Offset: 0x000B49B6
	public static void DestroyInstance()
	{
		GlobalChoreProvider.Instance = null;
	}

	// Token: 0x0600228D RID: 8845 RVA: 0x000B67BE File Offset: 0x000B49BE
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		GlobalChoreProvider.Instance = this;
		this.clearableManager = new ClearableManager();
	}

	// Token: 0x0600228E RID: 8846 RVA: 0x001C3370 File Offset: 0x001C1570
	protected override void OnWorldRemoved(object data)
	{
		int num = (int)data;
		int parentWorldId = ClusterManager.Instance.GetWorld(num).ParentWorldId;
		List<FetchChore> chores;
		if (this.fetchMap.TryGetValue(parentWorldId, out chores))
		{
			base.ClearWorldChores<FetchChore>(chores, num);
		}
		base.OnWorldRemoved(data);
	}

	// Token: 0x0600228F RID: 8847 RVA: 0x001C33B4 File Offset: 0x001C15B4
	protected override void OnWorldParentChanged(object data)
	{
		WorldParentChangedEventArgs worldParentChangedEventArgs = data as WorldParentChangedEventArgs;
		if (worldParentChangedEventArgs == null || worldParentChangedEventArgs.lastParentId == 255)
		{
			return;
		}
		base.OnWorldParentChanged(data);
		List<FetchChore> oldChores;
		if (!this.fetchMap.TryGetValue(worldParentChangedEventArgs.lastParentId, out oldChores))
		{
			return;
		}
		List<FetchChore> newChores;
		if (!this.fetchMap.TryGetValue(worldParentChangedEventArgs.world.ParentWorldId, out newChores))
		{
			newChores = (this.fetchMap[worldParentChangedEventArgs.world.ParentWorldId] = new List<FetchChore>());
		}
		base.TransferChores<FetchChore>(oldChores, newChores, worldParentChangedEventArgs.world.ParentWorldId);
	}

	// Token: 0x06002290 RID: 8848 RVA: 0x001C3440 File Offset: 0x001C1640
	public override void AddChore(Chore chore)
	{
		FetchChore fetchChore = chore as FetchChore;
		if (fetchChore != null)
		{
			int myParentWorldId = fetchChore.gameObject.GetMyParentWorldId();
			List<FetchChore> list;
			if (!this.fetchMap.TryGetValue(myParentWorldId, out list))
			{
				list = (this.fetchMap[myParentWorldId] = new List<FetchChore>());
			}
			chore.provider = this;
			list.Add(fetchChore);
			return;
		}
		base.AddChore(chore);
	}

	// Token: 0x06002291 RID: 8849 RVA: 0x001C349C File Offset: 0x001C169C
	public override void RemoveChore(Chore chore)
	{
		FetchChore fetchChore = chore as FetchChore;
		if (fetchChore != null)
		{
			int myParentWorldId = fetchChore.gameObject.GetMyParentWorldId();
			List<FetchChore> list;
			if (this.fetchMap.TryGetValue(myParentWorldId, out list))
			{
				list.Remove(fetchChore);
			}
			chore.provider = null;
			return;
		}
		base.RemoveChore(chore);
	}

	// Token: 0x06002292 RID: 8850 RVA: 0x001C34E8 File Offset: 0x001C16E8
	public void UpdateFetches(PathProber path_prober)
	{
		List<FetchChore> list = null;
		int myParentWorldId = path_prober.gameObject.GetMyParentWorldId();
		if (!this.fetchMap.TryGetValue(myParentWorldId, out list))
		{
			return;
		}
		this.fetches.Clear();
		Navigator component = path_prober.GetComponent<Navigator>();
		for (int i = list.Count - 1; i >= 0; i--)
		{
			FetchChore fetchChore = list[i];
			if (!(fetchChore.driver != null) && (!(fetchChore.automatable != null) || !fetchChore.automatable.GetAutomationOnly()))
			{
				if (fetchChore.provider == null)
				{
					fetchChore.Cancel("no provider");
					list[i] = list[list.Count - 1];
					list.RemoveAt(list.Count - 1);
				}
				else
				{
					Storage destination = fetchChore.destination;
					if (!(destination == null))
					{
						int navigationCost = component.GetNavigationCost(destination);
						if (navigationCost != -1)
						{
							this.fetches.Add(new GlobalChoreProvider.Fetch
							{
								chore = fetchChore,
								idsHash = fetchChore.tagsHash,
								cost = navigationCost,
								priority = fetchChore.masterPriority,
								category = destination.fetchCategory
							});
						}
					}
				}
			}
		}
		if (this.fetches.Count > 0)
		{
			this.fetches.Sort(GlobalChoreProvider.Comparer);
			int j = 1;
			int num = 0;
			while (j < this.fetches.Count)
			{
				if (!this.fetches[num].IsBetterThan(this.fetches[j]))
				{
					num++;
					this.fetches[num] = this.fetches[j];
				}
				j++;
			}
			this.fetches.RemoveRange(num + 1, this.fetches.Count - num - 1);
		}
		this.clearableManager.CollectAndSortClearables(component);
	}

	// Token: 0x06002293 RID: 8851 RVA: 0x001C36DC File Offset: 0x001C18DC
	public override void CollectChores(ChoreConsumerState consumer_state, List<Chore.Precondition.Context> succeeded, List<Chore.Precondition.Context> failed_contexts)
	{
		base.CollectChores(consumer_state, succeeded, failed_contexts);
		this.clearableManager.CollectChores(this.fetches, consumer_state, succeeded, failed_contexts);
		int num = CPUBudget.coreCount * 4;
		if (this.fetches.Count > num)
		{
			GlobalChoreProvider.batch_fetch_collector.Reset(this);
			int coreCount = CPUBudget.coreCount;
			int num2 = Math.Min(16, this.fetches.Count / coreCount);
			for (int i = 0; i < this.fetches.Count; i += num2)
			{
				GlobalChoreProvider.batch_fetch_collector.Add(new GlobalChoreProvider.FetchChoreCollectTask(i, Math.Min(i + num2, this.fetches.Count), consumer_state));
			}
			GlobalJobManager.Run(GlobalChoreProvider.batch_fetch_collector);
			for (int j = 0; j < coreCount; j++)
			{
				GlobalChoreProvider.batch_fetch_collector.GetWorkItem(j).Finish(succeeded, failed_contexts);
			}
			return;
		}
		for (int k = 0; k < this.fetches.Count; k++)
		{
			this.fetches[k].chore.CollectChoresFromGlobalChoreProvider(consumer_state, succeeded, failed_contexts, false);
		}
	}

	// Token: 0x06002294 RID: 8852 RVA: 0x000B67D7 File Offset: 0x000B49D7
	public HandleVector<int>.Handle RegisterClearable(Clearable clearable)
	{
		return this.clearableManager.RegisterClearable(clearable);
	}

	// Token: 0x06002295 RID: 8853 RVA: 0x000B67E5 File Offset: 0x000B49E5
	public void UnregisterClearable(HandleVector<int>.Handle handle)
	{
		this.clearableManager.UnregisterClearable(handle);
	}

	// Token: 0x06002296 RID: 8854 RVA: 0x000B67F3 File Offset: 0x000B49F3
	protected override void OnLoadLevel()
	{
		base.OnLoadLevel();
		GlobalChoreProvider.Instance = null;
	}

	// Token: 0x06002297 RID: 8855 RVA: 0x000B6801 File Offset: 0x000B4A01
	public void Render200ms(float dt)
	{
		this.UpdateStorageFetchableBits();
	}

	// Token: 0x06002298 RID: 8856 RVA: 0x001C37E8 File Offset: 0x001C19E8
	private void UpdateStorageFetchableBits()
	{
		ChoreType storageFetch = Db.Get().ChoreTypes.StorageFetch;
		ChoreType foodFetch = Db.Get().ChoreTypes.FoodFetch;
		this.storageFetchableTags.Clear();
		List<int> worldIDsSorted = ClusterManager.Instance.GetWorldIDsSorted();
		for (int i = 0; i < worldIDsSorted.Count; i++)
		{
			List<FetchChore> list;
			if (this.fetchMap.TryGetValue(worldIDsSorted[i], out list))
			{
				for (int j = 0; j < list.Count; j++)
				{
					FetchChore fetchChore = list[j];
					if ((fetchChore.choreType == storageFetch || fetchChore.choreType == foodFetch) && fetchChore.destination)
					{
						int cell = Grid.PosToCell(fetchChore.destination);
						if (MinionGroupProber.Get().IsReachable(cell, fetchChore.destination.GetOffsets(cell)))
						{
							this.storageFetchableTags.UnionWith(fetchChore.tags);
						}
					}
				}
			}
		}
	}

	// Token: 0x06002299 RID: 8857 RVA: 0x001C38D8 File Offset: 0x001C1AD8
	public bool ClearableHasDestination(Pickupable pickupable)
	{
		KPrefabID kprefabID = pickupable.KPrefabID;
		return this.storageFetchableTags.Contains(kprefabID.PrefabTag);
	}

	// Token: 0x040016BF RID: 5823
	public static GlobalChoreProvider Instance;

	// Token: 0x040016C0 RID: 5824
	public Dictionary<int, List<FetchChore>> fetchMap = new Dictionary<int, List<FetchChore>>();

	// Token: 0x040016C1 RID: 5825
	public List<GlobalChoreProvider.Fetch> fetches = new List<GlobalChoreProvider.Fetch>();

	// Token: 0x040016C2 RID: 5826
	private static readonly GlobalChoreProvider.FetchComparer Comparer = new GlobalChoreProvider.FetchComparer();

	// Token: 0x040016C3 RID: 5827
	private ClearableManager clearableManager;

	// Token: 0x040016C4 RID: 5828
	private HashSet<Tag> storageFetchableTags = new HashSet<Tag>();

	// Token: 0x040016C5 RID: 5829
	private static WorkItemCollection<GlobalChoreProvider.FetchChoreCollectTask, GlobalChoreProvider> batch_fetch_collector = new WorkItemCollection<GlobalChoreProvider.FetchChoreCollectTask, GlobalChoreProvider>();

	// Token: 0x0200077F RID: 1919
	public struct Fetch
	{
		// Token: 0x0600229C RID: 8860 RVA: 0x001C3900 File Offset: 0x001C1B00
		public bool IsBetterThan(GlobalChoreProvider.Fetch fetch)
		{
			if (this.category != fetch.category)
			{
				return false;
			}
			if (this.idsHash != fetch.idsHash)
			{
				return false;
			}
			if (this.chore.choreType != fetch.chore.choreType)
			{
				return false;
			}
			if (this.priority.priority_class > fetch.priority.priority_class)
			{
				return true;
			}
			if (this.priority.priority_class == fetch.priority.priority_class)
			{
				if (this.priority.priority_value > fetch.priority.priority_value)
				{
					return true;
				}
				if (this.priority.priority_value == fetch.priority.priority_value)
				{
					return this.cost <= fetch.cost;
				}
			}
			return false;
		}

		// Token: 0x040016C6 RID: 5830
		public FetchChore chore;

		// Token: 0x040016C7 RID: 5831
		public int idsHash;

		// Token: 0x040016C8 RID: 5832
		public int cost;

		// Token: 0x040016C9 RID: 5833
		public PrioritySetting priority;

		// Token: 0x040016CA RID: 5834
		public Storage.FetchCategory category;
	}

	// Token: 0x02000780 RID: 1920
	private struct FetchChoreCollectTask : IWorkItem<GlobalChoreProvider>
	{
		// Token: 0x0600229D RID: 8861 RVA: 0x000B6848 File Offset: 0x000B4A48
		public FetchChoreCollectTask(int start, int end, ChoreConsumerState consumer_state)
		{
			this.start = start;
			this.end = end;
			this.consumer_state = consumer_state;
			this.succeeded = ListPool<Chore.Precondition.Context, GlobalChoreProvider.FetchChoreCollectTask>.Allocate();
			this.failed = ListPool<Chore.Precondition.Context, GlobalChoreProvider.FetchChoreCollectTask>.Allocate();
			this.incomplete = ListPool<Chore.Precondition.Context, GlobalChoreProvider.FetchChoreCollectTask>.Allocate();
		}

		// Token: 0x0600229E RID: 8862 RVA: 0x001C39C0 File Offset: 0x001C1BC0
		public void Run(GlobalChoreProvider context)
		{
			for (int i = this.start; i < this.end; i++)
			{
				context.fetches[i].chore.CollectChoresFromGlobalChoreProvider(this.consumer_state, this.succeeded, this.incomplete, this.failed, false);
			}
		}

		// Token: 0x0600229F RID: 8863 RVA: 0x001C3A14 File Offset: 0x001C1C14
		public void Finish(List<Chore.Precondition.Context> combined_succeeded, List<Chore.Precondition.Context> combined_failed)
		{
			combined_succeeded.AddRange(this.succeeded);
			this.succeeded.Clear();
			this.succeeded.Recycle();
			combined_failed.AddRange(this.failed);
			this.failed.Clear();
			this.failed.Recycle();
			foreach (Chore.Precondition.Context item in this.incomplete)
			{
				item.FinishPreconditions();
				if (item.IsSuccess())
				{
					combined_succeeded.Add(item);
				}
				else
				{
					combined_failed.Add(item);
				}
			}
			this.incomplete.Clear();
			this.incomplete.Recycle();
		}

		// Token: 0x040016CB RID: 5835
		private int start;

		// Token: 0x040016CC RID: 5836
		private int end;

		// Token: 0x040016CD RID: 5837
		private ChoreConsumerState consumer_state;

		// Token: 0x040016CE RID: 5838
		public ListPool<Chore.Precondition.Context, GlobalChoreProvider.FetchChoreCollectTask>.PooledList succeeded;

		// Token: 0x040016CF RID: 5839
		public ListPool<Chore.Precondition.Context, GlobalChoreProvider.FetchChoreCollectTask>.PooledList failed;

		// Token: 0x040016D0 RID: 5840
		public ListPool<Chore.Precondition.Context, GlobalChoreProvider.FetchChoreCollectTask>.PooledList incomplete;
	}

	// Token: 0x02000781 RID: 1921
	private class FetchComparer : IComparer<GlobalChoreProvider.Fetch>
	{
		// Token: 0x060022A0 RID: 8864 RVA: 0x001C3ADC File Offset: 0x001C1CDC
		public int Compare(GlobalChoreProvider.Fetch a, GlobalChoreProvider.Fetch b)
		{
			int num = b.priority.priority_class - a.priority.priority_class;
			if (num != 0)
			{
				return num;
			}
			int num2 = b.priority.priority_value - a.priority.priority_value;
			if (num2 != 0)
			{
				return num2;
			}
			return a.cost - b.cost;
		}
	}

	// Token: 0x02000782 RID: 1922
	private struct FindTopPriorityTask : IWorkItem<object>
	{
		// Token: 0x060022A2 RID: 8866 RVA: 0x000B6880 File Offset: 0x000B4A80
		public FindTopPriorityTask(int start, int end, List<Prioritizable> worldCollection)
		{
			this.start = start;
			this.end = end;
			this.worldCollection = worldCollection;
			this.found = false;
		}

		// Token: 0x060022A3 RID: 8867 RVA: 0x001C3B30 File Offset: 0x001C1D30
		public void Run(object context)
		{
			if (GlobalChoreProvider.FindTopPriorityTask.abort)
			{
				return;
			}
			int num = this.start;
			while (num != this.end && this.worldCollection.Count > num)
			{
				if (!(this.worldCollection[num] == null) && this.worldCollection[num].IsTopPriority())
				{
					this.found = true;
					break;
				}
				num++;
			}
			if (this.found)
			{
				GlobalChoreProvider.FindTopPriorityTask.abort = true;
			}
		}

		// Token: 0x040016D1 RID: 5841
		private int start;

		// Token: 0x040016D2 RID: 5842
		private int end;

		// Token: 0x040016D3 RID: 5843
		private List<Prioritizable> worldCollection;

		// Token: 0x040016D4 RID: 5844
		public bool found;

		// Token: 0x040016D5 RID: 5845
		public static bool abort;
	}
}
