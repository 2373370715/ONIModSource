using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002FC RID: 764
public class CookedFishConfig : IEntityConfig
{
	// Token: 0x06000C01 RID: 3073 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000C02 RID: 3074 RVA: 0x00170D18 File Offset: 0x0016EF18
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("CookedFish", STRINGS.ITEMS.FOOD.COOKEDFISH.NAME, STRINGS.ITEMS.FOOD.COOKEDFISH.DESC, 1f, false, Assets.GetAnim("grilled_pacu_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.COOKED_FISH);
	}

	// Token: 0x06000C03 RID: 3075 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000C04 RID: 3076 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400091D RID: 2333
	public const string ID = "CookedFish";

	// Token: 0x0400091E RID: 2334
	public static ComplexRecipe recipe;
}
