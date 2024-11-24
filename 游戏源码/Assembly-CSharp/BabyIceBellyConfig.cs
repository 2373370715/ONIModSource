using System;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x0200023F RID: 575
[EntityConfigOrder(2)]
public class BabyIceBellyConfig : IEntityConfig
{
	// Token: 0x06000806 RID: 2054 RVA: 0x000A9B1E File Offset: 0x000A7D1E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_DLC_2;
	}

	// Token: 0x06000807 RID: 2055 RVA: 0x00161A54 File Offset: 0x0015FC54
	public GameObject CreatePrefab()
	{
		GameObject gameObject = IceBellyConfig.CreateIceBelly("IceBellyBaby", CREATURES.SPECIES.ICEBELLY.BABY.NAME, CREATURES.SPECIES.ICEBELLY.BABY.DESC, "baby_icebelly_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "IceBelly", null, false, 5f).AddOrGetDef<BabyMonitor.Def>().configureAdultOnMaturation = delegate(GameObject go)
		{
			AmountInstance amountInstance = Db.Get().Amounts.ScaleGrowth.Lookup(go);
			amountInstance.value = amountInstance.GetMax() * IceBellyConfig.SCALE_INITIAL_GROWTH_PCT;
		};
		return gameObject;
	}

	// Token: 0x06000808 RID: 2056 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000809 RID: 2057 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400060C RID: 1548
	public const string ID = "IceBellyBaby";
}
