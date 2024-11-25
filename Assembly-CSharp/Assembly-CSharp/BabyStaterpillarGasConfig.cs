using STRINGS;
using UnityEngine;

[EntityConfigOrder(2)]
public class BabyStaterpillarGasConfig : IEntityConfig {
    public const string   ID = "StaterpillarGasBaby";
    public       string[] GetDlcIds() { return DlcManager.AVAILABLE_EXPANSION1_ONLY; }

    public GameObject CreatePrefab() {
        var gameObject = StaterpillarGasConfig.CreateStaterpillarGas("StaterpillarGasBaby",
                                                                     CREATURES.SPECIES.STATERPILLAR.VARIANT_GAS.BABY
                                                                              .NAME,
                                                                     CREATURES.SPECIES.STATERPILLAR.VARIANT_GAS.BABY
                                                                              .DESC,
                                                                     "baby_caterpillar_kanim",
                                                                     true);

        EntityTemplates.ExtendEntityToBeingABaby(gameObject, "StaterpillarGas");
        return gameObject;
    }

    public void OnPrefabInit(GameObject prefab) {
        prefab.GetComponent<KBatchedAnimController>().SetSymbolVisiblity("electric_bolt_c_bloom", false);
    }

    public void OnSpawn(GameObject inst) { }
}