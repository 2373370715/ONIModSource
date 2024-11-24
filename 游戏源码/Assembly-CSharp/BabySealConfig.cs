using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200026E RID: 622
[EntityConfigOrder(2)]
public class BabySealConfig : IEntityConfig
{
	// Token: 0x06000918 RID: 2328 RVA: 0x000A9B1E File Offset: 0x000A7D1E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_DLC_2;
	}

	// Token: 0x06000919 RID: 2329 RVA: 0x000AA5AA File Offset: 0x000A87AA
	public GameObject CreatePrefab()
	{
		GameObject gameObject = SealConfig.CreateSeal("SealBaby", CREATURES.SPECIES.SEAL.BABY.NAME, CREATURES.SPECIES.SEAL.BABY.DESC, "baby_seal_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "Seal", null, false, 5f);
		return gameObject;
	}

	// Token: 0x0600091A RID: 2330 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x0600091B RID: 2331 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040006D4 RID: 1748
	public const string ID = "SealBaby";
}
