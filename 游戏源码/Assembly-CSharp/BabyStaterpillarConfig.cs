using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000275 RID: 629
[EntityConfigOrder(2)]
public class BabyStaterpillarConfig : IEntityConfig
{
	// Token: 0x06000941 RID: 2369 RVA: 0x000A6566 File Offset: 0x000A4766
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000942 RID: 2370 RVA: 0x000AA704 File Offset: 0x000A8904
	public GameObject CreatePrefab()
	{
		GameObject gameObject = StaterpillarConfig.CreateStaterpillar("StaterpillarBaby", CREATURES.SPECIES.STATERPILLAR.BABY.NAME, CREATURES.SPECIES.STATERPILLAR.BABY.DESC, "baby_caterpillar_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "Staterpillar", null, false, 5f);
		return gameObject;
	}

	// Token: 0x06000943 RID: 2371 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000944 RID: 2372 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040006F4 RID: 1780
	public const string ID = "StaterpillarBaby";
}
