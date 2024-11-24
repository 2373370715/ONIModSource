using STRINGS;
using TUNING;
using UnityEngine;

public class FriesCarrotConfig : IEntityConfig
{
	public const string ID = "FriesCarrot";

	public static ComplexRecipe recipe;

	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_DLC_2;
	}

	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("FriesCarrot", ITEMS.FOOD.FRIESCARROT.NAME, ITEMS.FOOD.FRIESCARROT.DESC, 1f, unitMass: false, Assets.GetAnim("rootfries_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.6f, isPickupable: true), FOOD.FOOD_TYPES.FRIES_CARROT);
	}

	public void OnPrefabInit(GameObject inst)
	{
	}

	public void OnSpawn(GameObject inst)
	{
	}
}
