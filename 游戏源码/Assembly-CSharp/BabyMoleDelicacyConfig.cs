using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000252 RID: 594
[EntityConfigOrder(2)]
public class BabyMoleDelicacyConfig : IEntityConfig
{
	// Token: 0x06000877 RID: 2167 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000878 RID: 2168 RVA: 0x000AA079 File Offset: 0x000A8279
	public GameObject CreatePrefab()
	{
		GameObject gameObject = MoleDelicacyConfig.CreateMole("MoleDelicacyBaby", CREATURES.SPECIES.MOLE.VARIANT_DELICACY.BABY.NAME, CREATURES.SPECIES.MOLE.VARIANT_DELICACY.BABY.DESC, "baby_driller_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "MoleDelicacy", null, false, 5f);
		return gameObject;
	}

	// Token: 0x06000879 RID: 2169 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x0600087A RID: 2170 RVA: 0x000AA00B File Offset: 0x000A820B
	public void OnSpawn(GameObject inst)
	{
		MoleConfig.SetSpawnNavType(inst);
	}

	// Token: 0x04000654 RID: 1620
	public const string ID = "MoleDelicacyBaby";
}
