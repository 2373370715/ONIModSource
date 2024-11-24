using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200023B RID: 571
[EntityConfigOrder(2)]
public class BabyHatchMetalConfig : IEntityConfig
{
	// Token: 0x060007EE RID: 2030 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060007EF RID: 2031 RVA: 0x000A9C65 File Offset: 0x000A7E65
	public GameObject CreatePrefab()
	{
		GameObject gameObject = HatchMetalConfig.CreateHatch("HatchMetalBaby", CREATURES.SPECIES.HATCH.VARIANT_METAL.BABY.NAME, CREATURES.SPECIES.HATCH.VARIANT_METAL.BABY.DESC, "baby_hatch_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "HatchMetal", null, false, 5f);
		return gameObject;
	}

	// Token: 0x060007F0 RID: 2032 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060007F1 RID: 2033 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040005FA RID: 1530
	public const string ID = "HatchMetalBaby";
}
