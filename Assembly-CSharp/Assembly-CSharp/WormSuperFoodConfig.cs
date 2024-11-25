﻿using System;
using STRINGS;
using TUNING;
using UnityEngine;

public class WormSuperFoodConfig : IEntityConfig
{
		public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

		public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("WormSuperFood", STRINGS.ITEMS.FOOD.WORMSUPERFOOD.NAME, STRINGS.ITEMS.FOOD.WORMSUPERFOOD.DESC, 1f, false, Assets.GetAnim("wormwood_preserved_berries_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.7f, 0.6f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.WORMSUPERFOOD);
	}

		public void OnPrefabInit(GameObject inst)
	{
	}

		public void OnSpawn(GameObject inst)
	{
	}

		public const string ID = "WormSuperFood";

		public static ComplexRecipe recipe;
}
