using System;
using System.Collections.Generic;
using TUNING;

// Token: 0x020000E6 RID: 230
public static class HatchTuning
{
	// Token: 0x04000269 RID: 617
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_BASE = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "HatchEgg".ToTag(),
			weight = 0.98f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "HatchHardEgg".ToTag(),
			weight = 0.02f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "HatchVeggieEgg".ToTag(),
			weight = 0.02f
		}
	};

	// Token: 0x0400026A RID: 618
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_HARD = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "HatchEgg".ToTag(),
			weight = 0.32f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "HatchHardEgg".ToTag(),
			weight = 0.65f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "HatchMetalEgg".ToTag(),
			weight = 0.02f
		}
	};

	// Token: 0x0400026B RID: 619
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_VEGGIE = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "HatchEgg".ToTag(),
			weight = 0.33f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "HatchVeggieEgg".ToTag(),
			weight = 0.67f
		}
	};

	// Token: 0x0400026C RID: 620
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_METAL = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "HatchEgg".ToTag(),
			weight = 0.11f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "HatchHardEgg".ToTag(),
			weight = 0.22f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "HatchMetalEgg".ToTag(),
			weight = 0.67f
		}
	};

	// Token: 0x0400026D RID: 621
	public static float STANDARD_CALORIES_PER_CYCLE = 700000f;

	// Token: 0x0400026E RID: 622
	public static float STANDARD_STARVE_CYCLES = 10f;

	// Token: 0x0400026F RID: 623
	public static float STANDARD_STOMACH_SIZE = HatchTuning.STANDARD_CALORIES_PER_CYCLE * HatchTuning.STANDARD_STARVE_CYCLES;

	// Token: 0x04000270 RID: 624
	public static int PEN_SIZE_PER_CREATURE = CREATURES.SPACE_REQUIREMENTS.TIER3;

	// Token: 0x04000271 RID: 625
	public static float EGG_MASS = 2f;
}
