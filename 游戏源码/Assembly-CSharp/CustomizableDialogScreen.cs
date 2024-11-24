using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001C8D RID: 7309
public class CustomizableDialogScreen : KModalScreen
{
	// Token: 0x06009868 RID: 39016 RVA: 0x00103234 File Offset: 0x00101434
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.gameObject.SetActive(false);
		this.buttons = new List<CustomizableDialogScreen.Button>();
	}

	// Token: 0x06009869 RID: 39017 RVA: 0x000A65EC File Offset: 0x000A47EC
	public override bool IsModal()
	{
		return true;
	}

	// Token: 0x0600986A RID: 39018 RVA: 0x003AFAC4 File Offset: 0x003ADCC4
	public void AddOption(string text, System.Action action)
	{
		GameObject gameObject = Util.KInstantiateUI(this.buttonPrefab, this.buttonPanel, true);
		this.buttons.Add(new CustomizableDialogScreen.Button
		{
			label = text,
			action = action,
			gameObject = gameObject
		});
	}

	// Token: 0x0600986B RID: 39019 RVA: 0x003AFB10 File Offset: 0x003ADD10
	public void PopupConfirmDialog(string text, string title_text = null, Sprite image_sprite = null)
	{
		foreach (CustomizableDialogScreen.Button button in this.buttons)
		{
			button.gameObject.GetComponentInChildren<LocText>().text = button.label;
			button.gameObject.GetComponent<KButton>().onClick += button.action;
		}
		if (image_sprite != null)
		{
			this.image.sprite = image_sprite;
			this.image.gameObject.SetActive(true);
		}
		if (title_text != null)
		{
			this.titleText.text = title_text;
		}
		this.popupMessage.text = text;
	}

	// Token: 0x0600986C RID: 39020 RVA: 0x00103253 File Offset: 0x00101453
	protected override void OnDeactivate()
	{
		if (this.onDeactivateCB != null)
		{
			this.onDeactivateCB();
		}
		base.OnDeactivate();
	}

	// Token: 0x040076A9 RID: 30377
	public System.Action onDeactivateCB;

	// Token: 0x040076AA RID: 30378
	[SerializeField]
	private GameObject buttonPrefab;

	// Token: 0x040076AB RID: 30379
	[SerializeField]
	private GameObject buttonPanel;

	// Token: 0x040076AC RID: 30380
	[SerializeField]
	private LocText titleText;

	// Token: 0x040076AD RID: 30381
	[SerializeField]
	private LocText popupMessage;

	// Token: 0x040076AE RID: 30382
	[SerializeField]
	private Image image;

	// Token: 0x040076AF RID: 30383
	private List<CustomizableDialogScreen.Button> buttons;

	// Token: 0x02001C8E RID: 7310
	private struct Button
	{
		// Token: 0x040076B0 RID: 30384
		public System.Action action;

		// Token: 0x040076B1 RID: 30385
		public GameObject gameObject;

		// Token: 0x040076B2 RID: 30386
		public string label;
	}
}
