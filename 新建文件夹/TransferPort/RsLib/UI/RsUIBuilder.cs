using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RsLib;

public delegate void ComponentInitDelegate<in T>(T c, RsUIBuilder r) where T : Component;

public class RsUIBuilder {
    public enum ActionType {
        ADD,
        GET,
        ADD_OR_GET
    }

    private          GameObject                 currentObject;
    private          GameObject                 currentParent;
    private readonly Stack<GameObject>          parentStack = new();
    private readonly Dictionary<string, object> reference   = new();
    private readonly List<Task>                 tasks       = new();

    public RsUIBuilder Parent(GameObject currentParent) {
        this.currentParent = currentParent;
        parentStack.Push(currentParent);
        return this;
    }

    public RsUIBuilder Go(string name = null, string referenceName = null) {
        currentObject = UIGameObject(name, currentParent);
        TryAddReference(referenceName, currentObject);
        return this;
    }

    public RsUIBuilder Go(GameObject gameObject) { return Go(null, gameObject); }

    public RsUIBuilder Go(string referenceName, GameObject gameObject) {
        currentObject = gameObject;
        currentObject.transform.SetParent(currentParent.transform);
        TryAddReference(referenceName, currentObject);
        return this;
    }

    public RsUIBuilder Go(Func<GameObject, GameObject> create) { return Go(null, create); }

    public RsUIBuilder Go(string referenceName, Func<GameObject, GameObject> create) {
        currentObject = create(currentParent);
        currentObject.transform.SetParent(currentParent.transform);
        TryAddReference(referenceName, currentObject);
        return this;
    }

    public RsUIBuilder Add<T>() where T : Component                                { return Add<T>(null, null); }
    public RsUIBuilder Add<T>(ComponentInitDelegate<T> create) where T : Component { return Add(null, create); }

    public RsUIBuilder Add<T>(string referenceName, ComponentInitDelegate<T> create) where T : Component {
        tasks.Add(new Task(currentObject, referenceName, typeof(T), create, ActionType.ADD));
        return this;
    }

    public RsUIBuilder AddOrGet<T>() where T : Component { return AddOrGet<T>(null); }

    public RsUIBuilder AddOrGet<T>(ComponentInitDelegate<T> create) where T : Component {
        return AddOrGet(null, create);
    }

    public RsUIBuilder AddOrGet<T>(string referenceName, ComponentInitDelegate<T> create) where T : Component {
        tasks.Add(new Task(currentObject, referenceName, typeof(T), create, ActionType.ADD_OR_GET));
        return this;
    }

    public RsUIBuilder Get<T>() where T : Component                                { return Get<T>(null); }
    public RsUIBuilder Get<T>(ComponentInitDelegate<T> create) where T : Component { return Get(null, create); }

    public RsUIBuilder Get<T>(string referenceName = null, ComponentInitDelegate<T> create = null) where T : Component {
        tasks.Add(new Task(currentObject, referenceName, typeof(T), create, ActionType.GET));
        return this;
    }

    public RsUIBuilder DebugColor(Color color) {
        Graphic graphic;
        if (!currentObject.TryGetComponent(out graphic)) graphic = currentObject.AddComponent<Image>();
        graphic.color = color;
        return this;
    }

    public void TryAddReference(string key, object obj) {
        if (key != null) reference[key] = obj;
    }

    public RsUIBuilder Child(Action<GameObject, RsUIBuilder> b) {
        parentStack.Push(currentObject);
        currentParent = currentObject;
        currentObject = null;
        b?.Invoke(currentParent, this);
        currentObject = parentStack.Pop();
        currentParent = parentStack.Count > 0 ? parentStack.Peek() : null;
        return this;
    }

    public T Reference<T>(string key) where T : class {
        if (reference.TryGetValue(key, out var a)) return a as T;

        return null;
    }

    public void Build() {
        foreach (var task in tasks) {
            var taskGameObject = task.gameObject;
            if (task.componentType != null) {
                Component component = null;
                if (task.actionType == ActionType.GET)
                    component = taskGameObject.GetComponent(task.componentType);
                else if (task.actionType == ActionType.ADD)
                    component = taskGameObject.AddComponent(task.componentType);
                else if (task.actionType == ActionType.ADD_OR_GET) {
                    component = taskGameObject.GetComponent(task.componentType);
                    if (component == null) component = taskGameObject.AddComponent(task.componentType);
                }

                if (task.referenceName != null) reference[task.referenceName] = component;

                task.instance = component;
            }
        }

        foreach (var task in tasks)
            if (task.create != null)
                task.create.DynamicInvoke(task.instance, this);
    }

    public GameObject CurrentObject() { return currentObject; }

    public void HorizontalLayout(bool expandWidth, bool expandHeight, bool controlWidth, bool controlHeight) {
        AddOrGet<HorizontalLayoutGroup>((c, u) => {
                                            c.childForceExpandWidth  = expandWidth;
                                            c.childForceExpandHeight = expandHeight;
                                            c.childControlWidth      = controlWidth;
                                            c.childControlHeight     = controlHeight;
                                        });
    }

    public void VerticalLayout(bool expandWidth, bool expandHeight, bool controlWidth, bool controlHeight) {
        AddOrGet<VerticalLayoutGroup>((c, u) => {
                                          c.childForceExpandWidth  = expandWidth;
                                          c.childForceExpandHeight = expandHeight;
                                          c.childControlWidth      = controlWidth;
                                          c.childControlHeight     = controlHeight;
                                      });
    }

    public static GameObject BlockLine(GameObject parent = null, float height = 5f) {
        var root = UIGameObject("BlockLine", parent);
        root.rectTransform().sizeDelta = new Vector2(height, height);
        var layoutElement = root.AddComponent<LayoutElement>();
        layoutElement.preferredHeight = height;
        layoutElement.minHeight       = height;
        return root;
    }

    public static GameObject UIGameObject(string name = "UIGameObject", GameObject parent = null, bool active = true) {
        var gameObject = new GameObject();
        gameObject.name  = name;
        gameObject.layer = LayerMask.NameToLayer("UI");
        if (parent != null) gameObject.transform.SetParent(parent.transform, false);
        gameObject.AddOrGet<RectTransform>();
        if (gameObject.activeSelf != active) gameObject.SetActive(active);
        return gameObject;
    }

    public static Image ImageGO(Sprite sprite = null, GameObject parent = null, bool setNativeSize = false) {
        var gameObject = UIGameObject("Image", parent);
        var image      = gameObject.AddComponent<Image>();
        image.sprite = sprite;
        if (setNativeSize) image.SetNativeSize();
        return image;
    }

    public static MultiToggle CheckBoxGO(GameObject parent = null) {
        var size = new Vector2(22, 22);
        var root = UIGameObject("CheckBox", parent, false);
        root.rectTransform().sizeDelta = size;

        var bg = ImageGO(RsUITuning.Images.CheckBorder, root);
        bg.name  = "BG";
        bg.color = Color.black;
        bg.rectTransform.FullParent();

        var check = ImageGO(RsUITuning.Images.Checked, root);
        bg.name = "check";
        check.rectTransform.FullParent();

        var layoutElement = root.AddComponent<LayoutElement>();
        layoutElement.preferredWidth  = size.x;
        layoutElement.preferredHeight = size.y;

        var multiToggle = root.AddComponent<MultiToggle>();
        multiToggle.toggle_image = check;

        multiToggle.states = new ToggleState[] {
            new() {
                Name                        = "off",
                sprite                      = null,
                additional_display_settings = new StatePresentationSetting[0],
                color                       = Color.clear
            },
            new() {
                Name                        = "on",
                sprite                      = RsUITuning.Images.Checked,
                additional_display_settings = new StatePresentationSetting[0],
                color                       = Color.black
            }
        };

        multiToggle.ChangeState(0);
        root.SetActive(true);
        return multiToggle;
    }

    public static LocText LocTextGo(LocString text, TextStyleSetting styleSetting = null, GameObject parent = null) {
        var root = UIGameObject("LocText", parent);
        root.SetActive(false);
        var locText = root.AddComponent<LocText>();
        locText.allowOverride    = false;
        locText.textStyleSetting = styleSetting != null ? styleSetting : RsUITuning.ScriptableObjects.style_labelText;
        locText.key              = text.key.IsValid() ? text.key.String : "";
        locText.text             = text.text;

        root.SetActive(true);
        root.rectTransform().sizeDelta = new Vector2(100, locText.textStyleSetting.fontSize);
        root.rectTransform().SetSizeWithPreferred();
        return locText;
    }

    public static LocText LocTextTooltipGo(LocString        text,
                                           string           tooltip,
                                           TextStyleSetting styleSetting = null,
                                           GameObject       parent       = null) {
        var root = UIGameObject("LocText", parent);
        root.SetActive(false);
        var locText = root.AddComponent<LocText>();
        locText.allowOverride    = false;
        locText.textStyleSetting = styleSetting != null ? styleSetting : RsUITuning.ScriptableObjects.style_labelText;
        locText.key              = text.key.IsValid() ? text.key.String : "";
        locText.text             = text.text;

        var cToolTip = root.AddComponent<ToolTip>();
        cToolTip.toolTip = tooltip;

        root.SetActive(true);
        root.rectTransform().sizeDelta = new Vector2(100, locText.textStyleSetting.fontSize);
        root.rectTransform().SetSizeWithPreferred();
        return locText;
    }

    public static GameObject LocTextCheckBoxGo(LocString text, GameObject parent = null) {
        var root = UIGameObject("LocTextCheckBox", parent, false);

        {
            var layoutGroup = root.AddComponent<HorizontalLayoutGroup>();
            layoutGroup.childForceExpandWidth  = false;
            layoutGroup.childForceExpandHeight = true;
            layoutGroup.childControlHeight     = true;
        }

        var toggleParent = UIGameObject("layout", root);

        {
            var layoutElement = toggleParent.AddComponent<LayoutElement>();
            layoutElement.minWidth  = 22;
            layoutElement.minHeight = 22;
        }

        var multiToggle = CheckBoxGO(toggleParent);

        {
            var toggleRect = multiToggle.rectTransform();
            toggleRect.anchorMin = new Vector2(0.5f, 0.5f);
            toggleRect.anchorMax = new Vector2(0.5f, 0.5f);
        }

        LocTextGo(text, null, root);
        root.SetActive(true);
        return root;
    }

    public static GameObject ToggleEntry(LocString                     text,
                                         ToolParameterMenu.ToggleState defaultState = ToolParameterMenu.ToggleState.Off,
                                         GameObject                    parent       = null) {
        var gameObject                      = Util.KInstantiateUI(RsUITuning.Prefabs.ToggleEntry, parent);
        var locText                         = gameObject.GetComponentInChildren<LocText>();
        if (text.key.IsValid()) locText.key = text.key.String;
        locText.text = text.text;

        var toggle = gameObject.GetComponentInChildren<MultiToggle>();
        switch (defaultState) {
            case ToolParameterMenu.ToggleState.On:
                toggle.ChangeState(1);
                break;
            case ToolParameterMenu.ToggleState.Disabled:
                toggle.ChangeState(2);
                break;
            default:
                toggle.ChangeState(0);
                break;
        }

        gameObject.SetActiveNR(true);
        return gameObject;
    }

    public static MultiToggle ToggleEntryToMultiToggle(LocString text,
                                                       ToolParameterMenu.ToggleState defaultState
                                                           = ToolParameterMenu.ToggleState.Off,
                                                       GameObject parent = null) {
        var gameObject                      = Util.KInstantiateUI(RsUITuning.Prefabs.ToggleEntry, parent);
        var locText                         = gameObject.GetComponentInChildren<LocText>();
        if (text.key.IsValid()) locText.key = text.key.String;
        locText.text = text.text;

        var toggle = gameObject.GetComponentInChildren<MultiToggle>();
        switch (defaultState) {
            case ToolParameterMenu.ToggleState.On:
                toggle.ChangeState(1);
                break;
            case ToolParameterMenu.ToggleState.Disabled:
                toggle.ChangeState(2);
                break;
            default:
                toggle.ChangeState(0);
                break;
        }

        gameObject.SetActiveNR(true);
        return toggle;
    }

    public class Task {
        public ActionType actionType;
        public Type       componentType;
        public Delegate   create;
        public GameObject gameObject;
        public Component  instance;
        public string     referenceName;

        public Task(GameObject gameObject,
                    string     referenceName,
                    Type       componentType,
                    Delegate   create,
                    ActionType actionType) {
            this.gameObject    = gameObject;
            this.referenceName = referenceName;
            this.componentType = componentType;
            this.create        = create;
            this.actionType    = actionType;
        }
    }
}