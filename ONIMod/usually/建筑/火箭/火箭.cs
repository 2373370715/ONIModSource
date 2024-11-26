using System.Collections.Generic;
using Database;
using HarmonyLib;
using STRINGS;
using TUNING;

[HarmonyPatch]
public class 火箭 {
#if usually
    [HarmonyPatch(typeof(RocketStats), "GetTotalMass"), HarmonyPostfix]
    public static void setMass(ref float __result) { __result = 0.1f; }

    [HarmonyPatch(typeof(RocketStats), "GetTotalThrust"), HarmonyPostfix]
    public static void setThrust(ref float __result) { __result *= 10f; }

    [HarmonyPatch(typeof(Db), "Initialize"), HarmonyPrefix]
    public static void Initialize() {
        ROCKETRY.ENGINE_POWER.EARLY_WEAK *= 10;
        ROCKETRY.ENGINE_POWER.EARLY_STRONG *= 10;
        ROCKETRY.ENGINE_POWER.MID_VERY_STRONG *= 10;
        ROCKETRY.ENGINE_POWER.MID_STRONG *= 10;
        ROCKETRY.ENGINE_POWER.LATE_STRONG *= 10;
        ROCKETRY.ENGINE_POWER.LATE_VERY_STRONG *= 10;
    
        ROCKETRY.ENGINE_EFFICIENCY.WEAK *= 10;
        ROCKETRY.ENGINE_EFFICIENCY.MEDIUM *= 10;
        ROCKETRY.ENGINE_EFFICIENCY.STRONG *= 10;
        ROCKETRY.ENGINE_EFFICIENCY.BOOSTER *= 10;
    }

    [HarmonyPatch(typeof(Spacecraft), nameof(Spacecraft.BeginMission)), HarmonyPostfix]
    public static void BeginMission(ref float ___missionDuration) { ___missionDuration = ___missionDuration * 0.05f; }
#endif
#if 火箭速度调节
    [HarmonyPatch(typeof(Spacecraft), nameof(Spacecraft.BeginMission)), HarmonyPostfix]
    public static void BeginMission(ref float ___missionDuration) {
        ___missionDuration = ___missionDuration * Setting.Get().rocketSpeed;
    }
#endif
}