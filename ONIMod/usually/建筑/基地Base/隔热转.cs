using HarmonyLib;
#if 隔热砖
[HarmonyPatch]
public class 隔热转 {
    [HarmonyPatch(typeof(InsulationTileConfig), "CreateBuildingDef"), HarmonyPostfix]
    public static void Postfix(ref BuildingDef __result) { __result.ThermalConductivity = 0; }
}
#endif