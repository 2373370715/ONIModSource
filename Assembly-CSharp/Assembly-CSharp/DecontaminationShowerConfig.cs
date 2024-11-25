using System;
using TUNING;
using UnityEngine;

public class DecontaminationShowerConfig : IBuildingConfig
{
		public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

		public override BuildingDef CreateBuildingDef()
	{
		string id = "DecontaminationShower";
		int width = 2;
		int height = 4;
		string anim = "decontamination_shower_kanim";
		int hitpoints = 250;
		float construction_time = 120f;
		string[] radiation_CONTAINMENT = MATERIALS.RADIATION_CONTAINMENT;
		float[] construction_mass = new float[]
		{
			BUILDINGS.CONSTRUCTION_MASS_KG.TIER5[0],
			BUILDINGS.CONSTRUCTION_MASS_KG.TIER2[0]
		};
		string[] construction_materials = radiation_CONTAINMENT;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier = NOISE_POLLUTION.NOISY.TIER0;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, construction_mass, construction_materials, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER3, tier, 0.2f);
		buildingDef.InputConduitType = ConduitType.Liquid;
		buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.UtilityInputOffset = new CellOffset(1, 2);
		return buildingDef;
	}

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		KBatchedAnimController kbatchedAnimController = go.AddOrGet<KBatchedAnimController>();
		kbatchedAnimController.sceneLayer = Grid.SceneLayer.BuildingBack;
		kbatchedAnimController.fgLayer = Grid.SceneLayer.BuildingFront;
		HandSanitizer handSanitizer = go.AddOrGet<HandSanitizer>();
		handSanitizer.massConsumedPerUse = 100f;
		handSanitizer.consumedElement = SimHashes.Water;
		handSanitizer.outputElement = SimHashes.DirtyWater;
		handSanitizer.diseaseRemovalCount = 1000000;
		handSanitizer.maxUses = 1;
		handSanitizer.canSanitizeSuit = true;
		handSanitizer.canSanitizeStorage = true;
		go.AddOrGet<DirectionControl>();
		HandSanitizer.Work work = go.AddOrGet<HandSanitizer.Work>();
		work.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_decontamination_shower_kanim")
		};
		work.workLayer = Grid.SceneLayer.BuildingUse;
		work.workTime = 15f;
		work.trackUses = true;
		work.removeIrritation = true;
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.conduitType = ConduitType.Liquid;
		conduitConsumer.capacityTag = ElementLoader.FindElementByHash(SimHashes.Water).tag;
		conduitConsumer.capacityKG = 100f;
		conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Store;
		AutoStorageDropper.Def def = go.AddOrGetDef<AutoStorageDropper.Def>();
		def.elementFilter = new SimHashes[]
		{
			SimHashes.DirtyWater
		};
		def.dropOffset = new CellOffset(1, 0);
		go.AddOrGet<Storage>().SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
	}

		public override void DoPostConfigureComplete(GameObject go)
	{
	}

		public const string ID = "DecontaminationShower";

		private const float MASS_PER_USE = 100f;

		private const int DISEASE_REMOVAL_COUNT = 1000000;

		private const float WATER_PER_USE = 100f;

		private const int USES_PER_FLUSH = 1;

		private const float WORK_TIME = 15f;

		private const SimHashes CONSUMED_ELEMENT = SimHashes.Water;

		private const SimHashes PRODUCED_ELEMENT = SimHashes.DirtyWater;
}
