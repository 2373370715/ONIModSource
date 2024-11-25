using System.Collections.Generic;
using TUNING;
using UnityEngine;
using ITEMS = STRINGS.ITEMS;

public class GarbageElectrobankConfig : IEntityConfig {
    public const string ID   = "GarbageElectrobank";
    public const float  MASS = 20f;

    public GameObject CreatePrefab() {
        var gameObject = EntityTemplates.CreateLooseEntity("GarbageElectrobank",
                                                           ITEMS.INDUSTRIAL_PRODUCTS.ELECTROBANK_GARBAGE.NAME,
                                                           ITEMS.INDUSTRIAL_PRODUCTS.ELECTROBANK_GARBAGE.DESC,
                                                           20f,
                                                           true,
                                                           Assets.GetAnim("electrobank_large_destroyed_kanim"),
                                                           "idle1",
                                                           Grid.SceneLayer.Ore,
                                                           EntityTemplates.CollisionShape.RECTANGLE,
                                                           0.5f,
                                                           0.8f,
                                                           true,
                                                           0,
                                                           SimHashes.Aluminum,
                                                           new List<Tag> { GameTags.PedestalDisplayable });

        gameObject.GetComponent<KCollider2D>();
        gameObject.AddTag(GameTags.IndustrialProduct);
        gameObject.AddOrGet<OccupyArea>().SetCellOffsets(EntityTemplates.GenerateOffsets(1, 1));
        gameObject.AddOrGet<DecorProvider>().SetValues(DECOR.PENALTY.TIER0);
        return gameObject;
    }

    public string[] GetDlcIds()                   { return DlcManager.DLC3; }
    public void     OnPrefabInit(GameObject inst) { }
    public void     OnSpawn(GameObject      inst) { }
}