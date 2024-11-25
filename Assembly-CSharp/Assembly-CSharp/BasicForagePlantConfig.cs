using TUNING;
using UnityEngine;
using ITEMS = STRINGS.ITEMS;

public class BasicForagePlantConfig : IEntityConfig {
    public const string   ID = "BasicForagePlant";
    public       string[] GetDlcIds() { return DlcManager.AVAILABLE_ALL_VERSIONS; }

    public GameObject CreatePrefab() {
        return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("BasicForagePlant",
                                                   ITEMS.FOOD.BASICFORAGEPLANT.NAME,
                                                   ITEMS.FOOD.BASICFORAGEPLANT.DESC,
                                                   1f,
                                                   false,
                                                   Assets.GetAnim("muckrootvegetable_kanim"),
                                                   "object",
                                                   Grid.SceneLayer.BuildingBack,
                                                   EntityTemplates.CollisionShape.CIRCLE,
                                                   0.3f,
                                                   0.3f,
                                                   true),
                                                  FOOD.FOOD_TYPES.BASICFORAGEPLANT);
    }

    public void OnPrefabInit(GameObject inst) { }
    public void OnSpawn(GameObject      inst) { }
}