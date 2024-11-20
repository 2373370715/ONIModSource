using HarmonyLib;
using UnityEngine;

#if 蒸汽机
[HarmonyPatch]
public class 蒸汽机 {
    [HarmonyPatch(typeof(SteamTurbineConfig2), "DoPostConfigureComplete"), HarmonyPostfix]
    public static void Postfix(GameObject go) {
        var steamTurbine = go.AddOrGet<SteamTurbine>();
        steamTurbine.minActiveTemperature     = 建筑.电力.蒸汽机.输入蒸汽温度;
        steamTurbine.outputElementTemperature = 建筑.电力.蒸汽机.输出水温度;
    }
}
#endif