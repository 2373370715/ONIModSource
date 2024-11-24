using System;
using STRINGS;

namespace Database
{
	// Token: 0x02002182 RID: 8578
	public class NumberOfDupes : VictoryColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x0600B652 RID: 46674 RVA: 0x0011572F File Offset: 0x0011392F
		public override string Name()
		{
			return string.Format(COLONY_ACHIEVEMENTS.THRIVING.REQUIREMENTS.MINIMUM_DUPLICANTS, this.numDupes);
		}

		// Token: 0x0600B653 RID: 46675 RVA: 0x0011574B File Offset: 0x0011394B
		public override string Description()
		{
			return string.Format(COLONY_ACHIEVEMENTS.THRIVING.REQUIREMENTS.MINIMUM_DUPLICANTS_DESCRIPTION, this.numDupes);
		}

		// Token: 0x0600B654 RID: 46676 RVA: 0x00115767 File Offset: 0x00113967
		public NumberOfDupes(int num)
		{
			this.numDupes = num;
		}

		// Token: 0x0600B655 RID: 46677 RVA: 0x00115776 File Offset: 0x00113976
		public override bool Success()
		{
			return Components.LiveMinionIdentities.Items.Count >= this.numDupes;
		}

		// Token: 0x0600B656 RID: 46678 RVA: 0x00115792 File Offset: 0x00113992
		public void Deserialize(IReader reader)
		{
			this.numDupes = reader.ReadInt32();
		}

		// Token: 0x0600B657 RID: 46679 RVA: 0x001157A0 File Offset: 0x001139A0
		public override string GetProgress(bool complete)
		{
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.POPULATION, complete ? this.numDupes : Components.LiveMinionIdentities.Items.Count, this.numDupes);
		}

		// Token: 0x040094F3 RID: 38131
		private int numDupes;
	}
}
