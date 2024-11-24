using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000316 RID: 790
public class PemmicanConfig : IEntityConfig
{
	// Token: 0x06000C86 RID: 3206 RVA: 0x000A9B1E File Offset: 0x000A7D1E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_DLC_2;
	}

	// Token: 0x06000C87 RID: 3207 RVA: 0x001718AC File Offset: 0x0016FAAC
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("Pemmican", STRINGS.ITEMS.FOOD.PEMMICAN.NAME, STRINGS.ITEMS.FOOD.PEMMICAN.DESC, 1f, false, Assets.GetAnim("pemmican_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null);
		gameObject = EntityTemplates.ExtendEntityToFood(gameObject, FOOD.FOOD_TYPES.PEMMICAN);
		ComplexRecipeManager.Get().GetRecipe(PemmicanConfig.recipe.id).FabricationVisualizer = MushBarConfig.CreateFabricationVisualizer(gameObject);
		return gameObject;
	}

	// Token: 0x06000C88 RID: 3208 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000C89 RID: 3209 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400094F RID: 2383
	public const string ID = "Pemmican";

	// Token: 0x04000950 RID: 2384
	public static ComplexRecipe recipe;
}
