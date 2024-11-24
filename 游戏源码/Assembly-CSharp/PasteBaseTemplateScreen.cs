using System;
using System.Collections.Generic;
using System.IO;
using Klei;
using ProcGen;
using STRINGS;
using UnityEngine;

// Token: 0x02001E93 RID: 7827
public class PasteBaseTemplateScreen : KScreen
{
	// Token: 0x0600A422 RID: 42018 RVA: 0x0010A7D1 File Offset: 0x001089D1
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		PasteBaseTemplateScreen.Instance = this;
		TemplateCache.Init();
		this.button_directory_up.onClick += this.UpDirectory;
		base.ConsumeMouseScroll = true;
		this.RefreshStampButtons();
	}

	// Token: 0x0600A423 RID: 42019 RVA: 0x0010A808 File Offset: 0x00108A08
	protected override void OnForcedCleanUp()
	{
		PasteBaseTemplateScreen.Instance = null;
		base.OnForcedCleanUp();
	}

	// Token: 0x0600A424 RID: 42020 RVA: 0x003E5068 File Offset: 0x003E3268
	[ContextMenu("Refresh")]
	public void RefreshStampButtons()
	{
		this.directory_path_text.text = this.m_CurrentDirectory;
		this.button_directory_up.isInteractable = (this.m_CurrentDirectory != PasteBaseTemplateScreen.NO_DIRECTORY);
		foreach (GameObject obj in this.m_template_buttons)
		{
			UnityEngine.Object.Destroy(obj);
		}
		this.m_template_buttons.Clear();
		if (this.m_CurrentDirectory == PasteBaseTemplateScreen.NO_DIRECTORY)
		{
			this.directory_path_text.text = "";
			using (List<string>.Enumerator enumerator2 = DlcManager.RELEASED_VERSIONS.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					string dlcId = enumerator2.Current;
					if (SaveLoader.Instance.IsDLCActiveForCurrentSave(dlcId))
					{
						GameObject gameObject = global::Util.KInstantiateUI(this.prefab_directory_button, this.button_list_container, true);
						gameObject.GetComponent<KButton>().onClick += delegate()
						{
							this.UpdateDirectory(SettingsCache.GetScope(dlcId));
						};
						gameObject.GetComponentInChildren<LocText>().text = ((dlcId == "") ? UI.DEBUG_TOOLS.SAVE_BASE_TEMPLATE.BASE_GAME_FOLDER_NAME.text : SettingsCache.GetScope(dlcId));
						this.m_template_buttons.Add(gameObject);
					}
				}
			}
			return;
		}
		string path = TemplateCache.RewriteTemplatePath(this.m_CurrentDirectory);
		if (Directory.Exists(path))
		{
			string[] directories = Directory.GetDirectories(path);
			for (int i = 0; i < directories.Length; i++)
			{
				string path2 = directories[i];
				string directory_name = System.IO.Path.GetFileNameWithoutExtension(path2);
				GameObject gameObject2 = global::Util.KInstantiateUI(this.prefab_directory_button, this.button_list_container, true);
				gameObject2.GetComponent<KButton>().onClick += delegate()
				{
					this.UpdateDirectory(directory_name);
				};
				gameObject2.GetComponentInChildren<LocText>().text = directory_name;
				this.m_template_buttons.Add(gameObject2);
			}
		}
		ListPool<FileHandle, PasteBaseTemplateScreen>.PooledList pooledList = ListPool<FileHandle, PasteBaseTemplateScreen>.Allocate();
		FileSystem.GetFiles(TemplateCache.RewriteTemplatePath(this.m_CurrentDirectory), "*.yaml", pooledList);
		foreach (FileHandle fileHandle in pooledList)
		{
			string file_path_no_extension = System.IO.Path.GetFileNameWithoutExtension(fileHandle.full_path);
			GameObject gameObject3 = global::Util.KInstantiateUI(this.prefab_paste_button, this.button_list_container, true);
			gameObject3.GetComponent<KButton>().onClick += delegate()
			{
				this.OnClickPasteButton(file_path_no_extension);
			};
			gameObject3.GetComponentInChildren<LocText>().text = file_path_no_extension;
			this.m_template_buttons.Add(gameObject3);
		}
	}

	// Token: 0x0600A425 RID: 42021 RVA: 0x003E5358 File Offset: 0x003E3558
	private void UpdateDirectory(string relativePath)
	{
		if (this.m_CurrentDirectory == PasteBaseTemplateScreen.NO_DIRECTORY)
		{
			this.m_CurrentDirectory = "";
		}
		this.m_CurrentDirectory = FileSystem.CombineAndNormalize(new string[]
		{
			this.m_CurrentDirectory,
			relativePath
		});
		this.RefreshStampButtons();
	}

	// Token: 0x0600A426 RID: 42022 RVA: 0x003E53A8 File Offset: 0x003E35A8
	private void UpDirectory()
	{
		int num = this.m_CurrentDirectory.LastIndexOf("/");
		if (num > 0)
		{
			this.m_CurrentDirectory = this.m_CurrentDirectory.Substring(0, num);
		}
		else
		{
			string dlcId;
			string str;
			SettingsCache.GetDlcIdAndPath(this.m_CurrentDirectory, out dlcId, out str);
			if (str.IsNullOrWhiteSpace())
			{
				this.m_CurrentDirectory = PasteBaseTemplateScreen.NO_DIRECTORY;
			}
			else
			{
				this.m_CurrentDirectory = SettingsCache.GetScope(dlcId);
			}
		}
		this.RefreshStampButtons();
	}

	// Token: 0x0600A427 RID: 42023 RVA: 0x003E5418 File Offset: 0x003E3618
	private void OnClickPasteButton(string template_name)
	{
		if (template_name == null)
		{
			return;
		}
		string text = FileSystem.CombineAndNormalize(new string[]
		{
			this.m_CurrentDirectory,
			template_name
		});
		DebugTool.Instance.DeactivateTool(null);
		DebugBaseTemplateButton.Instance.ClearSelection();
		DebugBaseTemplateButton.Instance.nameField.text = text;
		TemplateContainer template = TemplateCache.GetTemplate(text);
		StampTool.Instance.Activate(template, true, false);
	}

	// Token: 0x0400804A RID: 32842
	public static PasteBaseTemplateScreen Instance;

	// Token: 0x0400804B RID: 32843
	public GameObject button_list_container;

	// Token: 0x0400804C RID: 32844
	public GameObject prefab_paste_button;

	// Token: 0x0400804D RID: 32845
	public GameObject prefab_directory_button;

	// Token: 0x0400804E RID: 32846
	public KButton button_directory_up;

	// Token: 0x0400804F RID: 32847
	public LocText directory_path_text;

	// Token: 0x04008050 RID: 32848
	private List<GameObject> m_template_buttons = new List<GameObject>();

	// Token: 0x04008051 RID: 32849
	private static readonly string NO_DIRECTORY = "NONE";

	// Token: 0x04008052 RID: 32850
	private string m_CurrentDirectory = PasteBaseTemplateScreen.NO_DIRECTORY;
}
