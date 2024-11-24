using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000300 RID: 768
public class DehydratedCurryConfig : IEntityConfig
{
	// Token: 0x06000C15 RID: 3093 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000C16 RID: 3094 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000C17 RID: 3095 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x06000C18 RID: 3096 RVA: 0x00170EA8 File Offset: 0x0016F0A8
	public GameObject CreatePrefab()
	{
		KAnimFile anim = Assets.GetAnim("dehydrated_food_curry_kanim");
		GameObject gameObject = EntityTemplates.CreateLooseEntity(DehydratedCurryConfig.ID.Name, STRINGS.ITEMS.FOOD.CURRY.DEHYDRATED.NAME, STRINGS.ITEMS.FOOD.CURRY.DEHYDRATED.DESC, 1f, true, anim, "idle", Grid.SceneLayer.BuildingFront, EntityTemplates.CollisionShape.RECTANGLE, 0.6f, 0.7f, true, 0, SimHashes.Polypropylene, null);
		EntityTemplates.ExtendEntityToDehydratedFoodPackage(gameObject, FOOD.FOOD_TYPES.CURRY);
		return gameObject;
	}

	// Token: 0x04000924 RID: 2340
	public static Tag ID = new Tag("DehydratedCurry");

	// Token: 0x04000925 RID: 2341
	public const float MASS = 1f;

	// Token: 0x04000926 RID: 2342
	public const string ANIM_FILE = "dehydrated_food_curry_kanim";

	// Token: 0x04000927 RID: 2343
	public const string INITIAL_ANIM = "idle";
}
