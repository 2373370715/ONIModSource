using System;
using STRINGS;

namespace Database
{
	// Token: 0x0200218A RID: 8586
	public class DiscoverGeothermalFacility : VictoryColonyAchievementRequirement
	{
		// Token: 0x0600B67F RID: 46719 RVA: 0x00115A00 File Offset: 0x00113C00
		public override string Description()
		{
			return this.GetProgress(this.Success());
		}

		// Token: 0x0600B680 RID: 46720 RVA: 0x00115A4C File Offset: 0x00113C4C
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.REQUIREMENTS.DISCOVER_GEOTHERMAL_FACILITY_DESCRIPTION;
		}

		// Token: 0x0600B681 RID: 46721 RVA: 0x00115A58 File Offset: 0x00113C58
		public override string Name()
		{
			return COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.REQUIREMENTS.DISCOVER_GEOTHERMAL_FACILITY_TITLE;
		}

		// Token: 0x0600B682 RID: 46722 RVA: 0x00115A64 File Offset: 0x00113C64
		public override bool Success()
		{
			return GeothermalPlantComponent.GeothermalFacilityDiscovered();
		}
	}
}
