using System.Linq;
using HarmonyLib;

#if 挖掘范围
public class Patches {
    private static readonly CellOffset[][] TableExpansion = {
        new[] { O(-5, -8) },
        new[] { O(-5, -7) },
        new[] { O(-5, -6) },
        new[] { O(-5, -5) },
        new[] { O(-5, -4) },
        new[] { O(-5, -3) },
        new[] { O(-5, -2) },
        new[] { O(-5, -1) },
        new[] { O(-5, 0) },
        new[] { O(-5, 1) },
        new[] { O(-5, 2) },
        new[] { O(-5, 3) },
        new[] { O(-5, 4) },
        new[] { O(-5, 5) },
        new[] { O(-5, 6) },
        new[] { O(-5, 7) },
        new[] { O(-5, 8) },
        new[] { O(-4, -8) },
        new[] { O(-4, -7) },
        new[] { O(-4, -6) },
        new[] { O(-4, -5) },
        new[] { O(-4, -4) },
        new[] { O(-4, -3) },
        new[] { O(-4, -2) },
        new[] { O(-4, -1) },
        new[] { O(-4, 0) },
        new[] { O(-4, 1) },
        new[] { O(-4, 2) },
        new[] { O(-4, 3) },
        new[] { O(-4, 4) },
        new[] { O(-4, 5) },
        new[] { O(-4, 6) },
        new[] { O(-4, 7) },
        new[] { O(-4, 8) },
        new[] { O(-3, -8) },
        new[] { O(-3, -7) },
        new[] { O(-3, -6) },
        new[] { O(-3, -5) },
        new[] { O(-3, -4) },
        new[] { O(-3, -3) },
        new[] { O(-3, -2) },
        new[] { O(-3, -1) },
        new[] { O(-3, 0) },
        new[] { O(-3, 1) },
        new[] { O(-3, 2) },
        new[] { O(-3, 3) },
        new[] { O(-3, 4) },
        new[] { O(-3, 5) },
        new[] { O(-3, 6) },
        new[] { O(-3, 7) },
        new[] { O(-3, 8) },
        new[] { O(-2, -8) },
        new[] { O(-2, -7) },
        new[] { O(-2, -6) },
        new[] { O(-2, -5) },
        new[] { O(-2, -4) },
        new[] { O(-2, -3) },
        new[] { O(-2, -2) },
        new[] { O(-2, -1) },
        new[] { O(-2, 0) },
        new[] { O(-2, 1) },
        new[] { O(-2, 2) },
        new[] { O(-2, 3) },
        new[] { O(-2, 4) },
        new[] { O(-2, 5) },
        new[] { O(-2, 6) },
        new[] { O(-2, 7) },
        new[] { O(-2, 8) },
        new[] { O(-1, -8) },
        new[] { O(-1, -7) },
        new[] { O(-1, -6) },
        new[] { O(-1, -5) },
        new[] { O(-1, -4) },
        new[] { O(-1, -3) },
        new[] { O(-1, -2) },
        new[] { O(-1, -1) },
        new[] { O(-1, 0) },
        new[] { O(-1, 1) },
        new[] { O(-1, 2) },
        new[] { O(-1, 3) },
        new[] { O(-1, 4) },
        new[] { O(-1, 5) },
        new[] { O(-1, 6) },
        new[] { O(-1, 7) },
        new[] { O(-1, 8) },
        new[] { O(0,  -8) },
        new[] { O(0,  -7) },
        new[] { O(0,  -6) },
        new[] { O(0,  -5) },
        new[] { O(0,  -4) },
        new[] { O(0,  -3) },
        new[] { O(0,  -2) },
        new[] { O(0,  -1) },
        new[] { O(0,  0) },
        new[] { O(0,  1) },
        new[] { O(0,  2) },
        new[] { O(0,  3) },
        new[] { O(0,  4) },
        new[] { O(0,  5) },
        new[] { O(0,  6) },
        new[] { O(0,  7) },
        new[] { O(0,  8) },
        new[] { O(1,  -8) },
        new[] { O(1,  -7) },
        new[] { O(1,  -6) },
        new[] { O(1,  -5) },
        new[] { O(1,  -4) },
        new[] { O(1,  -3) },
        new[] { O(1,  -2) },
        new[] { O(1,  -1) },
        new[] { O(1,  0) },
        new[] { O(1,  1) },
        new[] { O(1,  2) },
        new[] { O(1,  3) },
        new[] { O(1,  4) },
        new[] { O(1,  5) },
        new[] { O(1,  6) },
        new[] { O(1,  7) },
        new[] { O(1,  8) },
        new[] { O(2,  -8) },
        new[] { O(2,  -7) },
        new[] { O(2,  -6) },
        new[] { O(2,  -5) },
        new[] { O(2,  -4) },
        new[] { O(2,  -3) },
        new[] { O(2,  -2) },
        new[] { O(2,  -1) },
        new[] { O(2,  0) },
        new[] { O(2,  1) },
        new[] { O(2,  2) },
        new[] { O(2,  3) },
        new[] { O(2,  4) },
        new[] { O(2,  5) },
        new[] { O(2,  6) },
        new[] { O(2,  7) },
        new[] { O(2,  8) },
        new[] { O(3,  -8) },
        new[] { O(3,  -7) },
        new[] { O(3,  -6) },
        new[] { O(3,  -5) },
        new[] { O(3,  -4) },
        new[] { O(3,  -3) },
        new[] { O(3,  -2) },
        new[] { O(3,  -1) },
        new[] { O(3,  0) },
        new[] { O(3,  1) },
        new[] { O(3,  2) },
        new[] { O(3,  3) },
        new[] { O(3,  4) },
        new[] { O(3,  5) },
        new[] { O(3,  6) },
        new[] { O(3,  7) },
        new[] { O(3,  8) },
        new[] { O(4,  -8) },
        new[] { O(4,  -7) },
        new[] { O(4,  -6) },
        new[] { O(4,  -5) },
        new[] { O(4,  -4) },
        new[] { O(4,  -3) },
        new[] { O(4,  -2) },
        new[] { O(4,  -1) },
        new[] { O(4,  0) },
        new[] { O(4,  1) },
        new[] { O(4,  2) },
        new[] { O(4,  3) },
        new[] { O(4,  4) },
        new[] { O(4,  5) },
        new[] { O(4,  6) },
        new[] { O(4,  7) },
        new[] { O(4,  8) },
        new[] { O(5,  -8) },
        new[] { O(5,  -7) },
        new[] { O(5,  -6) },
        new[] { O(5,  -5) },
        new[] { O(5,  -4) },
        new[] { O(5,  -3) },
        new[] { O(5,  -2) },
        new[] { O(5,  -1) },
        new[] { O(5,  0) },
        new[] { O(5,  1) },
        new[] { O(5,  2) },
        new[] { O(5,  3) },
        new[] { O(5,  4) },
        new[] { O(5,  5) },
        new[] { O(5,  6) },
        new[] { O(5,  7) },
        new[] { O(5,  8) }
    };

    private static CellOffset O(int x, int y) { return new CellOffset(x, y); }
    private static bool       OffsetsExpanded;
    public static  bool       SHOULDSHRINK = false;

    [HarmonyPatch(typeof(Game), "OnPrefabInit")]
    public static class GamePatch {
        public static void Postfix() { ExpandTables(); }

        public static void ExpandTables() {
            if (!OffsetsExpanded) {
                ExpandTable(ref OffsetGroups.InvertedStandardTable);
                ExpandTable(ref OffsetGroups.InvertedStandardTableWithCorners);
                OffsetsExpanded = true;
            }
        }

        public static void ExpandTable(ref CellOffset[][] inputTable) {
            CellOffset[][] array
                = OffsetTable.Mirror(Enumerable.ToArray(Enumerable.Concat(Enumerable.ToList(inputTable),
                                                                          TableExpansion)));

            inputTable = array;
        }

        public static bool Equals(CellOffset[] a, CellOffset[] b) {
            bool result;
            if (a == null || b == null || a.Length != b.Length || a.Length == 0)
                result = false;
            else {
                for (var i = 0; i < a.Length; i++)
                    if (!a[i].Equals(b[i]))
                        return false;

                result = true;
            }

            return result;
        }

        public static bool Contains(CellOffset[][] table, CellOffset[] array) {
            for (var i = 0; i < table.Length; i++)
                if (Equals(table[i], array))
                    return true;

            return false;
        }
    }
}
#endif