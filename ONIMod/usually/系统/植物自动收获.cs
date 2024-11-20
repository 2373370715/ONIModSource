using HarmonyLib;
using UnityEngine;
#if 植物自动收获
[HarmonyPatch(typeof(EntityTemplates), "ExtendEntityToBasicPlant")]
public class 植物自动收获 {
    //植物自动掉落
    public static void Prefix(ref GameObject template, ref float max_age) {
        var name = template.PrefabID().Name;
        max_age = 3f;
    }
}
#endif