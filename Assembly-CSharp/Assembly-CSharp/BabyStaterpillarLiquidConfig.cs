using STRINGS;
using UnityEngine;

[EntityConfigOrder(2)]
public class BabyStaterpillarLiquidConfig : IEntityConfig {
    public const string   ID = "StaterpillarLiquidBaby";
    public       string[] GetDlcIds() { return DlcManager.AVAILABLE_EXPANSION1_ONLY; }

    public GameObject CreatePrefab() {
        var gameObject = StaterpillarLiquidConfig.CreateStaterpillarLiquid("StaterpillarLiquidBaby",
                                                                           CREATURES.SPECIES.STATERPILLAR.VARIANT_LIQUID
                                                                               .BABY.NAME,
                                                                           CREATURES.SPECIES.STATERPILLAR.VARIANT_LIQUID
                                                                               .BABY.DESC,
                                                                           "baby_caterpillar_kanim",
                                                                           true);

        EntityTemplates.ExtendEntityToBeingABaby(gameObject, "StaterpillarLiquid");
        return gameObject;
    }

    public void OnPrefabInit(GameObject prefab) {
        prefab.GetComponent<KBatchedAnimController>().SetSymbolVisiblity("electric_bolt_c_bloom", false);
    }

    public void OnSpawn(GameObject inst) { }
}