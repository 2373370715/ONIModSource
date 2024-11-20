using HarmonyLib;
using UnityEngine;

#if 壁虎
[HarmonyPatch(typeof(DreckoConfig), "CreateDrecko")]
public class 毛鳞壁虎 {
    public static void Postfix(ref GameObject __result) {
        var def2 = __result.AddOrGetDef<ScaleGrowthMonitor.Def>();
        def2.targetAtmosphere = SimHashes.Oxygen;
    }
}

[HarmonyPatch(typeof(DreckoPlasticConfig), "CreateDrecko")]
public class 滑鳞壁虎 {
    public static void Postfix(ref GameObject __result) {
        var def2 = __result.AddOrGetDef<ScaleGrowthMonitor.Def>();
        def2.targetAtmosphere = SimHashes.Oxygen;
    }
}
#endif