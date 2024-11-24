using System;
using STRINGS;
using UnityEngine;

namespace Database
{
	// Token: 0x02002184 RID: 8580
	public class BeforeCycleNumber : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x0600B65E RID: 46686 RVA: 0x00115881 File Offset: 0x00113A81
		public BeforeCycleNumber(int cycleNumber = 100)
		{
			this.cycleNumber = cycleNumber;
		}

		// Token: 0x0600B65F RID: 46687 RVA: 0x00115890 File Offset: 0x00113A90
		public override bool Success()
		{
			return GameClock.Instance.GetCycle() + 1 <= this.cycleNumber;
		}

		// Token: 0x0600B660 RID: 46688 RVA: 0x001158A9 File Offset: 0x00113AA9
		public override bool Fail()
		{
			return !this.Success();
		}

		// Token: 0x0600B661 RID: 46689 RVA: 0x001158B4 File Offset: 0x00113AB4
		public void Deserialize(IReader reader)
		{
			this.cycleNumber = reader.ReadInt32();
		}

		// Token: 0x0600B662 RID: 46690 RVA: 0x001158C2 File Offset: 0x00113AC2
		public override string GetProgress(bool complete)
		{
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.REMAINING_CYCLES, Mathf.Max(this.cycleNumber - GameClock.Instance.GetCycle(), 0), this.cycleNumber);
		}

		// Token: 0x040094F5 RID: 38133
		private int cycleNumber;
	}
}
