using System;

namespace TUNING
{
	// Token: 0x020022A4 RID: 8868
	public class METEORS
	{
		// Token: 0x020022A5 RID: 8869
		public class DIFFICULTY
		{
			// Token: 0x020022A6 RID: 8870
			public class PEROID_MULTIPLIER
			{
				// Token: 0x04009BC0 RID: 39872
				public const float INFREQUENT = 2f;

				// Token: 0x04009BC1 RID: 39873
				public const float INTENSE = 1f;

				// Token: 0x04009BC2 RID: 39874
				public const float DOOMED = 1f;
			}

			// Token: 0x020022A7 RID: 8871
			public class SECONDS_PER_METEOR_MULTIPLIER
			{
				// Token: 0x04009BC3 RID: 39875
				public const float INFREQUENT = 1.5f;

				// Token: 0x04009BC4 RID: 39876
				public const float INTENSE = 0.8f;

				// Token: 0x04009BC5 RID: 39877
				public const float DOOMED = 0.5f;
			}

			// Token: 0x020022A8 RID: 8872
			public class BOMBARD_OFF_MULTIPLIER
			{
				// Token: 0x04009BC6 RID: 39878
				public const float INFREQUENT = 1f;

				// Token: 0x04009BC7 RID: 39879
				public const float INTENSE = 1f;

				// Token: 0x04009BC8 RID: 39880
				public const float DOOMED = 0.5f;
			}

			// Token: 0x020022A9 RID: 8873
			public class BOMBARD_ON_MULTIPLIER
			{
				// Token: 0x04009BC9 RID: 39881
				public const float INFREQUENT = 1f;

				// Token: 0x04009BCA RID: 39882
				public const float INTENSE = 1f;

				// Token: 0x04009BCB RID: 39883
				public const float DOOMED = 1f;
			}

			// Token: 0x020022AA RID: 8874
			public class MASS_MULTIPLIER
			{
				// Token: 0x04009BCC RID: 39884
				public const float INFREQUENT = 1f;

				// Token: 0x04009BCD RID: 39885
				public const float INTENSE = 0.8f;

				// Token: 0x04009BCE RID: 39886
				public const float DOOMED = 0.5f;
			}
		}

		// Token: 0x020022AB RID: 8875
		public class IDENTIFY_DURATION
		{
			// Token: 0x04009BCF RID: 39887
			public const float TIER1 = 20f;
		}

		// Token: 0x020022AC RID: 8876
		public class PEROID
		{
			// Token: 0x04009BD0 RID: 39888
			public const float TIER1 = 5f;

			// Token: 0x04009BD1 RID: 39889
			public const float TIER2 = 10f;

			// Token: 0x04009BD2 RID: 39890
			public const float TIER3 = 20f;

			// Token: 0x04009BD3 RID: 39891
			public const float TIER4 = 30f;
		}

		// Token: 0x020022AD RID: 8877
		public class DURATION
		{
			// Token: 0x04009BD4 RID: 39892
			public const float TIER0 = 1800f;

			// Token: 0x04009BD5 RID: 39893
			public const float TIER1 = 3000f;

			// Token: 0x04009BD6 RID: 39894
			public const float TIER2 = 4200f;

			// Token: 0x04009BD7 RID: 39895
			public const float TIER3 = 6000f;
		}

		// Token: 0x020022AE RID: 8878
		public class DURATION_CLUSTER
		{
			// Token: 0x04009BD8 RID: 39896
			public const float TIER0 = 75f;

			// Token: 0x04009BD9 RID: 39897
			public const float TIER1 = 150f;

			// Token: 0x04009BDA RID: 39898
			public const float TIER2 = 300f;

			// Token: 0x04009BDB RID: 39899
			public const float TIER3 = 600f;

			// Token: 0x04009BDC RID: 39900
			public const float TIER4 = 1800f;

			// Token: 0x04009BDD RID: 39901
			public const float TIER5 = 3000f;
		}

		// Token: 0x020022AF RID: 8879
		public class TRAVEL_DURATION
		{
			// Token: 0x04009BDE RID: 39902
			public const float TIER0 = 600f;

			// Token: 0x04009BDF RID: 39903
			public const float TIER1 = 3000f;

			// Token: 0x04009BE0 RID: 39904
			public const float TIER2 = 4500f;

			// Token: 0x04009BE1 RID: 39905
			public const float TIER3 = 6000f;

			// Token: 0x04009BE2 RID: 39906
			public const float TIER4 = 12000f;

			// Token: 0x04009BE3 RID: 39907
			public const float TIER5 = 30000f;
		}

		// Token: 0x020022B0 RID: 8880
		public class BOMBARDMENT_ON
		{
			// Token: 0x04009BE4 RID: 39908
			public static MathUtil.MinMax NONE = new MathUtil.MinMax(1f, 1f);

			// Token: 0x04009BE5 RID: 39909
			public static MathUtil.MinMax UNLIMITED = new MathUtil.MinMax(10000f, 10000f);

			// Token: 0x04009BE6 RID: 39910
			public static MathUtil.MinMax CYCLE = new MathUtil.MinMax(600f, 600f);
		}

		// Token: 0x020022B1 RID: 8881
		public class BOMBARDMENT_OFF
		{
			// Token: 0x04009BE7 RID: 39911
			public static MathUtil.MinMax NONE = new MathUtil.MinMax(1f, 1f);
		}

		// Token: 0x020022B2 RID: 8882
		public class TRAVELDURATION
		{
			// Token: 0x04009BE8 RID: 39912
			public static float TIER0 = 0f;

			// Token: 0x04009BE9 RID: 39913
			public static float TIER1 = 5f;

			// Token: 0x04009BEA RID: 39914
			public static float TIER2 = 10f;

			// Token: 0x04009BEB RID: 39915
			public static float TIER3 = 20f;

			// Token: 0x04009BEC RID: 39916
			public static float TIER4 = 30f;
		}
	}
}
