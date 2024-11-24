using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002F6 RID: 758
public class BurgerConfig : IEntityConfig
{
	// Token: 0x06000BE2 RID: 3042 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000BE3 RID: 3043 RVA: 0x00170A44 File Offset: 0x0016EC44
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("Burger", STRINGS.ITEMS.FOOD.BURGER.NAME, STRINGS.ITEMS.FOOD.BURGER.DESC, 1f, false, Assets.GetAnim("frost_burger_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.BURGER);
	}

	// Token: 0x06000BE4 RID: 3044 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000BE5 RID: 3045 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400090F RID: 2319
	public const string ID = "Burger";

	// Token: 0x04000910 RID: 2320
	public static ComplexRecipe recipe;
}
