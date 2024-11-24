using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000298 RID: 664
public class ForestForagePlantConfig : IEntityConfig
{
	// Token: 0x060009F0 RID: 2544 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060009F1 RID: 2545 RVA: 0x00168408 File Offset: 0x00166608
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("ForestForagePlant", STRINGS.ITEMS.FOOD.FORESTFORAGEPLANT.NAME, STRINGS.ITEMS.FOOD.FORESTFORAGEPLANT.DESC, 1f, false, Assets.GetAnim("podmelon_fruit_kanim"), "object", Grid.SceneLayer.BuildingBack, EntityTemplates.CollisionShape.CIRCLE, 0.3f, 0.3f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.FORESTFORAGEPLANT);
	}

	// Token: 0x060009F2 RID: 2546 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060009F3 RID: 2547 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000774 RID: 1908
	public const string ID = "ForestForagePlant";
}
