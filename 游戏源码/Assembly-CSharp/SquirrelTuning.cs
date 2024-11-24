using System;
using System.Collections.Generic;
using TUNING;

// Token: 0x02000100 RID: 256
public static class SquirrelTuning
{
	// Token: 0x040002D0 RID: 720
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_BASE = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "SquirrelEgg".ToTag(),
			weight = 0.98f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "SquirrelHugEgg".ToTag(),
			weight = 0.02f
		}
	};

	// Token: 0x040002D1 RID: 721
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_HUG = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "SquirrelEgg".ToTag(),
			weight = 0.35f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "SquirrelHugEgg".ToTag(),
			weight = 0.65f
		}
	};

	// Token: 0x040002D2 RID: 722
	public static float STANDARD_CALORIES_PER_CYCLE = 100000f;

	// Token: 0x040002D3 RID: 723
	public static float STANDARD_STARVE_CYCLES = 10f;

	// Token: 0x040002D4 RID: 724
	public static float STANDARD_STOMACH_SIZE = SquirrelTuning.STANDARD_CALORIES_PER_CYCLE * SquirrelTuning.STANDARD_STARVE_CYCLES;

	// Token: 0x040002D5 RID: 725
	public static int PEN_SIZE_PER_CREATURE = CREATURES.SPACE_REQUIREMENTS.TIER3;

	// Token: 0x040002D6 RID: 726
	public static int PEN_SIZE_PER_CREATURE_HUG = CREATURES.SPACE_REQUIREMENTS.TIER1;

	// Token: 0x040002D7 RID: 727
	public static float EGG_MASS = 2f;
}
