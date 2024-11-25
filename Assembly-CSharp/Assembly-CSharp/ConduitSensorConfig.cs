using System.Collections.Generic;
using TUNING;
using UnityEngine;

public abstract class ConduitSensorConfig : IBuildingConfig {
    protected abstract ConduitType ConduitType { get; }

    protected BuildingDef CreateBuildingDef(string                ID,
                                            string                anim,
                                            float[]               required_mass,
                                            string[]              required_materials,
                                            List<LogicPorts.Port> output_ports) {
        var width               = 1;
        var height              = 1;
        var hitpoints           = 30;
        var construction_time   = 30f;
        var melting_point       = 1600f;
        var build_location_rule = BuildLocationRule.Anywhere;
        var none                = NOISE_POLLUTION.NONE;
        var buildingDef = BuildingTemplates.CreateBuildingDef(ID,
                                                              width,
                                                              height,
                                                              anim,
                                                              hitpoints,
                                                              construction_time,
                                                              required_mass,
                                                              required_materials,
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
        buildingDef.LogicOutputPorts  = output_ports;
        SoundEventVolumeCache.instance.AddVolume(anim, "PowerSwitch_on",  NOISE_POLLUTION.NOISY.TIER3);
        SoundEventVolumeCache.instance.AddVolume(anim, "PowerSwitch_off", NOISE_POLLUTION.NOISY.TIER3);
        GeneratedBuildings.RegisterWithOverlay(OverlayModes.Logic.HighlightItemIDs, ID);
        return buildingDef;
    }

    public override void DoPostConfigureComplete(GameObject go) { }
}