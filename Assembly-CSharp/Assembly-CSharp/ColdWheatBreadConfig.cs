﻿using System;
using STRINGS;
using TUNING;
using UnityEngine;

public class ColdWheatBreadConfig : IEntityConfig
{
		public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

		public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("ColdWheatBread", STRINGS.ITEMS.FOOD.COLDWHEATBREAD.NAME, STRINGS.ITEMS.FOOD.COLDWHEATBREAD.DESC, 1f, false, Assets.GetAnim("frostbread_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.COLD_WHEAT_BREAD);
	}

		public void OnPrefabInit(GameObject inst)
	{
	}

		public void OnSpawn(GameObject inst)
	{
	}

		public const string ID = "ColdWheatBread";

		public static ComplexRecipe recipe;
}
