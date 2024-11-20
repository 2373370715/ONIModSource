using System;
using TMPro;
using UnityEngine;

// Token: 0x02000070 RID: 112
public class TextStyleSetting : ScriptableObject
{
	// Token: 0x06000497 RID: 1175 RVA: 0x00016BB8 File Offset: 0x00014DB8
	public void Init(TMP_FontAsset _sdfFont, int _fontSize, Color _color, bool _enableWordWrapping)
	{
		this.sdfFont = _sdfFont;
		this.fontSize = _fontSize;
		this.textColor = _color;
		this.enableWordWrapping = _enableWordWrapping;
	}

	// Token: 0x040004D1 RID: 1233
	public TMP_FontAsset sdfFont;

	// Token: 0x040004D2 RID: 1234
	public int fontSize;

	// Token: 0x040004D3 RID: 1235
	public Color textColor;

	// Token: 0x040004D4 RID: 1236
	public FontStyles style;

	// Token: 0x040004D5 RID: 1237
	public bool enableWordWrapping = true;
}
