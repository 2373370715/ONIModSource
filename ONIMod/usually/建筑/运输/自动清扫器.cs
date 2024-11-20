using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Klei.AI;
using UnityEngine;

#if 自动清扫器
[HarmonyPatch]
public class 自动清扫器 {
    // 最大32，超过32崩溃
    public static int pickupRange = 32;

    [HarmonyPatch(typeof(SolidTransferArmConfig), "DoPostConfigureComplete"), HarmonyPostfix]
    public static void adjust_radius(ref GameObject go) { go.AddOrGet<SolidTransferArm>().pickupRange = pickupRange; }

    [HarmonyPatch(typeof(SolidTransferArmConfig), "AddVisualizer"), HarmonyPostfix]
    public static void AddVisualizer(ref GameObject prefab) {
        var rangeVisualizer = prefab.AddOrGet<RangeVisualizer>();
        rangeVisualizer.RangeMin.x          = -10;
        rangeVisualizer.RangeMin.y          = -10;
        rangeVisualizer.RangeMax.x          = 10;
        rangeVisualizer.RangeMax.y          = 10;
        rangeVisualizer.BlockingTileVisible = false;
    }

    [HarmonyPatch(typeof(SolidTransferArm), "OnPrefabInit"), HarmonyPostfix]
    public static void adjust_max_pressure(SolidTransferArm __instance) {
        var attributeInstance = __instance.GetAttributes().Get(Db.Get().Attributes.CarryAmount.Id);
        if (attributeInstance != null) {
            attributeInstance.Modifiers.Clear();
            attributeInstance.Modifiers.Add(new AttributeModifier(Db.Get().Attributes.CarryAmount.Id,
                                                                  100000f,
                                                                  __instance.gameObject.GetProperName()));
        }
    }

    [HarmonyPatch(typeof(SolidTransferArmConfig), "DoPostConfigureUnderConstruction"), HarmonyPostfix]
    public static void Postfix(ref GameObject go) {
        var component = go.GetComponent<Constructable>();
        component.requiredSkillPerk = "";
    }

    [HarmonyPatch(typeof(SolidTransferArm), "AsyncUpdate"), HarmonyPrefix]
    private static bool Prefix(ref bool         __result,
                               SolidTransferArm __instance,
                               int              cell,
                               HashSet<int>     workspace,
                               GameObject       game_object) {
        workspace.Clear();
        int num;
        int num2;
        Grid.CellToXY(cell, out num, out num2);

        // 遍历以(num, num2)为中心，边长为2*pickupRange+1的正方形区域
        for (var i = num2 - pickupRange; i < num2 + pickupRange + 1; i++) {
            for (var j = num - pickupRange; j < num + pickupRange + 1; j++) {
                // 将二维坐标(j, i)转换为单元格编号
                var num3 = Grid.XYToCell(j, i);

                // 检查单元格是否有效且从(num, num2)到(j, i)是否物理可达
                if (Grid.IsValidCell(num3))

                    // 如果符合条件，则将单元格添加到工作区列表中
                    workspace.Add(num3);
            }
        }

        var reachableCells = AboutPrivate.GetPrivateValue(__instance, "reachableCells") as HashSet<int>;

        var flag = !reachableCells.SetEquals(workspace);
        if (flag) {
            reachableCells.Clear();
            reachableCells.UnionWith(workspace);
        }

        var pickupables = AboutPrivate.GetPrivateValue(__instance, "pickupables") as List<Pickupable>;

        pickupables.Clear();
        foreach (var obj in GameScenePartitioner.Instance
                                                .AsyncSafeEnumerate(num             - pickupRange,
                                                                    num2            - pickupRange,
                                                                    2 * pickupRange + 1,
                                                                    2 * pickupRange + 1,
                                                                    GameScenePartitioner.Instance.pickupablesLayer)
                                                .Concat(GameScenePartitioner.Instance.AsyncSafeEnumerate(num -
                                                             pickupRange,
                                                         num2            - pickupRange,
                                                         2 * pickupRange + 1,
                                                         2 * pickupRange + 1,
                                                         GameScenePartitioner.Instance.storedPickupablesLayer))) {
            var pickupable = obj as Pickupable;
            if (Grid.GetCellRange(cell, pickupable.cachedCell) <= pickupRange          &&
                Assets.IsTagSolidTransferArmConveyable(pickupable.KPrefabID.PrefabTag) &&
                __instance.IsCellReachable(pickupable.cachedCell)                      &&
                pickupable.CouldBePickedUpByTransferArm(game_object))
                pickupables.Add(pickupable);
        }

        return flag;
    }
}
#endif