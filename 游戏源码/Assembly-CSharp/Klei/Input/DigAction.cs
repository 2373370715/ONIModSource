using System;
using Klei.Actions;
using UnityEngine;

namespace Klei.Input
{
	// Token: 0x02003BA7 RID: 15271
	[ActionType("InterfaceTool", "Dig", true)]
	public abstract class DigAction
	{
		// Token: 0x0600EB4D RID: 60237 RVA: 0x004CCC78 File Offset: 0x004CAE78
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

		// Token: 0x0600EB4E RID: 60238
		public abstract void Dig(int cell, int distFromOrigin);

		// Token: 0x0600EB4F RID: 60239
		protected abstract void EntityDig(IDigActionEntity digAction);
	}
}
