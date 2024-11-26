using HarmonyLib;
using UnityEngine;

#if usually
[HarmonyPatch]
public class 地堡门 {
    [HarmonyPatch(typeof(BunkerDoorConfig), nameof(BunkerDoorConfig.DoPostConfigureComplete)), HarmonyPostfix]
    public static void postfix1(GameObject go) {
        go.AddOrGet<Door>().poweredAnimSpeed = 100f;
    }
}

#endif