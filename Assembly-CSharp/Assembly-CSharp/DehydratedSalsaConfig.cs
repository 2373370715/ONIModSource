﻿using System;
using STRINGS;
using TUNING;
using UnityEngine;

public class DehydratedSalsaConfig : IEntityConfig
{
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	public void OnPrefabInit(GameObject inst)
	{
	}

	public void OnSpawn(GameObject inst)
	{
	}

	public GameObject CreatePrefab()
	{
		KAnimFile anim = Assets.GetAnim("dehydrated_food_salsa_kanim");
		GameObject gameObject = EntityTemplates.CreateLooseEntity(DehydratedSalsaConfig.ID.Name, ITEMS.FOOD.SALSA.DEHYDRATED.NAME, ITEMS.FOOD.SALSA.DEHYDRATED.DESC, 1f, true, anim, "idle", Grid.SceneLayer.BuildingFront, EntityTemplates.CollisionShape.RECTANGLE, 0.6f, 0.7f, true, 0, SimHashes.Polypropylene, null);
		EntityTemplates.ExtendEntityToDehydratedFoodPackage(gameObject, FOOD.FOOD_TYPES.SALSA);
		return gameObject;
	}

	public static Tag ID = new Tag("DehydratedSalsa");

	public const float MASS = 1f;

	public const string ANIM_FILE = "dehydrated_food_salsa_kanim";

	public const string INITIAL_ANIM = "idle";
}