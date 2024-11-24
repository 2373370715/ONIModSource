using System;
using STRINGS;
using TMPro;
using UnityEngine;

public class SearchBar : KMonoBehaviour
{
	[SerializeField]
	protected KInputTextField inputField;

	[SerializeField]
	protected KButton clearButton;

	public Action<string> ValueChanged;

	public Action<bool> EditingStateChanged;

	public System.Action Focused;

	public string CurrentSearchValue
	{
		get
		{
			if (!string.IsNullOrEmpty(inputField.text))
			{
				return inputField.text;
			}
			return "";
		}
	}

	public bool IsInputFieldEmpty => inputField.text == "";

	public bool isEditing { get; protected set; }

	public virtual void SetPlaceholder(string text)
	{
		inputField.placeholder.GetComponent<TextMeshProUGUI>().text = text;
	}

	protected override void OnSpawn()
	{
		inputField.ActivateInputField();
		KInputTextField kInputTextField = inputField;
		kInputTextField.onFocus = (System.Action)Delegate.Combine(kInputTextField.onFocus, new System.Action(OnFocus));
		inputField.onEndEdit.AddListener(OnEndEdit);
		inputField.onValueChanged.AddListener(OnValueChanged);
		clearButton.onClick += ClearSearch;
		SetPlaceholder(UI.UISIDESCREENS.TREEFILTERABLESIDESCREEN.SEARCH_PLACEHOLDER);
	}

	protected void SetEditingState(bool editing)
	{
		isEditing = editing;
		EditingStateChanged?.Invoke(isEditing);
		KScreenManager.Instance.RefreshStack();
	}

	protected virtual void OnValueChanged(string value)
	{
		ValueChanged?.Invoke(value);
	}

	protected virtual void OnEndEdit(string value)
	{
		SetEditingState(editing: false);
	}

	protected virtual void OnFocus()
	{
		SetEditingState(editing: true);
		UISounds.PlaySound(UISounds.Sound.ClickHUD);
		Focused?.Invoke();
	}

	public virtual void ClearSearch()
	{
		SetValue("");
	}

	public void SetValue(string value)
	{
		inputField.text = value;
		ValueChanged?.Invoke(value);
	}
}
