using STRINGS;
using TUNING;
using UnityEngine;

public class PemmicanConfig : IEntityConfig
{
	public const string ID = "Pemmican";

	public static ComplexRecipe recipe;

	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_DLC_2;
	}

	public GameObject CreatePrefab()
	{
		GameObject template = EntityTemplates.CreateLooseEntity("Pemmican", ITEMS.FOOD.PEMMICAN.NAME, ITEMS.FOOD.PEMMICAN.DESC, 1f, unitMass: false, Assets.GetAnim("pemmican_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, isPickupable: true);
		template = EntityTemplates.ExtendEntityToFood(template, FOOD.FOOD_TYPES.PEMMICAN);
		ComplexRecipeManager.Get().GetRecipe(recipe.id).FabricationVisualizer = MushBarConfig.CreateFabricationVisualizer(template);
		return template;
	}

	public void OnPrefabInit(GameObject inst)
	{
	}

	public void OnSpawn(GameObject inst)
	{
	}
}
