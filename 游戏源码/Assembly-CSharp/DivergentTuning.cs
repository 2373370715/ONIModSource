using System;
using System.Collections.Generic;
using TUNING;

// Token: 0x020000E0 RID: 224
public static class DivergentTuning
{
	// Token: 0x04000252 RID: 594
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_BEETLE = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "DivergentBeetleEgg".ToTag(),
			weight = 0.98f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "DivergentWormEgg".ToTag(),
			weight = 0.02f
		}
	};

	// Token: 0x04000253 RID: 595
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_WORM = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "DivergentBeetleEgg".ToTag(),
			weight = 0.33f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "DivergentWormEgg".ToTag(),
			weight = 0.67f
		}
	};

	// Token: 0x04000254 RID: 596
	public static int TIMES_TENDED_PER_CYCLE_FOR_EVOLUTION = 2;

	// Token: 0x04000255 RID: 597
	public static float STANDARD_CALORIES_PER_CYCLE = 700000f;

	// Token: 0x04000256 RID: 598
	public static float STANDARD_STARVE_CYCLES = 10f;

	// Token: 0x04000257 RID: 599
	public static float STANDARD_STOMACH_SIZE = DivergentTuning.STANDARD_CALORIES_PER_CYCLE * DivergentTuning.STANDARD_STARVE_CYCLES;

	// Token: 0x04000258 RID: 600
	public static int PEN_SIZE_PER_CREATURE = CREATURES.SPACE_REQUIREMENTS.TIER3;

	// Token: 0x04000259 RID: 601
	public static int PEN_SIZE_PER_CREATURE_WORM = CREATURES.SPACE_REQUIREMENTS.TIER4;

	// Token: 0x0400025A RID: 602
	public static float EGG_MASS = 2f;
}
