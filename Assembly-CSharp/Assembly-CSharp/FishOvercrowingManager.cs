using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/FishOvercrowingManager")]
public class FishOvercrowingManager : KMonoBehaviour, ISim1000ms {
    public static    FishOvercrowingManager                 Instance;
    private readonly Dictionary<int, CavityInfo>            cavityIdToCavityInfo = new Dictionary<int, CavityInfo>();
    private          Cell[]                                 cells;
    private readonly Dictionary<int, int>                   cellToFishCount = new Dictionary<int, int>();
    private readonly List<FishOvercrowdingMonitor.Instance> fishes = new List<FishOvercrowdingMonitor.Instance>();
    private          int                                    versionCounter = 1;

    public void Sim1000ms(float dt) {
        var num = versionCounter;
        versionCounter = num + 1;
        var num2 = num;
        var num3 = 1;
        cavityIdToCavityInfo.Clear();
        cellToFishCount.Clear();
        var pooledList = ListPool<FishInfo, FishOvercrowingManager>.Allocate();
        foreach (var instance in fishes) {
            var num4 = Grid.PosToCell(instance);
            if (Grid.IsValidCell(num4)) {
                var item = new FishInfo { cell = num4, fish = instance };
                pooledList.Add(item);
                var num5 = 0;
                cellToFishCount.TryGetValue(num4, out num5);
                num5++;
                cellToFishCount[num4] = num5;
            }
        }

        foreach (var fishInfo in pooledList) {
            var pooledList2 = ListPool<int, FishOvercrowingManager>.Allocate();
            pooledList2.Add(fishInfo.cell);
            var i    = 0;
            var num6 = num3++;
            while (i < pooledList2.Count) {
                var num7 = pooledList2[i++];
                if (Grid.IsValidCell(num7)) {
                    var cell = cells[num7];
                    if (cell.version != num2 && Grid.IsLiquid(num7)) {
                        cell.cavityId = num6;
                        cell.version  = num2;
                        var num8 = 0;
                        cellToFishCount.TryGetValue(num7, out num8);
                        var value                                                     = default(CavityInfo);
                        if (!cavityIdToCavityInfo.TryGetValue(num6, out value)) value = default(CavityInfo);
                        value.fishCount += num8;
                        value.cellCount++;
                        cavityIdToCavityInfo[num6] = value;
                        pooledList2.Add(Grid.CellLeft(num7));
                        pooledList2.Add(Grid.CellRight(num7));
                        pooledList2.Add(Grid.CellAbove(num7));
                        pooledList2.Add(Grid.CellBelow(num7));
                        cells[num7] = cell;
                    }
                }
            }

            pooledList2.Recycle();
        }

        foreach (var fishInfo2 in pooledList) {
            var cell2      = cells[fishInfo2.cell];
            var cavityInfo = default(CavityInfo);
            cavityIdToCavityInfo.TryGetValue(cell2.cavityId, out cavityInfo);
            fishInfo2.fish.SetOvercrowdingInfo(cavityInfo.cellCount, cavityInfo.fishCount);
        }

        pooledList.Recycle();
    }

    public static void DestroyInstance() { Instance = null; }

    protected override void OnPrefabInit() {
        Instance = this;
        cells    = new Cell[Grid.CellCount];
    }

    public void Add(FishOvercrowdingMonitor.Instance    fish) { fishes.Add(fish); }
    public void Remove(FishOvercrowdingMonitor.Instance fish) { fishes.Remove(fish); }

    private struct Cell {
        public int version;
        public int cavityId;
    }

    private struct FishInfo {
        public int                              cell;
        public FishOvercrowdingMonitor.Instance fish;
    }

    private struct CavityInfo {
        public int fishCount;
        public int cellCount;
    }
}