using STRINGS;
using TUNING;
using UnityEngine;

public class QuicheConfig : IEntityConfig
{
	public const string ID = "Quiche";

	public static ComplexRecipe recipe;

	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("Quiche", ITEMS.FOOD.QUICHE.NAME, ITEMS.FOOD.QUICHE.DESC, 1f, unitMass: false, Assets.GetAnim("quiche_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, isPickupable: true), FOOD.FOOD_TYPES.QUICHE);
	}

	public void OnPrefabInit(GameObject inst)
	{
	}

	public void OnSpawn(GameObject inst)
	{
	}
}
