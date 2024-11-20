using TMPro;
using UnityEngine;

// Token: 0x02000B1A RID: 2842
public class LocText : TextMeshProUGUI
{
	// Token: 0x04003AD7 RID: 15063
	public string key;
	

	// Token: 0x04003AD8 RID: 15064
	public TextStyleSetting textStyleSetting;

	// Token: 0x04003AD9 RID: 15065
	public bool allowOverride;

	// Token: 0x04003ADA RID: 15066
	public bool staticLayout;

	// Token: 0x04003ADC RID: 15068
	private string originalString = string.Empty;

	// Token: 0x04003ADD RID: 15069
	[SerializeField]
	private bool allowLinksInternal;
	
}
