using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000328 RID: 808
public class DehydratedSurfAndTurfConfig : IEntityConfig
{
	// Token: 0x06000CE6 RID: 3302 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000CE7 RID: 3303 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000CE8 RID: 3304 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x06000CE9 RID: 3305 RVA: 0x00172128 File Offset: 0x00170328
	public GameObject CreatePrefab()
	{
		KAnimFile anim = Assets.GetAnim("dehydrated_food_surf_and_turf_kanim");
		GameObject gameObject = EntityTemplates.CreateLooseEntity(DehydratedSurfAndTurfConfig.ID.Name, STRINGS.ITEMS.FOOD.SURFANDTURF.DEHYDRATED.NAME, STRINGS.ITEMS.FOOD.SURFANDTURF.DEHYDRATED.DESC, 1f, true, anim, "idle", Grid.SceneLayer.BuildingFront, EntityTemplates.CollisionShape.RECTANGLE, 0.6f, 0.7f, true, 0, SimHashes.Polypropylene, null);
		EntityTemplates.ExtendEntityToDehydratedFoodPackage(gameObject, FOOD.FOOD_TYPES.SURF_AND_TURF);
		return gameObject;
	}

	// Token: 0x04000977 RID: 2423
	public static Tag ID = new Tag("DehydratedSurfAndTurf");

	// Token: 0x04000978 RID: 2424
	public const float MASS = 1f;

	// Token: 0x04000979 RID: 2425
	public const int FABRICATION_TIME_SECONDS = 300;

	// Token: 0x0400097A RID: 2426
	public const string ANIM_FILE = "dehydrated_food_surf_and_turf_kanim";

	// Token: 0x0400097B RID: 2427
	public const string INITIAL_ANIM = "idle";
}
