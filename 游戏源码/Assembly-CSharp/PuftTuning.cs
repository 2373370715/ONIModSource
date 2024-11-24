using System;
using System.Collections.Generic;
using TUNING;

// Token: 0x020000F9 RID: 249
public static class PuftTuning
{
	// Token: 0x040002BB RID: 699
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_BASE = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "PuftEgg".ToTag(),
			weight = 0.98f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "PuftAlphaEgg".ToTag(),
			weight = 0.02f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "PuftOxyliteEgg".ToTag(),
			weight = 0.02f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "PuftBleachstoneEgg".ToTag(),
			weight = 0.02f
		}
	};

	// Token: 0x040002BC RID: 700
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_ALPHA = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "PuftEgg".ToTag(),
			weight = 0.98f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "PuftAlphaEgg".ToTag(),
			weight = 0.02f
		}
	};

	// Token: 0x040002BD RID: 701
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_OXYLITE = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "PuftEgg".ToTag(),
			weight = 0.31f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "PuftAlphaEgg".ToTag(),
			weight = 0.02f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "PuftOxyliteEgg".ToTag(),
			weight = 0.67f
		}
	};

	// Token: 0x040002BE RID: 702
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_BLEACHSTONE = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "PuftEgg".ToTag(),
			weight = 0.31f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "PuftAlphaEgg".ToTag(),
			weight = 0.02f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "PuftBleachstoneEgg".ToTag(),
			weight = 0.67f
		}
	};

	// Token: 0x040002BF RID: 703
	public static float STANDARD_CALORIES_PER_CYCLE = 200000f;

	// Token: 0x040002C0 RID: 704
	public static float STANDARD_STARVE_CYCLES = 6f;

	// Token: 0x040002C1 RID: 705
	public static float STANDARD_STOMACH_SIZE = PuftTuning.STANDARD_CALORIES_PER_CYCLE * PuftTuning.STANDARD_STARVE_CYCLES;

	// Token: 0x040002C2 RID: 706
	public static int PEN_SIZE_PER_CREATURE = CREATURES.SPACE_REQUIREMENTS.TIER4;

	// Token: 0x040002C3 RID: 707
	public static float EGG_MASS = 0.5f;
}
