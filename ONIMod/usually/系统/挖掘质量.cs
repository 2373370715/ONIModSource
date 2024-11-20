using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using HarmonyLib;

// TODO 待修
#if 挖掘质量
public class 挖掘不损失质量 {
    [HarmonyPatch(typeof(WorldDamage), "OnDigComplete"), HarmonyTranspiler]
    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) {
        var codes = new List<CodeInstruction>(instructions);
        for (var i = 0; i < codes.Count; i++) {
            if (codes[i].opcode == OpCodes.Ldc_R4 && codes[i].operand.ToString() == "0.5") {
                codes[i].operand = 系统.复制人.挖掘掉落倍率;
                break;
            }
        }

        return codes.AsEnumerable();
    }
}
#endif