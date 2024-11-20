using HarmonyLib;
using UnityEngine;
using usually;

#if 复制人速度
public class SpeedBump {
    [HarmonyPatch(typeof(BipedTransitionLayer), "BeginTransition"), HarmonyPrefix]
    private static void Prefix(Navigator                  navigator,
                               Navigator.ActiveTransition transition,
                               bool                       ___isWalking,
                               ref float                  ___startTime) {
        // if (___isWalking) {
        transition.speed     =  系统.复制人.速度;
        transition.animSpeed += transition.animSpeed * 系统.复制人.速度;

        // ___startTime         =  Time.time;
        // }

        // return false;
    }
}
#endif