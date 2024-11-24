using System;

namespace Database
{
	// Token: 0x02002180 RID: 8576
	public abstract class ColonyAchievementRequirement
	{
		// Token: 0x0600B648 RID: 46664
		public abstract bool Success();

		// Token: 0x0600B649 RID: 46665 RVA: 0x000AD2F7 File Offset: 0x000AB4F7
		public virtual bool Fail()
		{
			return false;
		}

		// Token: 0x0600B64A RID: 46666 RVA: 0x000CA99D File Offset: 0x000C8B9D
		public virtual string GetProgress(bool complete)
		{
			return "";
		}
	}
}
