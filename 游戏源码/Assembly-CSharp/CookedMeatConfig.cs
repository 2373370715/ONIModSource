using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002FD RID: 765
public class CookedMeatConfig : IEntityConfig
{
	// Token: 0x06000C06 RID: 3078 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000C07 RID: 3079 RVA: 0x00170D7C File Offset: 0x0016EF7C
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("CookedMeat", STRINGS.ITEMS.FOOD.COOKEDMEAT.NAME, STRINGS.ITEMS.FOOD.COOKEDMEAT.DESC, 1f, false, Assets.GetAnim("barbeque_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.COOKED_MEAT);
	}

	// Token: 0x06000C08 RID: 3080 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000C09 RID: 3081 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400091F RID: 2335
	public const string ID = "CookedMeat";

	// Token: 0x04000920 RID: 2336
	public static ComplexRecipe recipe;
}
