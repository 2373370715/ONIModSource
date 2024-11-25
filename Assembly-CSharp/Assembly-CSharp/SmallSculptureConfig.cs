using TUNING;
using UnityEngine;

public class SmallSculptureConfig : IBuildingConfig {
    public const string ID = "SmallSculpture";

    public override BuildingDef CreateBuildingDef() {
        var id                  = "SmallSculpture";
        var width               = 1;
        var height              = 2;
        var anim                = "sculpture_1x2_kanim";
        var hitpoints           = 10;
        var construction_time   = 60f;
        var tier                = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
        var raw_MINERALS        = MATERIALS.RAW_MINERALS;
        var melting_point       = 1600f;
        var build_location_rule = BuildLocationRule.OnFloor;
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
                                                              new EffectorValues { amount = 5, radius = 4 },
                                                              none);

        buildingDef.Floodable           = false;
        buildingDef.Overheatable        = false;
        buildingDef.AudioCategory       = "Metal";
        buildingDef.BaseTimeUntilRepair = -1f;
        buildingDef.ViewMode            = OverlayModes.Decor.ID;
        buildingDef.DefaultAnimState    = "slab";
        buildingDef.PermittedRotations  = PermittedRotations.FlipH;
        return buildingDef;
    }

    public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag) {
        go.AddOrGet<BuildingComplete>().isArtable = true;
        go.GetComponent<KPrefabID>().AddTag(GameTags.Decoration);
    }

    public override void DoPostConfigureComplete(GameObject go) {
        go.AddComponent<Sculpture>().defaultAnimName = "slab";
    }
}