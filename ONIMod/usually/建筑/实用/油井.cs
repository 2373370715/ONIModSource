using HarmonyLib;
using UnityEngine;

#if 油井
[HarmonyPatch]
public class 油井 {
    [HarmonyPatch(typeof(OilWellCapConfig), "DoPostConfigureComplete"), HarmonyPostfix]
    public static void Postfix(GameObject go) {
        var elementConverter = go.AddOrGet<ElementConverter>();
        var conduitConsumer  = go.AddOrGet<ConduitConsumer>();
        conduitConsumer.conduitType                              = ConduitType.Liquid;
        conduitConsumer.consumptionRate                          = 10f;
        conduitConsumer.capacityKG                               = 30f;
        elementConverter.consumedElements[0].MassConsumptionRate = 10f;
        elementConverter.outputElements[0].massGenerationRate    = 33.333333f;

        var oilWellCap = go.AddOrGet<OilWellCap>();

        // 不需要释放压力
        oilWellCap.addGasRate = 0f;
    }
}
#endif