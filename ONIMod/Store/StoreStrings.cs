using System;
using STRINGS;

namespace Store {
    public static class StoreStrings {
        public static class BUILDINGS {
            public static class PREFABS {
                public static class TELEVAULT {
                    public static LocString NAME   = UI.FormatAsLink("吸收物品", "吸收物品");
                    public static LocString DESC   = string.Concat(new string[] { "吸收标记为清扫的碎片" });
                    public static LocString EFFECT = string.Concat(new string[] { "吸收标记为清扫的碎片" });
                }
            }
        }
    }
}