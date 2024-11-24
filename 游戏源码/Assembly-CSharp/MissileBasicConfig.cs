using System.Collections.Generic;
using STRINGS;
using UnityEngine;

public class MissileBasicConfig : IEntityConfig
{
	public const string ID = "MissileBasic";

	public static ComplexRecipe recipe;

	public const float MASS_PER_MISSILE = 10f;

	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("MissileBasic", ITEMS.MISSILE_BASIC.NAME, ITEMS.MISSILE_BASIC.DESC, 10f, unitMass: true, Assets.GetAnim("missile_kanim"), "object", Grid.SceneLayer.BuildingBack, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, isPickupable: true, 0, SimHashes.Iron, new List<Tag>());
		gameObject.AddTag(GameTags.IndustrialProduct);
		gameObject.AddOrGetDef<MissileProjectile.Def>();
		gameObject.AddOrGet<EntitySplitter>().maxStackSize = 50f;
		return gameObject;
	}

	public void OnPrefabInit(GameObject inst)
	{
	}

	public void OnSpawn(GameObject inst)
	{
	}
}
