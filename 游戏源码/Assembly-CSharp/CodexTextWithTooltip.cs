using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001C4F RID: 7247
public class CodexTextWithTooltip : CodexWidget<CodexTextWithTooltip>
{
	// Token: 0x170009F1 RID: 2545
	// (get) Token: 0x0600970E RID: 38670 RVA: 0x001022DE File Offset: 0x001004DE
	// (set) Token: 0x0600970F RID: 38671 RVA: 0x001022E6 File Offset: 0x001004E6
	public string text { get; set; }

	// Token: 0x170009F2 RID: 2546
	// (get) Token: 0x06009710 RID: 38672 RVA: 0x001022EF File Offset: 0x001004EF
	// (set) Token: 0x06009711 RID: 38673 RVA: 0x001022F7 File Offset: 0x001004F7
	public string tooltip { get; set; }

	// Token: 0x170009F3 RID: 2547
	// (get) Token: 0x06009712 RID: 38674 RVA: 0x00102300 File Offset: 0x00100500
	// (set) Token: 0x06009713 RID: 38675 RVA: 0x00102308 File Offset: 0x00100508
	public CodexTextStyle style { get; set; }

	// Token: 0x170009F4 RID: 2548
	// (get) Token: 0x06009715 RID: 38677 RVA: 0x00102324 File Offset: 0x00100524
	// (set) Token: 0x06009714 RID: 38676 RVA: 0x00102311 File Offset: 0x00100511
	public string stringKey
	{
		get
		{
			return "--> " + (this.text ?? "NULL");
		}
		set
		{
			this.text = Strings.Get(value);
		}
	}

	// Token: 0x06009716 RID: 38678 RVA: 0x0010233F File Offset: 0x0010053F
	public CodexTextWithTooltip()
	{
		this.style = CodexTextStyle.Body;
	}

	// Token: 0x06009717 RID: 38679 RVA: 0x0010234E File Offset: 0x0010054E
	public CodexTextWithTooltip(string text, string tooltip, CodexTextStyle style = CodexTextStyle.Body)
	{
		this.text = text;
		this.style = style;
		this.tooltip = tooltip;
	}

	// Token: 0x06009718 RID: 38680 RVA: 0x003AA24C File Offset: 0x003A844C
	public void ConfigureLabel(LocText label, Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
	{
		label.gameObject.SetActive(true);
		label.AllowLinks = (this.style == CodexTextStyle.Body);
		label.textStyleSetting = textStyles[this.style];
		label.text = this.text;
		label.ApplySettings();
	}

	// Token: 0x06009719 RID: 38681 RVA: 0x0010236B File Offset: 0x0010056B
	public void ConfigureTooltip(ToolTip tooltip)
	{
		tooltip.SetSimpleTooltip(this.tooltip);
	}

	// Token: 0x0600971A RID: 38682 RVA: 0x00102379 File Offset: 0x00100579
	public override void Configure(GameObject contentGameObject, Transform displayPane, Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
	{
		this.ConfigureLabel(contentGameObject.GetComponent<LocText>(), textStyles);
		this.ConfigureTooltip(contentGameObject.GetComponent<ToolTip>());
		base.ConfigurePreferredLayout(contentGameObject);
	}
}
