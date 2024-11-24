using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000318 RID: 792
public class PlantMeatConfig : IEntityConfig
{
	// Token: 0x06000C90 RID: 3216 RVA: 0x000A6566 File Offset: 0x000A4766
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000C91 RID: 3217 RVA: 0x001719AC File Offset: 0x0016FBAC
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("PlantMeat", STRINGS.ITEMS.FOOD.PLANTMEAT.NAME, STRINGS.ITEMS.FOOD.PLANTMEAT.DESC, 1f, false, Assets.GetAnim("critter_trap_fruit_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null);
		EntityTemplates.ExtendEntityToFood(gameObject, FOOD.FOOD_TYPES.PLANTMEAT);
		return gameObject;
	}

	// Token: 0x06000C92 RID: 3218 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000C93 RID: 3219 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000953 RID: 2387
	public const string ID = "PlantMeat";
}
