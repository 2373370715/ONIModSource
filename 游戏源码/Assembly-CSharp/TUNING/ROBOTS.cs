using System;

namespace TUNING
{
	// Token: 0x0200223B RID: 8763
	public class ROBOTS
	{
		// Token: 0x0200223C RID: 8764
		public class SCOUTBOT
		{
			// Token: 0x0400986A RID: 39018
			public static float CARRY_CAPACITY = DUPLICANTSTATS.STANDARD.BaseStats.CARRY_CAPACITY;

			// Token: 0x0400986B RID: 39019
			public static readonly float DIGGING = 1f;

			// Token: 0x0400986C RID: 39020
			public static readonly float CONSTRUCTION = 1f;

			// Token: 0x0400986D RID: 39021
			public static readonly float ATHLETICS = 1f;

			// Token: 0x0400986E RID: 39022
			public static readonly float HIT_POINTS = 100f;

			// Token: 0x0400986F RID: 39023
			public static readonly float BATTERY_DEPLETION_RATE = 30f;

			// Token: 0x04009870 RID: 39024
			public static readonly float BATTERY_CAPACITY = ROBOTS.SCOUTBOT.BATTERY_DEPLETION_RATE * 10f * 600f;
		}

		// Token: 0x0200223D RID: 8765
		public class MORBBOT
		{
			// Token: 0x04009871 RID: 39025
			public static float CARRY_CAPACITY = DUPLICANTSTATS.STANDARD.BaseStats.CARRY_CAPACITY * 2f;

			// Token: 0x04009872 RID: 39026
			public const float DIGGING = 1f;

			// Token: 0x04009873 RID: 39027
			public const float CONSTRUCTION = 1f;

			// Token: 0x04009874 RID: 39028
			public const float ATHLETICS = 3f;

			// Token: 0x04009875 RID: 39029
			public static readonly float HIT_POINTS = 100f;

			// Token: 0x04009876 RID: 39030
			public const float LIFETIME = 6000f;

			// Token: 0x04009877 RID: 39031
			public const float BATTERY_DEPLETION_RATE = 30f;

			// Token: 0x04009878 RID: 39032
			public const float BATTERY_CAPACITY = 180000f;

			// Token: 0x04009879 RID: 39033
			public const float DECONSTRUCTION_WORK_TIME = 10f;
		}

		// Token: 0x0200223E RID: 8766
		public class FETCHDRONE
		{
			// Token: 0x0400987A RID: 39034
			public static float CARRY_CAPACITY = DUPLICANTSTATS.STANDARD.BaseStats.CARRY_CAPACITY * 2f;

			// Token: 0x0400987B RID: 39035
			public static readonly float HIT_POINTS = 100f;

			// Token: 0x0400987C RID: 39036
			public const float BATTERY_DEPLETION_RATE = 30f;
		}
	}
}
