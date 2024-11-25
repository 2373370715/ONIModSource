using TUNING;
using UnityEngine;

public class DevHEPSpawnerConfig : IBuildingConfig {
    public const    string   ID = "DevHEPSpawner";
    public override string[] GetRequiredDlcIds() { return DlcManager.EXPANSION1; }

    public override BuildingDef CreateBuildingDef() {
        var id                  = "DevHEPSpawner";
        var width               = 1;
        var height              = 1;
        var anim                = "dev_radbolt_generator_kanim";
        var hitpoints           = 30;
        var construction_time   = 10f;
        var tier                = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
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
                                                              BUILDINGS.DECOR.PENALTY.TIER1,
                                                              none);

        buildingDef.Floodable = false;
        buildingDef.Invincible = true;
        buildingDef.Overheatable = false;
        buildingDef.Entombable = false;
        buildingDef.AudioCategory = "Metal";
        buildingDef.Overheatable = false;
        buildingDef.ViewMode = OverlayModes.Radiation.ID;
        buildingDef.PermittedRotations = PermittedRotations.R360;
        buildingDef.UseHighEnergyParticleOutputPort = true;
        buildingDef.HighEnergyParticleOutputOffset = new CellOffset(0, 0);
        buildingDef.RequiresPowerInput = false;
        buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
        GeneratedBuildings.RegisterWithOverlay(OverlayScreen.RadiationIDs, "DevHEPSpawner");
        buildingDef.Deprecated = !Sim.IsRadiationEnabled();
        buildingDef.DebugOnly  = true;
        return buildingDef;
    }

    public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag) {
        go.AddTag(GameTags.DevBuilding);
        BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
        Prioritizable.AddRef(go);
        go.AddOrGet<LoopingSounds>();
        go.AddOrGet<DevHEPSpawner>().boltAmount = 50f;
        go.AddOrGet<LogicOperationalController>();
    }

    public override void DoPostConfigureComplete(GameObject go) { }
}