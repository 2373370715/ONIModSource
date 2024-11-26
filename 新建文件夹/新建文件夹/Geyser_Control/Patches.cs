using HarmonyLib;
using PeterHan.PLib.Options;
using UnityEngine;

namespace Geyser_Control {
    public class Patches {
        [HarmonyPatch(typeof(GeyserGenericConfig), "CreateGeyser")]
        public class GeyserGenericConfig_CreateGeyser_Patch {
            public static void Postfix(GameObject __result) {
                if (SingletonOptions<Config>.Instance.休眠) __result.AddComponent<DormancyButton>();
                if (SingletonOptions<Config>.Instance.喷发) __result.AddComponent<EruptionButton>();
                if (SingletonOptions<Config>.Instance.重置) __result.AddComponent<GeyserSliders.ResetButton>();
                __result.AddComponent<GeyserSliders>();
            }
        }

        public static class Studyable_OnSidescreenButtonPressed_Patch {
            /// <summary>
            /// 使用Harmony库修补Studyable类的OnSidescreenButtonPressed方法。
            /// 此方法通过将原方法替换为前缀方法来修改游戏行为。
            /// </summary>
            /// <param name="harmony">Harmony实例，用于应用修补。</param>
            public static void Patch(Harmony harmony) {
                // 获取Studyable类中名为OnSidescreenButtonPressed的方法。
                var method  = typeof(Studyable).GetMethod("OnSidescreenButtonPressed");
                // 获取Studyable_OnSidescreenButtonPressed_Patch类中的前缀方法。
                var method2 = typeof(Studyable_OnSidescreenButtonPressed_Patch).GetMethod("Prefix");
                // 使用Harmony的Patch方法，将原方法的执行委托给前缀方法。
                harmony.Patch(method, new HarmonyMethod(method2));
            }

            /// <summary>
            /// 研究。
            /// </summary>
            /// <param name="__instance">研究实例。</param>
            /// <param name="___studied">研究是否已完成</param>
            public static bool Prefix(Studyable __instance, ref bool ___studied) {
                // 秒研究
                ___studied = true;
                
                // 取消当前的研究任务
                __instance.CancelChore();
                
                // 触发特定事件，参数为事件ID
                __instance.Trigger(-1436775550);
                
                // 刷新学习活动状态
                __instance.Refresh();
                return false;
            }
        }
    }
}