using TUNING;
using UnityEngine;

public class KeroseneEngineClusterConfig : IBuildingConfig {
    public const    string    ID   = "KeroseneEngineCluster";
    public const    SimHashes FUEL = SimHashes.Petroleum;
    public override string[]  GetRequiredDlcIds() { return DlcManager.EXPANSION1; }

    public override BuildingDef CreateBuildingDef() {
        var      id                     = "KeroseneEngineCluster";
        var      width                  = 7;
        var      height                 = 5;
        var      anim                   = "rocket_cluster_petroleum_engine_kanim";
        var      hitpoints              = 1000;
        var      construction_time      = 60f;
        var      engine_MASS_SMALL      = BUILDINGS.ROCKETRY_MASS_KG.ENGINE_MASS_SMALL;
        string[] construction_materials = { SimHashes.Steel.ToString() };
        var      melting_point          = 9999f;
        var      build_location_rule    = BuildLocationRule.Anywhere;
        var      tier                   = NOISE_POLLUTION.NOISY.TIER2;
        var buildingDef = BuildingTemplates.CreateBuildingDef(id,
                                                              width,
                                                              height,
                                                              anim,
                                                              hitpoints,
                                                              construction_time,
                                                              engine_MASS_SMALL,
                                                              construction_materials,
                                                              melting_point,
                                                              build_location_rule,
                                                              BUILDINGS.DECOR.NONE,
                                                              tier);

        BuildingTemplates.CreateRocketBuildingDef(buildingDef);
        buildingDef.SceneLayer             = Grid.SceneLayer.Building;
        buildingDef.OverheatTemperature    = 2273.15f;
        buildingDef.Floodable              = false;
        buildingDef.AttachmentSlotTag      = GameTags.Rocket;
        buildingDef.ObjectLayer            = ObjectLayer.Building;
        buildingDef.attachablePosition     = new CellOffset(0, 0);
        buildingDef.GeneratorWattageRating = 480f;
        buildingDef.GeneratorBaseCapacity  = buildingDef.GeneratorWattageRating;
        buildingDef.RequiresPowerInput     = false;
        buildingDef.RequiresPowerOutput    = false;
        buildingDef.CanMove                = true;
        buildingDef.Cancellable            = false;
        buildingDef.ShowInBuildMenu        = false;
        return buildingDef;
    }

    public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag) {
        BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
        go.AddOrGet<LoopingSounds>();
        go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
        go.AddOrGet<BuildingAttachPoint>().points = new[] {
            new BuildingAttachPoint.HardPoint(new CellOffset(0, 5), GameTags.Rocket, null)
        };
    }

    public override void DoPostConfigureComplete(GameObject go) {
        var rocketEngineCluster = go.AddOrGet<RocketEngineCluster>();
        rocketEngineCluster.maxModules          = 7;
        rocketEngineCluster.maxHeight           = ROCKETRY.ROCKET_HEIGHT.VERY_TALL;
        rocketEngineCluster.fuelTag             = SimHashes.Petroleum.CreateTag();
        rocketEngineCluster.efficiency          = ROCKETRY.ENGINE_EFFICIENCY.MEDIUM;
        rocketEngineCluster.requireOxidizer     = true;
        rocketEngineCluster.explosionEffectHash = SpawnFXHashes.MeteorImpactDust;
        rocketEngineCluster.exhaustElement      = SimHashes.CarbonDioxide;
        rocketEngineCluster.exhaustTemperature  = 1263.15f;
        go.AddOrGet<ModuleGenerator>();
        BuildingTemplates.ExtendBuildingToRocketModuleCluster(go,
                                                              null,
                                                              ROCKETRY.BURDEN.MAJOR,
                                                              ROCKETRY.ENGINE_POWER.MID_VERY_STRONG,
                                                              ROCKETRY.FUEL_COST_PER_DISTANCE.VERY_HIGH);

        go.GetComponent<KPrefabID>().prefabInitFn += delegate { };
    }
}