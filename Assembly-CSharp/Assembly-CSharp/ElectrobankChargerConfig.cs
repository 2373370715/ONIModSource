using TUNING;
using UnityEngine;

public class ElectrobankChargerConfig : IBuildingConfig {
    public const    string   ID          = "ElectrobankCharger";
    public const    float    CHARGE_RATE = 480f;
    public override string[] GetRequiredDlcIds() { return DlcManager.DLC3; }

    public override BuildingDef CreateBuildingDef() {
        var id                  = "ElectrobankCharger";
        var width               = 2;
        var height              = 2;
        var anim                = "electrobank_charger_small_kanim";
        var hitpoints           = 30;
        var construction_time   = 30f;
        var tier                = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
        var refined_METALS      = MATERIALS.REFINED_METALS;
        var melting_point       = 2400f;
        var build_location_rule = BuildLocationRule.OnFloor;
        var tier2               = NOISE_POLLUTION.NOISY.TIER1;
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
                                                              tier2);

        buildingDef.EnergyConsumptionWhenActive = 480f;
        buildingDef.ExhaustKilowattsWhenActive = 0f;
        buildingDef.SelfHeatKilowattsWhenActive = 1f;
        buildingDef.RequiresPowerInput = true;
        buildingDef.PowerInputOffset = new CellOffset(0, 0);
        buildingDef.ViewMode = OverlayModes.Power.ID;
        buildingDef.AudioCategory = "HollowMetal";
        buildingDef.AudioSize = "small";
        buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
        return buildingDef;
    }

    public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag) {
        go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
        var storage = go.AddOrGet<Storage>();
        storage.capacityKg = 1f;
        go.AddOrGet<LoopingSounds>();
        Prioritizable.AddRef(go);
        var manualDeliveryKG = go.AddOrGet<ManualDeliveryKG>();
        manualDeliveryKG.SetStorage(storage);
        manualDeliveryKG.RequestedItemTag = GameTags.EmptyPortableBattery;
        manualDeliveryKG.capacity         = storage.capacityKg;
        manualDeliveryKG.refillMass       = 20f;
        manualDeliveryKG.MassPerUnit      = 20f;
        manualDeliveryKG.MinimumMass      = 20f;
        manualDeliveryKG.choreTypeIDHash  = Db.Get().ChoreTypes.PowerFetch.IdHash;
    }

    public override void DoPostConfigureComplete(GameObject go) {
        go.AddOrGet<LogicOperationalController>();
        go.AddOrGetDef<ElectrobankCharger.Def>();
    }
}