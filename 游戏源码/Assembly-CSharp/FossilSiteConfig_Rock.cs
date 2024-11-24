using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

public class FossilSiteConfig_Rock : IEntityConfig
{
	public static readonly HashedString FossilQuestCriteriaID = "LostRockFossil";

	public const string ID = "FossilRock";

	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	public GameObject CreatePrefab()
	{
		GameObject obj = EntityTemplates.CreatePlacedEntity("FossilRock", CODEX.STORY_TRAITS.FOSSILHUNT.ENTITIES.FOSSIL_ROCK.NAME, CODEX.STORY_TRAITS.FOSSILHUNT.ENTITIES.FOSSIL_ROCK.DESC, 4000f, decor: TUNING.BUILDINGS.DECOR.BONUS.TIER4, noise: NOISE_POLLUTION.NOISY.TIER3, anim: Assets.GetAnim("fossil_rock_kanim"), initialAnim: "object", sceneLayer: Grid.SceneLayer.BuildingBack, width: 2, height: 2, element: SimHashes.Creature, additionalTags: new List<Tag> { GameTags.Gravitas });
		PrimaryElement component = obj.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Fossil);
		component.Temperature = 315f;
		obj.AddOrGet<Operational>();
		obj.AddOrGet<EntombVulnerable>();
		obj.AddOrGet<Demolishable>().allowDemolition = false;
		obj.AddOrGetDef<MinorFossilDigSite.Def>().fossilQuestCriteriaID = FossilQuestCriteriaID;
		obj.AddOrGetDef<FossilHuntInitializer.Def>();
		obj.AddOrGet<MinorDigSiteWorkable>();
		obj.AddOrGet<Prioritizable>();
		Prioritizable.AddRef(obj);
		obj.AddOrGet<LoopingSounds>();
		return obj;
	}

	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<EntombVulnerable>().SetStatusItem(Db.Get().BuildingStatusItems.FossilEntombed);
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[1] { ObjectLayer.Building };
	}

	public void OnSpawn(GameObject inst)
	{
	}
}
