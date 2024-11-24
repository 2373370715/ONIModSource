using System;
using UnityEngine;

// Token: 0x02001D05 RID: 7429
[AddComponentMenu("KMonoBehaviour/scripts/InfoScreenLineItem")]
public class InfoScreenLineItem : KMonoBehaviour
{
	// Token: 0x06009B13 RID: 39699 RVA: 0x00104E88 File Offset: 0x00103088
	public void SetText(string text)
	{
		this.locText.text = text;
	}

	// Token: 0x06009B14 RID: 39700 RVA: 0x00104E96 File Offset: 0x00103096
	public void SetTooltip(string tooltip)
	{
		this.toolTip.toolTip = tooltip;
	}

	// Token: 0x04007935 RID: 31029
	[SerializeField]
	private LocText locText;

	// Token: 0x04007936 RID: 31030
	[SerializeField]
	private ToolTip toolTip;

	// Token: 0x04007937 RID: 31031
	private string text;

	// Token: 0x04007938 RID: 31032
	private string tooltip;
}
