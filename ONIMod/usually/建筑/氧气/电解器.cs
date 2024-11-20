using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;
#if 电解器
[HarmonyPatch]
public class 电解器 {
    [HarmonyPatch(typeof(ElectrolyzerConfig), "ConfigureBuildingTemplate"), HarmonyPrefix]
    public static bool output_oxygen(GameObject go) {
        var cellOffset = new CellOffset(0, 1);
        go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
        var electrolyzer = go.AddOrGet<Electrolyzer>();
        electrolyzer.maxMass        = 30f;
        electrolyzer.hasMeter       = true;
        electrolyzer.emissionOffset = cellOffset;
        var conduitConsumer = go.AddOrGet<ConduitConsumer>();
        conduitConsumer.conduitType        = ConduitType.Liquid;
        conduitConsumer.consumptionRate    = 10f;
        conduitConsumer.capacityTag        = ElementLoader.FindElementByHash(SimHashes.Water).tag;
        conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
        var storage = go.AddOrGet<Storage>();
        storage.capacityKg = 20f;
        storage.showInUI   = true;
        storage.SetDefaultStoredItemModifiers(new List<Storage.StoredItemModifier> {
            Storage.StoredItemModifier.Hide, Storage.StoredItemModifier.Insulate
        });

        var elementConverter = go.AddOrGet<ElementConverter>();
        elementConverter.consumedElements = new[] { new ElementConverter.ConsumedElement(new Tag("Water"), 10f) };

        elementConverter.outputElements = new[] {
            new ElementConverter.OutputElement(8.88f,
                                               SimHashes.Oxygen,
                                               343.15f,
                                               false,
                                               false,
                                               cellOffset.x,
                                               cellOffset.y)
        };

        Prioritizable.AddRef(go);
        return false;
    }
}
#endif