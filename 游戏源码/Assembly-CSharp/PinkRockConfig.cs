using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000487 RID: 1159
public class PinkRockConfig : IEntityConfig
{
	// Token: 0x0600145D RID: 5213 RVA: 0x000A9B1E File Offset: 0x000A7D1E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_DLC_2;
	}

	// Token: 0x0600145E RID: 5214 RVA: 0x00190AD0 File Offset: 0x0018ECD0
	public GameObject CreatePrefab()
	{
		string id = this.ID;
		string name = STRINGS.CREATURES.SPECIES.PINKROCK.NAME;
		string desc = STRINGS.CREATURES.SPECIES.PINKROCK.DESC;
		float mass = 25f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("pinkrock_kanim"), "idle", Grid.SceneLayer.Building, 1, 1, tier, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Experimental
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Unobtanium, true);
		component.Temperature = 235.15f;
		gameObject.AddOrGet<Carvable>().dropItemPrefabId = "PinkRockCarved";
		gameObject.AddOrGet<Prioritizable>();
		gameObject.AddOrGet<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
		Light2D light2D = gameObject.AddOrGet<Light2D>();
		light2D.overlayColour = LIGHT2D.PINKROCK_COLOR;
		light2D.Color = LIGHT2D.PINKROCK_COLOR;
		light2D.Range = 2f;
		light2D.Angle = 0f;
		light2D.Direction = LIGHT2D.PINKROCK_DIRECTION;
		light2D.Offset = LIGHT2D.PINKROCK_OFFSET;
		light2D.shape = global::LightShape.Circle;
		light2D.drawOverlay = true;
		return gameObject;
	}

	// Token: 0x0600145F RID: 5215 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06001460 RID: 5216 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000DB1 RID: 3505
	public string ID = "PinkRock";
}
