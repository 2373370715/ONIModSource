using System;

namespace TUNING
{
	// Token: 0x0200222F RID: 8751
	public class SKILLS
	{
		// Token: 0x04009821 RID: 38945
		public static int TARGET_SKILLS_EARNED = 15;

		// Token: 0x04009822 RID: 38946
		public static int TARGET_SKILLS_CYCLE = 250;

		// Token: 0x04009823 RID: 38947
		public static float EXPERIENCE_LEVEL_POWER = 1.44f;

		// Token: 0x04009824 RID: 38948
		public static float PASSIVE_EXPERIENCE_PORTION = 0.5f;

		// Token: 0x04009825 RID: 38949
		public static float ACTIVE_EXPERIENCE_PORTION = 0.6f;

		// Token: 0x04009826 RID: 38950
		public static float FULL_EXPERIENCE = 1f;

		// Token: 0x04009827 RID: 38951
		public static float ALL_DAY_EXPERIENCE = SKILLS.FULL_EXPERIENCE / 0.9f;

		// Token: 0x04009828 RID: 38952
		public static float MOST_DAY_EXPERIENCE = SKILLS.FULL_EXPERIENCE / 0.75f;

		// Token: 0x04009829 RID: 38953
		public static float PART_DAY_EXPERIENCE = SKILLS.FULL_EXPERIENCE / 0.5f;

		// Token: 0x0400982A RID: 38954
		public static float BARELY_EVER_EXPERIENCE = SKILLS.FULL_EXPERIENCE / 0.25f;

		// Token: 0x0400982B RID: 38955
		public static float APTITUDE_EXPERIENCE_MULTIPLIER = 0.5f;

		// Token: 0x0400982C RID: 38956
		public static int[] SKILL_TIER_MORALE_COST = new int[]
		{
			1,
			2,
			3,
			4,
			5,
			6,
			7
		};
	}
}
