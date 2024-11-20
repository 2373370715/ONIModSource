using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using HarmonyLib;
using STRINGS;

#if 孵化器
[HarmonyPatch]
public class MyEggIncubator {
    [HarmonyPatch(typeof(EggIncubatorConfig), nameof(EggIncubatorConfig.CreateBuildingDef)), HarmonyPostfix]
    public static void Postfix(ref BuildingDef __result) {
#if usually
        __result.EnergyConsumptionWhenActive = 建筑.食物.孵化器.电力;
        __result.SelfHeatKilowattsWhenActive = 建筑.食物.孵化器.产热;
        __result.ExhaustKilowattsWhenActive = 建筑.食物.孵化器.产热;
#elif 孵化器
        __result.EnergyConsumptionWhenActive = Setting.Get().power;
        __result.SelfHeatKilowattsWhenActive = Setting.Get().selfheart;
        __result.ExhaustKilowattsWhenActive  = 0f;
#endif
    }

    [HarmonyPatch(typeof(ModifierSet), nameof(ModifierSet.Initialize)), HarmonyPostfix]
    public static void speech(ModifierSet __instance) {
        var effect = __instance.effects.Get("EggSong");
        if (effect == null) return;

        effect.duration = 1f;
        var attributeModifier = effect.SelfModifiers.FirstOrDefault();
#if usually
        attributeModifier?.SetValue(建筑.食物.孵化器.加速速率 / 100f);
#elif 孵化器
        attributeModifier?.SetValue(Setting.Get().songSpeed);
#endif
    }

    [HarmonyPatch(typeof(EggIncubator), "EggNeedsAttention"), HarmonyPostfix]
    public static void applybuff(EggIncubator __instance, ref bool __result) {
        __result = false;
        var occupant = __instance.Occupant;
        var instance = occupant != null ? occupant.GetSMI<IncubationMonitor.Instance>() : null;
        instance?.ApplySongBuff();
    }

    [HarmonyPatch(typeof(EggIncubator), "UpdateProgress"), HarmonyPostfix]
    public static void autoDrop(ref EggIncubator __instance) {
        if (__instance.GetProgress() == 1) __instance.OrderRemoveOccupant();
    }

    [HarmonyPatch(typeof(EggIncubator), nameof(EggIncubator.OrderRemoveOccupant)), HarmonyTranspiler]
    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) {
        var codes = new List<CodeInstruction>(instructions);

        var flag1 = false;
        var flag2 = true;
        for (var i = 0; i < codes.Count; i++) {
            var operand = codes[i].operand;
            var opcode  = codes[i].opcode;

            if (operand != null) Debug.Log(i + "\t" + operand);
            if (flag1 && flag2) {
                codes[i].opcode  = OpCodes.Nop;
                codes[i].operand = null;
            }

            if (operand != null && operand.ToString().Contains("tracker") && opcode == OpCodes.Stfld) flag1 = true;
            if (operand != null && operand.ToString().Contains("Void DropAll") && opcode == OpCodes.Callvirt)
                flag2 = false;
        }

        return codes.AsEnumerable();
    }
}

// buff名称
[HarmonyPatch(typeof(Localization), "Initialize")]
public class Buff文本 {
    public static void Postfix() {
        CREATURES.MODIFIERS.INCUBATOR_SONG.NAME = "摇篮曲";
        CREATURES.MODIFIERS.INCUBATOR_SONG.TOOLTIP = "这枚蛋最近听到了复制人的歌声" +
                                                     UI.PRE_KEYWORD   +
                                                     "孵化速率"           +
                                                     UI.PST_KEYWORD   +
                                                     "提高了\n\n复制人必须具有" +
                                                     UI.PRE_KEYWORD   +
                                                     "小动物养殖"          +
                                                     UI.PST_KEYWORD   +
                                                     "技能才能对蛋唱歌";
    }
}
#endif