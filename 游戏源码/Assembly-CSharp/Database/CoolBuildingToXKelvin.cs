using System;
using STRINGS;

namespace Database
{
	// Token: 0x02002198 RID: 8600
	public class CoolBuildingToXKelvin : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x0600B6C1 RID: 46785 RVA: 0x00115C36 File Offset: 0x00113E36
		public CoolBuildingToXKelvin(int kelvinToCoolTo)
		{
			this.kelvinToCoolTo = kelvinToCoolTo;
		}

		// Token: 0x0600B6C2 RID: 46786 RVA: 0x00115C45 File Offset: 0x00113E45
		public override bool Success()
		{
			return BuildingComplete.MinKelvinSeen <= (float)this.kelvinToCoolTo;
		}

		// Token: 0x0600B6C3 RID: 46787 RVA: 0x00115C58 File Offset: 0x00113E58
		public void Deserialize(IReader reader)
		{
			this.kelvinToCoolTo = reader.ReadInt32();
		}

		// Token: 0x0600B6C4 RID: 46788 RVA: 0x0045AA08 File Offset: 0x00458C08
		public override string GetProgress(bool complete)
		{
			float minKelvinSeen = BuildingComplete.MinKelvinSeen;
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.KELVIN_COOLING, minKelvinSeen);
		}

		// Token: 0x0400950B RID: 38155
		private int kelvinToCoolTo;
	}
}
