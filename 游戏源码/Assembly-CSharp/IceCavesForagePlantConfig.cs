using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002AD RID: 685
public class IceCavesForagePlantConfig : IEntityConfig
{
	// Token: 0x06000A34 RID: 2612 RVA: 0x000A9B1E File Offset: 0x000A7D1E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_DLC_2;
	}

	// Token: 0x06000A35 RID: 2613 RVA: 0x0016A254 File Offset: 0x00168454
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("IceCavesForagePlant", STRINGS.ITEMS.FOOD.ICECAVESFORAGEPLANT.NAME, STRINGS.ITEMS.FOOD.ICECAVESFORAGEPLANT.DESC, 1f, false, Assets.GetAnim("frozenberries_fruit_kanim"), "object", Grid.SceneLayer.BuildingBack, EntityTemplates.CollisionShape.CIRCLE, 0.3f, 0.3f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.ICECAVESFORAGEPLANT);
	}

	// Token: 0x06000A36 RID: 2614 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000A37 RID: 2615 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040007E2 RID: 2018
	public const string ID = "IceCavesForagePlant";
}
