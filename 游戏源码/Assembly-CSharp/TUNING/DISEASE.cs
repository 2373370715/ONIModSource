using System;

namespace TUNING
{
	// Token: 0x0200225B RID: 8795
	public class DISEASE
	{
		// Token: 0x04009958 RID: 39256
		public const int COUNT_SCALER = 1000;

		// Token: 0x04009959 RID: 39257
		public const int GENERIC_EMIT_COUNT = 100000;

		// Token: 0x0400995A RID: 39258
		public const float GENERIC_EMIT_INTERVAL = 5f;

		// Token: 0x0400995B RID: 39259
		public const float GENERIC_INFECTION_RADIUS = 1.5f;

		// Token: 0x0400995C RID: 39260
		public const float GENERIC_INFECTION_INTERVAL = 5f;

		// Token: 0x0400995D RID: 39261
		public const float STINKY_EMIT_MASS = 0.0025000002f;

		// Token: 0x0400995E RID: 39262
		public const float STINKY_EMIT_INTERVAL = 2.5f;

		// Token: 0x0400995F RID: 39263
		public const float STORAGE_TRANSFER_RATE = 0.05f;

		// Token: 0x04009960 RID: 39264
		public const float WORKABLE_TRANSFER_RATE = 0.33f;

		// Token: 0x04009961 RID: 39265
		public const float LADDER_TRANSFER_RATE = 0.005f;

		// Token: 0x04009962 RID: 39266
		public const float INTERNAL_GERM_DEATH_MULTIPLIER = -0.00066666666f;

		// Token: 0x04009963 RID: 39267
		public const float INTERNAL_GERM_DEATH_ADDEND = -0.8333333f;

		// Token: 0x04009964 RID: 39268
		public const float MINIMUM_IMMUNE_DAMAGE = 0.00016666666f;

		// Token: 0x0200225C RID: 8796
		public class DURATION
		{
			// Token: 0x04009965 RID: 39269
			public const float LONG = 10800f;

			// Token: 0x04009966 RID: 39270
			public const float LONGISH = 4620f;

			// Token: 0x04009967 RID: 39271
			public const float NORMAL = 2220f;

			// Token: 0x04009968 RID: 39272
			public const float SHORT = 1020f;

			// Token: 0x04009969 RID: 39273
			public const float TEMPORARY = 180f;

			// Token: 0x0400996A RID: 39274
			public const float VERY_BRIEF = 60f;
		}

		// Token: 0x0200225D RID: 8797
		public class IMMUNE_ATTACK_STRENGTH_PERCENT
		{
			// Token: 0x0400996B RID: 39275
			public const float SLOW_3 = 0.00025f;

			// Token: 0x0400996C RID: 39276
			public const float SLOW_2 = 0.0005f;

			// Token: 0x0400996D RID: 39277
			public const float SLOW_1 = 0.00125f;

			// Token: 0x0400996E RID: 39278
			public const float NORMAL = 0.005f;

			// Token: 0x0400996F RID: 39279
			public const float FAST_1 = 0.0125f;

			// Token: 0x04009970 RID: 39280
			public const float FAST_2 = 0.05f;

			// Token: 0x04009971 RID: 39281
			public const float FAST_3 = 0.125f;
		}

		// Token: 0x0200225E RID: 8798
		public class RADIATION_KILL_RATE
		{
			// Token: 0x04009972 RID: 39282
			public const float NO_EFFECT = 0f;

			// Token: 0x04009973 RID: 39283
			public const float SLOW = 1f;

			// Token: 0x04009974 RID: 39284
			public const float NORMAL = 2.5f;

			// Token: 0x04009975 RID: 39285
			public const float FAST = 5f;
		}

		// Token: 0x0200225F RID: 8799
		public static class GROWTH_FACTOR
		{
			// Token: 0x04009976 RID: 39286
			public const float NONE = float.PositiveInfinity;

			// Token: 0x04009977 RID: 39287
			public const float DEATH_1 = 12000f;

			// Token: 0x04009978 RID: 39288
			public const float DEATH_2 = 6000f;

			// Token: 0x04009979 RID: 39289
			public const float DEATH_3 = 3000f;

			// Token: 0x0400997A RID: 39290
			public const float DEATH_4 = 1200f;

			// Token: 0x0400997B RID: 39291
			public const float DEATH_5 = 300f;

			// Token: 0x0400997C RID: 39292
			public const float DEATH_MAX = 10f;

			// Token: 0x0400997D RID: 39293
			public const float DEATH_INSTANT = 0f;

			// Token: 0x0400997E RID: 39294
			public const float GROWTH_1 = -12000f;

			// Token: 0x0400997F RID: 39295
			public const float GROWTH_2 = -6000f;

			// Token: 0x04009980 RID: 39296
			public const float GROWTH_3 = -3000f;

			// Token: 0x04009981 RID: 39297
			public const float GROWTH_4 = -1200f;

			// Token: 0x04009982 RID: 39298
			public const float GROWTH_5 = -600f;

			// Token: 0x04009983 RID: 39299
			public const float GROWTH_6 = -300f;

			// Token: 0x04009984 RID: 39300
			public const float GROWTH_7 = -150f;
		}

		// Token: 0x02002260 RID: 8800
		public static class UNDERPOPULATION_DEATH_RATE
		{
			// Token: 0x04009985 RID: 39301
			public const float NONE = 0f;

			// Token: 0x04009986 RID: 39302
			private const float BASE_NUM_TO_KILL = 400f;

			// Token: 0x04009987 RID: 39303
			public const float SLOW = 0.6666667f;

			// Token: 0x04009988 RID: 39304
			public const float FAST = 2.6666667f;
		}
	}
}
