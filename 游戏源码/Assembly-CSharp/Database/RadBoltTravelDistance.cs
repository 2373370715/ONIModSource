using System;
using STRINGS;

namespace Database
{
	// Token: 0x020021B6 RID: 8630
	public class RadBoltTravelDistance : ColonyAchievementRequirement
	{
		// Token: 0x0600B734 RID: 46900 RVA: 0x00116028 File Offset: 0x00114228
		public RadBoltTravelDistance(int travelDistance)
		{
			this.travelDistance = travelDistance;
		}

		// Token: 0x0600B735 RID: 46901 RVA: 0x00116037 File Offset: 0x00114237
		public override string GetProgress(bool complete)
		{
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.RADBOLT_TRAVEL, SaveGame.Instance.ColonyAchievementTracker.radBoltTravelDistance, this.travelDistance);
		}

		// Token: 0x0600B736 RID: 46902 RVA: 0x00116067 File Offset: 0x00114267
		public override bool Success()
		{
			return SaveGame.Instance.ColonyAchievementTracker.radBoltTravelDistance > (float)this.travelDistance;
		}

		// Token: 0x04009527 RID: 38183
		private int travelDistance;
	}
}
