using TUNING;
using UnityEngine;

public class TravelTubeConfig : IBuildingConfig {
    public const string ID = "TravelTube";

    public override BuildingDef CreateBuildingDef() {
        var id                  = "TravelTube";
        var width               = 1;
        var height              = 1;
        var anim                = "travel_tube_kanim";
        var hitpoints           = 30;
        var construction_time   = 10f;
        var tier                = BUILDINGS.CONSTRUCTION_MASS_KG.TIER1;
        var plastics            = MATERIALS.PLASTICS;
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
                                                              plastics,
                                                              melting_point,
                                                              build_location_rule,
                                                              BUILDINGS.DECOR.BONUS.TIER0,
                                                              none);

        buildingDef.Floodable           = false;
        buildingDef.Overheatable        = false;
        buildingDef.Entombable          = false;
        buildingDef.ObjectLayer         = ObjectLayer.Building;
        buildingDef.TileLayer           = ObjectLayer.TravelTubeTile;
        buildingDef.ReplacementLayer    = ObjectLayer.ReplacementTravelTube;
        buildingDef.AudioCategory       = "Plastic";
        buildingDef.AudioSize           = "small";
        buildingDef.BaseTimeUntilRepair = 0f;
        buildingDef.UtilityInputOffset  = new CellOffset(0, 0);
        buildingDef.UtilityOutputOffset = new CellOffset(0, 0);
        buildingDef.SceneLayer          = Grid.SceneLayer.BuildingFront;
        buildingDef.isKAnimTile         = true;
        buildingDef.isUtility           = true;
        buildingDef.DragBuild           = true;
        return buildingDef;
    }

    public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag) {
        GeneratedBuildings.MakeBuildingAlwaysOperational(go);
        BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
        go.AddOrGet<TravelTube>();
    }

    public override void DoPostConfigureUnderConstruction(GameObject go) {
        var kanimGraphTileVisualizer = go.AddComponent<KAnimGraphTileVisualizer>();
        kanimGraphTileVisualizer.connectionSource   = KAnimGraphTileVisualizer.ConnectionSource.Tube;
        kanimGraphTileVisualizer.isPhysicalBuilding = false;
    }

    public override void DoPostConfigureComplete(GameObject go) {
        go.GetComponent<Building>().Def.BuildingUnderConstruction.GetComponent<Constructable>().isDiggingRequired
            = false;

        var kanimGraphTileVisualizer = go.AddComponent<KAnimGraphTileVisualizer>();
        kanimGraphTileVisualizer.connectionSource   = KAnimGraphTileVisualizer.ConnectionSource.Tube;
        kanimGraphTileVisualizer.isPhysicalBuilding = true;
    }
}