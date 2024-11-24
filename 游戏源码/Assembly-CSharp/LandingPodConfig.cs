using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020003B2 RID: 946
public class LandingPodConfig : IEntityConfig
{
	// Token: 0x06000FA9 RID: 4009 RVA: 0x000A6566 File Offset: 0x000A4766
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000FAA RID: 4010 RVA: 0x0017DA50 File Offset: 0x0017BC50
	public GameObject CreatePrefab()
	{
		string id = "LandingPod";
		string name = STRINGS.BUILDINGS.PREFABS.LANDING_POD.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.LANDING_POD.DESC;
		float mass = 2000f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("rocket_puft_pod_kanim"), "grounded", Grid.SceneLayer.Building, 3, 3, tier, tier2, SimHashes.Creature, null, 293f);
		gameObject.AddOrGet<PodLander>();
		gameObject.AddOrGet<MinionStorage>();
		return gameObject;
	}

	// Token: 0x06000FAB RID: 4011 RVA: 0x000A656D File Offset: 0x000A476D
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x06000FAC RID: 4012 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000B2C RID: 2860
	public const string ID = "LandingPod";
}
