using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000308 RID: 776
public class FriedMushroomConfig : IEntityConfig
{
	// Token: 0x06000C3E RID: 3134 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000C3F RID: 3135 RVA: 0x001711D8 File Offset: 0x0016F3D8
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("FriedMushroom", STRINGS.ITEMS.FOOD.FRIEDMUSHROOM.NAME, STRINGS.ITEMS.FOOD.FRIEDMUSHROOM.DESC, 1f, false, Assets.GetAnim("funguscapfried_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.6f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.FRIED_MUSHROOM);
	}

	// Token: 0x06000C40 RID: 3136 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000C41 RID: 3137 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000934 RID: 2356
	public const string ID = "FriedMushroom";

	// Token: 0x04000935 RID: 2357
	public static ComplexRecipe recipe;
}
