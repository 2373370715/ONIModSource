using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200032F RID: 815
public class WormBasicFoodConfig : IEntityConfig
{
	// Token: 0x06000D0A RID: 3338 RVA: 0x000A6566 File Offset: 0x000A4766
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000D0B RID: 3339 RVA: 0x001723E8 File Offset: 0x001705E8
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("WormBasicFood", STRINGS.ITEMS.FOOD.WORMBASICFOOD.NAME, STRINGS.ITEMS.FOOD.WORMBASICFOOD.DESC, 1f, false, Assets.GetAnim("wormwood_roast_nuts_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.7f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.WORMBASICFOOD);
	}

	// Token: 0x06000D0C RID: 3340 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000D0D RID: 3341 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000987 RID: 2439
	public const string ID = "WormBasicFood";

	// Token: 0x04000988 RID: 2440
	public static ComplexRecipe recipe;
}
