using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000329 RID: 809
public class SwampDelightsConfig : IEntityConfig
{
	// Token: 0x06000CEC RID: 3308 RVA: 0x000A6566 File Offset: 0x000A4766
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000CED RID: 3309 RVA: 0x00172198 File Offset: 0x00170398
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("SwampDelights", STRINGS.ITEMS.FOOD.SWAMPDELIGHTS.NAME, STRINGS.ITEMS.FOOD.SWAMPDELIGHTS.DESC, 1f, false, Assets.GetAnim("swamp_delights_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.7f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.SWAMP_DELIGHTS);
	}

	// Token: 0x06000CEE RID: 3310 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000CEF RID: 3311 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400097C RID: 2428
	public const string ID = "SwampDelights";
}
