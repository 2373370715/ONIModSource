using System;
using STRINGS;
using TMPro;
using UnityEngine;

public class SearchBar : KMonoBehaviour {
    [SerializeField]
    protected KButton clearButton;

    public Action<bool>  EditingStateChanged;
    public System.Action Focused;

    [SerializeField]
    protected KInputTextField inputField;

    public Action<string> ValueChanged;

    public string CurrentSearchValue {
        get {
            if (!string.IsNullOrEmpty(inputField.text)) return inputField.text;

            return "";
        }
    }

    public bool IsInputFieldEmpty => inputField.text == "";
    public bool isEditing         { get; protected set; }

    public virtual void SetPlaceholder(string text) {
        inputField.placeholder.GetComponent<TextMeshProUGUI>().text = text;
    }

    protected override void OnSpawn() {
        inputField.ActivateInputField();
        var kinputTextField = inputField;
        kinputTextField.onFocus = (System.Action)Delegate.Combine(kinputTextField.onFocus, new System.Action(OnFocus));
        inputField.onEndEdit.AddListener(OnEndEdit);
        inputField.onValueChanged.AddListener(OnValueChanged);
        clearButton.onClick += ClearSearch;
        SetPlaceholder(UI.UISIDESCREENS.TREEFILTERABLESIDESCREEN.SEARCH_PLACEHOLDER);
    }

    protected void SetEditingState(bool editing) {
        isEditing = editing;
        var editingStateChanged = EditingStateChanged;
        if (editingStateChanged != null) editingStateChanged(isEditing);
        KScreenManager.Instance.RefreshStack();
    }

    protected virtual void OnValueChanged(string value) {
        var valueChanged = ValueChanged;
        if (valueChanged == null) return;

        valueChanged(value);
    }

    protected virtual void OnEndEdit(string value) { SetEditingState(false); }

    protected virtual void OnFocus() {
        SetEditingState(true);
        UISounds.PlaySound(UISounds.Sound.ClickHUD);
        var focused = Focused;
        if (focused == null) return;

        focused();
    }

    public virtual void ClearSearch() { SetValue(""); }

    public void SetValue(string value) {
        inputField.text = value;
        var valueChanged = ValueChanged;
        if (valueChanged == null) return;

        valueChanged(value);
    }
}