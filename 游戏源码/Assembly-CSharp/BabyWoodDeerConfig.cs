using Klei.AI;
using STRINGS;
using UnityEngine;

[EntityConfigOrder(2)]
public class BabyWoodDeerConfig : IEntityConfig
{
	public const string ID = "WoodDeerBaby";

	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_DLC_2;
	}

	public GameObject CreatePrefab()
	{
		GameObject gameObject = WoodDeerConfig.CreateWoodDeer("WoodDeerBaby", CREATURES.SPECIES.WOODDEER.BABY.NAME, CREATURES.SPECIES.WOODDEER.BABY.DESC, "baby_ice_floof_kanim", is_baby: true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "WoodDeer").AddOrGetDef<BabyMonitor.Def>().configureAdultOnMaturation = delegate(GameObject go)
		{
			AmountInstance amountInstance = Db.Get().Amounts.ScaleGrowth.Lookup(go);
			amountInstance.value = amountInstance.GetMax() * WoodDeerConfig.ANTLER_STARTING_GROWTH_PCT;
		};
		return gameObject;
	}

	public void OnPrefabInit(GameObject prefab)
	{
	}

	public void OnSpawn(GameObject inst)
	{
	}
}
