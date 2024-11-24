using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001D02 RID: 7426
public class InfoDialogScreen : KModalScreen
{
	// Token: 0x06009AF7 RID: 39671 RVA: 0x00104CEE File Offset: 0x00102EEE
	public InfoScreenPlainText GetSubHeaderPrefab()
	{
		return this.subHeaderTemplate;
	}

	// Token: 0x06009AF8 RID: 39672 RVA: 0x00104CF6 File Offset: 0x00102EF6
	public InfoScreenPlainText GetPlainTextPrefab()
	{
		return this.plainTextTemplate;
	}

	// Token: 0x06009AF9 RID: 39673 RVA: 0x00104CFE File Offset: 0x00102EFE
	public InfoScreenLineItem GetLineItemPrefab()
	{
		return this.lineItemTemplate;
	}

	// Token: 0x06009AFA RID: 39674 RVA: 0x00104D06 File Offset: 0x00102F06
	public GameObject GetPrimaryButtonPrefab()
	{
		return this.leftButtonPrefab;
	}

	// Token: 0x06009AFB RID: 39675 RVA: 0x00104D0E File Offset: 0x00102F0E
	public GameObject GetSecondaryButtonPrefab()
	{
		return this.rightButtonPrefab;
	}

	// Token: 0x06009AFC RID: 39676 RVA: 0x00102C59 File Offset: 0x00100E59
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.gameObject.SetActive(false);
	}

	// Token: 0x06009AFD RID: 39677 RVA: 0x000A65EC File Offset: 0x000A47EC
	public override bool IsModal()
	{
		return true;
	}

	// Token: 0x06009AFE RID: 39678 RVA: 0x003BC9F0 File Offset: 0x003BABF0
	public override void OnKeyDown(KButtonEvent e)
	{
		if (!this.escapeCloses)
		{
			e.TryConsume(global::Action.Escape);
			return;
		}
		if (e.TryConsume(global::Action.Escape))
		{
			this.Deactivate();
			return;
		}
		if (PlayerController.Instance != null && PlayerController.Instance.ConsumeIfNotDragging(e, global::Action.MouseRight))
		{
			this.Deactivate();
			return;
		}
		base.OnKeyDown(e);
	}

	// Token: 0x06009AFF RID: 39679 RVA: 0x00104D16 File Offset: 0x00102F16
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		if (!show && this.onDeactivateFn != null)
		{
			this.onDeactivateFn();
		}
	}

	// Token: 0x06009B00 RID: 39680 RVA: 0x00104D35 File Offset: 0x00102F35
	public InfoDialogScreen AddDefaultOK(bool escapeCloses = false)
	{
		this.AddOption(UI.CONFIRMDIALOG.OK, delegate(InfoDialogScreen d)
		{
			d.Deactivate();
		}, true);
		this.escapeCloses = escapeCloses;
		return this;
	}

	// Token: 0x06009B01 RID: 39681 RVA: 0x00104D70 File Offset: 0x00102F70
	public InfoDialogScreen AddDefaultCancel()
	{
		this.AddOption(UI.CONFIRMDIALOG.CANCEL, delegate(InfoDialogScreen d)
		{
			d.Deactivate();
		}, false);
		this.escapeCloses = true;
		return this;
	}

	// Token: 0x06009B02 RID: 39682 RVA: 0x003BCA48 File Offset: 0x003BAC48
	public InfoDialogScreen AddOption(string text, Action<InfoDialogScreen> action, bool rightSide = false)
	{
		GameObject gameObject = Util.KInstantiateUI(rightSide ? this.rightButtonPrefab : this.leftButtonPrefab, rightSide ? this.rightButtonPanel : this.leftButtonPanel, true);
		gameObject.gameObject.GetComponentInChildren<LocText>().text = text;
		gameObject.gameObject.GetComponent<KButton>().onClick += delegate()
		{
			action(this);
		};
		return this;
	}

	// Token: 0x06009B03 RID: 39683 RVA: 0x003BCAC0 File Offset: 0x003BACC0
	public InfoDialogScreen AddOption(bool rightSide, out KButton button, out LocText buttonText)
	{
		GameObject gameObject = Util.KInstantiateUI(rightSide ? this.rightButtonPrefab : this.leftButtonPrefab, rightSide ? this.rightButtonPanel : this.leftButtonPanel, true);
		button = gameObject.GetComponent<KButton>();
		buttonText = gameObject.GetComponentInChildren<LocText>();
		return this;
	}

	// Token: 0x06009B04 RID: 39684 RVA: 0x00104DAB File Offset: 0x00102FAB
	public InfoDialogScreen SetHeader(string header)
	{
		this.header.text = header;
		return this;
	}

	// Token: 0x06009B05 RID: 39685 RVA: 0x00104DBA File Offset: 0x00102FBA
	public InfoDialogScreen AddSprite(Sprite sprite)
	{
		Util.KInstantiateUI<InfoScreenSpriteItem>(this.spriteItemTemplate.gameObject, this.contentContainer, false).SetSprite(sprite);
		return this;
	}

	// Token: 0x06009B06 RID: 39686 RVA: 0x00104DDA File Offset: 0x00102FDA
	public InfoDialogScreen AddPlainText(string text)
	{
		Util.KInstantiateUI<InfoScreenPlainText>(this.plainTextTemplate.gameObject, this.contentContainer, false).SetText(text);
		return this;
	}

	// Token: 0x06009B07 RID: 39687 RVA: 0x00104DFA File Offset: 0x00102FFA
	public InfoDialogScreen AddLineItem(string text, string tooltip)
	{
		InfoScreenLineItem infoScreenLineItem = Util.KInstantiateUI<InfoScreenLineItem>(this.lineItemTemplate.gameObject, this.contentContainer, false);
		infoScreenLineItem.SetText(text);
		infoScreenLineItem.SetTooltip(tooltip);
		return this;
	}

	// Token: 0x06009B08 RID: 39688 RVA: 0x00104E21 File Offset: 0x00103021
	public InfoDialogScreen AddSubHeader(string text)
	{
		Util.KInstantiateUI<InfoScreenPlainText>(this.subHeaderTemplate.gameObject, this.contentContainer, false).SetText(text);
		return this;
	}

	// Token: 0x06009B09 RID: 39689 RVA: 0x003BCB08 File Offset: 0x003BAD08
	public InfoDialogScreen AddSpacer(float height)
	{
		GameObject gameObject = new GameObject("spacer");
		gameObject.SetActive(false);
		gameObject.transform.SetParent(this.contentContainer.transform, false);
		LayoutElement layoutElement = gameObject.AddComponent<LayoutElement>();
		layoutElement.minHeight = height;
		layoutElement.preferredHeight = height;
		layoutElement.flexibleHeight = 0f;
		gameObject.SetActive(true);
		return this;
	}

	// Token: 0x06009B0A RID: 39690 RVA: 0x00104E41 File Offset: 0x00103041
	public InfoDialogScreen AddUI<T>(T prefab, out T spawn) where T : MonoBehaviour
	{
		spawn = Util.KInstantiateUI<T>(prefab.gameObject, this.contentContainer, true);
		return this;
	}

	// Token: 0x06009B0B RID: 39691 RVA: 0x003BCB64 File Offset: 0x003BAD64
	public InfoDialogScreen AddDescriptors(List<Descriptor> descriptors)
	{
		for (int i = 0; i < descriptors.Count; i++)
		{
			this.AddLineItem(descriptors[i].IndentedText(), descriptors[i].tooltipText);
		}
		return this;
	}

	// Token: 0x04007924 RID: 31012
	[SerializeField]
	private InfoScreenPlainText subHeaderTemplate;

	// Token: 0x04007925 RID: 31013
	[SerializeField]
	private InfoScreenPlainText plainTextTemplate;

	// Token: 0x04007926 RID: 31014
	[SerializeField]
	private InfoScreenLineItem lineItemTemplate;

	// Token: 0x04007927 RID: 31015
	[SerializeField]
	private InfoScreenSpriteItem spriteItemTemplate;

	// Token: 0x04007928 RID: 31016
	[Space(10f)]
	[SerializeField]
	private LocText header;

	// Token: 0x04007929 RID: 31017
	[SerializeField]
	private GameObject contentContainer;

	// Token: 0x0400792A RID: 31018
	[SerializeField]
	private GameObject leftButtonPrefab;

	// Token: 0x0400792B RID: 31019
	[SerializeField]
	private GameObject rightButtonPrefab;

	// Token: 0x0400792C RID: 31020
	[SerializeField]
	private GameObject leftButtonPanel;

	// Token: 0x0400792D RID: 31021
	[SerializeField]
	private GameObject rightButtonPanel;

	// Token: 0x0400792E RID: 31022
	private bool escapeCloses;

	// Token: 0x0400792F RID: 31023
	public System.Action onDeactivateFn;
}
