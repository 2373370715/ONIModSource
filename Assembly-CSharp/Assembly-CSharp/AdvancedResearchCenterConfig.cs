using System.Collections.Generic;
using TUNING;
using UnityEngine;

public class AdvancedResearchCenterConfig : IBuildingConfig {
    public const           string ID                     = "AdvancedResearchCenter";
    public const           float  BASE_SECONDS_PER_POINT = 60f;
    public const           float  MASS_PER_POINT         = 50f;
    public const           float  BASE_MASS_PER_SECOND   = 0.8333333f;
    public const           float  CAPACITY               = 750f;
    public static readonly Tag    INPUT_MATERIAL         = GameTags.Water;

    public override BuildingDef CreateBuildingDef() {
        var id                  = "AdvancedResearchCenter";
        var width               = 3;
        var height              = 3;
        var anim                = "research_center2_kanim";
        var hitpoints           = 30;
        var construction_time   = 30f;
        var tier                = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
        var all_METALS          = MATERIALS.ALL_METALS;
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
                                                              all_METALS,
                                                              melting_point,
                                                              build_location_rule,
                                                              BUILDINGS.DECOR.NONE,
                                                              tier2);

        buildingDef.RequiresPowerInput          = true;
        buildingDef.EnergyConsumptionWhenActive = 120f;
        buildingDef.ExhaustKilowattsWhenActive  = 0.5f;
        buildingDef.SelfHeatKilowattsWhenActive = 4f;
        buildingDef.ViewMode                    = OverlayModes.Power.ID;
        buildingDef.AudioCategory               = "Metal";
        buildingDef.AudioSize                   = "large";
        return buildingDef;
    }

    public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag) {
        go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.ScienceBuilding);
        go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
        Prioritizable.AddRef(go);
        var storage = go.AddOrGet<Storage>();
        storage.capacityKg = 1000f;
        storage.showInUI   = true;
        storage.SetDefaultStoredItemModifiers(new List<Storage.StoredItemModifier> {
            Storage.StoredItemModifier.Hide, Storage.StoredItemModifier.Insulate
        });

        var manualDeliveryKG = go.AddOrGet<ManualDeliveryKG>();
        manualDeliveryKG.SetStorage(storage);
        manualDeliveryKG.RequestedItemTag = INPUT_MATERIAL;
        manualDeliveryKG.refillMass       = 150f;
        manualDeliveryKG.capacity         = 750f;
        manualDeliveryKG.choreTypeIDHash  = Db.Get().ChoreTypes.ResearchFetch.IdHash;
        var researchCenter = go.AddOrGet<ResearchCenter>();
        researchCenter.overrideAnims          = new[] { Assets.GetAnim("anim_interacts_research2_kanim") };
        researchCenter.research_point_type_id = "advanced";
        researchCenter.inputMaterial          = INPUT_MATERIAL;
        researchCenter.mass_per_point         = 50f;
        researchCenter.requiredSkillPerk      = Db.Get().SkillPerks.AllowAdvancedResearch.Id;
        var elementConverter = go.AddOrGet<ElementConverter>();
        elementConverter.consumedElements = new[] { new ElementConverter.ConsumedElement(INPUT_MATERIAL, 0.8333333f) };
        elementConverter.showDescriptors  = false;
        go.AddOrGetDef<PoweredController.Def>();
    }

    public override void DoPostConfigureComplete(GameObject go) { }
}