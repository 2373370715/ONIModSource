using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002DA RID: 730
public class SuperWormPlantConfig : IEntityConfig
{
	// Token: 0x06000B63 RID: 2915 RVA: 0x000A6566 File Offset: 0x000A4766
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000B64 RID: 2916 RVA: 0x0016EEF4 File Offset: 0x0016D0F4
	public GameObject CreatePrefab()
	{
		GameObject gameObject = WormPlantConfig.BaseWormPlant("SuperWormPlant", STRINGS.CREATURES.SPECIES.SUPERWORMPLANT.NAME, STRINGS.CREATURES.SPECIES.SUPERWORMPLANT.DESC, "wormwood_kanim", SuperWormPlantConfig.SUPER_DECOR, "WormSuperFruit");
		gameObject.AddOrGet<SeedProducer>().Configure("WormPlantSeed", SeedProducer.ProductionType.Harvest, 1);
		return gameObject;
	}

	// Token: 0x06000B65 RID: 2917 RVA: 0x0016EF40 File Offset: 0x0016D140
	public void OnPrefabInit(GameObject prefab)
	{
		TransformingPlant transformingPlant = prefab.AddOrGet<TransformingPlant>();
		transformingPlant.SubscribeToTransformEvent(GameHashes.HarvestComplete);
		transformingPlant.transformPlantId = "WormPlant";
		prefab.GetComponent<KAnimControllerBase>().SetSymbolVisiblity("flower", false);
		prefab.AddOrGet<StandardCropPlant>().anims = SuperWormPlantConfig.animSet;
	}

	// Token: 0x06000B66 RID: 2918 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040008C7 RID: 2247
	public const string ID = "SuperWormPlant";

	// Token: 0x040008C8 RID: 2248
	public static readonly EffectorValues SUPER_DECOR = DECOR.BONUS.TIER1;

	// Token: 0x040008C9 RID: 2249
	public const string SUPER_CROP_ID = "WormSuperFruit";

	// Token: 0x040008CA RID: 2250
	public const int CROP_YIELD = 8;

	// Token: 0x040008CB RID: 2251
	private static StandardCropPlant.AnimSet animSet = new StandardCropPlant.AnimSet
	{
		grow = "super_grow",
		grow_pst = "super_grow_pst",
		idle_full = "super_idle_full",
		wilt_base = "super_wilt",
		harvest = "super_harvest"
	};
}
