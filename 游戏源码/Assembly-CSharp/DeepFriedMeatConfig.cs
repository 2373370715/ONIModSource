using STRINGS;
using TUNING;
using UnityEngine;

public class DeepFriedMeatConfig : IEntityConfig
{
	public const string ID = "DeepFriedMeat";

	public static ComplexRecipe recipe;

	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_DLC_2;
	}

	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("DeepFriedMeat", ITEMS.FOOD.DEEPFRIEDMEAT.NAME, ITEMS.FOOD.DEEPFRIEDMEAT.DESC, 1f, unitMass: false, Assets.GetAnim("deepfried_meat_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, isPickupable: true), FOOD.FOOD_TYPES.DEEP_FRIED_MEAT);
	}

	public void OnPrefabInit(GameObject inst)
	{
	}

	public void OnSpawn(GameObject inst)
	{
	}
}
