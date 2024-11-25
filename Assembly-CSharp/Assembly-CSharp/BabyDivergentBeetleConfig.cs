﻿using System;
using STRINGS;
using UnityEngine;

[EntityConfigOrder(2)]
public class BabyDivergentBeetleConfig : IEntityConfig
{
		public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

		public GameObject CreatePrefab()
	{
		GameObject gameObject = DivergentBeetleConfig.CreateDivergentBeetle("DivergentBeetleBaby", CREATURES.SPECIES.DIVERGENT.VARIANT_BEETLE.BABY.NAME, CREATURES.SPECIES.DIVERGENT.VARIANT_BEETLE.BABY.DESC, "baby_critter_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "DivergentBeetle", null, false, 5f);
		return gameObject;
	}

		public void OnPrefabInit(GameObject prefab)
	{
	}

		public void OnSpawn(GameObject inst)
	{
	}

		public const string ID = "DivergentBeetleBaby";
}
