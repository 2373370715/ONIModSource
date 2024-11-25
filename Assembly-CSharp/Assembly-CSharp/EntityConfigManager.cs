﻿using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/EntityConfigManager")]
public class EntityConfigManager : KMonoBehaviour
{
		public static void DestroyInstance()
	{
		EntityConfigManager.Instance = null;
	}

		protected override void OnPrefabInit()
	{
		EntityConfigManager.Instance = this;
	}

		private static int GetSortOrder(Type type)
	{
		foreach (Attribute attribute in type.GetCustomAttributes(true))
		{
			if (attribute.GetType() == typeof(EntityConfigOrder))
			{
				return (attribute as EntityConfigOrder).sortOrder;
			}
		}
		return 0;
	}

		public void LoadGeneratedEntities(List<Type> types)
	{
		Type typeFromHandle = typeof(IEntityConfig);
		Type typeFromHandle2 = typeof(IMultiEntityConfig);
		List<EntityConfigManager.ConfigEntry> list = new List<EntityConfigManager.ConfigEntry>();
		foreach (Type type in types)
		{
			if ((typeFromHandle.IsAssignableFrom(type) || typeFromHandle2.IsAssignableFrom(type)) && !type.IsAbstract && !type.IsInterface)
			{
				int sortOrder = EntityConfigManager.GetSortOrder(type);
				EntityConfigManager.ConfigEntry item = new EntityConfigManager.ConfigEntry
				{
					type = type,
					sortOrder = sortOrder
				};
				list.Add(item);
			}
		}
		list.Sort((EntityConfigManager.ConfigEntry x, EntityConfigManager.ConfigEntry y) => x.sortOrder.CompareTo(y.sortOrder));
		foreach (EntityConfigManager.ConfigEntry configEntry in list)
		{
			object obj = Activator.CreateInstance(configEntry.type);
			if (obj is IEntityConfig && DlcManager.IsDlcListValidForCurrentContent((obj as IEntityConfig).GetDlcIds()))
			{
				this.RegisterEntity(obj as IEntityConfig);
			}
			if (obj is IMultiEntityConfig)
			{
				this.RegisterEntities(obj as IMultiEntityConfig);
			}
		}
	}

		public void RegisterEntity(IEntityConfig config)
	{
		KPrefabID component = config.CreatePrefab().GetComponent<KPrefabID>();
		component.requiredDlcIds = config.GetDlcIds();
		component.prefabInitFn += config.OnPrefabInit;
		component.prefabSpawnFn += config.OnSpawn;
		Assets.AddPrefab(component);
	}

		public void RegisterEntities(IMultiEntityConfig config)
	{
		foreach (GameObject gameObject in config.CreatePrefabs())
		{
			KPrefabID component = gameObject.GetComponent<KPrefabID>();
			component.prefabInitFn += config.OnPrefabInit;
			component.prefabSpawnFn += config.OnSpawn;
			Assets.AddPrefab(component);
		}
	}

		public static EntityConfigManager Instance;

		private struct ConfigEntry
	{
				public Type type;

				public int sortOrder;
	}
}
