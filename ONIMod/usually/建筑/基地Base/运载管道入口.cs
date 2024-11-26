using HarmonyLib;
using UnityEngine;

#if usually
[HarmonyPatch]
public class 运载管道入口 {
    [HarmonyPatch(typeof(TravelTubeEntranceConfig), nameof(TravelTubeEntranceConfig.CreateBuildingDef)), HarmonyPostfix]
    public static void postfix1(ref BuildingDef __result) { __result.EnergyConsumptionWhenActive = 1f; }

    [HarmonyPatch(typeof(TravelTubeEntranceConfig), nameof(TravelTubeEntranceConfig.ConfigureBuildingTemplate)),
     HarmonyPostfix]
    public static void postfix2(GameObject go) { go.AddOrGet<TravelTubeEntrance>().joulesPerLaunch = 0f; }
}

#endif