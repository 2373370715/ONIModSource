using System;
using System.Collections.Generic;

// Token: 0x02001991 RID: 6545
public class RocketProcessConditionDisplayTarget : KMonoBehaviour, IProcessConditionSet, ISim1000ms
{
	// Token: 0x06008872 RID: 34930 RVA: 0x000F9401 File Offset: 0x000F7601
	public List<ProcessCondition> GetConditionSet(ProcessCondition.ProcessConditionType conditionType)
	{
		if (this.craftModuleInterface == null)
		{
			this.craftModuleInterface = base.GetComponent<RocketModuleCluster>().CraftInterface;
		}
		return this.craftModuleInterface.GetConditionSet(conditionType);
	}

	// Token: 0x06008873 RID: 34931 RVA: 0x00353DB0 File Offset: 0x00351FB0
	public void Sim1000ms(float dt)
	{
		bool flag = false;
		using (List<ProcessCondition>.Enumerator enumerator = this.GetConditionSet(ProcessCondition.ProcessConditionType.All).GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.EvaluateCondition() == ProcessCondition.Status.Failure)
				{
					flag = true;
					if (this.statusHandle == Guid.Empty)
					{
						this.statusHandle = base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.RocketChecklistIncomplete, null);
						break;
					}
					break;
				}
			}
		}
		if (!flag && this.statusHandle != Guid.Empty)
		{
			base.GetComponent<KSelectable>().RemoveStatusItem(this.statusHandle, false);
		}
	}

	// Token: 0x040066AA RID: 26282
	private CraftModuleInterface craftModuleInterface;

	// Token: 0x040066AB RID: 26283
	private Guid statusHandle = Guid.Empty;
}
