﻿using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

public class ElectrobankConfig : IEntityConfig
{
		public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("Electrobank", STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.ELECTROBANK.NAME, STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.ELECTROBANK.DESC, 20f, true, Assets.GetAnim("electrobank_large_kanim"), "idle1", Grid.SceneLayer.Ore, EntityTemplates.CollisionShape.RECTANGLE, 0.5f, 0.8f, true, 0, SimHashes.Aluminum, new List<Tag>
		{
			GameTags.ChargedPortableBattery,
			GameTags.PedestalDisplayable
		});
		gameObject.GetComponent<KCollider2D>();
		gameObject.AddTag(GameTags.IndustrialProduct);
		gameObject.AddComponent<Electrobank>().rechargeable = true;
		gameObject.AddOrGet<OccupyArea>().SetCellOffsets(EntityTemplates.GenerateOffsets(1, 1));
		gameObject.AddOrGet<DecorProvider>().SetValues(DECOR.PENALTY.TIER0);
		return gameObject;
	}

		public string[] GetDlcIds()
	{
		return DlcManager.DLC3;
	}

		public void OnPrefabInit(GameObject inst)
	{
	}

		public void OnSpawn(GameObject inst)
	{
	}

		public const string ID = "Electrobank";

		public const float MASS = 20f;

		public const float POWER_CAPACITY = 120000f;

		public static ComplexRecipe recipe;
}
