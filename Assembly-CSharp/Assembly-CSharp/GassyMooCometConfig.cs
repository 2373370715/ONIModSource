﻿using STRINGS;
using UnityEngine;

public class GassyMooCometConfig : IEntityConfig {
    public static string   ID = "GassyMoo";
    public        string[] GetDlcIds() { return DlcManager.AVAILABLE_ALL_VERSIONS; }

    public GameObject CreatePrefab() {
        var gameObject = EntityTemplates.CreateEntity(ID, UI.SPACEDESTINATIONS.COMETS.GASSYMOOCOMET.NAME);
        gameObject.AddOrGet<SaveLoadRoot>();
        gameObject.AddOrGet<LoopingSounds>();
        var gassyMooComet = gameObject.AddOrGet<GassyMooComet>();
        gassyMooComet.massRange                = new Vector2(100f, 200f);
        gassyMooComet.EXHAUST_ELEMENT          = SimHashes.Methane;
        gassyMooComet.temperatureRange         = new Vector2(296.15f, 318.15f);
        gassyMooComet.entityDamage             = 0;
        gassyMooComet.explosionOreCount        = new Vector2I(0, 0);
        gassyMooComet.totalTileDamage          = 0f;
        gassyMooComet.splashRadius             = 1;
        gassyMooComet.impactSound              = "Meteor_GassyMoo_Impact";
        gassyMooComet.flyingSoundID            = 4;
        gassyMooComet.explosionEffectHash      = SpawnFXHashes.MeteorImpactDust;
        gassyMooComet.addTiles                 = 0;
        gassyMooComet.affectedByDifficulty     = false;
        gassyMooComet.lootOnDestroyedByMissile = new[] { "Meat", "Meat", "Meat" };
        gassyMooComet.destroyOnExplode         = false;
        gassyMooComet.craterPrefabs            = new[] { "Moo" };
        var primaryElement = gameObject.AddOrGet<PrimaryElement>();
        primaryElement.SetElement(SimHashes.Creature);
        primaryElement.Temperature = (gassyMooComet.temperatureRange.x + gassyMooComet.temperatureRange.y) / 2f;
        var kbatchedAnimController = gameObject.AddOrGet<KBatchedAnimController>();
        kbatchedAnimController.AnimFiles                = new[] { Assets.GetAnim("meteor_gassymoo_kanim") };
        kbatchedAnimController.isMovable                = true;
        kbatchedAnimController.initialAnim              = "fall_loop";
        kbatchedAnimController.initialMode              = KAnim.PlayMode.Loop;
        kbatchedAnimController.visibilityType           = KAnimControllerBase.VisibilityType.OffscreenUpdate;
        gameObject.AddOrGet<KCircleCollider2D>().radius = 0.5f;
        gameObject.AddTag(GameTags.Comet);
        return gameObject;
    }

    public void OnPrefabInit(GameObject go) { }
    public void OnSpawn(GameObject      go) { }
}