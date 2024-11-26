using HarmonyLib;
using UnityEngine;

#if usually
[HarmonyPatch]
public class 爆破弹组装机 {
    [HarmonyPatch(typeof(MissileFabricatorConfig), "ConfigureBuildingTemplate"), HarmonyPostfix]
    public static void Postfix(GameObject go) { go.AddOrGet<ComplexFabricator>().duplicantOperated = false; }
}

#endif