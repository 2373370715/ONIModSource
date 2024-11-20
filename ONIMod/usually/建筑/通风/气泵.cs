using HarmonyLib;
using UnityEngine;

#if 气泵
[HarmonyPatch]
public class 气泵 {
    [HarmonyPatch(typeof(GasPumpConfig), "DoPostConfigureComplete"), HarmonyPostfix]
    public static void Postfix(GameObject go) {
        go.AddOrGet<ElementConsumer>().consumptionRadius = 建筑.通风.气泵.半径;
    }
}
#endif