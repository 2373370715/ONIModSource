using STRINGS;
using TUNING;
using UnityEngine;

public class DeepFriedFishConfig : IEntityConfig
{
	public const string ID = "DeepFriedFish";

	public static ComplexRecipe recipe;

	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_DLC_2;
	}

	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("DeepFriedFish", ITEMS.FOOD.DEEPFRIEDFISH.NAME, ITEMS.FOOD.DEEPFRIEDFISH.DESC, 1f, unitMass: false, Assets.GetAnim("deepfried_fish_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, isPickupable: true), FOOD.FOOD_TYPES.DEEP_FRIED_FISH);
	}

	public void OnPrefabInit(GameObject inst)
	{
	}

	public void OnSpawn(GameObject inst)
	{
	}
}
