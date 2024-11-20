using HarmonyLib;
using UnityEngine;

#if 分子熔炉
[HarmonyPatch]
public class 分子熔炉 {
    [HarmonyPatch(typeof(SupermaterialRefineryConfig), "ConfigureBuildingTemplate"), HarmonyPostfix]
    public static void Postfix(GameObject go) { go.AddOrGet<ComplexFabricator>().duplicantOperated = false; }
}
#endif