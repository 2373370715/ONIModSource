using System;
using UnityEngine;

// Token: 0x0200025D RID: 605
[AddComponentMenu("KMonoBehaviour/scripts/UpdateElementConsumerPosition")]
public class UpdateElementConsumerPosition : KMonoBehaviour, ISim200ms
{
	// Token: 0x060008B9 RID: 2233 RVA: 0x000AA2AA File Offset: 0x000A84AA
	public void Sim200ms(float dt)
	{
		base.GetComponent<ElementConsumer>().RefreshConsumptionRate();
	}
}
