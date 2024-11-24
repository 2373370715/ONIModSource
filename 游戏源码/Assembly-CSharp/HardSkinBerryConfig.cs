using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002AB RID: 683
public class HardSkinBerryConfig : IEntityConfig
{
	// Token: 0x06000A2A RID: 2602 RVA: 0x000A9B1E File Offset: 0x000A7D1E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_DLC_2;
	}

	// Token: 0x06000A2B RID: 2603 RVA: 0x0016A008 File Offset: 0x00168208
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("HardSkinBerry", STRINGS.ITEMS.FOOD.HARDSKINBERRY.NAME, STRINGS.ITEMS.FOOD.HARDSKINBERRY.DESC, 1f, false, Assets.GetAnim("iceBerry_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.CIRCLE, 0.25f, 0.25f, true, 0, SimHashes.Creature, null);
		EntityTemplates.ExtendEntityToFood(gameObject, FOOD.FOOD_TYPES.HARDSKINBERRY);
		return gameObject;
	}

	// Token: 0x06000A2C RID: 2604 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000A2D RID: 2605 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040007DA RID: 2010
	public const string ID = "HardSkinBerry";
}
