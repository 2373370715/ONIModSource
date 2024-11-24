using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

public class IceCavesForagePlantPlantedConfig : IEntityConfig
{
	public const string ID = "IceCavesForagePlantPlanted";

	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_DLC_2;
	}

	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreatePlacedEntity("IceCavesForagePlantPlanted", STRINGS.CREATURES.SPECIES.ICECAVESFORAGEPLANTPLANTED.NAME, STRINGS.CREATURES.SPECIES.ICECAVESFORAGEPLANTPLANTED.DESC, 100f, decor: DECOR.BONUS.TIER1, anim: Assets.GetAnim("frozenberries_kanim"), initialAnim: "idle", sceneLayer: Grid.SceneLayer.BuildingBack, width: 1, height: 2, noise: default(EffectorValues), element: SimHashes.Creature, additionalTags: new List<Tag> { GameTags.Hanging }, defaultTemperature: 253.15f);
		EntityTemplates.MakeHangingOffsets(gameObject, 1, 2);
		gameObject.AddOrGet<SimTemperatureTransfer>();
		gameObject.AddOrGet<OccupyArea>().objectLayers = new ObjectLayer[1] { ObjectLayer.Building };
		gameObject.AddOrGet<EntombVulnerable>();
		gameObject.AddOrGet<DrowningMonitor>();
		gameObject.AddOrGet<Prioritizable>();
		gameObject.AddOrGet<Uprootable>();
		gameObject.AddOrGet<UprootedMonitor>().monitorCells = new CellOffset[1]
		{
			new CellOffset(0, 1)
		};
		gameObject.AddOrGet<Harvestable>();
		gameObject.AddOrGet<HarvestDesignatable>();
		gameObject.AddOrGet<SeedProducer>().Configure("IceCavesForagePlant", SeedProducer.ProductionType.DigOnly, 2);
		gameObject.AddOrGet<BasicForagePlantPlanted>();
		gameObject.AddOrGet<KBatchedAnimController>().randomiseLoopedOffset = true;
		return gameObject;
	}

	public void OnPrefabInit(GameObject inst)
	{
	}

	public void OnSpawn(GameObject inst)
	{
	}
}
