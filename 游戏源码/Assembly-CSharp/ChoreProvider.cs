using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000773 RID: 1907
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/ChoreProvider")]
public class ChoreProvider : KMonoBehaviour
{
	// Token: 0x170000F4 RID: 244
	// (get) Token: 0x0600225D RID: 8797 RVA: 0x000B6678 File Offset: 0x000B4878
	// (set) Token: 0x0600225E RID: 8798 RVA: 0x000B6680 File Offset: 0x000B4880
	public string Name { get; private set; }

	// Token: 0x0600225F RID: 8799 RVA: 0x001C25CC File Offset: 0x001C07CC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Game.Instance.Subscribe(880851192, new Action<object>(this.OnWorldParentChanged));
		Game.Instance.Subscribe(586301400, new Action<object>(this.OnMinionMigrated));
		Game.Instance.Subscribe(1142724171, new Action<object>(this.OnEntityMigrated));
	}

	// Token: 0x06002260 RID: 8800 RVA: 0x000B6689 File Offset: 0x000B4889
	protected override void OnSpawn()
	{
		if (ClusterManager.Instance != null)
		{
			ClusterManager.Instance.Subscribe(-1078710002, new Action<object>(this.OnWorldRemoved));
		}
		base.OnSpawn();
		this.Name = base.name;
	}

	// Token: 0x06002261 RID: 8801 RVA: 0x001C2638 File Offset: 0x001C0838
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Game.Instance.Unsubscribe(880851192, new Action<object>(this.OnWorldParentChanged));
		Game.Instance.Unsubscribe(586301400, new Action<object>(this.OnMinionMigrated));
		Game.Instance.Unsubscribe(1142724171, new Action<object>(this.OnEntityMigrated));
		if (ClusterManager.Instance != null)
		{
			ClusterManager.Instance.Unsubscribe(-1078710002, new Action<object>(this.OnWorldRemoved));
		}
	}

	// Token: 0x06002262 RID: 8802 RVA: 0x001C26C8 File Offset: 0x001C08C8
	protected virtual void OnWorldRemoved(object data)
	{
		int num = (int)data;
		int parentWorldId = ClusterManager.Instance.GetWorld(num).ParentWorldId;
		List<Chore> chores;
		if (this.choreWorldMap.TryGetValue(parentWorldId, out chores))
		{
			this.ClearWorldChores<Chore>(chores, num);
		}
	}

	// Token: 0x06002263 RID: 8803 RVA: 0x001C2708 File Offset: 0x001C0908
	protected virtual void OnWorldParentChanged(object data)
	{
		WorldParentChangedEventArgs worldParentChangedEventArgs = data as WorldParentChangedEventArgs;
		List<Chore> oldChores;
		if (worldParentChangedEventArgs == null || worldParentChangedEventArgs.lastParentId == 255 || worldParentChangedEventArgs.lastParentId == worldParentChangedEventArgs.world.ParentWorldId || !this.choreWorldMap.TryGetValue(worldParentChangedEventArgs.lastParentId, out oldChores))
		{
			return;
		}
		List<Chore> newChores;
		if (!this.choreWorldMap.TryGetValue(worldParentChangedEventArgs.world.ParentWorldId, out newChores))
		{
			newChores = (this.choreWorldMap[worldParentChangedEventArgs.world.ParentWorldId] = new List<Chore>());
		}
		this.TransferChores<Chore>(oldChores, newChores, worldParentChangedEventArgs.world.ParentWorldId);
	}

	// Token: 0x06002264 RID: 8804 RVA: 0x001C27B0 File Offset: 0x001C09B0
	protected virtual void OnEntityMigrated(object data)
	{
		MigrationEventArgs migrationEventArgs = data as MigrationEventArgs;
		List<Chore> oldChores;
		if (migrationEventArgs == null || !(migrationEventArgs.entity == base.gameObject) || migrationEventArgs.prevWorldId == migrationEventArgs.targetWorldId || !this.choreWorldMap.TryGetValue(migrationEventArgs.prevWorldId, out oldChores))
		{
			return;
		}
		List<Chore> newChores;
		if (!this.choreWorldMap.TryGetValue(migrationEventArgs.targetWorldId, out newChores))
		{
			newChores = (this.choreWorldMap[migrationEventArgs.targetWorldId] = new List<Chore>());
		}
		this.TransferChores<Chore>(oldChores, newChores, migrationEventArgs.targetWorldId);
	}

	// Token: 0x06002265 RID: 8805 RVA: 0x001C2844 File Offset: 0x001C0A44
	protected virtual void OnMinionMigrated(object data)
	{
		MinionMigrationEventArgs minionMigrationEventArgs = data as MinionMigrationEventArgs;
		List<Chore> oldChores;
		if (minionMigrationEventArgs == null || !(minionMigrationEventArgs.minionId.gameObject == base.gameObject) || minionMigrationEventArgs.prevWorldId == minionMigrationEventArgs.targetWorldId || !this.choreWorldMap.TryGetValue(minionMigrationEventArgs.prevWorldId, out oldChores))
		{
			return;
		}
		List<Chore> newChores;
		if (!this.choreWorldMap.TryGetValue(minionMigrationEventArgs.targetWorldId, out newChores))
		{
			newChores = (this.choreWorldMap[minionMigrationEventArgs.targetWorldId] = new List<Chore>());
		}
		this.TransferChores<Chore>(oldChores, newChores, minionMigrationEventArgs.targetWorldId);
	}

	// Token: 0x06002266 RID: 8806 RVA: 0x001C28E0 File Offset: 0x001C0AE0
	protected void TransferChores<T>(List<T> oldChores, List<T> newChores, int transferId) where T : Chore
	{
		int num = oldChores.Count - 1;
		for (int i = num; i >= 0; i--)
		{
			T t = oldChores[i];
			if (t.isNull)
			{
				DebugUtil.DevLogError(string.Concat(new string[]
				{
					"[",
					t.GetType().Name,
					"] ",
					t.GetReportName(null),
					" has no target"
				}));
			}
			else if (t.gameObject.GetMyParentWorldId() == transferId)
			{
				newChores.Add(t);
				oldChores[i] = oldChores[num];
				oldChores.RemoveAt(num--);
			}
		}
	}

	// Token: 0x06002267 RID: 8807 RVA: 0x001C299C File Offset: 0x001C0B9C
	protected void ClearWorldChores<T>(List<T> chores, int worldId) where T : Chore
	{
		int num = chores.Count - 1;
		for (int i = num; i >= 0; i--)
		{
			if (chores[i].gameObject.GetMyWorldId() == worldId)
			{
				chores[i] = chores[num];
				chores.RemoveAt(num--);
			}
		}
	}

	// Token: 0x06002268 RID: 8808 RVA: 0x001C29F0 File Offset: 0x001C0BF0
	public virtual void AddChore(Chore chore)
	{
		chore.provider = this;
		List<Chore> list = null;
		int myParentWorldId = chore.gameObject.GetMyParentWorldId();
		if (!this.choreWorldMap.TryGetValue(myParentWorldId, out list))
		{
			list = (this.choreWorldMap[myParentWorldId] = new List<Chore>());
		}
		list.Add(chore);
	}

	// Token: 0x06002269 RID: 8809 RVA: 0x001C2A3C File Offset: 0x001C0C3C
	public virtual void RemoveChore(Chore chore)
	{
		if (chore == null)
		{
			return;
		}
		chore.provider = null;
		List<Chore> list = null;
		int myParentWorldId = chore.gameObject.GetMyParentWorldId();
		if (this.choreWorldMap.TryGetValue(myParentWorldId, out list))
		{
			list.Remove(chore);
		}
	}

	// Token: 0x0600226A RID: 8810 RVA: 0x001C2A7C File Offset: 0x001C0C7C
	public virtual void CollectChores(ChoreConsumerState consumer_state, List<Chore.Precondition.Context> succeeded, List<Chore.Precondition.Context> failed_contexts)
	{
		List<Chore> list = null;
		int myParentWorldId = consumer_state.gameObject.GetMyParentWorldId();
		if (!this.choreWorldMap.TryGetValue(myParentWorldId, out list))
		{
			return;
		}
		for (int i = list.Count - 1; i >= 0; i--)
		{
			if (list[i].provider == null)
			{
				list[i].Cancel("no provider");
				list[i] = list[list.Count - 1];
				list.RemoveAt(list.Count - 1);
			}
		}
		int num = CPUBudget.coreCount * 4;
		if (list.Count < num)
		{
			using (List<Chore>.Enumerator enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Chore chore = enumerator.Current;
					chore.CollectChores(consumer_state, succeeded, failed_contexts, false);
				}
				return;
			}
		}
		ChoreProvider.batch_chore_collector.Reset(list);
		int coreCount = CPUBudget.coreCount;
		int num2 = Math.Min(24, list.Count / coreCount);
		for (int j = 0; j < list.Count; j += num2)
		{
			ChoreProvider.batch_chore_collector.Add(new ChoreProvider.ChoreProviderCollectTask(j, Math.Min(j + num2, list.Count), consumer_state));
		}
		GlobalJobManager.Run(ChoreProvider.batch_chore_collector);
		for (int k = 0; k < ChoreProvider.batch_chore_collector.Count; k++)
		{
			ChoreProvider.batch_chore_collector.GetWorkItem(k).Finish(succeeded, failed_contexts);
		}
	}

	// Token: 0x040016A3 RID: 5795
	public Dictionary<int, List<Chore>> choreWorldMap = new Dictionary<int, List<Chore>>();

	// Token: 0x040016A4 RID: 5796
	private static WorkItemCollection<ChoreProvider.ChoreProviderCollectTask, List<Chore>> batch_chore_collector = new WorkItemCollection<ChoreProvider.ChoreProviderCollectTask, List<Chore>>();

	// Token: 0x02000774 RID: 1908
	private struct ChoreProviderCollectTask : IWorkItem<List<Chore>>
	{
		// Token: 0x0600226D RID: 8813 RVA: 0x000B66E6 File Offset: 0x000B48E6
		public ChoreProviderCollectTask(int start, int end, ChoreConsumerState consumer_state)
		{
			this.start = start;
			this.end = end;
			this.consumer_state = consumer_state;
			this.succeeded = ListPool<Chore.Precondition.Context, ChoreProvider.ChoreProviderCollectTask>.Allocate();
			this.failed = ListPool<Chore.Precondition.Context, ChoreProvider.ChoreProviderCollectTask>.Allocate();
			this.incomplete = ListPool<Chore.Precondition.Context, ChoreProvider.ChoreProviderCollectTask>.Allocate();
		}

		// Token: 0x0600226E RID: 8814 RVA: 0x001C2BF0 File Offset: 0x001C0DF0
		public void Run(List<Chore> chores)
		{
			for (int i = this.start; i < this.end; i++)
			{
				chores[i].CollectChores(this.consumer_state, this.succeeded, this.incomplete, this.failed, false);
			}
		}

		// Token: 0x0600226F RID: 8815 RVA: 0x001C2C38 File Offset: 0x001C0E38
		public void Finish(List<Chore.Precondition.Context> combined_succeeded, List<Chore.Precondition.Context> combined_failed)
		{
			combined_succeeded.AddRange(this.succeeded);
			this.succeeded.Recycle();
			combined_failed.AddRange(this.failed);
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
			this.incomplete.Recycle();
		}

		// Token: 0x040016A5 RID: 5797
		private int start;

		// Token: 0x040016A6 RID: 5798
		private int end;

		// Token: 0x040016A7 RID: 5799
		private ChoreConsumerState consumer_state;

		// Token: 0x040016A8 RID: 5800
		public ListPool<Chore.Precondition.Context, ChoreProvider.ChoreProviderCollectTask>.PooledList succeeded;

		// Token: 0x040016A9 RID: 5801
		public ListPool<Chore.Precondition.Context, ChoreProvider.ChoreProviderCollectTask>.PooledList failed;

		// Token: 0x040016AA RID: 5802
		public ListPool<Chore.Precondition.Context, ChoreProvider.ChoreProviderCollectTask>.PooledList incomplete;
	}
}
