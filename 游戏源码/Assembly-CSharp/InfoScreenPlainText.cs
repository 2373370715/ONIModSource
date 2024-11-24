using System;
using UnityEngine;

// Token: 0x02001D06 RID: 7430
[AddComponentMenu("KMonoBehaviour/scripts/InfoScreenPlainText")]
public class InfoScreenPlainText : KMonoBehaviour
{
	// Token: 0x06009B16 RID: 39702 RVA: 0x00104EA4 File Offset: 0x001030A4
	public void SetText(string text)
	{
		this.locText.text = text;
	}

	// Token: 0x04007939 RID: 31033
	[SerializeField]
	private LocText locText;
}
