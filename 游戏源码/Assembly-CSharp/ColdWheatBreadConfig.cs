using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002FA RID: 762
public class ColdWheatBreadConfig : IEntityConfig
{
	// Token: 0x06000BF7 RID: 3063 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000BF8 RID: 3064 RVA: 0x00170C50 File Offset: 0x0016EE50
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("ColdWheatBread", STRINGS.ITEMS.FOOD.COLDWHEATBREAD.NAME, STRINGS.ITEMS.FOOD.COLDWHEATBREAD.DESC, 1f, false, Assets.GetAnim("frostbread_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.COLD_WHEAT_BREAD);
	}

	// Token: 0x06000BF9 RID: 3065 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000BFA RID: 3066 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000919 RID: 2329
	public const string ID = "ColdWheatBread";

	// Token: 0x0400091A RID: 2330
	public static ComplexRecipe recipe;
}
