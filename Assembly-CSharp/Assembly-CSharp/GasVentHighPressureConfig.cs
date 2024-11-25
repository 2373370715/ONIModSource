using TUNING;
using UnityEngine;

public class GasVentHighPressureConfig : IBuildingConfig {
    public const  string      ID                = "GasVentHighPressure";
    private const ConduitType CONDUIT_TYPE      = ConduitType.Gas;
    public const  float       OVERPRESSURE_MASS = 20f;

    public override BuildingDef CreateBuildingDef() {
        var      id                = "GasVentHighPressure";
        var      width             = 1;
        var      height            = 1;
        var      anim              = "ventgas_powered_kanim";
        var      hitpoints         = 30;
        var      construction_time = 30f;
        string[] array             = { "RefinedMetal", "Plastic" };
        float[] construction_mass = {
            BUILDINGS.CONSTRUCTION_MASS_KG.TIER3[0], BUILDINGS.CONSTRUCTION_MASS_KG.TIER1[0]
        };

        var construction_materials = array;
        var melting_point          = 1600f;
        var build_location_rule    = BuildLocationRule.Anywhere;
        var none                   = NOISE_POLLUTION.NONE;
        var buildingDef = BuildingTemplates.CreateBuildingDef(id,
                                                              width,
                                                              height,
                                                              anim,
                                                              hitpoints,
                                                              construction_time,
                                                              construction_mass,
                                                              construction_materials,
                                                              melting_point,
                                                              build_location_rule,
                                                              BUILDINGS.DECOR.PENALTY.TIER1,
                                                              none);

        buildingDef.InputConduitType    = ConduitType.Gas;
        buildingDef.Floodable           = false;
        buildingDef.Overheatable        = false;
        buildingDef.ViewMode            = OverlayModes.GasConduits.ID;
        buildingDef.AudioCategory       = "Metal";
        buildingDef.UtilityInputOffset  = new CellOffset(0, 0);
        buildingDef.UtilityOutputOffset = new CellOffset(0, 0);
        buildingDef.LogicInputPorts     = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
        GeneratedBuildings.RegisterWithOverlay(OverlayScreen.GasVentIDs, "GasVentHighPressure");
        SoundEventVolumeCache.instance.AddVolume("ventgas_kanim", "GasVent_clunk", NOISE_POLLUTION.NOISY.TIER0);
        return buildingDef;
    }

    public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag) {
        go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
        go.AddOrGet<LoopingSounds>();
        go.AddOrGet<Exhaust>();
        go.AddOrGet<LogicOperationalController>();
        var vent = go.AddOrGet<Vent>();
        vent.conduitType      = ConduitType.Gas;
        vent.endpointType     = Endpoint.Sink;
        vent.overpressureMass = 20f;
        var conduitConsumer = go.AddOrGet<ConduitConsumer>();
        conduitConsumer.conduitType                         = ConduitType.Gas;
        conduitConsumer.ignoreMinMassCheck                  = true;
        BuildingTemplates.CreateDefaultStorage(go).showInUI = true;
        go.AddOrGet<SimpleVent>();
    }

    public override void DoPostConfigureComplete(GameObject go) {
        go.AddOrGetDef<VentController.Def>();
        go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayInFrontOfConduits);
    }
}