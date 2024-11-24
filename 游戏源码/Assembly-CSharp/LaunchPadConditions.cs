using System;
using System.Collections.Generic;

// Token: 0x020018FB RID: 6395
public class LaunchPadConditions : KMonoBehaviour, IProcessConditionSet
{
	// Token: 0x06008529 RID: 34089 RVA: 0x000F75FB File Offset: 0x000F57FB
	public List<ProcessCondition> GetConditionSet(ProcessCondition.ProcessConditionType conditionType)
	{
		if (conditionType != ProcessCondition.ProcessConditionType.RocketStorage)
		{
			return null;
		}
		return this.conditions;
	}

	// Token: 0x0600852A RID: 34090 RVA: 0x000F7609 File Offset: 0x000F5809
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.conditions = new List<ProcessCondition>();
		this.conditions.Add(new TransferCargoCompleteCondition(base.gameObject));
	}

	// Token: 0x0400649B RID: 25755
	private List<ProcessCondition> conditions;
}
