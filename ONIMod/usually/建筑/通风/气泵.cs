using HarmonyLib;
using UnityEngine;

#if 气泵
[HarmonyPatch]
public class 气泵 {
    [HarmonyPatch(typeof(GasPumpConfig), "DoPostConfigureComplete"), HarmonyPrefix]
    public static bool Postfix(GameObject go) {
        go.AddOrGet<LogicOperationalController>();
        go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
        go.AddOrGet<LoopingSounds>();
        go.AddOrGet<EnergyConsumer>();
        go.AddOrGet<Pump>();
        go.AddOrGet<Storage>().capacityKg = 1f;
        ElementConsumer elementConsumer = go.AddOrGet<ElementConsumer>();
        elementConsumer.configuration     = ElementConsumer.Configuration.AllGas;
        elementConsumer.consumptionRate   = 0.5f;
        elementConsumer.storeOnConsume    = true;
        elementConsumer.showInStatusPanel = false;
        elementConsumer.consumptionRadius = 建筑.通风.气泵.半径;
        ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
        conduitDispenser.conduitType    = ConduitType.Gas;
        conduitDispenser.alwaysDispense = true;
        conduitDispenser.elementFilter  = null;
        go.AddOrGetDef<OperationalController.Def>();
        go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayBehindConduits);
        return false;
    }
}
#endif