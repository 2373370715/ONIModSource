using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002FE RID: 766
public class CookedPikeappleConfig : IEntityConfig
{
	// Token: 0x06000C0B RID: 3083 RVA: 0x000A9B1E File Offset: 0x000A7D1E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_DLC_2;
	}

	// Token: 0x06000C0C RID: 3084 RVA: 0x00170DE0 File Offset: 0x0016EFE0
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("CookedPikeapple", STRINGS.ITEMS.FOOD.COOKEDPIKEAPPLE.NAME, STRINGS.ITEMS.FOOD.COOKEDPIKEAPPLE.DESC, 1f, false, Assets.GetAnim("iceberry_cooked_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.6f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.COOKED_PIKEAPPLE);
	}

	// Token: 0x06000C0D RID: 3085 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000C0E RID: 3086 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000921 RID: 2337
	public const string ID = "CookedPikeapple";

	// Token: 0x04000922 RID: 2338
	public static ComplexRecipe recipe;
}
