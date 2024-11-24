using System;
using System.Collections.Generic;
using System.Linq;
using Klei.AI;
using STRINGS;

namespace TUNING
{
	// Token: 0x0200226B RID: 8811
	public class CREATURES
	{
		// Token: 0x04009A0C RID: 39436
		public const float WILD_GROWTH_RATE_MODIFIER = 0.25f;

		// Token: 0x04009A0D RID: 39437
		public const int DEFAULT_PROBING_RADIUS = 32;

		// Token: 0x04009A0E RID: 39438
		public const float CREATURES_BASE_GENERATION_KILOWATTS = 10f;

		// Token: 0x04009A0F RID: 39439
		public const float FERTILITY_TIME_BY_LIFESPAN = 0.6f;

		// Token: 0x04009A10 RID: 39440
		public const float INCUBATION_TIME_BY_LIFESPAN = 0.2f;

		// Token: 0x04009A11 RID: 39441
		public const float INCUBATOR_INCUBATION_MULTIPLIER = 4f;

		// Token: 0x04009A12 RID: 39442
		public const float WILD_CALORIE_BURN_RATIO = 0.25f;

		// Token: 0x04009A13 RID: 39443
		public const float HUG_INCUBATION_MULTIPLIER = 1f;

		// Token: 0x04009A14 RID: 39444
		public const float VIABILITY_LOSS_RATE = -0.016666668f;

		// Token: 0x04009A15 RID: 39445
		public const float STATERPILLAR_POWER_CHARGE_LOSS_RATE = -0.055555556f;

		// Token: 0x0200226C RID: 8812
		public class HITPOINTS
		{
			// Token: 0x04009A16 RID: 39446
			public const float TIER0 = 5f;

			// Token: 0x04009A17 RID: 39447
			public const float TIER1 = 25f;

			// Token: 0x04009A18 RID: 39448
			public const float TIER2 = 50f;

			// Token: 0x04009A19 RID: 39449
			public const float TIER3 = 100f;

			// Token: 0x04009A1A RID: 39450
			public const float TIER4 = 150f;

			// Token: 0x04009A1B RID: 39451
			public const float TIER5 = 200f;

			// Token: 0x04009A1C RID: 39452
			public const float TIER6 = 400f;
		}

		// Token: 0x0200226D RID: 8813
		public class MASS_KG
		{
			// Token: 0x04009A1D RID: 39453
			public const float TIER0 = 5f;

			// Token: 0x04009A1E RID: 39454
			public const float TIER1 = 25f;

			// Token: 0x04009A1F RID: 39455
			public const float TIER2 = 50f;

			// Token: 0x04009A20 RID: 39456
			public const float TIER3 = 100f;

			// Token: 0x04009A21 RID: 39457
			public const float TIER4 = 200f;

			// Token: 0x04009A22 RID: 39458
			public const float TIER5 = 400f;
		}

		// Token: 0x0200226E RID: 8814
		public class TEMPERATURE
		{
			// Token: 0x04009A23 RID: 39459
			public const float SKIN_THICKNESS = 0.025f;

			// Token: 0x04009A24 RID: 39460
			public const float SURFACE_AREA = 17.5f;

			// Token: 0x04009A25 RID: 39461
			public const float GROUND_TRANSFER_SCALE = 0f;

			// Token: 0x04009A26 RID: 39462
			public static float FREEZING_10 = 173f;

			// Token: 0x04009A27 RID: 39463
			public static float FREEZING_9 = 183f;

			// Token: 0x04009A28 RID: 39464
			public static float FREEZING_3 = 243f;

			// Token: 0x04009A29 RID: 39465
			public static float FREEZING_2 = 253f;

			// Token: 0x04009A2A RID: 39466
			public static float FREEZING_1 = 263f;

			// Token: 0x04009A2B RID: 39467
			public static float FREEZING = 273f;

			// Token: 0x04009A2C RID: 39468
			public static float COOL = 283f;

			// Token: 0x04009A2D RID: 39469
			public static float MODERATE = 293f;

			// Token: 0x04009A2E RID: 39470
			public static float HOT = 303f;

			// Token: 0x04009A2F RID: 39471
			public static float HOT_1 = 313f;

			// Token: 0x04009A30 RID: 39472
			public static float HOT_2 = 323f;

			// Token: 0x04009A31 RID: 39473
			public static float HOT_3 = 333f;

			// Token: 0x04009A32 RID: 39474
			public static float HOT_7 = 373f;
		}

		// Token: 0x0200226F RID: 8815
		public class LIFESPAN
		{
			// Token: 0x04009A33 RID: 39475
			public const float TIER0 = 5f;

			// Token: 0x04009A34 RID: 39476
			public const float TIER1 = 25f;

			// Token: 0x04009A35 RID: 39477
			public const float TIER2 = 75f;

			// Token: 0x04009A36 RID: 39478
			public const float TIER3 = 100f;

			// Token: 0x04009A37 RID: 39479
			public const float TIER4 = 150f;

			// Token: 0x04009A38 RID: 39480
			public const float TIER5 = 200f;

			// Token: 0x04009A39 RID: 39481
			public const float TIER6 = 400f;
		}

		// Token: 0x02002270 RID: 8816
		public class CONVERSION_EFFICIENCY
		{
			// Token: 0x04009A3A RID: 39482
			public static float BAD_2 = 0.1f;

			// Token: 0x04009A3B RID: 39483
			public static float BAD_1 = 0.25f;

			// Token: 0x04009A3C RID: 39484
			public static float NORMAL = 0.5f;

			// Token: 0x04009A3D RID: 39485
			public static float GOOD_1 = 0.75f;

			// Token: 0x04009A3E RID: 39486
			public static float GOOD_2 = 0.95f;

			// Token: 0x04009A3F RID: 39487
			public static float GOOD_3 = 1f;
		}

		// Token: 0x02002271 RID: 8817
		public class SPACE_REQUIREMENTS
		{
			// Token: 0x04009A40 RID: 39488
			public static int TIER1 = 4;

			// Token: 0x04009A41 RID: 39489
			public static int TIER2 = 8;

			// Token: 0x04009A42 RID: 39490
			public static int TIER3 = 12;

			// Token: 0x04009A43 RID: 39491
			public static int TIER4 = 16;
		}

		// Token: 0x02002272 RID: 8818
		public class EGG_CHANCE_MODIFIERS
		{
			// Token: 0x0600B92B RID: 47403 RVA: 0x00117211 File Offset: 0x00115411
			private static System.Action CreateDietaryModifier(string id, Tag eggTag, HashSet<Tag> foodTags, float modifierPerCal)
			{
				Func<string, string> <>9__1;
				FertilityModifier.FertilityModFn <>9__2;
				return delegate()
				{
					string text = CREATURES.FERTILITY_MODIFIERS.DIET.NAME;
					string text2 = CREATURES.FERTILITY_MODIFIERS.DIET.DESC;
					ModifierSet modifierSet = Db.Get();
					string id2 = id;
					Tag eggTag2 = eggTag;
					string name = text;
					string description = text2;
					Func<string, string> tooltipCB;
					if ((tooltipCB = <>9__1) == null)
					{
						tooltipCB = (<>9__1 = delegate(string descStr)
						{
							string arg = string.Join(", ", (from t in foodTags
							select t.ProperName()).ToArray<string>());
							descStr = string.Format(descStr, arg);
							return descStr;
						});
					}
					FertilityModifier.FertilityModFn applyFunction;
					if ((applyFunction = <>9__2) == null)
					{
						applyFunction = (<>9__2 = delegate(FertilityMonitor.Instance inst, Tag eggType)
						{
							inst.gameObject.Subscribe(-2038961714, delegate(object data)
							{
								CreatureCalorieMonitor.CaloriesConsumedEvent caloriesConsumedEvent = (CreatureCalorieMonitor.CaloriesConsumedEvent)data;
								if (foodTags.Contains(caloriesConsumedEvent.tag))
								{
									inst.AddBreedingChance(eggType, caloriesConsumedEvent.calories * modifierPerCal);
								}
							});
						});
					}
					modifierSet.CreateFertilityModifier(id2, eggTag2, name, description, tooltipCB, applyFunction);
				};
			}

			// Token: 0x0600B92C RID: 47404 RVA: 0x0011723F File Offset: 0x0011543F
			private static System.Action CreateDietaryModifier(string id, Tag eggTag, Tag foodTag, float modifierPerCal)
			{
				return CREATURES.EGG_CHANCE_MODIFIERS.CreateDietaryModifier(id, eggTag, new HashSet<Tag>
				{
					foodTag
				}, modifierPerCal);
			}

			// Token: 0x0600B92D RID: 47405 RVA: 0x00117256 File Offset: 0x00115456
			private static System.Action CreateNearbyCreatureModifier(string id, Tag eggTag, Tag nearbyCreatureBaby, Tag nearbyCreatureAdult, float modifierPerSecond, bool alsoInvert)
			{
				Func<string, string> <>9__1;
				FertilityModifier.FertilityModFn <>9__2;
				return delegate()
				{
					string text = (modifierPerSecond < 0f) ? CREATURES.FERTILITY_MODIFIERS.NEARBY_CREATURE_NEG.NAME : CREATURES.FERTILITY_MODIFIERS.NEARBY_CREATURE.NAME;
					string text2 = (modifierPerSecond < 0f) ? CREATURES.FERTILITY_MODIFIERS.NEARBY_CREATURE_NEG.DESC : CREATURES.FERTILITY_MODIFIERS.NEARBY_CREATURE.DESC;
					ModifierSet modifierSet = Db.Get();
					string id2 = id;
					Tag eggTag2 = eggTag;
					string name = text;
					string description = text2;
					Func<string, string> tooltipCB;
					if ((tooltipCB = <>9__1) == null)
					{
						tooltipCB = (<>9__1 = ((string descStr) => string.Format(descStr, nearbyCreatureAdult.ProperName())));
					}
					FertilityModifier.FertilityModFn applyFunction;
					if ((applyFunction = <>9__2) == null)
					{
						applyFunction = (<>9__2 = delegate(FertilityMonitor.Instance inst, Tag eggType)
						{
							NearbyCreatureMonitor.Instance instance = inst.gameObject.GetSMI<NearbyCreatureMonitor.Instance>();
							if (instance == null)
							{
								instance = new NearbyCreatureMonitor.Instance(inst.master);
								instance.StartSM();
							}
							instance.OnUpdateNearbyCreatures += delegate(float dt, List<KPrefabID> creatures, List<KPrefabID> eggs)
							{
								bool flag = false;
								foreach (KPrefabID kprefabID in creatures)
								{
									if (kprefabID.PrefabTag == nearbyCreatureBaby || kprefabID.PrefabTag == nearbyCreatureAdult)
									{
										flag = true;
										break;
									}
								}
								if (flag)
								{
									inst.AddBreedingChance(eggType, dt * modifierPerSecond);
									return;
								}
								if (alsoInvert)
								{
									inst.AddBreedingChance(eggType, dt * -modifierPerSecond);
								}
							};
						});
					}
					modifierSet.CreateFertilityModifier(id2, eggTag2, name, description, tooltipCB, applyFunction);
				};
			}

			// Token: 0x0600B92E RID: 47406 RVA: 0x0046F13C File Offset: 0x0046D33C
			private static System.Action CreateElementCreatureModifier(string id, Tag eggTag, Tag element, float modifierPerSecond, bool alsoInvert, bool checkSubstantialLiquid, string tooltipOverride = null)
			{
				Func<string, string> <>9__1;
				FertilityModifier.FertilityModFn <>9__2;
				return delegate()
				{
					string text = CREATURES.FERTILITY_MODIFIERS.LIVING_IN_ELEMENT.NAME;
					string text2 = CREATURES.FERTILITY_MODIFIERS.LIVING_IN_ELEMENT.DESC;
					ModifierSet modifierSet = Db.Get();
					string id2 = id;
					Tag eggTag2 = eggTag;
					string name = text;
					string description = text2;
					Func<string, string> tooltipCB;
					if ((tooltipCB = <>9__1) == null)
					{
						tooltipCB = (<>9__1 = delegate(string descStr)
						{
							if (tooltipOverride == null)
							{
								return string.Format(descStr, ElementLoader.GetElement(element).name);
							}
							return tooltipOverride;
						});
					}
					FertilityModifier.FertilityModFn applyFunction;
					if ((applyFunction = <>9__2) == null)
					{
						applyFunction = (<>9__2 = delegate(FertilityMonitor.Instance inst, Tag eggType)
						{
							CritterElementMonitor.Instance instance = inst.gameObject.GetSMI<CritterElementMonitor.Instance>();
							if (instance == null)
							{
								instance = new CritterElementMonitor.Instance(inst.master);
								instance.StartSM();
							}
							instance.OnUpdateEggChances += delegate(float dt)
							{
								int num = Grid.PosToCell(inst);
								if (!Grid.IsValidCell(num))
								{
									return;
								}
								if (Grid.Element[num].HasTag(element) && (!checkSubstantialLiquid || Grid.IsSubstantialLiquid(num, 0.35f)))
								{
									inst.AddBreedingChance(eggType, dt * modifierPerSecond);
									return;
								}
								if (alsoInvert)
								{
									inst.AddBreedingChance(eggType, dt * -modifierPerSecond);
								}
							};
						});
					}
					modifierSet.CreateFertilityModifier(id2, eggTag2, name, description, tooltipCB, applyFunction);
				};
			}

			// Token: 0x0600B92F RID: 47407 RVA: 0x00117294 File Offset: 0x00115494
			private static System.Action CreateCropTendedModifier(string id, Tag eggTag, HashSet<Tag> cropTags, float modifierPerEvent)
			{
				Func<string, string> <>9__1;
				FertilityModifier.FertilityModFn <>9__2;
				return delegate()
				{
					string text = CREATURES.FERTILITY_MODIFIERS.CROPTENDING.NAME;
					string text2 = CREATURES.FERTILITY_MODIFIERS.CROPTENDING.DESC;
					ModifierSet modifierSet = Db.Get();
					string id2 = id;
					Tag eggTag2 = eggTag;
					string name = text;
					string description = text2;
					Func<string, string> tooltipCB;
					if ((tooltipCB = <>9__1) == null)
					{
						tooltipCB = (<>9__1 = delegate(string descStr)
						{
							string arg = string.Join(", ", (from t in cropTags
							select t.ProperName()).ToArray<string>());
							descStr = string.Format(descStr, arg);
							return descStr;
						});
					}
					FertilityModifier.FertilityModFn applyFunction;
					if ((applyFunction = <>9__2) == null)
					{
						applyFunction = (<>9__2 = delegate(FertilityMonitor.Instance inst, Tag eggType)
						{
							inst.gameObject.Subscribe(90606262, delegate(object data)
							{
								CropTendingStates.CropTendingEventData cropTendingEventData = (CropTendingStates.CropTendingEventData)data;
								if (cropTags.Contains(cropTendingEventData.cropId))
								{
									inst.AddBreedingChance(eggType, modifierPerEvent);
								}
							});
						});
					}
					modifierSet.CreateFertilityModifier(id2, eggTag2, name, description, tooltipCB, applyFunction);
				};
			}

			// Token: 0x0600B930 RID: 47408 RVA: 0x001172C2 File Offset: 0x001154C2
			private static System.Action CreateTemperatureModifier(string id, Tag eggTag, float minTemp, float maxTemp, float modifierPerSecond, bool alsoInvert)
			{
				Func<string, string> <>9__1;
				FertilityModifier.FertilityModFn <>9__2;
				return delegate()
				{
					string text = CREATURES.FERTILITY_MODIFIERS.TEMPERATURE.NAME;
					ModifierSet modifierSet = Db.Get();
					string id2 = id;
					Tag eggTag2 = eggTag;
					string name = text;
					string description = null;
					Func<string, string> tooltipCB;
					if ((tooltipCB = <>9__1) == null)
					{
						tooltipCB = (<>9__1 = ((string src) => string.Format(CREATURES.FERTILITY_MODIFIERS.TEMPERATURE.DESC, GameUtil.GetFormattedTemperature(minTemp, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false), GameUtil.GetFormattedTemperature(maxTemp, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false))));
					}
					FertilityModifier.FertilityModFn applyFunction;
					if ((applyFunction = <>9__2) == null)
					{
						applyFunction = (<>9__2 = delegate(FertilityMonitor.Instance inst, Tag eggType)
						{
							CritterTemperatureMonitor.Instance smi = inst.gameObject.GetSMI<CritterTemperatureMonitor.Instance>();
							if (smi != null)
							{
								CritterTemperatureMonitor.Instance instance = smi;
								instance.OnUpdate_GetTemperatureInternal = (Action<float, float>)Delegate.Combine(instance.OnUpdate_GetTemperatureInternal, new Action<float, float>(delegate(float dt, float newTemp)
								{
									if (newTemp > minTemp && newTemp < maxTemp)
									{
										inst.AddBreedingChance(eggType, dt * modifierPerSecond);
										return;
									}
									if (alsoInvert)
									{
										inst.AddBreedingChance(eggType, dt * -modifierPerSecond);
									}
								}));
								return;
							}
							DebugUtil.LogErrorArgs(new object[]
							{
								"Ack! Trying to add temperature modifier",
								id,
								"to",
								inst.master.name,
								"but it doesn't have a CritterTemperatureMonitor.Instance"
							});
						});
					}
					modifierSet.CreateFertilityModifier(id2, eggTag2, name, description, tooltipCB, applyFunction);
				};
			}

			// Token: 0x04009A44 RID: 39492
			public static List<System.Action> MODIFIER_CREATORS = new List<System.Action>
			{
				CREATURES.EGG_CHANCE_MODIFIERS.CreateDietaryModifier("HatchHard", "HatchHardEgg".ToTag(), SimHashes.SedimentaryRock.CreateTag(), 0.05f / HatchTuning.STANDARD_CALORIES_PER_CYCLE),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateDietaryModifier("HatchVeggie", "HatchVeggieEgg".ToTag(), SimHashes.Dirt.CreateTag(), 0.05f / HatchTuning.STANDARD_CALORIES_PER_CYCLE),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateDietaryModifier("HatchMetal", "HatchMetalEgg".ToTag(), HatchMetalConfig.METAL_ORE_TAGS, 0.05f / HatchTuning.STANDARD_CALORIES_PER_CYCLE),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateNearbyCreatureModifier("PuftAlphaBalance", "PuftAlphaEgg".ToTag(), "PuftAlphaBaby".ToTag(), "PuftAlpha".ToTag(), -0.00025f, true),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateNearbyCreatureModifier("PuftAlphaNearbyOxylite", "PuftOxyliteEgg".ToTag(), "PuftAlphaBaby".ToTag(), "PuftAlpha".ToTag(), 8.333333E-05f, false),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateNearbyCreatureModifier("PuftAlphaNearbyBleachstone", "PuftBleachstoneEgg".ToTag(), "PuftAlphaBaby".ToTag(), "PuftAlpha".ToTag(), 8.333333E-05f, false),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateTemperatureModifier("OilFloaterHighTemp", "OilfloaterHighTempEgg".ToTag(), 373.15f, 523.15f, 8.333333E-05f, false),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateTemperatureModifier("OilFloaterDecor", "OilfloaterDecorEgg".ToTag(), 293.15f, 333.15f, 8.333333E-05f, false),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateDietaryModifier("LightBugOrange", "LightBugOrangeEgg".ToTag(), "GrilledPrickleFruit".ToTag(), 0.00125f),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateDietaryModifier("LightBugPurple", "LightBugPurpleEgg".ToTag(), "FriedMushroom".ToTag(), 0.00125f),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateDietaryModifier("LightBugPink", "LightBugPinkEgg".ToTag(), "SpiceBread".ToTag(), 0.00125f),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateDietaryModifier("LightBugBlue", "LightBugBlueEgg".ToTag(), "Salsa".ToTag(), 0.00125f),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateDietaryModifier("LightBugBlack", "LightBugBlackEgg".ToTag(), SimHashes.Phosphorus.CreateTag(), 0.00125f),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateDietaryModifier("LightBugCrystal", "LightBugCrystalEgg".ToTag(), "CookedMeat".ToTag(), 0.00125f),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateTemperatureModifier("PacuTropical", "PacuTropicalEgg".ToTag(), 308.15f, 353.15f, 8.333333E-05f, false),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateTemperatureModifier("PacuCleaner", "PacuCleanerEgg".ToTag(), 243.15f, 278.15f, 8.333333E-05f, false),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateDietaryModifier("DreckoPlastic", "DreckoPlasticEgg".ToTag(), "BasicSingleHarvestPlant".ToTag(), 0.025f / DreckoTuning.STANDARD_CALORIES_PER_CYCLE),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateDietaryModifier("SquirrelHug", "SquirrelHugEgg".ToTag(), BasicFabricMaterialPlantConfig.ID.ToTag(), 0.025f / SquirrelTuning.STANDARD_CALORIES_PER_CYCLE),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateCropTendedModifier("DivergentWorm", "DivergentWormEgg".ToTag(), new HashSet<Tag>
				{
					"WormPlant".ToTag(),
					"SuperWormPlant".ToTag()
				}, 0.05f / (float)DivergentTuning.TIMES_TENDED_PER_CYCLE_FOR_EVOLUTION),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateElementCreatureModifier("PokeLumber", "CrabWoodEgg".ToTag(), SimHashes.Ethanol.CreateTag(), 0.00025f, true, true, null),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateElementCreatureModifier("PokeFreshWater", "CrabFreshWaterEgg".ToTag(), SimHashes.Water.CreateTag(), 0.00025f, true, true, null),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateTemperatureModifier("MoleDelicacy", "MoleDelicacyEgg".ToTag(), MoleDelicacyConfig.EGG_CHANCES_TEMPERATURE_MIN, MoleDelicacyConfig.EGG_CHANCES_TEMPERATURE_MAX, 8.333333E-05f, false),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateElementCreatureModifier("StaterpillarGas", "StaterpillarGasEgg".ToTag(), GameTags.Unbreathable, 0.00025f, true, false, CREATURES.FERTILITY_MODIFIERS.LIVING_IN_ELEMENT.UNBREATHABLE),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateElementCreatureModifier("StaterpillarLiquid", "StaterpillarLiquidEgg".ToTag(), GameTags.Liquid, 0.00025f, true, false, CREATURES.FERTILITY_MODIFIERS.LIVING_IN_ELEMENT.LIQUID),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateDietaryModifier("BellyGold", "GoldBellyEgg".ToTag(), "FriesCarrot".ToTag(), 0.05f / BellyTuning.STANDARD_CALORIES_PER_CYCLE)
			};
		}

		// Token: 0x0200227E RID: 8830
		public class SORTING
		{
			// Token: 0x04009A7C RID: 39548
			public static Dictionary<string, int> CRITTER_ORDER = new Dictionary<string, int>
			{
				{
					"Hatch",
					10
				},
				{
					"Puft",
					20
				},
				{
					"Drecko",
					30
				},
				{
					"Squirrel",
					40
				},
				{
					"Pacu",
					50
				},
				{
					"Oilfloater",
					60
				},
				{
					"LightBug",
					70
				},
				{
					"Crab",
					80
				},
				{
					"DivergentBeetle",
					90
				},
				{
					"Staterpillar",
					100
				},
				{
					"Mole",
					110
				},
				{
					"Bee",
					120
				},
				{
					"Moo",
					130
				},
				{
					"Glom",
					140
				},
				{
					"WoodDeer",
					140
				},
				{
					"Seal",
					150
				},
				{
					"IceBelly",
					160
				}
			};
		}
	}
}
