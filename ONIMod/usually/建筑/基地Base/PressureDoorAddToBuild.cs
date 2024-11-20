using HarmonyLib;

#if 机械气闸
[HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
public class PressureDoorAddToBuild {
    [HarmonyPostfix]
    private static void Postfix() {
        Strings.Add(new string[] { "STRINGS.BUILDINGS.PREFABS.PD.NAME", "隔水门" });
        Strings.Add(new string[] { "STRINGS.BUILDINGS.PREFABS.PD.DESC", "隔水" });
        Strings.Add(new string[] { "STRINGS.BUILDINGS.PREFABS.PD.EFFECT", "隔水" });
        ModUtil.AddBuildingToPlanScreen("Base", "PD");
    }
}
#endif