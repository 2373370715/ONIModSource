using System;
using HarmonyLib;
using Klei.CustomSettings;
using KMod;
using PeterHan.PLib.AVC;
using PeterHan.PLib.Core;
using PeterHan.PLib.Options;

namespace usually {
    internal class Patch : UserMod2 {
        public static bool SHOULDSHRINK = false;

        public override void OnLoad(Harmony harmony) {
            base.OnLoad(harmony);
            PUtil.InitLibrary();
            new POptions().RegisterOptions(this, typeof(Setting));
            Setting.Init(POptions.ReadSettings<Setting>());
#if 液冷 || usually
            LocString.CreateLocStringKeys(typeof(BetterCoolerControlStrings.UI));
#endif
#if usually
            new PVersionCheck().Register(this, new SteamVersionChecker());
            开局全科技.StartWithAllResearch = new ToggleSettingConfig("开局全部科技",
                                                                 "开局全部科技",
                                                                 "",
                                                                 new SettingLevel("Disabled",
                                                                  "Disabled",
                                                                  "Unchecked: Start with all Research is turned off (Default)"),
                                                                 new SettingLevel("Enabled",
                                                                  "Enabled",
                                                                  "Checked: Start with all Research is turned on"),
                                                                 "Disabled",
                                                                 "Disabled",
                                                                 -1L,
                                                                 false,
                                                                 false);
#endif
        }
    }
}