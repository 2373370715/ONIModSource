using HarmonyLib;

#if 不会过热
public class 不会过热 {
    [HarmonyPatch(typeof(BuildingLoader), "CreateBuildingComplete"), HarmonyPrefix]
    public static void Prefix(ref BuildingDef def) { def.Overheatable = 系统.过热; }
}
#endif