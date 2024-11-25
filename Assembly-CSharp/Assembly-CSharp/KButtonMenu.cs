using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class KButtonMenu : KScreen {
    private static readonly EventSystem.IntraObjectHandler<KButtonMenu> OnSetActivatorDelegate
        = new EventSystem.IntraObjectHandler<KButtonMenu>(delegate(KButtonMenu component, object data) {
                                                              component.OnSetActivator(data);
                                                          });

    [NonSerialized]
    public GameObject[] buttonObjects;

    [SerializeField]
    protected Transform buttonParent;

    public    GameObject        buttonPrefab;
    protected IList<ButtonInfo> buttons;

    [SerializeField]
    protected bool followGameObject;

    protected GameObject go;

    [SerializeField]
    protected bool keepMenuOpen;

    public bool ShouldConsumeMouseScroll;

    protected override void OnActivate() {
        ConsumeMouseScroll = ShouldConsumeMouseScroll;
        RefreshButtons();
    }

    public void SetButtons(IList<ButtonInfo> buttons) {
        this.buttons = buttons;
        if (activateOnSpawn) RefreshButtons();
    }

    public virtual void RefreshButtons() {
        if (buttonObjects != null) {
            for (var i = 0; i < buttonObjects.Length; i++) Destroy(buttonObjects[i]);
            buttonObjects = null;
        }

        if (buttons == null) return;

        buttonObjects = new GameObject[buttons.Count];
        for (var j = 0; j < buttons.Count; j++) {
            var binfo      = buttons[j];
            var gameObject = Instantiate(buttonPrefab, Vector3.zero, Quaternion.identity);
            buttonObjects[j] = gameObject;
            var parent = buttonParent != null ? buttonParent : transform;
            gameObject.transform.SetParent(parent, false);
            gameObject.SetActive(true);
            gameObject.name = binfo.text + "Button";
            var componentsInChildren = gameObject.GetComponentsInChildren<LocText>(true);
            if (componentsInChildren != null)
                foreach (var locText in componentsInChildren) {
                    locText.text  = locText.name == "Hotkey" ? GameUtil.GetActionString(binfo.shortcutKey) : binfo.text;
                    locText.color = binfo.isEnabled ? new Color(1f, 1f, 1f) : new Color(0.5f, 0.5f, 0.5f);
                }

            var componentInChildren = gameObject.GetComponentInChildren<ToolTip>();
            if (binfo.toolTip != null && binfo.toolTip != "" && componentInChildren != null)
                componentInChildren.toolTip = binfo.toolTip;

            var screen = this;
            var button = gameObject.GetComponent<KButton>();
            button.isInteractable = binfo.isEnabled;
            if (binfo.popupOptions == null && binfo.onPopulatePopup == null) {
                var onClick = binfo.onClick;
                System.Action value = delegate {
                                          onClick();
                                          if (!keepMenuOpen && screen != null) screen.Deactivate();
                                      };

                button.onClick += value;
            } else
                button.onClick += delegate { SetupPopupMenu(binfo, button); };

            binfo.uibutton = button;
            var onHover = binfo.onHover;
        }

        Update();
    }

    protected Button.ButtonClickedEvent SetupPopupMenu(ButtonInfo binfo, KButton button) {
        var buttonClickedEvent = new Button.ButtonClickedEvent();
        UnityAction unityAction = delegate {
                                      var list                                              = new List<ButtonInfo>();
                                      if (binfo.onPopulatePopup != null) binfo.popupOptions = binfo.onPopulatePopup();
                                      var popupOptions                                      = binfo.popupOptions;
                                      for (var i = 0; i < popupOptions.Length; i++) {
                                          var delegate_str2 = popupOptions[i];
                                          var delegate_str  = delegate_str2;
                                          list.Add(new ButtonInfo(delegate_str,
                                                                  delegate {
                                                                      binfo.onPopupClick(delegate_str);
                                                                      if (!keepMenuOpen) Deactivate();
                                                                  }));
                                      }

                                      var component = Util.KInstantiate(ScreenPrefabs.Instance.ButtonGrid.gameObject)
                                                          .GetComponent<KButtonMenu>();

                                      component.SetButtons(list.ToArray());
                                      RootMenu.Instance.AddSubMenu(component);
                                      Game.Instance.LocalPlayer.ScreenManager.ActivateScreen(component.gameObject);
                                      var b = default(Vector3);
                                      if (Util.IsOnLeftSideOfScreen(button.transform.GetPosition()))
                                          b.x = button.GetComponent<RectTransform>().rect.width * 0.25f;
                                      else
                                          b.x = -button.GetComponent<RectTransform>().rect.width * 0.25f;

                                      component.transform.SetPosition(button.transform.GetPosition() + b);
                                  };

        binfo.onClick = unityAction;
        buttonClickedEvent.AddListener(unityAction);
        return buttonClickedEvent;
    }

    public override void OnKeyDown(KButtonEvent e) {
        if (buttons == null) return;

        for (var i = 0; i < buttons.Count; i++) {
            var buttonInfo = buttons[i];
            if (e.TryConsume(buttonInfo.shortcutKey)) {
                buttonObjects[i].GetComponent<KButton>().PlayPointerDownSound();
                buttonObjects[i].GetComponent<KButton>().SignalClick(KKeyCode.Mouse0);
                break;
            }
        }

        base.OnKeyDown(e);
    }

    protected override void OnPrefabInit() { Subscribe(315865555, OnSetActivatorDelegate); }

    private void OnSetActivator(object data) {
        go = (GameObject)data;
        Update();
    }

    protected override void OnDeactivate() { }

    private void Update() {
        if (!followGameObject || go == null || canvas == null) return;

        var vector     = Camera.main.WorldToViewportPoint(go.transform.GetPosition());
        var component  = GetComponent<RectTransform>();
        var component2 = canvas.GetComponent<RectTransform>();
        if (component != null)
            component.anchoredPosition = new Vector2(vector.x * component2.sizeDelta.x - component2.sizeDelta.x * 0.5f,
                                                     vector.y * component2.sizeDelta.y - component2.sizeDelta.y * 0.5f);
    }

    public class ButtonInfo {
        public delegate void Callback();

        public delegate void HoverCallback(GameObject hoverTarget);

        public FMODAsset      clickSound;
        public bool           isEnabled = true;
        public UnityAction    onClick;
        public HoverCallback  onHover;
        public Func<string[]> onPopulatePopup;
        public Action<string> onPopupClick;
        public string[]       popupOptions;
        public Action         shortcutKey;
        public string         text;
        public string         toolTip;
        public KButton        uibutton;
        public object         userData;
        public GameObject     visualizer;

        public ButtonInfo(string         text              = null,
                          UnityAction    on_click          = null,
                          Action         shortcut_key      = Action.NumActions,
                          HoverCallback  on_hover          = null,
                          string         tool_tip          = null,
                          GameObject     visualizer        = null,
                          bool           is_enabled        = true,
                          string[]       popup_options     = null,
                          Action<string> on_popup_click    = null,
                          Func<string[]> on_populate_popup = null) {
            this.text       = text;
            shortcutKey     = shortcut_key;
            onClick         = on_click;
            onHover         = on_hover;
            this.visualizer = visualizer;
            toolTip         = tool_tip;
            isEnabled       = is_enabled;
            uibutton        = null;
            popupOptions    = popup_options;
            onPopupClick    = on_popup_click;
            onPopulatePopup = on_populate_popup;
        }

        public ButtonInfo(string        text,
                          Action        shortcutKey,
                          UnityAction   onClick,
                          HoverCallback onHover  = null,
                          object        userData = null) {
            this.text        = text;
            this.shortcutKey = shortcutKey;
            this.onClick     = onClick;
            this.onHover     = onHover;
            this.userData    = userData;
            visualizer       = null;
            uibutton         = null;
        }

        public ButtonInfo(string        text,
                          GameObject    visualizer,
                          Action        shortcutKey,
                          UnityAction   onClick,
                          HoverCallback onHover  = null,
                          object        userData = null) {
            this.text        = text;
            this.shortcutKey = shortcutKey;
            this.onClick     = onClick;
            this.onHover     = onHover;
            this.visualizer  = visualizer;
            this.userData    = userData;
            uibutton         = null;
        }
    }
}