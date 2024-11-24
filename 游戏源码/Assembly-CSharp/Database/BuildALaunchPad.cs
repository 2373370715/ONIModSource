using System;
using STRINGS;

namespace Database
{
	// Token: 0x020021B2 RID: 8626
	public class BuildALaunchPad : ColonyAchievementRequirement
	{
		// Token: 0x0600B728 RID: 46888 RVA: 0x00115F73 File Offset: 0x00114173
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.BUILD_A_LAUNCHPAD;
		}

		// Token: 0x0600B729 RID: 46889 RVA: 0x0045BEE0 File Offset: 0x0045A0E0
		public override bool Success()
		{
			foreach (LaunchPad component in Components.LaunchPads.Items)
			{
				WorldContainer myWorld = component.GetMyWorld();
				if (!myWorld.IsStartWorld && Components.WarpReceivers.GetWorldItems(myWorld.id, false).Count == 0)
				{
					return true;
				}
			}
			return false;
		}
	}
}
