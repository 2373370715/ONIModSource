﻿using System;
using STRINGS;
using TUNING;
using UnityEngine;

public class DehydratedBerryPieConfig : IEntityConfig
{
		public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

		public void OnPrefabInit(GameObject inst)
	{
	}

		public void OnSpawn(GameObject inst)
	{
	}

		public GameObject CreatePrefab()
	{
		KAnimFile anim = Assets.GetAnim("dehydrated_food_berry_pie_kanim");
		GameObject gameObject = EntityTemplates.CreateLooseEntity(DehydratedBerryPieConfig.ID.Name, STRINGS.ITEMS.FOOD.BERRYPIE.DEHYDRATED.NAME, STRINGS.ITEMS.FOOD.BERRYPIE.DEHYDRATED.DESC, 1f, true, anim, "idle", Grid.SceneLayer.BuildingFront, EntityTemplates.CollisionShape.RECTANGLE, 0.6f, 0.7f, true, 0, SimHashes.Polypropylene, null);
		EntityTemplates.ExtendEntityToDehydratedFoodPackage(gameObject, FOOD.FOOD_TYPES.BERRY_PIE);
		return gameObject;
	}

		public static Tag ID = new Tag("DehydratedBerryPie");

		public const float MASS = 1f;

		public const string ANIM_FILE = "dehydrated_food_berry_pie_kanim";

		public const string INITIAL_ANIM = "idle";
}
