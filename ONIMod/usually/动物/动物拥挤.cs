using HarmonyLib;

#if 动物拥挤
[HarmonyPatch(typeof(OvercrowdingMonitor))]
public class 动物拥挤 {
    [HarmonyPatch("IsConfined"), HarmonyPostfix]
    private static void patch1(ref bool __result) { __result = 动物.拥挤; }
    
    [HarmonyPatch("IsOvercrowded"),HarmonyPostfix]
     private static void patch2(ref bool __result) { __result = 动物.拥挤; }
     
     [HarmonyPatch("IsFutureOvercrowded"),HarmonyPostfix]
     private static void patch3(ref bool __result) { __result = 动物.拥挤; }
}
#endif