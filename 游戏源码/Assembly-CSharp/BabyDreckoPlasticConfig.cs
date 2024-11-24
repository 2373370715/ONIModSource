using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000230 RID: 560
[EntityConfigOrder(2)]
public class BabyDreckoPlasticConfig : IEntityConfig
{
	// Token: 0x060007B2 RID: 1970 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060007B3 RID: 1971 RVA: 0x000A9AD4 File Offset: 0x000A7CD4
	public GameObject CreatePrefab()
	{
		GameObject gameObject = DreckoPlasticConfig.CreateDrecko("DreckoPlasticBaby", CREATURES.SPECIES.DRECKO.VARIANT_PLASTIC.BABY.NAME, CREATURES.SPECIES.DRECKO.VARIANT_PLASTIC.BABY.DESC, "baby_drecko_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "DreckoPlastic", null, false, 5f);
		return gameObject;
	}

	// Token: 0x060007B4 RID: 1972 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060007B5 RID: 1973 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040005C8 RID: 1480
	public const string ID = "DreckoPlasticBaby";
}
