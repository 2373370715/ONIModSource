using System.Collections.Generic;
using TUNING;
using UnityEngine;
using CREATURES = STRINGS.CREATURES;

public class CarrotPlantConfig : IEntityConfig {
    public const string   ID                       = "CarrotPlant";
    public const string   SEED_ID                  = "CarrotPlantSeed";
    public const float    Temperature_lethal_low   = 118.149994f;
    public const float    Temperature_warning_low  = 218.15f;
    public const float    Temperature_lethal_high  = 269.15f;
    public const float    Temperature_warning_high = 259.15f;
    public const float    FERTILIZATION_RATE       = 0.025f;
    public       string[] GetDlcIds() { return DlcManager.AVAILABLE_DLC_2; }

    public GameObject CreatePrefab() {
        var    id   = "CarrotPlant";
        string name = CREATURES.SPECIES.CARROTPLANT.NAME;
        string desc = CREATURES.SPECIES.CARROTPLANT.DESC;
        var    mass = 1f;
        var    tier = DECOR.PENALTY.TIER1;
        var gameObject = EntityTemplates.CreatePlacedEntity(id,
                                                            name,
                                                            desc,
                                                            mass,
                                                            Assets.GetAnim("purpleroot_kanim"),
                                                            "idle_empty",
                                                            Grid.SceneLayer.BuildingBack,
                                                            1,
                                                            2,
                                                            tier,
                                                            default(EffectorValues),
                                                            SimHashes.Creature,
                                                            null,
                                                            255f);

        var template                 = gameObject;
        var temperature_lethal_low   = 118.149994f;
        var temperature_warning_low  = 218.15f;
        var temperature_warning_high = 259.15f;
        var temperature_lethal_high  = 269.15f;
        var text                     = CarrotConfig.ID;
        EntityTemplates.ExtendEntityToBasicPlant(template,
                                                 temperature_lethal_low,
                                                 temperature_warning_low,
                                                 temperature_warning_high,
                                                 temperature_lethal_high,
                                                 new[] {
                                                     SimHashes.Oxygen,
                                                     SimHashes.ContaminatedOxygen,
                                                     SimHashes.CarbonDioxide
                                                 },
                                                 true,
                                                 0f,
                                                 0.15f,
                                                 text,
                                                 true,
                                                 true,
                                                 true,
                                                 true,
                                                 2400f,
                                                 0f,
                                                 4600f,
                                                 "CarrotPlantOriginal",
                                                 CREATURES.SPECIES.CARROTPLANT.NAME);

        gameObject.AddOrGet<StandardCropPlant>();
        gameObject.AddOrGet<DirectlyEdiblePlant_Growth>();
        gameObject.AddOrGet<LoopingSounds>();
        var    plant          = gameObject;
        var    productionType = SeedProducer.ProductionType.Harvest;
        var    id2            = "CarrotPlantSeed";
        string name2          = CREATURES.SPECIES.SEEDS.CARROTPLANT.NAME;
        string desc2          = CREATURES.SPECIES.SEEDS.CARROTPLANT.DESC;
        var    anim           = Assets.GetAnim("seed_purpleroot_kanim");
        var    initialAnim    = "object";
        var    numberOfSeeds  = 1;
        var    list           = new List<Tag>();
        list.Add(GameTags.CropSeed);
        var planterDirection = SingleEntityReceptacle.ReceptacleDirection.Top;
        text = CREATURES.SPECIES.CARROTPLANT.DOMESTICATEDDESC;
        var dlcIds = GetDlcIds();
        var seed = EntityTemplates.CreateAndRegisterSeedForPlant(plant,
                                                                 productionType,
                                                                 id2,
                                                                 name2,
                                                                 desc2,
                                                                 anim,
                                                                 initialAnim,
                                                                 numberOfSeeds,
                                                                 list,
                                                                 planterDirection,
                                                                 default(Tag),
                                                                 1,
                                                                 text,
                                                                 EntityTemplates.CollisionShape.CIRCLE,
                                                                 0.3f,
                                                                 0.3f,
                                                                 null,
                                                                 "",
                                                                 false,
                                                                 dlcIds);

        EntityTemplates.ExtendPlantToIrrigated(gameObject,
                                               new[] {
                                                   new PlantElementAbsorber.ConsumeInfo {
                                                       tag = SimHashes.Ethanol.CreateTag(), massConsumptionRate = 0.025f
                                                   }
                                               });

        EntityTemplates.CreateAndRegisterPreviewForPlant(seed,
                                                         "CarrotPlant_preview",
                                                         Assets.GetAnim("purpleroot_kanim"),
                                                         "place",
                                                         1,
                                                         2);

        SoundEventVolumeCache.instance.AddVolume("bristleblossom_kanim",
                                                 "PrickleFlower_harvest",
                                                 NOISE_POLLUTION.CREATURES.TIER3);

        SoundEventVolumeCache.instance.AddVolume("bristleblossom_kanim",
                                                 "PrickleFlower_grow",
                                                 NOISE_POLLUTION.CREATURES.TIER3);

        return gameObject;
    }

    public void OnPrefabInit(GameObject prefab) { }
    public void OnSpawn(GameObject      inst)   { }
}