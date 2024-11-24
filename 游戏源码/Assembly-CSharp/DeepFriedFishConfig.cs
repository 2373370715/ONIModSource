using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000301 RID: 769
public class DeepFriedFishConfig : IEntityConfig
{
	// Token: 0x06000C1B RID: 3099 RVA: 0x000A9B1E File Offset: 0x000A7D1E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_DLC_2;
	}

	// Token: 0x06000C1C RID: 3100 RVA: 0x00170F18 File Offset: 0x0016F118
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("DeepFriedFish", STRINGS.ITEMS.FOOD.DEEPFRIEDFISH.NAME, STRINGS.ITEMS.FOOD.DEEPFRIEDFISH.DESC, 1f, false, Assets.GetAnim("deepfried_fish_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.DEEP_FRIED_FISH);
	}

	// Token: 0x06000C1D RID: 3101 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000C1E RID: 3102 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000928 RID: 2344
	public const string ID = "DeepFriedFish";

	// Token: 0x04000929 RID: 2345
	public static ComplexRecipe recipe;
}
