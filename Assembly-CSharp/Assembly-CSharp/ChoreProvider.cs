using System;
using System.Collections.Generic;
using UnityEngine;

[SkipSaveFileSerialization, AddComponentMenu("KMonoBehaviour/scripts/ChoreProvider")]
public class ChoreProvider : KMonoBehaviour {
    private static readonly WorkItemCollection<ChoreProviderCollectTask, List<Chore>> batch_chore_collector
        = new WorkItemCollection<ChoreProviderCollectTask, List<Chore>>();

    public Dictionary<int, List<Chore>> choreWorldMap = new Dictionary<int, List<Chore>>();
    public string                       Name { get; private set; }

    protected override void OnPrefabInit() {
        base.OnPrefabInit();
        Game.Instance.Subscribe(880851192,  OnWorldParentChanged);
        Game.Instance.Subscribe(586301400,  OnMinionMigrated);
        Game.Instance.Subscribe(1142724171, OnEntityMigrated);
    }

    protected override void OnSpawn() {
        if (ClusterManager.Instance != null) ClusterManager.Instance.Subscribe(-1078710002, OnWorldRemoved);
        base.OnSpawn();
        Name = name;
    }

    protected override void OnCleanUp() {
        base.OnCleanUp();
        Game.Instance.Unsubscribe(880851192,  OnWorldParentChanged);
        Game.Instance.Unsubscribe(586301400,  OnMinionMigrated);
        Game.Instance.Unsubscribe(1142724171, OnEntityMigrated);
        if (ClusterManager.Instance != null) ClusterManager.Instance.Unsubscribe(-1078710002, OnWorldRemoved);
    }

    protected virtual void OnWorldRemoved(object data) {
        var         num           = (int)data;
        var         parentWorldId = ClusterManager.Instance.GetWorld(num).ParentWorldId;
        List<Chore> chores;
        if (choreWorldMap.TryGetValue(parentWorldId, out chores)) ClearWorldChores(chores, num);
    }

    protected virtual void OnWorldParentChanged(object data) {
        var         worldParentChangedEventArgs = data as WorldParentChangedEventArgs;
        List<Chore> oldChores;
        if (worldParentChangedEventArgs              == null                                            ||
            worldParentChangedEventArgs.lastParentId == 255                                             ||
            worldParentChangedEventArgs.lastParentId == worldParentChangedEventArgs.world.ParentWorldId ||
            !choreWorldMap.TryGetValue(worldParentChangedEventArgs.lastParentId, out oldChores))
            return;

        List<Chore> newChores;
        if (!choreWorldMap.TryGetValue(worldParentChangedEventArgs.world.ParentWorldId, out newChores))
            newChores = choreWorldMap[worldParentChangedEventArgs.world.ParentWorldId] = new List<Chore>();

        TransferChores(oldChores, newChores, worldParentChangedEventArgs.world.ParentWorldId);
    }

    protected virtual void OnEntityMigrated(object data) {
        var         migrationEventArgs = data as MigrationEventArgs;
        List<Chore> oldChores;
        if (migrationEventArgs == null                                         ||
            !(migrationEventArgs.entity == gameObject)                         ||
            migrationEventArgs.prevWorldId == migrationEventArgs.targetWorldId ||
            !choreWorldMap.TryGetValue(migrationEventArgs.prevWorldId, out oldChores))
            return;

        List<Chore> newChores;
        if (!choreWorldMap.TryGetValue(migrationEventArgs.targetWorldId, out newChores))
            newChores = choreWorldMap[migrationEventArgs.targetWorldId] = new List<Chore>();

        TransferChores(oldChores, newChores, migrationEventArgs.targetWorldId);
    }

    protected virtual void OnMinionMigrated(object data) {
        var         minionMigrationEventArgs = data as MinionMigrationEventArgs;
        List<Chore> oldChores;
        if (minionMigrationEventArgs == null                                               ||
            !(minionMigrationEventArgs.minionId.gameObject == gameObject)                  ||
            minionMigrationEventArgs.prevWorldId == minionMigrationEventArgs.targetWorldId ||
            !choreWorldMap.TryGetValue(minionMigrationEventArgs.prevWorldId, out oldChores))
            return;

        List<Chore> newChores;
        if (!choreWorldMap.TryGetValue(minionMigrationEventArgs.targetWorldId, out newChores))
            newChores = choreWorldMap[minionMigrationEventArgs.targetWorldId] = new List<Chore>();

        TransferChores(oldChores, newChores, minionMigrationEventArgs.targetWorldId);
    }

    protected void TransferChores<T>(List<T> oldChores, List<T> newChores, int transferId) where T : Chore {
        var num = oldChores.Count - 1;
        for (var i = num; i >= 0; i--) {
            var t = oldChores[i];
            if (t.isNull)
                DebugUtil.DevLogError(string.Concat("[", t.GetType().Name, "] ", t.GetReportName(), " has no target"));
            else if (t.gameObject.GetMyParentWorldId() == transferId) {
                newChores.Add(t);
                oldChores[i] = oldChores[num];
                oldChores.RemoveAt(num--);
            }
        }
    }

    protected void ClearWorldChores<T>(List<T> chores, int worldId) where T : Chore {
        var num = chores.Count - 1;
        for (var i = num; i >= 0; i--)
            if (chores[i].gameObject.GetMyWorldId() == worldId) {
                chores[i] = chores[num];
                chores.RemoveAt(num--);
            }
    }

    public virtual void AddChore(Chore chore) {
        chore.provider = this;
        List<Chore> list            = null;
        var         myParentWorldId = chore.gameObject.GetMyParentWorldId();
        if (!choreWorldMap.TryGetValue(myParentWorldId, out list))
            list = choreWorldMap[myParentWorldId] = new List<Chore>();

        list.Add(chore);
    }

    public virtual void RemoveChore(Chore chore) {
        if (chore == null) return;

        chore.provider = null;
        List<Chore> list            = null;
        var         myParentWorldId = chore.gameObject.GetMyParentWorldId();
        if (choreWorldMap.TryGetValue(myParentWorldId, out list)) list.Remove(chore);
    }

    public virtual void CollectChores(ChoreConsumerState               consumer_state,
                                      List<Chore.Precondition.Context> succeeded,
                                      List<Chore.Precondition.Context> failed_contexts) {
        List<Chore> list            = null;
        var         myParentWorldId = consumer_state.gameObject.GetMyParentWorldId();
        if (!choreWorldMap.TryGetValue(myParentWorldId, out list)) return;

        for (var i = list.Count - 1; i >= 0; i--)
            if (list[i].provider == null) {
                list[i].Cancel("no provider");
                list[i] = list[list.Count - 1];
                list.RemoveAt(list.Count - 1);
            }

        var num = CPUBudget.coreCount * 4;
        if (list.Count < num)
            using (var enumerator = list.GetEnumerator()) {
                while (enumerator.MoveNext()) {
                    var chore = enumerator.Current;
                    chore.CollectChores(consumer_state, succeeded, failed_contexts, false);
                }

                return;
            }

        batch_chore_collector.Reset(list);
        var coreCount = CPUBudget.coreCount;
        var num2      = Math.Min(24, list.Count / coreCount);
        for (var j = 0; j < list.Count; j += num2)
            batch_chore_collector.Add(new ChoreProviderCollectTask(j, Math.Min(j + num2, list.Count), consumer_state));

        GlobalJobManager.Run(batch_chore_collector);
        for (var k = 0; k < batch_chore_collector.Count; k++)
            batch_chore_collector.GetWorkItem(k).Finish(succeeded, failed_contexts);
    }

    private struct ChoreProviderCollectTask : IWorkItem<List<Chore>> {
        public ChoreProviderCollectTask(int start, int end, ChoreConsumerState consumer_state) {
            this.start          = start;
            this.end            = end;
            this.consumer_state = consumer_state;
            succeeded           = ListPool<Chore.Precondition.Context, ChoreProviderCollectTask>.Allocate();
            failed              = ListPool<Chore.Precondition.Context, ChoreProviderCollectTask>.Allocate();
            incomplete          = ListPool<Chore.Precondition.Context, ChoreProviderCollectTask>.Allocate();
        }

        public void Run(List<Chore> chores) {
            for (var i = start; i < end; i++)
                chores[i].CollectChores(consumer_state, succeeded, incomplete, failed, false);
        }

        public void Finish(List<Chore.Precondition.Context> combined_succeeded,
                           List<Chore.Precondition.Context> combined_failed) {
            combined_succeeded.AddRange(succeeded);
            succeeded.Recycle();
            combined_failed.AddRange(failed);
            failed.Recycle();
            foreach (var item in incomplete) {
                item.FinishPreconditions();
                if (item.IsSuccess())
                    combined_succeeded.Add(item);
                else
                    combined_failed.Add(item);
            }

            incomplete.Recycle();
        }

        private readonly int                                                                       start;
        private readonly int                                                                       end;
        private readonly ChoreConsumerState                                                        consumer_state;
        public readonly  ListPool<Chore.Precondition.Context, ChoreProviderCollectTask>.PooledList succeeded;
        public readonly  ListPool<Chore.Precondition.Context, ChoreProviderCollectTask>.PooledList failed;
        public readonly  ListPool<Chore.Precondition.Context, ChoreProviderCollectTask>.PooledList incomplete;
    }
}