using System;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001C76 RID: 7286
public class ConfirmDialogScreen : KModalScreen
{
	// Token: 0x060097F7 RID: 38903 RVA: 0x00102C59 File Offset: 0x00100E59
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.gameObject.SetActive(false);
	}

	// Token: 0x060097F8 RID: 38904 RVA: 0x000A65EC File Offset: 0x000A47EC
	public override bool IsModal()
	{
		return true;
	}

	// Token: 0x060097F9 RID: 38905 RVA: 0x00102C6D File Offset: 0x00100E6D
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.Escape))
		{
			this.OnSelect_CANCEL();
			return;
		}
		base.OnKeyDown(e);
	}

	// Token: 0x060097FA RID: 38906 RVA: 0x003AE4E0 File Offset: 0x003AC6E0
	public void PopupConfirmDialog(string text, System.Action on_confirm, System.Action on_cancel, string configurable_text = null, System.Action on_configurable_clicked = null, string title_text = null, string confirm_text = null, string cancel_text = null, Sprite image_sprite = null)
	{
		while (base.transform.parent.GetComponent<Canvas>() == null && base.transform.parent.parent != null)
		{
			base.transform.SetParent(base.transform.parent.parent);
		}
		base.transform.SetAsLastSibling();
		this.confirmAction = on_confirm;
		this.cancelAction = on_cancel;
		this.configurableAction = on_configurable_clicked;
		int num = 0;
		if (this.confirmAction != null)
		{
			num++;
		}
		if (this.cancelAction != null)
		{
			num++;
		}
		if (this.configurableAction != null)
		{
			num++;
		}
		this.confirmButton.GetComponentInChildren<LocText>().text = ((confirm_text == null) ? UI.CONFIRMDIALOG.OK.text : confirm_text);
		this.cancelButton.GetComponentInChildren<LocText>().text = ((cancel_text == null) ? UI.CONFIRMDIALOG.CANCEL.text : cancel_text);
		this.confirmButton.GetComponent<KButton>().onClick += this.OnSelect_OK;
		this.cancelButton.GetComponent<KButton>().onClick += this.OnSelect_CANCEL;
		this.configurableButton.GetComponent<KButton>().onClick += this.OnSelect_third;
		this.cancelButton.SetActive(on_cancel != null);
		if (this.configurableButton != null)
		{
			this.configurableButton.SetActive(this.configurableAction != null);
			if (configurable_text != null)
			{
				this.configurableButton.GetComponentInChildren<LocText>().text = configurable_text;
			}
		}
		if (image_sprite != null)
		{
			this.image.sprite = image_sprite;
			this.image.gameObject.SetActive(true);
		}
		if (title_text != null)
		{
			this.titleText.key = "";
			this.titleText.text = title_text;
		}
		this.popupMessage.text = text;
	}

	// Token: 0x060097FB RID: 38907 RVA: 0x00102C86 File Offset: 0x00100E86
	public void OnSelect_OK()
	{
		if (this.deactivateOnConfirmAction)
		{
			this.Deactivate();
		}
		if (this.confirmAction != null)
		{
			this.confirmAction();
		}
	}

	// Token: 0x060097FC RID: 38908 RVA: 0x00102CA9 File Offset: 0x00100EA9
	public void OnSelect_CANCEL()
	{
		if (this.deactivateOnCancelAction)
		{
			this.Deactivate();
		}
		if (this.cancelAction != null)
		{
			this.cancelAction();
		}
	}

	// Token: 0x060097FD RID: 38909 RVA: 0x00102CCC File Offset: 0x00100ECC
	public void OnSelect_third()
	{
		if (this.deactivateOnConfigurableAction)
		{
			this.Deactivate();
		}
		if (this.configurableAction != null)
		{
			this.configurableAction();
		}
	}

	// Token: 0x060097FE RID: 38910 RVA: 0x00102CEF File Offset: 0x00100EEF
	protected override void OnDeactivate()
	{
		if (this.onDeactivateCB != null)
		{
			this.onDeactivateCB();
		}
		base.OnDeactivate();
	}

	// Token: 0x04007648 RID: 30280
	private System.Action confirmAction;

	// Token: 0x04007649 RID: 30281
	private System.Action cancelAction;

	// Token: 0x0400764A RID: 30282
	private System.Action configurableAction;

	// Token: 0x0400764B RID: 30283
	public bool deactivateOnConfigurableAction = true;

	// Token: 0x0400764C RID: 30284
	public bool deactivateOnConfirmAction = true;

	// Token: 0x0400764D RID: 30285
	public bool deactivateOnCancelAction = true;

	// Token: 0x0400764E RID: 30286
	public System.Action onDeactivateCB;

	// Token: 0x0400764F RID: 30287
	[SerializeField]
	private GameObject confirmButton;

	// Token: 0x04007650 RID: 30288
	[SerializeField]
	private GameObject cancelButton;

	// Token: 0x04007651 RID: 30289
	[SerializeField]
	private GameObject configurableButton;

	// Token: 0x04007652 RID: 30290
	[SerializeField]
	private LocText titleText;

	// Token: 0x04007653 RID: 30291
	[SerializeField]
	private LocText popupMessage;

	// Token: 0x04007654 RID: 30292
	[SerializeField]
	private Image image;
}
