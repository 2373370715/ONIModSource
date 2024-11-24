using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000279 RID: 633
[EntityConfigOrder(2)]
public class BabyStaterpillarLiquidConfig : IEntityConfig
{
	// Token: 0x06000959 RID: 2393 RVA: 0x000A6566 File Offset: 0x000A4766
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x0600095A RID: 2394 RVA: 0x000AA7C1 File Offset: 0x000A89C1
	public GameObject CreatePrefab()
	{
		GameObject gameObject = StaterpillarLiquidConfig.CreateStaterpillarLiquid("StaterpillarLiquidBaby", CREATURES.SPECIES.STATERPILLAR.VARIANT_LIQUID.BABY.NAME, CREATURES.SPECIES.STATERPILLAR.VARIANT_LIQUID.BABY.DESC, "baby_caterpillar_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "StaterpillarLiquid", null, false, 5f);
		return gameObject;
	}

	// Token: 0x0600095B RID: 2395 RVA: 0x000AA7A9 File Offset: 0x000A89A9
	public void OnPrefabInit(GameObject prefab)
	{
		prefab.GetComponent<KBatchedAnimController>().SetSymbolVisiblity("electric_bolt_c_bloom", false);
	}

	// Token: 0x0600095C RID: 2396 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400070C RID: 1804
	public const string ID = "StaterpillarLiquidBaby";
}
