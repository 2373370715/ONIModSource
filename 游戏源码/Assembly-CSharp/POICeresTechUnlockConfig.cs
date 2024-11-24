using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

public class POICeresTechUnlockConfig : IEntityConfig
{
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_DLC_2;
	}

	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreatePlacedEntity("POICeresTechUnlock", STRINGS.BUILDINGS.PREFABS.DLC2POITECHUNLOCKS.NAME, STRINGS.BUILDINGS.PREFABS.DLC2POITECHUNLOCKS.DESC, 100f, decor: TUNING.BUILDINGS.DECOR.BONUS.TIER0, noise: NOISE_POLLUTION.NOISY.TIER0, anim: Assets.GetAnim("research_unlock_kanim"), initialAnim: "on", sceneLayer: Grid.SceneLayer.Building, width: 3, height: 3, element: SimHashes.Creature, additionalTags: new List<Tag>
		{
			GameTags.Gravitas,
			RoomConstraints.ConstraintTags.LightSource,
			GameTags.RoomProberBuilding
		});
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Unobtanium);
		component.Temperature = 294.15f;
		gameObject.AddOrGet<OccupyArea>().objectLayers = new ObjectLayer[1] { ObjectLayer.Building };
		gameObject.AddOrGet<Demolishable>();
		gameObject.AddOrGet<POITechItemUnlockWorkable>().workTime = 5f;
		POITechItemUnlocks.Def def = gameObject.AddOrGetDef<POITechItemUnlocks.Def>();
		def.POITechUnlockIDs = new List<string> { "Campfire", "IceKettle", "WoodTile" };
		def.PopUpName = STRINGS.BUILDINGS.PREFABS.DLC2POITECHUNLOCKS.NAME;
		def.animName = "ceres_remote_archive_kanim";
		def.loreUnlockId = "notes_welcometoceres";
		Light2D light2D = gameObject.AddComponent<Light2D>();
		light2D.Color = LIGHT2D.POI_TECH_UNLOCK_COLOR;
		light2D.Range = 5f;
		light2D.Angle = 2.6f;
		light2D.Direction = LIGHT2D.POI_TECH_DIRECTION;
		light2D.Offset = LIGHT2D.POI_TECH_UNLOCK_OFFSET;
		light2D.overlayColour = LIGHT2D.POI_TECH_UNLOCK_OVERLAYCOLOR;
		light2D.shape = LightShape.Cone;
		light2D.drawOverlay = true;
		light2D.Lux = 1800;
		gameObject.AddOrGet<Prioritizable>();
		return gameObject;
	}

	public void OnPrefabInit(GameObject inst)
	{
	}

	public void OnSpawn(GameObject inst)
	{
	}
}
