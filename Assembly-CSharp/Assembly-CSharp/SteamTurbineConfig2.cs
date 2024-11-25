using System.Collections.Generic;
using TUNING;
using UnityEngine;

public class SteamTurbineConfig2 : IBuildingConfig {
    public const  string ID          = "SteamTurbine2";
    private const int    HEIGHT      = 3;
    private const int    WIDTH       = 5;
    public static float  MAX_WATTAGE = 850f;

    private static readonly List<Storage.StoredItemModifier> StoredItemModifiers
        = new List<Storage.StoredItemModifier> {
            Storage.StoredItemModifier.Hide, Storage.StoredItemModifier.Insulate, Storage.StoredItemModifier.Seal
        };

    public override BuildingDef CreateBuildingDef() {
        var      id                = "SteamTurbine2";
        var      width             = 5;
        var      height            = 3;
        var      anim              = "steamturbine2_kanim";
        var      hitpoints         = 30;
        var      construction_time = 60f;
        string[] array             = { "RefinedMetal", "Plastic" };
        float[] construction_mass = {
            BUILDINGS.CONSTRUCTION_MASS_KG.TIER5[0], BUILDINGS.CONSTRUCTION_MASS_KG.TIER3[0]
        };

        var construction_materials = array;
        var melting_point          = 1600f;
        var build_location_rule    = BuildLocationRule.OnFloor;
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
                                                              BUILDINGS.DECOR.NONE,
                                                              none,
                                                              1f);

        buildingDef.OutputConduitType = ConduitType.Liquid;
        buildingDef.UtilityOutputOffset = new CellOffset(2, 2);
        buildingDef.GeneratorWattageRating = MAX_WATTAGE;
        buildingDef.GeneratorBaseCapacity = MAX_WATTAGE;
        buildingDef.Entombable = true;
        buildingDef.IsFoundation = false;
        buildingDef.PermittedRotations = PermittedRotations.FlipH;
        buildingDef.ViewMode = OverlayModes.Power.ID;
        buildingDef.AudioCategory = "Metal";
        buildingDef.RequiresPowerOutput = true;
        buildingDef.PowerOutputOffset = new CellOffset(1, 0);
        buildingDef.OverheatTemperature = 1273.15f;
        buildingDef.SelfHeatKilowattsWhenActive = 4f;
        buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
        return buildingDef;
    }

    public override void DoPostConfigureUnderConstruction(GameObject go) {
        go.GetComponent<Constructable>().requiredSkillPerk = Db.Get().SkillPerks.CanPowerTinker.Id;
        AddVisualizer(go);
    }

    public override void DoPostConfigurePreview(BuildingDef def, GameObject go) { AddVisualizer(go); }

    public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag) {
        go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
        go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.PowerBuilding);
        go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.GeneratorType);
        go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.HeavyDutyGeneratorType);
    }

    public override void DoPostConfigureComplete(GameObject go) {
        var storage = go.AddComponent<Storage>();
        storage.showDescriptor = false;
        storage.showInUI       = false;
        storage.storageFilters = STORAGEFILTERS.LIQUIDS;
        storage.SetDefaultStoredItemModifiers(StoredItemModifiers);
        storage.capacityKg = 10f;
        var storage2 = go.AddComponent<Storage>();
        storage2.showDescriptor = false;
        storage2.showInUI       = false;
        storage2.storageFilters = STORAGEFILTERS.GASES;
        storage2.SetDefaultStoredItemModifiers(StoredItemModifiers);
        var steamTurbine = go.AddOrGet<SteamTurbine>();
        steamTurbine.srcElem                   = SimHashes.Steam;
        steamTurbine.destElem                  = SimHashes.Water;
        steamTurbine.pumpKGRate                = 2f;
        steamTurbine.maxSelfHeat               = 64f;
        steamTurbine.wasteHeatToTurbinePercent = 0.1f;
        var conduitDispenser = go.AddOrGet<ConduitDispenser>();
        conduitDispenser.elementFilter  = new[] { SimHashes.Water };
        conduitDispenser.conduitType    = ConduitType.Liquid;
        conduitDispenser.storage        = storage;
        conduitDispenser.alwaysDispense = true;
        go.AddOrGet<LogicOperationalController>();
        Prioritizable.AddRef(go);
        go.GetComponent<KPrefabID>().prefabSpawnFn += delegate(GameObject game_object) {
                                                          var handle
                                                              = GameComps.StructureTemperatures.GetHandle(game_object);

                                                          var payload
                                                              = GameComps.StructureTemperatures.GetPayload(handle);

                                                          var extents = game_object.GetComponent<Building>()
                                                              .GetExtents();

                                                          var newExtents
                                                              = new Extents(extents.x,
                                                                            extents.y - 1,
                                                                            extents.width,
                                                                            extents.height + 1);

                                                          payload.OverrideExtents(newExtents);
                                                          GameComps.StructureTemperatures.SetPayload(handle,
                                                           ref payload);

                                                          var components = game_object.GetComponents<Storage>();
                                                          game_object.GetComponent<SteamTurbine>()
                                                                     .SetStorage(components[1], components[0]);
                                                      };

        Tinkerable.MakePowerTinkerable(go);
        AddVisualizer(go);
    }

    private static void AddVisualizer(GameObject go) {
        RangeVisualizer rangeVisualizer = go2.AddOrGet<RangeVisualizer>();
        rangeVisualizer.RangeMin.x      = -2;
        rangeVisualizer.RangeMin.y      = -2;
        rangeVisualizer.RangeMax.x      = 2;
        rangeVisualizer.RangeMax.y      = -2;
        rangeVisualizer.TestLineOfSight = false;
        go2.GetComponent<KPrefabID>().instantiateFn += delegate(GameObject go) {
                                                           go.GetComponent<RangeVisualizer>().BlockingCb
                                                               = SteamTurbineBlockingCB;
                                                       };
    }

    public static bool SteamTurbineBlockingCB(int cell) {
        var element = ElementLoader.elements[Grid.ElementIdx[cell]];
        return element.IsLiquid || element.IsSolid;
    }
}