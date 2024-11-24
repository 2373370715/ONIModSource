using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000306 RID: 774
public class FishMeatConfig : IEntityConfig
{
	// Token: 0x06000C34 RID: 3124 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000C35 RID: 3125 RVA: 0x0017110C File Offset: 0x0016F30C
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("FishMeat", STRINGS.ITEMS.FOOD.FISHMEAT.NAME, STRINGS.ITEMS.FOOD.FISHMEAT.DESC, 1f, false, Assets.GetAnim("pacufillet_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null);
		EntityTemplates.ExtendEntityToFood(gameObject, FOOD.FOOD_TYPES.FISH_MEAT);
		return gameObject;
	}

	// Token: 0x06000C36 RID: 3126 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000C37 RID: 3127 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000931 RID: 2353
	public const string ID = "FishMeat";
}
