﻿using System;
using UnityEngine;

public class RepairableStorageProxy : IEntityConfig
{
		public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

		public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(RepairableStorageProxy.ID, RepairableStorageProxy.ID, true);
		gameObject.AddOrGet<Storage>();
		gameObject.AddTag(GameTags.NotConversationTopic);
		return gameObject;
	}

		public void OnPrefabInit(GameObject go)
	{
	}

		public void OnSpawn(GameObject go)
	{
	}

		public static string ID = "RepairableStorageProxy";
}
