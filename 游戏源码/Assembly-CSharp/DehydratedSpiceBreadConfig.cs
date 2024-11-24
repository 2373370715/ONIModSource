using STRINGS;
using TUNING;
using UnityEngine;

public class DehydratedSpiceBreadConfig : IEntityConfig
{
	public static Tag ID = new Tag("DehydratedSpiceBread");

	public const float MASS = 1f;

	public const string ANIM_FILE = "dehydrated_food_spicebread_kanim";

	public const string INITIAL_ANIM = "idle";

	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	public void OnPrefabInit(GameObject inst)
	{
	}

	public void OnSpawn(GameObject inst)
	{
	}

	public GameObject CreatePrefab()
	{
		KAnimFile anim = Assets.GetAnim("dehydrated_food_spicebread_kanim");
		GameObject gameObject = EntityTemplates.CreateLooseEntity(ID.Name, ITEMS.FOOD.SPICEBREAD.DEHYDRATED.NAME, ITEMS.FOOD.SPICEBREAD.DEHYDRATED.DESC, 1f, unitMass: true, anim, "idle", Grid.SceneLayer.BuildingFront, EntityTemplates.CollisionShape.RECTANGLE, 0.6f, 0.7f, isPickupable: true, 0, SimHashes.Polypropylene);
		EntityTemplates.ExtendEntityToDehydratedFoodPackage(gameObject, FOOD.FOOD_TYPES.SPICEBREAD);
		return gameObject;
	}
}
