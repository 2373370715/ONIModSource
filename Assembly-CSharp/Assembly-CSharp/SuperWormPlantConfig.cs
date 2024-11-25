using TUNING;
using UnityEngine;
using CREATURES = STRINGS.CREATURES;

public class SuperWormPlantConfig : IEntityConfig {
    public const           string         ID            = "SuperWormPlant";
    public const           string         SUPER_CROP_ID = "WormSuperFruit";
    public const           int            CROP_YIELD    = 8;
    public static readonly EffectorValues SUPER_DECOR   = DECOR.BONUS.TIER1;

    private static readonly StandardCropPlant.AnimSet animSet = new StandardCropPlant.AnimSet {
        grow      = "super_grow",
        grow_pst  = "super_grow_pst",
        idle_full = "super_idle_full",
        wilt_base = "super_wilt",
        harvest   = "super_harvest"
    };

    public string[] GetDlcIds() { return DlcManager.AVAILABLE_EXPANSION1_ONLY; }

    public GameObject CreatePrefab() {
        var gameObject = WormPlantConfig.BaseWormPlant("SuperWormPlant",
                                                       CREATURES.SPECIES.SUPERWORMPLANT.NAME,
                                                       CREATURES.SPECIES.SUPERWORMPLANT.DESC,
                                                       "wormwood_kanim",
                                                       SUPER_DECOR,
                                                       "WormSuperFruit");

        gameObject.AddOrGet<SeedProducer>().Configure("WormPlantSeed", SeedProducer.ProductionType.Harvest);
        return gameObject;
    }

    public void OnPrefabInit(GameObject prefab) {
        var transformingPlant = prefab.AddOrGet<TransformingPlant>();
        transformingPlant.SubscribeToTransformEvent(GameHashes.HarvestComplete);
        transformingPlant.transformPlantId = "WormPlant";
        prefab.GetComponent<KAnimControllerBase>().SetSymbolVisiblity("flower", false);
        prefab.AddOrGet<StandardCropPlant>().anims = animSet;
    }

    public void OnSpawn(GameObject inst) { }
}