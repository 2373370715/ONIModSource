using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200030C RID: 780
public class GrilledPrickleFruitConfig : IEntityConfig
{
	// Token: 0x06000C52 RID: 3154 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000C53 RID: 3155 RVA: 0x0017138C File Offset: 0x0016F58C
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("GrilledPrickleFruit", STRINGS.ITEMS.FOOD.GRILLEDPRICKLEFRUIT.NAME, STRINGS.ITEMS.FOOD.GRILLEDPRICKLEFRUIT.DESC, 1f, false, Assets.GetAnim("gristleberry_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.7f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.GRILLED_PRICKLEFRUIT);
	}

	// Token: 0x06000C54 RID: 3156 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000C55 RID: 3157 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400093C RID: 2364
	public const string ID = "GrilledPrickleFruit";

	// Token: 0x0400093D RID: 2365
	public static ComplexRecipe recipe;
}
