using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

public class FossilBitsSmallConfig : IEntityConfig
{
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	public GameObject CreatePrefab()
	{
		GameObject obj = EntityTemplates.CreatePlacedEntity("FossilBitsSmall", CODEX.STORY_TRAITS.FOSSILHUNT.ENTITIES.FOSSIL_BITS.NAME, CODEX.STORY_TRAITS.FOSSILHUNT.ENTITIES.FOSSIL_BITS.DESC, 1500f, decor: TUNING.BUILDINGS.DECOR.BONUS.TIER0, noise: NOISE_POLLUTION.NOISY.TIER1, anim: Assets.GetAnim("fossil_bits1x2_kanim"), initialAnim: "object", sceneLayer: Grid.SceneLayer.BuildingBack, width: 1, height: 2, element: SimHashes.Creature, additionalTags: new List<Tag> { GameTags.Gravitas });
		PrimaryElement component = obj.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Fossil);
		component.Temperature = 315f;
		obj.AddOrGet<Operational>();
		obj.AddOrGet<EntombVulnerable>();
		obj.AddOrGet<FossilBits>();
		obj.AddOrGet<Prioritizable>();
		Prioritizable.AddRef(obj);
		obj.AddOrGet<LoopingSounds>();
		return obj;
	}

	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[1] { ObjectLayer.Building };
		EntombVulnerable component = inst.GetComponent<EntombVulnerable>();
		component.SetStatusItem(Db.Get().BuildingStatusItems.FossilEntombed);
		component.SetShowStatusItemOnEntombed(val: false);
	}

	public void OnSpawn(GameObject inst)
	{
	}
}
