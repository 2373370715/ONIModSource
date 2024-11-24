using System;
using STRINGS;

namespace Database
{
	// Token: 0x0200218D RID: 8589
	public class ClearBlockedGeothermalVent : VictoryColonyAchievementRequirement
	{
		// Token: 0x0600B68E RID: 46734 RVA: 0x00115A00 File Offset: 0x00113C00
		public override string Description()
		{
			return this.GetProgress(this.Success());
		}

		// Token: 0x0600B68F RID: 46735 RVA: 0x00115AAC File Offset: 0x00113CAC
		public override string Name()
		{
			return COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.REQUIREMENTS.UNBLOCK_VENT_TITLE;
		}

		// Token: 0x0600B690 RID: 46736 RVA: 0x00115AB8 File Offset: 0x00113CB8
		public override bool Success()
		{
			return SaveGame.Instance.ColonyAchievementTracker.GeothermalClearedEntombedVent;
		}

		// Token: 0x0600B691 RID: 46737 RVA: 0x00115AC9 File Offset: 0x00113CC9
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.REQUIREMENTS.UNBLOCK_VENT_DESCRIPTION;
		}
	}
}
