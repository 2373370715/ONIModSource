using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000315 RID: 789
public class PancakesConfig : IEntityConfig
{
	// Token: 0x06000C81 RID: 3201 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000C82 RID: 3202 RVA: 0x00171848 File Offset: 0x0016FA48
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("Pancakes", STRINGS.ITEMS.FOOD.PANCAKES.NAME, STRINGS.ITEMS.FOOD.PANCAKES.DESC, 1f, false, Assets.GetAnim("stackedpancakes_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.8f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.PANCAKES);
	}

	// Token: 0x06000C83 RID: 3203 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000C84 RID: 3204 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400094D RID: 2381
	public const string ID = "Pancakes";

	// Token: 0x0400094E RID: 2382
	public static ComplexRecipe recipe;
}
