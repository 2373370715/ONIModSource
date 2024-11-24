using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

public class GasGrassConfig : IEntityConfig
{
	public const string ID = "GasGrass";

	public const string SEED_ID = "GasGrassSeed";

	public const float CHLORINE_FERTILIZATION_RATE = 0.00083333335f;

	public const float DIRT_FERTILIZATION_RATE = 1f / 24f;

	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreatePlacedEntity("GasGrass", STRINGS.CREATURES.SPECIES.GASGRASS.NAME, STRINGS.CREATURES.SPECIES.GASGRASS.DESC, 1f, decor: DECOR.BONUS.TIER3, anim: Assets.GetAnim("gassygrass_kanim"), initialAnim: "idle_empty", sceneLayer: Grid.SceneLayer.BuildingFront, width: 1, height: 3, noise: default(EffectorValues), element: SimHashes.Creature, additionalTags: null, defaultTemperature: 255f);
		EntityTemplates.ExtendEntityToBasicPlant(gameObject, 218.15f, 0f, 348.15f, 373.15f, null, pressure_sensitive: false, 0f, 0.15f, "GasGrassHarvested", can_drown: true, can_tinker: true, require_solid_tile: true, should_grow_old: true, 2400f, 0f, 12200f, "GasGrassOriginal", STRINGS.CREATURES.SPECIES.GASGRASS.NAME);
		EntityTemplates.ExtendPlantToIrrigated(gameObject, new PlantElementAbsorber.ConsumeInfo[1]
		{
			new PlantElementAbsorber.ConsumeInfo
			{
				tag = GameTags.Chlorine,
				massConsumptionRate = 0.00083333335f
			}
		});
		EntityTemplates.ExtendPlantToFertilizable(gameObject, new PlantElementAbsorber.ConsumeInfo[1]
		{
			new PlantElementAbsorber.ConsumeInfo
			{
				tag = SimHashes.Dirt.CreateTag(),
				massConsumptionRate = 1f / 24f
			}
		});
		gameObject.AddOrGet<StandardCropPlant>();
		gameObject.AddOrGet<DirectlyEdiblePlant_Growth>();
		gameObject.AddOrGet<HarvestDesignatable>().defaultHarvestStateWhenPlanted = false;
		Modifiers component = gameObject.GetComponent<Modifiers>();
		Db.Get().traits.Get(component.initialTraits[0]).Add(new AttributeModifier(Db.Get().PlantAttributes.MinLightLux.Id, 10000f, STRINGS.CREATURES.SPECIES.GASGRASS.NAME));
		component.initialAttributes.Add(Db.Get().PlantAttributes.MinLightLux.Id);
		gameObject.AddOrGet<IlluminationVulnerable>().SetPrefersDarkness();
		EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(gameObject, DlcManager.FeaturePlantMutationsEnabled() ? SeedProducer.ProductionType.Harvest : SeedProducer.ProductionType.Hidden, "GasGrassSeed", STRINGS.CREATURES.SPECIES.SEEDS.GASGRASS.NAME, STRINGS.CREATURES.SPECIES.SEEDS.GASGRASS.DESC, Assets.GetAnim("seed_gassygrass_kanim"), "object", 1, new List<Tag> { GameTags.CropSeed }, SingleEntityReceptacle.ReceptacleDirection.Top, default(Tag), 22, STRINGS.CREATURES.SPECIES.GASGRASS.DOMESTICATEDDESC, EntityTemplates.CollisionShape.CIRCLE, 0.2f, 0.2f), "GasGrass_preview", Assets.GetAnim("gassygrass_kanim"), "place", 1, 1);
		SoundEventVolumeCache.instance.AddVolume("gassygrass_kanim", "GasGrass_grow", NOISE_POLLUTION.CREATURES.TIER3);
		SoundEventVolumeCache.instance.AddVolume("gassygrass_kanim", "GasGrass_harvest", NOISE_POLLUTION.CREATURES.TIER3);
		return gameObject;
	}

	public void OnPrefabInit(GameObject inst)
	{
	}

	public void OnSpawn(GameObject inst)
	{
	}
}
