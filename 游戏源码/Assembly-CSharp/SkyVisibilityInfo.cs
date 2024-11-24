using System.Collections.Generic;
using UnityEngine;

public readonly struct SkyVisibilityInfo
{
	public readonly CellOffset scanLeftOffset;

	public readonly int scanLeftCount;

	public readonly CellOffset scanRightOffset;

	public readonly int scanRightCount;

	public readonly int verticalStep;

	public readonly int totalColumnsCount;

	public SkyVisibilityInfo(CellOffset scanLeftOffset, int scanLeftCount, CellOffset scanRightOffset, int scanRightCount, int verticalStep)
	{
		this.scanLeftOffset = scanLeftOffset;
		this.scanLeftCount = scanLeftCount;
		this.scanRightOffset = scanRightOffset;
		this.scanRightCount = scanRightCount;
		this.verticalStep = verticalStep;
		totalColumnsCount = scanLeftCount + scanRightCount + (scanRightOffset.x - scanLeftOffset.x + 1);
	}

	public (bool isAnyVisible, float percentVisible01) GetVisibilityOf(GameObject gameObject)
	{
		return GetVisibilityOf(Grid.PosToCell(gameObject));
	}

	public (bool isAnyVisible, float percentVisible01) GetVisibilityOf(int buildingCenterCellId)
	{
		int num = 0;
		WorldContainer world = ClusterManager.Instance.GetWorld(Grid.WorldIdx[buildingCenterCellId]);
		num += ScanAndGetVisibleCellCount(Grid.OffsetCell(buildingCenterCellId, scanLeftOffset), -1, verticalStep, scanLeftCount, world);
		num += ScanAndGetVisibleCellCount(Grid.OffsetCell(buildingCenterCellId, scanRightOffset), 1, verticalStep, scanRightCount, world);
		if (scanLeftOffset.x == scanRightOffset.x)
		{
			num = Mathf.Max(0, num - 1);
		}
		return (isAnyVisible: num > 0, percentVisible01: (float)num / (float)totalColumnsCount);
	}

	public void CollectVisibleCellsTo(HashSet<int> visibleCells, int buildingBottomLeftCellId, WorldContainer originWorld)
	{
		ScanAndCollectVisibleCellsTo(visibleCells, Grid.OffsetCell(buildingBottomLeftCellId, scanLeftOffset), -1, verticalStep, scanLeftCount, originWorld);
		ScanAndCollectVisibleCellsTo(visibleCells, Grid.OffsetCell(buildingBottomLeftCellId, scanRightOffset), 1, verticalStep, scanRightCount, originWorld);
	}

	private static void ScanAndCollectVisibleCellsTo(HashSet<int> visibleCells, int originCellId, int stepX, int stepY, int stepCountInclusive, WorldContainer originWorld)
	{
		for (int i = 0; i <= stepCountInclusive; i++)
		{
			int num = Grid.OffsetCell(originCellId, i * stepX, i * stepY);
			if (IsVisible(num, originWorld))
			{
				visibleCells.Add(num);
				continue;
			}
			break;
		}
	}

	private static int ScanAndGetVisibleCellCount(int originCellId, int stepX, int stepY, int stepCountInclusive, WorldContainer originWorld)
	{
		for (int i = 0; i <= stepCountInclusive; i++)
		{
			if (!IsVisible(Grid.OffsetCell(originCellId, i * stepX, i * stepY), originWorld))
			{
				return i;
			}
		}
		return stepCountInclusive + 1;
	}

	public static bool IsVisible(int cellId, WorldContainer originWorld)
	{
		if (!Grid.IsValidCell(cellId))
		{
			return false;
		}
		if (Grid.ExposedToSunlight[cellId] > 0)
		{
			return true;
		}
		WorldContainer world = ClusterManager.Instance.GetWorld(Grid.WorldIdx[cellId]);
		if (world != null && world.IsModuleInterior)
		{
			return true;
		}
		_ = originWorld != world;
		return false;
	}
}
