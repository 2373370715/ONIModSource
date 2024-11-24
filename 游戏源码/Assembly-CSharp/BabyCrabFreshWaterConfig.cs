using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000226 RID: 550
[EntityConfigOrder(2)]
public class BabyCrabFreshWaterConfig : IEntityConfig
{
	// Token: 0x06000776 RID: 1910 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000777 RID: 1911 RVA: 0x000A9979 File Offset: 0x000A7B79
	public GameObject CreatePrefab()
	{
		GameObject gameObject = CrabFreshWaterConfig.CreateCrabFreshWater("CrabFreshWaterBaby", CREATURES.SPECIES.CRAB.VARIANT_FRESH_WATER.BABY.NAME, CREATURES.SPECIES.CRAB.VARIANT_FRESH_WATER.BABY.DESC, "baby_pincher_kanim", true, null);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "CrabFreshWater", null, false, 5f);
		return gameObject;
	}

	// Token: 0x06000778 RID: 1912 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000779 RID: 1913 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000589 RID: 1417
	public const string ID = "CrabFreshWaterBaby";
}
