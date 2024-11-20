using System.Collections.Generic;
using TUNING;
using UnityEngine;

public class ElectrolyzerConfig : IBuildingConfig {
    public const string ID                 = "Electrolyzer";
    public const float  WATER2OXYGEN_RATIO = 0.888f;
    public const float  OXYGEN_TEMPERATURE = 343.15f;

    public override BuildingDef CreateBuildingDef() {
        var id                  = "Electrolyzer";
        var width               = 2;
        var height              = 2;
        var anim                = "electrolyzer_kanim";
        var hitpoints           = 30;
        var construction_time   = 30f;
        var tier                = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
        var all_METALS          = MATERIALS.ALL_METALS;
        var melting_point       = 800f;
        var build_location_rule = BuildLocationRule.OnFloor;
        var tier2               = NOISE_POLLUTION.NOISY.TIER3;
        var buildingDef = BuildingTemplates.CreateBuildingDef(id,
                                                              width,
                                                              height,
                                                              anim,
                                                              hitpoints,
                                                              construction_time,
                                                              tier,
                                                              all_METALS,
                                                              melting_point,
                                                              build_location_rule,
                                                              BUILDINGS.DECOR.PENALTY.TIER1,
                                                              tier2);

        buildingDef.RequiresPowerInput = true;
        buildingDef.PowerInputOffset = new CellOffset(1, 0);
        buildingDef.EnergyConsumptionWhenActive = 120f;
        buildingDef.ExhaustKilowattsWhenActive = 0.25f;
        buildingDef.SelfHeatKilowattsWhenActive = 1f;
        buildingDef.ViewMode = OverlayModes.Oxygen.ID;
        buildingDef.AudioCategory = "HollowMetal";
        buildingDef.InputConduitType = ConduitType.Liquid;
        buildingDef.UtilityInputOffset = new CellOffset(0, 0);
        buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(1, 1));
        return buildingDef;
    }

    public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag) {
        var cellOffset = new CellOffset(0, 1);
        go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
        var electrolyzer = go.AddOrGet<Electrolyzer>();
        electrolyzer.maxMass        = 1.8f;
        electrolyzer.hasMeter       = true;
        electrolyzer.emissionOffset = cellOffset;
        var conduitConsumer = go.AddOrGet<ConduitConsumer>();
        conduitConsumer.conduitType        = ConduitType.Liquid;
        conduitConsumer.consumptionRate    = 1f;
        conduitConsumer.capacityTag        = ElementLoader.FindElementByHash(SimHashes.Water).tag;
        conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
        var storage = go.AddOrGet<Storage>();
        storage.capacityKg = 2f;
        storage.showInUI   = true;
        storage.SetDefaultStoredItemModifiers(new List<Storage.StoredItemModifier> {
            Storage.StoredItemModifier.Hide, Storage.StoredItemModifier.Insulate
        });

        var elementConverter = go.AddOrGet<ElementConverter>();
        elementConverter.consumedElements = new[] { new ElementConverter.ConsumedElement(new Tag("Water"), 1f) };
        elementConverter.outputElements = new[] {
            new ElementConverter.OutputElement(0.888f,
                                               SimHashes.Oxygen,
                                               343.15f,
                                               false,
                                               false,
                                               cellOffset.x,
                                               cellOffset.y),
            new ElementConverter.OutputElement(0.11199999f,
                                               SimHashes.Hydrogen,
                                               343.15f,
                                               false,
                                               false,
                                               cellOffset.x,
                                               cellOffset.y)
        };

        Prioritizable.AddRef(go);
    }

    public override void DoPostConfigureComplete(GameObject go) {
        go.AddOrGet<LogicOperationalController>();
        go.AddOrGetDef<PoweredActiveController.Def>();
    }
}