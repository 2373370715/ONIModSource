using UnityEngine;

public class WoodLogConfig : IOreConfig
{
	public const string ID = "WoodLog";

	public const float C02MassEmissionWhenBurned = 0.142f;

	public const float HeatWhenBurned = 7500f;

	public const float EnergyWhenBurned = 250f;

	public static readonly Tag TAG = TagManager.Create("WoodLog");

	public SimHashes ElementID => SimHashes.WoodLog;

	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateSolidOreEntity(ElementID);
		KPrefabID component = gameObject.GetComponent<KPrefabID>();
		component.prefabInitFn += OnInit;
		component.prefabSpawnFn += OnSpawn;
		component.RemoveTag(GameTags.HideFromSpawnTool);
		return gameObject;
	}

	public void OnInit(GameObject inst)
	{
		PrimaryElement component = inst.GetComponent<PrimaryElement>();
		component.SetElement(ElementID);
		_ = component.Element;
	}

	public void OnSpawn(GameObject inst)
	{
		inst.GetComponent<PrimaryElement>().SetElement(ElementID);
	}
}
