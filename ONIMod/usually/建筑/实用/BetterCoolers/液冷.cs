using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using HarmonyLib;
using UnityEngine;
using Patch = usually.Patch;

#if 液冷
[HarmonyPatch]
public class 液冷 {
    [HarmonyPatch(typeof(LiquidConditionerConfig), "ConfigureBuildingTemplate"), HarmonyPostfix]
    public static void 液冷面板(GameObject go) {
        go.AddOrGet<BetterCoolerControl>();
    }

    [HarmonyPatch(typeof(AirConditionerConfig), "ConfigureBuildingTemplate"), HarmonyPostfix]
    public static void 风冷面板(GameObject go) {
        go.AddOrGet<BetterCoolerControl>();
    }

    [HarmonyPatch(typeof(AirConditioner), "UpdateState"), HarmonyTranspiler]
    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) {
        var list = new List<CodeInstruction>(instructions);
        for (var i = 1; i < list.Count - 1; i++) {
            var flag3 = list[i].opcode     == OpCodes.Stloc_S     &&
                        list[i + 1].opcode == OpCodes.Ldloc_S     &&
                        list[i].operand    == list[i + 1].operand &&
                        list[i                       - 1].opcode == OpCodes.Add;

            if (flag3) {
                list.InsertRange(i + 1,
                                 new[] {
                                     new CodeInstruction(OpCodes.Ldarg_0),
                                     new CodeInstruction(OpCodes.Callvirt, typeof(液冷).GetMethod("SetTargetHeat")),
                                     new CodeInstruction(list[i].opcode,   list[i].operand)
                                 });

                break;
            }
        }

        for (var j = 0; j < list.Count - 1; j++) {
            var flag4 = list[j].opcode     == OpCodes.Mul     &&
                        list[j + 1].opcode == OpCodes.Ldloc_S &&
                        list[j + 2].opcode == OpCodes.Mul     &&
                        list[j + 3].opcode == OpCodes.Stloc_S;

            if (flag4) {
                list.InsertRange(j + 3,
                                 new[] { new CodeInstruction(OpCodes.Ldc_R4, 0f), new CodeInstruction(OpCodes.Mul) });

                break;
            }
        }

        return list.AsEnumerable();
    }

    public static float SetTargetHeat(AirConditioner conditioner) {
        return conditioner.gameObject.GetComponent<BetterCoolerControl>().TargetTemp;
    }
}
#endif