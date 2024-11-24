using System;
using System.Collections;
using STRINGS;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02001CBB RID: 7355
public class EditableTitleBar : TitleBar
{
	// Token: 0x1400002B RID: 43
	// (add) Token: 0x0600999B RID: 39323 RVA: 0x003B5F50 File Offset: 0x003B4150
	// (remove) Token: 0x0600999C RID: 39324 RVA: 0x003B5F88 File Offset: 0x003B4188
	public event Action<string> OnNameChanged;

	// Token: 0x1400002C RID: 44
	// (add) Token: 0x0600999D RID: 39325 RVA: 0x003B5FC0 File Offset: 0x003B41C0
	// (remove) Token: 0x0600999E RID: 39326 RVA: 0x003B5FF8 File Offset: 0x003B41F8
	public event System.Action OnStartedEditing;

	// Token: 0x0600999F RID: 39327 RVA: 0x003B6030 File Offset: 0x003B4230
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.randomNameButton != null)
		{
			this.randomNameButton.onClick += this.GenerateRandomName;
		}
		if (this.editNameButton != null)
		{
			this.EnableEditButtonClick();
		}
		if (this.inputField != null)
		{
			this.inputField.onEndEdit.AddListener(new UnityAction<string>(this.OnEndEdit));
		}
	}

	// Token: 0x060099A0 RID: 39328 RVA: 0x003B60A8 File Offset: 0x003B42A8
	public void UpdateRenameTooltip(GameObject target)
	{
		if (this.editNameButton != null && target != null)
		{
			if (target.GetComponent<MinionBrain>() != null)
			{
				this.editNameButton.GetComponent<ToolTip>().toolTip = UI.TOOLTIPS.EDITNAME;
			}
			if (target.GetComponent<ClustercraftExteriorDoor>() != null || target.GetComponent<CommandModule>() != null)
			{
				this.editNameButton.GetComponent<ToolTip>().toolTip = UI.TOOLTIPS.EDITNAMEROCKET;
				return;
			}
			this.editNameButton.GetComponent<ToolTip>().toolTip = string.Format(UI.TOOLTIPS.EDITNAMEGENERIC, target.GetProperName());
		}
	}

	// Token: 0x060099A1 RID: 39329 RVA: 0x003B6158 File Offset: 0x003B4358
	private void OnEndEdit(string finalStr)
	{
		finalStr = Localization.FilterDirtyWords(finalStr);
		this.SetEditingState(false);
		if (string.IsNullOrEmpty(finalStr))
		{
			return;
		}
		if (this.OnNameChanged != null)
		{
			this.OnNameChanged(finalStr);
		}
		this.titleText.text = finalStr;
		if (this.postEndEdit != null)
		{
			base.StopCoroutine(this.postEndEdit);
		}
		if (base.gameObject.activeInHierarchy && base.enabled)
		{
			this.postEndEdit = base.StartCoroutine(this.PostOnEndEditRoutine());
		}
	}

	// Token: 0x060099A2 RID: 39330 RVA: 0x00103FD0 File Offset: 0x001021D0
	private IEnumerator PostOnEndEditRoutine()
	{
		int i = 0;
		while (i < 10)
		{
			int num = i;
			i = num + 1;
			yield return SequenceUtil.WaitForEndOfFrame;
		}
		this.EnableEditButtonClick();
		if (this.randomNameButton != null)
		{
			this.randomNameButton.gameObject.SetActive(false);
		}
		yield break;
	}

	// Token: 0x060099A3 RID: 39331 RVA: 0x00103FDF File Offset: 0x001021DF
	private IEnumerator PreToggleNameEditingRoutine()
	{
		yield return SequenceUtil.WaitForEndOfFrame;
		this.ToggleNameEditing();
		this.preToggleNameEditing = null;
		yield break;
	}

	// Token: 0x060099A4 RID: 39332 RVA: 0x00103FEE File Offset: 0x001021EE
	private void EnableEditButtonClick()
	{
		this.editNameButton.onClick += delegate()
		{
			if (this.preToggleNameEditing != null)
			{
				return;
			}
			this.preToggleNameEditing = base.StartCoroutine(this.PreToggleNameEditingRoutine());
		};
	}

	// Token: 0x060099A5 RID: 39333 RVA: 0x003B61D8 File Offset: 0x003B43D8
	private void GenerateRandomName()
	{
		if (this.postEndEdit != null)
		{
			base.StopCoroutine(this.postEndEdit);
		}
		string text = GameUtil.GenerateRandomDuplicantName();
		if (this.OnNameChanged != null)
		{
			this.OnNameChanged(text);
		}
		this.titleText.text = text;
		this.SetEditingState(true);
	}

	// Token: 0x060099A6 RID: 39334 RVA: 0x003B6228 File Offset: 0x003B4428
	private void ToggleNameEditing()
	{
		this.editNameButton.ClearOnClick();
		bool flag = !this.inputField.gameObject.activeInHierarchy;
		if (this.randomNameButton != null)
		{
			this.randomNameButton.gameObject.SetActive(flag);
		}
		this.SetEditingState(flag);
	}

	// Token: 0x060099A7 RID: 39335 RVA: 0x003B627C File Offset: 0x003B447C
	private void SetEditingState(bool state)
	{
		this.titleText.gameObject.SetActive(!state);
		if (this.setCameraControllerState)
		{
			CameraController.Instance.DisableUserCameraControl = state;
		}
		if (this.inputField == null)
		{
			return;
		}
		this.inputField.gameObject.SetActive(state);
		if (state)
		{
			this.inputField.text = this.titleText.text;
			this.inputField.Select();
			this.inputField.ActivateInputField();
			if (this.OnStartedEditing != null)
			{
				this.OnStartedEditing();
				return;
			}
		}
		else
		{
			this.inputField.DeactivateInputField();
		}
	}

	// Token: 0x060099A8 RID: 39336 RVA: 0x00104007 File Offset: 0x00102207
	public void ForceStopEditing()
	{
		if (this.postEndEdit != null)
		{
			base.StopCoroutine(this.postEndEdit);
		}
		this.editNameButton.ClearOnClick();
		this.SetEditingState(false);
		this.EnableEditButtonClick();
	}

	// Token: 0x060099A9 RID: 39337 RVA: 0x00104035 File Offset: 0x00102235
	public void SetUserEditable(bool editable)
	{
		this.userEditable = editable;
		this.editNameButton.gameObject.SetActive(editable);
		this.editNameButton.ClearOnClick();
		this.EnableEditButtonClick();
	}

	// Token: 0x040077E1 RID: 30689
	public KButton editNameButton;

	// Token: 0x040077E2 RID: 30690
	public KButton randomNameButton;

	// Token: 0x040077E3 RID: 30691
	public KInputTextField inputField;

	// Token: 0x040077E6 RID: 30694
	private Coroutine postEndEdit;

	// Token: 0x040077E7 RID: 30695
	private Coroutine preToggleNameEditing;
}
