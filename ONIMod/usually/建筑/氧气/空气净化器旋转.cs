using HarmonyLib;

#if 空气净化器
[HarmonyPatch]
public class 空气净化器旋转 {
    [HarmonyPatch(typeof(AirFilterConfig), "CreateBuildingDef"), HarmonyPostfix]
    public static void Postfix(ref BuildingDef __result) {
        __result.PermittedRotations = PermittedRotations.R360;
        __result.BuildLocationRule  = BuildLocationRule.Tile;
    }
}
#endif