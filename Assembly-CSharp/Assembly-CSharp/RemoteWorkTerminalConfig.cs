using System.Collections.Generic;
using TUNING;
using UnityEngine;

public class RemoteWorkTerminalConfig : IBuildingConfig {
    public const           float  INPUT_CAPACITY               = 10f;
    public const           float  INPUT_CONSUMPTION_RATE_PER_S = 0.006666667f;
    public const           float  INPUT_REFILL_RATIO           = 0.5f;
    public static          string ID                           = "RemoteWorkTerminal";
    public static readonly Tag    INPUT_MATERIAL               = new Tag("OrbitalResearchDatabank");

    public override BuildingDef CreateBuildingDef() {
        var id                  = ID;
        var width               = 3;
        var height              = 2;
        var anim                = "remote_work_terminal_kanim";
        var hitpoints           = 30;
        var construction_time   = 60f;
        var tier                = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
        var raw_METALS          = MATERIALS.RAW_METALS;
        var melting_point       = 1600f;
        var build_location_rule = BuildLocationRule.OnFloor;
        var tier2               = NOISE_POLLUTION.NOISY.TIER3;
        var buildingDef = BuildingTemplates.CreateBuildingDef(id,
                                                              width,
                                                              height,
                                                              anim,
                                                              hitpoints,
                                                              construction_time,
                                                              tier,
                                                              raw_METALS,
                                                              melting_point,
                                                              build_location_rule,
                                                              BUILDINGS.DECOR.BONUS.TIER2,
                                                              tier2);

        buildingDef.RequiresPowerInput          = true;
        buildingDef.EnergyConsumptionWhenActive = 120f;
        buildingDef.SelfHeatKilowattsWhenActive = 2f;
        buildingDef.ExhaustKilowattsWhenActive  = 0f;
        return buildingDef;
    }

    public override void DoPostConfigureComplete(GameObject go) {
        go.AddComponent<RemoteWorkTerminal>().workTime = float.PositiveInfinity;
        go.AddComponent<RemoteWorkTerminalSM>();
        go.AddOrGet<Operational>();
        var storage = go.AddOrGet<Storage>();
        storage.capacityKg = 100f;
        storage.showInUI   = true;
        storage.SetDefaultStoredItemModifiers(new List<Storage.StoredItemModifier> {
            Storage.StoredItemModifier.Hide, Storage.StoredItemModifier.Insulate
        });

        var manualDeliveryKG = go.AddOrGet<ManualDeliveryKG>();
        manualDeliveryKG.SetStorage(storage);
        manualDeliveryKG.RequestedItemTag       = INPUT_MATERIAL;
        manualDeliveryKG.refillMass             = 5f;
        manualDeliveryKG.capacity               = 10f;
        manualDeliveryKG.choreTypeIDHash        = Db.Get().ChoreTypes.ResearchFetch.IdHash;
        manualDeliveryKG.operationalRequirement = Operational.State.Functional;
        var elementConverter = go.AddOrGet<ElementConverter>();
        elementConverter.consumedElements
            = new[] { new ElementConverter.ConsumedElement(INPUT_MATERIAL, 0.006666667f) };

        elementConverter.showDescriptors = false;
        go.AddOrGet<ElementConverterOperationalRequirement>();
        Prioritizable.AddRef(go);
    }

    public override string[] GetRequiredDlcIds() { return new[] { "DLC3_ID" }; }
}