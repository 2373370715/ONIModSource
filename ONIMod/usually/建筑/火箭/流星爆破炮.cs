using HarmonyLib;

#if usually
[HarmonyPatch]
public class 流星爆破炮 {
    [HarmonyPatch(typeof(MissileLauncherConfig), nameof(MissileLauncherConfig.CreateBuildingDef)), HarmonyPostfix]
    public static void fix1(ref BuildingDef __result) {
        __result.ExhaustKilowattsWhenActive  = 0f;
        __result.SelfHeatKilowattsWhenActive = 0f;

    }
}

#endif