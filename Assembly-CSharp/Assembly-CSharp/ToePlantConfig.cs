using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

public class ToePlantConfig : IEntityConfig
{
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	public GameObject CreatePrefab()
	{
		string id = "ToePlant";
		string name = STRINGS.CREATURES.SPECIES.TOEPLANT.NAME;
		string desc = STRINGS.CREATURES.SPECIES.TOEPLANT.DESC;
		float mass = 1f;
		EffectorValues positive_DECOR_EFFECT = ToePlantConfig.POSITIVE_DECOR_EFFECT;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("potted_toes_kanim"), "grow_seed", Grid.SceneLayer.BuildingFront, 1, 1, positive_DECOR_EFFECT, default(EffectorValues), SimHashes.Creature, null, TUNING.CREATURES.TEMPERATURE.FREEZING_3);
		GameObject template = gameObject;
		SimHashes[] safe_elements = new SimHashes[]
		{
			SimHashes.Oxygen,
			SimHashes.ContaminatedOxygen,
			SimHashes.CarbonDioxide
		};
		EntityTemplates.ExtendEntityToBasicPlant(template, TUNING.CREATURES.TEMPERATURE.FREEZING_10, TUNING.CREATURES.TEMPERATURE.FREEZING_9, TUNING.CREATURES.TEMPERATURE.FREEZING, TUNING.CREATURES.TEMPERATURE.COOL, safe_elements, true, 0f, 0.15f, null, true, false, true, true, 2400f, 0f, 2200f, "ToePlantOriginal", STRINGS.CREATURES.SPECIES.TOEPLANT.NAME);
		PrickleGrass prickleGrass = gameObject.AddOrGet<PrickleGrass>();
		prickleGrass.positive_decor_effect = ToePlantConfig.POSITIVE_DECOR_EFFECT;
		prickleGrass.negative_decor_effect = ToePlantConfig.NEGATIVE_DECOR_EFFECT;
		EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(gameObject, SeedProducer.ProductionType.Hidden, "ToePlantSeed", STRINGS.CREATURES.SPECIES.SEEDS.TOEPLANT.NAME, STRINGS.CREATURES.SPECIES.SEEDS.TOEPLANT.DESC, Assets.GetAnim("seed_potted_toes_kanim"), "object", 1, new List<Tag>
		{
			GameTags.DecorSeed
		}, SingleEntityReceptacle.ReceptacleDirection.Top, default(Tag), 12, STRINGS.CREATURES.SPECIES.TOEPLANT.DOMESTICATEDDESC, EntityTemplates.CollisionShape.CIRCLE, 0.25f, 0.25f, null, "", false, this.GetDlcIds()), "ToePlant_preview", Assets.GetAnim("potted_toes_kanim"), "place", 1, 1);
		return gameObject;
	}

	public void OnPrefabInit(GameObject inst)
	{
	}

	public void OnSpawn(GameObject inst)
	{
	}

	public const string ID = "ToePlant";

	public const string SEED_ID = "ToePlantSeed";

	public static readonly EffectorValues POSITIVE_DECOR_EFFECT = DECOR.BONUS.TIER3;

	public static readonly EffectorValues NEGATIVE_DECOR_EFFECT = DECOR.PENALTY.TIER3;
}
