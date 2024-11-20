using HarmonyLib;
using UnityEngine;
#if 存储箱
[HarmonyPatch]
public class 存储箱 {
    [HarmonyPatch(typeof(StorageLockerConfig), nameof(StorageLockerConfig.ConfigureBuildingTemplate)),HarmonyPostfix]
    public static void Postfix(GameObject go, Tag prefab_tag) { go.AddOrGet<Storage>().capacityKg = 建筑.基地.存储箱.容量; }
}
#endif