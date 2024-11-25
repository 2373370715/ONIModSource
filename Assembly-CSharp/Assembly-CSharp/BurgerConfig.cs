﻿using System;
using STRINGS;
using TUNING;
using UnityEngine;

public class BurgerConfig : IEntityConfig
{
		public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

		public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("Burger", STRINGS.ITEMS.FOOD.BURGER.NAME, STRINGS.ITEMS.FOOD.BURGER.DESC, 1f, false, Assets.GetAnim("frost_burger_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.BURGER);
	}

		public void OnPrefabInit(GameObject inst)
	{
	}

		public void OnSpawn(GameObject inst)
	{
	}

		public const string ID = "Burger";

		public static ComplexRecipe recipe;
}
