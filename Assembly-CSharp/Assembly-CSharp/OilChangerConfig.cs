using TUNING;
using UnityEngine;

public class OilChangerConfig : IBuildingConfig {
    public const    string   ID           = "OilChanger";
    public          float    OIL_CAPACITY = 400f;
    public override string[] GetRequiredDlcIds() { return DlcManager.DLC3; }

    public override BuildingDef CreateBuildingDef() {
        var id                  = "OilChanger";
        var width               = 3;
        var height              = 3;
        var anim                = "oilchange_station_kanim";
        var hitpoints           = 30;
        var construction_time   = 60f;
        var tier                = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
        var raw_METALS          = MATERIALS.RAW_METALS;
        var melting_point       = 800f;
        var build_location_rule = BuildLocationRule.OnFloor;
        var none                = NOISE_POLLUTION.NONE;
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
                                                              BUILDINGS.DECOR.PENALTY.TIER2,
                                                              none);

        buildingDef.Overheatable                = false;
        buildingDef.ExhaustKilowattsWhenActive  = 0.25f;
        buildingDef.SelfHeatKilowattsWhenActive = 0f;
        buildingDef.InputConduitType            = ConduitType.Liquid;
        buildingDef.UtilityInputOffset          = new CellOffset(-1, 0);
        buildingDef.ViewMode                    = OverlayModes.LiquidConduits.ID;
        buildingDef.AudioCategory               = "Metal";
        buildingDef.PermittedRotations          = PermittedRotations.Unrotatable;
        return buildingDef;
    }

    public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag) {
        go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.BionicUpkeepType);
        var storage = go.AddComponent<Storage>();
        storage.capacityKg = OIL_CAPACITY;
        storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
        var oilChangerWorkableUse = go.AddOrGet<OilChangerWorkableUse>();
        oilChangerWorkableUse.overrideAnims       = new[] { Assets.GetAnim("anim_interacts_oilchange_kanim") };
        oilChangerWorkableUse.resetProgressOnStop = true;
        var conduitConsumer = go.AddOrGet<ConduitConsumer>();
        conduitConsumer.forceAlwaysSatisfied = true;
        conduitConsumer.conduitType          = ConduitType.Liquid;
        conduitConsumer.capacityTag          = GameTags.LubricatingOil;
        conduitConsumer.capacityKG           = OIL_CAPACITY;
        conduitConsumer.wrongElementResult   = ConduitConsumer.WrongElementResult.Dump;
        go.AddOrGetDef<OilChanger.Def>();
    }

    public override void DoPostConfigureComplete(GameObject go) { }
}