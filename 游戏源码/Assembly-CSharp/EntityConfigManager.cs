using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200129E RID: 4766
[AddComponentMenu("KMonoBehaviour/scripts/EntityConfigManager")]
public class EntityConfigManager : KMonoBehaviour
{
	// Token: 0x06006212 RID: 25106 RVA: 0x000DFFEA File Offset: 0x000DE1EA
	public static void DestroyInstance()
	{
		EntityConfigManager.Instance = null;
	}

	// Token: 0x06006213 RID: 25107 RVA: 0x000DFFF2 File Offset: 0x000DE1F2
	protected override void OnPrefabInit()
	{
		EntityConfigManager.Instance = this;
	}

	// Token: 0x06006214 RID: 25108 RVA: 0x002B51B0 File Offset: 0x002B33B0
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

	// Token: 0x06006215 RID: 25109 RVA: 0x002B5200 File Offset: 0x002B3400
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

	// Token: 0x06006216 RID: 25110 RVA: 0x002B5360 File Offset: 0x002B3560
	public void RegisterEntity(IEntityConfig config)
	{
		KPrefabID component = config.CreatePrefab().GetComponent<KPrefabID>();
		component.requiredDlcIds = config.GetDlcIds();
		component.prefabInitFn += config.OnPrefabInit;
		component.prefabSpawnFn += config.OnSpawn;
		Assets.AddPrefab(component);
	}

	// Token: 0x06006217 RID: 25111 RVA: 0x002B53B0 File Offset: 0x002B35B0
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

	// Token: 0x040045CB RID: 17867
	public static EntityConfigManager Instance;

	// Token: 0x0200129F RID: 4767
	private struct ConfigEntry
	{
		// Token: 0x040045CC RID: 17868
		public Type type;

		// Token: 0x040045CD RID: 17869
		public int sortOrder;
	}
}
