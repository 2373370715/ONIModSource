using HarmonyLib;
using UnityEngine;
#if 金属精炼器
[HarmonyPatch]
public class 金属精炼机 {
    [HarmonyPatch(typeof(MetalRefineryConfig), "ConfigureBuildingTemplate"), HarmonyPostfix]
    public static void Postfix(GameObject go) { go.AddOrGet<LiquidCooledRefinery>().duplicantOperated = false; }
}
#endif