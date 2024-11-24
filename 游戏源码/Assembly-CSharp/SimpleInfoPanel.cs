using System;
using UnityEngine;

// Token: 0x02001CAA RID: 7338
public class SimpleInfoPanel
{
	// Token: 0x06009924 RID: 39204 RVA: 0x001039FE File Offset: 0x00101BFE
	public SimpleInfoPanel(SimpleInfoScreen simpleInfoRoot)
	{
		this.simpleInfoRoot = simpleInfoRoot;
	}

	// Token: 0x06009925 RID: 39205 RVA: 0x000A5E40 File Offset: 0x000A4040
	public virtual void Refresh(CollapsibleDetailContentPanel panel, GameObject selectedTarget)
	{
	}

	// Token: 0x04007762 RID: 30562
	protected SimpleInfoScreen simpleInfoRoot;
}
