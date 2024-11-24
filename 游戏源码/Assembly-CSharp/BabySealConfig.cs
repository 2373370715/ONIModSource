using STRINGS;
using UnityEngine;

[EntityConfigOrder(2)]
public class BabySealConfig : IEntityConfig
{
	public const string ID = "SealBaby";

	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_DLC_2;
	}

	public GameObject CreatePrefab()
	{
		GameObject gameObject = SealConfig.CreateSeal("SealBaby", CREATURES.SPECIES.SEAL.BABY.NAME, CREATURES.SPECIES.SEAL.BABY.DESC, "baby_seal_kanim", is_baby: true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "Seal");
		return gameObject;
	}

	public void OnPrefabInit(GameObject prefab)
	{
	}

	public void OnSpawn(GameObject inst)
	{
	}
}
