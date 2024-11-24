﻿using System;
using STRINGS;
using TUNING;
using UnityEngine;

public class MushroomWrapConfig : IEntityConfig
{
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("MushroomWrap", ITEMS.FOOD.MUSHROOMWRAP.NAME, ITEMS.FOOD.MUSHROOMWRAP.DESC, 1f, false, Assets.GetAnim("mushroom_wrap_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.5f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.MUSHROOM_WRAP);
	}

	public void OnPrefabInit(GameObject inst)
	{
	}

	public void OnSpawn(GameObject inst)
	{
	}

	public const string ID = "MushroomWrap";

	public static ComplexRecipe recipe;
}
