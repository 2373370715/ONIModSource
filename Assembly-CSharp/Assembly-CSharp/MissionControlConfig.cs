using TUNING;
using UnityEngine;

public class MissionControlConfig : IBuildingConfig {
    public const string ID                   = "MissionControl";
    public const float  EFFECT_DURATION      = 600f;
    public const float  SPEED_MULTIPLIER     = 1.2f;
    public const int    SCAN_RADIUS          = 1;
    public const int    VERTICAL_SCAN_OFFSET = 2;

    public static readonly SkyVisibilityInfo SKY_VISIBILITY_INFO
        = new SkyVisibilityInfo(new CellOffset(0, 2), 1, new CellOffset(0, 2), 1, 0);

    public override string[] GetForbiddenDlcIds() { return DlcManager.EXPANSION1; }

    public override BuildingDef CreateBuildingDef() {
        var id                  = "MissionControl";
        var width               = 3;
        var height              = 3;
        var anim                = "mission_control_station_kanim";
        var hitpoints           = 100;
        var construction_time   = 30f;
        var tier                = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
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
                                                              BUILDINGS.DECOR.PENALTY.TIER0,
                                                              none);

        buildingDef.RequiresPowerInput          = true;
        buildingDef.EnergyConsumptionWhenActive = 960f;
        buildingDef.ExhaustKilowattsWhenActive  = 0.5f;
        buildingDef.SelfHeatKilowattsWhenActive = 2f;
        buildingDef.ViewMode                    = OverlayModes.Power.ID;
        buildingDef.AudioCategory               = "Metal";
        buildingDef.AudioSize                   = "large";
        buildingDef.DefaultAnimState            = "off";
        return buildingDef;
    }

    public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag) {
        go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.ScienceBuilding);
        var def = go.GetComponent<BuildingComplete>().Def;
        Prioritizable.AddRef(go);
        go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
        go.AddOrGetDef<PoweredController.Def>();
        go.AddOrGetDef<SkyVisibilityMonitor.Def>().skyVisibilityInfo = SKY_VISIBILITY_INFO;
        go.AddOrGetDef<MissionControl.Def>();
        var missionControlWorkable = go.AddOrGet<MissionControlWorkable>();
        missionControlWorkable.requiredSkillPerk = Db.Get().SkillPerks.CanMissionControl.Id;
        missionControlWorkable.workLayer         = Grid.SceneLayer.BuildingUse;
    }

    public override void DoPostConfigureComplete(GameObject go) {
        var roomTracker = go.AddOrGet<RoomTracker>();
        roomTracker.requiredRoomType = Db.Get().RoomTypes.Laboratory.Id;
        roomTracker.requirement      = RoomTracker.Requirement.Required;
        AddVisualizer(go);
    }

    public override void DoPostConfigurePreview(BuildingDef          def, GameObject go) { AddVisualizer(go); }
    public override void DoPostConfigureUnderConstruction(GameObject go) { AddVisualizer(go); }

    private static void AddVisualizer(GameObject prefab) {
        var skyVisibilityVisualizer = prefab.AddOrGet<SkyVisibilityVisualizer>();
        skyVisibilityVisualizer.OriginOffset.y        = 2;
        skyVisibilityVisualizer.RangeMin              = -1;
        skyVisibilityVisualizer.RangeMax              = 1;
        skyVisibilityVisualizer.SkipOnModuleInteriors = true;
    }
}