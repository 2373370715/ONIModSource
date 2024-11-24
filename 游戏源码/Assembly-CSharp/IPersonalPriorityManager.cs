using System;

// Token: 0x02001DC6 RID: 7622
public interface IPersonalPriorityManager
{
	// Token: 0x06009F27 RID: 40743
	int GetAssociatedSkillLevel(ChoreGroup group);

	// Token: 0x06009F28 RID: 40744
	int GetPersonalPriority(ChoreGroup group);

	// Token: 0x06009F29 RID: 40745
	void SetPersonalPriority(ChoreGroup group, int value);

	// Token: 0x06009F2A RID: 40746
	bool IsChoreGroupDisabled(ChoreGroup group);

	// Token: 0x06009F2B RID: 40747
	void ResetPersonalPriorities();
}
