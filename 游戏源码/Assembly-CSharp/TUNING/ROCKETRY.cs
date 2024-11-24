using System;
using UnityEngine;

namespace TUNING
{
	// Token: 0x02002291 RID: 8849
	public class ROCKETRY
	{
		// Token: 0x0600B974 RID: 47476 RVA: 0x0011744D File Offset: 0x0011564D
		public static float MassFromPenaltyPercentage(float penaltyPercentage = 0.5f)
		{
			return -(1f / Mathf.Pow(penaltyPercentage - 1f, 5f));
		}

		// Token: 0x0600B975 RID: 47477 RVA: 0x004707A0 File Offset: 0x0046E9A0
		public static float CalculateMassWithPenalty(float realMass)
		{
			float b = Mathf.Pow(realMass / ROCKETRY.MASS_PENALTY_DIVISOR, ROCKETRY.MASS_PENALTY_EXPONENT);
			return Mathf.Max(realMass, b);
		}

		// Token: 0x04009B65 RID: 39781
		public static float MISSION_DURATION_SCALE = 1800f;

		// Token: 0x04009B66 RID: 39782
		public static float MASS_PENALTY_EXPONENT = 3.2f;

		// Token: 0x04009B67 RID: 39783
		public static float MASS_PENALTY_DIVISOR = 300f;

		// Token: 0x04009B68 RID: 39784
		public const float SELF_DESTRUCT_REFUND_FACTOR = 0.5f;

		// Token: 0x04009B69 RID: 39785
		public static float CARGO_CAPACITY_SCALE = 10f;

		// Token: 0x04009B6A RID: 39786
		public static float LIQUID_CARGO_BAY_CLUSTER_CAPACITY = 2700f;

		// Token: 0x04009B6B RID: 39787
		public static float SOLID_CARGO_BAY_CLUSTER_CAPACITY = 2700f;

		// Token: 0x04009B6C RID: 39788
		public static float GAS_CARGO_BAY_CLUSTER_CAPACITY = 1100f;

		// Token: 0x04009B6D RID: 39789
		public const float ENTITIES_CARGO_BAY_CLUSTER_CAPACITY = 100f;

		// Token: 0x04009B6E RID: 39790
		public static Vector2I ROCKET_INTERIOR_SIZE = new Vector2I(32, 32);

		// Token: 0x02002292 RID: 8850
		public class DESTINATION_RESEARCH
		{
			// Token: 0x04009B6F RID: 39791
			public static int EVERGREEN = 10;

			// Token: 0x04009B70 RID: 39792
			public static int BASIC = 50;

			// Token: 0x04009B71 RID: 39793
			public static int HIGH = 150;
		}

		// Token: 0x02002293 RID: 8851
		public class DESTINATION_ANALYSIS
		{
			// Token: 0x04009B72 RID: 39794
			public static int DISCOVERED = 50;

			// Token: 0x04009B73 RID: 39795
			public static int COMPLETE = 100;

			// Token: 0x04009B74 RID: 39796
			public static float DEFAULT_CYCLES_PER_DISCOVERY = 0.5f;
		}

		// Token: 0x02002294 RID: 8852
		public class DESTINATION_THRUST_COSTS
		{
			// Token: 0x04009B75 RID: 39797
			public static int LOW = 3;

			// Token: 0x04009B76 RID: 39798
			public static int MID = 5;

			// Token: 0x04009B77 RID: 39799
			public static int HIGH = 7;

			// Token: 0x04009B78 RID: 39800
			public static int VERY_HIGH = 9;
		}

		// Token: 0x02002295 RID: 8853
		public class CLUSTER_FOW
		{
			// Token: 0x04009B79 RID: 39801
			public static float POINTS_TO_REVEAL = 100f;

			// Token: 0x04009B7A RID: 39802
			public static float DEFAULT_CYCLES_PER_REVEAL = 0.5f;
		}

		// Token: 0x02002296 RID: 8854
		public class ENGINE_EFFICIENCY
		{
			// Token: 0x04009B7B RID: 39803
			public static float WEAK = 20f;

			// Token: 0x04009B7C RID: 39804
			public static float MEDIUM = 40f;

			// Token: 0x04009B7D RID: 39805
			public static float STRONG = 60f;

			// Token: 0x04009B7E RID: 39806
			public static float BOOSTER = 30f;
		}

		// Token: 0x02002297 RID: 8855
		public class ROCKET_HEIGHT
		{
			// Token: 0x04009B7F RID: 39807
			public static int VERY_SHORT = 10;

			// Token: 0x04009B80 RID: 39808
			public static int SHORT = 16;

			// Token: 0x04009B81 RID: 39809
			public static int MEDIUM = 20;

			// Token: 0x04009B82 RID: 39810
			public static int TALL = 25;

			// Token: 0x04009B83 RID: 39811
			public static int VERY_TALL = 35;

			// Token: 0x04009B84 RID: 39812
			public static int MAX_MODULE_STACK_HEIGHT = ROCKETRY.ROCKET_HEIGHT.VERY_TALL - 5;
		}

		// Token: 0x02002298 RID: 8856
		public class OXIDIZER_EFFICIENCY
		{
			// Token: 0x04009B85 RID: 39813
			public static float VERY_LOW = 0.334f;

			// Token: 0x04009B86 RID: 39814
			public static float LOW = 1f;

			// Token: 0x04009B87 RID: 39815
			public static float HIGH = 1.33f;
		}

		// Token: 0x02002299 RID: 8857
		public class DLC1_OXIDIZER_EFFICIENCY
		{
			// Token: 0x04009B88 RID: 39816
			public static float VERY_LOW = 1f;

			// Token: 0x04009B89 RID: 39817
			public static float LOW = 2f;

			// Token: 0x04009B8A RID: 39818
			public static float HIGH = 4f;
		}

		// Token: 0x0200229A RID: 8858
		public class CARGO_CONTAINER_MASS
		{
			// Token: 0x04009B8B RID: 39819
			public static float STATIC_MASS = 1000f;

			// Token: 0x04009B8C RID: 39820
			public static float PAYLOAD_MASS = 1000f;
		}

		// Token: 0x0200229B RID: 8859
		public class BURDEN
		{
			// Token: 0x04009B8D RID: 39821
			public static int INSIGNIFICANT = 1;

			// Token: 0x04009B8E RID: 39822
			public static int MINOR = 2;

			// Token: 0x04009B8F RID: 39823
			public static int MINOR_PLUS = 3;

			// Token: 0x04009B90 RID: 39824
			public static int MODERATE = 4;

			// Token: 0x04009B91 RID: 39825
			public static int MODERATE_PLUS = 5;

			// Token: 0x04009B92 RID: 39826
			public static int MAJOR = 6;

			// Token: 0x04009B93 RID: 39827
			public static int MAJOR_PLUS = 7;

			// Token: 0x04009B94 RID: 39828
			public static int MEGA = 9;

			// Token: 0x04009B95 RID: 39829
			public static int MONUMENTAL = 15;
		}

		// Token: 0x0200229C RID: 8860
		public class ENGINE_POWER
		{
			// Token: 0x04009B96 RID: 39830
			public static int EARLY_WEAK = 16;

			// Token: 0x04009B97 RID: 39831
			public static int EARLY_STRONG = 23;

			// Token: 0x04009B98 RID: 39832
			public static int MID_VERY_STRONG = 48;

			// Token: 0x04009B99 RID: 39833
			public static int MID_STRONG = 31;

			// Token: 0x04009B9A RID: 39834
			public static int MID_WEAK = 27;

			// Token: 0x04009B9B RID: 39835
			public static int LATE_STRONG = 34;

			// Token: 0x04009B9C RID: 39836
			public static int LATE_VERY_STRONG = 55;
		}

		// Token: 0x0200229D RID: 8861
		public class FUEL_COST_PER_DISTANCE
		{
			// Token: 0x04009B9D RID: 39837
			public static float VERY_LOW = 0.033333335f;

			// Token: 0x04009B9E RID: 39838
			public static float LOW = 0.0375f;

			// Token: 0x04009B9F RID: 39839
			public static float MEDIUM = 0.075f;

			// Token: 0x04009BA0 RID: 39840
			public static float HIGH = 0.09375f;

			// Token: 0x04009BA1 RID: 39841
			public static float VERY_HIGH = 0.15f;

			// Token: 0x04009BA2 RID: 39842
			public static float GAS_VERY_LOW = 0.025f;

			// Token: 0x04009BA3 RID: 39843
			public static float GAS_LOW = 0.027777778f;

			// Token: 0x04009BA4 RID: 39844
			public static float GAS_HIGH = 0.041666668f;

			// Token: 0x04009BA5 RID: 39845
			public static float PARTICLES = 0.33333334f;
		}
	}
}
