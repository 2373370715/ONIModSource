using HarmonyLib;
using UnityEngine;

#if 液泵
[HarmonyPatch]
public class 液泵 {
    [HarmonyPatch(typeof(LiquidPumpConfig), "DoPostConfigureComplete"), HarmonyPostfix]
    public static void Postfix(GameObject go) {
        go.AddOrGet<ElementConsumer>().consumptionRadius = 建筑.水管.液泵.半径;
    }
}
#endif