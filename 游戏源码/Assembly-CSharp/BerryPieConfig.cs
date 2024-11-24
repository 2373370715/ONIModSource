using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002F4 RID: 756
public class BerryPieConfig : IEntityConfig
{
	// Token: 0x06000BD7 RID: 3031 RVA: 0x000A6566 File Offset: 0x000A4766
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000BD8 RID: 3032 RVA: 0x00170970 File Offset: 0x0016EB70
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("BerryPie", STRINGS.ITEMS.FOOD.BERRYPIE.NAME, STRINGS.ITEMS.FOOD.BERRYPIE.DESC, 1f, false, Assets.GetAnim("wormwood_berry_pie_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.55f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.BERRY_PIE);
	}

	// Token: 0x06000BD9 RID: 3033 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000BDA RID: 3034 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000909 RID: 2313
	public const string ID = "BerryPie";

	// Token: 0x0400090A RID: 2314
	public static ComplexRecipe recipe;
}
