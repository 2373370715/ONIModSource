using HarmonyLib;

#if 电力调整
[HarmonyPatch]
public class WireAdjust {
    [HarmonyPatch(typeof(Wire), "GetMaxWattageAsFloat"),HarmonyPostfix]
    public static void Postfix(Wire __instance, ref float __result, Wire.WattageRating rating) {
        switch (rating) {
            case Wire.WattageRating.Max500:
                __result = 500f;
                break;
            case Wire.WattageRating.Max1000:
                // 电线
                __result = 系统.电线.Max1000;
                break;
            case Wire.WattageRating.Max2000:
                // 导线
                __result = 系统.电线.Max2000;
                break;
            case Wire.WattageRating.Max20000:
                // 高负荷电线
                __result = 系统.电线.Max20000;
                break;
            case Wire.WattageRating.Max50000:
                // 高负荷导线
                __result = 系统.电线.Max50000;
                break;
            default:
                __result = 0f;
                break;
        }
    }
}
#endif