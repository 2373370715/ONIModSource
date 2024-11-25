using System.Collections.Generic;
using TUNING;
using UnityEngine;

public class SugarEngineConfig : IBuildingConfig {
    public const    string    ID              = "SugarEngine";
    public const    SimHashes FUEL            = SimHashes.Sucrose;
    public const    float     FUEL_CAPACITY   = 450f;
    public static   float     FUEL_EFFICIENCY = 0.125f;
    public override string[]  GetRequiredDlcIds() { return DlcManager.EXPANSION1; }

    public override BuildingDef CreateBuildingDef() {
        var id                  = "SugarEngine";
        var width               = 3;
        var height              = 3;
        var anim                = "rocket_sugar_engine_kanim";
        var hitpoints           = 1000;
        var construction_time   = 30f;
        var dense_TIER          = BUILDINGS.ROCKETRY_MASS_KG.DENSE_TIER1;
        var raw_METALS          = MATERIALS.RAW_METALS;
        var melting_point       = 9999f;
        var build_location_rule = BuildLocationRule.Anywhere;
        var tier                = NOISE_POLLUTION.NOISY.TIER2;
        var buildingDef = BuildingTemplates.CreateBuildingDef(id,
                                                              width,
                                                              height,
                                                              anim,
                                                              hitpoints,
                                                              construction_time,
                                                              dense_TIER,
                                                              raw_METALS,
                                                              melting_point,
                                                              build_location_rule,
                                                              BUILDINGS.DECOR.NONE,
                                                              tier);

        BuildingTemplates.CreateRocketBuildingDef(buildingDef);
        buildingDef.AttachmentSlotTag      = GameTags.Rocket;
        buildingDef.SceneLayer             = Grid.SceneLayer.Building;
        buildingDef.OverheatTemperature    = 2273.15f;
        buildingDef.Floodable              = false;
        buildingDef.attachablePosition     = new CellOffset(0, 0);
        buildingDef.ObjectLayer            = ObjectLayer.Building;
        buildingDef.InputConduitType       = ConduitType.None;
        buildingDef.GeneratorWattageRating = 60f;
        buildingDef.GeneratorBaseCapacity  = buildingDef.GeneratorWattageRating;
        buildingDef.RequiresPowerInput     = false;
        buildingDef.RequiresPowerOutput    = false;
        buildingDef.CanMove                = true;
        buildingDef.Cancellable            = false;
        return buildingDef;
    }

    public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag) {
        BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
        go.AddOrGet<LoopingSounds>();
        go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
        go.AddOrGet<BuildingAttachPoint>().points = new[] {
            new BuildingAttachPoint.HardPoint(new CellOffset(0, 3), GameTags.Rocket, null)
        };
    }

    public override void DoPostConfigurePreview(BuildingDef          def, GameObject go) { }
    public override void DoPostConfigureUnderConstruction(GameObject go) { }

    public override void DoPostConfigureComplete(GameObject go) {
        var rocketEngineCluster = go.AddOrGet<RocketEngineCluster>();
        rocketEngineCluster.maxModules          = 5;
        rocketEngineCluster.maxHeight           = ROCKETRY.ROCKET_HEIGHT.SHORT;
        rocketEngineCluster.fuelTag             = SimHashes.Sucrose.CreateTag();
        rocketEngineCluster.efficiency          = ROCKETRY.ENGINE_EFFICIENCY.STRONG;
        rocketEngineCluster.explosionEffectHash = SpawnFXHashes.MeteorImpactDust;
        rocketEngineCluster.requireOxidizer     = true;
        rocketEngineCluster.exhaustElement      = SimHashes.CarbonDioxide;
        go.AddOrGet<ModuleGenerator>();
        var storage = go.AddOrGet<Storage>();
        storage.capacityKg = 450f;
        storage.SetDefaultStoredItemModifiers(new List<Storage.StoredItemModifier> {
            Storage.StoredItemModifier.Hide, Storage.StoredItemModifier.Seal, Storage.StoredItemModifier.Insulate
        });

        var fuelTank = go.AddOrGet<FuelTank>();
        fuelTank.consumeFuelOnLand    = false;
        fuelTank.storage              = storage;
        fuelTank.FuelType             = SimHashes.Sucrose.CreateTag();
        fuelTank.targetFillMass       = storage.capacityKg;
        fuelTank.physicalFuelCapacity = storage.capacityKg;
        go.AddOrGet<CopyBuildingSettings>();
        var manualDeliveryKG = go.AddOrGet<ManualDeliveryKG>();
        manualDeliveryKG.SetStorage(storage);
        manualDeliveryKG.RequestedItemTag       = ElementLoader.FindElementByHash(SimHashes.Sucrose).tag;
        manualDeliveryKG.refillMass             = storage.capacityKg;
        manualDeliveryKG.capacity               = storage.capacityKg;
        manualDeliveryKG.operationalRequirement = Operational.State.None;
        manualDeliveryKG.choreTypeIDHash        = Db.Get().ChoreTypes.MachineFetch.IdHash;
        BuildingTemplates.ExtendBuildingToRocketModuleCluster(go,
                                                              null,
                                                              ROCKETRY.BURDEN.INSIGNIFICANT,
                                                              ROCKETRY.ENGINE_POWER.EARLY_WEAK,
                                                              FUEL_EFFICIENCY);

        go.GetComponent<KPrefabID>().prefabInitFn += delegate { };
    }
}