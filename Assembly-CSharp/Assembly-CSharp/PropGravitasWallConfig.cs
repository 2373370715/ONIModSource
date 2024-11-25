using TUNING;
using UnityEngine;

public class PropGravitasWallConfig : IBuildingConfig {
    public const string ID = "PropGravitasWall";

    public override BuildingDef CreateBuildingDef() {
        var id                  = "PropGravitasWall";
        var width               = 1;
        var height              = 1;
        var anim                = "gravitas_walls_kanim";
        var hitpoints           = 30;
        var construction_time   = 30f;
        var tier                = BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
        var raw_MINERALS        = MATERIALS.RAW_MINERALS;
        var melting_point       = 1600f;
        var build_location_rule = BuildLocationRule.NotInTiles;
        var none                = NOISE_POLLUTION.NONE;
        var buildingDef = BuildingTemplates.CreateBuildingDef(id,
                                                              width,
                                                              height,
                                                              anim,
                                                              hitpoints,
                                                              construction_time,
                                                              tier,
                                                              raw_MINERALS,
                                                              melting_point,
                                                              build_location_rule,
                                                              DECOR.BONUS.TIER0,
                                                              none);

        buildingDef.PermittedRotations  = PermittedRotations.R360;
        buildingDef.Entombable          = false;
        buildingDef.Floodable           = false;
        buildingDef.Overheatable        = false;
        buildingDef.AudioCategory       = "Metal";
        buildingDef.BaseTimeUntilRepair = -1f;
        buildingDef.DefaultAnimState    = "off";
        buildingDef.ObjectLayer         = ObjectLayer.Backwall;
        buildingDef.SceneLayer          = Grid.SceneLayer.Backwall;
        buildingDef.ShowInBuildMenu     = false;
        return buildingDef;
    }

    public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag) {
        go.AddOrGet<AnimTileable>().objectLayer = ObjectLayer.Backwall;
        go.AddComponent<ZoneTile>();
        go.GetComponent<PrimaryElement>().SetElement(SimHashes.Granite);
        go.GetComponent<PrimaryElement>().Temperature = 273f;
        go.GetComponent<KPrefabID>().AddTag(GameTags.Gravitas);
        BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
    }

    public override void DoPostConfigureComplete(GameObject go) { }
}