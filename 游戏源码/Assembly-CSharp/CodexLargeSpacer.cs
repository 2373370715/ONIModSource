using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001C54 RID: 7252
public class CodexLargeSpacer : CodexWidget<CodexLargeSpacer>
{
	// Token: 0x06009738 RID: 38712 RVA: 0x0010251C File Offset: 0x0010071C
	public override void Configure(GameObject contentGameObject, Transform displayPane, Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
	{
		base.ConfigurePreferredLayout(contentGameObject);
	}
}
