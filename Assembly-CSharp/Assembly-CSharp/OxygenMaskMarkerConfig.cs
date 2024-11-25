using TUNING;
using UnityEngine;

public class OxygenMaskMarkerConfig : IBuildingConfig {
    public const string ID = "OxygenMaskMarker";

    public override BuildingDef CreateBuildingDef() {
        var id                     = "OxygenMaskMarker";
        var width                  = 1;
        var height                 = 2;
        var anim                   = "oxygen_checkpoint_arrow_kanim";
        var hitpoints              = 30;
        var construction_time      = 30f;
        var raw_METALS             = MATERIALS.RAW_METALS;
        var tier                   = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
        var construction_materials = raw_METALS;
        var melting_point          = 1600f;
        var build_location_rule    = BuildLocationRule.OnFloor;
        var none                   = NOISE_POLLUTION.NONE;
        var buildingDef = BuildingTemplates.CreateBuildingDef(id,
                                                              width,
                                                              height,
                                                              anim,
                                                              hitpoints,
                                                              construction_time,
                                                              tier,
                                                              construction_materials,
                                                              melting_point,
                                                              build_location_rule,
                                                              BUILDINGS.DECOR.BONUS.TIER1,
                                                              none);

        buildingDef.PermittedRotations = PermittedRotations.FlipH;
        buildingDef.PreventIdleTraversalPastBuilding = true;
        buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
        GeneratedBuildings.RegisterWithOverlay(OverlayScreen.SuitIDs, "OxygenMaskMarker");
        return buildingDef;
    }

    public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag) {
        var suitMarker = go.AddOrGet<SuitMarker>();
        suitMarker.LockerTags            = new[] { new Tag("OxygenMaskLocker") };
        suitMarker.PathFlag              = PathFinder.PotentialPath.Flags.HasOxygenMask;
        go.AddOrGet<AnimTileable>().tags = new[] { new Tag("OxygenMaskMarker"), new Tag("OxygenMaskLocker") };
        go.AddTag(GameTags.JetSuitBlocker);
    }

    public override void DoPostConfigureComplete(GameObject go) { go.AddOrGet<LogicOperationalController>(); }
}