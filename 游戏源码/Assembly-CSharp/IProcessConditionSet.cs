using System;
using System.Collections.Generic;

// Token: 0x02001F56 RID: 8022
public interface IProcessConditionSet
{
	// Token: 0x0600A95B RID: 43355
	List<ProcessCondition> GetConditionSet(ProcessCondition.ProcessConditionType conditionType);
}
