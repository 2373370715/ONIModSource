using System;

namespace TUNING
{
	// Token: 0x02002236 RID: 8758
	public class NOISE_POLLUTION
	{
		// Token: 0x0400984D RID: 38989
		public static readonly EffectorValues NONE = new EffectorValues
		{
			amount = 0,
			radius = 0
		};

		// Token: 0x0400984E RID: 38990
		public static readonly EffectorValues CONE_OF_SILENCE = new EffectorValues
		{
			amount = -120,
			radius = 5
		};

		// Token: 0x0400984F RID: 38991
		public static float DUPLICANT_TIME_THRESHOLD = 3f;

		// Token: 0x02002237 RID: 8759
		public class LENGTHS
		{
			// Token: 0x04009850 RID: 38992
			public static float VERYSHORT = 0.25f;

			// Token: 0x04009851 RID: 38993
			public static float SHORT = 0.5f;

			// Token: 0x04009852 RID: 38994
			public static float NORMAL = 1f;

			// Token: 0x04009853 RID: 38995
			public static float LONG = 1.5f;

			// Token: 0x04009854 RID: 38996
			public static float VERYLONG = 2f;
		}

		// Token: 0x02002238 RID: 8760
		public class NOISY
		{
			// Token: 0x04009855 RID: 38997
			public static readonly EffectorValues TIER0 = new EffectorValues
			{
				amount = 45,
				radius = 10
			};

			// Token: 0x04009856 RID: 38998
			public static readonly EffectorValues TIER1 = new EffectorValues
			{
				amount = 55,
				radius = 10
			};

			// Token: 0x04009857 RID: 38999
			public static readonly EffectorValues TIER2 = new EffectorValues
			{
				amount = 65,
				radius = 10
			};

			// Token: 0x04009858 RID: 39000
			public static readonly EffectorValues TIER3 = new EffectorValues
			{
				amount = 75,
				radius = 15
			};

			// Token: 0x04009859 RID: 39001
			public static readonly EffectorValues TIER4 = new EffectorValues
			{
				amount = 90,
				radius = 15
			};

			// Token: 0x0400985A RID: 39002
			public static readonly EffectorValues TIER5 = new EffectorValues
			{
				amount = 105,
				radius = 20
			};

			// Token: 0x0400985B RID: 39003
			public static readonly EffectorValues TIER6 = new EffectorValues
			{
				amount = 125,
				radius = 20
			};
		}

		// Token: 0x02002239 RID: 8761
		public class CREATURES
		{
			// Token: 0x0400985C RID: 39004
			public static readonly EffectorValues TIER0 = new EffectorValues
			{
				amount = 30,
				radius = 5
			};

			// Token: 0x0400985D RID: 39005
			public static readonly EffectorValues TIER1 = new EffectorValues
			{
				amount = 35,
				radius = 5
			};

			// Token: 0x0400985E RID: 39006
			public static readonly EffectorValues TIER2 = new EffectorValues
			{
				amount = 45,
				radius = 5
			};

			// Token: 0x0400985F RID: 39007
			public static readonly EffectorValues TIER3 = new EffectorValues
			{
				amount = 55,
				radius = 5
			};

			// Token: 0x04009860 RID: 39008
			public static readonly EffectorValues TIER4 = new EffectorValues
			{
				amount = 65,
				radius = 5
			};

			// Token: 0x04009861 RID: 39009
			public static readonly EffectorValues TIER5 = new EffectorValues
			{
				amount = 75,
				radius = 5
			};

			// Token: 0x04009862 RID: 39010
			public static readonly EffectorValues TIER6 = new EffectorValues
			{
				amount = 90,
				radius = 10
			};

			// Token: 0x04009863 RID: 39011
			public static readonly EffectorValues TIER7 = new EffectorValues
			{
				amount = 105,
				radius = 10
			};
		}

		// Token: 0x0200223A RID: 8762
		public class DAMPEN
		{
			// Token: 0x04009864 RID: 39012
			public static readonly EffectorValues TIER0 = new EffectorValues
			{
				amount = -5,
				radius = 1
			};

			// Token: 0x04009865 RID: 39013
			public static readonly EffectorValues TIER1 = new EffectorValues
			{
				amount = -10,
				radius = 2
			};

			// Token: 0x04009866 RID: 39014
			public static readonly EffectorValues TIER2 = new EffectorValues
			{
				amount = -15,
				radius = 3
			};

			// Token: 0x04009867 RID: 39015
			public static readonly EffectorValues TIER3 = new EffectorValues
			{
				amount = -20,
				radius = 4
			};

			// Token: 0x04009868 RID: 39016
			public static readonly EffectorValues TIER4 = new EffectorValues
			{
				amount = -20,
				radius = 5
			};

			// Token: 0x04009869 RID: 39017
			public static readonly EffectorValues TIER5 = new EffectorValues
			{
				amount = -25,
				radius = 6
			};
		}
	}
}
