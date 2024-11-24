using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000317 RID: 791
public class PickledMealConfig : IEntityConfig
{
	// Token: 0x06000C8B RID: 3211 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000C8C RID: 3212 RVA: 0x00171934 File Offset: 0x0016FB34
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("PickledMeal", STRINGS.ITEMS.FOOD.PICKLEDMEAL.NAME, STRINGS.ITEMS.FOOD.PICKLEDMEAL.DESC, 1f, false, Assets.GetAnim("pickledmeal_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.6f, 0.7f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.PICKLEDMEAL);
		gameObject.GetComponent<KPrefabID>().AddTag(GameTags.Pickled, false);
		return gameObject;
	}

	// Token: 0x06000C8D RID: 3213 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000C8E RID: 3214 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000951 RID: 2385
	public const string ID = "PickledMeal";

	// Token: 0x04000952 RID: 2386
	public static ComplexRecipe recipe;
}
