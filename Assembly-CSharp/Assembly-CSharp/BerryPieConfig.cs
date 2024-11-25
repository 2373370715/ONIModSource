using TUNING;
using UnityEngine;
using ITEMS = STRINGS.ITEMS;

public class BerryPieConfig : IEntityConfig {
    public const  string        ID = "BerryPie";
    public static ComplexRecipe recipe;
    public        string[]      GetDlcIds() { return DlcManager.AVAILABLE_EXPANSION1_ONLY; }

    public GameObject CreatePrefab() {
        return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("BerryPie",
                                                   ITEMS.FOOD.BERRYPIE.NAME,
                                                   ITEMS.FOOD.BERRYPIE.DESC,
                                                   1f,
                                                   false,
                                                   Assets.GetAnim("wormwood_berry_pie_kanim"),
                                                   "object",
                                                   Grid.SceneLayer.Front,
                                                   EntityTemplates.CollisionShape.RECTANGLE,
                                                   0.8f,
                                                   0.55f,
                                                   true),
                                                  FOOD.FOOD_TYPES.BERRY_PIE);
    }

    public void OnPrefabInit(GameObject inst) { }
    public void OnSpawn(GameObject      inst) { }
}