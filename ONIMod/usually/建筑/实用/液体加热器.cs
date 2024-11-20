using HarmonyLib;
using UnityEngine;

#if 液体加热器
[HarmonyPatch]
public class 液体加热器 {
    [HarmonyPatch(typeof(LiquidHeaterConfig), "CreateBuildingDef"), HarmonyPostfix]
    public static void fix1(ref BuildingDef __result) { __result.ExhaustKilowattsWhenActive = 120000f; }

    [HarmonyPatch(typeof(LiquidHeaterConfig), "ConfigureBuildingTemplate"), HarmonyPostfix]
    public static void fix2(GameObject go, Tag prefab_tag) {
        go.AddOrGet<SpaceHeater>().targetTemperature = 273.15f + 405f;
    }
}
#endif