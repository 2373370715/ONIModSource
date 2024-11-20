using TUNING;
using UnityEngine;

public class WaterPurifierConfig : IBuildingConfig {
    public const  string ID                        = "WaterPurifier";
    private const float  FILTER_INPUT_RATE         = 1f;
    private const float  DIRTY_WATER_INPUT_RATE    = 5f;
    private const float  FILTER_CAPACITY           = 1200f;
    private const float  USED_FILTER_OUTPUT_RATE   = 0.2f;
    private const float  CLEAN_WATER_OUTPUT_RATE   = 5f;
    private const float  TARGET_OUTPUT_TEMPERATURE = 313.15f;

    public override BuildingDef CreateBuildingDef() {
        var id                  = "WaterPurifier";
        var width               = 4;
        var height              = 3;
        var anim                = "waterpurifier_kanim";
        var hitpoints           = 100;
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
                                                              BUILDINGS.DECOR.PENALTY.TIER2,
                                                              tier2);

        buildingDef.RequiresPowerInput = true;
        buildingDef.EnergyConsumptionWhenActive = 120f;
        buildingDef.ExhaustKilowattsWhenActive = 0f;
        buildingDef.SelfHeatKilowattsWhenActive = 4f;
        buildingDef.InputConduitType = ConduitType.Liquid;
        buildingDef.OutputConduitType = ConduitType.Liquid;
        buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(-1, 0));
        buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
        buildingDef.AudioCategory = "HollowMetal";
        buildingDef.PowerInputOffset = new CellOffset(2, 0);
        buildingDef.UtilityInputOffset = new CellOffset(-1, 2);
        buildingDef.UtilityOutputOffset = new CellOffset(2, 2);
        buildingDef.PermittedRotations = PermittedRotations.FlipH;
        GeneratedBuildings.RegisterWithOverlay(OverlayScreen.LiquidVentIDs, "WaterPurifier");
        return buildingDef;
    }

    public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag) {
        go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
        var storage = BuildingTemplates.CreateDefaultStorage(go);
        storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
        go.AddOrGet<WaterPurifier>();
        Prioritizable.AddRef(go);
        var elementConverter = go.AddOrGet<ElementConverter>();
        elementConverter.consumedElements = new[] {
            new ElementConverter.ConsumedElement(new Tag("Filter"),     1f),
            new ElementConverter.ConsumedElement(new Tag("DirtyWater"), 5f)
        };

        elementConverter.outputElements = new[] {
            new ElementConverter.OutputElement(5f,
                                               SimHashes.Water,
                                               0f,
                                               false,
                                               true,
                                               0f,
                                               0.5f,
                                               0.75f),
            new ElementConverter.OutputElement(0.2f,
                                               SimHashes.ToxicSand,
                                               0f,
                                               false,
                                               true,
                                               0f,
                                               0.5f,
                                               0.25f)
        };

        var elementDropper = go.AddComponent<ElementDropper>();
        elementDropper.emitMass   = 10f;
        elementDropper.emitTag    = new Tag("ToxicSand");
        elementDropper.emitOffset = new Vector3(0f, 1f, 0f);
        var manualDeliveryKG = go.AddComponent<ManualDeliveryKG>();
        manualDeliveryKG.SetStorage(storage);
        manualDeliveryKG.RequestedItemTag = new Tag("Filter");
        manualDeliveryKG.capacity         = 1200f;
        manualDeliveryKG.refillMass       = 300f;
        manualDeliveryKG.choreTypeIDHash  = Db.Get().ChoreTypes.FetchCritical.IdHash;
        var conduitConsumer = go.AddOrGet<ConduitConsumer>();
        conduitConsumer.conduitType          = ConduitType.Liquid;
        conduitConsumer.consumptionRate      = 10f;
        conduitConsumer.capacityKG           = 20f;
        conduitConsumer.capacityTag          = GameTags.AnyWater;
        conduitConsumer.forceAlwaysSatisfied = true;
        conduitConsumer.wrongElementResult   = ConduitConsumer.WrongElementResult.Store;
        var conduitDispenser = go.AddOrGet<ConduitDispenser>();
        conduitDispenser.conduitType         = ConduitType.Liquid;
        conduitDispenser.invertElementFilter = true;
        conduitDispenser.elementFilter       = new[] { SimHashes.DirtyWater };
    }

    public override void DoPostConfigureComplete(GameObject go) {
        go.AddOrGet<LogicOperationalController>();
        go.AddOrGetDef<PoweredActiveController.Def>();
        go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayBehindConduits);
    }
}