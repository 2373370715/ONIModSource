using HarmonyLib;
using TUNING;
using UnityEngine;

#if 机械气闸
public class PresureDoorConfig2 : PressureDoorConfig {
    public override BuildingDef CreateBuildingDef() {
        var id                  = "PD";
        var width               = 1;
        var height              = 2;
        var anim                = "door_external_kanim";
        var hitpoints           = 30;
        var construction_time   = 60f;
        var tier                = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
        var all_METALS          = MATERIALS.ALL_METALS;
        var melting_point       = 1600f;
        var build_location_rule = BuildLocationRule.Tile;
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
                                                              BUILDINGS.DECOR.PENALTY.TIER1,
                                                              none,
                                                              0f);

        buildingDef.Overheatable       = false;
        buildingDef.RequiresPowerInput = false;

        // buildingDef.EnergyConsumptionWhenActive = 120f;
        buildingDef.Floodable    = false;
        buildingDef.Entombable   = false;
        buildingDef.IsFoundation = true;

        // buildingDef.ViewMode                    = OverlayModes.Power.ID;
        buildingDef.TileLayer           = ObjectLayer.FoundationTile;
        buildingDef.AudioCategory       = "Metal";
        buildingDef.PermittedRotations  = PermittedRotations.R90;
        buildingDef.SceneLayer          = Grid.SceneLayer.TileMain;
        buildingDef.ForegroundLayer     = Grid.SceneLayer.InteriorWall;
        buildingDef.ThermalConductivity = 0f;
        buildingDef.LogicInputPorts     = DoorConfig.CreateSingleInputPortList(new CellOffset(0, 0));
        SoundEventVolumeCache.instance.AddVolume("door_external_kanim",
                                                 "Open_DoorPressure",
                                                 NOISE_POLLUTION.NOISY.TIER2);

        SoundEventVolumeCache.instance.AddVolume("door_external_kanim",
                                                 "Close_DoorPressure",
                                                 NOISE_POLLUTION.NOISY.TIER2);

        return buildingDef;
    }

    public override void DoPostConfigureComplete(GameObject go) {
        var door = go.AddOrGet<Door>();
        door.hasComplexUserControls    = true;
        door.unpoweredAnimSpeed        = 建筑.基地.机械气闸.通电速度;
        door.poweredAnimSpeed          = 建筑.基地.机械气闸.通电速度;
        door.doorClosingSoundEventName = "MechanizedAirlock_closing";
        door.doorOpeningSoundEventName = "MechanizedAirlock_opening";
        go.AddOrGet<ZoneTile>();
        go.AddOrGet<AccessControl>();
        go.AddOrGet<KBoxCollider2D>();
        Prioritizable.AddRef(go);
        go.AddOrGet<CopyBuildingSettings>().copyGroupTag = GameTags.Door;
        go.AddOrGet<Workable>().workTime                 = 5f;
        Object.DestroyImmediate(go.GetComponent<BuildingEnabledButton>());
        go.GetComponent<AccessControl>().controlEnabled       = true;
        go.GetComponent<KBatchedAnimController>().initialAnim = "closed";
    }

    public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag) {
        GeneratedBuildings.MakeBuildingAlwaysOperational(go);
        BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
        var simCellOccupier = go.AddOrGet<SimCellOccupier>();
        simCellOccupier.setLiquidImpermeable = true;
        simCellOccupier.setGasImpermeable    = true;
        go.AddComponent<SimTemperatureTransfer>();
    }
}
#endif