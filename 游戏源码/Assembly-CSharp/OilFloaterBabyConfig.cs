using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000256 RID: 598
[EntityConfigOrder(2)]
public class OilFloaterBabyConfig : IEntityConfig
{
	// Token: 0x06000890 RID: 2192 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000891 RID: 2193 RVA: 0x000AA14C File Offset: 0x000A834C
	public GameObject CreatePrefab()
	{
		GameObject gameObject = OilFloaterConfig.CreateOilFloater("OilfloaterBaby", CREATURES.SPECIES.OILFLOATER.BABY.NAME, CREATURES.SPECIES.OILFLOATER.BABY.DESC, "baby_oilfloater_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "Oilfloater", null, false, 5f);
		return gameObject;
	}

	// Token: 0x06000892 RID: 2194 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000893 RID: 2195 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400066C RID: 1644
	public const string ID = "OilfloaterBaby";
}
