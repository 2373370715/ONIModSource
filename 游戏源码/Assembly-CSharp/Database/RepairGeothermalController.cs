using System;
using STRINGS;

namespace Database
{
	// Token: 0x0200218B RID: 8587
	public class RepairGeothermalController : VictoryColonyAchievementRequirement
	{
		// Token: 0x0600B684 RID: 46724 RVA: 0x00115A00 File Offset: 0x00113C00
		public override string Description()
		{
			return this.GetProgress(this.Success());
		}

		// Token: 0x0600B685 RID: 46725 RVA: 0x00115A6B File Offset: 0x00113C6B
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.REQUIREMENTS.REPAIR_CONTROLLER_DESCRIPTION;
		}

		// Token: 0x0600B686 RID: 46726 RVA: 0x00115A77 File Offset: 0x00113C77
		public override string Name()
		{
			return COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.REQUIREMENTS.REPAIR_CONTROLLER_TITLE;
		}

		// Token: 0x0600B687 RID: 46727 RVA: 0x000E32E0 File Offset: 0x000E14E0
		public override bool Success()
		{
			return SaveGame.Instance.ColonyAchievementTracker.GeothermalControllerRepaired;
		}
	}
}
