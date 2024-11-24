using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200030B RID: 779
public class GammaMushConfig : IEntityConfig
{
	// Token: 0x06000C4D RID: 3149 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000C4E RID: 3150 RVA: 0x00171328 File Offset: 0x0016F528
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("GammaMush", STRINGS.ITEMS.FOOD.GAMMAMUSH.NAME, STRINGS.ITEMS.FOOD.GAMMAMUSH.DESC, 1f, false, Assets.GetAnim("mushbarfried_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.GAMMAMUSH);
	}

	// Token: 0x06000C4F RID: 3151 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000C50 RID: 3152 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400093A RID: 2362
	public const string ID = "GammaMush";

	// Token: 0x0400093B RID: 2363
	public static ComplexRecipe recipe;
}
