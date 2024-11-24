using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000271 RID: 625
[EntityConfigOrder(2)]
public class BabySquirrelConfig : IEntityConfig
{
	// Token: 0x06000929 RID: 2345 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x0600092A RID: 2346 RVA: 0x000AA61E File Offset: 0x000A881E
	public GameObject CreatePrefab()
	{
		GameObject gameObject = SquirrelConfig.CreateSquirrel("SquirrelBaby", CREATURES.SPECIES.SQUIRREL.BABY.NAME, CREATURES.SPECIES.SQUIRREL.BABY.DESC, "baby_squirrel_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "Squirrel", null, false, 5f);
		return gameObject;
	}

	// Token: 0x0600092B RID: 2347 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x0600092C RID: 2348 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040006E1 RID: 1761
	public const string ID = "SquirrelBaby";
}
