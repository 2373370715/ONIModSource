using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000332 RID: 818
public class WormSuperFruitConfig : IEntityConfig
{
	// Token: 0x06000D19 RID: 3353 RVA: 0x000A6566 File Offset: 0x000A4766
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000D1A RID: 3354 RVA: 0x00172514 File Offset: 0x00170714
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("WormSuperFruit", STRINGS.ITEMS.FOOD.WORMSUPERFRUIT.NAME, STRINGS.ITEMS.FOOD.WORMSUPERFRUIT.DESC, 1f, false, Assets.GetAnim("wormwood_super_fruits_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.6f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.WORMSUPERFRUIT);
	}

	// Token: 0x06000D1B RID: 3355 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000D1C RID: 3356 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400098C RID: 2444
	public const string ID = "WormSuperFruit";
}
