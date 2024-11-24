using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000261 RID: 609
[EntityConfigOrder(2)]
public class BabyPacuTropicalConfig : IEntityConfig
{
	// Token: 0x060008CD RID: 2253 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060008CE RID: 2254 RVA: 0x000AA30A File Offset: 0x000A850A
	public GameObject CreatePrefab()
	{
		GameObject gameObject = PacuTropicalConfig.CreatePacu("PacuTropicalBaby", CREATURES.SPECIES.PACU.VARIANT_TROPICAL.BABY.NAME, CREATURES.SPECIES.PACU.VARIANT_TROPICAL.BABY.DESC, "baby_pacu_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "PacuTropical", null, false, 5f);
		return gameObject;
	}

	// Token: 0x060008CF RID: 2255 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060008D0 RID: 2256 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000692 RID: 1682
	public const string ID = "PacuTropicalBaby";
}
