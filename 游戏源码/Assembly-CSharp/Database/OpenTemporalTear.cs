using System;
using STRINGS;

namespace Database
{
	// Token: 0x0200218E RID: 8590
	public class OpenTemporalTear : VictoryColonyAchievementRequirement
	{
		// Token: 0x0600B693 RID: 46739 RVA: 0x00115AD5 File Offset: 0x00113CD5
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.OPEN_TEMPORAL_TEAR;
		}

		// Token: 0x0600B694 RID: 46740 RVA: 0x00115A00 File Offset: 0x00113C00
		public override string Description()
		{
			return this.GetProgress(this.Success());
		}

		// Token: 0x0600B695 RID: 46741 RVA: 0x00115AE1 File Offset: 0x00113CE1
		public override bool Success()
		{
			return ClusterManager.Instance.GetComponent<ClusterPOIManager>().IsTemporalTearOpen();
		}

		// Token: 0x0600B696 RID: 46742 RVA: 0x00115AF2 File Offset: 0x00113CF2
		public override string Name()
		{
			return COLONY_ACHIEVEMENTS.DISTANT_PLANET_REACHED.REQUIREMENTS.OPEN_TEMPORAL_TEAR;
		}
	}
}
