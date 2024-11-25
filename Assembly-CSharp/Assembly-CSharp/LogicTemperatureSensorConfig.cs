using System.Collections.Generic;
using TUNING;
using UnityEngine;

public class LogicTemperatureSensorConfig : IBuildingConfig {
    public static string ID = "LogicTemperatureSensor";

    public override BuildingDef CreateBuildingDef() {
        var id                  = ID;
        var width               = 1;
        var height              = 1;
        var anim                = "switchthermal_kanim";
        var hitpoints           = 30;
        var construction_time   = 30f;
        var tier                = BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
        var refined_METALS      = MATERIALS.REFINED_METALS;
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
                                                              refined_METALS,
                                                              melting_point,
                                                              build_location_rule,
                                                              BUILDINGS.DECOR.PENALTY.TIER0,
                                                              none);

        buildingDef.Overheatable      = false;
        buildingDef.Floodable         = false;
        buildingDef.Entombable        = false;
        buildingDef.ViewMode          = OverlayModes.Logic.ID;
        buildingDef.AudioCategory     = "Metal";
        buildingDef.SceneLayer        = Grid.SceneLayer.Building;
        buildingDef.AlwaysOperational = true;
        buildingDef.LogicOutputPorts  = new List<LogicPorts.Port>();
        buildingDef.LogicOutputPorts.Add(LogicPorts.Port.OutputPort(LogicSwitch.PORT_ID,
                                                                    new CellOffset(0, 0),
                                                                    STRINGS.BUILDINGS.PREFABS.LOGICTEMPERATURESENSOR
                                                                           .LOGIC_PORT,
                                                                    STRINGS.BUILDINGS.PREFABS.LOGICTEMPERATURESENSOR
                                                                           .LOGIC_PORT_ACTIVE,
                                                                    STRINGS.BUILDINGS.PREFABS.LOGICTEMPERATURESENSOR
                                                                           .LOGIC_PORT_INACTIVE,
                                                                    true));

        SoundEventVolumeCache.instance.AddVolume("switchthermal_kanim", "PowerSwitch_on",  NOISE_POLLUTION.NOISY.TIER3);
        SoundEventVolumeCache.instance.AddVolume("switchthermal_kanim", "PowerSwitch_off", NOISE_POLLUTION.NOISY.TIER3);
        GeneratedBuildings.RegisterWithOverlay(OverlayModes.Logic.HighlightItemIDs, ID);
        return buildingDef;
    }

    public override void DoPostConfigureComplete(GameObject go) {
        var logicTemperatureSensor = go.AddOrGet<LogicTemperatureSensor>();
        logicTemperatureSensor.manuallyControlled = false;
        logicTemperatureSensor.minTemp            = 0f;
        logicTemperatureSensor.maxTemp            = 9999f;
        go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayInFrontOfConduits);
    }
}