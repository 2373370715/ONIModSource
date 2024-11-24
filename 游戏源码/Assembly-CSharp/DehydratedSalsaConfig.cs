using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000320 RID: 800
public class DehydratedSalsaConfig : IEntityConfig
{
	// Token: 0x06000CBA RID: 3258 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000CBB RID: 3259 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000CBC RID: 3260 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x06000CBD RID: 3261 RVA: 0x00171DAC File Offset: 0x0016FFAC
	public GameObject CreatePrefab()
	{
		KAnimFile anim = Assets.GetAnim("dehydrated_food_salsa_kanim");
		GameObject gameObject = EntityTemplates.CreateLooseEntity(DehydratedSalsaConfig.ID.Name, STRINGS.ITEMS.FOOD.SALSA.DEHYDRATED.NAME, STRINGS.ITEMS.FOOD.SALSA.DEHYDRATED.DESC, 1f, true, anim, "idle", Grid.SceneLayer.BuildingFront, EntityTemplates.CollisionShape.RECTANGLE, 0.6f, 0.7f, true, 0, SimHashes.Polypropylene, null);
		EntityTemplates.ExtendEntityToDehydratedFoodPackage(gameObject, FOOD.FOOD_TYPES.SALSA);
		return gameObject;
	}

	// Token: 0x04000962 RID: 2402
	public static Tag ID = new Tag("DehydratedSalsa");

	// Token: 0x04000963 RID: 2403
	public const float MASS = 1f;

	// Token: 0x04000964 RID: 2404
	public const string ANIM_FILE = "dehydrated_food_salsa_kanim";

	// Token: 0x04000965 RID: 2405
	public const string INITIAL_ANIM = "idle";
}
