using HarmonyLib;
using UnityEngine;

#if 碎石机
[HarmonyPatch]
public class 碎石机 {
    [HarmonyPatch(typeof(RockCrusherConfig), "ConfigureBuildingTemplate"), HarmonyPostfix]
    public static void Postfix(ref GameObject go) { go.AddOrGet<ComplexFabricator>().duplicantOperated = false; }
}
#endif