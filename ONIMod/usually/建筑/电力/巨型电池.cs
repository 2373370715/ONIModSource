using HarmonyLib;
using UnityEngine;

#if 电池
[HarmonyPatch]
public class 巨型电池 {
    [HarmonyPatch(typeof(BatteryMediumConfig), "DoPostConfigureComplete"), HarmonyPostfix]
    public static void Postfix(GameObject go) {
        var battery = go.AddOrGet<Battery>();
        battery.capacity            = 建筑.电力.巨型电池.容量;
        battery.joulesLostPerSecond = 建筑.电力.巨型电池.能量损失;
    }
}
#endif