using HarmonyLib;
using Klei.AI;

#if 驯化加速 || usually
[HarmonyPatch(typeof(AttributeModifier),
                 MethodType.Constructor,
                 typeof(string),
                 typeof(float),
                 typeof(string),
                 typeof(bool),
                 typeof(bool),
                 typeof(bool))]
internal class 驯化 {
    private static void Prefix(string attribute_id, ref float value) {
        bool speech = 动物.驯化加速;
        if (speech && attribute_id == Db.Get().Amounts.Wildness.deltaAttribute.Id)
            if (value < 0)
                value = -1;
    }
}
#endif