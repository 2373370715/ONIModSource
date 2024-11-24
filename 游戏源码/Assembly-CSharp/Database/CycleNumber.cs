using System;
using STRINGS;

namespace Database
{
	// Token: 0x02002183 RID: 8579
	public class CycleNumber : VictoryColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x0600B658 RID: 46680 RVA: 0x001157DB File Offset: 0x001139DB
		public override string Name()
		{
			return string.Format(COLONY_ACHIEVEMENTS.THRIVING.REQUIREMENTS.MINIMUM_CYCLE, this.cycleNumber);
		}

		// Token: 0x0600B659 RID: 46681 RVA: 0x001157F7 File Offset: 0x001139F7
		public override string Description()
		{
			return string.Format(COLONY_ACHIEVEMENTS.THRIVING.REQUIREMENTS.MINIMUM_CYCLE_DESCRIPTION, this.cycleNumber);
		}

		// Token: 0x0600B65A RID: 46682 RVA: 0x00115813 File Offset: 0x00113A13
		public CycleNumber(int cycleNumber = 100)
		{
			this.cycleNumber = cycleNumber;
		}

		// Token: 0x0600B65B RID: 46683 RVA: 0x00115822 File Offset: 0x00113A22
		public override bool Success()
		{
			return GameClock.Instance.GetCycle() + 1 >= this.cycleNumber;
		}

		// Token: 0x0600B65C RID: 46684 RVA: 0x0011583B File Offset: 0x00113A3B
		public void Deserialize(IReader reader)
		{
			this.cycleNumber = reader.ReadInt32();
		}

		// Token: 0x0600B65D RID: 46685 RVA: 0x00115849 File Offset: 0x00113A49
		public override string GetProgress(bool complete)
		{
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.CYCLE_NUMBER, complete ? this.cycleNumber : (GameClock.Instance.GetCycle() + 1), this.cycleNumber);
		}

		// Token: 0x040094F4 RID: 38132
		private int cycleNumber;
	}
}
