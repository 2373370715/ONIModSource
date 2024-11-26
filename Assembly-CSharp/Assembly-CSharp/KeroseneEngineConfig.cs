using TUNING;
using UnityEngine;

public class KeroseneEngineConfig : IBuildingConfig {
    public const    string   ID = "KeroseneEngine";
    public override string[] GetForbiddenDlcIds() { return DlcManager.EXPANSION1; }

    public override BuildingDef CreateBuildingDef() {
        var      id                     = "KeroseneEngine";
        var      width                  = 7;
        var      height                 = 5;
        var      anim                   = "rocket_petroleum_engine_kanim";
        var      hitpoints              = 1000;
        var      construction_time      = 60f;
        var      engine_MASS_SMALL      = BUILDINGS.ROCKETRY_MASS_KG.ENGINE_MASS_SMALL;
        string[] construction_materials = { SimHashes.Steel.ToString() };
        var      melting_point          = 9999f;
        var      build_location_rule    = BuildLocationRule.OnFloor;
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
        buildingDef.SceneLayer          = Grid.SceneLayer.BuildingFront;
        buildingDef.OverheatTemperature = 2273.15f;
        buildingDef.Floodable           = false;
        buildingDef.AttachmentSlotTag   = GameTags.Rocket;
        buildingDef.ObjectLayer         = ObjectLayer.Building;
        buildingDef.attachablePosition  = new CellOffset(0, 0);
        buildingDef.RequiresPowerInput  = false;
        buildingDef.CanMove             = true;
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
        var rocketEngine = go.AddOrGet<RocketEngine>();
        rocketEngine.fuelTag             = ElementLoader.FindElementByHash(SimHashes.Petroleum).tag;
        rocketEngine.efficiency          = ROCKETRY.ENGINE_EFFICIENCY.MEDIUM;
        rocketEngine.explosionEffectHash = SpawnFXHashes.MeteorImpactDust;
        BuildingTemplates.ExtendBuildingToRocketModule(go, "rocket_petroleum_engine_bg_kanim");
    }
}