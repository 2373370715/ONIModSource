using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000228 RID: 552
[EntityConfigOrder(2)]
public class BabyCrabWoodConfig : IEntityConfig
{
	// Token: 0x06000782 RID: 1922 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000783 RID: 1923 RVA: 0x0015FEC4 File Offset: 0x0015E0C4
	public GameObject CreatePrefab()
	{
		GameObject gameObject = CrabWoodConfig.CreateCrabWood("CrabWoodBaby", CREATURES.SPECIES.CRAB.VARIANT_WOOD.BABY.NAME, CREATURES.SPECIES.CRAB.VARIANT_WOOD.BABY.DESC, "baby_pincher_kanim", true, "BabyCrabWoodShell");
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "CrabWood", "BabyCrabWoodShell", false, 5f);
		return gameObject;
	}

	// Token: 0x06000784 RID: 1924 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000785 RID: 1925 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000593 RID: 1427
	public const string ID = "CrabWoodBaby";
}
