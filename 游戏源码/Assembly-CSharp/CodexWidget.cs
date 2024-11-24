using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001C4B RID: 7243
public abstract class CodexWidget<SubClass> : ICodexWidget
{
	// Token: 0x170009E7 RID: 2535
	// (get) Token: 0x060096F0 RID: 38640 RVA: 0x0010215C File Offset: 0x0010035C
	// (set) Token: 0x060096F1 RID: 38641 RVA: 0x00102164 File Offset: 0x00100364
	public int preferredWidth { get; set; }

	// Token: 0x170009E8 RID: 2536
	// (get) Token: 0x060096F2 RID: 38642 RVA: 0x0010216D File Offset: 0x0010036D
	// (set) Token: 0x060096F3 RID: 38643 RVA: 0x00102175 File Offset: 0x00100375
	public int preferredHeight { get; set; }

	// Token: 0x060096F4 RID: 38644 RVA: 0x0010217E File Offset: 0x0010037E
	protected CodexWidget()
	{
		this.preferredWidth = -1;
		this.preferredHeight = -1;
	}

	// Token: 0x060096F5 RID: 38645 RVA: 0x00102194 File Offset: 0x00100394
	protected CodexWidget(int preferredWidth, int preferredHeight)
	{
		this.preferredWidth = preferredWidth;
		this.preferredHeight = preferredHeight;
	}

	// Token: 0x060096F6 RID: 38646
	public abstract void Configure(GameObject contentGameObject, Transform displayPane, Dictionary<CodexTextStyle, TextStyleSetting> textStyles);

	// Token: 0x060096F7 RID: 38647 RVA: 0x001021AA File Offset: 0x001003AA
	protected void ConfigurePreferredLayout(GameObject contentGameObject)
	{
		LayoutElement componentInChildren = contentGameObject.GetComponentInChildren<LayoutElement>();
		componentInChildren.preferredHeight = (float)this.preferredHeight;
		componentInChildren.preferredWidth = (float)this.preferredWidth;
	}
}
