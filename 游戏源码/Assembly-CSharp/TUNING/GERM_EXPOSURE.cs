﻿using System;
using System.Collections.Generic;

namespace TUNING
{
	// Token: 0x02002262 RID: 8802
	public class GERM_EXPOSURE
	{
		// Token: 0x0600B915 RID: 47381 RVA: 0x0046E180 File Offset: 0x0046C380
		// Note: this type is marked as 'beforefieldinit'.
		static GERM_EXPOSURE()
		{
			float[] array = new float[3];
			array[0] = 3f;
			array[1] = 1.5f;
			GERM_EXPOSURE.EXPOSURE_TIER_RESISTANCE_BONUSES = array;
			GERM_EXPOSURE.TYPES = new ExposureType[]
			{
				new ExposureType
				{
					germ_id = "FoodPoisoning",
					sickness_id = "FoodSickness",
					exposure_threshold = 100,
					excluded_traits = new List<string>
					{
						"IronGut"
					},
					base_resistance = 2,
					excluded_effects = new List<string>
					{
						"FoodSicknessRecovery"
					}
				},
				new ExposureType
				{
					germ_id = "SlimeLung",
					sickness_id = "SlimeSickness",
					exposure_threshold = 100,
					base_resistance = 4,
					excluded_effects = new List<string>
					{
						"SlimeSicknessRecovery"
					}
				},
				new ExposureType
				{
					germ_id = "ZombieSpores",
					sickness_id = "ZombieSickness",
					exposure_threshold = 1,
					base_resistance = -2,
					excluded_effects = new List<string>
					{
						"ZombieSicknessRecovery"
					}
				},
				new ExposureType
				{
					germ_id = "RadiationSickness",
					sickness_id = null,
					exposure_threshold = 1,
					base_resistance = -2,
					excluded_effects = new List<string>
					{
						"ZombieSicknessRecovery"
					}
				},
				new ExposureType
				{
					germ_id = "PollenGerms",
					sickness_id = "Allergies",
					exposure_threshold = 2,
					infect_immediately = true,
					required_traits = new List<string>
					{
						"Allergies"
					},
					excluded_effects = new List<string>
					{
						"HistamineSuppression"
					}
				},
				new ExposureType
				{
					germ_id = "PollenGerms",
					infection_effect = "SmelledFlowers",
					exposure_threshold = 2,
					infect_immediately = true,
					excluded_traits = new List<string>
					{
						"Allergies"
					}
				}
			};
		}

		// Token: 0x04009995 RID: 39317
		public const float MIN_EXPOSURE_PERIOD = 540f;

		// Token: 0x04009996 RID: 39318
		public static readonly int[] INHALE_TICK_THRESHOLD = new int[]
		{
			5,
			10,
			15,
			20
		};

		// Token: 0x04009997 RID: 39319
		public static readonly float[] EXPOSURE_TIER_RESISTANCE_BONUSES;

		// Token: 0x04009998 RID: 39320
		public const int MAX_EXPOSURE_TIER = 3;

		// Token: 0x04009999 RID: 39321
		public static ExposureType[] TYPES;
	}
}
