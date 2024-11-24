using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000302 RID: 770
public class DeepFriedMeatConfig : IEntityConfig
{
	// Token: 0x06000C20 RID: 3104 RVA: 0x000A9B1E File Offset: 0x000A7D1E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_DLC_2;
	}

	// Token: 0x06000C21 RID: 3105 RVA: 0x00170F7C File Offset: 0x0016F17C
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("DeepFriedMeat", STRINGS.ITEMS.FOOD.DEEPFRIEDMEAT.NAME, STRINGS.ITEMS.FOOD.DEEPFRIEDMEAT.DESC, 1f, false, Assets.GetAnim("deepfried_meat_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.DEEP_FRIED_MEAT);
	}

	// Token: 0x06000C22 RID: 3106 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000C23 RID: 3107 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400092A RID: 2346
	public const string ID = "DeepFriedMeat";

	// Token: 0x0400092B RID: 2347
	public static ComplexRecipe recipe;
}
