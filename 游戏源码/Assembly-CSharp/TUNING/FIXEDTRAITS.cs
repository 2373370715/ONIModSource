using System;

namespace TUNING
{
	// Token: 0x02002219 RID: 8729
	public class FIXEDTRAITS
	{
		// Token: 0x0200221A RID: 8730
		public class NORTHERNLIGHTS
		{
			// Token: 0x040097A4 RID: 38820
			public static int NONE = 0;

			// Token: 0x040097A5 RID: 38821
			public static int ENABLED = 1;

			// Token: 0x040097A6 RID: 38822
			public static int DEFAULT_VALUE = FIXEDTRAITS.NORTHERNLIGHTS.NONE;

			// Token: 0x0200221B RID: 8731
			public class NAME
			{
				// Token: 0x040097A7 RID: 38823
				public static string NONE = "northernLightsNone";

				// Token: 0x040097A8 RID: 38824
				public static string ENABLED = "northernLightsOn";

				// Token: 0x040097A9 RID: 38825
				public static string DEFAULT = FIXEDTRAITS.NORTHERNLIGHTS.NAME.NONE;
			}
		}

		// Token: 0x0200221C RID: 8732
		public class SUNLIGHT
		{
			// Token: 0x040097AA RID: 38826
			public static int DEFAULT_SPACED_OUT_SUNLIGHT = 40000;

			// Token: 0x040097AB RID: 38827
			public static int NONE = 0;

			// Token: 0x040097AC RID: 38828
			public static int VERY_VERY_LOW = (int)((float)FIXEDTRAITS.SUNLIGHT.DEFAULT_SPACED_OUT_SUNLIGHT * 0.25f);

			// Token: 0x040097AD RID: 38829
			public static int VERY_LOW = (int)((float)FIXEDTRAITS.SUNLIGHT.DEFAULT_SPACED_OUT_SUNLIGHT * 0.5f);

			// Token: 0x040097AE RID: 38830
			public static int LOW = (int)((float)FIXEDTRAITS.SUNLIGHT.DEFAULT_SPACED_OUT_SUNLIGHT * 0.75f);

			// Token: 0x040097AF RID: 38831
			public static int MED_LOW = (int)((float)FIXEDTRAITS.SUNLIGHT.DEFAULT_SPACED_OUT_SUNLIGHT * 0.875f);

			// Token: 0x040097B0 RID: 38832
			public static int MED = FIXEDTRAITS.SUNLIGHT.DEFAULT_SPACED_OUT_SUNLIGHT;

			// Token: 0x040097B1 RID: 38833
			public static int MED_HIGH = (int)((float)FIXEDTRAITS.SUNLIGHT.DEFAULT_SPACED_OUT_SUNLIGHT * 1.25f);

			// Token: 0x040097B2 RID: 38834
			public static int HIGH = (int)((float)FIXEDTRAITS.SUNLIGHT.DEFAULT_SPACED_OUT_SUNLIGHT * 1.5f);

			// Token: 0x040097B3 RID: 38835
			public static int VERY_HIGH = FIXEDTRAITS.SUNLIGHT.DEFAULT_SPACED_OUT_SUNLIGHT * 2;

			// Token: 0x040097B4 RID: 38836
			public static int VERY_VERY_HIGH = (int)((float)FIXEDTRAITS.SUNLIGHT.DEFAULT_SPACED_OUT_SUNLIGHT * 2.5f);

			// Token: 0x040097B5 RID: 38837
			public static int VERY_VERY_VERY_HIGH = FIXEDTRAITS.SUNLIGHT.DEFAULT_SPACED_OUT_SUNLIGHT * 3;

			// Token: 0x040097B6 RID: 38838
			public static int DEFAULT_VALUE = FIXEDTRAITS.SUNLIGHT.VERY_HIGH;

			// Token: 0x0200221D RID: 8733
			public class NAME
			{
				// Token: 0x040097B7 RID: 38839
				public static string NONE = "sunlightNone";

				// Token: 0x040097B8 RID: 38840
				public static string VERY_VERY_LOW = "sunlightVeryVeryLow";

				// Token: 0x040097B9 RID: 38841
				public static string VERY_LOW = "sunlightVeryLow";

				// Token: 0x040097BA RID: 38842
				public static string LOW = "sunlightLow";

				// Token: 0x040097BB RID: 38843
				public static string MED_LOW = "sunlightMedLow";

				// Token: 0x040097BC RID: 38844
				public static string MED = "sunlightMed";

				// Token: 0x040097BD RID: 38845
				public static string MED_HIGH = "sunlightMedHigh";

				// Token: 0x040097BE RID: 38846
				public static string HIGH = "sunlightHigh";

				// Token: 0x040097BF RID: 38847
				public static string VERY_HIGH = "sunlightVeryHigh";

				// Token: 0x040097C0 RID: 38848
				public static string VERY_VERY_HIGH = "sunlightVeryVeryHigh";

				// Token: 0x040097C1 RID: 38849
				public static string VERY_VERY_VERY_HIGH = "sunlightVeryVeryVeryHigh";

				// Token: 0x040097C2 RID: 38850
				public static string DEFAULT = FIXEDTRAITS.SUNLIGHT.NAME.VERY_HIGH;
			}
		}

		// Token: 0x0200221E RID: 8734
		public class COSMICRADIATION
		{
			// Token: 0x040097C3 RID: 38851
			public static int BASELINE = 250;

			// Token: 0x040097C4 RID: 38852
			public static int NONE = 0;

			// Token: 0x040097C5 RID: 38853
			public static int VERY_VERY_LOW = (int)((float)FIXEDTRAITS.COSMICRADIATION.BASELINE * 0.25f);

			// Token: 0x040097C6 RID: 38854
			public static int VERY_LOW = (int)((float)FIXEDTRAITS.COSMICRADIATION.BASELINE * 0.5f);

			// Token: 0x040097C7 RID: 38855
			public static int LOW = (int)((float)FIXEDTRAITS.COSMICRADIATION.BASELINE * 0.75f);

			// Token: 0x040097C8 RID: 38856
			public static int MED_LOW = (int)((float)FIXEDTRAITS.COSMICRADIATION.BASELINE * 0.875f);

			// Token: 0x040097C9 RID: 38857
			public static int MED = FIXEDTRAITS.COSMICRADIATION.BASELINE;

			// Token: 0x040097CA RID: 38858
			public static int MED_HIGH = (int)((float)FIXEDTRAITS.COSMICRADIATION.BASELINE * 1.25f);

			// Token: 0x040097CB RID: 38859
			public static int HIGH = (int)((float)FIXEDTRAITS.COSMICRADIATION.BASELINE * 1.5f);

			// Token: 0x040097CC RID: 38860
			public static int VERY_HIGH = FIXEDTRAITS.COSMICRADIATION.BASELINE * 2;

			// Token: 0x040097CD RID: 38861
			public static int VERY_VERY_HIGH = FIXEDTRAITS.COSMICRADIATION.BASELINE * 3;

			// Token: 0x040097CE RID: 38862
			public static int DEFAULT_VALUE = FIXEDTRAITS.COSMICRADIATION.MED;

			// Token: 0x040097CF RID: 38863
			public static float TELESCOPE_RADIATION_SHIELDING = 0.5f;

			// Token: 0x0200221F RID: 8735
			public class NAME
			{
				// Token: 0x040097D0 RID: 38864
				public static string NONE = "cosmicRadiationNone";

				// Token: 0x040097D1 RID: 38865
				public static string VERY_VERY_LOW = "cosmicRadiationVeryVeryLow";

				// Token: 0x040097D2 RID: 38866
				public static string VERY_LOW = "cosmicRadiationVeryLow";

				// Token: 0x040097D3 RID: 38867
				public static string LOW = "cosmicRadiationLow";

				// Token: 0x040097D4 RID: 38868
				public static string MED_LOW = "cosmicRadiationMedLow";

				// Token: 0x040097D5 RID: 38869
				public static string MED = "cosmicRadiationMed";

				// Token: 0x040097D6 RID: 38870
				public static string MED_HIGH = "cosmicRadiationMedHigh";

				// Token: 0x040097D7 RID: 38871
				public static string HIGH = "cosmicRadiationHigh";

				// Token: 0x040097D8 RID: 38872
				public static string VERY_HIGH = "cosmicRadiationVeryHigh";

				// Token: 0x040097D9 RID: 38873
				public static string VERY_VERY_HIGH = "cosmicRadiationVeryVeryHigh";

				// Token: 0x040097DA RID: 38874
				public static string DEFAULT = FIXEDTRAITS.COSMICRADIATION.NAME.MED;
			}
		}
	}
}
