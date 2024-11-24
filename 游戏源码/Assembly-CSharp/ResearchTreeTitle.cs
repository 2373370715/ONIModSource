using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000006 RID: 6
public class ResearchTreeTitle : MonoBehaviour
{
	// Token: 0x06000016 RID: 22 RVA: 0x000A5D11 File Offset: 0x000A3F11
	public void SetLabel(string txt)
	{
		this.treeLabel.text = txt;
	}

	// Token: 0x06000017 RID: 23 RVA: 0x000A5D1F File Offset: 0x000A3F1F
	public void SetColor(int id)
	{
		this.BG.enabled = (id % 2 != 0);
	}

	// Token: 0x04000013 RID: 19
	[Header("References")]
	[SerializeField]
	private LocText treeLabel;

	// Token: 0x04000014 RID: 20
	[SerializeField]
	private Image BG;
}
