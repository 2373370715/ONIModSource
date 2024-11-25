using System.Collections.Generic;
using TUNING;
using UnityEngine;

public class TravelTubeEntranceConfig : IBuildingConfig {
    public const  string ID                                = "TravelTubeEntrance";
    public const  float  WAX_PER_LAUNCH                    = 0.05f;
    public const  int    STORAGE_WAX_LAUNCHECOUNT_CAPACITY = 200;
    private const float  JOULES_PER_LAUNCH                 = 10000f;
    private const float  LAUNCHES_FROM_FULL_CHARGE         = 4f;

    public override BuildingDef CreateBuildingDef() {
        var id                  = "TravelTubeEntrance";
        var width               = 3;
        var height              = 2;
        var anim                = "tube_launcher_kanim";
        var hitpoints           = 100;
        var construction_time   = 120f;
        var tier                = BUILDINGS.CONSTRUCTION_MASS_KG.TIER5;
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
                                                              BUILDINGS.DECOR.PENALTY.TIER1,
                                                              none);

        buildingDef.Overheatable = false;
        buildingDef.RequiresPowerInput = true;
        buildingDef.EnergyConsumptionWhenActive = 960f;
        buildingDef.Entombable = true;
        buildingDef.AudioCategory = "Metal";
        buildingDef.PowerInputOffset = new CellOffset(1, 0);
        buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(1, 1));
        return buildingDef;
    }

    public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag) {
        var travelTubeEntrance = go.AddOrGet<TravelTubeEntrance>();
        travelTubeEntrance.waxPerLaunch    = 0.05f;
        travelTubeEntrance.joulesPerLaunch = 10000f;
        travelTubeEntrance.jouleCapacity   = 40000f;
        var storage = go.AddOrGet<Storage>();
        storage.capacityKg = 10f;
        var defaultStoredItemModifiers = new List<Storage.StoredItemModifier> {
            Storage.StoredItemModifier.Hide,
            Storage.StoredItemModifier.Seal,
            Storage.StoredItemModifier.Insulate,
            Storage.StoredItemModifier.Preserve
        };

        storage.SetDefaultStoredItemModifiers(defaultStoredItemModifiers);
        var manualDeliveryKG = go.AddOrGet<ManualDeliveryKG>();
        manualDeliveryKG.requestedItemTag = SimHashes.MilkFat.CreateTag();
        manualDeliveryKG.choreTypeIDHash  = Db.Get().ChoreTypes.Fetch.IdHash;
        manualDeliveryKG.capacity         = storage.capacityKg;
        manualDeliveryKG.refillMass       = 0.05f;
        manualDeliveryKG.SetStorage(storage);
        go.AddOrGet<TravelTubeEntrance.Work>();
        go.AddOrGet<LogicOperationalController>();
        go.AddOrGet<EnergyConsumerSelfSustaining>();
    }

    public override void DoPostConfigureComplete(GameObject go) {
        go.GetComponent<RequireInputs>().visualizeRequirements = RequireInputs.Requirements.NoWire;
    }
}