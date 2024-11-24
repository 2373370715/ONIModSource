using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001C4E RID: 7246
public class CodexText : CodexWidget<CodexText>
{
	// Token: 0x170009ED RID: 2541
	// (get) Token: 0x06009702 RID: 38658 RVA: 0x00102238 File Offset: 0x00100438
	// (set) Token: 0x06009703 RID: 38659 RVA: 0x00102240 File Offset: 0x00100440
	public string text { get; set; }

	// Token: 0x170009EE RID: 2542
	// (get) Token: 0x06009704 RID: 38660 RVA: 0x00102249 File Offset: 0x00100449
	// (set) Token: 0x06009705 RID: 38661 RVA: 0x00102251 File Offset: 0x00100451
	public string messageID { get; set; }

	// Token: 0x170009EF RID: 2543
	// (get) Token: 0x06009706 RID: 38662 RVA: 0x0010225A File Offset: 0x0010045A
	// (set) Token: 0x06009707 RID: 38663 RVA: 0x00102262 File Offset: 0x00100462
	public CodexTextStyle style { get; set; }

	// Token: 0x170009F0 RID: 2544
	// (get) Token: 0x06009709 RID: 38665 RVA: 0x0010227E File Offset: 0x0010047E
	// (set) Token: 0x06009708 RID: 38664 RVA: 0x0010226B File Offset: 0x0010046B
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

	// Token: 0x0600970A RID: 38666 RVA: 0x00102299 File Offset: 0x00100499
	public CodexText()
	{
		this.style = CodexTextStyle.Body;
	}

	// Token: 0x0600970B RID: 38667 RVA: 0x001022A8 File Offset: 0x001004A8
	public CodexText(string text, CodexTextStyle style = CodexTextStyle.Body, string id = null)
	{
		this.text = text;
		this.style = style;
		if (id != null)
		{
			this.messageID = id;
		}
	}

	// Token: 0x0600970C RID: 38668 RVA: 0x003AA200 File Offset: 0x003A8400
	public void ConfigureLabel(LocText label, Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
	{
		label.gameObject.SetActive(true);
		label.AllowLinks = (this.style == CodexTextStyle.Body);
		label.textStyleSetting = textStyles[this.style];
		label.text = this.text;
		label.ApplySettings();
	}

	// Token: 0x0600970D RID: 38669 RVA: 0x001022C8 File Offset: 0x001004C8
	public override void Configure(GameObject contentGameObject, Transform displayPane, Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
	{
		this.ConfigureLabel(contentGameObject.GetComponent<LocText>(), textStyles);
		base.ConfigurePreferredLayout(contentGameObject);
	}
}
