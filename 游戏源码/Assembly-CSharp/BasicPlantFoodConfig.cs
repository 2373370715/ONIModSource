using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002F3 RID: 755
public class BasicPlantFoodConfig : IEntityConfig
{
	// Token: 0x06000BD2 RID: 3026 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000BD3 RID: 3027 RVA: 0x00170908 File Offset: 0x0016EB08
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("BasicPlantFood", STRINGS.ITEMS.FOOD.BASICPLANTFOOD.NAME, STRINGS.ITEMS.FOOD.BASICPLANTFOOD.DESC, 1f, false, Assets.GetAnim("meallicegrain_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.CIRCLE, 0.25f, 0.25f, true, 0, SimHashes.Creature, null);
		EntityTemplates.ExtendEntityToFood(gameObject, FOOD.FOOD_TYPES.BASICPLANTFOOD);
		return gameObject;
	}

	// Token: 0x06000BD4 RID: 3028 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000BD5 RID: 3029 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000908 RID: 2312
	public const string ID = "BasicPlantFood";
}
