using System;
using STRINGS;

namespace Database
{
	// Token: 0x0200219B RID: 8603
	public class EatXCalories : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x0600B6CE RID: 46798 RVA: 0x00115CB4 File Offset: 0x00113EB4
		public EatXCalories(int numCalories)
		{
			this.numCalories = numCalories;
		}

		// Token: 0x0600B6CF RID: 46799 RVA: 0x00115CC3 File Offset: 0x00113EC3
		public override bool Success()
		{
			return WorldResourceAmountTracker<RationTracker>.Get().GetAmountConsumed() / 1000f > (float)this.numCalories;
		}

		// Token: 0x0600B6D0 RID: 46800 RVA: 0x00115CDE File Offset: 0x00113EDE
		public void Deserialize(IReader reader)
		{
			this.numCalories = reader.ReadInt32();
		}

		// Token: 0x0600B6D1 RID: 46801 RVA: 0x0045ABCC File Offset: 0x00458DCC
		public override string GetProgress(bool complete)
		{
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.CONSUME_CALORIES, GameUtil.GetFormattedCalories(complete ? ((float)this.numCalories * 1000f) : WorldResourceAmountTracker<RationTracker>.Get().GetAmountConsumed(), GameUtil.TimeSlice.None, true), GameUtil.GetFormattedCalories((float)this.numCalories * 1000f, GameUtil.TimeSlice.None, true));
		}

		// Token: 0x0400950E RID: 38158
		private int numCalories;
	}
}
