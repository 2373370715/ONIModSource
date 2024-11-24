using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200025F RID: 607
[EntityConfigOrder(2)]
public class BabyPacuConfig : IEntityConfig
{
	// Token: 0x060008C1 RID: 2241 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060008C2 RID: 2242 RVA: 0x000AA2C0 File Offset: 0x000A84C0
	public GameObject CreatePrefab()
	{
		GameObject gameObject = PacuConfig.CreatePacu("PacuBaby", CREATURES.SPECIES.PACU.BABY.NAME, CREATURES.SPECIES.PACU.BABY.DESC, "baby_pacu_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "Pacu", null, false, 5f);
		return gameObject;
	}

	// Token: 0x060008C3 RID: 2243 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060008C4 RID: 2244 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400068C RID: 1676
	public const string ID = "PacuBaby";
}
