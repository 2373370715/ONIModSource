using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000263 RID: 611
[EntityConfigOrder(2)]
public class BabyPuftAlphaConfig : IEntityConfig
{
	// Token: 0x060008D9 RID: 2265 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060008DA RID: 2266 RVA: 0x000AA39B File Offset: 0x000A859B
	public GameObject CreatePrefab()
	{
		GameObject gameObject = PuftAlphaConfig.CreatePuftAlpha("PuftAlphaBaby", CREATURES.SPECIES.PUFT.VARIANT_ALPHA.BABY.NAME, CREATURES.SPECIES.PUFT.VARIANT_ALPHA.BABY.DESC, "baby_puft_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "PuftAlpha", null, false, 5f);
		return gameObject;
	}

	// Token: 0x060008DB RID: 2267 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060008DC RID: 2268 RVA: 0x000AA361 File Offset: 0x000A8561
	public void OnSpawn(GameObject inst)
	{
		BasePuftConfig.OnSpawn(inst);
	}

	// Token: 0x0400069E RID: 1694
	public const string ID = "PuftAlphaBaby";
}
