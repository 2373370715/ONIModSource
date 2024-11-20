using System;
using System.Collections.Generic;

public static class OffsetGroups {
    public static CellOffset[] Use = new CellOffset[1];

    public static CellOffset[] Chat = {
        new CellOffset(1,  0),
        new CellOffset(-1, 0),
        new CellOffset(1,  1),
        new CellOffset(1,  -1),
        new CellOffset(-1, 1),
        new CellOffset(-1, -1)
    };

    public static CellOffset[] LeftOnly    = { new CellOffset(-1, 0) };
    public static CellOffset[] RightOnly   = { new CellOffset(1,  0) };
    public static CellOffset[] LeftOrRight = { new CellOffset(-1, 0), new CellOffset(1, 0) };
    public static CellOffset[] Standard    = InitGrid(-2, 2, -3, 3);

    public static CellOffset[] LiquidSource = {
        new CellOffset(0,  0),
        new CellOffset(1,  0),
        new CellOffset(-1, 0),
        new CellOffset(0,  1),
        new CellOffset(0,  -1),
        new CellOffset(1,  1),
        new CellOffset(1,  -1),
        new CellOffset(-1, 1),
        new CellOffset(-1, -1),
        new CellOffset(2,  0),
        new CellOffset(-2, 0)
    };

    public static CellOffset[][] InvertedStandardTable = OffsetTable.Mirror(new[] {
        new[] { new CellOffset(0, 0) },
        new[] { new CellOffset(0, 1) },
        new[] { new CellOffset(0, 2), new CellOffset(0, 1) },
        new[] { new CellOffset(0, 3), new CellOffset(0, 2), new CellOffset(0, 1) },
        new[] { new CellOffset(0, -1) },
        new[] { new CellOffset(0, -2) },
        new[] { new CellOffset(0, -3), new CellOffset(0, -2), new CellOffset(0, -1) },
        new[] { new CellOffset(1, 0) },
        new[] { new CellOffset(1, 1), new CellOffset(0, 1) },
        new[] { new CellOffset(1, 1), new CellOffset(1, 0) },
        new[] { new CellOffset(1, 2), new CellOffset(1, 1), new CellOffset(1, 0) },
        new[] { new CellOffset(1, 2), new CellOffset(0, 2), new CellOffset(0, 1) },
        new[] { new CellOffset(1, 3), new CellOffset(1, 2), new CellOffset(1, 1), new CellOffset(0, 1) },
        new[] { new CellOffset(1, 3), new CellOffset(0, 3), new CellOffset(0, 2), new CellOffset(0, 1) },
        new[] { new CellOffset(1, -1) },
        new[] { new CellOffset(1, -2), new CellOffset(1, -1), new CellOffset(1, 0) },
        new[] { new CellOffset(1, -2), new CellOffset(1, -1), new CellOffset(0, -1) },
        new[] { new CellOffset(1, -3), new CellOffset(1, -2), new CellOffset(1, -1), new CellOffset(1, 0) },
        new[] { new CellOffset(1, -3), new CellOffset(1, -2), new CellOffset(0, -2), new CellOffset(0, -1) },
        new[] { new CellOffset(2, 0), new CellOffset(1,  0) },
        new[] { new CellOffset(2, 1), new CellOffset(1,  1), new CellOffset(0, 1) },
        new[] { new CellOffset(2, 1), new CellOffset(1,  1), new CellOffset(1, 0) },
        new[] { new CellOffset(2, 2), new CellOffset(1,  2), new CellOffset(1, 1), new CellOffset(0, 1) },
        new[] { new CellOffset(2, 2), new CellOffset(1,  2), new CellOffset(1, 1), new CellOffset(1, 0) },
        new[] {
            new CellOffset(2, 3), new CellOffset(1, 3), new CellOffset(1, 2), new CellOffset(1, 1), new CellOffset(0, 1)
        },
        new[] { new CellOffset(2, -1), new CellOffset(2, 0), new CellOffset(1,  0) },
        new[] { new CellOffset(2, -2), new CellOffset(2, -1), new CellOffset(1, -1), new CellOffset(1, 0) },
        new[] { new CellOffset(2, -3), new CellOffset(1, -2), new CellOffset(1, -1), new CellOffset(1, 0) }
    });

    public static CellOffset[][] InvertedStandardTableWithCorners = OffsetTable.Mirror(new[] {
        new[] { new CellOffset(0, 0) },
        new[] { new CellOffset(0, 1) },
        new[] { new CellOffset(0, 2), new CellOffset(0, 1) },
        new[] { new CellOffset(0, 3), new CellOffset(0, 2), new CellOffset(0, 1) },
        new[] { new CellOffset(0, -1) },
        new[] { new CellOffset(0, -2) },
        new[] { new CellOffset(0, -3), new CellOffset(0, -2), new CellOffset(0, -1) },
        new[] { new CellOffset(1, 0) },
        new[] { new CellOffset(1, 1) },
        new[] { new CellOffset(1, 2), new CellOffset(1, 1) },
        new[] { new CellOffset(1, 2), new CellOffset(0, 2), new CellOffset(0, 1) },
        new[] { new CellOffset(1, 3), new CellOffset(1, 2), new CellOffset(1, 1) },
        new[] { new CellOffset(1, 3), new CellOffset(0, 3), new CellOffset(0, 2), new CellOffset(0, 1) },
        new[] { new CellOffset(1, -1) },
        new[] { new CellOffset(1, -2), new CellOffset(1, -1) },
        new[] { new CellOffset(1, -3), new CellOffset(1, -2), new CellOffset(0, -2), new CellOffset(0, -1) },
        new[] { new CellOffset(1, -3), new CellOffset(1, -2), new CellOffset(1, -1) },
        new[] { new CellOffset(2, 0), new CellOffset(1,  0) },
        new[] { new CellOffset(2, 1), new CellOffset(1,  1) },
        new[] { new CellOffset(2, 2), new CellOffset(1,  2), new CellOffset(1,  1) },
        new[] { new CellOffset(2, 3), new CellOffset(1,  3), new CellOffset(1,  2), new CellOffset(1, 1) },
        new[] { new CellOffset(2, -1), new CellOffset(2, 0), new CellOffset(1,  0) },
        new[] { new CellOffset(2, -2), new CellOffset(2, -1), new CellOffset(1, -1) },
        new[] { new CellOffset(2, -3), new CellOffset(1, -2), new CellOffset(1, -1) }
    });

    public static CellOffset[][] InvertedWideTable = OffsetTable.Mirror(new[] {
        new[] { new CellOffset(0, 0) },
        new[] { new CellOffset(0, 1) },
        new[] { new CellOffset(0, 2), new CellOffset(0, 1) },
        new[] { new CellOffset(0, 3), new CellOffset(0, 2), new CellOffset(0, 1) },
        new[] { new CellOffset(0, -1) },
        new[] { new CellOffset(0, -2) },
        new[] { new CellOffset(0, -3), new CellOffset(0, -2), new CellOffset(0, -1) },
        new[] { new CellOffset(1, 0) },
        new[] { new CellOffset(1, 1), new CellOffset(0, 1) },
        new[] { new CellOffset(1, 1), new CellOffset(1, 0) },
        new[] { new CellOffset(1, 2), new CellOffset(1, 1), new CellOffset(1, 0) },
        new[] { new CellOffset(1, 2), new CellOffset(0, 2), new CellOffset(0, 1) },
        new[] { new CellOffset(1, 3), new CellOffset(1, 2), new CellOffset(1, 1), new CellOffset(0, 1) },
        new[] { new CellOffset(1, 3), new CellOffset(0, 3), new CellOffset(0, 2), new CellOffset(0, 1) },
        new[] { new CellOffset(1, -1) },
        new[] { new CellOffset(1, -2), new CellOffset(1, -1), new CellOffset(1, 0) },
        new[] { new CellOffset(1, -2), new CellOffset(1, -1), new CellOffset(0, -1) },
        new[] { new CellOffset(1, -3), new CellOffset(1, -2), new CellOffset(1, -1), new CellOffset(1, 0) },
        new[] { new CellOffset(1, -3), new CellOffset(1, -2), new CellOffset(0, -2), new CellOffset(0, -1) },
        new[] { new CellOffset(2, 0), new CellOffset(1,  0) },
        new[] { new CellOffset(2, 1), new CellOffset(1,  1), new CellOffset(0, 1) },
        new[] { new CellOffset(2, 1), new CellOffset(1,  1), new CellOffset(1, 0) },
        new[] { new CellOffset(2, 2), new CellOffset(1,  2), new CellOffset(1, 1), new CellOffset(0, 1) },
        new[] { new CellOffset(2, 2), new CellOffset(1,  2), new CellOffset(1, 1), new CellOffset(1, 0) },
        new[] {
            new CellOffset(2, 3), new CellOffset(1, 3), new CellOffset(1, 2), new CellOffset(1, 1), new CellOffset(0, 1)
        },
        new[] { new CellOffset(2, -1), new CellOffset(2, 0), new CellOffset(1,  0) },
        new[] { new CellOffset(2, -2), new CellOffset(2, -1), new CellOffset(1, -1), new CellOffset(1, 0) },
        new[] { new CellOffset(2, -3), new CellOffset(1, -2), new CellOffset(1, -1), new CellOffset(1, 0) },
        new[] { new CellOffset(3, 0), new CellOffset(2,  0), new CellOffset(1,  0) },
        new[] { new CellOffset(3, 1), new CellOffset(2,  1), new CellOffset(1,  1), new CellOffset(0,  1) },
        new[] { new CellOffset(3, 1), new CellOffset(2,  1), new CellOffset(1,  1), new CellOffset(1,  0) },
        new[] { new CellOffset(3, -1), new CellOffset(2, -1), new CellOffset(1, -1), new CellOffset(0, -1) },
        new[] { new CellOffset(3, -1), new CellOffset(2, -1), new CellOffset(1, -1), new CellOffset(1, 0) }
    });

    private static readonly
        Dictionary<CellOffset[], Dictionary<CellOffset[][], Dictionary<CellOffset[], CellOffset[][]>>>
        reachabilityTableCache
            = new Dictionary<CellOffset[], Dictionary<CellOffset[][], Dictionary<CellOffset[], CellOffset[][]>>>();

    private static readonly CellOffset[] nullFilter = new CellOffset[0];

    public static CellOffset[] InitGrid(int x0, int x1, int y0, int y1) {
        var list = new List<CellOffset>();
        for (var i = y0; i <= y1; i++) {
            for (var j = x0; j <= x1; j++) list.Add(new CellOffset(j, i));
        }

        var array = list.ToArray();
        Array.Sort(array, 0, array.Length, new CellOffsetComparer());
        return array;
    }

    public static CellOffset[][] BuildReachabilityTable(CellOffset[]   area_offsets,
                                                        CellOffset[][] table,
                                                        CellOffset[]   filter) {
        Dictionary<CellOffset[][], Dictionary<CellOffset[], CellOffset[][]>> dictionary  = null;
        Dictionary<CellOffset[], CellOffset[][]>                             dictionary2 = null;
        CellOffset[][]                                                       array       = null;
        if (reachabilityTableCache.TryGetValue(area_offsets, out dictionary) &&
            dictionary.TryGetValue(table, out dictionary2)                   &&
            dictionary2.TryGetValue(filter == null ? nullFilter : filter, out array))
            return array;

        var hashSet = new HashSet<CellOffset>();
        foreach (var a in area_offsets) {
            foreach (var array2 in table)
                if (filter == null || Array.IndexOf(filter, array2[0]) == -1) {
                    var item = a + array2[0];
                    hashSet.Add(item);
                }
        }

        var list = new List<CellOffset[]>();
        foreach (var cellOffset in hashSet) {
            var b = area_offsets[0];
            foreach (var cellOffset2 in area_offsets)
                if ((cellOffset - b).GetOffsetDistance() > (cellOffset - cellOffset2).GetOffsetDistance())
                    b = cellOffset2;

            foreach (var array3 in table)
                if ((filter == null || Array.IndexOf(filter, array3[0]) == -1) && array3[0] + b == cellOffset) {
                    var array4                                        = new CellOffset[array3.Length];
                    for (var k = 0; k < array3.Length; k++) array4[k] = array3[k] + b;
                    list.Add(array4);
                }
        }

        array = list.ToArray();
        Array.Sort(array, (x, y) => x[0].GetOffsetDistance().CompareTo(y[0].GetOffsetDistance()));
        if (dictionary == null) {
            dictionary = new Dictionary<CellOffset[][], Dictionary<CellOffset[], CellOffset[][]>>();
            reachabilityTableCache.Add(area_offsets, dictionary);
        }

        if (dictionary2 == null) {
            dictionary2 = new Dictionary<CellOffset[], CellOffset[][]>();
            dictionary.Add(table, dictionary2);
        }

        dictionary2.Add(filter == null ? nullFilter : filter, array);
        return array;
    }

    private class CellOffsetComparer : IComparer<CellOffset> {
        public int Compare(CellOffset a, CellOffset b) {
            var num   = Math.Abs(a.x) + Math.Abs(a.y);
            var value = Math.Abs(b.x) + Math.Abs(b.y);
            return num.CompareTo(value);
        }
    }
}