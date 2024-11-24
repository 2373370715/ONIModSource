using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200032A RID: 810
public class SwampFruitConfig : IEntityConfig
{
	// Token: 0x06000CF1 RID: 3313 RVA: 0x000A6566 File Offset: 0x000A4766
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000CF2 RID: 3314 RVA: 0x001721FC File Offset: 0x001703FC
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity(SwampFruitConfig.ID, STRINGS.ITEMS.FOOD.SWAMPFRUIT.NAME, STRINGS.ITEMS.FOOD.SWAMPFRUIT.DESC, 1f, false, Assets.GetAnim("swampcrop_fruit_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 1f, 0.72f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.SWAMPFRUIT);
	}

	// Token: 0x06000CF3 RID: 3315 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000CF4 RID: 3316 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400097D RID: 2429
	public static string ID = "SwampFruit";
}
