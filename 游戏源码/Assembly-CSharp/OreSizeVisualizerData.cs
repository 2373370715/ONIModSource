using System;
using UnityEngine;

// Token: 0x02001685 RID: 5765
public struct OreSizeVisualizerData
{
	// Token: 0x0600771D RID: 30493 RVA: 0x000EE3BA File Offset: 0x000EC5BA
	public OreSizeVisualizerData(GameObject go)
	{
		this.primaryElement = go.GetComponent<PrimaryElement>();
		this.onMassChangedCB = null;
	}

	// Token: 0x04005913 RID: 22803
	public PrimaryElement primaryElement;

	// Token: 0x04005914 RID: 22804
	public Action<object> onMassChangedCB;
}
