using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using STRINGS;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputBindingsScreen : KModalScreen {
    private const           string              ROOT_KEY = "STRINGS.INPUT_BINDINGS.";
    private static readonly KeyCode[]           validKeys;
    private                 Action              actionToRebind = Action.NumActions;
    private                 KButton             activeButton;
    private                 int                 activeScreen = -1;
    public                  KButton             backButton;
    public                  KButton             closeButton;
    private                 ConfirmDialogScreen confirmDialog;

    [SerializeField]
    private ConfirmDialogScreen confirmPrefab;

    private UIPool<HorizontalLayoutGroup> entryPool;

    [SerializeField]
    private GameObject entryPrefab;

    private bool    ignoreRootConflicts;
    public  KButton nextScreenButton;

    [SerializeField]
    private OptionsMenuScreen optionsScreen;

    [SerializeField]
    private GameObject parent;

    public           KButton      prevScreenButton;
    public           KButton      resetButton;
    private readonly List<string> screens = new List<string>();

    [SerializeField]
    private LocText screenTitle;

    private bool waitingForKeyPress;

    // Note: this type is marked as 'beforefieldinit'.
    static InputBindingsScreen() {
        var array = new KeyCode[111];
        RuntimeHelpers.InitializeArray(array,
                                       fieldof( <
                                       PrivateImplementationDetails >
                                       .4522A529DBF1D30936B6BCC06D2E607CD76E3B0FB1C18D9DA2635843A2840CD7)
                      .FieldHandle);
        validKeys = array;
    }

    public override bool IsModal()                   { return true; }
    private         bool IsKeyDown(KeyCode key_code) { return Input.GetKey(key_code) || Input.GetKeyDown(key_code); }

    private string GetModifierString(Modifier modifiers) {
        var text = "";
        foreach (var obj in Enum.GetValues(typeof(Modifier))) {
            var modifier                                      = (Modifier)obj;
            if ((modifiers & modifier) != Modifier.None) text = text + " + " + modifier;
        }

        return text;
    }

    protected override void OnSpawn() {
        base.OnSpawn();
        entryPrefab.SetActive(false);
        prevScreenButton.onClick += OnPrevScreen;
        nextScreenButton.onClick += OnNextScreen;
    }

    protected override void OnActivate() {
        CollectScreens();
        var text = screens[activeScreen];
        var key  = "STRINGS.INPUT_BINDINGS." + text.ToUpper() + ".NAME";
        screenTitle.text    =  Strings.Get(key);
        closeButton.onClick += OnBack;
        backButton.onClick  += OnBack;
        resetButton.onClick += OnReset;
        BuildDisplay();
    }

    private void CollectScreens() {
        screens.Clear();
        for (var i = 0; i < GameInputMapping.KeyBindings.Length; i++) {
            var bindingEntry = GameInputMapping.KeyBindings[i];
            if (bindingEntry.mGroup != null            &&
                bindingEntry.mRebindable               &&
                !screens.Contains(bindingEntry.mGroup) &&
                DlcManager.IsAllContentSubscribed(bindingEntry.dlcIds)) {
                if (bindingEntry.mGroup == "Root") activeScreen = screens.Count;
                screens.Add(bindingEntry.mGroup);
            }
        }
    }

    protected override void OnDeactivate() {
        GameInputMapping.SaveBindings();
        DestroyDisplay();
    }

    private LocString GetActionString(Action action) { return null; }

    private string GetBindingText(BindingEntry binding) {
        var text = GameUtil.GetKeycodeLocalized(binding.mKeyCode);
        if (binding.mKeyCode != KKeyCode.LeftAlt      &&
            binding.mKeyCode != KKeyCode.RightAlt     &&
            binding.mKeyCode != KKeyCode.LeftControl  &&
            binding.mKeyCode != KKeyCode.RightControl &&
            binding.mKeyCode != KKeyCode.LeftShift    &&
            binding.mKeyCode != KKeyCode.RightShift)
            text += GetModifierString(binding.mModifier);

        return text;
    }

    private void BuildDisplay() {
        var text = screens[activeScreen];
        var key  = "STRINGS.INPUT_BINDINGS." + text.ToUpper() + ".NAME";
        screenTitle.text = Strings.Get(key);
        if (entryPool == null)
            entryPool = new UIPool<HorizontalLayoutGroup>(entryPrefab.GetComponent<HorizontalLayoutGroup>());

        DestroyDisplay();
        var num = 0;
        for (var i = 0; i < GameInputMapping.KeyBindings.Length; i++) {
            var binding = GameInputMapping.KeyBindings[i];
            if (binding.mGroup == screens[activeScreen] &&
                binding.mRebindable                     &&
                DlcManager.IsAllContentSubscribed(binding.dlcIds)) {
                var      gameObject          = entryPool.GetFreeElement(parent, true).gameObject;
                TMP_Text componentInChildren = gameObject.transform.GetChild(0).GetComponentInChildren<LocText>();
                var key2 = "STRINGS.INPUT_BINDINGS." +
                           binding.mGroup.ToUpper()  +
                           "."                       +
                           binding.mAction.ToString().ToUpper();

                componentInChildren.text = Strings.Get(key2);
                var key_label = gameObject.transform.GetChild(1).GetComponentInChildren<LocText>();
                key_label.text = GetBindingText(binding);
                var button = gameObject.GetComponentInChildren<KButton>();
                button.onClick += delegate {
                                      waitingForKeyPress  = true;
                                      actionToRebind      = binding.mAction;
                                      ignoreRootConflicts = binding.mIgnoreRootConflics;
                                      activeButton        = button;
                                      key_label.text      = UI.FRONTEND.INPUT_BINDINGS_SCREEN.WAITING_FOR_INPUT;
                                  };

                gameObject.transform.SetSiblingIndex(num);
                num++;
            }
        }
    }

    private void DestroyDisplay() { entryPool.ClearAll(); }

    private void Update() {
        if (waitingForKeyPress) {
            var modifier = Modifier.None;
            modifier |= IsKeyDown(KeyCode.LeftAlt) || IsKeyDown(KeyCode.RightAlt) ? Modifier.Alt : Modifier.None;
            modifier |= IsKeyDown(KeyCode.LeftControl) || IsKeyDown(KeyCode.RightControl)
                            ? Modifier.Ctrl
                            : Modifier.None;

            modifier |= IsKeyDown(KeyCode.LeftShift) || IsKeyDown(KeyCode.RightShift) ? Modifier.Shift : Modifier.None;
            modifier |= IsKeyDown(KeyCode.CapsLock) ? Modifier.CapsLock : Modifier.None;
            modifier |= IsKeyDown(KeyCode.BackQuote) ? Modifier.Backtick : Modifier.None;
            var flag = false;
            for (var i = 0; i < validKeys.Length; i++) {
                var keyCode = validKeys[i];
                if (Input.GetKeyDown(keyCode)) {
                    var kkey_code = (KKeyCode)keyCode;
                    Bind(kkey_code, modifier);
                    flag = true;
                }
            }

            if (!flag) {
                var axis     = Input.GetAxis("Mouse ScrollWheel");
                var kkeyCode = KKeyCode.None;
                if (axis < 0f)
                    kkeyCode                 = KKeyCode.MouseScrollDown;
                else if (axis > 0f) kkeyCode = KKeyCode.MouseScrollUp;

                if (kkeyCode != KKeyCode.None) Bind(kkeyCode, modifier);
            }
        }
    }

    private BindingEntry GetDuplicatedBinding(string activeScreen, BindingEntry new_binding) {
        var result = default(BindingEntry);
        for (var i = 0; i < GameInputMapping.KeyBindings.Length; i++) {
            var bindingEntry = GameInputMapping.KeyBindings[i];
            if (new_binding.IsBindingEqual(bindingEntry) &&
                (bindingEntry.mGroup == null         ||
                 bindingEntry.mGroup == activeScreen ||
                 bindingEntry.mGroup == "Root"       ||
                 activeScreen        == "Root")                                         &&
                (!(activeScreen        == "Root") || !bindingEntry.mIgnoreRootConflics) &&
                (!(bindingEntry.mGroup == "Root") || !new_binding.mIgnoreRootConflics)) {
                result = bindingEntry;
                break;
            }
        }

        return result;
    }

    public override void OnKeyDown(KButtonEvent e) {
        if (waitingForKeyPress) {
            e.Consumed = true;
            return;
        }

        if (e.TryConsume(Action.Escape) || e.TryConsume(Action.MouseRight)) {
            Deactivate();
            return;
        }

        base.OnKeyDown(e);
    }

    public override void OnKeyUp(KButtonEvent e) { e.Consumed = true; }

    private void OnBack() {
        var num = NumUnboundActions();
        if (num == 0) {
            Deactivate();
            return;
        }

        string text;
        if (num == 1) {
            var firstUnbound = GetFirstUnbound();
            text = string.Format(UI.FRONTEND.INPUT_BINDINGS_SCREEN.UNBOUND_ACTION, firstUnbound.mAction.ToString());
        } else
            text = UI.FRONTEND.INPUT_BINDINGS_SCREEN.MULTIPLE_UNBOUND_ACTIONS;

        confirmDialog = Util.KInstantiateUI(confirmPrefab.gameObject, transform.gameObject)
                            .GetComponent<ConfirmDialogScreen>();

        confirmDialog.PopupConfirmDialog(text, delegate { Deactivate(); }, delegate { confirmDialog.Deactivate(); });
        confirmDialog.gameObject.SetActive(true);
    }

    private int NumUnboundActions() {
        var num = 0;
        for (var i = 0; i < GameInputMapping.KeyBindings.Length; i++) {
            var bindingEntry = GameInputMapping.KeyBindings[i];
            if (bindingEntry.mKeyCode == KKeyCode.None &&
                bindingEntry.mRebindable               &&
                (BuildMenu.UseHotkeyBuildMenu() || !bindingEntry.mIgnoreRootConflics))
                num++;
        }

        return num;
    }

    private BindingEntry GetFirstUnbound() {
        var result = default(BindingEntry);
        for (var i = 0; i < GameInputMapping.KeyBindings.Length; i++) {
            var bindingEntry = GameInputMapping.KeyBindings[i];
            if (bindingEntry.mKeyCode == KKeyCode.None) {
                result = bindingEntry;
                break;
            }
        }

        return result;
    }

    private void OnReset() {
        GameInputMapping.KeyBindings = (BindingEntry[])GameInputMapping.DefaultBindings.Clone();
        Global.GetInputManager().RebindControls();
        BuildDisplay();
    }

    public void OnPrevScreen() {
        if (activeScreen > 0)
            activeScreen--;
        else
            activeScreen = screens.Count - 1;

        BuildDisplay();
    }

    public void OnNextScreen() {
        if (activeScreen < screens.Count - 1)
            activeScreen++;
        else
            activeScreen = 0;

        BuildDisplay();
    }

    private void Bind(KKeyCode kkey_code, Modifier modifier) {
        var bindingEntry = new BindingEntry(screens[activeScreen],
                                            GamepadButton.NumButtons,
                                            kkey_code,
                                            modifier,
                                            actionToRebind,
                                            true,
                                            ignoreRootConflicts);

        for (var i = 0; i < GameInputMapping.KeyBindings.Length; i++) {
            var bindingEntry2 = GameInputMapping.KeyBindings[i];
            if (bindingEntry2.mRebindable && bindingEntry2.mAction == actionToRebind) {
                var duplicatedBinding = GetDuplicatedBinding(screens[activeScreen], bindingEntry);
                bindingEntry.mButton                                = GameInputMapping.KeyBindings[i].mButton;
                GameInputMapping.KeyBindings[i]                     = bindingEntry;
                activeButton.GetComponentInChildren<LocText>().text = GetBindingText(bindingEntry);
                if (duplicatedBinding.mAction != Action.Invalid && duplicatedBinding.mAction != actionToRebind) {
                    confirmDialog = Util.KInstantiateUI(confirmPrefab.gameObject, transform.gameObject)
                                        .GetComponent<ConfirmDialogScreen>();

                    string arg = Strings.Get("STRINGS.INPUT_BINDINGS."          +
                                             duplicatedBinding.mGroup.ToUpper() +
                                             "."                                +
                                             duplicatedBinding.mAction.ToString().ToUpper());

                    var bindingText = GetBindingText(duplicatedBinding);
                    var text        = string.Format(UI.FRONTEND.INPUT_BINDINGS_SCREEN.DUPLICATE, arg, bindingText);
                    Unbind(duplicatedBinding.mAction);
                    confirmDialog.PopupConfirmDialog(text, null, null);
                    confirmDialog.gameObject.SetActive(true);
                }

                Global.GetInputManager().RebindControls();
                waitingForKeyPress = false;
                actionToRebind     = Action.NumActions;
                activeButton       = null;
                BuildDisplay();
                return;
            }
        }
    }

    private void Unbind(Action action) {
        for (var i = 0; i < GameInputMapping.KeyBindings.Length; i++) {
            var bindingEntry = GameInputMapping.KeyBindings[i];
            if (bindingEntry.mAction == action) {
                bindingEntry.mKeyCode           = KKeyCode.None;
                bindingEntry.mModifier          = Modifier.None;
                GameInputMapping.KeyBindings[i] = bindingEntry;
            }
        }
    }
}