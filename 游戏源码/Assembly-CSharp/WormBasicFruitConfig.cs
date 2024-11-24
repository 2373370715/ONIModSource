using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000330 RID: 816
public class WormBasicFruitConfig : IEntityConfig
{
	// Token: 0x06000D0F RID: 3343 RVA: 0x000A6566 File Offset: 0x000A4766
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000D10 RID: 3344 RVA: 0x0017244C File Offset: 0x0017064C
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("WormBasicFruit", STRINGS.ITEMS.FOOD.WORMBASICFRUIT.NAME, STRINGS.ITEMS.FOOD.WORMBASICFRUIT.DESC, 1f, false, Assets.GetAnim("wormwood_basic_fruit_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.7f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.WORMBASICFRUIT);
	}

	// Token: 0x06000D11 RID: 3345 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000D12 RID: 3346 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000989 RID: 2441
	public const string ID = "WormBasicFruit";
}
