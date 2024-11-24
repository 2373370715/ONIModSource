using System;
using System.Collections.Generic;
using STRINGS;

namespace Database
{
	// Token: 0x020021B0 RID: 8624
	public class TeleportDuplicant : ColonyAchievementRequirement
	{
		// Token: 0x0600B722 RID: 46882 RVA: 0x00115F4A File Offset: 0x0011414A
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.TELEPORT_DUPLICANT;
		}

		// Token: 0x0600B723 RID: 46883 RVA: 0x0045BE80 File Offset: 0x0045A080
		public override bool Success()
		{
			using (List<WarpReceiver>.Enumerator enumerator = Components.WarpReceivers.Items.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Used)
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
