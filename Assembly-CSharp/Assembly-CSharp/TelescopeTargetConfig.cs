﻿using System;
using UnityEngine;

public class TelescopeTargetConfig : IEntityConfig
{
		public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

		public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity("TelescopeTarget", "TelescopeTarget", true);
		gameObject.AddOrGet<TelescopeTarget>();
		return gameObject;
	}

		public void OnPrefabInit(GameObject inst)
	{
	}

		public void OnSpawn(GameObject inst)
	{
	}

		public const string ID = "TelescopeTarget";
}
