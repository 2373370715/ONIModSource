using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000573 RID: 1395
public class POICeresTechUnlockConfig : IEntityConfig
{
	// Token: 0x060018B1 RID: 6321 RVA: 0x000A9B1E File Offset: 0x000A7D1E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_DLC_2;
	}

	// Token: 0x060018B2 RID: 6322 RVA: 0x001A0708 File Offset: 0x0019E908
	public GameObject CreatePrefab()
	{
		string id = "POICeresTechUnlock";
		string name = STRINGS.BUILDINGS.PREFABS.DLC2POITECHUNLOCKS.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.DLC2POITECHUNLOCKS.DESC;
		float mass = 100f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("research_unlock_kanim"), "on", Grid.SceneLayer.Building, 3, 3, tier, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas,
			RoomConstraints.ConstraintTags.LightSource,
			GameTags.RoomProberBuilding
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Unobtanium, true);
		component.Temperature = 294.15f;
		gameObject.AddOrGet<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
		gameObject.AddOrGet<Demolishable>();
		gameObject.AddOrGet<POITechItemUnlockWorkable>().workTime = 5f;
		POITechItemUnlocks.Def def = gameObject.AddOrGetDef<POITechItemUnlocks.Def>();
		def.POITechUnlockIDs = new List<string>
		{
			"Campfire",
			"IceKettle",
			"WoodTile"
		};
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
		light2D.shape = global::LightShape.Cone;
		light2D.drawOverlay = true;
		light2D.Lux = 1800;
		gameObject.AddOrGet<Prioritizable>();
		return gameObject;
	}

	// Token: 0x060018B3 RID: 6323 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060018B4 RID: 6324 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}
}
