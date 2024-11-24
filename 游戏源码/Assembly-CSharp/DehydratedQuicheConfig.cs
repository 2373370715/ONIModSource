using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200031C RID: 796
public class DehydratedQuicheConfig : IEntityConfig
{
	// Token: 0x06000CA4 RID: 3236 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000CA5 RID: 3237 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000CA6 RID: 3238 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x06000CA7 RID: 3239 RVA: 0x00171BB0 File Offset: 0x0016FDB0
	public GameObject CreatePrefab()
	{
		KAnimFile anim = Assets.GetAnim("dehydrated_food_quiche_kanim");
		GameObject gameObject = EntityTemplates.CreateLooseEntity(DehydratedQuicheConfig.ID.Name, STRINGS.ITEMS.FOOD.QUICHE.DEHYDRATED.NAME, STRINGS.ITEMS.FOOD.QUICHE.DEHYDRATED.DESC, 1f, true, anim, "idle", Grid.SceneLayer.BuildingFront, EntityTemplates.CollisionShape.RECTANGLE, 0.6f, 0.7f, true, 0, SimHashes.Polypropylene, null);
		EntityTemplates.ExtendEntityToDehydratedFoodPackage(gameObject, FOOD.FOOD_TYPES.QUICHE);
		return gameObject;
	}

	// Token: 0x0400095A RID: 2394
	public static Tag ID = new Tag("DehydratedQuiche");

	// Token: 0x0400095B RID: 2395
	public const float MASS = 1f;

	// Token: 0x0400095C RID: 2396
	public const string ANIM_FILE = "dehydrated_food_quiche_kanim";

	// Token: 0x0400095D RID: 2397
	public const string INITIAL_ANIM = "idle";
}
