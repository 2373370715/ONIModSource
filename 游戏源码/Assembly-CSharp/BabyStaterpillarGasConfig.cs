using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000277 RID: 631
[EntityConfigOrder(2)]
public class BabyStaterpillarGasConfig : IEntityConfig
{
	// Token: 0x0600094D RID: 2381 RVA: 0x000A6566 File Offset: 0x000A4766
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x0600094E RID: 2382 RVA: 0x000AA76B File Offset: 0x000A896B
	public GameObject CreatePrefab()
	{
		GameObject gameObject = StaterpillarGasConfig.CreateStaterpillarGas("StaterpillarGasBaby", CREATURES.SPECIES.STATERPILLAR.VARIANT_GAS.BABY.NAME, CREATURES.SPECIES.STATERPILLAR.VARIANT_GAS.BABY.DESC, "baby_caterpillar_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "StaterpillarGas", null, false, 5f);
		return gameObject;
	}

	// Token: 0x0600094F RID: 2383 RVA: 0x000AA7A9 File Offset: 0x000A89A9
	public void OnPrefabInit(GameObject prefab)
	{
		prefab.GetComponent<KBatchedAnimController>().SetSymbolVisiblity("electric_bolt_c_bloom", false);
	}

	// Token: 0x06000950 RID: 2384 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000700 RID: 1792
	public const string ID = "StaterpillarGasBaby";
}
