using System;
using System.Collections.Generic;
using STRINGS;

namespace Database
{
	// Token: 0x020021B8 RID: 8632
	public class SurviveARocketWithMinimumMorale : ColonyAchievementRequirement
	{
		// Token: 0x0600B73A RID: 46906 RVA: 0x0011609E File Offset: 0x0011429E
		public SurviveARocketWithMinimumMorale(float minimumMorale, int numberOfCycles)
		{
			this.minimumMorale = minimumMorale;
			this.numberOfCycles = numberOfCycles;
		}

		// Token: 0x0600B73B RID: 46907 RVA: 0x001160B4 File Offset: 0x001142B4
		public override string GetProgress(bool complete)
		{
			if (complete)
			{
				return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.SURVIVE_SPACE_COMPLETE, this.minimumMorale, this.numberOfCycles);
			}
			return base.GetProgress(complete);
		}

		// Token: 0x0600B73C RID: 46908 RVA: 0x0045C054 File Offset: 0x0045A254
		public override bool Success()
		{
			foreach (KeyValuePair<int, int> keyValuePair in SaveGame.Instance.ColonyAchievementTracker.cyclesRocketDupeMoraleAboveRequirement)
			{
				if (keyValuePair.Value >= this.numberOfCycles)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04009528 RID: 38184
		public float minimumMorale;

		// Token: 0x04009529 RID: 38185
		public int numberOfCycles;
	}
}
