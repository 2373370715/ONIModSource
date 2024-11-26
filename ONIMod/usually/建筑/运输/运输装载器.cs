using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using HarmonyLib;
using UnityEngine;

#if usually
[HarmonyPatch]
public class 运输装载器 {
    [HarmonyPatch(typeof(SolidConduitDispenser), "ConduitUpdate"), HarmonyTranspiler]
    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) {
        var codes = new List<CodeInstruction>(instructions);

        for (var i = 0; i < codes.Count; i++)
            if (codes[i].opcode == OpCodes.Ldc_R4 && codes[i].operand.ToString() == "20")
                codes[i].operand = 1000f;

        return codes.AsEnumerable();
    }

    [HarmonyPatch(typeof(SolidConduitInboxConfig), nameof(SolidConduitInboxConfig.DoPostConfigureComplete)),
     HarmonyPostfix]
    public static void Postfix(GameObject go) {
        go.AddOrGet<Storage>().capacityKg = 1000f;
    }
}

#endif