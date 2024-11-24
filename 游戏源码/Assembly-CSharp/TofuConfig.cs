using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200032E RID: 814
public class TofuConfig : IEntityConfig
{
	// Token: 0x06000D05 RID: 3333 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000D06 RID: 3334 RVA: 0x00172360 File Offset: 0x00170560
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("Tofu", STRINGS.ITEMS.FOOD.TOFU.NAME, STRINGS.ITEMS.FOOD.TOFU.DESC, 1f, false, Assets.GetAnim("loafu_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.9f, 0.6f, true, 0, SimHashes.Creature, null);
		gameObject = EntityTemplates.ExtendEntityToFood(gameObject, FOOD.FOOD_TYPES.TOFU);
		ComplexRecipeManager.Get().GetRecipe(TofuConfig.recipe.id).FabricationVisualizer = MushBarConfig.CreateFabricationVisualizer(gameObject);
		return gameObject;
	}

	// Token: 0x06000D07 RID: 3335 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000D08 RID: 3336 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000985 RID: 2437
	public const string ID = "Tofu";

	// Token: 0x04000986 RID: 2438
	public static ComplexRecipe recipe;
}
