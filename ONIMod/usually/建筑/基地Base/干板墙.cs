using HarmonyLib;
using System;

#if 干板墙
public class 干板墙 {
    [HarmonyPatch(typeof(ExteriorWallConfig), "CreateBuildingDef")]
    public static void Postfix(ref BuildingDef __result) { __result.BaseMeltingPoint = 16000f; }
}
#endif