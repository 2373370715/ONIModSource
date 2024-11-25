using System.Collections.Generic;
using TUNING;
using UnityEngine;

public class LiquidBottlerConfig : IBuildingConfig {
    public const  string      ID           = "LiquidBottler";
    private const ConduitType CONDUIT_TYPE = ConduitType.Liquid;
    private const int         WIDTH        = 3;
    private const int         HEIGHT       = 2;
    private const float       CAPACITY     = 200f;

    private static readonly List<Storage.StoredItemModifier> LiquidBottlerStoredItemModifiers
        = new List<Storage.StoredItemModifier> { Storage.StoredItemModifier.Hide, Storage.StoredItemModifier.Seal };

    public override BuildingDef CreateBuildingDef() {
        var buildingDef = BuildingTemplates.CreateBuildingDef("LiquidBottler",
                                                              3,
                                                              2,
                                                              "liquid_bottler_kanim",
                                                              100,
                                                              120f,
                                                              BUILDINGS.CONSTRUCTION_MASS_KG.TIER4,
                                                              MATERIALS.ALL_METALS,
                                                              800f,
                                                              BuildLocationRule.OnFloor,
                                                              BUILDINGS.DECOR.PENALTY.TIER1,
                                                              NOISE_POLLUTION.NOISY.TIER0);

        buildingDef.InputConduitType   = ConduitType.Liquid;
        buildingDef.Floodable          = false;
        buildingDef.ViewMode           = OverlayModes.LiquidConduits.ID;
        buildingDef.AudioCategory      = "HollowMetal";
        buildingDef.UtilityInputOffset = new CellOffset(0, 0);
        GeneratedBuildings.RegisterWithOverlay(OverlayScreen.LiquidVentIDs, "LiquidBottler");
        return buildingDef;
    }

    public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag) {
        var storage = BuildingTemplates.CreateDefaultStorage(go);
        storage.showDescriptor = true;
        storage.storageFilters = STORAGEFILTERS.LIQUIDS;
        storage.capacityKg     = 200f;
        storage.SetDefaultStoredItemModifiers(LiquidBottlerStoredItemModifiers);
        storage.allowItemRemoval = false;
        go.AddTag(GameTags.LiquidSource);
        var dropAllWorkable = go.AddOrGet<DropAllWorkable>();
        dropAllWorkable.removeTags                        = new List<Tag> { GameTags.LiquidSource };
        dropAllWorkable.resetTargetWorkableOnCompleteWork = true;
        var conduitConsumer = go.AddOrGet<ConduitConsumer>();
        conduitConsumer.storage              = storage;
        conduitConsumer.conduitType          = ConduitType.Liquid;
        conduitConsumer.ignoreMinMassCheck   = true;
        conduitConsumer.forceAlwaysSatisfied = true;
        conduitConsumer.alwaysConsume        = true;
        conduitConsumer.capacityKG           = 200f;
        conduitConsumer.keepZeroMassObject   = false;
        var bottler = go.AddOrGet<Bottler>();
        bottler.storage         = storage;
        bottler.workTime        = 9f;
        bottler.consumer        = conduitConsumer;
        bottler.userMaxCapacity = 200f;
    }

    public override void DoPostConfigureComplete(GameObject go) { }
}