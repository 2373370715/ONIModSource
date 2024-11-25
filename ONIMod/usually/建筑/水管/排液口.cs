using HarmonyLib;
using UnityEngine;

#if 排液口
[HarmonyPatch]
public class 排液口 {
    [HarmonyPatch(typeof(LiquidVentConfig), "ConfigureBuildingTemplate"), HarmonyPostfix]
    public static void postfix1(GameObject go) { go.AddOrGet<Vent>().overpressureMass = 建筑.水管.排液口.压力; }
}
#endif