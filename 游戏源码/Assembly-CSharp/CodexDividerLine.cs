using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001C51 RID: 7249
public class CodexDividerLine : CodexWidget<CodexDividerLine>
{
	// Token: 0x0600972C RID: 38700 RVA: 0x00102488 File Offset: 0x00100688
	public override void Configure(GameObject contentGameObject, Transform displayPane, Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
	{
		contentGameObject.GetComponent<LayoutElement>().minWidth = 530f;
	}
}
