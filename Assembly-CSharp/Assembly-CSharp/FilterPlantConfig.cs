using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

public class FilterPlantConfig : IEntityConfig
{
		public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

		public GameObject CreatePrefab()
	{
		string id = "FilterPlant";
		string name = STRINGS.CREATURES.SPECIES.FILTERPLANT.NAME;
		string desc = STRINGS.CREATURES.SPECIES.FILTERPLANT.DESC;
		float mass = 2f;
		EffectorValues tier = DECOR.PENALTY.TIER1;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("cactus_kanim"), "idle_empty", Grid.SceneLayer.BuildingFront, 1, 2, tier, default(EffectorValues), SimHashes.Creature, null, 348.15f);
		GameObject template = gameObject;
		float temperature_lethal_low = 253.15f;
		float temperature_warning_low = 293.15f;
		float temperature_warning_high = 383.15f;
		float temperature_lethal_high = 443.15f;
		string crop_id = SimHashes.Water.ToString();
		string text = STRINGS.CREATURES.SPECIES.FILTERPLANT.NAME;
		EntityTemplates.ExtendEntityToBasicPlant(template, temperature_lethal_low, temperature_warning_low, temperature_warning_high, temperature_lethal_high, new SimHashes[]
		{
			SimHashes.Oxygen
		}, true, 0f, 0.025f, crop_id, true, true, true, true, 2400f, 0f, 2200f, "FilterPlantOriginal", text);
		EntityTemplates.ExtendPlantToFertilizable(gameObject, new PlantElementAbsorber.ConsumeInfo[]
		{
			new PlantElementAbsorber.ConsumeInfo
			{
				tag = SimHashes.Sand.CreateTag(),
				massConsumptionRate = 0.008333334f
			}
		});
		EntityTemplates.ExtendPlantToIrrigated(gameObject, new PlantElementAbsorber.ConsumeInfo[]
		{
			new PlantElementAbsorber.ConsumeInfo
			{
				tag = GameTags.DirtyWater,
				massConsumptionRate = 0.108333334f
			}
		});
		gameObject.AddOrGet<StandardCropPlant>();
		gameObject.AddOrGet<SaltPlant>();
		ElementConsumer elementConsumer = gameObject.AddOrGet<ElementConsumer>();
		elementConsumer.showInStatusPanel = true;
		elementConsumer.showDescriptor = true;
		elementConsumer.storeOnConsume = false;
		elementConsumer.elementToConsume = SimHashes.Oxygen;
		elementConsumer.configuration = ElementConsumer.Configuration.Element;
		elementConsumer.consumptionRadius = 4;
		elementConsumer.sampleCellOffset = new Vector3(0f, 0f);
		elementConsumer.consumptionRate = 0.008333334f;
		GameObject plant = gameObject;
		SeedProducer.ProductionType productionType = SeedProducer.ProductionType.Harvest;
		string id2 = "FilterPlantSeed";
		string name2 = STRINGS.CREATURES.SPECIES.SEEDS.FILTERPLANT.NAME;
		string desc2 = STRINGS.CREATURES.SPECIES.SEEDS.FILTERPLANT.DESC;
		KAnimFile anim = Assets.GetAnim("seed_cactus_kanim");
		string initialAnim = "object";
		int numberOfSeeds = 1;
		List<Tag> list = new List<Tag>();
		list.Add(GameTags.CropSeed);
		SingleEntityReceptacle.ReceptacleDirection planterDirection = SingleEntityReceptacle.ReceptacleDirection.Top;
		text = STRINGS.CREATURES.SPECIES.FILTERPLANT.DOMESTICATEDDESC;
		string[] dlcIds = this.GetDlcIds();
		EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(plant, productionType, id2, name2, desc2, anim, initialAnim, numberOfSeeds, list, planterDirection, default(Tag), 21, text, EntityTemplates.CollisionShape.CIRCLE, 0.35f, 0.35f, null, "", false, dlcIds), "FilterPlant_preview", Assets.GetAnim("cactus_kanim"), "place", 1, 2);
		gameObject.AddTag(GameTags.DeprecatedContent);
		return gameObject;
	}

		public void OnPrefabInit(GameObject prefab)
	{
	}

		public void OnSpawn(GameObject inst)
	{
	}

		public const string ID = "FilterPlant";

		public const string SEED_ID = "FilterPlantSeed";

		public const float SAND_CONSUMPTION_RATE = 0.008333334f;

		public const float WATER_CONSUMPTION_RATE = 0.108333334f;

		public const float OXYGEN_CONSUMPTION_RATE = 0.008333334f;
}
