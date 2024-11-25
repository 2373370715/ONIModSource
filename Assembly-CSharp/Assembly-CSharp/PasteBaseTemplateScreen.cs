using System.Collections.Generic;
using System.IO;
using Klei;
using ProcGen;
using STRINGS;
using UnityEngine;
using Path = System.IO.Path;

public class PasteBaseTemplateScreen : KScreen {
    public static           PasteBaseTemplateScreen Instance;
    private static readonly string                  NO_DIRECTORY = "NONE";
    public                  KButton                 button_directory_up;
    public                  GameObject              button_list_container;
    public                  LocText                 directory_path_text;
    private                 string                  m_CurrentDirectory = NO_DIRECTORY;
    private readonly        List<GameObject>        m_template_buttons = new List<GameObject>();
    public                  GameObject              prefab_directory_button;
    public                  GameObject              prefab_paste_button;

    protected override void OnPrefabInit() {
        base.OnPrefabInit();
        Instance = this;
        TemplateCache.Init();
        button_directory_up.onClick += UpDirectory;
        ConsumeMouseScroll          =  true;
        RefreshStampButtons();
    }

    protected override void OnForcedCleanUp() {
        Instance = null;
        base.OnForcedCleanUp();
    }

    [ContextMenu("Refresh")]
    public void RefreshStampButtons() {
        directory_path_text.text           = m_CurrentDirectory;
        button_directory_up.isInteractable = m_CurrentDirectory != NO_DIRECTORY;
        foreach (var obj in m_template_buttons) Destroy(obj);
        m_template_buttons.Clear();
        if (m_CurrentDirectory == NO_DIRECTORY) {
            directory_path_text.text = "";
            using (var enumerator2 = DlcManager.RELEASED_VERSIONS.GetEnumerator()) {
                while (enumerator2.MoveNext()) {
                    var dlcId = enumerator2.Current;
                    if (SaveLoader.Instance.IsDLCActiveForCurrentSave(dlcId)) {
                        var gameObject = Util.KInstantiateUI(prefab_directory_button, button_list_container, true);
                        gameObject.GetComponent<KButton>().onClick += delegate {
                                                                          UpdateDirectory(SettingsCache
                                                                              .GetScope(dlcId));
                                                                      };

                        gameObject.GetComponentInChildren<LocText>().text
                            = dlcId == ""
                                  ? UI.DEBUG_TOOLS.SAVE_BASE_TEMPLATE.BASE_GAME_FOLDER_NAME.text
                                  : SettingsCache.GetScope(dlcId);

                        m_template_buttons.Add(gameObject);
                    }
                }
            }

            return;
        }

        var path = TemplateCache.RewriteTemplatePath(m_CurrentDirectory);
        if (Directory.Exists(path)) {
            var directories = Directory.GetDirectories(path);
            for (var i = 0; i < directories.Length; i++) {
                var path2          = directories[i];
                var directory_name = Path.GetFileNameWithoutExtension(path2);
                var gameObject2    = Util.KInstantiateUI(prefab_directory_button, button_list_container, true);
                gameObject2.GetComponent<KButton>().onClick        += delegate { UpdateDirectory(directory_name); };
                gameObject2.GetComponentInChildren<LocText>().text =  directory_name;
                m_template_buttons.Add(gameObject2);
            }
        }

        var pooledList = ListPool<FileHandle, PasteBaseTemplateScreen>.Allocate();
        FileSystem.GetFiles(TemplateCache.RewriteTemplatePath(m_CurrentDirectory), "*.yaml", pooledList);
        foreach (var fileHandle in pooledList) {
            var file_path_no_extension = Path.GetFileNameWithoutExtension(fileHandle.full_path);
            var gameObject3 = Util.KInstantiateUI(prefab_paste_button, button_list_container, true);
            gameObject3.GetComponent<KButton>().onClick += delegate { OnClickPasteButton(file_path_no_extension); };
            gameObject3.GetComponentInChildren<LocText>().text = file_path_no_extension;
            m_template_buttons.Add(gameObject3);
        }
    }

    private void UpdateDirectory(string relativePath) {
        if (m_CurrentDirectory == NO_DIRECTORY) m_CurrentDirectory = "";
        m_CurrentDirectory = FileSystem.CombineAndNormalize(m_CurrentDirectory, relativePath);
        RefreshStampButtons();
    }

    private void UpDirectory() {
        var num = m_CurrentDirectory.LastIndexOf("/");
        if (num > 0)
            m_CurrentDirectory = m_CurrentDirectory.Substring(0, num);
        else {
            string dlcId;
            string str;
            SettingsCache.GetDlcIdAndPath(m_CurrentDirectory, out dlcId, out str);
            if (str.IsNullOrWhiteSpace())
                m_CurrentDirectory = NO_DIRECTORY;
            else
                m_CurrentDirectory = SettingsCache.GetScope(dlcId);
        }

        RefreshStampButtons();
    }

    private void OnClickPasteButton(string template_name) {
        if (template_name == null) return;

        var text = FileSystem.CombineAndNormalize(m_CurrentDirectory, template_name);
        DebugTool.Instance.DeactivateTool();
        DebugBaseTemplateButton.Instance.ClearSelection();
        DebugBaseTemplateButton.Instance.nameField.text = text;
        var template = TemplateCache.GetTemplate(text);
        StampTool.Instance.Activate(template, true);
    }
}