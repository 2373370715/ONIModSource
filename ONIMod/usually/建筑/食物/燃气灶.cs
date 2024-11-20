using HarmonyLib;
using UnityEngine;

#if 燃气灶
[HarmonyPatch]
public class 燃气灶 {
    [HarmonyPatch(typeof(GourmetCookingStationConfig), "CreateBuildingDef"), HarmonyPostfix]
    public static void no_heat(ref BuildingDef __result) { __result.SelfHeatKilowattsWhenActive = 建筑.食物.电动烤炉.产热; }

    [HarmonyPatch(typeof(GourmetCookingStationConfig), "DoPostConfigureComplete"), HarmonyPostfix]
    public static void no_need_people(GameObject go) {
        go.AddOrGet<GourmetCookingStation>().duplicantOperated = 建筑.食物.电动烤炉.需要复制人;
    }
}
#endif