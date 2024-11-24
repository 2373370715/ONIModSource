using System;
using System.Collections.Generic;
using TUNING;

// Token: 0x020000D7 RID: 215
public static class BellyTuning
{
	// Token: 0x0400022E RID: 558
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_BASE = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "IceBellyEgg".ToTag(),
			weight = 1f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "GoldBellyEgg".ToTag(),
			weight = 0f
		}
	};

	// Token: 0x0400022F RID: 559
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_GOLD = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "IceBellyEgg".ToTag(),
			weight = 0.02f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "GoldBellyEgg".ToTag(),
			weight = 0.98f
		}
	};

	// Token: 0x04000230 RID: 560
	public const float KW_GENERATED_TO_WARM_UP = 1.3f;

	// Token: 0x04000231 RID: 561
	public static float STANDARD_CALORIES_PER_CYCLE = 4f * FOOD.FOOD_TYPES.CARROT.CaloriesPerUnit / (CROPS.CROP_TYPES.Find((Crop.CropVal m) => m.cropId == CarrotConfig.ID).cropDuration / 600f);

	// Token: 0x04000232 RID: 562
	public const float STANDARD_STARVE_CYCLES = 10f;

	// Token: 0x04000233 RID: 563
	public static float STANDARD_STOMACH_SIZE = BellyTuning.STANDARD_CALORIES_PER_CYCLE * 10f;

	// Token: 0x04000234 RID: 564
	public static int PEN_SIZE_PER_CREATURE = CREATURES.SPACE_REQUIREMENTS.TIER3;

	// Token: 0x04000235 RID: 565
	public const float EGG_MASS = 8f;

	// Token: 0x04000236 RID: 566
	public const int GERMS_EMMITED_PER_KG_POOPED = 1000;

	// Token: 0x04000237 RID: 567
	public static string GERM_ID_EMMITED_ON_POOP = "PollenGerms";

	// Token: 0x04000238 RID: 568
	public static float CALORIES_PER_UNIT_EATEN = FOOD.FOOD_TYPES.CARROT.CaloriesPerUnit;

	// Token: 0x04000239 RID: 569
	public static float CONSUMABLE_PLANT_MATURITY_LEVELS = CROPS.CROP_TYPES.Find((Crop.CropVal m) => m.cropId == CarrotConfig.ID).cropDuration / 600f;

	// Token: 0x0400023A RID: 570
	public const float CONSUMED_MASS_TO_POOP_MASS_MULTIPLIER = 67.474f;

	// Token: 0x0400023B RID: 571
	public const float MIN_POOP_SIZE_IN_KG = 1f;
}
