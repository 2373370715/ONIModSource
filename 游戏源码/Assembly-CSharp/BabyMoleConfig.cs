using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000250 RID: 592
[EntityConfigOrder(2)]
public class BabyMoleConfig : IEntityConfig
{
	// Token: 0x0600086A RID: 2154 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x0600086B RID: 2155 RVA: 0x000AA033 File Offset: 0x000A8233
	public GameObject CreatePrefab()
	{
		GameObject gameObject = MoleConfig.CreateMole("MoleBaby", CREATURES.SPECIES.MOLE.BABY.NAME, CREATURES.SPECIES.MOLE.BABY.DESC, "baby_driller_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "Mole", null, false, 5f);
		return gameObject;
	}

	// Token: 0x0600086C RID: 2156 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x0600086D RID: 2157 RVA: 0x000AA00B File Offset: 0x000A820B
	public void OnSpawn(GameObject inst)
	{
		MoleConfig.SetSpawnNavType(inst);
	}

	// Token: 0x04000646 RID: 1606
	public const string ID = "MoleBaby";
}
