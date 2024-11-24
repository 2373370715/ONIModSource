using System;
using UnityEngine;

// Token: 0x02001FCB RID: 8139
public class SideScreen : KScreen
{
	// Token: 0x0600AC4B RID: 44107 RVA: 0x00110007 File Offset: 0x0010E207
	public void SetContent(SideScreenContent sideScreenContent, GameObject target)
	{
		if (sideScreenContent.transform.parent != this.contentBody.transform)
		{
			sideScreenContent.transform.SetParent(this.contentBody.transform);
		}
		sideScreenContent.SetTarget(target);
	}

	// Token: 0x04008766 RID: 34662
	[SerializeField]
	private GameObject contentBody;
}
