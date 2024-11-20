using HarmonyLib;

namespace Ray {
    [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
    public class AddToBuild {
        [HarmonyPostfix]
        private static void Postfix() {
            Strings.Add(new string[] { "STRINGS.BUILDINGS.PREFABS.XRAY.NAME", "杀菌灯" });
            Strings.Add(new string[] { "STRINGS.BUILDINGS.PREFABS.XRAY.DESC", "杀灭细菌" });
            Strings.Add(new string[] { "STRINGS.BUILDINGS.PREFABS.XRAY.EFFECT", "杀灭细菌" });
            ModUtil.AddBuildingToPlanScreen("Utilities", "XRAY");
        }
    }
}