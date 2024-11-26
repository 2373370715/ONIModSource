using System.Collections.Generic;
using TUNING;
using UnityEngine;

public class SolidConduitInboxConfig : IBuildingConfig {
    public const string ID = "SolidConduitInbox";

    public override BuildingDef CreateBuildingDef() {
        var id                  = "SolidConduitInbox";
        var width               = 1;
        var height              = 2;
        var anim                = "conveyorin_kanim";
        var hitpoints           = 100;
        var construction_time   = 60f;
        var tier                = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
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
                                                              BUILDINGS.DECOR.PENALTY.TIER1,
                                                              none);

        buildingDef.RequiresPowerInput = true;
        buildingDef.EnergyConsumptionWhenActive = 120f;
        buildingDef.ExhaustKilowattsWhenActive = 0f;
        buildingDef.SelfHeatKilowattsWhenActive = 2f;
        buildingDef.Floodable = false;
        buildingDef.ViewMode = OverlayModes.SolidConveyor.ID;
        buildingDef.AudioCategory = "Metal";
        buildingDef.OutputConduitType = ConduitType.Solid;
        buildingDef.PowerInputOffset = new CellOffset(0, 1);
        buildingDef.UtilityOutputOffset = new CellOffset(0, 0);
        buildingDef.PermittedRotations = PermittedRotations.R360;
        buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 1));
        GeneratedBuildings.RegisterWithOverlay(OverlayScreen.SolidConveyorIDs, "SolidConduitInbox");
        return buildingDef;
    }

    public override void DoPostConfigureUnderConstruction(GameObject go) {
        go.GetComponent<Constructable>().requiredSkillPerk = Db.Get().SkillPerks.ConveyorBuild.Id;
    }

    public override void DoPostConfigureComplete(GameObject go) {
        go.AddOrGet<LogicOperationalController>();
        Prioritizable.AddRef(go);
        go.AddOrGet<EnergyConsumer>();
        go.AddOrGet<Automatable>();
        var list = new List<Tag>();
        list.AddRange(STORAGEFILTERS.STORAGE_LOCKERS_STANDARD);
        list.AddRange(STORAGEFILTERS.FOOD);
        var storage = go.AddOrGet<Storage>();
        storage.capacityKg                    = 1000f;
        storage.showInUI                      = true;
        storage.showDescriptor                = true;
        storage.storageFilters                = list;
        storage.allowItemRemoval              = false;
        storage.onlyTransferFromLowerPriority = true;
        storage.showCapacityStatusItem        = true;
        storage.showCapacityAsMainStatus      = true;
        go.AddOrGet<TreeFilterable>();
        go.AddOrGet<SolidConduitInbox>();
        go.AddOrGet<SolidConduitDispenser>();
    }
}