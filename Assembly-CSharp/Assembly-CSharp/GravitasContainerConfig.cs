using TUNING;
using UnityEngine;

public class GravitasContainerConfig : IBuildingConfig {
    public const  string ID        = "GravitasContainer";
    private const float  WORK_TIME = 1.5f;

    public override BuildingDef CreateBuildingDef() {
        var id                  = "GravitasContainer";
        var width               = 2;
        var height              = 2;
        var anim                = "gravitas_container_kanim";
        var hitpoints           = 30;
        var construction_time   = 10f;
        var tier                = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
        var all_METALS          = MATERIALS.ALL_METALS;
        var melting_point       = 2400f;
        var build_location_rule = BuildLocationRule.OnFloor;
        var none                = NOISE_POLLUTION.NONE;
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
                                                              BUILDINGS.DECOR.NONE,
                                                              none);

        buildingDef.ShowInBuildMenu = false;
        buildingDef.Entombable      = false;
        buildingDef.Floodable       = false;
        buildingDef.Invincible      = true;
        buildingDef.AudioCategory   = "Metal";
        return buildingDef;
    }

    public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag) {
        go.AddTag(GameTags.Gravitas);
        go.AddOrGet<KBatchedAnimController>().sceneLayer = Grid.SceneLayer.Building;
        Prioritizable.AddRef(go);
    }

    public override void DoPostConfigureComplete(GameObject go) {
        var pajamaDispenser = go.AddComponent<PajamaDispenser>();
        pajamaDispenser.overrideAnims = new[] { Assets.GetAnim("anim_interacts_gravitas_container_kanim") };
        pajamaDispenser.SetWorkTime(30f);
        go.AddOrGet<Demolishable>();
        go.GetComponent<Deconstructable>().allowDeconstruction = false;
    }
}