﻿using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

public class WormPlantConfig : IEntityConfig
{
		public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

		public static GameObject BaseWormPlant(string id, string name, string desc, string animFile, EffectorValues decor, string cropID)
	{
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, 1f, Assets.GetAnim(animFile), "idle_empty", Grid.SceneLayer.BuildingBack, 1, 2, decor, default(EffectorValues), SimHashes.Creature, null, 307.15f);
		EntityTemplates.ExtendEntityToBasicPlant(gameObject, 273.15f, 288.15f, 323.15f, 373.15f, new SimHashes[]
		{
			SimHashes.Oxygen,
			SimHashes.ContaminatedOxygen,
			SimHashes.CarbonDioxide
		}, true, 0f, 0.15f, cropID, true, true, true, true, 2400f, 0f, 9800f, id + "Original", name);
		EntityTemplates.ExtendPlantToFertilizable(gameObject, new PlantElementAbsorber.ConsumeInfo[]
		{
			new PlantElementAbsorber.ConsumeInfo
			{
				tag = SimHashes.Sulfur.CreateTag(),
				massConsumptionRate = 0.016666668f
			}
		});
		gameObject.AddOrGet<StandardCropPlant>();
		gameObject.AddOrGet<LoopingSounds>();
		return gameObject;
	}

		public GameObject CreatePrefab()
	{
		GameObject gameObject = WormPlantConfig.BaseWormPlant("WormPlant", STRINGS.CREATURES.SPECIES.WORMPLANT.NAME, STRINGS.CREATURES.SPECIES.WORMPLANT.DESC, "wormwood_kanim", WormPlantConfig.BASIC_DECOR, "WormBasicFruit");
		SeedProducer.ProductionType productionType = SeedProducer.ProductionType.Harvest;
		string id = "WormPlantSeed";
		string name = STRINGS.CREATURES.SPECIES.SEEDS.WORMPLANT.NAME;
		string desc = STRINGS.CREATURES.SPECIES.SEEDS.WORMPLANT.DESC;
		KAnimFile anim = Assets.GetAnim("seed_wormwood_kanim");
		string initialAnim = "object";
		int numberOfSeeds = 1;
		List<Tag> list = new List<Tag>();
		list.Add(GameTags.CropSeed);
		SingleEntityReceptacle.ReceptacleDirection planterDirection = SingleEntityReceptacle.ReceptacleDirection.Top;
		string domesticatedDescription = STRINGS.CREATURES.SPECIES.WORMPLANT.DOMESTICATEDDESC;
		string[] dlcIds = this.GetDlcIds();
		EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(gameObject, productionType, id, name, desc, anim, initialAnim, numberOfSeeds, list, planterDirection, default(Tag), 3, domesticatedDescription, EntityTemplates.CollisionShape.CIRCLE, 0.3f, 0.3f, null, "", false, dlcIds), "WormPlant_preview", Assets.GetAnim("wormwood_kanim"), "place", 1, 2);
		return gameObject;
	}

		public void OnPrefabInit(GameObject prefab)
	{
		TransformingPlant transformingPlant = prefab.AddOrGet<TransformingPlant>();
		transformingPlant.transformPlantId = "SuperWormPlant";
		transformingPlant.SubscribeToTransformEvent(GameHashes.CropTended);
		transformingPlant.useGrowthTimeRatio = true;
		transformingPlant.eventDataCondition = delegate(object data)
		{
			CropTendingStates.CropTendingEventData cropTendingEventData = (CropTendingStates.CropTendingEventData)data;
			if (cropTendingEventData != null)
			{
				CreatureBrain component = cropTendingEventData.source.GetComponent<CreatureBrain>();
				if (component != null && component.species == GameTags.Creatures.Species.DivergentSpecies)
				{
					return true;
				}
			}
			return false;
		};
		transformingPlant.fxKAnim = "plant_transform_fx_kanim";
		transformingPlant.fxAnim = "plant_transform";
		prefab.AddOrGet<StandardCropPlant>().anims = WormPlantConfig.animSet;
	}

		public void OnSpawn(GameObject inst)
	{
	}

		public const string ID = "WormPlant";

		public const string SEED_ID = "WormPlantSeed";

		public const float SULFUR_CONSUMPTION_RATE = 0.016666668f;

		public static readonly EffectorValues BASIC_DECOR = DECOR.PENALTY.TIER0;

		public const string BASIC_CROP_ID = "WormBasicFruit";

		private static StandardCropPlant.AnimSet animSet = new StandardCropPlant.AnimSet
	{
		grow = "basic_grow",
		grow_pst = "basic_grow_pst",
		idle_full = "basic_idle_full",
		wilt_base = "basic_wilt",
		harvest = "basic_harvest"
	};
}
