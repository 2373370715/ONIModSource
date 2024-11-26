using System.Collections.Generic;
using TUNING;
using UnityEngine;

public class CometDetectorConfig : IBuildingConfig {
    public const  float  COVERAGE_REQUIRED_01          = 0.5f;
    public const  float  BEST_WARNING_TIME_IN_SECONDS  = 200f;
    public const  float  WORST_WARNING_TIME_IN_SECONDS = 1f;
    public const  int    SCAN_RADIUS                   = 15;
    public const  float  LOGIC_SIGNAL_DELAY_ON_LOAD    = 3f;
    public static string ID                            = "CometDetector";

    public static readonly SkyVisibilityInfo SKY_VISIBILITY_INFO
        = new SkyVisibilityInfo(new CellOffset(0, 0), 15, new CellOffset(0, 0), 15, 1);

    public override BuildingDef CreateBuildingDef() {
        var id                  = ID;
        var width               = 2;
        var height              = 4;
        var anim                = "meteor_detector_kanim";
        var hitpoints           = 30;
        var construction_time   = 30f;
        var tier                = BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
        var refined_METALS      = MATERIALS.REFINED_METALS;
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
                                                              refined_METALS,
                                                              melting_point,
                                                              build_location_rule,
                                                              BUILDINGS.DECOR.PENALTY.TIER0,
                                                              none);

        buildingDef.Overheatable                = false;
        buildingDef.Floodable                   = true;
        buildingDef.Entombable                  = true;
        buildingDef.RequiresPowerInput          = true;
        buildingDef.AddLogicPowerPort           = false;
        buildingDef.EnergyConsumptionWhenActive = 120f;
        buildingDef.ViewMode                    = OverlayModes.Logic.ID;
        buildingDef.AudioCategory               = "Metal";
        buildingDef.SceneLayer                  = Grid.SceneLayer.Building;
        buildingDef.LogicOutputPorts = new List<LogicPorts.Port> {
            LogicPorts.Port.OutputPort(LogicSwitch.PORT_ID,
                                       new CellOffset(0, 0),
                                       STRINGS.BUILDINGS.PREFABS.LOGICSWITCH.LOGIC_PORT,
                                       STRINGS.BUILDINGS.PREFABS.LOGICSWITCH.LOGIC_PORT_ACTIVE,
                                       STRINGS.BUILDINGS.PREFABS.LOGICSWITCH.LOGIC_PORT_INACTIVE,
                                       true)
        };

        SoundEventVolumeCache.instance.AddVolume("world_element_sensor_kanim",
                                                 "PowerSwitch_on",
                                                 NOISE_POLLUTION.NOISY.TIER3);

        SoundEventVolumeCache.instance.AddVolume("world_element_sensor_kanim",
                                                 "PowerSwitch_off",
                                                 NOISE_POLLUTION.NOISY.TIER3);

        GeneratedBuildings.RegisterWithOverlay(OverlayModes.Logic.HighlightItemIDs, ID);
        return buildingDef;
    }

    public override void DoPostConfigureComplete(GameObject go) {
        go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
        if (DlcManager.IsExpansion1Active())
            go.AddOrGetDef<ClusterCometDetector.Def>();
        else
            go.AddOrGetDef<CometDetector.Def>();

        go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayBehindConduits);
        AddVisualizer(go);
    }

    public override void DoPostConfigurePreview(BuildingDef          def, GameObject go) { AddVisualizer(go); }
    public override void DoPostConfigureUnderConstruction(GameObject go) { AddVisualizer(go); }

    private static void AddVisualizer(GameObject prefab) {
        var scannerNetworkVisualizer = prefab.AddOrGet<ScannerNetworkVisualizer>();
        scannerNetworkVisualizer.RangeMin = -15;
        scannerNetworkVisualizer.RangeMax = 15;
    }
}