using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000323 RID: 803
public class DehydratedSpiceBreadConfig : IEntityConfig
{
	// Token: 0x06000CCA RID: 3274 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000CCB RID: 3275 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000CCC RID: 3276 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x06000CCD RID: 3277 RVA: 0x00171EE8 File Offset: 0x001700E8
	public GameObject CreatePrefab()
	{
		KAnimFile anim = Assets.GetAnim("dehydrated_food_spicebread_kanim");
		GameObject gameObject = EntityTemplates.CreateLooseEntity(DehydratedSpiceBreadConfig.ID.Name, STRINGS.ITEMS.FOOD.SPICEBREAD.DEHYDRATED.NAME, STRINGS.ITEMS.FOOD.SPICEBREAD.DEHYDRATED.DESC, 1f, true, anim, "idle", Grid.SceneLayer.BuildingFront, EntityTemplates.CollisionShape.RECTANGLE, 0.6f, 0.7f, true, 0, SimHashes.Polypropylene, null);
		EntityTemplates.ExtendEntityToDehydratedFoodPackage(gameObject, FOOD.FOOD_TYPES.SPICEBREAD);
		return gameObject;
	}

	// Token: 0x04000969 RID: 2409
	public static Tag ID = new Tag("DehydratedSpiceBread");

	// Token: 0x0400096A RID: 2410
	public const float MASS = 1f;

	// Token: 0x0400096B RID: 2411
	public const string ANIM_FILE = "dehydrated_food_spicebread_kanim";

	// Token: 0x0400096C RID: 2412
	public const string INITIAL_ANIM = "idle";
}
