using HarmonyLib;
using UnityEngine;

#if 电动烤炉
[HarmonyPatch]
public class 电动烤炉 {
    [HarmonyPatch(typeof(CookingStationConfig), "CreateBuildingDef"),HarmonyPostfix]
    public static void 产热(ref BuildingDef __result) {
        __result.SelfHeatKilowattsWhenActive = 建筑.食物.电动烤炉.产热;
    }

    [HarmonyPatch(typeof(CookingStationConfig), "DoPostConfigureComplete")]
    public static void 需要人(GameObject go) {
                go.AddOrGet<CookingStation>().duplicantOperated = 建筑.食物.电动烤炉.需要复制人;

    }
}
#endif