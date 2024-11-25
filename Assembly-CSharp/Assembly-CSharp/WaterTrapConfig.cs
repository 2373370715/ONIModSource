using System.Collections.Generic;
using TUNING;
using UnityEngine;

public class WaterTrapConfig : IBuildingConfig {
    public const string ID                   = "WaterTrap";
    public const string OUTPUT_LOGIC_PORT_ID = "TRAP_HAS_PREY_STATUS_PORT";
    public const int    TRAIL_LENGTH         = 4;

    private static readonly List<Storage.StoredItemModifier> StoredItemModifiers
        = new List<Storage.StoredItemModifier>();

    public override BuildingDef CreateBuildingDef() {
        var buildingDef = BuildingTemplates.CreateBuildingDef("WaterTrap",
                                                              1,
                                                              2,
                                                              "critter_trap_water_kanim",
                                                              10,
                                                              10f,
                                                              BUILDINGS.CONSTRUCTION_MASS_KG.TIER3,
                                                              MATERIALS.RAW_METALS,
                                                              1600f,
                                                              BuildLocationRule.Anywhere,
                                                              BUILDINGS.DECOR.PENALTY.TIER2,
                                                              NOISE_POLLUTION.NOISY.TIER0);

        buildingDef.LogicInputPorts = new List<LogicPorts.Port> {
            LogicPorts.Port.InputPort(LogicOperationalController.PORT_ID,
                                      new CellOffset(0, 0),
                                      STRINGS.BUILDINGS.PREFABS.REUSABLETRAP.INPUT_LOGIC_PORT,
                                      STRINGS.BUILDINGS.PREFABS.REUSABLETRAP.INPUT_LOGIC_PORT_ACTIVE,
                                      STRINGS.BUILDINGS.PREFABS.REUSABLETRAP.INPUT_LOGIC_PORT_INACTIVE)
        };

        buildingDef.LogicOutputPorts = new List<LogicPorts.Port> {
            LogicPorts.Port.OutputPort("TRAP_HAS_PREY_STATUS_PORT",
                                       new CellOffset(0, 1),
                                       STRINGS.BUILDINGS.PREFABS.REUSABLETRAP.LOGIC_PORT,
                                       STRINGS.BUILDINGS.PREFABS.REUSABLETRAP.LOGIC_PORT_ACTIVE,
                                       STRINGS.BUILDINGS.PREFABS.REUSABLETRAP.LOGIC_PORT_INACTIVE)
        };

        buildingDef.AudioCategory = "Metal";
        buildingDef.Floodable     = false;
        return buildingDef;
    }

    public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag) {
        go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
        go.AddOrGet<Prioritizable>();
        Prioritizable.AddRef(go);
        var armTrapWorkable = go.AddOrGet<ArmTrapWorkable>();
        armTrapWorkable.overrideAnims  = new[] { Assets.GetAnim("anim_interacts_critter_trap_water_kanim") };
        armTrapWorkable.initialOffsets = new[] { new CellOffset(0, 1) };
        go.AddOrGet<Operational>();
        var rangeVisualizer = go.AddOrGet<RangeVisualizer>();
        rangeVisualizer.OriginOffset        = new Vector2I(0, 0);
        rangeVisualizer.BlockingTileVisible = false;
        var storage = go.AddOrGet<Storage>();
        storage.allowItemRemoval = true;
        storage.SetDefaultStoredItemModifiers(StoredItemModifiers);
        storage.sendOnStoreOnSpawn = true;
        var trapTrigger = go.AddOrGet<TrapTrigger>();
        trapTrigger.trappableCreatures = new[] { GameTags.Creatures.Swimmer };
        trapTrigger.trappedOffset      = new Vector2(0f, 1f);
        go.AddOrGetDef<WaterTrapTrail.Def>();
        var def = go.AddOrGetDef<ReusableTrap.Def>();
        def.releaseCellOffset                   = new CellOffset(0, 1);
        def.OUTPUT_LOGIC_PORT_ID                = "TRAP_HAS_PREY_STATUS_PORT";
        def.lures                               = new[] { GameTags.Creatures.FishTrapLure };
        def.usingSymbolChaseCapturingAnimations = true;
        def.getTrappedAnimationNameCallback     = () => "trapped";
        go.AddOrGet<LogicPorts>();
        go.AddOrGet<LogicOperationalController>();
    }

    private static void AddGuide(GameObject go, bool occupy_tiles) {
        var gameObject = new GameObject();
        gameObject.transform.parent = go.transform;
        gameObject.transform.SetLocalPosition(Vector3.zero);
        var kbatchedAnimController = gameObject.AddComponent<KBatchedAnimController>();
        kbatchedAnimController.Offset         = go.GetComponent<Building>().Def.GetVisualizerOffset();
        kbatchedAnimController.AnimFiles      = new[] { Assets.GetAnim(new HashedString("critter_trap_water_kanim")) };
        kbatchedAnimController.initialAnim    = "place_guide";
        kbatchedAnimController.visibilityType = KAnimControllerBase.VisibilityType.OffscreenUpdate;
        kbatchedAnimController.isMovable      = true;
        var waterTrapGuide = gameObject.AddComponent<WaterTrapGuide>();
        waterTrapGuide.parent      = go;
        waterTrapGuide.occupyTiles = occupy_tiles;
    }

    public override void DoPostConfigureComplete(GameObject go) {
        AddGuide(go.GetComponent<Building>().Def.BuildingPreview,           false);
        AddGuide(go.GetComponent<Building>().Def.BuildingUnderConstruction, false);
        var def = go.AddOrGetDef<Lure.Def>();
        def.defaultLurePoints                      = new[] { new CellOffset(0, 0) };
        def.radius                                 = 32;
        go.AddOrGet<FakeFloorAdder>().floorOffsets = new[] { new CellOffset(0, 0) };
    }
}