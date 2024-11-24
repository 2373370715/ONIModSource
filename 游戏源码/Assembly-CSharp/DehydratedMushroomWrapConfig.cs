using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000313 RID: 787
public class DehydratedMushroomWrapConfig : IEntityConfig
{
	// Token: 0x06000C76 RID: 3190 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000C77 RID: 3191 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000C78 RID: 3192 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x06000C79 RID: 3193 RVA: 0x00171770 File Offset: 0x0016F970
	public GameObject CreatePrefab()
	{
		KAnimFile anim = Assets.GetAnim("dehydrated_food_mushroom_wrap_kanim");
		GameObject gameObject = EntityTemplates.CreateLooseEntity(DehydratedMushroomWrapConfig.ID.Name, STRINGS.ITEMS.FOOD.MUSHROOMWRAP.DEHYDRATED.NAME, STRINGS.ITEMS.FOOD.MUSHROOMWRAP.DEHYDRATED.DESC, 1f, true, anim, "idle", Grid.SceneLayer.BuildingFront, EntityTemplates.CollisionShape.RECTANGLE, 0.6f, 0.7f, true, 0, SimHashes.Polypropylene, null);
		EntityTemplates.ExtendEntityToDehydratedFoodPackage(gameObject, FOOD.FOOD_TYPES.MUSHROOM_WRAP);
		return gameObject;
	}

	// Token: 0x04000948 RID: 2376
	public static Tag ID = new Tag("DehydratedMushroomWrap");

	// Token: 0x04000949 RID: 2377
	public const float MASS = 1f;

	// Token: 0x0400094A RID: 2378
	public const string ANIM_FILE = "dehydrated_food_mushroom_wrap_kanim";

	// Token: 0x0400094B RID: 2379
	public const string INITIAL_ANIM = "idle";
}
