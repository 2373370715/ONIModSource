using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02001AC8 RID: 6856
public class FileNameDialog : KModalScreen
{
	// Token: 0x06008F9A RID: 36762 RVA: 0x000FDD4B File Offset: 0x000FBF4B
	public override float GetSortKey()
	{
		return 150f;
	}

	// Token: 0x06008F9B RID: 36763 RVA: 0x000FDD52 File Offset: 0x000FBF52
	public void SetTextAndSelect(string text)
	{
		if (this.inputField == null)
		{
			return;
		}
		this.inputField.text = text;
		this.inputField.Select();
	}

	// Token: 0x06008F9C RID: 36764 RVA: 0x00377824 File Offset: 0x00375A24
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.confirmButton.onClick += this.OnConfirm;
		this.cancelButton.onClick += this.OnCancel;
		this.closeButton.onClick += this.OnCancel;
		this.inputField.onValueChanged.AddListener(delegate(string <p0>)
		{
			Util.ScrubInputField(this.inputField, false, false);
		});
		this.inputField.onEndEdit.AddListener(new UnityAction<string>(this.OnEndEdit));
	}

	// Token: 0x06008F9D RID: 36765 RVA: 0x000FDD7A File Offset: 0x000FBF7A
	protected override void OnActivate()
	{
		base.OnActivate();
		this.inputField.Select();
		this.inputField.ActivateInputField();
		CameraController.Instance.DisableUserCameraControl = true;
	}

	// Token: 0x06008F9E RID: 36766 RVA: 0x000FDDA3 File Offset: 0x000FBFA3
	protected override void OnDeactivate()
	{
		CameraController.Instance.DisableUserCameraControl = false;
		base.OnDeactivate();
	}

	// Token: 0x06008F9F RID: 36767 RVA: 0x003778B4 File Offset: 0x00375AB4
	public void OnConfirm()
	{
		if (this.onConfirm != null && !string.IsNullOrEmpty(this.inputField.text))
		{
			string text = this.inputField.text;
			if (!text.EndsWith(".sav"))
			{
				text += ".sav";
			}
			this.onConfirm(text);
			this.Deactivate();
		}
	}

	// Token: 0x06008FA0 RID: 36768 RVA: 0x000FDDB6 File Offset: 0x000FBFB6
	private void OnEndEdit(string str)
	{
		if (Localization.HasDirtyWords(str))
		{
			this.inputField.text = "";
		}
	}

	// Token: 0x06008FA1 RID: 36769 RVA: 0x000FDDD0 File Offset: 0x000FBFD0
	public void OnCancel()
	{
		if (this.onCancel != null)
		{
			this.onCancel();
		}
		this.Deactivate();
	}

	// Token: 0x06008FA2 RID: 36770 RVA: 0x000FDDEB File Offset: 0x000FBFEB
	public override void OnKeyUp(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.Escape))
		{
			this.Deactivate();
		}
		else if (e.TryConsume(global::Action.DialogSubmit))
		{
			this.OnConfirm();
		}
		e.Consumed = true;
	}

	// Token: 0x06008FA3 RID: 36771 RVA: 0x000FDE18 File Offset: 0x000FC018
	public override void OnKeyDown(KButtonEvent e)
	{
		e.Consumed = true;
	}

	// Token: 0x04006C5E RID: 27742
	public Action<string> onConfirm;

	// Token: 0x04006C5F RID: 27743
	public System.Action onCancel;

	// Token: 0x04006C60 RID: 27744
	[SerializeField]
	private KInputTextField inputField;

	// Token: 0x04006C61 RID: 27745
	[SerializeField]
	private KButton confirmButton;

	// Token: 0x04006C62 RID: 27746
	[SerializeField]
	private KButton cancelButton;

	// Token: 0x04006C63 RID: 27747
	[SerializeField]
	private KButton closeButton;
}
