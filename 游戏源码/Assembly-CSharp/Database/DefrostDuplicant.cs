using System;
using STRINGS;

namespace Database
{
	// Token: 0x020021B1 RID: 8625
	public class DefrostDuplicant : ColonyAchievementRequirement
	{
		// Token: 0x0600B725 RID: 46885 RVA: 0x00115F56 File Offset: 0x00114156
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.DEFROST_DUPLICANT;
		}

		// Token: 0x0600B726 RID: 46886 RVA: 0x00115F62 File Offset: 0x00114162
		public override bool Success()
		{
			return SaveGame.Instance.ColonyAchievementTracker.defrostedDuplicant;
		}
	}
}
