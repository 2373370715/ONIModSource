using System;
using System.Collections.Generic;
using TUNING;

// Token: 0x020000F7 RID: 247
public static class PacuTuning
{
	// Token: 0x040002B0 RID: 688
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_BASE = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "PacuEgg".ToTag(),
			weight = 0.98f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "PacuTropicalEgg".ToTag(),
			weight = 0.02f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "PacuCleanerEgg".ToTag(),
			weight = 0.02f
		}
	};

	// Token: 0x040002B1 RID: 689
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_TROPICAL = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "PacuEgg".ToTag(),
			weight = 0.32f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "PacuTropicalEgg".ToTag(),
			weight = 0.65f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "PacuCleanerEgg".ToTag(),
			weight = 0.02f
		}
	};

	// Token: 0x040002B2 RID: 690
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_CLEANER = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "PacuEgg".ToTag(),
			weight = 0.32f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "PacuCleanerEgg".ToTag(),
			weight = 0.65f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "PacuTropicalEgg".ToTag(),
			weight = 0.02f
		}
	};

	// Token: 0x040002B3 RID: 691
	public static float STANDARD_CALORIES_PER_CYCLE = 100000f;

	// Token: 0x040002B4 RID: 692
	public static float STANDARD_STARVE_CYCLES = 5f;

	// Token: 0x040002B5 RID: 693
	public static float STANDARD_STOMACH_SIZE = PacuTuning.STANDARD_CALORIES_PER_CYCLE * PacuTuning.STANDARD_STARVE_CYCLES;

	// Token: 0x040002B6 RID: 694
	public static int PEN_SIZE_PER_CREATURE = CREATURES.SPACE_REQUIREMENTS.TIER2;

	// Token: 0x040002B7 RID: 695
	public static float EGG_MASS = 4f;
}
