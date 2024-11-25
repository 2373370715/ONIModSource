using TUNING;
using UnityEngine;
using BUILDINGS = TUNING.BUILDINGS;
using CREATURES = STRINGS.CREATURES;

public class ChlorineGeyserConfig : IEntityConfig {
    public const string   ID = "ChlorineGeyser";
    public       string[] GetDlcIds() { return DlcManager.AVAILABLE_ALL_VERSIONS; }

    public GameObject CreatePrefab() {
        var    id    = "ChlorineGeyser";
        string name  = CREATURES.SPECIES.CHLORINEGEYSER.NAME;
        string desc  = CREATURES.SPECIES.CHLORINEGEYSER.DESC;
        var    mass  = 2000f;
        var    tier  = BUILDINGS.DECOR.BONUS.TIER1;
        var    tier2 = NOISE_POLLUTION.NOISY.TIER5;
        var gameObject = EntityTemplates.CreatePlacedEntity(id,
                                                            name,
                                                            desc,
                                                            mass,
                                                            Assets.GetAnim("geyser_side_chlorine_kanim"),
                                                            "inactive",
                                                            Grid.SceneLayer.BuildingBack,
                                                            4,
                                                            2,
                                                            tier,
                                                            tier2);

        gameObject.GetComponent<KPrefabID>().AddTag(GameTags.DeprecatedContent);
        var component = gameObject.GetComponent<PrimaryElement>();
        component.SetElement(SimHashes.IgneousRock);
        component.Temperature                      = 372.15f;
        gameObject.AddOrGet<Geyser>().outputOffset = new Vector2I(0, 1);
        var geyserConfigurator = gameObject.AddOrGet<GeyserConfigurator>();
        geyserConfigurator.presetType = "chlorine_gas";
        geyserConfigurator.presetMin  = 0.35f;
        geyserConfigurator.presetMax  = 0.65f;
        var studyable = gameObject.AddOrGet<Studyable>();
        studyable.meterTrackerSymbol = "geotracker_target";
        studyable.meterAnim          = "tracker";
        gameObject.AddOrGet<LoopingSounds>();
        SoundEventVolumeCache.instance.AddVolume("geyser_methane_kanim",
                                                 "GeyserMethane_shake_LP",
                                                 NOISE_POLLUTION.NOISY.TIER5);

        SoundEventVolumeCache.instance.AddVolume("geyser_methane_kanim",
                                                 "GeyserMethane_shake_LP",
                                                 NOISE_POLLUTION.NOISY.TIER6);

        return gameObject;
    }

    public void OnPrefabInit(GameObject inst) { }
    public void OnSpawn(GameObject      inst) { }
}