using HarmonyLib;

#if 变温板
[HarmonyPatch]
public class 变温板 {
    [HarmonyPatch(typeof(ThermalBlockConfig), "CreateBuildingDef"), HarmonyPostfix]
    public static void Postfix(ref BuildingDef __result) {
        // 120f
        __result.ConstructionTime = 1f;

        // 1600f
        __result.BaseMeltingPoint = 16000f;
    }
}
#endif