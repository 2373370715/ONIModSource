using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200022E RID: 558
[EntityConfigOrder(2)]
public class BabyDreckoConfig : IEntityConfig
{
	// Token: 0x060007A6 RID: 1958 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060007A7 RID: 1959 RVA: 0x000A9A96 File Offset: 0x000A7C96
	public GameObject CreatePrefab()
	{
		GameObject gameObject = DreckoConfig.CreateDrecko("DreckoBaby", CREATURES.SPECIES.DRECKO.BABY.NAME, CREATURES.SPECIES.DRECKO.BABY.DESC, "baby_drecko_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "Drecko", null, false, 5f);
		return gameObject;
	}

	// Token: 0x060007A8 RID: 1960 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060007A9 RID: 1961 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040005BA RID: 1466
	public const string ID = "DreckoBaby";
}
