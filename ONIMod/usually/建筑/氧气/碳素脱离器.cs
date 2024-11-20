using HarmonyLib;
using UnityEngine;
#if 碳素脱离器
[HarmonyPatch]
public class 碳素脱离器2 {
    [HarmonyPatch(typeof(CO2ScrubberConfig), "ConfigureBuildingTemplate"), HarmonyPostfix]
    public static void Postfix(GameObject go) {
        go.AddOrGet<PassiveElementConsumer>().consumptionRadius = 建筑.氧气.碳素脱离器.半径;
    }
}
#endif