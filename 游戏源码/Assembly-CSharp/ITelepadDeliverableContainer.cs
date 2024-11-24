using System;
using UnityEngine;

// Token: 0x02001407 RID: 5127
public interface ITelepadDeliverableContainer
{
	// Token: 0x0600694E RID: 26958
	void SelectDeliverable();

	// Token: 0x0600694F RID: 26959
	void DeselectDeliverable();

	// Token: 0x06006950 RID: 26960
	GameObject GetGameObject();
}
