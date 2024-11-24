using STRINGS;
using TUNING;
using UnityEngine;

public class PancakesConfig : IEntityConfig
{
	public const string ID = "Pancakes";

	public static ComplexRecipe recipe;

	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("Pancakes", ITEMS.FOOD.PANCAKES.NAME, ITEMS.FOOD.PANCAKES.DESC, 1f, unitMass: false, Assets.GetAnim("stackedpancakes_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.8f, isPickupable: true), FOOD.FOOD_TYPES.PANCAKES);
	}

	public void OnPrefabInit(GameObject inst)
	{
	}

	public void OnSpawn(GameObject inst)
	{
	}
}
