using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

public class PropHumanMurphyBedConfig : IEntityConfig
{
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_DLC_2;
	}

	public GameObject CreatePrefab()
	{
		GameObject obj = EntityTemplates.CreatePlacedEntity("PropHumanMurphyBed", STRINGS.BUILDINGS.PREFABS.PROPHUMANMURPHYBED.NAME, STRINGS.BUILDINGS.PREFABS.PROPHUMANMURPHYBED.DESC, 50f, decor: TUNING.BUILDINGS.DECOR.BONUS.TIER0, noise: NOISE_POLLUTION.NOISY.TIER0, anim: Assets.GetAnim("poi_murphybed_kanim"), initialAnim: "on", sceneLayer: Grid.SceneLayer.Building, width: 5, height: 4, element: SimHashes.Creature, additionalTags: new List<Tag> { GameTags.Gravitas });
		PrimaryElement component = obj.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Granite);
		component.Temperature = 294.15f;
		obj.AddOrGet<Demolishable>();
		return obj;
	}

	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[1] { ObjectLayer.Building };
	}

	public void OnSpawn(GameObject inst)
	{
	}
}
