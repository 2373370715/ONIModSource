using System;
using System.Collections;
using STRINGS;

namespace Database
{
	// Token: 0x020021AF RID: 8623
	public class LaunchedCraft : ColonyAchievementRequirement
	{
		// Token: 0x0600B71F RID: 46879 RVA: 0x00115F3E File Offset: 0x0011413E
		public override string GetProgress(bool completed)
		{
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.LAUNCHED_ROCKET;
		}

		// Token: 0x0600B720 RID: 46880 RVA: 0x0045BE20 File Offset: 0x0045A020
		public override bool Success()
		{
			using (IEnumerator enumerator = Components.Clustercrafts.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (((Clustercraft)enumerator.Current).Status == Clustercraft.CraftStatus.InFlight)
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
