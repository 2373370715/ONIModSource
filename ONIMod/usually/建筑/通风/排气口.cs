using HarmonyLib;
using UnityEngine;

#if 排气口
[HarmonyPatch]
public class 排气口 {
    [HarmonyPatch(typeof(GasVentConfig), "ConfigureBuildingTemplate"), HarmonyPostfix]
    public static void postfix(GameObject go, Tag prefab_tag) {
        go.AddOrGet<Vent>().overpressureMass = 建筑.通风.排气口.压力;
    }
}
#endif