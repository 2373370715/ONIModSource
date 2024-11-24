using System;
using System.Collections.Generic;
using STRINGS;

namespace Database
{
	// Token: 0x0200219A RID: 8602
	public class EatXCaloriesFromY : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x0600B6CA RID: 46794 RVA: 0x00115C72 File Offset: 0x00113E72
		public EatXCaloriesFromY(int numCalories, List<string> fromFoodType)
		{
			this.numCalories = numCalories;
			this.fromFoodType = fromFoodType;
		}

		// Token: 0x0600B6CB RID: 46795 RVA: 0x00115C93 File Offset: 0x00113E93
		public override bool Success()
		{
			return WorldResourceAmountTracker<RationTracker>.Get().GetAmountConsumedForIDs(this.fromFoodType) / 1000f > (float)this.numCalories;
		}

		// Token: 0x0600B6CC RID: 46796 RVA: 0x0045AB24 File Offset: 0x00458D24
		public void Deserialize(IReader reader)
		{
			this.numCalories = reader.ReadInt32();
			int num = reader.ReadInt32();
			this.fromFoodType = new List<string>(num);
			for (int i = 0; i < num; i++)
			{
				string item = reader.ReadKleiString();
				this.fromFoodType.Add(item);
			}
		}

		// Token: 0x0600B6CD RID: 46797 RVA: 0x0045AB70 File Offset: 0x00458D70
		public override string GetProgress(bool complete)
		{
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.CALORIES_FROM_MEAT, GameUtil.GetFormattedCalories(complete ? ((float)this.numCalories * 1000f) : WorldResourceAmountTracker<RationTracker>.Get().GetAmountConsumedForIDs(this.fromFoodType), GameUtil.TimeSlice.None, true), GameUtil.GetFormattedCalories((float)this.numCalories * 1000f, GameUtil.TimeSlice.None, true));
		}

		// Token: 0x0400950C RID: 38156
		private int numCalories;

		// Token: 0x0400950D RID: 38157
		private List<string> fromFoodType = new List<string>();
	}
}
