using Klei.Actions;
using UnityEngine;

namespace Klei.Input;

[ActionType("InterfaceTool", "Dig", true)]
public abstract class DigAction
{
	public void Uproot(int cell)
	{
		ListPool<ScenePartitionerEntry, GameScenePartitioner>.PooledList pooledList = ListPool<ScenePartitionerEntry, GameScenePartitioner>.Allocate();
		Grid.CellToXY(cell, out var x, out var y);
		GameScenePartitioner.Instance.GatherEntries(x, y, 1, 1, GameScenePartitioner.Instance.plants, pooledList);
		if (pooledList.Count > 0)
		{
			EntityDig((pooledList[0].obj as Component).GetComponent<IDigActionEntity>());
		}
		pooledList.Recycle();
	}

	public abstract void Dig(int cell, int distFromOrigin);

	protected abstract void EntityDig(IDigActionEntity digAction);
}
