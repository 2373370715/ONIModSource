using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000325 RID: 805
public class SpicyTofuConfig : IEntityConfig
{
	// Token: 0x06000CD6 RID: 3286 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000CD7 RID: 3287 RVA: 0x00171FF0 File Offset: 0x001701F0
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("SpicyTofu", STRINGS.ITEMS.FOOD.SPICYTOFU.NAME, STRINGS.ITEMS.FOOD.SPICYTOFU.DESC, 1f, false, Assets.GetAnim("spicey_tofu_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.9f, 0.6f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.SPICY_TOFU);
	}

	// Token: 0x06000CD8 RID: 3288 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000CD9 RID: 3289 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400096F RID: 2415
	public const string ID = "SpicyTofu";

	// Token: 0x04000970 RID: 2416
	public static ComplexRecipe recipe;
}
