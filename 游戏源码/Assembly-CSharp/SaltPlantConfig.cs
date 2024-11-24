using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002B8 RID: 696
public class SaltPlantConfig : IEntityConfig
{
	// Token: 0x06000A6C RID: 2668 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000A6D RID: 2669 RVA: 0x0016B244 File Offset: 0x00169444
	public GameObject CreatePrefab()
	{
		string id = "SaltPlant";
		string name = STRINGS.CREATURES.SPECIES.SALTPLANT.NAME;
		string desc = STRINGS.CREATURES.SPECIES.SALTPLANT.DESC;
		float mass = 2f;
		EffectorValues tier = DECOR.PENALTY.TIER1;
		KAnimFile anim = Assets.GetAnim("saltplant_kanim");
		string initialAnim = "idle_empty";
		Grid.SceneLayer sceneLayer = Grid.SceneLayer.BuildingFront;
		int width = 1;
		int height = 2;
		EffectorValues decor = tier;
		List<Tag> additionalTags = new List<Tag>
		{
			GameTags.Hanging
		};
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, anim, initialAnim, sceneLayer, width, height, decor, default(EffectorValues), SimHashes.Creature, additionalTags, 258.15f);
		EntityTemplates.MakeHangingOffsets(gameObject, 1, 2);
		GameObject template = gameObject;
		float temperature_lethal_low = 198.15f;
		float temperature_warning_low = 248.15f;
		float temperature_warning_high = 323.15f;
		float temperature_lethal_high = 393.15f;
		string crop_id = SimHashes.Salt.ToString();
		string text = STRINGS.CREATURES.SPECIES.SALTPLANT.NAME;
		EntityTemplates.ExtendEntityToBasicPlant(template, temperature_lethal_low, temperature_warning_low, temperature_warning_high, temperature_lethal_high, new SimHashes[]
		{
			SimHashes.ChlorineGas
		}, true, 0f, 0.025f, crop_id, true, true, true, true, 2400f, 0f, 7400f, "SaltPlantOriginal", text);
		gameObject.AddOrGet<SaltPlant>();
		EntityTemplates.ExtendPlantToFertilizable(gameObject, new PlantElementAbsorber.ConsumeInfo[]
		{
			new PlantElementAbsorber.ConsumeInfo
			{
				tag = SimHashes.Sand.CreateTag(),
				massConsumptionRate = 0.011666667f
			}
		});
		Storage storage = gameObject.AddOrGet<Storage>();
		storage.showInUI = false;
		storage.capacityKg = 1f;
		ElementConsumer elementConsumer = gameObject.AddOrGet<ElementConsumer>();
		elementConsumer.showInStatusPanel = true;
		elementConsumer.showDescriptor = true;
		elementConsumer.storeOnConsume = false;
		elementConsumer.elementToConsume = SimHashes.ChlorineGas;
		elementConsumer.configuration = ElementConsumer.Configuration.Element;
		elementConsumer.consumptionRadius = 4;
		elementConsumer.sampleCellOffset = new Vector3(0f, -1f);
		elementConsumer.consumptionRate = 0.006f;
		gameObject.GetComponent<UprootedMonitor>().monitorCells = new CellOffset[]
		{
			new CellOffset(0, 1)
		};
		gameObject.AddOrGet<StandardCropPlant>();
		GameObject plant = gameObject;
		SeedProducer.ProductionType productionType = SeedProducer.ProductionType.Harvest;
		string id2 = "SaltPlantSeed";
		string name2 = STRINGS.CREATURES.SPECIES.SEEDS.SALTPLANT.NAME;
		string desc2 = STRINGS.CREATURES.SPECIES.SEEDS.SALTPLANT.DESC;
		KAnimFile anim2 = Assets.GetAnim("seed_saltplant_kanim");
		string initialAnim2 = "object";
		int numberOfSeeds = 1;
		List<Tag> list = new List<Tag>();
		list.Add(GameTags.CropSeed);
		SingleEntityReceptacle.ReceptacleDirection planterDirection = SingleEntityReceptacle.ReceptacleDirection.Bottom;
		text = STRINGS.CREATURES.SPECIES.SALTPLANT.DOMESTICATEDDESC;
		EntityTemplates.MakeHangingOffsets(EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(plant, productionType, id2, name2, desc2, anim2, initialAnim2, numberOfSeeds, list, planterDirection, default(Tag), 5, text, EntityTemplates.CollisionShape.CIRCLE, 0.35f, 0.35f, null, "", false, null), "SaltPlant_preview", Assets.GetAnim("saltplant_kanim"), "place", 1, 2), 1, 2);
		return gameObject;
	}

	// Token: 0x06000A6E RID: 2670 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000A6F RID: 2671 RVA: 0x000AAB0E File Offset: 0x000A8D0E
	public void OnSpawn(GameObject inst)
	{
		inst.GetComponent<ElementConsumer>().EnableConsumption(true);
	}

	// Token: 0x04000800 RID: 2048
	public const string ID = "SaltPlant";

	// Token: 0x04000801 RID: 2049
	public const string SEED_ID = "SaltPlantSeed";

	// Token: 0x04000802 RID: 2050
	public const float FERTILIZATION_RATE = 0.011666667f;

	// Token: 0x04000803 RID: 2051
	public const float CHLORINE_CONSUMPTION_RATE = 0.006f;
}
