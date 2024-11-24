﻿using System;
using STRINGS;
using TUNING;
using UnityEngine;

public class DeepFriedFishConfig : IEntityConfig
{
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_DLC_2;
	}

	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("DeepFriedFish", ITEMS.FOOD.DEEPFRIEDFISH.NAME, ITEMS.FOOD.DEEPFRIEDFISH.DESC, 1f, false, Assets.GetAnim("deepfried_fish_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.DEEP_FRIED_FISH);
	}

	public void OnPrefabInit(GameObject inst)
	{
	}

	public void OnSpawn(GameObject inst)
	{
	}

	public const string ID = "DeepFriedFish";

	public static ComplexRecipe recipe;
}
