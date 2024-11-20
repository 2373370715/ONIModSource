using System;
using System.Collections.Generic;
using System.IO;
using HarmonyLib;
using Hjson;
using KMod;

namespace RsLib;

public class RsLocalization : RsModule<RsLocalization> {
    protected HashSet<Type> addStringsTypes = new();
    protected HashSet<Type> loadTypes       = new();

    protected override void Initialized() {
        Harmony.Patch(typeof(Localization),
                      "Initialize",
                      postfix: new HarmonyMethod(typeof(RsLocalization), nameof(Localization_Initialize_Patch)));
    }

    private static void Localization_Initialize_Patch() {
        foreach (var type in Instance.loadTypes) Load(type, Instance.Mod);
        foreach (var type in Instance.addStringsTypes) CreateLocStringKeys(type);
    }

    /// <summary>
    ///     注册从mod位置的translations位置读取翻译内容
    /// </summary>
    /// <param name="type"></param>
    public RsLocalization RegisterLoad(Type type) {
        loadTypes.Add(type);
        return this;
    }

    public RsLocalization RegisterAddStrings(Type type) {
        addStringsTypes.Add(type);
        return this;
    }

    public static void Load(Type type, Mod mod) {
        var locale = Localization.GetLocale();
        var text   = locale != null ? locale.Code : null;
        if (text.IsNullOrWhiteSpace()) return;

        //Hjson
        var path = Path.Combine(mod.ContentPath, "translations", Localization.GetLocale().Code + ".hjson");
        if (File.Exists(path)) {
            OverloadStrings(LoadStringsFileByHJson(path), type.Name, type);
            return;
        }

        //Po文件
        path = Path.Combine(mod.ContentPath, "translations", Localization.GetLocale().Code + ".po");

        if (File.Exists(path)) OverloadStrings(Localization.LoadStringsFile(path, false), type.Name, type);
    }

    public static void OverloadStrings(Dictionary<string, string> translated_strings,
                                       string                     path,
                                       Type                       locStringTreeRoot) {
        var parameter_errors  = "";
        var link_errors       = "";
        var link_count_errors = "";
        Localization.OverloadStrings(translated_strings,
                                     path,
                                     locStringTreeRoot,
                                     ref parameter_errors,
                                     ref link_errors,
                                     ref link_count_errors);

        if (!string.IsNullOrEmpty(parameter_errors))
            DebugUtil.LogArgs("TRANSLATION ERROR! The following have missing or mismatched parameters:\n" +
                              parameter_errors);

        if (!string.IsNullOrEmpty(link_errors))
            DebugUtil.LogArgs("TRANSLATION ERROR! The following have mismatched <link> tags:\n" + link_errors);

        if (string.IsNullOrEmpty(link_count_errors)) return;

        DebugUtil.LogArgs("TRANSLATION ERROR! The following do not have the same amount of <link> tags as the english string which can cause nested link errors:\n" +
                          link_count_errors);
    }

    public static Dictionary<string, string> LoadStringsFileByHJson(string path) {
        var jsonObject = HjsonValue.Load(path).Qo();
        var dictionary = new Dictionary<string, string>();
        FlatHjsonOnlyString(dictionary, jsonObject);
        return dictionary;
    }

    private static void FlatHjsonOnlyString(Dictionary<string, string> dic, JsonObject jsonObject, string prefix = "") {
        foreach (var keyValuePair in jsonObject)
            if (keyValuePair.Value.JsonType == JsonType.Object)
                FlatHjsonOnlyString(dic, (JsonObject)keyValuePair.Value, prefix + keyValuePair.Key + ".");
            else if (keyValuePair.Value.JsonType == JsonType.String)
                dic.Add(prefix + keyValuePair.Key, keyValuePair.Value.Qs());
    }

    public static void ConvertPoToHjson(string poPath, string outHjsonPath, bool isPoTemple = false) {
        var po = Localization.LoadStringsFile(poPath, isPoTemple);

        var jsonObject = new JsonObject();
        foreach (var keyValuePair in po) RsHjsonUtil.PathAddString(jsonObject, keyValuePair.Key, keyValuePair.Value);

        HjsonValue.Save(jsonObject, outHjsonPath);
    }

    public static void CreateLocStringKeys(Type type) { LocString.CreateLocStringKeys(type); }
}