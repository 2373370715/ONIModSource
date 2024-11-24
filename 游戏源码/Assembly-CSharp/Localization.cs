using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using ArabicSupport;
using Klei;
using KMod;
using Steamworks;
using STRINGS;
using TMPro;
using UnityEngine;

// Token: 0x020014A0 RID: 5280
public static class Localization
{
	// Token: 0x17000707 RID: 1799
	// (get) Token: 0x06006DD9 RID: 28121 RVA: 0x000E7FFC File Offset: 0x000E61FC
	public static TMP_FontAsset FontAsset
	{
		get
		{
			return Localization.sFontAsset;
		}
	}

	// Token: 0x17000708 RID: 1800
	// (get) Token: 0x06006DDA RID: 28122 RVA: 0x000E8003 File Offset: 0x000E6203
	public static bool IsRightToLeft
	{
		get
		{
			return Localization.sLocale != null && Localization.sLocale.IsRightToLeft;
		}
	}

	// Token: 0x06006DDB RID: 28123 RVA: 0x002EC9B0 File Offset: 0x002EABB0
	private static IEnumerable<Type> CollectLocStringTreeRoots(string locstrings_namespace, Assembly assembly)
	{
		return from type in assembly.GetTypes()
		where type.IsClass && type.Namespace == locstrings_namespace && !type.IsNested
		select type;
	}

	// Token: 0x06006DDC RID: 28124 RVA: 0x002EC9E4 File Offset: 0x002EABE4
	private static Dictionary<string, object> MakeRuntimeLocStringTree(Type locstring_tree_root)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		foreach (FieldInfo fieldInfo in locstring_tree_root.GetFields())
		{
			if (!(fieldInfo.FieldType != typeof(LocString)))
			{
				if (!fieldInfo.IsStatic)
				{
					DebugUtil.DevLogError("LocString fields must be static, skipping. " + fieldInfo.Name);
				}
				else
				{
					LocString locString = (LocString)fieldInfo.GetValue(null);
					if (locString == null)
					{
						global::Debug.LogError("Tried to generate LocString for " + fieldInfo.Name + " but it is null so skipping");
					}
					else
					{
						dictionary[fieldInfo.Name] = locString.text;
					}
				}
			}
		}
		foreach (Type type in locstring_tree_root.GetNestedTypes())
		{
			Dictionary<string, object> dictionary2 = Localization.MakeRuntimeLocStringTree(type);
			if (dictionary2.Count > 0)
			{
				dictionary[type.Name] = dictionary2;
			}
		}
		return dictionary;
	}

	// Token: 0x06006DDD RID: 28125 RVA: 0x002ECACC File Offset: 0x002EACCC
	private static void WriteStringsTemplate(string path, StreamWriter writer, Dictionary<string, object> runtime_locstring_tree)
	{
		List<string> list = new List<string>(runtime_locstring_tree.Keys);
		list.Sort();
		foreach (string text in list)
		{
			string text2 = path + "." + text;
			object obj = runtime_locstring_tree[text];
			if (obj.GetType() != typeof(string))
			{
				Localization.WriteStringsTemplate(text2, writer, obj as Dictionary<string, object>);
			}
			else
			{
				string text3 = obj as string;
				text3 = text3.Replace("\\", "\\\\");
				text3 = text3.Replace("\"", "\\\"");
				text3 = text3.Replace("\n", "\\n");
				text3 = text3.Replace("’", "'");
				text3 = text3.Replace("“", "\\\"");
				text3 = text3.Replace("”", "\\\"");
				text3 = text3.Replace("…", "...");
				writer.WriteLine("#. " + text2);
				writer.WriteLine("msgctxt \"{0}\"", text2);
				writer.WriteLine("msgid \"" + text3 + "\"");
				writer.WriteLine("msgstr \"\"");
				writer.WriteLine("");
			}
		}
	}

	// Token: 0x06006DDE RID: 28126 RVA: 0x002ECC4C File Offset: 0x002EAE4C
	public static void GenerateStringsTemplate(string locstrings_namespace, Assembly assembly, string output_filename, Dictionary<string, object> current_runtime_locstring_forest)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		foreach (Type type in Localization.CollectLocStringTreeRoots(locstrings_namespace, assembly))
		{
			Dictionary<string, object> dictionary2 = Localization.MakeRuntimeLocStringTree(type);
			if (dictionary2.Count > 0)
			{
				dictionary[type.Name] = dictionary2;
			}
		}
		if (current_runtime_locstring_forest != null)
		{
			dictionary.Concat(current_runtime_locstring_forest);
		}
		using (StreamWriter streamWriter = new StreamWriter(output_filename, false, new UTF8Encoding(false)))
		{
			streamWriter.WriteLine("msgid \"\"");
			streamWriter.WriteLine("msgstr \"\"");
			streamWriter.WriteLine("\"Application: Oxygen Not Included\"");
			streamWriter.WriteLine("\"POT Version: 2.0\"");
			streamWriter.WriteLine("");
			Localization.WriteStringsTemplate(locstrings_namespace, streamWriter, dictionary);
		}
		DebugUtil.LogArgs(new object[]
		{
			"Generated " + output_filename
		});
	}

	// Token: 0x06006DDF RID: 28127 RVA: 0x002ECD48 File Offset: 0x002EAF48
	public static void GenerateStringsTemplate(Type locstring_tree_root, string output_folder)
	{
		output_folder = FileSystem.Normalize(output_folder);
		if (!FileUtil.CreateDirectory(output_folder, 5))
		{
			return;
		}
		Localization.GenerateStringsTemplate(locstring_tree_root.Namespace, Assembly.GetAssembly(locstring_tree_root), FileSystem.Normalize(Path.Combine(output_folder, string.Format("{0}_template.pot", locstring_tree_root.Namespace.ToLower()))), null);
	}

	// Token: 0x06006DE0 RID: 28128 RVA: 0x002ECD9C File Offset: 0x002EAF9C
	public static void Initialize()
	{
		DebugUtil.LogArgs(new object[]
		{
			"Localization.Initialize!"
		});
		bool flag = false;
		switch (Localization.GetSelectedLanguageType())
		{
		case Localization.SelectedLanguageType.None:
			Localization.sFontAsset = Localization.GetFont(Localization.GetDefaultLocale().FontName);
			break;
		case Localization.SelectedLanguageType.Preinstalled:
		{
			string currentLanguageCode = Localization.GetCurrentLanguageCode();
			if (!string.IsNullOrEmpty(currentLanguageCode))
			{
				DebugUtil.LogArgs(new object[]
				{
					"Localization Initialize... Preinstalled localization"
				});
				DebugUtil.LogArgs(new object[]
				{
					" -> ",
					currentLanguageCode
				});
				Localization.LoadPreinstalledTranslation(currentLanguageCode);
			}
			else
			{
				flag = true;
			}
			break;
		}
		case Localization.SelectedLanguageType.UGC:
			if (LanguageOptionsScreen.HasInstalledLanguage())
			{
				DebugUtil.LogArgs(new object[]
				{
					"Localization Initialize... Mod-based localization"
				});
				string savedLanguageMod = LanguageOptionsScreen.GetSavedLanguageMod();
				if (LanguageOptionsScreen.SetCurrentLanguage(savedLanguageMod))
				{
					DebugUtil.LogArgs(new object[]
					{
						" -> Loaded language from mod: " + savedLanguageMod
					});
				}
				else
				{
					DebugUtil.LogArgs(new object[]
					{
						" -> Failed to load language from mod: " + savedLanguageMod
					});
				}
			}
			else
			{
				flag = true;
			}
			break;
		}
		if (flag)
		{
			Localization.ClearLanguage();
		}
	}

	// Token: 0x06006DE1 RID: 28129 RVA: 0x002ECEA0 File Offset: 0x002EB0A0
	public static void VerifyTranslationModSubscription(GameObject context)
	{
		if (Localization.GetSelectedLanguageType() != Localization.SelectedLanguageType.UGC)
		{
			return;
		}
		if (!SteamManager.Initialized)
		{
			return;
		}
		if (LanguageOptionsScreen.HasInstalledLanguage())
		{
			return;
		}
		PublishedFileId_t publishedFileId_t = new PublishedFileId_t((ulong)KPlayerPrefs.GetInt("InstalledLanguage", (int)PublishedFileId_t.Invalid.m_PublishedFileId));
		Label rhs = new Label
		{
			distribution_platform = Label.DistributionPlatform.Steam,
			id = publishedFileId_t.ToString()
		};
		string arg = UI.FRONTEND.TRANSLATIONS_SCREEN.UNKNOWN;
		foreach (Mod mod in Global.Instance.modManager.mods)
		{
			if (mod.label.Match(rhs))
			{
				arg = mod.title;
				break;
			}
		}
		Localization.ClearLanguage();
		KScreen component = KScreenManager.AddChild(context, ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject).GetComponent<KScreen>();
		component.Activate();
		ConfirmDialogScreen component2 = component.GetComponent<ConfirmDialogScreen>();
		string title_text = UI.CONFIRMDIALOG.DIALOG_HEADER;
		string text = string.Format(UI.FRONTEND.TRANSLATIONS_SCREEN.MISSING_LANGUAGE_PACK, arg);
		string confirm_text = UI.FRONTEND.TRANSLATIONS_SCREEN.RESTART;
		component2.PopupConfirmDialog(text, new System.Action(App.instance.Restart), null, null, null, title_text, confirm_text, null, null);
	}

	// Token: 0x06006DE2 RID: 28130 RVA: 0x002ECFE8 File Offset: 0x002EB1E8
	public static void LoadPreinstalledTranslation(string code)
	{
		if (!string.IsNullOrEmpty(code) && code != Localization.DEFAULT_LANGUAGE_CODE)
		{
			string preinstalledLocalizationFilePath = Localization.GetPreinstalledLocalizationFilePath(code);
			if (Localization.LoadLocalTranslationFile(Localization.SelectedLanguageType.Preinstalled, preinstalledLocalizationFilePath))
			{
				KPlayerPrefs.SetString(Localization.SELECTED_LANGUAGE_CODE_KEY, code);
				return;
			}
		}
		else
		{
			Localization.ClearLanguage();
		}
	}

	// Token: 0x06006DE3 RID: 28131 RVA: 0x000E8018 File Offset: 0x000E6218
	public static bool LoadLocalTranslationFile(Localization.SelectedLanguageType source, string path)
	{
		if (!File.Exists(path))
		{
			return false;
		}
		bool flag = Localization.LoadTranslationFromLines(File.ReadAllLines(path, Encoding.UTF8));
		if (flag)
		{
			KPlayerPrefs.SetString(Localization.SELECTED_LANGUAGE_TYPE_KEY, source.ToString());
			return flag;
		}
		Localization.ClearLanguage();
		return flag;
	}

	// Token: 0x06006DE4 RID: 28132 RVA: 0x002ED02C File Offset: 0x002EB22C
	private static bool LoadTranslationFromLines(string[] lines)
	{
		if (lines == null || lines.Length == 0)
		{
			return false;
		}
		Localization.sLocale = Localization.GetLocale(lines);
		DebugUtil.LogArgs(new object[]
		{
			" -> Locale is now ",
			Localization.sLocale.ToString()
		});
		bool flag = Localization.LoadTranslation(lines, false);
		if (flag)
		{
			Localization.currentFontName = Localization.GetFontName(lines);
			Localization.SwapToLocalizedFont(Localization.currentFontName);
		}
		return flag;
	}

	// Token: 0x06006DE5 RID: 28133 RVA: 0x002ED090 File Offset: 0x002EB290
	public static bool LoadTranslation(string[] lines, bool isTemplate = false)
	{
		bool result;
		try
		{
			Localization.OverloadStrings(Localization.ExtractTranslatedStrings(lines, isTemplate));
			result = true;
		}
		catch (Exception ex)
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				ex
			});
			result = false;
		}
		return result;
	}

	// Token: 0x06006DE6 RID: 28134 RVA: 0x000E8054 File Offset: 0x000E6254
	public static Dictionary<string, string> LoadStringsFile(string path, bool isTemplate)
	{
		return Localization.ExtractTranslatedStrings(File.ReadAllLines(path, Encoding.UTF8), isTemplate);
	}

	// Token: 0x06006DE7 RID: 28135 RVA: 0x002ED0D4 File Offset: 0x002EB2D4
	public static Dictionary<string, string> ExtractTranslatedStrings(string[] lines, bool isTemplate = false)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		Localization.Entry entry = default(Localization.Entry);
		string key = isTemplate ? "msgid" : "msgstr";
		for (int i = 0; i < lines.Length; i++)
		{
			string text = lines[i];
			if (text == null || text.Length == 0)
			{
				entry = default(Localization.Entry);
			}
			else
			{
				string parameter = Localization.GetParameter("msgctxt", i, lines);
				if (parameter != null)
				{
					entry.msgctxt = parameter;
				}
				parameter = Localization.GetParameter(key, i, lines);
				if (parameter != null)
				{
					entry.msgstr = parameter;
				}
			}
			if (entry.IsPopulated)
			{
				dictionary[entry.msgctxt] = entry.msgstr;
				entry = default(Localization.Entry);
			}
		}
		return dictionary;
	}

	// Token: 0x06006DE8 RID: 28136 RVA: 0x002ED180 File Offset: 0x002EB380
	private static string FixupString(string result)
	{
		result = result.Replace("\\n", "\n");
		result = result.Replace("\\\"", "\"");
		result = result.Replace("<style=“", "<style=\"");
		result = result.Replace("”>", "\">");
		result = result.Replace("<color=^p", "<color=#");
		return result;
	}

	// Token: 0x06006DE9 RID: 28137 RVA: 0x002ED1E8 File Offset: 0x002EB3E8
	private static string GetParameter(string key, int idx, string[] all_lines)
	{
		if (!all_lines[idx].StartsWith(key))
		{
			return null;
		}
		List<string> list = new List<string>();
		string text = all_lines[idx];
		text = text.Substring(key.Length + 1, text.Length - key.Length - 1);
		list.Add(text);
		for (int i = idx + 1; i < all_lines.Length; i++)
		{
			string text2 = all_lines[i];
			if (!text2.StartsWith("\""))
			{
				break;
			}
			list.Add(text2);
		}
		string text3 = "";
		foreach (string text4 in list)
		{
			if (text4.EndsWith("\r"))
			{
				text4 = text4.Substring(0, text4.Length - 1);
			}
			text4 = text4.Substring(1, text4.Length - 2);
			text4 = Localization.FixupString(text4);
			text3 += text4;
		}
		return text3;
	}

	// Token: 0x06006DEA RID: 28138 RVA: 0x002ED2E8 File Offset: 0x002EB4E8
	private static void AddAssembly(string locstrings_namespace, Assembly assembly)
	{
		List<Assembly> list;
		if (!Localization.translatable_assemblies.TryGetValue(locstrings_namespace, out list))
		{
			list = new List<Assembly>();
			Localization.translatable_assemblies.Add(locstrings_namespace, list);
		}
		list.Add(assembly);
	}

	// Token: 0x06006DEB RID: 28139 RVA: 0x000E8067 File Offset: 0x000E6267
	public static void AddAssembly(Assembly assembly)
	{
		Localization.AddAssembly("STRINGS", assembly);
	}

	// Token: 0x06006DEC RID: 28140 RVA: 0x002ED320 File Offset: 0x002EB520
	public static void RegisterForTranslation(Type locstring_tree_root)
	{
		Assembly assembly = Assembly.GetAssembly(locstring_tree_root);
		Localization.AddAssembly(locstring_tree_root.Namespace, assembly);
		string parent_path = locstring_tree_root.Namespace + ".";
		foreach (Type type in Localization.CollectLocStringTreeRoots(locstring_tree_root.Namespace, assembly))
		{
			LocString.CreateLocStringKeys(type, parent_path);
		}
	}

	// Token: 0x06006DED RID: 28141 RVA: 0x002ED398 File Offset: 0x002EB598
	public static void OverloadStrings(Dictionary<string, string> translated_strings)
	{
		string text = "";
		string text2 = "";
		string text3 = "";
		foreach (KeyValuePair<string, List<Assembly>> keyValuePair in Localization.translatable_assemblies)
		{
			foreach (Assembly assembly in keyValuePair.Value)
			{
				foreach (Type type in Localization.CollectLocStringTreeRoots(keyValuePair.Key, assembly))
				{
					string path = keyValuePair.Key + "." + type.Name;
					Localization.OverloadStrings(translated_strings, path, type, ref text, ref text2, ref text3);
				}
			}
		}
		if (!string.IsNullOrEmpty(text))
		{
			DebugUtil.LogArgs(new object[]
			{
				"TRANSLATION ERROR! The following have missing or mismatched parameters:\n" + text
			});
		}
		if (!string.IsNullOrEmpty(text2))
		{
			DebugUtil.LogArgs(new object[]
			{
				"TRANSLATION ERROR! The following have mismatched <link> tags:\n" + text2
			});
		}
		if (!string.IsNullOrEmpty(text3))
		{
			DebugUtil.LogArgs(new object[]
			{
				"TRANSLATION ERROR! The following do not have the same amount of <link> tags as the english string which can cause nested link errors:\n" + text3
			});
		}
	}

	// Token: 0x06006DEE RID: 28142 RVA: 0x002ED50C File Offset: 0x002EB70C
	public static void OverloadStrings(Dictionary<string, string> translated_strings, string path, Type locstring_hierarchy, ref string parameter_errors, ref string link_errors, ref string link_count_errors)
	{
		foreach (FieldInfo fieldInfo in locstring_hierarchy.GetFields())
		{
			if (!(fieldInfo.FieldType != typeof(LocString)))
			{
				string text = path + "." + fieldInfo.Name;
				string text2 = null;
				if (translated_strings.TryGetValue(text, out text2))
				{
					LocString locString = (LocString)fieldInfo.GetValue(null);
					LocString value = new LocString(text2, text);
					if (!Localization.AreParametersPreserved(locString.text, text2))
					{
						parameter_errors = parameter_errors + "\t" + text + "\n";
					}
					else if (!Localization.HasSameOrLessLinkCountAsEnglish(locString.text, text2))
					{
						link_count_errors = link_count_errors + "\t" + text + "\n";
					}
					else if (!Localization.HasMatchingLinkTags(text2, 0))
					{
						link_errors = link_errors + "\t" + text + "\n";
					}
					else
					{
						fieldInfo.SetValue(null, value);
					}
				}
			}
		}
		foreach (Type type in locstring_hierarchy.GetNestedTypes())
		{
			string path2 = path + "." + type.Name;
			Localization.OverloadStrings(translated_strings, path2, type, ref parameter_errors, ref link_errors, ref link_count_errors);
		}
	}

	// Token: 0x06006DEF RID: 28143 RVA: 0x000E8074 File Offset: 0x000E6274
	public static string GetDefaultLocalizationFilePath()
	{
		return Path.Combine(Application.streamingAssetsPath, "strings/strings_template.pot");
	}

	// Token: 0x06006DF0 RID: 28144 RVA: 0x000E8085 File Offset: 0x000E6285
	public static string GetModLocalizationFilePath()
	{
		return Path.Combine(Application.streamingAssetsPath, "strings/strings.po");
	}

	// Token: 0x06006DF1 RID: 28145 RVA: 0x002ED648 File Offset: 0x002EB848
	public static string GetPreinstalledLocalizationFilePath(string code)
	{
		string path = "strings/strings_preinstalled_" + code + ".po";
		return Path.Combine(Application.streamingAssetsPath, path);
	}

	// Token: 0x06006DF2 RID: 28146 RVA: 0x000E8096 File Offset: 0x000E6296
	public static string GetPreinstalledLocalizationTitle(string code)
	{
		return Strings.Get("STRINGS.UI.FRONTEND.TRANSLATIONS_SCREEN.PREINSTALLED_LANGUAGES." + code.ToUpper());
	}

	// Token: 0x06006DF3 RID: 28147 RVA: 0x002ED674 File Offset: 0x002EB874
	public static Texture2D GetPreinstalledLocalizationImage(string code)
	{
		string path = Path.Combine(Application.streamingAssetsPath, "strings/preinstalled_icon_" + code + ".png");
		if (File.Exists(path))
		{
			byte[] data = File.ReadAllBytes(path);
			Texture2D texture2D = new Texture2D(2, 2);
			texture2D.LoadImage(data);
			return texture2D;
		}
		return null;
	}

	// Token: 0x06006DF4 RID: 28148 RVA: 0x000E80B2 File Offset: 0x000E62B2
	public static void SetLocale(Localization.Locale locale)
	{
		Localization.sLocale = locale;
		DebugUtil.LogArgs(new object[]
		{
			" -> Locale is now ",
			Localization.sLocale.ToString()
		});
	}

	// Token: 0x06006DF5 RID: 28149 RVA: 0x000E80DA File Offset: 0x000E62DA
	public static Localization.Locale GetLocale()
	{
		return Localization.sLocale;
	}

	// Token: 0x06006DF6 RID: 28150 RVA: 0x002ED6BC File Offset: 0x002EB8BC
	private static string GetFontParam(string line)
	{
		string text = null;
		if (line.StartsWith("\"Font:"))
		{
			text = line.Substring("\"Font:".Length).Trim();
			text = text.Replace("\\n", "");
			text = text.Replace("\"", "");
		}
		return text;
	}

	// Token: 0x06006DF7 RID: 28151 RVA: 0x002ED714 File Offset: 0x002EB914
	public static string GetCurrentLanguageCode()
	{
		switch (Localization.GetSelectedLanguageType())
		{
		case Localization.SelectedLanguageType.None:
			return Localization.DEFAULT_LANGUAGE_CODE;
		case Localization.SelectedLanguageType.Preinstalled:
			return KPlayerPrefs.GetString(Localization.SELECTED_LANGUAGE_CODE_KEY);
		case Localization.SelectedLanguageType.UGC:
			return LanguageOptionsScreen.GetInstalledLanguageCode();
		default:
			return "";
		}
	}

	// Token: 0x06006DF8 RID: 28152 RVA: 0x002ED758 File Offset: 0x002EB958
	public static Localization.SelectedLanguageType GetSelectedLanguageType()
	{
		return (Localization.SelectedLanguageType)Enum.Parse(typeof(Localization.SelectedLanguageType), KPlayerPrefs.GetString(Localization.SELECTED_LANGUAGE_TYPE_KEY, Localization.SelectedLanguageType.None.ToString()), true);
	}

	// Token: 0x06006DF9 RID: 28153 RVA: 0x002ED794 File Offset: 0x002EB994
	private static string GetLanguageCode(string line)
	{
		string text = null;
		if (line.StartsWith("\"Language:"))
		{
			text = line.Substring("\"Language:".Length).Trim();
			text = text.Replace("\\n", "");
			text = text.Replace("\"", "");
		}
		return text;
	}

	// Token: 0x06006DFA RID: 28154 RVA: 0x002ED7EC File Offset: 0x002EB9EC
	private static Localization.Locale GetLocaleForCode(string code)
	{
		Localization.Locale result = null;
		foreach (Localization.Locale locale in Localization.Locales)
		{
			if (locale.MatchesCode(code))
			{
				result = locale;
				break;
			}
		}
		return result;
	}

	// Token: 0x06006DFB RID: 28155 RVA: 0x002ED848 File Offset: 0x002EBA48
	public static Localization.Locale GetLocale(string[] lines)
	{
		Localization.Locale locale = null;
		string text = null;
		if (lines != null && lines.Length != 0)
		{
			foreach (string text2 in lines)
			{
				if (text2 != null && text2.Length != 0)
				{
					text = Localization.GetLanguageCode(text2);
					if (text != null)
					{
						locale = Localization.GetLocaleForCode(text);
					}
					if (text != null)
					{
						break;
					}
				}
			}
		}
		if (locale == null)
		{
			locale = Localization.GetDefaultLocale();
		}
		if (text != null && locale.Code == "")
		{
			locale.SetCode(text);
		}
		return locale;
	}

	// Token: 0x06006DFC RID: 28156 RVA: 0x000E80E1 File Offset: 0x000E62E1
	private static string GetFontName(string filename)
	{
		return Localization.GetFontName(File.ReadAllLines(filename, Encoding.UTF8));
	}

	// Token: 0x06006DFD RID: 28157 RVA: 0x002ED8C0 File Offset: 0x002EBAC0
	public static Localization.Locale GetDefaultLocale()
	{
		Localization.Locale result = null;
		foreach (Localization.Locale locale in Localization.Locales)
		{
			if (locale.Lang == Localization.Language.Unspecified)
			{
				result = new Localization.Locale(locale);
				break;
			}
		}
		return result;
	}

	// Token: 0x06006DFE RID: 28158 RVA: 0x002ED920 File Offset: 0x002EBB20
	public static string GetDefaultFontName()
	{
		string result = null;
		foreach (Localization.Locale locale in Localization.Locales)
		{
			if (locale.Lang == Localization.Language.Unspecified)
			{
				result = locale.FontName;
				break;
			}
		}
		return result;
	}

	// Token: 0x06006DFF RID: 28159 RVA: 0x002ED980 File Offset: 0x002EBB80
	public static string ValidateFontName(string fontName)
	{
		foreach (Localization.Locale locale in Localization.Locales)
		{
			if (locale.MatchesFont(fontName))
			{
				return locale.FontName;
			}
		}
		return null;
	}

	// Token: 0x06006E00 RID: 28160 RVA: 0x002ED9E0 File Offset: 0x002EBBE0
	public static string GetFontName(string[] lines)
	{
		string text = null;
		if (lines != null)
		{
			foreach (string text2 in lines)
			{
				if (!string.IsNullOrEmpty(text2))
				{
					string fontParam = Localization.GetFontParam(text2);
					if (fontParam != null)
					{
						text = Localization.ValidateFontName(fontParam);
					}
				}
				if (text != null)
				{
					break;
				}
			}
		}
		if (text == null)
		{
			if (Localization.sLocale != null)
			{
				text = Localization.sLocale.FontName;
			}
			else
			{
				text = Localization.GetDefaultFontName();
			}
		}
		return text;
	}

	// Token: 0x06006E01 RID: 28161 RVA: 0x000E80F3 File Offset: 0x000E62F3
	public static void SwapToLocalizedFont()
	{
		Localization.SwapToLocalizedFont(Localization.currentFontName);
	}

	// Token: 0x06006E02 RID: 28162 RVA: 0x002EDA44 File Offset: 0x002EBC44
	public static bool SwapToLocalizedFont(string fontname)
	{
		if (string.IsNullOrEmpty(fontname))
		{
			return false;
		}
		Localization.sFontAsset = Localization.GetFont(fontname);
		foreach (TextStyleSetting textStyleSetting in Resources.FindObjectsOfTypeAll<TextStyleSetting>())
		{
			if (textStyleSetting != null)
			{
				textStyleSetting.sdfFont = Localization.sFontAsset;
			}
		}
		bool isRightToLeft = Localization.IsRightToLeft;
		foreach (LocText locText in Resources.FindObjectsOfTypeAll<LocText>())
		{
			if (locText != null)
			{
				locText.SwapFont(Localization.sFontAsset, isRightToLeft);
			}
		}
		return true;
	}

	// Token: 0x06006E03 RID: 28163 RVA: 0x002EDACC File Offset: 0x002EBCCC
	private static bool SetFont(Type target_type, object target, TMP_FontAsset font, bool is_right_to_left, HashSet<MemberInfo> excluded_members)
	{
		if (target_type == null || target == null || font == null)
		{
			return false;
		}
		foreach (FieldInfo fieldInfo in target_type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy))
		{
			if (!excluded_members.Contains(fieldInfo))
			{
				if (fieldInfo.FieldType == typeof(TextStyleSetting))
				{
					((TextStyleSetting)fieldInfo.GetValue(target)).sdfFont = font;
				}
				else if (fieldInfo.FieldType == typeof(LocText))
				{
					((LocText)fieldInfo.GetValue(target)).SwapFont(font, is_right_to_left);
				}
				else if (fieldInfo.FieldType == typeof(GameObject))
				{
					foreach (Component component in ((GameObject)fieldInfo.GetValue(target)).GetComponents<Component>())
					{
						Localization.SetFont(component.GetType(), component, font, is_right_to_left, excluded_members);
					}
				}
				else if (fieldInfo.MemberType == MemberTypes.Field && fieldInfo.FieldType != fieldInfo.DeclaringType)
				{
					Localization.SetFont(fieldInfo.FieldType, fieldInfo.GetValue(target), font, is_right_to_left, excluded_members);
				}
			}
		}
		return true;
	}

	// Token: 0x06006E04 RID: 28164 RVA: 0x000E8100 File Offset: 0x000E6300
	public static bool SetFont<T>(T target, TMP_FontAsset font, bool is_right_to_left, HashSet<MemberInfo> excluded_members)
	{
		return Localization.SetFont(typeof(T), target, font, is_right_to_left, excluded_members);
	}

	// Token: 0x06006E05 RID: 28165 RVA: 0x002EDC08 File Offset: 0x002EBE08
	public static TMP_FontAsset GetFont(string fontname)
	{
		foreach (TMP_FontAsset tmp_FontAsset in Resources.FindObjectsOfTypeAll<TMP_FontAsset>())
		{
			if (tmp_FontAsset.name == fontname)
			{
				return tmp_FontAsset;
			}
		}
		return null;
	}

	// Token: 0x06006E06 RID: 28166 RVA: 0x002EDC40 File Offset: 0x002EBE40
	private static bool HasSameOrLessTokenCount(string english_string, string translated_string, string token)
	{
		int num = english_string.Split(new string[]
		{
			token
		}, StringSplitOptions.None).Length;
		int num2 = translated_string.Split(new string[]
		{
			token
		}, StringSplitOptions.None).Length;
		return num >= num2;
	}

	// Token: 0x06006E07 RID: 28167 RVA: 0x000E811A File Offset: 0x000E631A
	private static bool HasSameOrLessLinkCountAsEnglish(string english_string, string translated_string)
	{
		return Localization.HasSameOrLessTokenCount(english_string, translated_string, "<link") && Localization.HasSameOrLessTokenCount(english_string, translated_string, "</link");
	}

	// Token: 0x06006E08 RID: 28168 RVA: 0x002EDC7C File Offset: 0x002EBE7C
	private static bool HasMatchingLinkTags(string str, int idx = 0)
	{
		int num = str.IndexOf("<link", idx);
		int num2 = str.IndexOf("</link", idx);
		if (num == -1 && num2 == -1)
		{
			return true;
		}
		if (num == -1 && num2 != -1)
		{
			return false;
		}
		if (num != -1 && num2 == -1)
		{
			return false;
		}
		if (num2 < num)
		{
			return false;
		}
		int num3 = str.IndexOf("<link", num + 1);
		return (num < 0 || num3 == -1 || num3 >= num2) && Localization.HasMatchingLinkTags(str, num2 + 1);
	}

	// Token: 0x06006E09 RID: 28169 RVA: 0x002EDCF0 File Offset: 0x002EBEF0
	private static bool AreParametersPreserved(string old_string, string new_string)
	{
		MatchCollection matchCollection = Regex.Matches(old_string, "({.[^}]*?})(?!.*\\1)");
		MatchCollection matchCollection2 = Regex.Matches(new_string, "({.[^}]*?})(?!.*\\1)");
		bool result = false;
		if (matchCollection == null && matchCollection2 == null)
		{
			result = true;
		}
		else if (matchCollection != null && matchCollection2 != null && matchCollection.Count == matchCollection2.Count)
		{
			result = true;
			foreach (object obj in matchCollection)
			{
				string a = obj.ToString();
				bool flag = false;
				foreach (object obj2 in matchCollection2)
				{
					string b = obj2.ToString();
					if (a == b)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					result = false;
					break;
				}
			}
		}
		return result;
	}

	// Token: 0x06006E0A RID: 28170 RVA: 0x000E8138 File Offset: 0x000E6338
	public static bool HasDirtyWords(string str)
	{
		return Localization.FilterDirtyWords(str) != str;
	}

	// Token: 0x06006E0B RID: 28171 RVA: 0x000E8146 File Offset: 0x000E6346
	public static string FilterDirtyWords(string str)
	{
		return DistributionPlatform.Inst.ApplyWordFilter(str);
	}

	// Token: 0x06006E0C RID: 28172 RVA: 0x000E8153 File Offset: 0x000E6353
	public static string GetFileDateFormat(int format_idx)
	{
		return "{" + format_idx.ToString() + ":dd / MMM / yyyy}";
	}

	// Token: 0x06006E0D RID: 28173 RVA: 0x002EDDE8 File Offset: 0x002EBFE8
	public static void ClearLanguage()
	{
		DebugUtil.LogArgs(new object[]
		{
			" -> Clearing selected language! Either it didn't load correct or returning to english by menu."
		});
		Localization.sFontAsset = null;
		Localization.sLocale = null;
		KPlayerPrefs.SetString(Localization.SELECTED_LANGUAGE_TYPE_KEY, Localization.SelectedLanguageType.None.ToString());
		KPlayerPrefs.SetString(Localization.SELECTED_LANGUAGE_CODE_KEY, "");
		Localization.SwapToLocalizedFont(Localization.GetDefaultLocale().FontName);
		string defaultLocalizationFilePath = Localization.GetDefaultLocalizationFilePath();
		if (File.Exists(defaultLocalizationFilePath))
		{
			Localization.LoadTranslation(File.ReadAllLines(defaultLocalizationFilePath, Encoding.UTF8), true);
		}
		LanguageOptionsScreen.CleanUpSavedLanguageMod();
	}

	// Token: 0x06006E0E RID: 28174 RVA: 0x002EDE74 File Offset: 0x002EC074
	private static string ReverseText(string source)
	{
		char[] separator = new char[]
		{
			'\n'
		};
		string[] array = source.Split(separator);
		string text = "";
		int num = 0;
		foreach (string text2 in array)
		{
			num++;
			char[] array3 = new char[text2.Length];
			for (int j = 0; j < text2.Length; j++)
			{
				array3[array3.Length - 1 - j] = text2[j];
			}
			text += new string(array3);
			if (num < array.Length)
			{
				text += "\n";
			}
		}
		return text;
	}

	// Token: 0x06006E0F RID: 28175 RVA: 0x000E816B File Offset: 0x000E636B
	public static string Fixup(string text)
	{
		if (Localization.sLocale != null && text != null && text != "" && Localization.sLocale.Lang == Localization.Language.Arabic)
		{
			return Localization.ReverseText(ArabicFixer.Fix(text));
		}
		return text;
	}

	// Token: 0x0400523A RID: 21050
	private static TMP_FontAsset sFontAsset = null;

	// Token: 0x0400523B RID: 21051
	private static readonly List<Localization.Locale> Locales = new List<Localization.Locale>
	{
		new Localization.Locale(Localization.Language.Chinese, Localization.Direction.LeftToRight, "zh", "NotoSansCJKsc-Regular"),
		new Localization.Locale(Localization.Language.Japanese, Localization.Direction.LeftToRight, "ja", "NotoSansCJKjp-Regular"),
		new Localization.Locale(Localization.Language.Korean, Localization.Direction.LeftToRight, "ko", "NotoSansCJKkr-Regular"),
		new Localization.Locale(Localization.Language.Russian, Localization.Direction.LeftToRight, "ru", "RobotoCondensed-Regular"),
		new Localization.Locale(Localization.Language.Thai, Localization.Direction.LeftToRight, "th", "NotoSansThai-Regular"),
		new Localization.Locale(Localization.Language.Arabic, Localization.Direction.RightToLeft, "ar", "NotoNaskhArabic-Regular"),
		new Localization.Locale(Localization.Language.Hebrew, Localization.Direction.RightToLeft, "he", "NotoSansHebrew-Regular"),
		new Localization.Locale(Localization.Language.Unspecified, Localization.Direction.LeftToRight, "", "RobotoCondensed-Regular")
	};

	// Token: 0x0400523C RID: 21052
	private static Localization.Locale sLocale = null;

	// Token: 0x0400523D RID: 21053
	private static string currentFontName = null;

	// Token: 0x0400523E RID: 21054
	public static string DEFAULT_LANGUAGE_CODE = "en";

	// Token: 0x0400523F RID: 21055
	public static readonly List<string> PreinstalledLanguages = new List<string>
	{
		Localization.DEFAULT_LANGUAGE_CODE,
		"zh_klei",
		"ko_klei",
		"ru_klei"
	};

	// Token: 0x04005240 RID: 21056
	public static string SELECTED_LANGUAGE_TYPE_KEY = "SelectedLanguageType";

	// Token: 0x04005241 RID: 21057
	public static string SELECTED_LANGUAGE_CODE_KEY = "SelectedLanguageCode";

	// Token: 0x04005242 RID: 21058
	private static Dictionary<string, List<Assembly>> translatable_assemblies = new Dictionary<string, List<Assembly>>();

	// Token: 0x04005243 RID: 21059
	public const BindingFlags non_static_data_member_fields = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy;

	// Token: 0x04005244 RID: 21060
	private const string start_link_token = "<link";

	// Token: 0x04005245 RID: 21061
	private const string end_link_token = "</link";

	// Token: 0x020014A1 RID: 5281
	public enum Language
	{
		// Token: 0x04005247 RID: 21063
		Chinese,
		// Token: 0x04005248 RID: 21064
		Japanese,
		// Token: 0x04005249 RID: 21065
		Korean,
		// Token: 0x0400524A RID: 21066
		Russian,
		// Token: 0x0400524B RID: 21067
		Thai,
		// Token: 0x0400524C RID: 21068
		Arabic,
		// Token: 0x0400524D RID: 21069
		Hebrew,
		// Token: 0x0400524E RID: 21070
		Unspecified
	}

	// Token: 0x020014A2 RID: 5282
	public enum Direction
	{
		// Token: 0x04005250 RID: 21072
		LeftToRight,
		// Token: 0x04005251 RID: 21073
		RightToLeft
	}

	// Token: 0x020014A3 RID: 5283
	public class Locale
	{
		// Token: 0x06006E11 RID: 28177 RVA: 0x000E819E File Offset: 0x000E639E
		public Locale(Localization.Locale other)
		{
			this.mLanguage = other.mLanguage;
			this.mDirection = other.mDirection;
			this.mCode = other.mCode;
			this.mFontName = other.mFontName;
		}

		// Token: 0x06006E12 RID: 28178 RVA: 0x000E81D6 File Offset: 0x000E63D6
		public Locale(Localization.Language language, Localization.Direction direction, string code, string fontName)
		{
			this.mLanguage = language;
			this.mDirection = direction;
			this.mCode = code.ToLower();
			this.mFontName = fontName;
		}

		// Token: 0x17000709 RID: 1801
		// (get) Token: 0x06006E13 RID: 28179 RVA: 0x000E8200 File Offset: 0x000E6400
		public Localization.Language Lang
		{
			get
			{
				return this.mLanguage;
			}
		}

		// Token: 0x06006E14 RID: 28180 RVA: 0x000E8208 File Offset: 0x000E6408
		public void SetCode(string code)
		{
			this.mCode = code;
		}

		// Token: 0x1700070A RID: 1802
		// (get) Token: 0x06006E15 RID: 28181 RVA: 0x000E8211 File Offset: 0x000E6411
		public string Code
		{
			get
			{
				return this.mCode;
			}
		}

		// Token: 0x1700070B RID: 1803
		// (get) Token: 0x06006E16 RID: 28182 RVA: 0x000E8219 File Offset: 0x000E6419
		public string FontName
		{
			get
			{
				return this.mFontName;
			}
		}

		// Token: 0x1700070C RID: 1804
		// (get) Token: 0x06006E17 RID: 28183 RVA: 0x000E8221 File Offset: 0x000E6421
		public bool IsRightToLeft
		{
			get
			{
				return this.mDirection == Localization.Direction.RightToLeft;
			}
		}

		// Token: 0x06006E18 RID: 28184 RVA: 0x000E822C File Offset: 0x000E642C
		public bool MatchesCode(string language_code)
		{
			return language_code.ToLower().Contains(this.mCode);
		}

		// Token: 0x06006E19 RID: 28185 RVA: 0x000E823F File Offset: 0x000E643F
		public bool MatchesFont(string fontname)
		{
			return fontname.ToLower() == this.mFontName.ToLower();
		}

		// Token: 0x06006E1A RID: 28186 RVA: 0x002EE058 File Offset: 0x002EC258
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				this.mCode,
				":",
				this.mLanguage.ToString(),
				":",
				this.mDirection.ToString(),
				":",
				this.mFontName
			});
		}

		// Token: 0x04005252 RID: 21074
		private Localization.Language mLanguage;

		// Token: 0x04005253 RID: 21075
		private string mCode;

		// Token: 0x04005254 RID: 21076
		private string mFontName;

		// Token: 0x04005255 RID: 21077
		private Localization.Direction mDirection;
	}

	// Token: 0x020014A4 RID: 5284
	private struct Entry
	{
		// Token: 0x1700070D RID: 1805
		// (get) Token: 0x06006E1B RID: 28187 RVA: 0x000E8257 File Offset: 0x000E6457
		public bool IsPopulated
		{
			get
			{
				return this.msgctxt != null && this.msgstr != null && this.msgstr.Length > 0;
			}
		}

		// Token: 0x04005256 RID: 21078
		public string msgctxt;

		// Token: 0x04005257 RID: 21079
		public string msgstr;
	}

	// Token: 0x020014A5 RID: 5285
	public enum SelectedLanguageType
	{
		// Token: 0x04005259 RID: 21081
		None,
		// Token: 0x0400525A RID: 21082
		Preinstalled,
		// Token: 0x0400525B RID: 21083
		UGC
	}
}
