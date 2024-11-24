using STRINGS;
using UnityEngine;

public class AlgaeCometConfig : IEntityConfig
{
	public static string ID = "AlgaeComet";

	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	public GameObject CreatePrefab()
	{
		GameObject gameObject = BaseCometConfig.BaseComet(ID, UI.SPACEDESTINATIONS.COMETS.ALGAECOMET.NAME, "meteor_algae_kanim", SimHashes.Algae, new Vector2(3f, 20f), new Vector2(310.15f, 323.15f), "Meteor_algae_Impact", 7, SimHashes.Void, SpawnFXHashes.MeteorImpactAlgae, 0.3f);
		Comet component = gameObject.GetComponent<Comet>();
		component.explosionOreCount = new Vector2I(2, 4);
		component.explosionSpeedRange = new Vector2(4f, 7f);
		component.entityDamage = 0;
		component.totalTileDamage = 0f;
		return gameObject;
	}

	public void OnPrefabInit(GameObject go)
	{
	}

	public void OnSpawn(GameObject go)
	{
	}
}
