using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000267 RID: 615
[EntityConfigOrder(2)]
public class BabyPuftConfig : IEntityConfig
{
	// Token: 0x060008F1 RID: 2289 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060008F2 RID: 2290 RVA: 0x000AA479 File Offset: 0x000A8679
	public GameObject CreatePrefab()
	{
		GameObject gameObject = PuftConfig.CreatePuft("PuftBaby", CREATURES.SPECIES.PUFT.BABY.NAME, CREATURES.SPECIES.PUFT.BABY.DESC, "baby_puft_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "Puft", null, false, 5f);
		return gameObject;
	}

	// Token: 0x060008F3 RID: 2291 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060008F4 RID: 2292 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040006B4 RID: 1716
	public const string ID = "PuftBaby";
}
