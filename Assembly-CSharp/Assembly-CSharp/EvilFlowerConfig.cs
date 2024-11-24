﻿using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

public class EvilFlowerConfig : IEntityConfig
{
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	public GameObject CreatePrefab()
	{
		string id = "EvilFlower";
		string name = STRINGS.CREATURES.SPECIES.EVILFLOWER.NAME;
		string desc = STRINGS.CREATURES.SPECIES.EVILFLOWER.DESC;
		float mass = 1f;
		EffectorValues positive_DECOR_EFFECT = this.POSITIVE_DECOR_EFFECT;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("potted_evilflower_kanim"), "grow_seed", Grid.SceneLayer.BuildingFront, 1, 1, positive_DECOR_EFFECT, default(EffectorValues), SimHashes.Creature, null, 293f);
		EntityTemplates.ExtendEntityToBasicPlant(gameObject, 168.15f, 258.15f, 513.15f, 563.15f, new SimHashes[]
		{
			SimHashes.CarbonDioxide
		}, true, 0f, 0.15f, null, true, false, true, true, 2400f, 0f, 12200f, "EvilFlowerOriginal", STRINGS.CREATURES.SPECIES.EVILFLOWER.NAME);
		EvilFlower evilFlower = gameObject.AddOrGet<EvilFlower>();
		evilFlower.positive_decor_effect = this.POSITIVE_DECOR_EFFECT;
		evilFlower.negative_decor_effect = this.NEGATIVE_DECOR_EFFECT;
		EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(gameObject, SeedProducer.ProductionType.Hidden, "EvilFlowerSeed", STRINGS.CREATURES.SPECIES.SEEDS.EVILFLOWER.NAME, STRINGS.CREATURES.SPECIES.SEEDS.EVILFLOWER.DESC, Assets.GetAnim("seed_potted_evilflower_kanim"), "object", 1, new List<Tag>
		{
			GameTags.DecorSeed
		}, SingleEntityReceptacle.ReceptacleDirection.Top, default(Tag), 19, STRINGS.CREATURES.SPECIES.EVILFLOWER.DOMESTICATEDDESC, EntityTemplates.CollisionShape.CIRCLE, 0.4f, 0.4f, null, "", false, null), "EvilFlower_preview", Assets.GetAnim("potted_evilflower_kanim"), "place", 1, 1);
		DiseaseDropper.Def def = gameObject.AddOrGetDef<DiseaseDropper.Def>();
		def.diseaseIdx = Db.Get().Diseases.GetIndex("ZombieSpores");
		def.emitFrequency = 1f;
		def.averageEmitPerSecond = 1000;
		def.singleEmitQuantity = 100000;
		return gameObject;
	}

	public void OnPrefabInit(GameObject inst)
	{
	}

	public void OnSpawn(GameObject inst)
	{
	}

	public const string ID = "EvilFlower";

	public const string SEED_ID = "EvilFlowerSeed";

	public readonly EffectorValues POSITIVE_DECOR_EFFECT = DECOR.BONUS.TIER7;

	public readonly EffectorValues NEGATIVE_DECOR_EFFECT = DECOR.PENALTY.TIER5;

	public const int GERMS_PER_SECOND = 1000;
}
