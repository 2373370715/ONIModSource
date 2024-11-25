using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public readonly struct SkyVisibilityInfo {
    public SkyVisibilityInfo(CellOffset scanLeftOffset,
                             int        scanLeftCount,
                             CellOffset scanRightOffset,
                             int        scanRightCount,
                             int        verticalStep) {
        this.scanLeftOffset  = scanLeftOffset;
        this.scanLeftCount   = scanLeftCount;
        this.scanRightOffset = scanRightOffset;
        this.scanRightCount  = scanRightCount;
        this.verticalStep    = verticalStep;
        totalColumnsCount    = scanLeftCount + scanRightCount + (scanRightOffset.x - scanLeftOffset.x) + 1;
    }

    [return: TupleElementNames(new[] { "isAnyVisible", "percentVisible01" })]
    public ValueTuple<bool, float> GetVisibilityOf(GameObject gameObject) {
        return GetVisibilityOf(Grid.PosToCell(gameObject));
    }

    [return: TupleElementNames(new[] { "isAnyVisible", "percentVisible01" })]
    public ValueTuple<bool, float> GetVisibilityOf(int buildingCenterCellId) {
        var num   = 0;
        var world = ClusterManager.Instance.GetWorld(Grid.WorldIdx[buildingCenterCellId]);
        num += ScanAndGetVisibleCellCount(Grid.OffsetCell(buildingCenterCellId, scanLeftOffset),
                                          -1,
                                          verticalStep,
                                          scanLeftCount,
                                          world);

        num += ScanAndGetVisibleCellCount(Grid.OffsetCell(buildingCenterCellId, scanRightOffset),
                                          1,
                                          verticalStep,
                                          scanRightCount,
                                          world);

        if (scanLeftOffset.x == scanRightOffset.x) num = Mathf.Max(0, num - 1);
        return new ValueTuple<bool, float>(num > 0, num / (float)totalColumnsCount);
    }

    public void CollectVisibleCellsTo(HashSet<int>   visibleCells,
                                      int            buildingBottomLeftCellId,
                                      WorldContainer originWorld) {
        ScanAndCollectVisibleCellsTo(visibleCells,
                                     Grid.OffsetCell(buildingBottomLeftCellId, scanLeftOffset),
                                     -1,
                                     verticalStep,
                                     scanLeftCount,
                                     originWorld);

        ScanAndCollectVisibleCellsTo(visibleCells,
                                     Grid.OffsetCell(buildingBottomLeftCellId, scanRightOffset),
                                     1,
                                     verticalStep,
                                     scanRightCount,
                                     originWorld);
    }

    private static void ScanAndCollectVisibleCellsTo(HashSet<int>   visibleCells,
                                                     int            originCellId,
                                                     int            stepX,
                                                     int            stepY,
                                                     int            stepCountInclusive,
                                                     WorldContainer originWorld) {
        for (var i = 0; i <= stepCountInclusive; i++) {
            var num = Grid.OffsetCell(originCellId, i * stepX, i * stepY);
            if (!IsVisible(num, originWorld)) break;

            visibleCells.Add(num);
        }
    }

    private static int ScanAndGetVisibleCellCount(int            originCellId,
                                                  int            stepX,
                                                  int            stepY,
                                                  int            stepCountInclusive,
                                                  WorldContainer originWorld) {
        for (var i = 0; i <= stepCountInclusive; i++)
            if (!IsVisible(Grid.OffsetCell(originCellId, i * stepX, i * stepY), originWorld))
                return i;

        return stepCountInclusive + 1;
    }

    public static bool IsVisible(int cellId, WorldContainer originWorld) {
        if (!Grid.IsValidCell(cellId)) return false;

        if (Grid.ExposedToSunlight[cellId] > 0) return true;

        var world = ClusterManager.Instance.GetWorld(Grid.WorldIdx[cellId]);
        if (world != null && world.IsModuleInterior) return true;

        originWorld != world;
        return false;
    }

    public readonly CellOffset scanLeftOffset;
    public readonly int        scanLeftCount;
    public readonly CellOffset scanRightOffset;
    public readonly int        scanRightCount;
    public readonly int        verticalStep;
    public readonly int        totalColumnsCount;
}