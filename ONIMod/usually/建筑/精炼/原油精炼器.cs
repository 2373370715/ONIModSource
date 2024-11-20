using HarmonyLib;
using UnityEngine;
#if 原油精炼器
[HarmonyPatch]
public class 原油精炼器 {
    [HarmonyPatch(typeof(OilRefineryConfig), "ConfigureBuildingTemplate"), HarmonyPrefix]
    public static bool Prefix(GameObject go, Tag prefab_tag) {
        Object.Destroy(go.AddOrGet<OilRefinery>());
        go.AddOrGet<AutomaticOilRefinery>();
        go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
        go.AddOrGet<BuildingComplete>().isManuallyOperated = false;
        OilRefinery oilRefinery = go.AddOrGet<OilRefinery>();
        oilRefinery.overpressureWarningMass = 4.5f;
        oilRefinery.overpressureMass        = 5f;
        ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
        conduitConsumer.conduitType          = ConduitType.Liquid;
        conduitConsumer.consumptionRate      = 10f;
        conduitConsumer.capacityTag          = SimHashes.CrudeOil.CreateTag();
        conduitConsumer.wrongElementResult   = ConduitConsumer.WrongElementResult.Dump;
        conduitConsumer.capacityKG           = 100f;
        conduitConsumer.forceAlwaysSatisfied = true;
        ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
        conduitDispenser.conduitType         = ConduitType.Liquid;
        conduitDispenser.invertElementFilter = true;
        conduitDispenser.elementFilter       = new[] { SimHashes.CrudeOil };
        go.AddOrGet<Storage>().showInUI      = true;
        ElementConverter elementConverter = go.AddOrGet<ElementConverter>();
        elementConverter.consumedElements = new[] {
            new ElementConverter.ConsumedElement(SimHashes.CrudeOil.CreateTag(), 10f)
        };

        elementConverter.outputElements = new[] {
            new ElementConverter.OutputElement(10f,
                                               SimHashes.Petroleum,
                                               Tools.getTemperature(10),
                                               storeOutput: true,
                                               outputElementOffsety: 1f)
        };

        Prioritizable.AddRef(go);
        return false;
    }
}
#endif