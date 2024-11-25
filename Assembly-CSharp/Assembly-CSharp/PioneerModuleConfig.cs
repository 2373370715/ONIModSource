using System.Collections.Generic;
using TUNING;
using UnityEngine;

public class PioneerModuleConfig : IBuildingConfig {
    public const    string   ID = "PioneerModule";
    public override string[] GetRequiredDlcIds() { return DlcManager.EXPANSION1; }

    public override BuildingDef CreateBuildingDef() {
        var id                  = "PioneerModule";
        var width               = 3;
        var height              = 3;
        var anim                = "rocket_pioneer_cargo_module_kanim";
        var hitpoints           = 1000;
        var construction_time   = 30f;
        var hollow_TIER         = BUILDINGS.ROCKETRY_MASS_KG.HOLLOW_TIER1;
        var refined_METALS      = MATERIALS.REFINED_METALS;
        var melting_point       = 9999f;
        var build_location_rule = BuildLocationRule.Anywhere;
        var tier                = NOISE_POLLUTION.NOISY.TIER2;
        var buildingDef = BuildingTemplates.CreateBuildingDef(id,
                                                              width,
                                                              height,
                                                              anim,
                                                              hitpoints,
                                                              construction_time,
                                                              hollow_TIER,
                                                              refined_METALS,
                                                              melting_point,
                                                              build_location_rule,
                                                              BUILDINGS.DECOR.NONE,
                                                              tier);

        BuildingTemplates.CreateRocketBuildingDef(buildingDef);
        buildingDef.DefaultAnimState    = "deployed";
        buildingDef.AttachmentSlotTag   = GameTags.Rocket;
        buildingDef.SceneLayer          = Grid.SceneLayer.Building;
        buildingDef.ForegroundLayer     = Grid.SceneLayer.Front;
        buildingDef.OverheatTemperature = 2273.15f;
        buildingDef.Floodable           = false;
        buildingDef.ObjectLayer         = ObjectLayer.Building;
        buildingDef.RequiresPowerInput  = false;
        buildingDef.CanMove             = true;
        buildingDef.Cancellable         = false;
        return buildingDef;
    }

    public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag) {
        BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
        go.AddOrGet<LoopingSounds>();
        go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
        var storage = go.AddComponent<Storage>();
        storage.showInUI = true;
        storage.SetDefaultStoredItemModifiers(Storage.StandardInsulatedStorage);
        var def = go.AddOrGetDef<BuildingInternalConstructor.Def>();
        def.constructionMass   = 400f;
        def.outputIDs          = new List<string> { "PioneerLander" };
        def.spawnIntoStorage   = true;
        def.storage            = storage;
        def.constructionSymbol = "under_construction";
        go.AddOrGet<BuildingInternalConstructorWorkable>().SetWorkTime(30f);
        var def2 = go.AddOrGetDef<JettisonableCargoModule.Def>();
        def2.landerPrefabID       = "PioneerLander".ToTag();
        def2.landerContainer      = storage;
        def2.clusterMapFXPrefabID = "DeployingPioneerLanderFX";
        go.AddOrGet<BuildingAttachPoint>().points = new[] {
            new BuildingAttachPoint.HardPoint(new CellOffset(0, 3), GameTags.Rocket, null)
        };

        go.AddOrGet<NavTeleporter>();
    }

    public override void DoPostConfigureComplete(GameObject go) {
        Prioritizable.AddRef(go);
        BuildingTemplates.ExtendBuildingToRocketModuleCluster(go, null, ROCKETRY.BURDEN.MODERATE);
        var fakeFloorAdder = go.AddOrGet<FakeFloorAdder>();
        fakeFloorAdder.floorOffsets    = new[] { new CellOffset(-1, -1), new CellOffset(0, -1), new CellOffset(1, -1) };
        fakeFloorAdder.initiallyActive = false;
    }
}