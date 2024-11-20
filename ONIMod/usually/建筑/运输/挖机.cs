using HarmonyLib;
using UnityEngine;

#if 挖机

[HarmonyPatch]
public class 挖机 {
    [HarmonyPatch(typeof(AutoMinerConfig), nameof(AutoMinerConfig.DoPostConfigureComplete))]
    [HarmonyPostfix]
    public static void fix_range(GameObject go) {
        var autoMiner = go.AddOrGet<AutoMiner>();
        autoMiner.x      = -32;
        autoMiner.y      = 0;
        autoMiner.width  = 64;
        autoMiner.height = 64;
    }

    [HarmonyPatch(typeof(AutoMinerConfig), "AddVisualizer")]
    [HarmonyPostfix]
    public static void fix_visual_range(GameObject prefab) {
        var range = prefab.AddOrGet<RangeVisualizer>();
        range.RangeMin.x = -10;
        range.RangeMin.y = 0;
        range.RangeMax.x = 10;
        range.RangeMax.y = 10;
    }

    [HarmonyPatch(typeof(AutoMiner), nameof(AutoMiner.UpdateDig))]
    [HarmonyPrefix]
    public static void fix_dig_rate(ref AutoMiner __instance) {
        var dig_cell = (int)AboutPrivate.GetPrivateValue(__instance, "dig_cell");
        if (Grid.Damage[dig_cell] != 1f) { Grid.Damage[dig_cell] = 1f; }
    }

    [HarmonyPatch(typeof(AutoMiner), "ValidDigCell")]
    [HarmonyPostfix]
    public static void delete_hardness1(int cell, ref bool __result) {
        var flag = Grid.HasDoor[cell] && Grid.Foundation[cell] && Grid.ObjectLayers[9].ContainsKey(cell);
        if (flag) {
            var component = Grid.ObjectLayers[9][cell].GetComponent<Door>();
            flag = component != null && component.IsOpen() && !component.IsPendingClose();
        }
    
        __result = Grid.Solid[cell] && (!Grid.Foundation[cell] || flag);
    }
    
    [HarmonyPatch(typeof(AutoMiner), "DigBlockingCB")]
    [HarmonyPostfix]
    public static void delete_hardness2(int cell, ref bool __result) {
        var flag = Grid.HasDoor[cell] && Grid.Foundation[cell] && Grid.ObjectLayers[9].ContainsKey(cell);
        if (flag) {
            var component = Grid.ObjectLayers[9][cell].GetComponent<Door>();
            flag = component != null && component.IsOpen() && !component.IsPendingClose();
        }
    
        __result = Grid.Foundation[cell] && Grid.Solid[cell] && !flag;
    }
}
#endif