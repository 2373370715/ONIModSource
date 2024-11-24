using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000265 RID: 613
[EntityConfigOrder(2)]
public class BabyPuftBleachstoneConfig : IEntityConfig
{
	// Token: 0x060008E5 RID: 2277 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060008E6 RID: 2278 RVA: 0x000AA40B File Offset: 0x000A860B
	public GameObject CreatePrefab()
	{
		GameObject gameObject = PuftBleachstoneConfig.CreatePuftBleachstone("PuftBleachstoneBaby", CREATURES.SPECIES.PUFT.VARIANT_BLEACHSTONE.BABY.NAME, CREATURES.SPECIES.PUFT.VARIANT_BLEACHSTONE.BABY.DESC, "baby_puft_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "PuftBleachstone", null, false, 5f);
		return gameObject;
	}

	// Token: 0x060008E7 RID: 2279 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060008E8 RID: 2280 RVA: 0x000AA361 File Offset: 0x000A8561
	public void OnSpawn(GameObject inst)
	{
		BasePuftConfig.OnSpawn(inst);
	}

	// Token: 0x040006A8 RID: 1704
	public const string ID = "PuftBleachstoneBaby";
}
