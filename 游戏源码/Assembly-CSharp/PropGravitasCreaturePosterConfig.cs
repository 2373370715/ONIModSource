using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000513 RID: 1299
public class PropGravitasCreaturePosterConfig : IEntityConfig
{
	// Token: 0x060016E5 RID: 5861 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060016E6 RID: 5862 RVA: 0x00198E74 File Offset: 0x00197074
	public GameObject CreatePrefab()
	{
		string id = "PropGravitasCreaturePoster";
		string name = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASCREATUREPOSTER.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASCREATUREPOSTER.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_poster_kanim"), "off", Grid.SceneLayer.Building, 2, 2, tier, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Granite, true);
		component.Temperature = 294.15f;
		LoreBearerUtil.AddLoreTo(gameObject, LoreBearerUtil.UnlockSpecificEntry("storytrait_crittermanipulator_workiversary", UI.USERMENUACTIONS.READLORE.SEARCH_PROPGRAVITASCREATUREPOSTER));
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

	// Token: 0x060016E7 RID: 5863 RVA: 0x000A656D File Offset: 0x000A476D
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x060016E8 RID: 5864 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}
}
