using TUNING;
using UnityEngine;

public class WoodGasGeneratorConfig : IBuildingConfig {
    public const  string    ID                     = "WoodGasGenerator";
    private const float     BRANCHES_PER_GENERATOR = 8f;
    public const  float     CONSUMPTION_RATE       = 1.2f;
    private const float     WOOD_PER_REFILL        = 360f;
    private const SimHashes EXHAUST_ELEMENT_GAS    = SimHashes.CarbonDioxide;
    private const SimHashes EXHAUST_ELEMENT_GAS2   = SimHashes.Syngas;
    public const  float     CO2_EXHAUST_RATE       = 0.17f;
    private const int       WIDTH                  = 2;
    private const int       HEIGHT                 = 2;

    public override BuildingDef CreateBuildingDef() {
        var id                     = "WoodGasGenerator";
        var width                  = 2;
        var height                 = 2;
        var anim                   = "generatorwood_kanim";
        var hitpoints              = 100;
        var construction_time      = 120f;
        var all_METALS             = MATERIALS.ALL_METALS;
        var tier                   = BUILDINGS.CONSTRUCTION_MASS_KG.TIER5;
        var construction_materials = all_METALS;
        var melting_point          = 2400f;
        var build_location_rule    = BuildLocationRule.OnFloor;
        var tier2                  = NOISE_POLLUTION.NOISY.TIER5;
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
                                                              BUILDINGS.DECOR.PENALTY.TIER2,
                                                              tier2);

        buildingDef.GeneratorWattageRating = 300f;
        buildingDef.GeneratorBaseCapacity = buildingDef.GeneratorWattageRating;
        buildingDef.ExhaustKilowattsWhenActive = 8f;
        buildingDef.SelfHeatKilowattsWhenActive = 1f;
        buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(1, 1));
        buildingDef.ViewMode = OverlayModes.Power.ID;
        buildingDef.AudioCategory = "Metal";
        buildingDef.RequiresPowerOutput = true;
        buildingDef.PowerOutputOffset = new CellOffset(0, 0);
        return buildingDef;
    }

    public override void DoPostConfigureComplete(GameObject go) {
        go.AddOrGet<LogicOperationalController>();
        go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
        go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.PowerBuilding);
        go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.GeneratorType);
        go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.HeavyDutyGeneratorType);
        go.AddOrGet<LoopingSounds>();
        var storage = go.AddOrGet<Storage>();
        storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
        storage.showInUI = true;
        var max_stored_input_mass = 720f;
        go.AddOrGet<LoopingSounds>();
        var manualDeliveryKG = go.AddOrGet<ManualDeliveryKG>();
        manualDeliveryKG.SetStorage(storage);
        manualDeliveryKG.RequestedItemTag = SimHashes.WoodLog.CreateTag();
        manualDeliveryKG.capacity         = 360f;
        manualDeliveryKG.refillMass       = 180f;
        manualDeliveryKG.choreTypeIDHash  = Db.Get().ChoreTypes.FetchCritical.IdHash;
        var energyGenerator = go.AddOrGet<EnergyGenerator>();
        energyGenerator.powerDistributionOrder = 8;
        energyGenerator.hasMeter               = true;
        energyGenerator.formula = EnergyGenerator.CreateSimpleFormula(SimHashes.WoodLog.CreateTag(),
                                                                      1.2f,
                                                                      max_stored_input_mass,
                                                                      SimHashes.CarbonDioxide,
                                                                      0.17f,
                                                                      false,
                                                                      new CellOffset(0, 1),
                                                                      383.15f);

        Tinkerable.MakePowerTinkerable(go);
        go.AddOrGetDef<PoweredActiveController.Def>();
    }
}