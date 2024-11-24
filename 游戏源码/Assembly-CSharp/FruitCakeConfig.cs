using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200030A RID: 778
public class FruitCakeConfig : IEntityConfig
{
	// Token: 0x06000C48 RID: 3144 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000C49 RID: 3145 RVA: 0x001712A0 File Offset: 0x0016F4A0
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("FruitCake", STRINGS.ITEMS.FOOD.FRUITCAKE.NAME, STRINGS.ITEMS.FOOD.FRUITCAKE.DESC, 1f, false, Assets.GetAnim("fruitcake_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null);
		gameObject = EntityTemplates.ExtendEntityToFood(gameObject, FOOD.FOOD_TYPES.FRUITCAKE);
		ComplexRecipeManager.Get().GetRecipe(FruitCakeConfig.recipe.id).FabricationVisualizer = MushBarConfig.CreateFabricationVisualizer(gameObject);
		return gameObject;
	}

	// Token: 0x06000C4A RID: 3146 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000C4B RID: 3147 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000938 RID: 2360
	public const string ID = "FruitCake";

	// Token: 0x04000939 RID: 2361
	public static ComplexRecipe recipe;
}
