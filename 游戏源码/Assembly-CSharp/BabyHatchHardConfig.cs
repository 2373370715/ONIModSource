using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000239 RID: 569
[EntityConfigOrder(2)]
public class BabyHatchHardConfig : IEntityConfig
{
	// Token: 0x060007E1 RID: 2017 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060007E2 RID: 2018 RVA: 0x000A9BF5 File Offset: 0x000A7DF5
	public GameObject CreatePrefab()
	{
		GameObject gameObject = HatchHardConfig.CreateHatch("HatchHardBaby", CREATURES.SPECIES.HATCH.VARIANT_HARD.BABY.NAME, CREATURES.SPECIES.HATCH.VARIANT_HARD.BABY.DESC, "baby_hatch_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "HatchHard", null, false, 5f);
		return gameObject;
	}

	// Token: 0x060007E3 RID: 2019 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060007E4 RID: 2020 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040005F2 RID: 1522
	public const string ID = "HatchHardBaby";
}
