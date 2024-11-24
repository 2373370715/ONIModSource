using System;
using System.Collections.Generic;
using TUNING;

// Token: 0x020000FE RID: 254
public static class SealTuning
{
	// Token: 0x040002CA RID: 714
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_BASE = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "SealEgg".ToTag(),
			weight = 1f
		}
	};

	// Token: 0x040002CB RID: 715
	public const float STANDARD_CALORIES_PER_CYCLE = 100000f;

	// Token: 0x040002CC RID: 716
	public const float STANDARD_STARVE_CYCLES = 10f;

	// Token: 0x040002CD RID: 717
	public const float STANDARD_STOMACH_SIZE = 1000000f;

	// Token: 0x040002CE RID: 718
	public static int PEN_SIZE_PER_CREATURE = CREATURES.SPACE_REQUIREMENTS.TIER3;

	// Token: 0x040002CF RID: 719
	public static float EGG_MASS = 2f;
}
