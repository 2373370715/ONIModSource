using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001C4A RID: 7242
public interface ICodexWidget
{
	// Token: 0x060096EF RID: 38639
	void Configure(GameObject contentGameObject, Transform displayPane, Dictionary<CodexTextStyle, TextStyleSetting> textStyles);
}
