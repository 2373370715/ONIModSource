using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000327 RID: 807
public class SurfAndTurfConfig : IEntityConfig
{
	// Token: 0x06000CE1 RID: 3297 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000CE2 RID: 3298 RVA: 0x001720C4 File Offset: 0x001702C4
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("SurfAndTurf", STRINGS.ITEMS.FOOD.SURFANDTURF.NAME, STRINGS.ITEMS.FOOD.SURFANDTURF.DESC, 1f, false, Assets.GetAnim("surfnturf_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.SURF_AND_TURF);
	}

	// Token: 0x06000CE3 RID: 3299 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000CE4 RID: 3300 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000975 RID: 2421
	public const string ID = "SurfAndTurf";

	// Token: 0x04000976 RID: 2422
	public static ComplexRecipe recipe;
}
