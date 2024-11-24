using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000326 RID: 806
public class DehydratedSpicyTofuConfig : IEntityConfig
{
	// Token: 0x06000CDB RID: 3291 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000CDC RID: 3292 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000CDD RID: 3293 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x06000CDE RID: 3294 RVA: 0x00172054 File Offset: 0x00170254
	public GameObject CreatePrefab()
	{
		KAnimFile anim = Assets.GetAnim("dehydrated_food_spicy_tofu_kanim");
		GameObject gameObject = EntityTemplates.CreateLooseEntity(DehydratedSpicyTofuConfig.ID.Name, STRINGS.ITEMS.FOOD.SPICYTOFU.DEHYDRATED.NAME, STRINGS.ITEMS.FOOD.SPICYTOFU.DEHYDRATED.DESC, 1f, true, anim, "idle", Grid.SceneLayer.BuildingFront, EntityTemplates.CollisionShape.RECTANGLE, 0.6f, 0.7f, true, 0, SimHashes.Polypropylene, null);
		EntityTemplates.ExtendEntityToDehydratedFoodPackage(gameObject, FOOD.FOOD_TYPES.SPICY_TOFU);
		return gameObject;
	}

	// Token: 0x04000971 RID: 2417
	public static Tag ID = new Tag("DehydratedSpicyTofu");

	// Token: 0x04000972 RID: 2418
	public const float MASS = 1f;

	// Token: 0x04000973 RID: 2419
	public const string ANIM_FILE = "dehydrated_food_spicy_tofu_kanim";

	// Token: 0x04000974 RID: 2420
	public const string INITIAL_ANIM = "idle";
}
