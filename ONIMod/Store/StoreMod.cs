using System;
using HarmonyLib;
using KMod;
using PeterHan.PLib.AVC;
using PeterHan.PLib.Buildings;
using PeterHan.PLib.Core;
using PeterHan.PLib.Database;

namespace Store {
    public sealed class StoreMod : UserMod2 {
        // OnLoad方法在模组加载时被调用，用于初始化模组的各种组件和设置
        public override void OnLoad(Harmony harmony) {
            base.OnLoad(harmony);
            PUtil.InitLibrary();
            new PLocalization().Register();
            new PBuildingManager().Register(StoreConfig.CreateBuilding());
            new PVersionCheck().Register(this, new SteamVersionChecker());
            PrimaryElement.MAX_MASS = 10000000f;
        }
        
        [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
        public static class GeneratedBuildings_LoadGeneratedBuildings_Patch {
            // 添加文本
            internal static void Prefix() { LocString.CreateLocStringKeys(typeof(StoreStrings.BUILDINGS)); }
        }
    }
}