using System;
using System.Collections.Generic;
using TUNING;

// Token: 0x020000F2 RID: 242
public static class OilFloaterTuning
{
	// Token: 0x040002A1 RID: 673
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_BASE = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "OilfloaterEgg".ToTag(),
			weight = 0.98f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "OilfloaterHighTempEgg".ToTag(),
			weight = 0.02f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "OilfloaterDecorEgg".ToTag(),
			weight = 0.02f
		}
	};

	// Token: 0x040002A2 RID: 674
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_HIGHTEMP = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "OilfloaterEgg".ToTag(),
			weight = 0.33f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "OilfloaterHighTempEgg".ToTag(),
			weight = 0.66f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "OilfloaterDecorEgg".ToTag(),
			weight = 0.02f
		}
	};

	// Token: 0x040002A3 RID: 675
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_DECOR = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "OilfloaterEgg".ToTag(),
			weight = 0.33f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "OilfloaterHighTempEgg".ToTag(),
			weight = 0.02f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "OilfloaterDecorEgg".ToTag(),
			weight = 0.66f
		}
	};

	// Token: 0x040002A4 RID: 676
	public static float STANDARD_CALORIES_PER_CYCLE = 120000f;

	// Token: 0x040002A5 RID: 677
	public static float STANDARD_STARVE_CYCLES = 5f;

	// Token: 0x040002A6 RID: 678
	public static float STANDARD_STOMACH_SIZE = OilFloaterTuning.STANDARD_CALORIES_PER_CYCLE * OilFloaterTuning.STANDARD_STARVE_CYCLES;

	// Token: 0x040002A7 RID: 679
	public static int PEN_SIZE_PER_CREATURE = CREATURES.SPACE_REQUIREMENTS.TIER3;

	// Token: 0x040002A8 RID: 680
	public static float EGG_MASS = 2f;
}
