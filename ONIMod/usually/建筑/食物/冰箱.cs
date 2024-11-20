using System;
using HarmonyLib;
using UnityEngine;

#if 冰箱
[HarmonyPatch]
public class RefrigeratorPatch1 {
    [HarmonyPatch(typeof(RefrigeratorConfig), nameof(RefrigeratorConfig.DoPostConfigureComplete)), HarmonyPostfix]
    public static void increase_capacity(GameObject go) {
        var storage = go.AddOrGet<Storage>();
        storage.capacityKg = 建筑.食物.冰箱.容量;
    }

    [HarmonyPatch(typeof(RefrigeratorController), "AllFoodCool"), HarmonyPostfix]
    public static void decrease_temperature(RefrigeratorController.StatesInstance smi, float dt, ref bool __result) {
        foreach (var gameObject in smi.storage.items)
            if (!(gameObject == null)) {
                var component = gameObject.GetComponent<PrimaryElement>();
                if (!(component == null) && component.Mass >= 0.01f && component.Temperature >= 248.15f) {
                    component.Temperature = 248.15f;
                }
            }

        __result = false;
    }

    [HarmonyPatch(typeof(RefrigeratorController.StatesInstance),
                     MethodType.Constructor,
                     new Type[] { typeof(IStateMachineTarget), typeof(RefrigeratorController.Def) }), HarmonyPrefix]
    public static bool Prefix(IStateMachineTarget master, RefrigeratorController.Def def) {
        def.simulatedInternalTemperature = 248.15f;
        return true;
    }
}
#endif