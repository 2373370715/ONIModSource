using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002DB RID: 731
public class SwampForagePlantConfig : IEntityConfig
{
	// Token: 0x06000B69 RID: 2921 RVA: 0x000A6566 File Offset: 0x000A4766
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000B6A RID: 2922 RVA: 0x0016EFE8 File Offset: 0x0016D1E8
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("SwampForagePlant", STRINGS.ITEMS.FOOD.SWAMPFORAGEPLANT.NAME, STRINGS.ITEMS.FOOD.SWAMPFORAGEPLANT.DESC, 1f, false, Assets.GetAnim("swamptuber_vegetable_kanim"), "object", Grid.SceneLayer.BuildingBack, EntityTemplates.CollisionShape.CIRCLE, 0.3f, 0.3f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.SWAMPFORAGEPLANT);
	}

	// Token: 0x06000B6B RID: 2923 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000B6C RID: 2924 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040008CC RID: 2252
	public const string ID = "SwampForagePlant";
}
