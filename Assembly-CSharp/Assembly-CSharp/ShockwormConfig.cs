using TUNING;
using UnityEngine;
using CREATURES = STRINGS.CREATURES;

public class ShockwormConfig : IEntityConfig {
    public const string   ID = "ShockWorm";
    public       string[] GetDlcIds() { return DlcManager.AVAILABLE_ALL_VERSIONS; }

    public GameObject CreatePrefab() {
        var    id   = "ShockWorm";
        string name = CREATURES.SPECIES.SHOCKWORM.NAME;
        string desc = CREATURES.SPECIES.SHOCKWORM.DESC;
        var    mass = 50f;
        var    tier = DECOR.BONUS.TIER0;
        var gameObject = EntityTemplates.CreatePlacedEntity(id,
                                                            name,
                                                            desc,
                                                            mass,
                                                            Assets.GetAnim("shockworm_kanim"),
                                                            "idle",
                                                            Grid.SceneLayer.Creatures,
                                                            1,
                                                            2,
                                                            tier);

        var    faction            = FactionManager.FactionID.Hostile;
        string initialTraitID     = null;
        var    navGridName        = "FlyerNavGrid1x2";
        var    navType            = NavType.Hover;
        var    max_probing_radius = 32;
        var    moveSpeed          = 2f;
        var    onDeathDropID      = "Meat";
        var    onDeathDropCount   = 3;
        var    drownVulnerable    = true;
        var    entombVulnerable   = true;
        var    freezing_          = TUNING.CREATURES.TEMPERATURE.FREEZING_2;
        EntityTemplates.ExtendEntityToBasicCreature(gameObject,
                                                    faction,
                                                    initialTraitID,
                                                    navGridName,
                                                    navType,
                                                    max_probing_radius,
                                                    moveSpeed,
                                                    onDeathDropID,
                                                    onDeathDropCount,
                                                    drownVulnerable,
                                                    entombVulnerable,
                                                    TUNING.CREATURES.TEMPERATURE.FREEZING_1,
                                                    TUNING.CREATURES.TEMPERATURE.HOT_1,
                                                    freezing_,
                                                    TUNING.CREATURES.TEMPERATURE.HOT_2);

        gameObject.AddOrGet<LoopingSounds>();
        gameObject.AddWeapon(3f,
                             6f,
                             AttackProperties.DamageType.Standard,
                             AttackProperties.TargetType.AreaOfEffect,
                             10,
                             4f)
                  .AddEffect();

        SoundEventVolumeCache.instance.AddVolume("shockworm_kanim",
                                                 "Shockworm_attack_arc",
                                                 NOISE_POLLUTION.CREATURES.TIER6);

        return gameObject;
    }

    public void OnPrefabInit(GameObject prefab) { }
    public void OnSpawn(GameObject      inst)   { }
}