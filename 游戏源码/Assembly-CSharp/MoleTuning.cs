using System;
using System.Collections.Generic;
using TUNING;

// Token: 0x020000ED RID: 237
public static class MoleTuning
{
	// Token: 0x04000284 RID: 644
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_BASE = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "MoleEgg".ToTag(),
			weight = 0.98f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "MoleDelicacyEgg".ToTag(),
			weight = 0.02f
		}
	};

	// Token: 0x04000285 RID: 645
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_DELICACY = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "MoleEgg".ToTag(),
			weight = 0.32f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "MoleDelicacyEgg".ToTag(),
			weight = 0.65f
		}
	};

	// Token: 0x04000286 RID: 646
	public static float STANDARD_CALORIES_PER_CYCLE = 4800000f;

	// Token: 0x04000287 RID: 647
	public static float STANDARD_STARVE_CYCLES = 10f;

	// Token: 0x04000288 RID: 648
	public static float STANDARD_STOMACH_SIZE = MoleTuning.STANDARD_CALORIES_PER_CYCLE * MoleTuning.STANDARD_STARVE_CYCLES;

	// Token: 0x04000289 RID: 649
	public static float DELICACY_STOMACH_SIZE = MoleTuning.STANDARD_STOMACH_SIZE / 2f;

	// Token: 0x0400028A RID: 650
	public static int PEN_SIZE_PER_CREATURE = CREATURES.SPACE_REQUIREMENTS.TIER2;

	// Token: 0x0400028B RID: 651
	public static float EGG_MASS = 2f;

	// Token: 0x0400028C RID: 652
	public static int DEPTH_TO_HIDE = 2;

	// Token: 0x0400028D RID: 653
	public static HashedString[] GINGER_SYMBOL_NAMES = new HashedString[]
	{
		"del_ginger",
		"del_ginger1",
		"del_ginger2",
		"del_ginger3",
		"del_ginger4",
		"del_ginger5"
	};
}
