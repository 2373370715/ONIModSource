using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200057A RID: 1402
public class PropHumanMurphyBedConfig : IEntityConfig
{
	// Token: 0x060018D4 RID: 6356 RVA: 0x000A9B1E File Offset: 0x000A7D1E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_DLC_2;
	}

	// Token: 0x060018D5 RID: 6357 RVA: 0x001A0DC8 File Offset: 0x0019EFC8
	public GameObject CreatePrefab()
	{
		string id = "PropHumanMurphyBed";
		string name = STRINGS.BUILDINGS.PREFABS.PROPHUMANMURPHYBED.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPHUMANMURPHYBED.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("poi_murphybed_kanim"), "on", Grid.SceneLayer.Building, 5, 4, tier, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Granite, true);
		component.Temperature = 294.15f;
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

	// Token: 0x060018D6 RID: 6358 RVA: 0x000A656D File Offset: 0x000A476D
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x060018D7 RID: 6359 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}
}
