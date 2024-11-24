using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000106 RID: 262
[EntityConfigOrder(2)]
public class BabyBeeConfig : IEntityConfig
{
	// Token: 0x0600040D RID: 1037 RVA: 0x000A6566 File Offset: 0x000A4766
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x0600040E RID: 1038 RVA: 0x0015544C File Offset: 0x0015364C
	public GameObject CreatePrefab()
	{
		GameObject gameObject = BeeConfig.CreateBee("BeeBaby", CREATURES.SPECIES.BEE.BABY.NAME, CREATURES.SPECIES.BEE.BABY.DESC, "baby_blarva_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "Bee", null, true, 2f);
		gameObject.GetComponent<KPrefabID>().AddTag(GameTags.Creatures.Walker, false);
		return gameObject;
	}

	// Token: 0x0600040F RID: 1039 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000410 RID: 1040 RVA: 0x000A73D9 File Offset: 0x000A55D9
	public void OnSpawn(GameObject inst)
	{
		BaseBeeConfig.SetupLoopingSounds(inst);
	}

	// Token: 0x040002E4 RID: 740
	public const string ID = "BeeBaby";
}
