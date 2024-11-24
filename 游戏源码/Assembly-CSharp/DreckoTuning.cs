using System;
using System.Collections.Generic;
using TUNING;

// Token: 0x020000E3 RID: 227
public static class DreckoTuning
{
	// Token: 0x04000260 RID: 608
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_BASE = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "DreckoEgg".ToTag(),
			weight = 0.98f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "DreckoPlasticEgg".ToTag(),
			weight = 0.02f
		}
	};

	// Token: 0x04000261 RID: 609
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_PLASTIC = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "DreckoEgg".ToTag(),
			weight = 0.35f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "DreckoPlasticEgg".ToTag(),
			weight = 0.65f
		}
	};

	// Token: 0x04000262 RID: 610
	public static float STANDARD_CALORIES_PER_CYCLE = 2000000f;

	// Token: 0x04000263 RID: 611
	public static float STANDARD_STARVE_CYCLES = 5f;

	// Token: 0x04000264 RID: 612
	public static float STANDARD_STOMACH_SIZE = DreckoTuning.STANDARD_CALORIES_PER_CYCLE * DreckoTuning.STANDARD_STARVE_CYCLES;

	// Token: 0x04000265 RID: 613
	public static int PEN_SIZE_PER_CREATURE = CREATURES.SPACE_REQUIREMENTS.TIER3;

	// Token: 0x04000266 RID: 614
	public static float EGG_MASS = 2f;
}
