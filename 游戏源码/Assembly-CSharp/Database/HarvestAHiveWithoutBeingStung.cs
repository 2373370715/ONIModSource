using System;
using STRINGS;

namespace Database
{
	// Token: 0x020021B7 RID: 8631
	public class HarvestAHiveWithoutBeingStung : ColonyAchievementRequirement
	{
		// Token: 0x0600B737 RID: 46903 RVA: 0x00116081 File Offset: 0x00114281
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.HARVEST_HIVE;
		}

		// Token: 0x0600B738 RID: 46904 RVA: 0x0011608D File Offset: 0x0011428D
		public override bool Success()
		{
			return SaveGame.Instance.ColonyAchievementTracker.harvestAHiveWithoutGettingStung;
		}
	}
}
