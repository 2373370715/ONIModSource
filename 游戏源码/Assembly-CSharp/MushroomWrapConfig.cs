using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000312 RID: 786
public class MushroomWrapConfig : IEntityConfig
{
	// Token: 0x06000C71 RID: 3185 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000C72 RID: 3186 RVA: 0x0017170C File Offset: 0x0016F90C
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("MushroomWrap", STRINGS.ITEMS.FOOD.MUSHROOMWRAP.NAME, STRINGS.ITEMS.FOOD.MUSHROOMWRAP.DESC, 1f, false, Assets.GetAnim("mushroom_wrap_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.5f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.MUSHROOM_WRAP);
	}

	// Token: 0x06000C73 RID: 3187 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000C74 RID: 3188 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000946 RID: 2374
	public const string ID = "MushroomWrap";

	// Token: 0x04000947 RID: 2375
	public static ComplexRecipe recipe;
}
