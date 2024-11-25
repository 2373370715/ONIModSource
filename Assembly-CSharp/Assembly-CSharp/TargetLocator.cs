﻿using System;
using UnityEngine;

public class TargetLocator : IEntityConfig
{
		public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

		public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(TargetLocator.ID, TargetLocator.ID, false);
		gameObject.AddTag(GameTags.NotConversationTopic);
		return gameObject;
	}

		public void OnPrefabInit(GameObject go)
	{
	}

		public void OnSpawn(GameObject go)
	{
	}

		public static readonly string ID = "TargetLocator";
}
