using System.Collections.Generic;
using TUNING;
using UnityEngine;
using CREATURES = STRINGS.CREATURES;

public class BasicFabricMaterialPlantConfig : IEntityConfig {
    public const  float    WATER_RATE = 0.26666668f;
    public static string   ID         = "BasicFabricPlant";
    public static string   SEED_ID    = "BasicFabricMaterialPlantSeed";
    public        string[] GetDlcIds() { return DlcManager.AVAILABLE_ALL_VERSIONS; }

    public GameObject CreatePrefab() {
        var    id   = ID;
        string name = CREATURES.SPECIES.BASICFABRICMATERIALPLANT.NAME;
        string desc = CREATURES.SPECIES.BASICFABRICMATERIALPLANT.DESC;
        var    mass = 1f;
        var    tier = DECOR.BONUS.TIER0;
        var gameObject = EntityTemplates.CreatePlacedEntity(id,
                                                            name,
                                                            desc,
                                                            mass,
                                                            Assets.GetAnim("swampreed_kanim"),
                                                            "idle_empty",
                                                            Grid.SceneLayer.BuildingBack,
                                                            1,
                                                            3,
                                                            tier);

        var template                 = gameObject;
        var temperature_lethal_low   = 248.15f;
        var temperature_warning_low  = 295.15f;
        var temperature_warning_high = 310.15f;
        var temperature_lethal_high  = 398.15f;
        var text                     = BasicFabricConfig.ID;
        EntityTemplates.ExtendEntityToBasicPlant(template,
                                                 temperature_lethal_low,
                                                 temperature_warning_low,
                                                 temperature_warning_high,
                                                 temperature_lethal_high,
                                                 new[] {
                                                     SimHashes.Oxygen,
                                                     SimHashes.ContaminatedOxygen,
                                                     SimHashes.CarbonDioxide,
                                                     SimHashes.DirtyWater,
                                                     SimHashes.Water
                                                 },
                                                 false,
                                                 0f,
                                                 0.15f,
                                                 text,
                                                 false,
                                                 true,
                                                 true,
                                                 true,
                                                 2400f,
                                                 0f,
                                                 4600f,
                                                 ID + "Original",
                                                 CREATURES.SPECIES.BASICFABRICMATERIALPLANT.NAME);

        EntityTemplates.ExtendPlantToIrrigated(gameObject,
                                               new[] {
                                                   new PlantElementAbsorber.ConsumeInfo {
                                                       tag = GameTags.DirtyWater, massConsumptionRate = 0.26666668f
                                                   }
                                               });

        gameObject.AddOrGet<StandardCropPlant>();
        gameObject.AddOrGet<DirectlyEdiblePlant_Growth>();
        gameObject.AddOrGet<LoopingSounds>();
        var    plant          = gameObject;
        var    productionType = SeedProducer.ProductionType.Harvest;
        var    seed_ID        = SEED_ID;
        string name2          = CREATURES.SPECIES.SEEDS.BASICFABRICMATERIALPLANT.NAME;
        string desc2          = CREATURES.SPECIES.SEEDS.BASICFABRICMATERIALPLANT.DESC;
        var    anim           = Assets.GetAnim("seed_swampreed_kanim");
        var    initialAnim    = "object";
        var    numberOfSeeds  = 1;
        var    list           = new List<Tag>();
        list.Add(GameTags.WaterSeed);
        var planterDirection = SingleEntityReceptacle.ReceptacleDirection.Top;
        text = CREATURES.SPECIES.BASICFABRICMATERIALPLANT.DOMESTICATEDDESC;
        EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(plant,
                                                          productionType,
                                                          seed_ID,
                                                          name2,
                                                          desc2,
                                                          anim,
                                                          initialAnim,
                                                          numberOfSeeds,
                                                          list,
                                                          planterDirection,
                                                          default(Tag),
                                                          20,
                                                          text),
                                                         ID + "_preview",
                                                         Assets.GetAnim("swampreed_kanim"),
                                                         "place",
                                                         1,
                                                         3);

        SoundEventVolumeCache.instance.AddVolume("swampreed_kanim",
                                                 "FabricPlant_grow",
                                                 NOISE_POLLUTION.CREATURES.TIER3);

        SoundEventVolumeCache.instance.AddVolume("swampreed_kanim",
                                                 "FabricPlant_harvest",
                                                 NOISE_POLLUTION.CREATURES.TIER3);

        return gameObject;
    }

    public void OnPrefabInit(GameObject inst) { }
    public void OnSpawn(GameObject      inst) { }
}