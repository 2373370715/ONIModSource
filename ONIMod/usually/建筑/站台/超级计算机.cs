using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;

#if 超级计算机
[HarmonyPatch]
public class 超级计算机 {
    [HarmonyPatch(typeof(AdvancedResearchCenterConfig), "ConfigureBuildingTemplate"),HarmonyPrefix]
    public static bool Prefix(GameObject go, Tag prefab_tag) {
        go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.ScienceBuilding);
        go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
        Prioritizable.AddRef(go);
        Storage storage = go.AddOrGet<Storage>();
        storage.capacityKg = 1000f;
        storage.showInUI   = true;
        storage.SetDefaultStoredItemModifiers(new List<Storage.StoredItemModifier>() {
            Storage.StoredItemModifier.Hide, Storage.StoredItemModifier.Insulate
        });

        ManualDeliveryKG manualDeliveryKg = go.AddOrGet<ManualDeliveryKG>();
        manualDeliveryKg.SetStorage(storage);
        manualDeliveryKg.RequestedItemTag = AdvancedResearchCenterConfig.INPUT_MATERIAL;
        manualDeliveryKg.refillMass       = 150f;
        manualDeliveryKg.capacity         = 750f;
        manualDeliveryKg.choreTypeIDHash  = Db.Get().ChoreTypes.ResearchFetch.IdHash;
        ResearchCenter researchCenter = go.AddOrGet<ResearchCenter>();
        researchCenter.overrideAnims = new[] { Assets.GetAnim((HashedString)"anim_interacts_research2_kanim") };

        researchCenter.research_point_type_id = "advanced";
        researchCenter.inputMaterial          = AdvancedResearchCenterConfig.INPUT_MATERIAL;

        ElementConverter elementConverter = go.AddOrGet<ElementConverter>();

        if (建筑.站台.超级计算机.需要水) {
            researchCenter.mass_per_point = 50f;
            elementConverter.consumedElements = new[] {
                new ElementConverter.ConsumedElement(AdvancedResearchCenterConfig.INPUT_MATERIAL, 0.8333333f)
            };
        } else {
            researchCenter.mass_per_point = 0.001f;
            elementConverter.consumedElements = new[] {
                new ElementConverter.ConsumedElement(AdvancedResearchCenterConfig.INPUT_MATERIAL, 0.001f)
            };
        }

        researchCenter.requiredSkillPerk = Db.Get().SkillPerks.AllowAdvancedResearch.Id;

        elementConverter.showDescriptors = false;
        go.AddOrGetDef<PoweredController.Def>();
        return false;
    }
}
#endif