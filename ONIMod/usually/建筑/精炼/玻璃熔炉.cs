using HarmonyLib;
using UnityEngine;

#if 玻璃熔炉
[HarmonyPatch]
public class 玻璃熔炉 {
    [HarmonyPatch(typeof(GlassForgeConfig), "ConfigureBuildingTemplate"), HarmonyPostfix]
    public static void Postfix(GameObject go) { go.AddOrGet<GlassForge>().duplicantOperated = false; }
}
#endif