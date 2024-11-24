using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000290 RID: 656
public class CarrotPlantConfig : IEntityConfig
{
	// Token: 0x060009C6 RID: 2502 RVA: 0x000A9B1E File Offset: 0x000A7D1E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_DLC_2;
	}

	// Token: 0x060009C7 RID: 2503 RVA: 0x00167458 File Offset: 0x00165658
	public GameObject CreatePrefab()
	{
		string id = "CarrotPlant";
		string name = STRINGS.CREATURES.SPECIES.CARROTPLANT.NAME;
		string desc = STRINGS.CREATURES.SPECIES.CARROTPLANT.DESC;
		float mass = 1f;
		EffectorValues tier = DECOR.PENALTY.TIER1;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("purpleroot_kanim"), "idle_empty", Grid.SceneLayer.BuildingBack, 1, 2, tier, default(EffectorValues), SimHashes.Creature, null, 255f);
		GameObject template = gameObject;
		float temperature_lethal_low = 118.149994f;
		float temperature_warning_low = 218.15f;
		float temperature_warning_high = 259.15f;
		float temperature_lethal_high = 269.15f;
		string text = CarrotConfig.ID;
		EntityTemplates.ExtendEntityToBasicPlant(template, temperature_lethal_low, temperature_warning_low, temperature_warning_high, temperature_lethal_high, new SimHashes[]
		{
			SimHashes.Oxygen,
			SimHashes.ContaminatedOxygen,
			SimHashes.CarbonDioxide
		}, true, 0f, 0.15f, text, true, true, true, true, 2400f, 0f, 4600f, "CarrotPlantOriginal", STRINGS.CREATURES.SPECIES.CARROTPLANT.NAME);
		gameObject.AddOrGet<StandardCropPlant>();
		gameObject.AddOrGet<DirectlyEdiblePlant_Growth>();
		gameObject.AddOrGet<LoopingSounds>();
		GameObject plant = gameObject;
		SeedProducer.ProductionType productionType = SeedProducer.ProductionType.Harvest;
		string id2 = "CarrotPlantSeed";
		string name2 = STRINGS.CREATURES.SPECIES.SEEDS.CARROTPLANT.NAME;
		string desc2 = STRINGS.CREATURES.SPECIES.SEEDS.CARROTPLANT.DESC;
		KAnimFile anim = Assets.GetAnim("seed_purpleroot_kanim");
		string initialAnim = "object";
		int numberOfSeeds = 1;
		List<Tag> list = new List<Tag>();
		list.Add(GameTags.CropSeed);
		SingleEntityReceptacle.ReceptacleDirection planterDirection = SingleEntityReceptacle.ReceptacleDirection.Top;
		text = STRINGS.CREATURES.SPECIES.CARROTPLANT.DOMESTICATEDDESC;
		string[] dlcIds = this.GetDlcIds();
		GameObject seed = EntityTemplates.CreateAndRegisterSeedForPlant(plant, productionType, id2, name2, desc2, anim, initialAnim, numberOfSeeds, list, planterDirection, default(Tag), 1, text, EntityTemplates.CollisionShape.CIRCLE, 0.3f, 0.3f, null, "", false, dlcIds);
		EntityTemplates.ExtendPlantToIrrigated(gameObject, new PlantElementAbsorber.ConsumeInfo[]
		{
			new PlantElementAbsorber.ConsumeInfo
			{
				tag = SimHashes.Ethanol.CreateTag(),
				massConsumptionRate = 0.025f
			}
		});
		EntityTemplates.CreateAndRegisterPreviewForPlant(seed, "CarrotPlant_preview", Assets.GetAnim("purpleroot_kanim"), "place", 1, 2);
		SoundEventVolumeCache.instance.AddVolume("bristleblossom_kanim", "PrickleFlower_harvest", NOISE_POLLUTION.CREATURES.TIER3);
		SoundEventVolumeCache.instance.AddVolume("bristleblossom_kanim", "PrickleFlower_grow", NOISE_POLLUTION.CREATURES.TIER3);
		return gameObject;
	}

	// Token: 0x060009C8 RID: 2504 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060009C9 RID: 2505 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400074D RID: 1869
	public const string ID = "CarrotPlant";

	// Token: 0x0400074E RID: 1870
	public const string SEED_ID = "CarrotPlantSeed";

	// Token: 0x0400074F RID: 1871
	public const float Temperature_lethal_low = 118.149994f;

	// Token: 0x04000750 RID: 1872
	public const float Temperature_warning_low = 218.15f;

	// Token: 0x04000751 RID: 1873
	public const float Temperature_lethal_high = 269.15f;

	// Token: 0x04000752 RID: 1874
	public const float Temperature_warning_high = 259.15f;

	// Token: 0x04000753 RID: 1875
	public const float FERTILIZATION_RATE = 0.025f;
}
