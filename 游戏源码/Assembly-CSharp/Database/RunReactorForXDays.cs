using System;
using System.Collections.Generic;
using STRINGS;

namespace Database
{
	// Token: 0x020021B9 RID: 8633
	public class RunReactorForXDays : ColonyAchievementRequirement
	{
		// Token: 0x0600B73D RID: 46909 RVA: 0x001160E6 File Offset: 0x001142E6
		public RunReactorForXDays(int numCycles)
		{
			this.numCycles = numCycles;
		}

		// Token: 0x0600B73E RID: 46910 RVA: 0x0045C0C0 File Offset: 0x0045A2C0
		public override string GetProgress(bool complete)
		{
			int num = 0;
			foreach (Reactor reactor in Components.NuclearReactors.Items)
			{
				if (reactor.numCyclesRunning > num)
				{
					num = reactor.numCyclesRunning;
				}
			}
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.RUN_A_REACTOR, complete ? this.numCycles : num, this.numCycles);
		}

		// Token: 0x0600B73F RID: 46911 RVA: 0x0045C150 File Offset: 0x0045A350
		public override bool Success()
		{
			using (List<Reactor>.Enumerator enumerator = Components.NuclearReactors.Items.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.numCyclesRunning >= this.numCycles)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0400952A RID: 38186
		private int numCycles;
	}
}
