using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002FF RID: 767
public class CurryConfig : IEntityConfig
{
	// Token: 0x06000C10 RID: 3088 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000C11 RID: 3089 RVA: 0x00170E44 File Offset: 0x0016F044
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("Curry", STRINGS.ITEMS.FOOD.CURRY.NAME, STRINGS.ITEMS.FOOD.CURRY.DESC, 1f, false, Assets.GetAnim("curried_beans_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.9f, 0.5f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.CURRY);
	}

	// Token: 0x06000C12 RID: 3090 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000C13 RID: 3091 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000923 RID: 2339
	public const string ID = "Curry";
}
