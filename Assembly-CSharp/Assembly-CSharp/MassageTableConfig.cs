using TUNING;
using UnityEngine;

public class MassageTableConfig : IBuildingConfig {
    public const string ID = "MassageTable";

    public override BuildingDef CreateBuildingDef() {
        var id                  = "MassageTable";
        var width               = 2;
        var height              = 2;
        var anim                = "masseur_kanim";
        var hitpoints           = 10;
        var construction_time   = 10f;
        var tier                = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
        var raw_MINERALS        = MATERIALS.RAW_MINERALS;
        var melting_point       = 1600f;
        var build_location_rule = BuildLocationRule.OnFloor;
        var tier2               = NOISE_POLLUTION.NOISY.TIER0;
        var buildingDef = BuildingTemplates.CreateBuildingDef(id,
                                                              width,
                                                              height,
                                                              anim,
                                                              hitpoints,
                                                              construction_time,
                                                              tier,
                                                              raw_MINERALS,
                                                              melting_point,
                                                              build_location_rule,
                                                              BUILDINGS.DECOR.NONE,
                                                              tier2);

        buildingDef.RequiresPowerInput          = true;
        buildingDef.PowerInputOffset            = new CellOffset(0, 0);
        buildingDef.Overheatable                = true;
        buildingDef.EnergyConsumptionWhenActive = 240f;
        buildingDef.ExhaustKilowattsWhenActive  = 0.125f;
        buildingDef.SelfHeatKilowattsWhenActive = 0.5f;
        buildingDef.AudioCategory               = "Metal";
        return buildingDef;
    }

    public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag) {
        go.AddOrGet<LoopingSounds>();
        go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.DeStressingBuilding);
        var massageTable = go.AddOrGet<MassageTable>();
        massageTable.overrideAnims               = new[] { Assets.GetAnim("anim_interacts_masseur_kanim") };
        massageTable.stressModificationValue     = -30f;
        massageTable.roomStressModificationValue = -60f;
        massageTable.workLayer                   = Grid.SceneLayer.BuildingFront;
        var ownable = go.AddOrGet<Ownable>();
        ownable.slotID      = Db.Get().AssignableSlots.MassageTable.Id;
        ownable.canBePublic = true;
        var roomTracker = go.AddOrGet<RoomTracker>();
        roomTracker.requiredRoomType = Db.Get().RoomTypes.MassageClinic.Id;
        roomTracker.requirement      = RoomTracker.Requirement.Recommended;
    }

    public override void DoPostConfigureComplete(GameObject go) {
        go.GetComponent<KAnimControllerBase>().initialAnim = "off";
        go.AddOrGet<CopyBuildingSettings>();
    }
}