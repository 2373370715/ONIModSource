using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000304 RID: 772
public class DeepFriedShellfishConfig : IEntityConfig
{
	// Token: 0x06000C2A RID: 3114 RVA: 0x000A9B1E File Offset: 0x000A7D1E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_DLC_2;
	}

	// Token: 0x06000C2B RID: 3115 RVA: 0x00171044 File Offset: 0x0016F244
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("DeepFriedShellfish", STRINGS.ITEMS.FOOD.DEEPFRIEDSHELLFISH.NAME, STRINGS.ITEMS.FOOD.DEEPFRIEDSHELLFISH.DESC, 1f, false, Assets.GetAnim("deepfried_shellfish_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.DEEP_FRIED_SHELLFISH);
	}

	// Token: 0x06000C2C RID: 3116 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000C2D RID: 3117 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400092E RID: 2350
	public const string ID = "DeepFriedShellfish";

	// Token: 0x0400092F RID: 2351
	public static ComplexRecipe recipe;
}
