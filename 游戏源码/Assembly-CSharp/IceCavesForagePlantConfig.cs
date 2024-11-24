using STRINGS;
using TUNING;
using UnityEngine;

public class IceCavesForagePlantConfig : IEntityConfig
{
	public const string ID = "IceCavesForagePlant";

	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_DLC_2;
	}

	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("IceCavesForagePlant", ITEMS.FOOD.ICECAVESFORAGEPLANT.NAME, ITEMS.FOOD.ICECAVESFORAGEPLANT.DESC, 1f, unitMass: false, Assets.GetAnim("frozenberries_fruit_kanim"), "object", Grid.SceneLayer.BuildingBack, EntityTemplates.CollisionShape.CIRCLE, 0.3f, 0.3f, isPickupable: true), FOOD.FOOD_TYPES.ICECAVESFORAGEPLANT);
	}

	public void OnPrefabInit(GameObject inst)
	{
	}

	public void OnSpawn(GameObject inst)
	{
	}
}
