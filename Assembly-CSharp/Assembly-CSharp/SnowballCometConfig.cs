﻿using System;
using STRINGS;
using UnityEngine;

public class SnowballCometConfig : IEntityConfig
{
		public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY.Append("DLC2_ID");
	}

		public GameObject CreatePrefab()
	{
		GameObject gameObject = BaseCometConfig.BaseComet(SnowballCometConfig.ID, UI.SPACEDESTINATIONS.COMETS.SNOWBALLCOMET.NAME, "meteor_snow_kanim", SimHashes.Snow, new Vector2(3f, 20f), new Vector2(253.15f, 263.15f), "Meteor_snowball_Impact", 5, SimHashes.Void, SpawnFXHashes.None, 0.3f);
		Comet component = gameObject.GetComponent<Comet>();
		component.entityDamage = 0;
		component.totalTileDamage = 0f;
		component.splashRadius = 1;
		component.addTiles = 3;
		component.addTilesMinHeight = 1;
		component.addTilesMaxHeight = 2;
		return gameObject;
	}

		public void OnPrefabInit(GameObject go)
	{
	}

		public void OnSpawn(GameObject go)
	{
	}

		public static string ID = "SnowballComet";
}
