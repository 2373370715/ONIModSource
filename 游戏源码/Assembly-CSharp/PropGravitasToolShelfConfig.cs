using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

public class PropGravitasToolShelfConfig : IEntityConfig
{
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	public GameObject CreatePrefab()
	{
		GameObject obj = EntityTemplates.CreatePlacedEntity("PropGravitasToolShelf", STRINGS.BUILDINGS.PREFABS.PROPGRAVITASTOOLSHELF.NAME, STRINGS.BUILDINGS.PREFABS.PROPGRAVITASTOOLSHELF.DESC, 50f, decor: TUNING.BUILDINGS.DECOR.BONUS.TIER0, noise: NOISE_POLLUTION.NOISY.TIER0, anim: Assets.GetAnim("poi_toolshelf_kanim"), initialAnim: "off", sceneLayer: Grid.SceneLayer.Building, width: 2, height: 2, element: SimHashes.Creature, additionalTags: new List<Tag> { GameTags.Gravitas });
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
