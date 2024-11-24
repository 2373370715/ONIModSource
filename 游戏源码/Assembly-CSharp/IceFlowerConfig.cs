using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

public class IceFlowerConfig : IEntityConfig
{
	public const string ID = "IceFlower";

	public const string SEED_ID = "IceFlowerSeed";

	public readonly EffectorValues POSITIVE_DECOR_EFFECT = DECOR.BONUS.TIER3;

	public readonly EffectorValues NEGATIVE_DECOR_EFFECT = DECOR.PENALTY.TIER3;

	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_DLC_2;
	}

	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreatePlacedEntity("IceFlower", STRINGS.CREATURES.SPECIES.ICEFLOWER.NAME, STRINGS.CREATURES.SPECIES.ICEFLOWER.DESC, 1f, decor: POSITIVE_DECOR_EFFECT, anim: Assets.GetAnim("potted_ice_flower_kanim"), initialAnim: "grow_seed", sceneLayer: Grid.SceneLayer.BuildingFront, width: 1, height: 1, noise: default(EffectorValues), element: SimHashes.Creature, additionalTags: null, defaultTemperature: 243.15f);
		EntityTemplates.ExtendEntityToBasicPlant(gameObject, 173.15f, 203.15f, 278.15f, 318.15f, new SimHashes[5]
		{
			SimHashes.Oxygen,
			SimHashes.ContaminatedOxygen,
			SimHashes.CarbonDioxide,
			SimHashes.ChlorineGas,
			SimHashes.Hydrogen
		}, pressure_sensitive: true, 0f, 0.15f, null, can_drown: true, can_tinker: false, require_solid_tile: true, should_grow_old: true, 2400f, 0f, 2200f, "IceFlowerOriginal", STRINGS.CREATURES.SPECIES.ICEFLOWER.NAME);
		PrickleGrass prickleGrass = gameObject.AddOrGet<PrickleGrass>();
		prickleGrass.positive_decor_effect = POSITIVE_DECOR_EFFECT;
		prickleGrass.negative_decor_effect = NEGATIVE_DECOR_EFFECT;
		EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(gameObject, SeedProducer.ProductionType.Hidden, "IceFlowerSeed", STRINGS.CREATURES.SPECIES.SEEDS.ICEFLOWER.NAME, STRINGS.CREATURES.SPECIES.SEEDS.ICEFLOWER.DESC, Assets.GetAnim("seed_ice_flower_kanim"), "object", 1, new List<Tag> { GameTags.DecorSeed }, SingleEntityReceptacle.ReceptacleDirection.Top, default(Tag), 12, STRINGS.CREATURES.SPECIES.ICEFLOWER.DOMESTICATEDDESC, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.6f, null, "", ignoreDefaultSeedTag: false, GetDlcIds()), "IceFlower_preview", Assets.GetAnim("potted_ice_flower_kanim"), "place", 1, 1);
		return gameObject;
	}

	public void OnPrefabInit(GameObject inst)
	{
	}

	public void OnSpawn(GameObject inst)
	{
	}
}
