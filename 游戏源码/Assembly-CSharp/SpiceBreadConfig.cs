using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000322 RID: 802
public class SpiceBreadConfig : IEntityConfig
{
	// Token: 0x06000CC5 RID: 3269 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000CC6 RID: 3270 RVA: 0x00171E84 File Offset: 0x00170084
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("SpiceBread", STRINGS.ITEMS.FOOD.SPICEBREAD.NAME, STRINGS.ITEMS.FOOD.SPICEBREAD.DESC, 1f, false, Assets.GetAnim("pepperbread_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.9f, 0.6f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.SPICEBREAD);
	}

	// Token: 0x06000CC7 RID: 3271 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000CC8 RID: 3272 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000967 RID: 2407
	public const string ID = "SpiceBread";

	// Token: 0x04000968 RID: 2408
	public static ComplexRecipe recipe;
}
