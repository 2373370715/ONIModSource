using System;
using HarmonyLib;
using Klei.CustomSettings;
using KMod;
using PeterHan.PLib.AVC;
using PeterHan.PLib.Core;

#if usually
[HarmonyPatch]
public class 开局全科技 {
    public static SettingConfig StartWithAllResearch;

    [HarmonyPatch(typeof(CustomGameSettings), "OnPrefabInit"), HarmonyPostfix]
    public static void Postfix(CustomGameSettings __instance) {
        __instance.AddQualitySettingConfig(StartWithAllResearch);
    }

    [HarmonyPatch(typeof(ResearchScreen), "OnSpawn"), HarmonyPostfix]
    public static void Postfix() {
        if (CustomGameSettings.Instance.GetCurrentQualitySetting(StartWithAllResearch).id != "Enabled") { return; }

        foreach (var tech in Db.Get().Techs.resources) {
            if (!tech.IsComplete()) { Research.Instance.Get(tech).Purchased(); }
        }
    }

    [HarmonyPatch(typeof(ResearchEntry), "ResearchCompleted"), HarmonyPrefix]
    public static void Prefix(ref bool notify) {
        if (CustomGameSettings.Instance.GetCurrentQualitySetting(StartWithAllResearch).id == "Enabled") {
            notify = false;
        }
    }
}
#endif