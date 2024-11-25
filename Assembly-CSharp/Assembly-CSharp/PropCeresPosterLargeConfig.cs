﻿using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

public class PropCeresPosterLargeConfig : IEntityConfig
{
		public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_DLC_2;
	}

		public GameObject CreatePrefab()
	{
		string id = "PropCeresPosterLarge";
		string name = STRINGS.BUILDINGS.PREFABS.PROPCERESPOSTERLARGE.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPCERESPOSTERLARGE.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("poster_ceres_7x5_kanim"), "art_7x5", Grid.SceneLayer.Building, 5, 7, tier, PermittedRotations.R90, Orientation.Neutral, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Unobtanium, true);
		component.Temperature = 294.15f;
		gameObject.AddOrGet<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

		public void OnPrefabInit(GameObject inst)
	{
	}

		public void OnSpawn(GameObject inst)
	{
	}
}
