using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020012F7 RID: 4855
[AddComponentMenu("KMonoBehaviour/scripts/FishOvercrowingManager")]
public class FishOvercrowingManager : KMonoBehaviour, ISim1000ms
{
	// Token: 0x060063BD RID: 25533 RVA: 0x000E10AD File Offset: 0x000DF2AD
	public static void DestroyInstance()
	{
		FishOvercrowingManager.Instance = null;
	}

	// Token: 0x060063BE RID: 25534 RVA: 0x000E10B5 File Offset: 0x000DF2B5
	protected override void OnPrefabInit()
	{
		FishOvercrowingManager.Instance = this;
		this.cells = new FishOvercrowingManager.Cell[Grid.CellCount];
	}

	// Token: 0x060063BF RID: 25535 RVA: 0x000E10CD File Offset: 0x000DF2CD
	public void Add(FishOvercrowdingMonitor.Instance fish)
	{
		this.fishes.Add(fish);
	}

	// Token: 0x060063C0 RID: 25536 RVA: 0x000E10DB File Offset: 0x000DF2DB
	public void Remove(FishOvercrowdingMonitor.Instance fish)
	{
		this.fishes.Remove(fish);
	}

	// Token: 0x060063C1 RID: 25537 RVA: 0x002BC968 File Offset: 0x002BAB68
	public void Sim1000ms(float dt)
	{
		int num = this.versionCounter;
		this.versionCounter = num + 1;
		int num2 = num;
		int num3 = 1;
		this.cavityIdToCavityInfo.Clear();
		this.cellToFishCount.Clear();
		ListPool<FishOvercrowingManager.FishInfo, FishOvercrowingManager>.PooledList pooledList = ListPool<FishOvercrowingManager.FishInfo, FishOvercrowingManager>.Allocate();
		foreach (FishOvercrowdingMonitor.Instance instance in this.fishes)
		{
			int num4 = Grid.PosToCell(instance);
			if (Grid.IsValidCell(num4))
			{
				FishOvercrowingManager.FishInfo item = new FishOvercrowingManager.FishInfo
				{
					cell = num4,
					fish = instance
				};
				pooledList.Add(item);
				int num5 = 0;
				this.cellToFishCount.TryGetValue(num4, out num5);
				num5++;
				this.cellToFishCount[num4] = num5;
			}
		}
		foreach (FishOvercrowingManager.FishInfo fishInfo in pooledList)
		{
			ListPool<int, FishOvercrowingManager>.PooledList pooledList2 = ListPool<int, FishOvercrowingManager>.Allocate();
			pooledList2.Add(fishInfo.cell);
			int i = 0;
			int num6 = num3++;
			while (i < pooledList2.Count)
			{
				int num7 = pooledList2[i++];
				if (Grid.IsValidCell(num7))
				{
					FishOvercrowingManager.Cell cell = this.cells[num7];
					if (cell.version != num2 && Grid.IsLiquid(num7))
					{
						cell.cavityId = num6;
						cell.version = num2;
						int num8 = 0;
						this.cellToFishCount.TryGetValue(num7, out num8);
						FishOvercrowingManager.CavityInfo value = default(FishOvercrowingManager.CavityInfo);
						if (!this.cavityIdToCavityInfo.TryGetValue(num6, out value))
						{
							value = default(FishOvercrowingManager.CavityInfo);
						}
						value.fishCount += num8;
						value.cellCount++;
						this.cavityIdToCavityInfo[num6] = value;
						pooledList2.Add(Grid.CellLeft(num7));
						pooledList2.Add(Grid.CellRight(num7));
						pooledList2.Add(Grid.CellAbove(num7));
						pooledList2.Add(Grid.CellBelow(num7));
						this.cells[num7] = cell;
					}
				}
			}
			pooledList2.Recycle();
		}
		foreach (FishOvercrowingManager.FishInfo fishInfo2 in pooledList)
		{
			FishOvercrowingManager.Cell cell2 = this.cells[fishInfo2.cell];
			FishOvercrowingManager.CavityInfo cavityInfo = default(FishOvercrowingManager.CavityInfo);
			this.cavityIdToCavityInfo.TryGetValue(cell2.cavityId, out cavityInfo);
			fishInfo2.fish.SetOvercrowdingInfo(cavityInfo.cellCount, cavityInfo.fishCount);
		}
		pooledList.Recycle();
	}

	// Token: 0x04004740 RID: 18240
	public static FishOvercrowingManager Instance;

	// Token: 0x04004741 RID: 18241
	private List<FishOvercrowdingMonitor.Instance> fishes = new List<FishOvercrowdingMonitor.Instance>();

	// Token: 0x04004742 RID: 18242
	private Dictionary<int, FishOvercrowingManager.CavityInfo> cavityIdToCavityInfo = new Dictionary<int, FishOvercrowingManager.CavityInfo>();

	// Token: 0x04004743 RID: 18243
	private Dictionary<int, int> cellToFishCount = new Dictionary<int, int>();

	// Token: 0x04004744 RID: 18244
	private FishOvercrowingManager.Cell[] cells;

	// Token: 0x04004745 RID: 18245
	private int versionCounter = 1;

	// Token: 0x020012F8 RID: 4856
	private struct Cell
	{
		// Token: 0x04004746 RID: 18246
		public int version;

		// Token: 0x04004747 RID: 18247
		public int cavityId;
	}

	// Token: 0x020012F9 RID: 4857
	private struct FishInfo
	{
		// Token: 0x04004748 RID: 18248
		public int cell;

		// Token: 0x04004749 RID: 18249
		public FishOvercrowdingMonitor.Instance fish;
	}

	// Token: 0x020012FA RID: 4858
	private struct CavityInfo
	{
		// Token: 0x0400474A RID: 18250
		public int fishCount;

		// Token: 0x0400474B RID: 18251
		public int cellCount;
	}
}
