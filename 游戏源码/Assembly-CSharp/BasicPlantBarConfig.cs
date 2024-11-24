using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002F2 RID: 754
public class BasicPlantBarConfig : IEntityConfig
{
	// Token: 0x06000BCD RID: 3021 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000BCE RID: 3022 RVA: 0x00170880 File Offset: 0x0016EA80
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("BasicPlantBar", STRINGS.ITEMS.FOOD.BASICPLANTBAR.NAME, STRINGS.ITEMS.FOOD.BASICPLANTBAR.DESC, 1f, false, Assets.GetAnim("liceloaf_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null);
		gameObject = EntityTemplates.ExtendEntityToFood(gameObject, FOOD.FOOD_TYPES.BASICPLANTBAR);
		ComplexRecipeManager.Get().GetRecipe(BasicPlantBarConfig.recipe.id).FabricationVisualizer = MushBarConfig.CreateFabricationVisualizer(gameObject);
		return gameObject;
	}

	// Token: 0x06000BCF RID: 3023 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000BD0 RID: 3024 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000906 RID: 2310
	public const string ID = "BasicPlantBar";

	// Token: 0x04000907 RID: 2311
	public static ComplexRecipe recipe;
}
