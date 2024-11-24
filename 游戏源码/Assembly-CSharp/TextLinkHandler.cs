using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02002029 RID: 8233
public class TextLinkHandler : MonoBehaviour, IPointerClickHandler, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler
{
	// Token: 0x0600AF45 RID: 44869 RVA: 0x0041F634 File Offset: 0x0041D834
	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button != PointerEventData.InputButton.Left)
		{
			return;
		}
		if (!this.text.AllowLinks)
		{
			return;
		}
		int num = TMP_TextUtilities.FindIntersectingLink(this.text, KInputManager.GetMousePos(), null);
		if (num != -1)
		{
			string text = CodexCache.FormatLinkID(this.text.textInfo.linkInfo[num].GetLinkID());
			if (this.overrideLinkAction == null || this.overrideLinkAction(text))
			{
				if (!CodexCache.entries.ContainsKey(text))
				{
					SubEntry subEntry = CodexCache.FindSubEntry(text);
					if (subEntry == null || subEntry.disabled)
					{
						text = "PAGENOTFOUND";
					}
				}
				else if (CodexCache.entries[text].disabled)
				{
					text = "PAGENOTFOUND";
				}
				if (!ManagementMenu.Instance.codexScreen.gameObject.activeInHierarchy)
				{
					ManagementMenu.Instance.ToggleCodex();
				}
				ManagementMenu.Instance.codexScreen.ChangeArticle(text, true, default(Vector3), CodexScreen.HistoryDirection.NewArticle);
			}
		}
	}

	// Token: 0x0600AF46 RID: 44870 RVA: 0x00111EA9 File Offset: 0x001100A9
	private void Update()
	{
		this.CheckMouseOver();
		if (TextLinkHandler.hoveredText == this && this.text.AllowLinks)
		{
			PlayerController.Instance.ActiveTool.SetLinkCursor(this.hoverLink);
		}
	}

	// Token: 0x0600AF47 RID: 44871 RVA: 0x00111EE0 File Offset: 0x001100E0
	private void OnEnable()
	{
		this.CheckMouseOver();
	}

	// Token: 0x0600AF48 RID: 44872 RVA: 0x00111EE8 File Offset: 0x001100E8
	private void OnDisable()
	{
		this.ClearState();
	}

	// Token: 0x0600AF49 RID: 44873 RVA: 0x00111EF0 File Offset: 0x001100F0
	private void Awake()
	{
		this.text = base.GetComponent<LocText>();
		if (this.text.AllowLinks && !this.text.raycastTarget)
		{
			this.text.raycastTarget = true;
		}
	}

	// Token: 0x0600AF4A RID: 44874 RVA: 0x00111F24 File Offset: 0x00110124
	public void OnPointerEnter(PointerEventData eventData)
	{
		this.SetMouseOver();
	}

	// Token: 0x0600AF4B RID: 44875 RVA: 0x00111EE8 File Offset: 0x001100E8
	public void OnPointerExit(PointerEventData eventData)
	{
		this.ClearState();
	}

	// Token: 0x0600AF4C RID: 44876 RVA: 0x0041F724 File Offset: 0x0041D924
	private void ClearState()
	{
		if (this == null || this.Equals(null))
		{
			return;
		}
		if (TextLinkHandler.hoveredText == this)
		{
			if (this.hoverLink && PlayerController.Instance != null && PlayerController.Instance.ActiveTool != null)
			{
				PlayerController.Instance.ActiveTool.SetLinkCursor(false);
			}
			TextLinkHandler.hoveredText = null;
			this.hoverLink = false;
		}
	}

	// Token: 0x0600AF4D RID: 44877 RVA: 0x0041F798 File Offset: 0x0041D998
	public void CheckMouseOver()
	{
		if (this.text == null)
		{
			return;
		}
		if (TMP_TextUtilities.FindIntersectingLink(this.text, KInputManager.GetMousePos(), null) != -1)
		{
			this.SetMouseOver();
			this.hoverLink = true;
			return;
		}
		if (TextLinkHandler.hoveredText == this)
		{
			this.hoverLink = false;
		}
	}

	// Token: 0x0600AF4E RID: 44878 RVA: 0x00111F2C File Offset: 0x0011012C
	private void SetMouseOver()
	{
		if (TextLinkHandler.hoveredText != null && TextLinkHandler.hoveredText != this)
		{
			TextLinkHandler.hoveredText.hoverLink = false;
		}
		TextLinkHandler.hoveredText = this;
	}

	// Token: 0x040089F8 RID: 35320
	private static TextLinkHandler hoveredText;

	// Token: 0x040089F9 RID: 35321
	[MyCmpGet]
	private LocText text;

	// Token: 0x040089FA RID: 35322
	private bool hoverLink;

	// Token: 0x040089FB RID: 35323
	public Func<string, bool> overrideLinkAction;
}
