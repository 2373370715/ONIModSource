﻿using System.Collections.Generic;
using Klei.AI;
using TUNING;
using UnityEngine;
using ITEMS = STRINGS.ITEMS;

public class BasicFabricConfig : IEntityConfig {
    public static string ID = "BasicFabric";

    private readonly AttributeModifier decorModifier
        = new AttributeModifier("Decor", 0.1f, ITEMS.INDUSTRIAL_PRODUCTS.BASIC_FABRIC.NAME, true);

    public string[] GetDlcIds() { return DlcManager.AVAILABLE_ALL_VERSIONS; }

    public GameObject CreatePrefab() {
        var gameObject = EntityTemplates.CreateLooseEntity(ID,
                                                           ITEMS.INDUSTRIAL_PRODUCTS.BASIC_FABRIC.NAME,
                                                           ITEMS.INDUSTRIAL_PRODUCTS.BASIC_FABRIC.DESC,
                                                           1f,
                                                           true,
                                                           Assets.GetAnim("swampreedwool_kanim"),
                                                           "object",
                                                           Grid.SceneLayer.BuildingBack,
                                                           EntityTemplates.CollisionShape.RECTANGLE,
                                                           0.8f,
                                                           0.45f,
                                                           true,
                                                           SORTORDER.BUILDINGELEMENTS + BasicFabricTuning.SORTORDER,
                                                           SimHashes.Creature,
                                                           new List<Tag> {
                                                               GameTags.IndustrialIngredient, GameTags.BuildingFiber
                                                           });

        gameObject.AddOrGet<EntitySplitter>();
        gameObject.AddOrGet<PrefabAttributeModifiers>().AddAttributeDescriptor(decorModifier);
        return gameObject;
    }

    public void OnPrefabInit(GameObject inst) { }
    public void OnSpawn(GameObject      inst) { }
}