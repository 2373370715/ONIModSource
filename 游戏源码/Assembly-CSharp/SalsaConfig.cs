using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200031F RID: 799
public class SalsaConfig : IEntityConfig
{
	// Token: 0x06000CB5 RID: 3253 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000CB6 RID: 3254 RVA: 0x00171D48 File Offset: 0x0016FF48
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("Salsa", STRINGS.ITEMS.FOOD.SALSA.NAME, STRINGS.ITEMS.FOOD.SALSA.DESC, 1f, false, Assets.GetAnim("zestysalsa_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.5f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.SALSA);
	}

	// Token: 0x06000CB7 RID: 3255 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000CB8 RID: 3256 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000960 RID: 2400
	public const string ID = "Salsa";

	// Token: 0x04000961 RID: 2401
	public static ComplexRecipe recipe;
}
