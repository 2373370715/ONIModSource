using STRINGS;
using UnityEngine;

[EntityConfigOrder(2)]
public class BabyPuftBleachstoneConfig : IEntityConfig {
    public const string   ID = "PuftBleachstoneBaby";
    public       string[] GetDlcIds() { return DlcManager.AVAILABLE_ALL_VERSIONS; }

    public GameObject CreatePrefab() {
        var gameObject = PuftBleachstoneConfig.CreatePuftBleachstone("PuftBleachstoneBaby",
                                                                     CREATURES.SPECIES.PUFT.VARIANT_BLEACHSTONE.BABY
                                                                              .NAME,
                                                                     CREATURES.SPECIES.PUFT.VARIANT_BLEACHSTONE.BABY
                                                                              .DESC,
                                                                     "baby_puft_kanim",
                                                                     true);

        EntityTemplates.ExtendEntityToBeingABaby(gameObject, "PuftBleachstone");
        return gameObject;
    }

    public void OnPrefabInit(GameObject prefab) { }
    public void OnSpawn(GameObject      inst)   { BasePuftConfig.OnSpawn(inst); }
}