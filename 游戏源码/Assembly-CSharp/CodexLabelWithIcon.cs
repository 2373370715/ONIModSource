using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001C56 RID: 7254
public class CodexLabelWithIcon : CodexWidget<CodexLabelWithIcon>
{
	// Token: 0x170009FC RID: 2556
	// (get) Token: 0x0600973C RID: 38716 RVA: 0x00102535 File Offset: 0x00100735
	// (set) Token: 0x0600973D RID: 38717 RVA: 0x0010253D File Offset: 0x0010073D
	public CodexImage icon { get; set; }

	// Token: 0x170009FD RID: 2557
	// (get) Token: 0x0600973E RID: 38718 RVA: 0x00102546 File Offset: 0x00100746
	// (set) Token: 0x0600973F RID: 38719 RVA: 0x0010254E File Offset: 0x0010074E
	public CodexText label { get; set; }

	// Token: 0x06009740 RID: 38720 RVA: 0x00102557 File Offset: 0x00100757
	public CodexLabelWithIcon()
	{
	}

	// Token: 0x06009741 RID: 38721 RVA: 0x0010255F File Offset: 0x0010075F
	public CodexLabelWithIcon(string text, CodexTextStyle style, global::Tuple<Sprite, Color> coloredSprite)
	{
		this.icon = new CodexImage(coloredSprite);
		this.label = new CodexText(text, style, null);
	}

	// Token: 0x06009742 RID: 38722 RVA: 0x00102581 File Offset: 0x00100781
	public CodexLabelWithIcon(string text, CodexTextStyle style, global::Tuple<Sprite, Color> coloredSprite, int iconWidth, int iconHeight)
	{
		this.icon = new CodexImage(iconWidth, iconHeight, coloredSprite);
		this.label = new CodexText(text, style, null);
	}

	// Token: 0x06009743 RID: 38723 RVA: 0x003AA3D4 File Offset: 0x003A85D4
	public override void Configure(GameObject contentGameObject, Transform displayPane, Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
	{
		this.icon.ConfigureImage(contentGameObject.GetComponentInChildren<Image>());
		if (this.icon.preferredWidth != -1 && this.icon.preferredHeight != -1)
		{
			LayoutElement component = contentGameObject.GetComponentInChildren<Image>().GetComponent<LayoutElement>();
			component.minWidth = (float)this.icon.preferredHeight;
			component.minHeight = (float)this.icon.preferredWidth;
			component.preferredHeight = (float)this.icon.preferredHeight;
			component.preferredWidth = (float)this.icon.preferredWidth;
		}
		this.label.ConfigureLabel(contentGameObject.GetComponentInChildren<LocText>(), textStyles);
	}
}
