using System;
using System.Collections.Generic;
using Klei;
using ProcGen;

// Token: 0x0200120F RID: 4623
public static class TemplateCache
{
	// Token: 0x170005A5 RID: 1445
	// (get) Token: 0x06005E69 RID: 24169 RVA: 0x000DDAD3 File Offset: 0x000DBCD3
	// (set) Token: 0x06005E6A RID: 24170 RVA: 0x000DDADA File Offset: 0x000DBCDA
	public static bool Initted { get; private set; }

	// Token: 0x06005E6B RID: 24171 RVA: 0x000DDAE2 File Offset: 0x000DBCE2
	public static void Init()
	{
		if (TemplateCache.Initted)
		{
			return;
		}
		TemplateCache.templates = new Dictionary<string, TemplateContainer>();
		TemplateCache.Initted = true;
	}

	// Token: 0x06005E6C RID: 24172 RVA: 0x000DDAFC File Offset: 0x000DBCFC
	public static void Clear()
	{
		TemplateCache.templates = null;
		TemplateCache.Initted = false;
	}

	// Token: 0x06005E6D RID: 24173 RVA: 0x002A38EC File Offset: 0x002A1AEC
	public static string RewriteTemplatePath(string scopePath)
	{
		string dlcId;
		string str;
		SettingsCache.GetDlcIdAndPath(scopePath, out dlcId, out str);
		return SettingsCache.GetAbsoluteContentPath(dlcId, "templates/" + str);
	}

	// Token: 0x06005E6E RID: 24174 RVA: 0x000DDB0A File Offset: 0x000DBD0A
	public static string RewriteTemplateYaml(string scopePath)
	{
		return TemplateCache.RewriteTemplatePath(scopePath) + ".yaml";
	}

	// Token: 0x06005E6F RID: 24175 RVA: 0x002A3914 File Offset: 0x002A1B14
	public static TemplateContainer GetTemplate(string templatePath)
	{
		if (!TemplateCache.templates.ContainsKey(templatePath))
		{
			TemplateCache.templates.Add(templatePath, null);
		}
		if (TemplateCache.templates[templatePath] == null)
		{
			string text = TemplateCache.RewriteTemplateYaml(templatePath);
			TemplateContainer templateContainer = YamlIO.LoadFile<TemplateContainer>(text, null, null);
			if (templateContainer == null)
			{
				Debug.LogWarning("Missing template [" + text + "]");
			}
			templateContainer.name = templatePath;
			TemplateCache.templates[templatePath] = templateContainer;
		}
		return TemplateCache.templates[templatePath];
	}

	// Token: 0x06005E70 RID: 24176 RVA: 0x000DDB1C File Offset: 0x000DBD1C
	public static bool TemplateExists(string templatePath)
	{
		return FileSystem.FileExists(TemplateCache.RewriteTemplateYaml(templatePath));
	}

	// Token: 0x040042E3 RID: 17123
	private const string defaultAssetFolder = "bases";

	// Token: 0x040042E4 RID: 17124
	private static Dictionary<string, TemplateContainer> templates;
}
