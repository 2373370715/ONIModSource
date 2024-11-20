using HarmonyLib;
using UnityEngine;

#if 食物压制器
[HarmonyPatch]
public class 食物压制器 {
    [HarmonyPatch(typeof(MicrobeMusherConfig), "ConfigureBuildingTemplate")]
    public static void Postfix(GameObject go) { go.AddOrGet<ComplexFabricator>().duplicantOperated = 建筑.食物.电动烤炉.需要复制人; }
}
#endif