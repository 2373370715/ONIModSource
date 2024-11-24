using System;
using STRINGS;

namespace Database
{
	// Token: 0x02002192 RID: 8594
	public class CalorieSurplus : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x0600B6A7 RID: 46759 RVA: 0x00115B65 File Offset: 0x00113D65
		public CalorieSurplus(float surplusAmount)
		{
			this.surplusAmount = (double)surplusAmount;
		}

		// Token: 0x0600B6A8 RID: 46760 RVA: 0x00115B75 File Offset: 0x00113D75
		public override bool Success()
		{
			return (double)(ClusterManager.Instance.CountAllRations() / 1000f) >= this.surplusAmount;
		}

		// Token: 0x0600B6A9 RID: 46761 RVA: 0x001158A9 File Offset: 0x00113AA9
		public override bool Fail()
		{
			return !this.Success();
		}

		// Token: 0x0600B6AA RID: 46762 RVA: 0x00115B93 File Offset: 0x00113D93
		public void Deserialize(IReader reader)
		{
			this.surplusAmount = reader.ReadDouble();
		}

		// Token: 0x0600B6AB RID: 46763 RVA: 0x00115BA1 File Offset: 0x00113DA1
		public override string GetProgress(bool complete)
		{
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.CALORIE_SURPLUS, GameUtil.GetFormattedCalories(complete ? ((float)this.surplusAmount) : ClusterManager.Instance.CountAllRations(), GameUtil.TimeSlice.None, true), GameUtil.GetFormattedCalories((float)this.surplusAmount, GameUtil.TimeSlice.None, true));
		}

		// Token: 0x040094FF RID: 38143
		private double surplusAmount;
	}
}
