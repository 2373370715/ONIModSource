using System;
using System.Collections;
using STRINGS;

namespace Database
{
	// Token: 0x02002181 RID: 8577
	public class MonumentBuilt : VictoryColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x0600B64C RID: 46668 RVA: 0x00115707 File Offset: 0x00113907
		public override string Name()
		{
			return COLONY_ACHIEVEMENTS.THRIVING.REQUIREMENTS.BUILT_MONUMENT;
		}

		// Token: 0x0600B64D RID: 46669 RVA: 0x00115713 File Offset: 0x00113913
		public override string Description()
		{
			return COLONY_ACHIEVEMENTS.THRIVING.REQUIREMENTS.BUILT_MONUMENT_DESCRIPTION;
		}

		// Token: 0x0600B64E RID: 46670 RVA: 0x00459C3C File Offset: 0x00457E3C
		public override bool Success()
		{
			using (IEnumerator enumerator = Components.MonumentParts.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (((MonumentPart)enumerator.Current).IsMonumentCompleted())
					{
						Game.Instance.unlocks.Unlock("thriving", true);
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600B64F RID: 46671 RVA: 0x000A5E40 File Offset: 0x000A4040
		public void Deserialize(IReader reader)
		{
		}

		// Token: 0x0600B650 RID: 46672 RVA: 0x0011571F File Offset: 0x0011391F
		public override string GetProgress(bool complete)
		{
			return this.Name();
		}
	}
}
