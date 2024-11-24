using STRINGS;
using TUNING;
using UnityEngine;

public class DeepFriedNoshConfig : IEntityConfig
{
	public const string ID = "DeepFriedNosh";

	public static ComplexRecipe recipe;

	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_DLC_2;
	}

	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("DeepFriedNosh", ITEMS.FOOD.DEEPFRIEDNOSH.NAME, ITEMS.FOOD.DEEPFRIEDNOSH.DESC, 1f, unitMass: false, Assets.GetAnim("deepfried_nosh_beans_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, isPickupable: true), FOOD.FOOD_TYPES.DEEP_FRIED_NOSH);
	}

	public void OnPrefabInit(GameObject inst)
	{
	}

	public void OnSpawn(GameObject inst)
	{
	}
}
