using HarmonyLib;
using UnityEngine;

#if 电池
[HarmonyPatch]
public class 智能电池 {
    [HarmonyPatch(typeof(BatterySmartConfig), "DoPostConfigureComplete"), HarmonyPostfix]
    public static void Postfix(GameObject go) {
        var batterySmart = go.AddOrGet<BatterySmart>();
        batterySmart.capacity            = 建筑.电力.智能电池.容量;
        batterySmart.joulesLostPerSecond = 建筑.电力.智能电池.能量损失;
    }
}
#endif