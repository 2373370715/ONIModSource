using System;
using STRINGS;

namespace Database
{
	// Token: 0x0200219F RID: 8607
	public class ExploreOilFieldSubZone : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x0600B6DF RID: 46815 RVA: 0x00115D24 File Offset: 0x00113F24
		public override bool Success()
		{
			return Game.Instance.savedInfo.discoveredOilField;
		}

		// Token: 0x0600B6E0 RID: 46816 RVA: 0x000A5E40 File Offset: 0x000A4040
		public void Deserialize(IReader reader)
		{
		}

		// Token: 0x0600B6E1 RID: 46817 RVA: 0x00115D35 File Offset: 0x00113F35
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.ENTER_OIL_BIOME;
		}
	}
}
