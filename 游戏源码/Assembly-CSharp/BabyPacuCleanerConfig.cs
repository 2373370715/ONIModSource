using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200025C RID: 604
[EntityConfigOrder(2)]
public class BabyPacuCleanerConfig : IEntityConfig
{
	// Token: 0x060008B4 RID: 2228 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060008B5 RID: 2229 RVA: 0x000AA26C File Offset: 0x000A846C
	public GameObject CreatePrefab()
	{
		GameObject gameObject = PacuCleanerConfig.CreatePacu("PacuCleanerBaby", CREATURES.SPECIES.PACU.VARIANT_CLEANER.BABY.NAME, CREATURES.SPECIES.PACU.VARIANT_CLEANER.BABY.DESC, "baby_pacu_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "PacuCleaner", null, false, 5f);
		return gameObject;
	}

	// Token: 0x060008B6 RID: 2230 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060008B7 RID: 2231 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000687 RID: 1671
	public const string ID = "PacuCleanerBaby";
}
