using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000289 RID: 649
public class BasicForagePlantConfig : IEntityConfig
{
	// Token: 0x060009A3 RID: 2467 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060009A4 RID: 2468 RVA: 0x00166A24 File Offset: 0x00164C24
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("BasicForagePlant", STRINGS.ITEMS.FOOD.BASICFORAGEPLANT.NAME, STRINGS.ITEMS.FOOD.BASICFORAGEPLANT.DESC, 1f, false, Assets.GetAnim("muckrootvegetable_kanim"), "object", Grid.SceneLayer.BuildingBack, EntityTemplates.CollisionShape.CIRCLE, 0.3f, 0.3f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.BASICFORAGEPLANT);
	}

	// Token: 0x060009A5 RID: 2469 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060009A6 RID: 2470 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000738 RID: 1848
	public const string ID = "BasicForagePlant";
}
