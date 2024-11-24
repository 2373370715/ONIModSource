using System.Collections.Generic;
using UnityEngine;

[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/ChoreProvider")]
public class ChoreProvider : KMonoBehaviour
{
	public Dictionary<int, List<Chore>> choreWorldMap = new Dictionary<int, List<Chore>>();

	public string Name { get; private set; }

	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Game.Instance.Subscribe(880851192, OnWorldParentChanged);
		Game.Instance.Subscribe(586301400, OnMinionMigrated);
		Game.Instance.Subscribe(1142724171, OnEntityMigrated);
	}

	protected override void OnSpawn()
	{
		if (ClusterManager.Instance != null)
		{
			ClusterManager.Instance.Subscribe(-1078710002, OnWorldRemoved);
		}
		base.OnSpawn();
		Name = base.name;
	}

	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Game.Instance.Unsubscribe(880851192, OnWorldParentChanged);
		Game.Instance.Unsubscribe(586301400, OnMinionMigrated);
		Game.Instance.Unsubscribe(1142724171, OnEntityMigrated);
		if (ClusterManager.Instance != null)
		{
			ClusterManager.Instance.Unsubscribe(-1078710002, OnWorldRemoved);
		}
	}

	protected virtual void OnWorldRemoved(object data)
	{
		int num = (int)data;
		int parentWorldId = ClusterManager.Instance.GetWorld(num).ParentWorldId;
		if (choreWorldMap.TryGetValue(parentWorldId, out var value))
		{
			ClearWorldChores(value, num);
		}
	}

	protected virtual void OnWorldParentChanged(object data)
	{
		if (data is WorldParentChangedEventArgs { lastParentId: not 255 } worldParentChangedEventArgs && worldParentChangedEventArgs.lastParentId != worldParentChangedEventArgs.world.ParentWorldId && choreWorldMap.TryGetValue(worldParentChangedEventArgs.lastParentId, out var value))
		{
			if (!choreWorldMap.TryGetValue(worldParentChangedEventArgs.world.ParentWorldId, out var value2))
			{
				value2 = (choreWorldMap[worldParentChangedEventArgs.world.ParentWorldId] = new List<Chore>());
			}
			TransferChores(value, value2, worldParentChangedEventArgs.world.ParentWorldId);
		}
	}

	protected virtual void OnEntityMigrated(object data)
	{
		if (data is MigrationEventArgs migrationEventArgs && migrationEventArgs.entity == base.gameObject && migrationEventArgs.prevWorldId != migrationEventArgs.targetWorldId && choreWorldMap.TryGetValue(migrationEventArgs.prevWorldId, out var value))
		{
			if (!choreWorldMap.TryGetValue(migrationEventArgs.targetWorldId, out var value2))
			{
				value2 = (choreWorldMap[migrationEventArgs.targetWorldId] = new List<Chore>());
			}
			TransferChores(value, value2, migrationEventArgs.targetWorldId);
		}
	}

	protected virtual void OnMinionMigrated(object data)
	{
		if (data is MinionMigrationEventArgs minionMigrationEventArgs && minionMigrationEventArgs.minionId.gameObject == base.gameObject && minionMigrationEventArgs.prevWorldId != minionMigrationEventArgs.targetWorldId && choreWorldMap.TryGetValue(minionMigrationEventArgs.prevWorldId, out var value))
		{
			if (!choreWorldMap.TryGetValue(minionMigrationEventArgs.targetWorldId, out var value2))
			{
				value2 = (choreWorldMap[minionMigrationEventArgs.targetWorldId] = new List<Chore>());
			}
			TransferChores(value, value2, minionMigrationEventArgs.targetWorldId);
		}
	}

	protected void TransferChores<T>(List<T> oldChores, List<T> newChores, int transferId) where T : Chore
	{
		int num = oldChores.Count - 1;
		for (int num2 = num; num2 >= 0; num2--)
		{
			T val = oldChores[num2];
			if (val.isNull)
			{
				DebugUtil.DevLogError("[" + val.GetType().Name + "] " + val.GetReportName() + " has no target");
			}
			else if (val.gameObject.GetMyParentWorldId() == transferId)
			{
				newChores.Add(val);
				oldChores[num2] = oldChores[num];
				oldChores.RemoveAt(num--);
			}
		}
	}

	protected void ClearWorldChores<T>(List<T> chores, int worldId) where T : Chore
	{
		int num = chores.Count - 1;
		for (int num2 = num; num2 >= 0; num2--)
		{
			if (chores[num2].gameObject.GetMyWorldId() == worldId)
			{
				chores[num2] = chores[num];
				chores.RemoveAt(num--);
			}
		}
	}

	public virtual void AddChore(Chore chore)
	{
		chore.provider = this;
		List<Chore> value = null;
		int myParentWorldId = chore.gameObject.GetMyParentWorldId();
		if (!choreWorldMap.TryGetValue(myParentWorldId, out value))
		{
			value = (choreWorldMap[myParentWorldId] = new List<Chore>());
		}
		value.Add(chore);
	}

	public virtual void RemoveChore(Chore chore)
	{
		if (chore != null)
		{
			chore.provider = null;
			List<Chore> value = null;
			int myParentWorldId = chore.gameObject.GetMyParentWorldId();
			if (choreWorldMap.TryGetValue(myParentWorldId, out value))
			{
				value.Remove(chore);
			}
		}
	}

	public virtual void CollectChores(ChoreConsumerState consumer_state, List<Chore.Precondition.Context> succeeded, List<Chore.Precondition.Context> failed_contexts)
	{
		List<Chore> value = null;
		int myParentWorldId = consumer_state.gameObject.GetMyParentWorldId();
		if (!choreWorldMap.TryGetValue(myParentWorldId, out value))
		{
			return;
		}
		for (int num = value.Count - 1; num >= 0; num--)
		{
			Chore chore = value[num];
			if (chore.provider == null)
			{
				chore.Cancel("no provider");
				value[num] = value[value.Count - 1];
				value.RemoveAt(value.Count - 1);
			}
			else
			{
				chore.CollectChores(consumer_state, succeeded, failed_contexts, is_attempting_override: false);
			}
		}
	}
}
