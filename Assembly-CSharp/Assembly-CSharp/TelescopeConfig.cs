﻿using System;
using TUNING;
using UnityEngine;

public class TelescopeConfig : IBuildingConfig
{
		public override string[] GetForbiddenDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

		public override BuildingDef CreateBuildingDef()
	{
		string id = "Telescope";
		int width = 4;
		int height = 6;
		string anim = "telescope_kanim";
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
		buildingDef.ExhaustKilowattsWhenActive = 0.125f;
		buildingDef.SelfHeatKilowattsWhenActive = 0f;
		buildingDef.InputConduitType = ConduitType.Gas;
		buildingDef.UtilityInputOffset = new CellOffset(0, 0);
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
		Telescope telescope = go.AddOrGet<Telescope>();
		telescope.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_telescope_kanim")
		};
		telescope.requiredSkillPerk = Db.Get().SkillPerks.CanStudyWorldObjects.Id;
		telescope.workLayer = Grid.SceneLayer.BuildingFront;
		Storage storage = go.AddOrGet<Storage>();
		storage.capacityKg = 1000f;
		storage.showInUI = true;
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.conduitType = ConduitType.Gas;
		conduitConsumer.consumptionRate = 1f;
		conduitConsumer.capacityTag = ElementLoader.FindElementByHash(SimHashes.Oxygen).tag;
		conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
		conduitConsumer.capacityKG = 10f;
		conduitConsumer.forceAlwaysSatisfied = true;
		go.AddOrGetDef<PoweredController.Def>();
	}

		public override void DoPostConfigureComplete(GameObject go)
	{
		TelescopeConfig.AddVisualizer(go);
	}

		public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		TelescopeConfig.AddVisualizer(go);
	}

		public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		TelescopeConfig.AddVisualizer(go);
	}

		private static void AddVisualizer(GameObject prefab)
	{
		SkyVisibilityVisualizer skyVisibilityVisualizer = prefab.AddOrGet<SkyVisibilityVisualizer>();
		skyVisibilityVisualizer.OriginOffset.y = 3;
		skyVisibilityVisualizer.TwoWideOrgin = true;
		skyVisibilityVisualizer.RangeMin = -4;
		skyVisibilityVisualizer.RangeMax = 5;
		skyVisibilityVisualizer.SkipOnModuleInteriors = true;
	}

		public const string ID = "Telescope";

		public const float POINTS_PER_DAY = 2f;

		public const float MASS_PER_POINT = 2f;

		public const float CAPACITY = 30f;

		public const int SCAN_RADIUS = 4;

		public const int VERTICAL_SCAN_OFFSET = 3;

		public static readonly SkyVisibilityInfo SKY_VISIBILITY_INFO = new SkyVisibilityInfo(new CellOffset(0, 3), 4, new CellOffset(1, 3), 4, 0);

		public static readonly Tag INPUT_MATERIAL = GameTags.Glass;
}
