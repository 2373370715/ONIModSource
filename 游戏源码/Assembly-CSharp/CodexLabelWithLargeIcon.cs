using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001C57 RID: 7255
public class CodexLabelWithLargeIcon : CodexLabelWithIcon
{
	// Token: 0x170009FE RID: 2558
	// (get) Token: 0x06009744 RID: 38724 RVA: 0x001025A7 File Offset: 0x001007A7
	// (set) Token: 0x06009745 RID: 38725 RVA: 0x001025AF File Offset: 0x001007AF
	public string linkID { get; set; }

	// Token: 0x06009746 RID: 38726 RVA: 0x001025B8 File Offset: 0x001007B8
	public CodexLabelWithLargeIcon()
	{
	}

	// Token: 0x06009747 RID: 38727 RVA: 0x003AA474 File Offset: 0x003A8674
	public CodexLabelWithLargeIcon(string text, CodexTextStyle style, global::Tuple<Sprite, Color> coloredSprite, string targetEntrylinkID) : base(text, style, coloredSprite, 128, 128)
	{
		base.icon = new CodexImage(128, 128, coloredSprite);
		base.label = new CodexText(text, style, null);
		this.linkID = targetEntrylinkID;
	}

	// Token: 0x06009748 RID: 38728 RVA: 0x003AA4C0 File Offset: 0x003A86C0
	public override void Configure(GameObject contentGameObject, Transform displayPane, Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
	{
		base.icon.ConfigureImage(contentGameObject.GetComponentsInChildren<Image>()[1]);
		if (base.icon.preferredWidth != -1 && base.icon.preferredHeight != -1)
		{
			LayoutElement component = contentGameObject.GetComponentsInChildren<Image>()[1].GetComponent<LayoutElement>();
			component.minWidth = (float)base.icon.preferredHeight;
			component.minHeight = (float)base.icon.preferredWidth;
			component.preferredHeight = (float)base.icon.preferredHeight;
			component.preferredWidth = (float)base.icon.preferredWidth;
		}
		base.label.text = UI.StripLinkFormatting(base.label.text);
		base.label.ConfigureLabel(contentGameObject.GetComponentInChildren<LocText>(), textStyles);
		contentGameObject.GetComponent<KButton>().ClearOnClick();
		contentGameObject.GetComponent<KButton>().onClick += delegate()
		{
			ManagementMenu.Instance.codexScreen.ChangeArticle(this.linkID, false, default(Vector3), CodexScreen.HistoryDirection.NewArticle);
		};
	}
}
