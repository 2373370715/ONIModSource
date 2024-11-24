using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

public class PropCeresPosterLargeConfig : IEntityConfig
{
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_DLC_2;
	}

	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreatePlacedEntity("PropCeresPosterLarge", STRINGS.BUILDINGS.PREFABS.PROPCERESPOSTERLARGE.NAME, STRINGS.BUILDINGS.PREFABS.PROPCERESPOSTERLARGE.DESC, 50f, decor: TUNING.BUILDINGS.DECOR.BONUS.TIER0, noise: NOISE_POLLUTION.NOISY.TIER0, anim: Assets.GetAnim("poster_ceres_7x5_kanim"), initialAnim: "art_7x5", sceneLayer: Grid.SceneLayer.Building, width: 5, height: 7, permittedRotation: PermittedRotations.R90, orientation: Orientation.Neutral, element: SimHashes.Creature, additionalTags: new List<Tag> { GameTags.Gravitas });
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Unobtanium);
		component.Temperature = 294.15f;
		gameObject.AddOrGet<OccupyArea>().objectLayers = new ObjectLayer[1] { ObjectLayer.Building };
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

	public void OnPrefabInit(GameObject inst)
	{
	}

	public void OnSpawn(GameObject inst)
	{
	}
}
