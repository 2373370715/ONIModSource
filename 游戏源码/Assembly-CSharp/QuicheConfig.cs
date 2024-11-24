using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200031B RID: 795
public class QuicheConfig : IEntityConfig
{
	// Token: 0x06000C9F RID: 3231 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000CA0 RID: 3232 RVA: 0x00171B4C File Offset: 0x0016FD4C
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("Quiche", STRINGS.ITEMS.FOOD.QUICHE.NAME, STRINGS.ITEMS.FOOD.QUICHE.DESC, 1f, false, Assets.GetAnim("quiche_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.QUICHE);
	}

	// Token: 0x06000CA1 RID: 3233 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000CA2 RID: 3234 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000958 RID: 2392
	public const string ID = "Quiche";

	// Token: 0x04000959 RID: 2393
	public static ComplexRecipe recipe;
}
