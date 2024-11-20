using System;
using Klei.Actions;
using UnityEngine;

namespace Klei.Input
{
	[ActionType("InterfaceTool", "Dig", true)]
	public abstract class DigAction
	{
		public void Uproot(int cell)
		{
			ListPool<ScenePartitionerEntry, GameScenePartitioner>.PooledList pooledList = ListPool<ScenePartitionerEntry, GameScenePartitioner>.Allocate();
			int x_bottomLeft;
			int y_bottomLeft;
			Grid.CellToXY(cell, out x_bottomLeft, out y_bottomLeft);
			GameScenePartitioner.Instance.GatherEntries(x_bottomLeft, y_bottomLeft, 1, 1, GameScenePartitioner.Instance.plants, pooledList);
			if (pooledList.Count > 0)
			{
				this.EntityDig((pooledList[0].obj as Component).GetComponent<IDigActionEntity>());
			}
			pooledList.Recycle();
		}

		public abstract void Dig(int cell, int distFromOrigin);

		protected abstract void EntityDig(IDigActionEntity digAction);
	}
}
