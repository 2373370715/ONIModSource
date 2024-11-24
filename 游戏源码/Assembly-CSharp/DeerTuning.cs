using System;
using System.Collections.Generic;
using TUNING;

// Token: 0x020000DD RID: 221
public static class DeerTuning
{
	// Token: 0x04000248 RID: 584
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_BASE = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "WoodDeerEgg".ToTag(),
			weight = 1f
		}
	};

	// Token: 0x04000249 RID: 585
	public const float STANDARD_CALORIES_PER_CYCLE = 100000f;

	// Token: 0x0400024A RID: 586
	public const float STANDARD_STARVE_CYCLES = 10f;

	// Token: 0x0400024B RID: 587
	public const float STANDARD_STOMACH_SIZE = 1000000f;

	// Token: 0x0400024C RID: 588
	public static int PEN_SIZE_PER_CREATURE = CREATURES.SPACE_REQUIREMENTS.TIER3;

	// Token: 0x0400024D RID: 589
	public static int PEN_SIZE_PER_CREATURE_HUG = CREATURES.SPACE_REQUIREMENTS.TIER1;

	// Token: 0x0400024E RID: 590
	public static float EGG_MASS = 2f;

	// Token: 0x0400024F RID: 591
	public static float DROP_ANTLER_DURATION = 1200f;
}
