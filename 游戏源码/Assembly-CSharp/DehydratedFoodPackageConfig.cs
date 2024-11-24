using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002F7 RID: 759
public class DehydratedFoodPackageConfig : IEntityConfig
{
	// Token: 0x06000BE7 RID: 3047 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000BE8 RID: 3048 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000BE9 RID: 3049 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x06000BEA RID: 3050 RVA: 0x00170AA8 File Offset: 0x0016ECA8
	public GameObject CreatePrefab()
	{
		KAnimFile anim = Assets.GetAnim("dehydrated_food_burger_kanim");
		GameObject gameObject = EntityTemplates.CreateLooseEntity(DehydratedFoodPackageConfig.ID.Name, STRINGS.ITEMS.FOOD.BURGER.DEHYDRATED.NAME, STRINGS.ITEMS.FOOD.BURGER.DEHYDRATED.DESC, 1f, true, anim, "idle", Grid.SceneLayer.BuildingFront, EntityTemplates.CollisionShape.RECTANGLE, 0.6f, 0.7f, true, 0, SimHashes.Polypropylene, null);
		EntityTemplates.ExtendEntityToDehydratedFoodPackage(gameObject, FOOD.FOOD_TYPES.BURGER);
		return gameObject;
	}

	// Token: 0x04000911 RID: 2321
	public static Tag ID = new Tag("DehydratedFoodPackage");

	// Token: 0x04000912 RID: 2322
	public const float MASS = 1f;

	// Token: 0x04000913 RID: 2323
	public const string ANIM_FILE = "dehydrated_food_burger_kanim";

	// Token: 0x04000914 RID: 2324
	public const string INITIAL_ANIM = "idle";
}
