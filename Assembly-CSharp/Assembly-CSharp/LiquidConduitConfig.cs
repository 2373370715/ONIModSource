﻿using System.Collections.Generic;
using TUNING;
using UnityEngine;

// 液体管道配置
public class LiquidConduitConfig : IBuildingConfig {
    public const  string ID = "LiquidConduit";
    public static void CommonConduitPostConfigureComplete(GameObject go) { GeneratedBuildings.RemoveLoopingSounds(go); }

    public override BuildingDef CreateBuildingDef() {
        var id                  = "LiquidConduit";
        var width               = 1;
        var height              = 1;
        var anim                = "utilities_liquid_kanim";
        var hitpoints           = 10;
        var construction_time   = 3f;
        var tier                = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
        var plumbable_OR_METALS = MATERIALS.PLUMBABLE_OR_METALS;
        var melting_point       = 1600f;
        var build_location_rule = BuildLocationRule.Anywhere;
        var none                = NOISE_POLLUTION.NONE;
        var buildingDef = BuildingTemplates.CreateBuildingDef(id,
                                                              width,
                                                              height,
                                                              anim,
                                                              hitpoints,
                                                              construction_time,
                                                              tier,
                                                              plumbable_OR_METALS,
                                                              melting_point,
                                                              build_location_rule,
                                                              BUILDINGS.DECOR.NONE,
                                                              none);

        buildingDef.Floodable           = false;
        buildingDef.Overheatable        = false;
        buildingDef.Entombable          = false;
        buildingDef.ViewMode            = OverlayModes.LiquidConduits.ID;
        buildingDef.ObjectLayer         = ObjectLayer.LiquidConduit;
        buildingDef.TileLayer           = ObjectLayer.LiquidConduitTile;
        buildingDef.ReplacementLayer    = ObjectLayer.ReplacementLiquidConduit;
        buildingDef.AudioCategory       = "Metal";
        buildingDef.AudioSize           = "small";
        buildingDef.BaseTimeUntilRepair = -1f;
        buildingDef.UtilityInputOffset  = new CellOffset(0, 0);
        buildingDef.UtilityOutputOffset = new CellOffset(0, 0);
        buildingDef.SceneLayer          = Grid.SceneLayer.LiquidConduits;
        buildingDef.isKAnimTile         = true;
        buildingDef.isUtility           = true;
        buildingDef.DragBuild           = true;
        buildingDef.ReplacementTags     = new List<Tag>();
        buildingDef.ReplacementTags.Add(GameTags.Pipes);
        GeneratedBuildings.RegisterWithOverlay(OverlayScreen.LiquidVentIDs, "LiquidConduit");
        return buildingDef;
    }

    public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag) {
        GeneratedBuildings.MakeBuildingAlwaysOperational(go);
        BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
        go.AddOrGet<Conduit>().type = ConduitType.Liquid;
    }

    public override void DoPostConfigureComplete(GameObject go) {
        go.GetComponent<Building>().Def.BuildingUnderConstruction.GetComponent<Constructable>().isDiggingRequired
            = false;

        go.AddComponent<EmptyConduitWorkable>();
        var kanimGraphTileVisualizer = go.AddComponent<KAnimGraphTileVisualizer>();
        kanimGraphTileVisualizer.connectionSource   = KAnimGraphTileVisualizer.ConnectionSource.Liquid;
        kanimGraphTileVisualizer.isPhysicalBuilding = true;
        go.GetComponent<KPrefabID>().AddTag(GameTags.Pipes);
        CommonConduitPostConfigureComplete(go);
    }

    public override void DoPostConfigureUnderConstruction(GameObject go) {
        var kanimGraphTileVisualizer = go.AddComponent<KAnimGraphTileVisualizer>();
        kanimGraphTileVisualizer.connectionSource   = KAnimGraphTileVisualizer.ConnectionSource.Liquid;
        kanimGraphTileVisualizer.isPhysicalBuilding = false;
    }
}