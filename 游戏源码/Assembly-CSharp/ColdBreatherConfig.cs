using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000292 RID: 658
public class ColdBreatherConfig : IEntityConfig
{
	// Token: 0x060009D0 RID: 2512 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060009D1 RID: 2513 RVA: 0x00167760 File Offset: 0x00165960
	public GameObject CreatePrefab()
	{
		string id = "ColdBreather";
		string name = STRINGS.CREATURES.SPECIES.COLDBREATHER.NAME;
		string desc = STRINGS.CREATURES.SPECIES.COLDBREATHER.DESC;
		float mass = 400f;
		EffectorValues tier = DECOR.BONUS.TIER1;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER2;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("coldbreather_kanim"), "grow_seed", Grid.SceneLayer.BuildingFront, 1, 2, tier, tier2, SimHashes.Creature, null, 293f);
		gameObject.AddOrGet<ReceptacleMonitor>();
		gameObject.AddOrGet<EntombVulnerable>();
		gameObject.AddOrGet<WiltCondition>();
		gameObject.AddOrGet<Prioritizable>();
		gameObject.AddOrGet<Uprootable>();
		gameObject.AddOrGet<UprootedMonitor>();
		gameObject.AddOrGet<DrowningMonitor>();
		EntityTemplates.ExtendPlantToFertilizable(gameObject, new PlantElementAbsorber.ConsumeInfo[]
		{
			new PlantElementAbsorber.ConsumeInfo
			{
				tag = SimHashes.Phosphorite.CreateTag(),
				massConsumptionRate = 0.006666667f
			}
		});
		gameObject.AddOrGet<TemperatureVulnerable>().Configure(213.15f, 183.15f, 368.15f, 463.15f);
		gameObject.AddOrGet<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
		ColdBreather coldBreather = gameObject.AddOrGet<ColdBreather>();
		coldBreather.deltaEmitTemperature = -5f;
		coldBreather.emitOffsetCell = new Vector3(0f, 1f);
		coldBreather.consumptionRate = 1f;
		gameObject.AddOrGet<KBatchedAnimController>().randomiseLoopedOffset = true;
		BuildingTemplates.CreateDefaultStorage(gameObject, false).showInUI = false;
		ElementConsumer elementConsumer = gameObject.AddOrGet<ElementConsumer>();
		elementConsumer.storeOnConsume = true;
		elementConsumer.configuration = ElementConsumer.Configuration.AllGas;
		elementConsumer.capacityKG = 2f;
		elementConsumer.consumptionRate = 0.25f;
		elementConsumer.consumptionRadius = 1;
		elementConsumer.sampleCellOffset = new Vector3(0f, 0f);
		SimTemperatureTransfer component = gameObject.GetComponent<SimTemperatureTransfer>();
		component.SurfaceArea = 10f;
		component.Thickness = 0.001f;
		if (DlcManager.FeatureRadiationEnabled())
		{
			RadiationEmitter radiationEmitter = gameObject.AddComponent<RadiationEmitter>();
			radiationEmitter.emitType = RadiationEmitter.RadiationEmitterType.Constant;
			radiationEmitter.radiusProportionalToRads = false;
			radiationEmitter.emitRadiusX = 6;
			radiationEmitter.emitRadiusY = radiationEmitter.emitRadiusX;
			radiationEmitter.emitRads = 480f;
			radiationEmitter.emissionOffset = new Vector3(0f, 0f, 0f);
		}
		GameObject plant = gameObject;
		SeedProducer.ProductionType productionType = SeedProducer.ProductionType.Hidden;
		string id2 = "ColdBreatherSeed";
		string name2 = STRINGS.CREATURES.SPECIES.SEEDS.COLDBREATHER.NAME;
		string desc2 = STRINGS.CREATURES.SPECIES.SEEDS.COLDBREATHER.DESC;
		KAnimFile anim = Assets.GetAnim("seed_coldbreather_kanim");
		string initialAnim = "object";
		int numberOfSeeds = 1;
		List<Tag> list = new List<Tag>();
		list.Add(GameTags.CropSeed);
		SingleEntityReceptacle.ReceptacleDirection planterDirection = SingleEntityReceptacle.ReceptacleDirection.Top;
		string domesticatedDescription = STRINGS.CREATURES.SPECIES.COLDBREATHER.DOMESTICATEDDESC;
		EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(plant, productionType, id2, name2, desc2, anim, initialAnim, numberOfSeeds, list, planterDirection, default(Tag), 21, domesticatedDescription, EntityTemplates.CollisionShape.CIRCLE, 0.3f, 0.3f, null, "", false, null), "ColdBreather_preview", Assets.GetAnim("coldbreather_kanim"), "place", 1, 2);
		SoundEventVolumeCache.instance.AddVolume("coldbreather_kanim", "ColdBreather_grow", NOISE_POLLUTION.CREATURES.TIER3);
		SoundEventVolumeCache.instance.AddVolume("coldbreather_kanim", "ColdBreather_intake", NOISE_POLLUTION.CREATURES.TIER3);
		gameObject.AddOrGet<EntityCellVisualizer>();
		return gameObject;
	}

	// Token: 0x060009D2 RID: 2514 RVA: 0x00167A2C File Offset: 0x00165C2C
	public void OnPrefabInit(GameObject inst)
	{
		inst.AddOrGet<EntityCellVisualizer>().AddPort(EntityCellVisualizer.Ports.HeatSink, default(CellOffset));
	}

	// Token: 0x060009D3 RID: 2515 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000755 RID: 1877
	public const string ID = "ColdBreather";

	// Token: 0x04000756 RID: 1878
	public static readonly Tag TAG = TagManager.Create("ColdBreather");

	// Token: 0x04000757 RID: 1879
	public const float FERTILIZATION_RATE = 0.006666667f;

	// Token: 0x04000758 RID: 1880
	public const SimHashes FERTILIZER = SimHashes.Phosphorite;

	// Token: 0x04000759 RID: 1881
	public const float TEMP_DELTA = -5f;

	// Token: 0x0400075A RID: 1882
	public const float CONSUMPTION_RATE = 1f;

	// Token: 0x0400075B RID: 1883
	public const float RADIATION_STRENGTH = 480f;

	// Token: 0x0400075C RID: 1884
	public const string SEED_ID = "ColdBreatherSeed";

	// Token: 0x0400075D RID: 1885
	public static readonly Tag SEED_TAG = TagManager.Create("ColdBreatherSeed");
}
