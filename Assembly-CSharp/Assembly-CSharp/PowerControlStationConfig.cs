using System.Collections.Generic;
using TUNING;
using UnityEngine;

public class PowerControlStationConfig : IBuildingConfig {
    public const  string ID                  = "PowerControlStation";
    public const  float  MASS_PER_TINKER     = 5f;
    public const  float  OUTPUT_TEMPERATURE  = 308.15f;
    public static Tag    MATERIAL_FOR_TINKER = GameTags.RefinedMetal;
    public static Tag    TINKER_TOOLS        = PowerStationToolsConfig.tag;
    public static string ROLE_PERK           = "CanPowerTinker";

    public override BuildingDef CreateBuildingDef() {
        var id                  = "PowerControlStation";
        var width               = 2;
        var height              = 4;
        var anim                = "electricianworkdesk_kanim";
        var hitpoints           = 30;
        var construction_time   = 30f;
        var tier                = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
        var refined_METALS      = MATERIALS.REFINED_METALS;
        var melting_point       = 1600f;
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
                                                              BUILDINGS.DECOR.NONE,
                                                              tier2);

        buildingDef.ViewMode        = OverlayModes.Rooms.ID;
        buildingDef.Overheatable    = false;
        buildingDef.AudioCategory   = "Metal";
        buildingDef.AudioSize       = "large";
        buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
        return buildingDef;
    }

    public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag) {
        go.AddOrGet<LoopingSounds>();
        go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
        go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.PowerBuilding);
    }

    public override void DoPostConfigureComplete(GameObject go) {
        go.AddOrGet<LogicOperationalController>();
        var storage = go.AddOrGet<Storage>();
        storage.capacityKg     = 50f;
        storage.showInUI       = true;
        storage.storageFilters = new List<Tag> { MATERIAL_FOR_TINKER };
        var tinkerstation = go.AddOrGet<TinkerStation>();
        tinkerstation.overrideAnims      = new[] { Assets.GetAnim("anim_interacts_electricianworkdesk_kanim") };
        tinkerstation.inputMaterial      = MATERIAL_FOR_TINKER;
        tinkerstation.massPerTinker      = 5f;
        tinkerstation.outputPrefab       = TINKER_TOOLS;
        tinkerstation.outputTemperature  = 308.15f;
        tinkerstation.requiredSkillPerk  = ROLE_PERK;
        tinkerstation.choreType          = Db.Get().ChoreTypes.PowerFabricate.IdHash;
        tinkerstation.useFilteredStorage = true;
        tinkerstation.fetchChoreType     = Db.Get().ChoreTypes.PowerFetch.IdHash;
        var roomTracker = go.AddOrGet<RoomTracker>();
        roomTracker.requiredRoomType = Db.Get().RoomTypes.PowerPlant.Id;
        roomTracker.requirement      = RoomTracker.Requirement.Required;
        Prioritizable.AddRef(go);
        go.GetComponent<KPrefabID>().prefabInitFn += delegate(GameObject game_object) {
                                                         var component = game_object.GetComponent<TinkerStation>();
                                                         component.AttributeConverter
                                                             = Db.Get().AttributeConverters.MachinerySpeed;

                                                         component.AttributeExperienceMultiplier
                                                             = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MOST_DAY_EXPERIENCE;

                                                         component.SkillExperienceSkillGroup
                                                             = Db.Get().SkillGroups.Technicals.Id;

                                                         component.SkillExperienceMultiplier
                                                             = SKILLS.MOST_DAY_EXPERIENCE;

                                                         tinkerstation.SetWorkTime(160f);
                                                     };
    }
}