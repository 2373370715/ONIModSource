using HarmonyLib;
using UnityEngine;

#if 脱盐器
[HarmonyPatch]
public class 脱盐器 {
    [HarmonyPatch(typeof(DesalinatorConfig), "ConfigureBuildingTemplate"), HarmonyPostfix]
    public static void IO质量(GameObject go) {
        var conduitConsumer = go.AddOrGet<ConduitConsumer>();
        conduitConsumer.consumptionRate = 30f;
        conduitConsumer.capacityKG      = 60f;

        var elementConverter = go.AddComponent<ElementConverter>();
        elementConverter.consumedElements = new[] { new ElementConverter.ConsumedElement(new Tag("SaltWater"), 10f) };

        elementConverter.outputElements = new[] {
            new ElementConverter.OutputElement(9.3f,
                                               SimHashes.Water,
                                               0f,
                                               false,
                                               true,
                                               0f,
                                               0.5f,
                                               0.75f),
            new ElementConverter.OutputElement(0.7f,
                                               SimHashes.Salt,
                                               0f,
                                               false,
                                               true,
                                               0f,
                                               0.5f,
                                               0.25f)
        };

        var elementConverter2 = go.AddComponent<ElementConverter>();
        elementConverter2.consumedElements = new[] { new ElementConverter.ConsumedElement(new Tag("Brine"), 10f) };
        elementConverter2.outputElements = new[] {
            new ElementConverter.OutputElement(7f,
                                               SimHashes.Water,
                                               0f,
                                               false,
                                               true,
                                               0f,
                                               0.5f,
                                               0.75f),
            new ElementConverter.OutputElement(3f,
                                               SimHashes.Salt,
                                               0f,
                                               false,
                                               true,
                                               0f,
                                               0.5f,
                                               0.25f)
        };
    }

    [HarmonyPatch(typeof(Desalinator.StatesInstance), "UpdateStorageLeft"), HarmonyPostfix]
    public static void 需要操作(Desalinator.StatesInstance __instance) {
        if (__instance.master.SaltStorageLeft < 900f) {
            var value = Traverse.Create(__instance.master).Field("storage").GetValue<Storage>();
            __instance.emptyChore = null;
            var tag        = GameTagExtensions.Create(SimHashes.Salt);
            var pooledList = ListPool<GameObject, Desalinator>.Allocate();
            value.Find(tag, pooledList);
            foreach (var go in pooledList) value.Drop(go);

            pooledList.Recycle();
        }
    }
}
#endif