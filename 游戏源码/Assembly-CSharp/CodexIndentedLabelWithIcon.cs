using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001C53 RID: 7251
public class CodexIndentedLabelWithIcon : CodexWidget<CodexIndentedLabelWithIcon>
{
	// Token: 0x170009FA RID: 2554
	// (get) Token: 0x06009730 RID: 38704 RVA: 0x001024AA File Offset: 0x001006AA
	// (set) Token: 0x06009731 RID: 38705 RVA: 0x001024B2 File Offset: 0x001006B2
	public CodexImage icon { get; set; }

	// Token: 0x170009FB RID: 2555
	// (get) Token: 0x06009732 RID: 38706 RVA: 0x001024BB File Offset: 0x001006BB
	// (set) Token: 0x06009733 RID: 38707 RVA: 0x001024C3 File Offset: 0x001006C3
	public CodexText label { get; set; }

	// Token: 0x06009734 RID: 38708 RVA: 0x001024CC File Offset: 0x001006CC
	public CodexIndentedLabelWithIcon()
	{
	}

	// Token: 0x06009735 RID: 38709 RVA: 0x001024D4 File Offset: 0x001006D4
	public CodexIndentedLabelWithIcon(string text, CodexTextStyle style, global::Tuple<Sprite, Color> coloredSprite)
	{
		this.icon = new CodexImage(coloredSprite);
		this.label = new CodexText(text, style, null);
	}

	// Token: 0x06009736 RID: 38710 RVA: 0x001024F6 File Offset: 0x001006F6
	public CodexIndentedLabelWithIcon(string text, CodexTextStyle style, global::Tuple<Sprite, Color> coloredSprite, int iconWidth, int iconHeight)
	{
		this.icon = new CodexImage(iconWidth, iconHeight, coloredSprite);
		this.label = new CodexText(text, style, null);
	}

	// Token: 0x06009737 RID: 38711 RVA: 0x003AA338 File Offset: 0x003A8538
	public override void Configure(GameObject contentGameObject, Transform displayPane, Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
	{
		Image componentInChildren = contentGameObject.GetComponentInChildren<Image>();
		this.icon.ConfigureImage(componentInChildren);
		this.label.ConfigureLabel(contentGameObject.GetComponentInChildren<LocText>(), textStyles);
		if (this.icon.preferredWidth != -1 && this.icon.preferredHeight != -1)
		{
			LayoutElement component = componentInChildren.GetComponent<LayoutElement>();
			component.minWidth = (float)this.icon.preferredHeight;
			component.minHeight = (float)this.icon.preferredWidth;
			component.preferredHeight = (float)this.icon.preferredHeight;
			component.preferredWidth = (float)this.icon.preferredWidth;
		}
	}
}
