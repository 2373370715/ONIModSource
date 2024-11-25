﻿using System;
using STRINGS;
using TUNING;
using UnityEngine;

public class LettuceConfig : IEntityConfig
{
		public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

		public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("Lettuce", STRINGS.ITEMS.FOOD.LETTUCE.NAME, STRINGS.ITEMS.FOOD.LETTUCE.DESC, 1f, false, Assets.GetAnim("sea_lettuce_leaves_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.LETTUCE);
	}

		public void OnPrefabInit(GameObject inst)
	{
	}

		public void OnSpawn(GameObject inst)
	{
	}

		public const string ID = "Lettuce";
}
