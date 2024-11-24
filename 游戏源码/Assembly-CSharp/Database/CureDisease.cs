using System;
using STRINGS;

namespace Database
{
	// Token: 0x020021A7 RID: 8615
	public class CureDisease : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x0600B6FF RID: 46847 RVA: 0x00115DEB File Offset: 0x00113FEB
		public override bool Success()
		{
			return Game.Instance.savedInfo.curedDisease;
		}

		// Token: 0x0600B700 RID: 46848 RVA: 0x000A5E40 File Offset: 0x000A4040
		public void Deserialize(IReader reader)
		{
		}

		// Token: 0x0600B701 RID: 46849 RVA: 0x00115DFC File Offset: 0x00113FFC
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.CURED_DISEASE;
		}
	}
}
