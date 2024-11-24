using System;

namespace TUNING
{
	// Token: 0x0200227F RID: 8831
	public class RADIATION
	{
		// Token: 0x04009A7D RID: 39549
		public const float GERM_RAD_SCALE = 0.01f;

		// Token: 0x04009A7E RID: 39550
		public const float STANDARD_DAILY_RECOVERY = 100f;

		// Token: 0x04009A7F RID: 39551
		public const float EXTRA_VOMIT_RECOVERY = 20f;

		// Token: 0x04009A80 RID: 39552
		public const float REACT_THRESHOLD = 133f;

		// Token: 0x02002280 RID: 8832
		public class STANDARD_EMITTER
		{
			// Token: 0x04009A81 RID: 39553
			public const float STEADY_PULSE_RATE = 0.2f;

			// Token: 0x04009A82 RID: 39554
			public const float DOUBLE_SPEED_PULSE_RATE = 0.1f;

			// Token: 0x04009A83 RID: 39555
			public const float RADIUS_SCALE = 1f;
		}

		// Token: 0x02002281 RID: 8833
		public class RADIATION_PER_SECOND
		{
			// Token: 0x04009A84 RID: 39556
			public const float TRIVIAL = 60f;

			// Token: 0x04009A85 RID: 39557
			public const float VERY_LOW = 120f;

			// Token: 0x04009A86 RID: 39558
			public const float LOW = 240f;

			// Token: 0x04009A87 RID: 39559
			public const float MODERATE = 600f;

			// Token: 0x04009A88 RID: 39560
			public const float HIGH = 1800f;

			// Token: 0x04009A89 RID: 39561
			public const float VERY_HIGH = 4800f;

			// Token: 0x04009A8A RID: 39562
			public const int EXTREME = 9600;
		}

		// Token: 0x02002282 RID: 8834
		public class RADIATION_CONSTANT_RADS_PER_CYCLE
		{
			// Token: 0x04009A8B RID: 39563
			public const float LESS_THAN_TRIVIAL = 60f;

			// Token: 0x04009A8C RID: 39564
			public const float TRIVIAL = 120f;

			// Token: 0x04009A8D RID: 39565
			public const float VERY_LOW = 240f;

			// Token: 0x04009A8E RID: 39566
			public const float LOW = 480f;

			// Token: 0x04009A8F RID: 39567
			public const float MODERATE = 1200f;

			// Token: 0x04009A90 RID: 39568
			public const float MODERATE_PLUS = 2400f;

			// Token: 0x04009A91 RID: 39569
			public const float HIGH = 3600f;

			// Token: 0x04009A92 RID: 39570
			public const float VERY_HIGH = 8400f;

			// Token: 0x04009A93 RID: 39571
			public const int EXTREME = 16800;
		}
	}
}
