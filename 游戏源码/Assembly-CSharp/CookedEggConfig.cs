using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002FB RID: 763
public class CookedEggConfig : IEntityConfig
{
	// Token: 0x06000BFC RID: 3068 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000BFD RID: 3069 RVA: 0x00170CB4 File Offset: 0x0016EEB4
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("CookedEgg", STRINGS.ITEMS.FOOD.COOKEDEGG.NAME, STRINGS.ITEMS.FOOD.COOKEDEGG.DESC, 1f, false, Assets.GetAnim("cookedegg_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.COOKED_EGG);
	}

	// Token: 0x06000BFE RID: 3070 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000BFF RID: 3071 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400091B RID: 2331
	public const string ID = "CookedEgg";

	// Token: 0x0400091C RID: 2332
	public static ComplexRecipe recipe;
}
