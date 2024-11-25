﻿using System;
using STRINGS;
using UnityEngine;

public class NiobiumGeyserConfig : IEntityConfig
{
		public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

		public GameObject CreatePrefab()
	{
		GeyserConfigurator.GeyserType geyserType = new GeyserConfigurator.GeyserType("molten_niobium", SimHashes.MoltenNiobium, GeyserConfigurator.GeyserShape.Molten, 3500f, 800f, 1600f, 150f, 6000f, 12000f, 0.005f, 0.01f, 15000f, 135000f, 0.4f, 0.8f, 372.15f, "");
		GameObject gameObject = GeyserGenericConfig.CreateGeyser("NiobiumGeyser", "geyser_molten_niobium_kanim", 3, 3, CREATURES.SPECIES.GEYSER.MOLTEN_NIOBIUM.NAME, CREATURES.SPECIES.GEYSER.MOLTEN_NIOBIUM.DESC, geyserType.idHash, geyserType.geyserTemperature);
		gameObject.GetComponent<KPrefabID>().AddTag(GameTags.DeprecatedContent, false);
		return gameObject;
	}

		public void OnPrefabInit(GameObject inst)
	{
	}

		public void OnSpawn(GameObject inst)
	{
	}

		public const string ID = "NiobiumGeyser";
}
