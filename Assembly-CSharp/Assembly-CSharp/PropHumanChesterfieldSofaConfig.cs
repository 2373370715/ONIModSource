﻿using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

public class PropHumanChesterfieldSofaConfig : IEntityConfig
{
		public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_DLC_2;
	}

		public GameObject CreatePrefab()
	{
		string id = "PropHumanChesterfieldSofa";
		string name = STRINGS.BUILDINGS.PREFABS.PROPHUMANCHESTERFIELDSOFA.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPHUMANCHESTERFIELDSOFA.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("poi_couch_kanim"), "off", Grid.SceneLayer.Building, 3, 2, tier, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Granite, true);
		component.Temperature = 294.15f;
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

		public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

		public void OnSpawn(GameObject inst)
	{
	}
}
