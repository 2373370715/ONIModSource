using System;
using System.IO;
using System.Reflection;
using HarmonyLib;
using KMod;

namespace Geyser_Control {
    internal class LocalizePatch {
        [HarmonyPatch(typeof(Localization), "Initialize")]
        public class Localization_Initialize_Patch {
            public static void Postfix() { Translate(typeof(STRINGS)); }

            public static void Translate(Type root) {
                Localization.RegisterForTranslation(root);
                LoadStrings();
                LocString.CreateLocStringKeys(root, null);
                Localization.GenerateStringsTemplate(root, Path.Combine(Manager.GetDirectory(), "strings_templates"));
            }

            private static void LoadStrings() {
                var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                var path          = "translations";
                var locale        = Localization.GetLocale();
                var path2         = Path.Combine(directoryName, path, (locale != null ? locale.Code : null) + ".po");
                if (File.Exists(path2)) Localization.OverloadStrings(Localization.LoadStringsFile(path2, false));
            }
        }
    }
}