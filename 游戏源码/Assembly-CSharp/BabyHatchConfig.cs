using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000237 RID: 567
[EntityConfigOrder(2)]
public class BabyHatchConfig : IEntityConfig
{
	// Token: 0x060007D5 RID: 2005 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060007D6 RID: 2006 RVA: 0x000A9B85 File Offset: 0x000A7D85
	public GameObject CreatePrefab()
	{
		GameObject gameObject = HatchConfig.CreateHatch("HatchBaby", CREATURES.SPECIES.HATCH.BABY.NAME, CREATURES.SPECIES.HATCH.BABY.DESC, "baby_hatch_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "Hatch", null, false, 5f);
		return gameObject;
	}

	// Token: 0x060007D7 RID: 2007 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060007D8 RID: 2008 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040005E9 RID: 1513
	public const string ID = "HatchBaby";
}
