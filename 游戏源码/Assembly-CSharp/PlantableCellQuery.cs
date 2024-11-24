using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000800 RID: 2048
public class PlantableCellQuery : PathFinderQuery
{
	// Token: 0x06002497 RID: 9367 RVA: 0x000B7D45 File Offset: 0x000B5F45
	public PlantableCellQuery Reset(PlantableSeed seed, int max_results)
	{
		this.seed = seed;
		this.max_results = max_results;
		this.result_cells.Clear();
		return this;
	}

	// Token: 0x06002498 RID: 9368 RVA: 0x001C9EF4 File Offset: 0x001C80F4
	public override bool IsMatch(int cell, int parent_cell, int cost)
	{
		if (!this.result_cells.Contains(cell) && this.CheckValidPlotCell(this.seed, cell))
		{
			this.result_cells.Add(cell);
		}
		return this.result_cells.Count >= this.max_results;
	}

	// Token: 0x06002499 RID: 9369 RVA: 0x001C9F40 File Offset: 0x001C8140
	private bool CheckValidPlotCell(PlantableSeed seed, int plant_cell)
	{
		if (!Grid.IsValidCell(plant_cell))
		{
			return false;
		}
		int num;
		if (seed.Direction == SingleEntityReceptacle.ReceptacleDirection.Bottom)
		{
			num = Grid.CellAbove(plant_cell);
		}
		else
		{
			num = Grid.CellBelow(plant_cell);
		}
		if (!Grid.IsValidCell(num))
		{
			return false;
		}
		if (!Grid.Solid[num])
		{
			return false;
		}
		if (Grid.Objects[plant_cell, 5])
		{
			return false;
		}
		if (Grid.Objects[plant_cell, 1])
		{
			return false;
		}
		GameObject gameObject = Grid.Objects[num, 1];
		if (gameObject)
		{
			PlantablePlot component = gameObject.GetComponent<PlantablePlot>();
			if (component == null)
			{
				return false;
			}
			if (component.Direction != seed.Direction)
			{
				return false;
			}
			if (component.Occupant != null)
			{
				return false;
			}
		}
		else
		{
			if (!seed.TestSuitableGround(plant_cell))
			{
				return false;
			}
			if (PlantableCellQuery.CountNearbyPlants(num, this.plantDetectionRadius) > this.maxPlantsInRadius)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x0600249A RID: 9370 RVA: 0x001CA01C File Offset: 0x001C821C
	private static int CountNearbyPlants(int cell, int radius)
	{
		int num = 0;
		int num2 = 0;
		Grid.PosToXY(Grid.CellToPos(cell), out num, out num2);
		int num3 = radius * 2;
		num -= radius;
		num2 -= radius;
		ListPool<ScenePartitionerEntry, GameScenePartitioner>.PooledList pooledList = ListPool<ScenePartitionerEntry, GameScenePartitioner>.Allocate();
		GameScenePartitioner.Instance.GatherEntries(num, num2, num3, num3, GameScenePartitioner.Instance.plants, pooledList);
		int num4 = 0;
		using (List<ScenePartitionerEntry>.Enumerator enumerator = pooledList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (!((KPrefabID)enumerator.Current.obj).GetComponent<TreeBud>())
				{
					num4++;
				}
			}
		}
		pooledList.Recycle();
		return num4;
	}

	// Token: 0x0400189E RID: 6302
	public List<int> result_cells = new List<int>();

	// Token: 0x0400189F RID: 6303
	private PlantableSeed seed;

	// Token: 0x040018A0 RID: 6304
	private int max_results;

	// Token: 0x040018A1 RID: 6305
	private int plantDetectionRadius = 6;

	// Token: 0x040018A2 RID: 6306
	private int maxPlantsInRadius = 2;
}
