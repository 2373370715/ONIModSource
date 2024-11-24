using System;
using STRINGS;

namespace Database
{
	// Token: 0x020021B4 RID: 8628
	public class HarvestAmountFromSpacePOI : ColonyAchievementRequirement
	{
		// Token: 0x0600B72E RID: 46894 RVA: 0x00115FD0 File Offset: 0x001141D0
		public HarvestAmountFromSpacePOI(float amountToHarvest)
		{
			this.amountToHarvest = amountToHarvest;
		}

		// Token: 0x0600B72F RID: 46895 RVA: 0x00115FDF File Offset: 0x001141DF
		public override string GetProgress(bool complete)
		{
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.MINE_SPACE_POI, SaveGame.Instance.ColonyAchievementTracker.totalMaterialsHarvestFromPOI, this.amountToHarvest);
		}

		// Token: 0x0600B730 RID: 46896 RVA: 0x0011600F File Offset: 0x0011420F
		public override bool Success()
		{
			return SaveGame.Instance.ColonyAchievementTracker.totalMaterialsHarvestFromPOI > this.amountToHarvest;
		}

		// Token: 0x04009526 RID: 38182
		private float amountToHarvest;
	}
}
