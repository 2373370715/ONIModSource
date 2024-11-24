using System;
using STRINGS;

namespace Database
{
	// Token: 0x0200218C RID: 8588
	public class UseGeothermalPlant : VictoryColonyAchievementRequirement
	{
		// Token: 0x0600B689 RID: 46729 RVA: 0x00115A00 File Offset: 0x00113C00
		public override string Description()
		{
			return this.GetProgress(this.Success());
		}

		// Token: 0x0600B68A RID: 46730 RVA: 0x00115A83 File Offset: 0x00113C83
		public override string Name()
		{
			return COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.REQUIREMENTS.ACTIVATE_PLANT_TITLE;
		}

		// Token: 0x0600B68B RID: 46731 RVA: 0x00115A8F File Offset: 0x00113C8F
		public override bool Success()
		{
			return SaveGame.Instance.ColonyAchievementTracker.GeothermalControllerHasVented;
		}

		// Token: 0x0600B68C RID: 46732 RVA: 0x00115AA0 File Offset: 0x00113CA0
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.REQUIREMENTS.ACTIVATE_PLANT_DESCRIPTION;
		}
	}
}
