using System;
using System.Collections.Generic;

namespace TUNING
{
	// Token: 0x02002263 RID: 8803
	public class FOOD
	{
		// Token: 0x0400999A RID: 39322
		public const float EATING_SECONDS_PER_CALORIE = 2E-05f;

		// Token: 0x0400999B RID: 39323
		public static float FOOD_CALORIES_PER_CYCLE = -DUPLICANTSTATS.STANDARD.BaseStats.CALORIES_BURNED_PER_CYCLE;

		// Token: 0x0400999C RID: 39324
		public const int FOOD_AMOUNT_INGREDIENT_ONLY = 0;

		// Token: 0x0400999D RID: 39325
		public const float KCAL_SMALL_PORTION = 600000f;

		// Token: 0x0400999E RID: 39326
		public const float KCAL_BONUS_COOKING_LOW = 250000f;

		// Token: 0x0400999F RID: 39327
		public const float KCAL_BASIC_PORTION = 800000f;

		// Token: 0x040099A0 RID: 39328
		public const float KCAL_PREPARED_FOOD = 4000000f;

		// Token: 0x040099A1 RID: 39329
		public const float KCAL_BONUS_COOKING_BASIC = 400000f;

		// Token: 0x040099A2 RID: 39330
		public const float KCAL_BONUS_COOKING_DEEPFRIED = 1200000f;

		// Token: 0x040099A3 RID: 39331
		public const float DEFAULT_PRESERVE_TEMPERATURE = 255.15f;

		// Token: 0x040099A4 RID: 39332
		public const float DEFAULT_ROT_TEMPERATURE = 277.15f;

		// Token: 0x040099A5 RID: 39333
		public const float HIGH_PRESERVE_TEMPERATURE = 283.15f;

		// Token: 0x040099A6 RID: 39334
		public const float HIGH_ROT_TEMPERATURE = 308.15f;

		// Token: 0x040099A7 RID: 39335
		public const float EGG_COOK_TEMPERATURE = 344.15f;

		// Token: 0x040099A8 RID: 39336
		public const float DEFAULT_MASS = 1f;

		// Token: 0x040099A9 RID: 39337
		public const float DEFAULT_SPICE_MASS = 1f;

		// Token: 0x040099AA RID: 39338
		public const float ROT_TO_ELEMENT_TIME = 600f;

		// Token: 0x040099AB RID: 39339
		public const int MUSH_BAR_SPAWN_GERMS = 1000;

		// Token: 0x040099AC RID: 39340
		public const float IDEAL_TEMPERATURE_TOLERANCE = 10f;

		// Token: 0x040099AD RID: 39341
		public const int FOOD_QUALITY_AWFUL = -1;

		// Token: 0x040099AE RID: 39342
		public const int FOOD_QUALITY_TERRIBLE = 0;

		// Token: 0x040099AF RID: 39343
		public const int FOOD_QUALITY_MEDIOCRE = 1;

		// Token: 0x040099B0 RID: 39344
		public const int FOOD_QUALITY_GOOD = 2;

		// Token: 0x040099B1 RID: 39345
		public const int FOOD_QUALITY_GREAT = 3;

		// Token: 0x040099B2 RID: 39346
		public const int FOOD_QUALITY_AMAZING = 4;

		// Token: 0x040099B3 RID: 39347
		public const int FOOD_QUALITY_WONDERFUL = 5;

		// Token: 0x040099B4 RID: 39348
		public const int FOOD_QUALITY_MORE_WONDERFUL = 6;

		// Token: 0x02002264 RID: 8804
		public class SPOIL_TIME
		{
			// Token: 0x040099B5 RID: 39349
			public const float DEFAULT = 4800f;

			// Token: 0x040099B6 RID: 39350
			public const float QUICK = 2400f;

			// Token: 0x040099B7 RID: 39351
			public const float SLOW = 9600f;

			// Token: 0x040099B8 RID: 39352
			public const float VERYSLOW = 19200f;
		}

		// Token: 0x02002265 RID: 8805
		public class FOOD_TYPES
		{
			// Token: 0x040099B9 RID: 39353
			public static readonly EdiblesManager.FoodInfo FIELDRATION = new EdiblesManager.FoodInfo("FieldRation", "", 800000f, -1, 255.15f, 277.15f, 19200f, false);

			// Token: 0x040099BA RID: 39354
			public static readonly EdiblesManager.FoodInfo MUSHBAR = new EdiblesManager.FoodInfo("MushBar", "", 800000f, -1, 255.15f, 277.15f, 4800f, true);

			// Token: 0x040099BB RID: 39355
			public static readonly EdiblesManager.FoodInfo BASICPLANTFOOD = new EdiblesManager.FoodInfo("BasicPlantFood", "", 600000f, -1, 255.15f, 277.15f, 4800f, true);

			// Token: 0x040099BC RID: 39356
			public static readonly EdiblesManager.FoodInfo BASICFORAGEPLANT = new EdiblesManager.FoodInfo("BasicForagePlant", "", 800000f, -1, 255.15f, 277.15f, 4800f, false);

			// Token: 0x040099BD RID: 39357
			public static readonly EdiblesManager.FoodInfo FORESTFORAGEPLANT = new EdiblesManager.FoodInfo("ForestForagePlant", "", 6400000f, -1, 255.15f, 277.15f, 4800f, false);

			// Token: 0x040099BE RID: 39358
			public static readonly EdiblesManager.FoodInfo SWAMPFORAGEPLANT = new EdiblesManager.FoodInfo("SwampForagePlant", "EXPANSION1_ID", 2400000f, -1, 255.15f, 277.15f, 4800f, false);

			// Token: 0x040099BF RID: 39359
			public static readonly EdiblesManager.FoodInfo ICECAVESFORAGEPLANT = new EdiblesManager.FoodInfo("IceCavesForagePlant", "DLC2_ID", 800000f, -1, 255.15f, 277.15f, 4800f, false);

			// Token: 0x040099C0 RID: 39360
			public static readonly EdiblesManager.FoodInfo MUSHROOM = new EdiblesManager.FoodInfo(MushroomConfig.ID, "", 2400000f, 0, 255.15f, 277.15f, 4800f, true);

			// Token: 0x040099C1 RID: 39361
			public static readonly EdiblesManager.FoodInfo LETTUCE = new EdiblesManager.FoodInfo("Lettuce", "", 400000f, 0, 255.15f, 277.15f, 2400f, true).AddEffects(new List<string>
			{
				"SeafoodRadiationResistance"
			}, DlcManager.AVAILABLE_EXPANSION1_ONLY);

			// Token: 0x040099C2 RID: 39362
			public static readonly EdiblesManager.FoodInfo RAWEGG = new EdiblesManager.FoodInfo("RawEgg", "", 1600000f, -1, 255.15f, 277.15f, 4800f, true);

			// Token: 0x040099C3 RID: 39363
			public static readonly EdiblesManager.FoodInfo MEAT = new EdiblesManager.FoodInfo("Meat", "", 1600000f, -1, 255.15f, 277.15f, 4800f, true);

			// Token: 0x040099C4 RID: 39364
			public static readonly EdiblesManager.FoodInfo PLANTMEAT = new EdiblesManager.FoodInfo("PlantMeat", "EXPANSION1_ID", 1200000f, 1, 255.15f, 277.15f, 2400f, true);

			// Token: 0x040099C5 RID: 39365
			public static readonly EdiblesManager.FoodInfo PRICKLEFRUIT = new EdiblesManager.FoodInfo(PrickleFruitConfig.ID, "", 1600000f, 0, 255.15f, 277.15f, 4800f, true);

			// Token: 0x040099C6 RID: 39366
			public static readonly EdiblesManager.FoodInfo SWAMPFRUIT = new EdiblesManager.FoodInfo(SwampFruitConfig.ID, "EXPANSION1_ID", 1840000f, 0, 255.15f, 277.15f, 2400f, true);

			// Token: 0x040099C7 RID: 39367
			public static readonly EdiblesManager.FoodInfo FISH_MEAT = new EdiblesManager.FoodInfo("FishMeat", "", 1000000f, 2, 255.15f, 277.15f, 2400f, true).AddEffects(new List<string>
			{
				"SeafoodRadiationResistance"
			}, DlcManager.AVAILABLE_EXPANSION1_ONLY);

			// Token: 0x040099C8 RID: 39368
			public static readonly EdiblesManager.FoodInfo SHELLFISH_MEAT = new EdiblesManager.FoodInfo("ShellfishMeat", "", 1000000f, 2, 255.15f, 277.15f, 2400f, true).AddEffects(new List<string>
			{
				"SeafoodRadiationResistance"
			}, DlcManager.AVAILABLE_EXPANSION1_ONLY);

			// Token: 0x040099C9 RID: 39369
			public static readonly EdiblesManager.FoodInfo WORMBASICFRUIT = new EdiblesManager.FoodInfo("WormBasicFruit", "EXPANSION1_ID", 800000f, 0, 255.15f, 277.15f, 4800f, true);

			// Token: 0x040099CA RID: 39370
			public static readonly EdiblesManager.FoodInfo WORMSUPERFRUIT = new EdiblesManager.FoodInfo("WormSuperFruit", "EXPANSION1_ID", 250000f, 1, 255.15f, 277.15f, 2400f, true);

			// Token: 0x040099CB RID: 39371
			public static readonly EdiblesManager.FoodInfo HARDSKINBERRY = new EdiblesManager.FoodInfo("HardSkinBerry", "DLC2_ID", 800000f, -1, 255.15f, 277.15f, 9600f, true);

			// Token: 0x040099CC RID: 39372
			public static readonly EdiblesManager.FoodInfo CARROT = new EdiblesManager.FoodInfo(CarrotConfig.ID, "DLC2_ID", 4000000f, 0, 255.15f, 277.15f, 9600f, true);

			// Token: 0x040099CD RID: 39373
			public static readonly EdiblesManager.FoodInfo PEMMICAN = new EdiblesManager.FoodInfo("Pemmican", "DLC2_ID", FOOD.FOOD_TYPES.HARDSKINBERRY.CaloriesPerUnit * 2f + 1000000f, 2, 255.15f, 277.15f, 19200f, false);

			// Token: 0x040099CE RID: 39374
			public static readonly EdiblesManager.FoodInfo FRIES_CARROT = new EdiblesManager.FoodInfo("FriesCarrot", "DLC2_ID", 5400000f, 3, 255.15f, 277.15f, 2400f, true);

			// Token: 0x040099CF RID: 39375
			public static readonly EdiblesManager.FoodInfo DEEP_FRIED_MEAT = new EdiblesManager.FoodInfo("DeepFriedMeat", "DLC2_ID", 4000000f, 3, 255.15f, 277.15f, 2400f, true);

			// Token: 0x040099D0 RID: 39376
			public static readonly EdiblesManager.FoodInfo DEEP_FRIED_NOSH = new EdiblesManager.FoodInfo("DeepFriedNosh", "DLC2_ID", 5000000f, 3, 255.15f, 277.15f, 4800f, true);

			// Token: 0x040099D1 RID: 39377
			public static readonly EdiblesManager.FoodInfo DEEP_FRIED_FISH = new EdiblesManager.FoodInfo("DeepFriedFish", "DLC2_ID", 4200000f, 4, 255.15f, 277.15f, 2400f, true).AddEffects(new List<string>
			{
				"SeafoodRadiationResistance"
			}, DlcManager.AVAILABLE_EXPANSION1_ONLY);

			// Token: 0x040099D2 RID: 39378
			public static readonly EdiblesManager.FoodInfo DEEP_FRIED_SHELLFISH = new EdiblesManager.FoodInfo("DeepFriedShellfish", "DLC2_ID", 4200000f, 4, 255.15f, 277.15f, 2400f, true).AddEffects(new List<string>
			{
				"SeafoodRadiationResistance"
			}, DlcManager.AVAILABLE_EXPANSION1_ONLY);

			// Token: 0x040099D3 RID: 39379
			public static readonly EdiblesManager.FoodInfo PICKLEDMEAL = new EdiblesManager.FoodInfo("PickledMeal", "", 1800000f, -1, 255.15f, 277.15f, 19200f, true);

			// Token: 0x040099D4 RID: 39380
			public static readonly EdiblesManager.FoodInfo BASICPLANTBAR = new EdiblesManager.FoodInfo("BasicPlantBar", "", 1700000f, 0, 255.15f, 277.15f, 4800f, true);

			// Token: 0x040099D5 RID: 39381
			public static readonly EdiblesManager.FoodInfo FRIEDMUSHBAR = new EdiblesManager.FoodInfo("FriedMushBar", "", 1050000f, 0, 255.15f, 277.15f, 4800f, true);

			// Token: 0x040099D6 RID: 39382
			public static readonly EdiblesManager.FoodInfo GAMMAMUSH = new EdiblesManager.FoodInfo("GammaMush", "", 1050000f, 1, 255.15f, 277.15f, 2400f, true);

			// Token: 0x040099D7 RID: 39383
			public static readonly EdiblesManager.FoodInfo GRILLED_PRICKLEFRUIT = new EdiblesManager.FoodInfo("GrilledPrickleFruit", "", 2000000f, 1, 255.15f, 277.15f, 4800f, true);

			// Token: 0x040099D8 RID: 39384
			public static readonly EdiblesManager.FoodInfo SWAMP_DELIGHTS = new EdiblesManager.FoodInfo("SwampDelights", "EXPANSION1_ID", 2240000f, 1, 255.15f, 277.15f, 4800f, true);

			// Token: 0x040099D9 RID: 39385
			public static readonly EdiblesManager.FoodInfo FRIED_MUSHROOM = new EdiblesManager.FoodInfo("FriedMushroom", "", 2800000f, 1, 255.15f, 277.15f, 4800f, true);

			// Token: 0x040099DA RID: 39386
			public static readonly EdiblesManager.FoodInfo COOKED_PIKEAPPLE = new EdiblesManager.FoodInfo("CookedPikeapple", "DLC2_ID", 1200000f, 1, 255.15f, 277.15f, 4800f, true);

			// Token: 0x040099DB RID: 39387
			public static readonly EdiblesManager.FoodInfo COLD_WHEAT_BREAD = new EdiblesManager.FoodInfo("ColdWheatBread", "", 1200000f, 2, 255.15f, 277.15f, 4800f, true);

			// Token: 0x040099DC RID: 39388
			public static readonly EdiblesManager.FoodInfo COOKED_EGG = new EdiblesManager.FoodInfo("CookedEgg", "", 2800000f, 2, 255.15f, 277.15f, 2400f, true);

			// Token: 0x040099DD RID: 39389
			public static readonly EdiblesManager.FoodInfo COOKED_FISH = new EdiblesManager.FoodInfo("CookedFish", "", 1600000f, 3, 255.15f, 277.15f, 2400f, true).AddEffects(new List<string>
			{
				"SeafoodRadiationResistance"
			}, DlcManager.AVAILABLE_EXPANSION1_ONLY);

			// Token: 0x040099DE RID: 39390
			public static readonly EdiblesManager.FoodInfo COOKED_MEAT = new EdiblesManager.FoodInfo("CookedMeat", "", 4000000f, 3, 255.15f, 277.15f, 2400f, true);

			// Token: 0x040099DF RID: 39391
			public static readonly EdiblesManager.FoodInfo PANCAKES = new EdiblesManager.FoodInfo("Pancakes", "", 3600000f, 3, 255.15f, 277.15f, 4800f, true);

			// Token: 0x040099E0 RID: 39392
			public static readonly EdiblesManager.FoodInfo WORMBASICFOOD = new EdiblesManager.FoodInfo("WormBasicFood", "EXPANSION1_ID", 1200000f, 1, 255.15f, 277.15f, 4800f, true);

			// Token: 0x040099E1 RID: 39393
			public static readonly EdiblesManager.FoodInfo WORMSUPERFOOD = new EdiblesManager.FoodInfo("WormSuperFood", "EXPANSION1_ID", 2400000f, 3, 255.15f, 277.15f, 19200f, true);

			// Token: 0x040099E2 RID: 39394
			public static readonly EdiblesManager.FoodInfo FRUITCAKE = new EdiblesManager.FoodInfo("FruitCake", "", 4000000f, 3, 255.15f, 277.15f, 19200f, false);

			// Token: 0x040099E3 RID: 39395
			public static readonly EdiblesManager.FoodInfo SALSA = new EdiblesManager.FoodInfo("Salsa", "", 4400000f, 4, 255.15f, 277.15f, 2400f, true);

			// Token: 0x040099E4 RID: 39396
			public static readonly EdiblesManager.FoodInfo SURF_AND_TURF = new EdiblesManager.FoodInfo("SurfAndTurf", "", 6000000f, 4, 255.15f, 277.15f, 2400f, true).AddEffects(new List<string>
			{
				"SeafoodRadiationResistance"
			}, DlcManager.AVAILABLE_EXPANSION1_ONLY);

			// Token: 0x040099E5 RID: 39397
			public static readonly EdiblesManager.FoodInfo MUSHROOM_WRAP = new EdiblesManager.FoodInfo("MushroomWrap", "", 4800000f, 4, 255.15f, 277.15f, 2400f, true).AddEffects(new List<string>
			{
				"SeafoodRadiationResistance"
			}, DlcManager.AVAILABLE_EXPANSION1_ONLY);

			// Token: 0x040099E6 RID: 39398
			public static readonly EdiblesManager.FoodInfo TOFU = new EdiblesManager.FoodInfo("Tofu", "", 3600000f, 2, 255.15f, 277.15f, 2400f, true);

			// Token: 0x040099E7 RID: 39399
			public static readonly EdiblesManager.FoodInfo CURRY = new EdiblesManager.FoodInfo("Curry", "", 5000000f, 4, 255.15f, 277.15f, 9600f, true).AddEffects(new List<string>
			{
				"HotStuff",
				"WarmTouchFood"
			}, DlcManager.AVAILABLE_ALL_VERSIONS);

			// Token: 0x040099E8 RID: 39400
			public static readonly EdiblesManager.FoodInfo SPICEBREAD = new EdiblesManager.FoodInfo("SpiceBread", "", 4000000f, 5, 255.15f, 277.15f, 4800f, true);

			// Token: 0x040099E9 RID: 39401
			public static readonly EdiblesManager.FoodInfo SPICY_TOFU = new EdiblesManager.FoodInfo("SpicyTofu", "", 4000000f, 5, 255.15f, 277.15f, 2400f, true).AddEffects(new List<string>
			{
				"WarmTouchFood"
			}, DlcManager.AVAILABLE_ALL_VERSIONS);

			// Token: 0x040099EA RID: 39402
			public static readonly EdiblesManager.FoodInfo QUICHE = new EdiblesManager.FoodInfo("Quiche", "", 6400000f, 5, 255.15f, 277.15f, 2400f, true).AddEffects(new List<string>
			{
				"SeafoodRadiationResistance"
			}, DlcManager.AVAILABLE_EXPANSION1_ONLY);

			// Token: 0x040099EB RID: 39403
			public static readonly EdiblesManager.FoodInfo BERRY_PIE = new EdiblesManager.FoodInfo("BerryPie", "EXPANSION1_ID", 4200000f, 5, 255.15f, 277.15f, 2400f, true);

			// Token: 0x040099EC RID: 39404
			public static readonly EdiblesManager.FoodInfo BURGER = new EdiblesManager.FoodInfo("Burger", "", 6000000f, 6, 255.15f, 277.15f, 2400f, true).AddEffects(new List<string>
			{
				"GoodEats"
			}, DlcManager.AVAILABLE_ALL_VERSIONS).AddEffects(new List<string>
			{
				"SeafoodRadiationResistance"
			}, DlcManager.AVAILABLE_EXPANSION1_ONLY);

			// Token: 0x040099ED RID: 39405
			public static readonly EdiblesManager.FoodInfo BEAN = new EdiblesManager.FoodInfo("BeanPlantSeed", "", 0f, 3, 255.15f, 277.15f, 4800f, true);

			// Token: 0x040099EE RID: 39406
			public static readonly EdiblesManager.FoodInfo SPICENUT = new EdiblesManager.FoodInfo(SpiceNutConfig.ID, "", 0f, 0, 255.15f, 277.15f, 2400f, true);

			// Token: 0x040099EF RID: 39407
			public static readonly EdiblesManager.FoodInfo COLD_WHEAT_SEED = new EdiblesManager.FoodInfo("ColdWheatSeed", "", 0f, 0, 283.15f, 308.15f, 9600f, true);
		}

		// Token: 0x02002266 RID: 8806
		public class RECIPES
		{
			// Token: 0x040099F0 RID: 39408
			public static float SMALL_COOK_TIME = 30f;

			// Token: 0x040099F1 RID: 39409
			public static float STANDARD_COOK_TIME = 50f;
		}
	}
}
