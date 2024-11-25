using TUNING;
using UnityEngine;
using ITEMS = STRINGS.ITEMS;

public class TofuConfig : IEntityConfig {
    public const  string        ID = "Tofu";
    public static ComplexRecipe recipe;
    public        string[]      GetDlcIds() { return DlcManager.AVAILABLE_ALL_VERSIONS; }

    public GameObject CreatePrefab() {
        var gameObject = EntityTemplates.CreateLooseEntity("Tofu",
                                                           ITEMS.FOOD.TOFU.NAME,
                                                           ITEMS.FOOD.TOFU.DESC,
                                                           1f,
                                                           false,
                                                           Assets.GetAnim("loafu_kanim"),
                                                           "object",
                                                           Grid.SceneLayer.Front,
                                                           EntityTemplates.CollisionShape.RECTANGLE,
                                                           0.9f,
                                                           0.6f,
                                                           true);

        gameObject = EntityTemplates.ExtendEntityToFood(gameObject, FOOD.FOOD_TYPES.TOFU);
        ComplexRecipeManager.Get().GetRecipe(recipe.id).FabricationVisualizer
            = MushBarConfig.CreateFabricationVisualizer(gameObject);

        return gameObject;
    }

    public void OnPrefabInit(GameObject inst) { }
    public void OnSpawn(GameObject      inst) { }
}