using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;

#if 聚合物压塑器
[HarmonyPatch]
public class 聚合物压塑器 {
    [HarmonyPatch(typeof(PolymerizerConfig), "ConfigureBuildingTemplate"), HarmonyPostfix]
    public static void Postfix(GameObject go) {
        var elementConverter = go.AddOrGet<ElementConverter>();
        var outputitems = new List<ElementConverter.OutputElement> {
            new ElementConverter.OutputElement(1f, SimHashes.Polypropylene, Tools.getTemperature(20), false, true)
        };

        if (建筑.精炼.聚合物压塑器.输出水蒸气)
            outputitems.Add(new ElementConverter.OutputElement(0.008333334f, SimHashes.Steam, 473.15f, false, true));

        if (建筑.精炼.聚合物压塑器.输出二氧化碳)
            outputitems.Add(new ElementConverter.OutputElement(0.008333334f,
                                                               SimHashes.CarbonDioxide,
                                                               423.15f,
                                                               false,
                                                               true));

        elementConverter.outputElements = outputitems.ToArray();
        Prioritizable.AddRef(go);
    }
}
#endif