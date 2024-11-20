using HarmonyLib;
using UnityEngine;

#if 净水器
[HarmonyPatch]
public class 净水器 {
    [HarmonyPatch(typeof(WaterPurifierConfig), "ConfigureBuildingTemplate"), HarmonyPostfix]
    public static void Postfix(GameObject go) {
        var elementConverter = go.AddOrGet<ElementConverter>();

        var conduitConsumer = go.AddOrGet<ConduitConsumer>();
        conduitConsumer.consumptionRate = 20f;
        conduitConsumer.capacityKG      = 40f;
        elementConverter.consumedElements = new[] {
            new ElementConverter.ConsumedElement(new Tag("Filter"),     2f),
            new ElementConverter.ConsumedElement(new Tag("DirtyWater"), 10f)
        };

        elementConverter.outputElements = new[] {
            new ElementConverter.OutputElement(10f,
                                               SimHashes.Water,
                                               0f,
                                               false,
                                               true,
                                               0f,
                                               0.5f,
                                               0.75f),
            new ElementConverter.OutputElement(2f,
                                               SimHashes.ToxicSand,
                                               0f,
                                               false,
                                               true,
                                               0f,
                                               0.5f,
                                               0.25f)
        };
    }
}
#endif