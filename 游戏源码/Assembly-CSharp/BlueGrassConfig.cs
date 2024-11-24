using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

public class BlueGrassConfig : IEntityConfig
{
	public const string ID = "BlueGrass";

	public const string SEED_ID = "BlueGrassSeed";

	public const float CO2_RATE = 0.002f;

	public const float FERTILIZATION_RATE = 20f;

	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_DLC_2;
	}

	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreatePlacedEntity("BlueGrass", STRINGS.CREATURES.SPECIES.BLUE_GRASS.NAME, STRINGS.CREATURES.SPECIES.BLUE_GRASS.DESC, 2f, decor: DECOR.BONUS.TIER1, anim: Assets.GetAnim("bluegrass_kanim"), initialAnim: "idle_full", sceneLayer: Grid.SceneLayer.BuildingFront, width: 1, height: 2, noise: default(EffectorValues), element: SimHashes.Creature, additionalTags: null, defaultTemperature: 240f);
		EntityTemplates.ExtendEntityToBasicPlant(gameObject, 193.15f, 193.15f, 273.15f, 273.15f, baseTraitName: STRINGS.CREATURES.SPECIES.BLUE_GRASS.NAME, safe_elements: new SimHashes[1] { SimHashes.CarbonDioxide }, pressure_sensitive: true, pressure_lethal_low: 0f, pressure_warning_low: 0f, crop_id: "OxyRock", can_drown: true, can_tinker: true, require_solid_tile: true, should_grow_old: true, max_age: 2400f, min_radiation: 0f, max_radiation: 2200f, baseTraitId: "BlueGrassOriginal");
		ElementConsumer elementConsumer = gameObject.AddOrGet<ElementConsumer>();
		elementConsumer.showInStatusPanel = true;
		elementConsumer.storeOnConsume = false;
		elementConsumer.elementToConsume = SimHashes.CarbonDioxide;
		elementConsumer.configuration = ElementConsumer.Configuration.Element;
		elementConsumer.consumptionRadius = 2;
		elementConsumer.EnableConsumption(enabled: true);
		elementConsumer.sampleCellOffset = new Vector3(0f, 0f);
		elementConsumer.consumptionRate = 0.0005f;
		EntityTemplates.ExtendPlantToFertilizable(gameObject, new PlantElementAbsorber.ConsumeInfo[1]
		{
			new PlantElementAbsorber.ConsumeInfo
			{
				tag = SimHashes.Ice.CreateTag(),
				massConsumptionRate = 1f / 30f
			}
		});
		gameObject.GetComponent<UprootedMonitor>();
		gameObject.AddOrGet<StandardCropPlant>();
		gameObject.AddOrGet<BlueGrass>();
		EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(gameObject, SeedProducer.ProductionType.Harvest, "BlueGrassSeed", STRINGS.CREATURES.SPECIES.SEEDS.BLUE_GRASS.NAME, STRINGS.CREATURES.SPECIES.SEEDS.BLUE_GRASS.DESC, Assets.GetAnim("seed_bluegrass_kanim"), "object", 1, new List<Tag> { GameTags.CropSeed }, SingleEntityReceptacle.ReceptacleDirection.Top, default(Tag), 4, STRINGS.CREATURES.SPECIES.BLUE_GRASS.DOMESTICATEDDESC, EntityTemplates.CollisionShape.CIRCLE, 0.3f, 0.3f, null, "", ignoreDefaultSeedTag: false, GetDlcIds()), "BlueGrass_preview", Assets.GetAnim("bluegrass_kanim"), "place", 1, 1);
		return gameObject;
	}

	public void OnPrefabInit(GameObject inst)
	{
	}

	public void OnSpawn(GameObject inst)
	{
	}
}
