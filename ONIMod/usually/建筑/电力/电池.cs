using HarmonyLib;
using UnityEngine;

#if 电池
[HarmonyPatch]
public class 电池 {
    [HarmonyPatch(typeof(BatteryConfig), "DoPostConfigureComplete"), HarmonyPostfix]
    private static void Postfix(GameObject go) {
        var batterySmart = go.AddOrGet<Battery>();
        batterySmart.capacity            = 建筑.电力.电池.容量;
        batterySmart.joulesLostPerSecond = 建筑.电力.电池.能量损失;
    }
}
#endif