using HarmonyLib;
using UnityEngine;

#if 变速
[HarmonyPatch(typeof(SpeedControlScreen), "OnChanged")]
public class 变速 {
    public static void Postfix(ref SpeedControlScreen __instance) {
        if (__instance.IsPaused)
            Time.timeScale = 0f;
        else {
            if (__instance.GetSpeed() == 0)
                Time.timeScale = __instance.normalSpeed * 系统.游戏速度.low;

            else if (__instance.GetSpeed() == 1)
                Time.timeScale = __instance.normalSpeed * 系统.游戏速度.midium;

            else if (__instance.GetSpeed() == 2) Time.timeScale = __instance.normalSpeed * 系统.游戏速度.high;
        }
    }
}
#endif