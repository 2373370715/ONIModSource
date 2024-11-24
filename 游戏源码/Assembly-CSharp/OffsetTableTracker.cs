using System;

// Token: 0x0200167B RID: 5755
public class OffsetTableTracker : OffsetTracker
{
	// Token: 0x1700077F RID: 1919
	// (get) Token: 0x060076E5 RID: 30437 RVA: 0x000EE1A6 File Offset: 0x000EC3A6
	private static NavGrid navGrid
	{
		get
		{
			if (OffsetTableTracker.navGridImpl == null)
			{
				OffsetTableTracker.navGridImpl = Pathfinding.Instance.GetNavGrid("MinionNavGrid");
			}
			return OffsetTableTracker.navGridImpl;
		}
	}

	// Token: 0x060076E6 RID: 30438 RVA: 0x000EE1C8 File Offset: 0x000EC3C8
	public OffsetTableTracker(CellOffset[][] table, KMonoBehaviour cmp)
	{
		this.table = table;
		this.cmp = cmp;
	}

	// Token: 0x060076E7 RID: 30439 RVA: 0x0030C16C File Offset: 0x0030A36C
	protected override void UpdateCell(int previous_cell, int current_cell)
	{
		if (previous_cell == current_cell)
		{
			return;
		}
		base.UpdateCell(previous_cell, current_cell);
		Extents extents = new Extents(current_cell, this.table);
		extents.height += 2;
		extents.y--;
		if (!this.solidPartitionerEntry.IsValid())
		{
			this.solidPartitionerEntry = GameScenePartitioner.Instance.Add("OffsetTableTracker.UpdateCell", this.cmp.gameObject, extents, GameScenePartitioner.Instance.solidChangedLayer, new Action<object>(this.OnCellChanged));
			this.validNavCellChangedPartitionerEntry = GameScenePartitioner.Instance.Add("OffsetTableTracker.UpdateCell", this.cmp.gameObject, extents, GameScenePartitioner.Instance.validNavCellChangedLayer, new Action<object>(this.OnCellChanged));
		}
		else
		{
			GameScenePartitioner.Instance.UpdatePosition(this.solidPartitionerEntry, extents);
			GameScenePartitioner.Instance.UpdatePosition(this.validNavCellChangedPartitionerEntry, extents);
		}
		this.offsets = null;
	}

	// Token: 0x060076E8 RID: 30440 RVA: 0x0030C254 File Offset: 0x0030A454
	private static bool IsValidRow(int current_cell, CellOffset[] row, int rowIdx, int[] debugIdxs)
	{
		for (int i = 1; i < row.Length; i++)
		{
			int num = Grid.OffsetCell(current_cell, row[i]);
			if (!Grid.IsValidCell(num))
			{
				return false;
			}
			if (Grid.Solid[num])
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x060076E9 RID: 30441 RVA: 0x0030C298 File Offset: 0x0030A498
	private void UpdateOffsets(int cell, CellOffset[][] table)
	{
		HashSetPool<CellOffset, OffsetTableTracker>.PooledHashSet pooledHashSet = HashSetPool<CellOffset, OffsetTableTracker>.Allocate();
		if (Grid.IsValidCell(cell))
		{
			for (int i = 0; i < table.Length; i++)
			{
				CellOffset[] array = table[i];
				if (!pooledHashSet.Contains(array[0]))
				{
					int cell2 = Grid.OffsetCell(cell, array[0]);
					for (int j = 0; j < OffsetTableTracker.navGrid.ValidNavTypes.Length; j++)
					{
						NavType navType = OffsetTableTracker.navGrid.ValidNavTypes[j];
						if (navType != NavType.Tube && OffsetTableTracker.navGrid.NavTable.IsValid(cell2, navType) && OffsetTableTracker.IsValidRow(cell, array, i, this.DEBUG_rowValidIdx))
						{
							pooledHashSet.Add(array[0]);
							break;
						}
					}
				}
			}
		}
		if (this.offsets == null || this.offsets.Length != pooledHashSet.Count)
		{
			this.offsets = new CellOffset[pooledHashSet.Count];
		}
		pooledHashSet.CopyTo(this.offsets);
		pooledHashSet.Recycle();
	}

	// Token: 0x060076EA RID: 30442 RVA: 0x000EE1DE File Offset: 0x000EC3DE
	protected override void UpdateOffsets(int current_cell)
	{
		base.UpdateOffsets(current_cell);
		this.UpdateOffsets(current_cell, this.table);
	}

	// Token: 0x060076EB RID: 30443 RVA: 0x000EE1F4 File Offset: 0x000EC3F4
	private void OnCellChanged(object data)
	{
		this.offsets = null;
	}

	// Token: 0x060076EC RID: 30444 RVA: 0x000EE1FD File Offset: 0x000EC3FD
	public override void Clear()
	{
		GameScenePartitioner.Instance.Free(ref this.solidPartitionerEntry);
		GameScenePartitioner.Instance.Free(ref this.validNavCellChangedPartitionerEntry);
	}

	// Token: 0x060076ED RID: 30445 RVA: 0x000EE21F File Offset: 0x000EC41F
	public static void OnPathfindingInvalidated()
	{
		OffsetTableTracker.navGridImpl = null;
	}

	// Token: 0x040058F1 RID: 22769
	private readonly CellOffset[][] table;

	// Token: 0x040058F2 RID: 22770
	public HandleVector<int>.Handle solidPartitionerEntry;

	// Token: 0x040058F3 RID: 22771
	public HandleVector<int>.Handle validNavCellChangedPartitionerEntry;

	// Token: 0x040058F4 RID: 22772
	private static NavGrid navGridImpl;

	// Token: 0x040058F5 RID: 22773
	private KMonoBehaviour cmp;

	// Token: 0x040058F6 RID: 22774
	private int[] DEBUG_rowValidIdx;
}
