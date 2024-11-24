using System;
using STRINGS;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02001FC1 RID: 8129
public class SearchBar : KMonoBehaviour
{
	// Token: 0x17000AFC RID: 2812
	// (get) Token: 0x0600ABF2 RID: 44018 RVA: 0x0010FBBF File Offset: 0x0010DDBF
	public string CurrentSearchValue
	{
		get
		{
			if (!string.IsNullOrEmpty(this.inputField.text))
			{
				return this.inputField.text;
			}
			return "";
		}
	}

	// Token: 0x17000AFD RID: 2813
	// (get) Token: 0x0600ABF3 RID: 44019 RVA: 0x0010FBE4 File Offset: 0x0010DDE4
	public bool IsInputFieldEmpty
	{
		get
		{
			return this.inputField.text == "";
		}
	}

	// Token: 0x17000AFE RID: 2814
	// (get) Token: 0x0600ABF5 RID: 44021 RVA: 0x0010FC04 File Offset: 0x0010DE04
	// (set) Token: 0x0600ABF4 RID: 44020 RVA: 0x0010FBFB File Offset: 0x0010DDFB
	public bool isEditing { get; protected set; }

	// Token: 0x0600ABF6 RID: 44022 RVA: 0x0010FC0C File Offset: 0x0010DE0C
	public virtual void SetPlaceholder(string text)
	{
		this.inputField.placeholder.GetComponent<TextMeshProUGUI>().text = text;
	}

	// Token: 0x0600ABF7 RID: 44023 RVA: 0x0040C344 File Offset: 0x0040A544
	protected override void OnSpawn()
	{
		this.inputField.ActivateInputField();
		KInputTextField kinputTextField = this.inputField;
		kinputTextField.onFocus = (System.Action)Delegate.Combine(kinputTextField.onFocus, new System.Action(this.OnFocus));
		this.inputField.onEndEdit.AddListener(new UnityAction<string>(this.OnEndEdit));
		this.inputField.onValueChanged.AddListener(new UnityAction<string>(this.OnValueChanged));
		this.clearButton.onClick += this.ClearSearch;
		this.SetPlaceholder(UI.UISIDESCREENS.TREEFILTERABLESIDESCREEN.SEARCH_PLACEHOLDER);
	}

	// Token: 0x0600ABF8 RID: 44024 RVA: 0x0010FC24 File Offset: 0x0010DE24
	protected void SetEditingState(bool editing)
	{
		this.isEditing = editing;
		Action<bool> editingStateChanged = this.EditingStateChanged;
		if (editingStateChanged != null)
		{
			editingStateChanged(this.isEditing);
		}
		KScreenManager.Instance.RefreshStack();
	}

	// Token: 0x0600ABF9 RID: 44025 RVA: 0x0010FC4E File Offset: 0x0010DE4E
	protected virtual void OnValueChanged(string value)
	{
		Action<string> valueChanged = this.ValueChanged;
		if (valueChanged == null)
		{
			return;
		}
		valueChanged(value);
	}

	// Token: 0x0600ABFA RID: 44026 RVA: 0x0010FC61 File Offset: 0x0010DE61
	protected virtual void OnEndEdit(string value)
	{
		this.SetEditingState(false);
	}

	// Token: 0x0600ABFB RID: 44027 RVA: 0x0010FC6A File Offset: 0x0010DE6A
	protected virtual void OnFocus()
	{
		this.SetEditingState(true);
		UISounds.PlaySound(UISounds.Sound.ClickHUD);
		System.Action focused = this.Focused;
		if (focused == null)
		{
			return;
		}
		focused();
	}

	// Token: 0x0600ABFC RID: 44028 RVA: 0x0010FC89 File Offset: 0x0010DE89
	public virtual void ClearSearch()
	{
		this.SetValue("");
	}

	// Token: 0x0600ABFD RID: 44029 RVA: 0x0010FC96 File Offset: 0x0010DE96
	public void SetValue(string value)
	{
		this.inputField.text = value;
		Action<string> valueChanged = this.ValueChanged;
		if (valueChanged == null)
		{
			return;
		}
		valueChanged(value);
	}

	// Token: 0x0400871E RID: 34590
	[SerializeField]
	protected KInputTextField inputField;

	// Token: 0x0400871F RID: 34591
	[SerializeField]
	protected KButton clearButton;

	// Token: 0x04008721 RID: 34593
	public Action<string> ValueChanged;

	// Token: 0x04008722 RID: 34594
	public Action<bool> EditingStateChanged;

	// Token: 0x04008723 RID: 34595
	public System.Action Focused;
}
