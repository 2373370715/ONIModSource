using TUNING;
using UnityEngine;

public class SolidCargoBaySmallConfig : IBuildingConfig {
    public const    string   ID       = "SolidCargoBaySmall";
    public          float    CAPACITY = 1200f * ROCKETRY.CARGO_CAPACITY_SCALE;
    public override string[] GetRequiredDlcIds() { return DlcManager.EXPANSION1; }

    public override BuildingDef CreateBuildingDef() {
        var id                  = "SolidCargoBaySmall";
        var width               = 3;
        var height              = 3;
        var anim                = "rocket_storage_solid_small_kanim";
        var hitpoints           = 1000;
        var construction_time   = 30f;
        var hollow_TIER         = BUILDINGS.ROCKETRY_MASS_KG.HOLLOW_TIER1;
        var refined_METALS      = MATERIALS.REFINED_METALS;
        var melting_point       = 9999f;
        var build_location_rule = BuildLocationRule.Anywhere;
        var tier                = NOISE_POLLUTION.NOISY.TIER2;
        var buildingDef = BuildingTemplates.CreateBuildingDef(id,
                                                              width,
                                                              height,
                                                              anim,
                                                              hitpoints,
                                                              construction_time,
                                                              hollow_TIER,
                                                              refined_METALS,
                                                              melting_point,
                                                              build_location_rule,
                                                              BUILDINGS.DECOR.NONE,
                                                              tier);

        BuildingTemplates.CreateRocketBuildingDef(buildingDef);
        buildingDef.SceneLayer          = Grid.SceneLayer.Building;
        buildingDef.Invincible          = true;
        buildingDef.OverheatTemperature = 2273.15f;
        buildingDef.Floodable           = false;
        buildingDef.AttachmentSlotTag   = GameTags.Rocket;
        buildingDef.ObjectLayer         = ObjectLayer.Building;
        buildingDef.RequiresPowerInput  = false;
        buildingDef.attachablePosition  = new CellOffset(0, 0);
        buildingDef.CanMove             = true;
        buildingDef.Cancellable         = false;
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

    public override void DoPostConfigureComplete(GameObject go) {
        go = BuildingTemplates.ExtendBuildingToClusterCargoBay(go,
                                                               CAPACITY,
                                                               STORAGEFILTERS.STORAGE_LOCKERS_STANDARD,
                                                               CargoBay.CargoType.Solids);

        BuildingTemplates.ExtendBuildingToRocketModuleCluster(go, null, ROCKETRY.BURDEN.MODERATE);
    }
}