using System;
using STRINGS;

namespace Database
{
	// Token: 0x020021B3 RID: 8627
	public class AnalyzeSeed : ColonyAchievementRequirement
	{
		// Token: 0x0600B72B RID: 46891 RVA: 0x00115F7F File Offset: 0x0011417F
		public AnalyzeSeed(string seedname)
		{
			this.seedName = seedname;
		}

		// Token: 0x0600B72C RID: 46892 RVA: 0x00115F8E File Offset: 0x0011418E
		public override string GetProgress(bool complete)
		{
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.ANALYZE_SEED, this.seedName.ProperName());
		}

		// Token: 0x0600B72D RID: 46893 RVA: 0x00115FAF File Offset: 0x001141AF
		public override bool Success()
		{
			return SaveGame.Instance.ColonyAchievementTracker.analyzedSeeds.Contains(this.seedName);
		}

		// Token: 0x04009525 RID: 38181
		private string seedName;
	}
}
