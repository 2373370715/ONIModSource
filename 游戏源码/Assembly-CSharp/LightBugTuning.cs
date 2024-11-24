using System;
using System.Collections.Generic;
using TUNING;

// Token: 0x020000E9 RID: 233
public static class LightBugTuning
{
	// Token: 0x04000274 RID: 628
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_BASE = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "LightBugEgg".ToTag(),
			weight = 0.98f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "LightBugOrangeEgg".ToTag(),
			weight = 0.02f
		}
	};

	// Token: 0x04000275 RID: 629
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_ORANGE = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "LightBugEgg".ToTag(),
			weight = 0.33f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "LightBugOrangeEgg".ToTag(),
			weight = 0.66f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "LightBugPurpleEgg".ToTag(),
			weight = 0.02f
		}
	};

	// Token: 0x04000276 RID: 630
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_PURPLE = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "LightBugOrangeEgg".ToTag(),
			weight = 0.33f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "LightBugPurpleEgg".ToTag(),
			weight = 0.66f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "LightBugPinkEgg".ToTag(),
			weight = 0.02f
		}
	};

	// Token: 0x04000277 RID: 631
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_PINK = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "LightBugPurpleEgg".ToTag(),
			weight = 0.33f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "LightBugPinkEgg".ToTag(),
			weight = 0.66f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "LightBugBlueEgg".ToTag(),
			weight = 0.02f
		}
	};

	// Token: 0x04000278 RID: 632
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_BLUE = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "LightBugPinkEgg".ToTag(),
			weight = 0.33f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "LightBugBlueEgg".ToTag(),
			weight = 0.66f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "LightBugBlackEgg".ToTag(),
			weight = 0.02f
		}
	};

	// Token: 0x04000279 RID: 633
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_BLACK = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "LightBugBlueEgg".ToTag(),
			weight = 0.33f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "LightBugBlackEgg".ToTag(),
			weight = 0.66f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "LightBugCrystalEgg".ToTag(),
			weight = 0.02f
		}
	};

	// Token: 0x0400027A RID: 634
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_CRYSTAL = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "LightBugCrystalEgg".ToTag(),
			weight = 0.98f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "LightBugEgg".ToTag(),
			weight = 0.02f
		}
	};

	// Token: 0x0400027B RID: 635
	public static float STANDARD_CALORIES_PER_CYCLE = 40000f;

	// Token: 0x0400027C RID: 636
	public static float STANDARD_STARVE_CYCLES = 8f;

	// Token: 0x0400027D RID: 637
	public static float STANDARD_STOMACH_SIZE = LightBugTuning.STANDARD_CALORIES_PER_CYCLE * LightBugTuning.STANDARD_STARVE_CYCLES;

	// Token: 0x0400027E RID: 638
	public static int PEN_SIZE_PER_CREATURE = CREATURES.SPACE_REQUIREMENTS.TIER3;

	// Token: 0x0400027F RID: 639
	public static float EGG_MASS = 0.2f;
}
