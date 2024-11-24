using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02002014 RID: 8212
public class SpriteListDialogScreen : KModalScreen
{
	// Token: 0x0600AEA5 RID: 44709 RVA: 0x00111A3E File Offset: 0x0010FC3E
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.gameObject.SetActive(false);
		this.buttons = new List<SpriteListDialogScreen.Button>();
	}

	// Token: 0x0600AEA6 RID: 44710 RVA: 0x000A65EC File Offset: 0x000A47EC
	public override bool IsModal()
	{
		return true;
	}

	// Token: 0x0600AEA7 RID: 44711 RVA: 0x00111A5D File Offset: 0x0010FC5D
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.Escape))
		{
			this.Deactivate();
			return;
		}
		base.OnKeyDown(e);
	}

	// Token: 0x0600AEA8 RID: 44712 RVA: 0x0041A1E0 File Offset: 0x004183E0
	public void AddOption(string text, System.Action action)
	{
		GameObject gameObject = Util.KInstantiateUI(this.buttonPrefab, this.buttonPanel, true);
		this.buttons.Add(new SpriteListDialogScreen.Button
		{
			label = text,
			action = action,
			gameObject = gameObject
		});
	}

	// Token: 0x0600AEA9 RID: 44713 RVA: 0x0041A22C File Offset: 0x0041842C
	public void AddListRow(Sprite sprite, string text, float width = -1f, float height = -1f)
	{
		GameObject gameObject = Util.KInstantiateUI(this.listPrefab, this.listPanel, true);
		gameObject.GetComponentInChildren<LocText>().text = text;
		Image componentInChildren = gameObject.GetComponentInChildren<Image>();
		componentInChildren.sprite = sprite;
		if (sprite == null)
		{
			Color color = componentInChildren.color;
			color.a = 0f;
			componentInChildren.color = color;
		}
		if (width >= 0f || height >= 0f)
		{
			componentInChildren.GetComponent<AspectRatioFitter>().enabled = false;
			LayoutElement component = componentInChildren.GetComponent<LayoutElement>();
			component.minWidth = width;
			component.preferredWidth = width;
			component.minHeight = height;
			component.preferredHeight = height;
			return;
		}
		AspectRatioFitter component2 = componentInChildren.GetComponent<AspectRatioFitter>();
		float aspectRatio = (sprite == null) ? 1f : (sprite.rect.width / sprite.rect.height);
		component2.aspectRatio = aspectRatio;
	}

	// Token: 0x0600AEAA RID: 44714 RVA: 0x0041A304 File Offset: 0x00418504
	public void PopupConfirmDialog(string text, string title_text = null)
	{
		foreach (SpriteListDialogScreen.Button button in this.buttons)
		{
			button.gameObject.GetComponentInChildren<LocText>().text = button.label;
			button.gameObject.GetComponent<KButton>().onClick += button.action;
		}
		if (title_text != null)
		{
			this.titleText.text = title_text;
		}
		this.popupMessage.text = text;
	}

	// Token: 0x0600AEAB RID: 44715 RVA: 0x00111A76 File Offset: 0x0010FC76
	protected override void OnDeactivate()
	{
		if (this.onDeactivateCB != null)
		{
			this.onDeactivateCB();
		}
		base.OnDeactivate();
	}

	// Token: 0x04008953 RID: 35155
	public System.Action onDeactivateCB;

	// Token: 0x04008954 RID: 35156
	[SerializeField]
	private GameObject buttonPrefab;

	// Token: 0x04008955 RID: 35157
	[SerializeField]
	private GameObject buttonPanel;

	// Token: 0x04008956 RID: 35158
	[SerializeField]
	private LocText titleText;

	// Token: 0x04008957 RID: 35159
	[SerializeField]
	private LocText popupMessage;

	// Token: 0x04008958 RID: 35160
	[SerializeField]
	private GameObject listPanel;

	// Token: 0x04008959 RID: 35161
	[SerializeField]
	private GameObject listPrefab;

	// Token: 0x0400895A RID: 35162
	private List<SpriteListDialogScreen.Button> buttons;

	// Token: 0x02002015 RID: 8213
	private struct Button
	{
		// Token: 0x0400895B RID: 35163
		public System.Action action;

		// Token: 0x0400895C RID: 35164
		public GameObject gameObject;

		// Token: 0x0400895D RID: 35165
		public string label;
	}
}
