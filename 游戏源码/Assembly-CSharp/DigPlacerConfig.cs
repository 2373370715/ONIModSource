using System;
using STRINGS;
using UnityEngine;

// Token: 0x020004EB RID: 1259
public class DigPlacerConfig : CommonPlacerConfig, IEntityConfig
{
	// Token: 0x06001630 RID: 5680 RVA: 0x00196F08 File Offset: 0x00195108
	public GameObject CreatePrefab()
	{
		GameObject gameObject = base.CreatePrefab(DigPlacerConfig.ID, MISC.PLACERS.DIGPLACER.NAME, Assets.instance.digPlacerAssets.materials[0]);
		Diggable diggable = gameObject.AddOrGet<Diggable>();
		diggable.workTime = 5f;
		diggable.synchronizeAnims = false;
		diggable.workAnims = new HashedString[]
		{
			"place",
			"release"
		};
		diggable.materials = Assets.instance.digPlacerAssets.materials;
		diggable.materialDisplay = gameObject.GetComponentInChildren<MeshRenderer>(true);
		gameObject.AddOrGet<CancellableDig>();
		return gameObject;
	}

	// Token: 0x06001631 RID: 5681 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06001632 RID: 5682 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000EFA RID: 3834
	public static string ID = "DigPlacer";

	// Token: 0x020004EC RID: 1260
	[Serializable]
	public class DigPlacerAssets
	{
		// Token: 0x04000EFB RID: 3835
		public Material[] materials;
	}
}
