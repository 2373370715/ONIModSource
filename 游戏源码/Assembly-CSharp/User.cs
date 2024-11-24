using System;
using UnityEngine;

// Token: 0x02000789 RID: 1929
[AddComponentMenu("KMonoBehaviour/scripts/User")]
public class User : KMonoBehaviour
{
	// Token: 0x060022B2 RID: 8882 RVA: 0x000B68EB File Offset: 0x000B4AEB
	public void OnStateMachineStop(string reason, StateMachine.Status status)
	{
		if (status == StateMachine.Status.Success)
		{
			base.Trigger(58624316, null);
			return;
		}
		base.Trigger(1572098533, null);
	}
}
