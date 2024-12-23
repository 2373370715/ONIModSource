﻿using TUNING;
using UnityEngine;
using ITEMS = STRINGS.ITEMS;

public class DehydratedSurfAndTurfConfig : IEntityConfig {
    public const  float    MASS                     = 1f;
    public const  int      FABRICATION_TIME_SECONDS = 300;
    public const  string   ANIM_FILE                = "dehydrated_food_surf_and_turf_kanim";
    public const  string   INITIAL_ANIM             = "idle";
    public static Tag      ID                       = new Tag("DehydratedSurfAndTurf");
    public        string[] GetDlcIds()                   { return DlcManager.AVAILABLE_ALL_VERSIONS; }
    public        void     OnPrefabInit(GameObject inst) { }
    public        void     OnSpawn(GameObject      inst) { }

    public GameObject CreatePrefab() {
        var anim = Assets.GetAnim("dehydrated_food_surf_and_turf_kanim");
        var gameObject = EntityTemplates.CreateLooseEntity(ID.Name,
                                                           ITEMS.FOOD.SURFANDTURF.DEHYDRATED.NAME,
                                                           ITEMS.FOOD.SURFANDTURF.DEHYDRATED.DESC,
                                                           1f,
                                                           true,
                                                           anim,
                                                           "idle",
                                                           Grid.SceneLayer.BuildingFront,
                                                           EntityTemplates.CollisionShape.RECTANGLE,
                                                           0.6f,
                                                           0.7f,
                                                           true,
                                                           0,
                                                           SimHashes.Polypropylene);

        EntityTemplates.ExtendEntityToDehydratedFoodPackage(gameObject, FOOD.FOOD_TYPES.SURF_AND_TURF);
        return gameObject;
    }
}