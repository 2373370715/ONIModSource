using System;
using STRINGS;

namespace Database
{
	// Token: 0x020021AB RID: 8619
	public class BlockedCometWithBunkerDoor : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x0600B710 RID: 46864 RVA: 0x00115E8B File Offset: 0x0011408B
		public override bool Success()
		{
			return Game.Instance.savedInfo.blockedCometWithBunkerDoor;
		}

		// Token: 0x0600B711 RID: 46865 RVA: 0x000A5E40 File Offset: 0x000A4040
		public void Deserialize(IReader reader)
		{
		}

		// Token: 0x0600B712 RID: 46866 RVA: 0x00115E9C File Offset: 0x0011409C
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.BLOCKED_A_COMET;
		}
	}
}
