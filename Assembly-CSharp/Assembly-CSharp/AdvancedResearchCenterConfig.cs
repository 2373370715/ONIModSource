﻿using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

public class AdvancedResearchCenterConfig : IBuildingConfig
{
		public override BuildingDef CreateBuildingDef()
	{
		string id = "AdvancedResearchCenter";
		int width = 3;
		int height = 3;
		string anim = "research_center2_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER1;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tier2, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 120f;
		buildingDef.ExhaustKilowattsWhenActive = 0.5f;
		buildingDef.SelfHeatKilowattsWhenActive = 4f;
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "large";
		return buildingDef;
	}

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.ScienceBuilding, false);
		go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
		Prioritizable.AddRef(go);
		Storage storage = go.AddOrGet<Storage>();
		storage.capacityKg = 1000f;
		storage.showInUI = true;
		storage.SetDefaultStoredItemModifiers(new List<Storage.StoredItemModifier>
		{
			Storage.StoredItemModifier.Hide,
			Storage.StoredItemModifier.Insulate
		});
		ManualDeliveryKG manualDeliveryKG = go.AddOrGet<ManualDeliveryKG>();
		manualDeliveryKG.SetStorage(storage);
		manualDeliveryKG.RequestedItemTag = AdvancedResearchCenterConfig.INPUT_MATERIAL;
		manualDeliveryKG.refillMass = 150f;
		manualDeliveryKG.capacity = 750f;
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.ResearchFetch.IdHash;
		ResearchCenter researchCenter = go.AddOrGet<ResearchCenter>();
		researchCenter.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_research2_kanim")
		};
		researchCenter.research_point_type_id = "advanced";
		researchCenter.inputMaterial = AdvancedResearchCenterConfig.INPUT_MATERIAL;
		researchCenter.mass_per_point = 50f;
		researchCenter.requiredSkillPerk = Db.Get().SkillPerks.AllowAdvancedResearch.Id;
		ElementConverter elementConverter = go.AddOrGet<ElementConverter>();
		elementConverter.consumedElements = new ElementConverter.ConsumedElement[]
		{
			new ElementConverter.ConsumedElement(AdvancedResearchCenterConfig.INPUT_MATERIAL, 0.8333333f, true)
		};
		elementConverter.showDescriptors = false;
		go.AddOrGetDef<PoweredController.Def>();
	}

		public override void DoPostConfigureComplete(GameObject go)
	{
	}

		public const string ID = "AdvancedResearchCenter";

		public const float BASE_SECONDS_PER_POINT = 60f;

		public const float MASS_PER_POINT = 50f;

		public const float BASE_MASS_PER_SECOND = 0.8333333f;

		public const float CAPACITY = 750f;

		public static readonly Tag INPUT_MATERIAL = GameTags.Water;
}
