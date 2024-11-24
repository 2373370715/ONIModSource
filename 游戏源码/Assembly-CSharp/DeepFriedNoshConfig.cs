using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000303 RID: 771
public class DeepFriedNoshConfig : IEntityConfig
{
	// Token: 0x06000C25 RID: 3109 RVA: 0x000A9B1E File Offset: 0x000A7D1E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_DLC_2;
	}

	// Token: 0x06000C26 RID: 3110 RVA: 0x00170FE0 File Offset: 0x0016F1E0
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("DeepFriedNosh", STRINGS.ITEMS.FOOD.DEEPFRIEDNOSH.NAME, STRINGS.ITEMS.FOOD.DEEPFRIEDNOSH.DESC, 1f, false, Assets.GetAnim("deepfried_nosh_beans_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.DEEP_FRIED_NOSH);
	}

	// Token: 0x06000C27 RID: 3111 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000C28 RID: 3112 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400092C RID: 2348
	public const string ID = "DeepFriedNosh";

	// Token: 0x0400092D RID: 2349
	public static ComplexRecipe recipe;
}
