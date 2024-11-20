using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200006D RID: 109
[ExecuteInEditMode]
[AddComponentMenu("KMonoBehaviour/Plugins/SetTextStyleSetting")]
public class SetTextStyleSetting : MonoBehaviour
{
	private Text text;

	private TextMeshProUGUI sdfText;

	// Token: 0x040004CD RID: 1229
	[SerializeField]
	private TextStyleSetting style;

	// Token: 0x040004CE RID: 1230
	private TextStyleSetting currentStyle;

	// Token: 0x020009C4 RID: 2500
	public enum TextStyle
	{
		// Token: 0x040021CD RID: 8653
		Standard,
		// Token: 0x040021CE RID: 8654
		Header
	}
}
