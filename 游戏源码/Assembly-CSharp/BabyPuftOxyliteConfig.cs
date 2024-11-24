using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000269 RID: 617
[EntityConfigOrder(2)]
public class BabyPuftOxyliteConfig : IEntityConfig
{
	// Token: 0x060008FD RID: 2301 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060008FE RID: 2302 RVA: 0x000AA4E9 File Offset: 0x000A86E9
	public GameObject CreatePrefab()
	{
		GameObject gameObject = PuftOxyliteConfig.CreatePuftOxylite("PuftOxyliteBaby", CREATURES.SPECIES.PUFT.VARIANT_OXYLITE.BABY.NAME, CREATURES.SPECIES.PUFT.VARIANT_OXYLITE.BABY.DESC, "baby_puft_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "PuftOxylite", null, false, 5f);
		return gameObject;
	}

	// Token: 0x060008FF RID: 2303 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000900 RID: 2304 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040006BE RID: 1726
	public const string ID = "PuftOxyliteBaby";
}
