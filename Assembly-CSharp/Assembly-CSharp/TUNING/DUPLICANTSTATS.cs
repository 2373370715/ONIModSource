﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace TUNING
{
	public class DUPLICANTSTATS
	{
		public static DUPLICANTSTATS.TraitVal GetTraitVal(string id)
		{
			foreach (DUPLICANTSTATS.TraitVal traitVal in DUPLICANTSTATS.SPECIALTRAITS)
			{
				if (id == traitVal.id)
				{
					return traitVal;
				}
			}
			foreach (DUPLICANTSTATS.TraitVal traitVal2 in DUPLICANTSTATS.GOODTRAITS)
			{
				if (id == traitVal2.id)
				{
					return traitVal2;
				}
			}
			foreach (DUPLICANTSTATS.TraitVal traitVal3 in DUPLICANTSTATS.BADTRAITS)
			{
				if (id == traitVal3.id)
				{
					return traitVal3;
				}
			}
			foreach (DUPLICANTSTATS.TraitVal traitVal4 in DUPLICANTSTATS.CONGENITALTRAITS)
			{
				if (id == traitVal4.id)
				{
					return traitVal4;
				}
			}
			DebugUtil.Assert(true, "Could not find TraitVal with ID: " + id);
			return DUPLICANTSTATS.INVALID_TRAIT_VAL;
		}

		public const float DEFAULT_MASS = 30f;

		public const float PEE_FUSE_TIME = 120f;

		public const float PEE_PER_FLOOR_PEE = 2f;

		public const float PEE_PER_TOILET_PEE = 6.7f;

		public const string PEE_DISEASE = "FoodPoisoning";

		public const int DISEASE_PER_PEE = 100000;

		public const int DISEASE_PER_VOMIT = 100000;

		public const float KCAL2JOULES = 4184f;

		public const float COOLING_EFFICIENCY = 0.08f;

		public const float DUPLICANT_COOLING_KILOWATTS = 0.5578667f;

		public const float WARMING_EFFICIENCY = 0.08f;

		public const float DUPLICANT_WARMING_KILOWATTS = 0.5578667f;

		public const float HEAT_GENERATION_EFFICIENCY = 0.012f;

		public const float DUPLICANT_BASE_GENERATION_KILOWATTS = 0.08368001f;

		public const float STANDARD_STRESS_PENALTY = 0.016666668f;

		public const float STANDARD_STRESS_BONUS = -0.033333335f;

		public const float RANCHING_DURATION_MULTIPLIER_BONUS_PER_POINT = 0.1f;

		public const float FARMING_DURATION_MULTIPLIER_BONUS_PER_POINT = 0.1f;

		public const float POWER_DURATION_MULTIPLIER_BONUS_PER_POINT = 0.025f;

		public const float RANCHING_CAPTURABLE_MULTIPLIER_BONUS_PER_POINT = 0.05f;

		public const float STRESS_BELOW_EXPECTATIONS_FOOD = 0.25f;

		public const float STRESS_ABOVE_EXPECTATIONS_FOOD = -0.5f;

		public const float STANDARD_STRESS_PENALTY_SECOND = 0.25f;

		public const float STANDARD_STRESS_BONUS_SECOND = -0.5f;

		public const float RECOVER_BREATH_DELTA = 3f;

		public const float TRAVEL_TIME_WARNING_THRESHOLD = 0.4f;

		public static readonly string[] ALL_ATTRIBUTES = new string[]
		{
			"Strength",
			"Caring",
			"Construction",
			"Digging",
			"Machinery",
			"Learning",
			"Cooking",
			"Botanist",
			"Art",
			"Ranching",
			"Athletics",
			"SpaceNavigation"
		};

		public static readonly string[] DISTRIBUTED_ATTRIBUTES = new string[]
		{
			"Strength",
			"Caring",
			"Construction",
			"Digging",
			"Machinery",
			"Learning",
			"Cooking",
			"Botanist",
			"Art",
			"Ranching"
		};

		public static readonly string[] ROLLED_ATTRIBUTES = new string[]
		{
			"Athletics"
		};

		public static readonly int[] APTITUDE_ATTRIBUTE_BONUSES = new int[]
		{
			7,
			3,
			1
		};

		public static int ROLLED_ATTRIBUTE_MAX = 5;

		public static float ROLLED_ATTRIBUTE_POWER = 4f;

		public static Dictionary<string, List<string>> ARCHETYPE_TRAIT_EXCLUSIONS = new Dictionary<string, List<string>>
		{
			{
				"Mining",
				new List<string>
				{
					"Anemic",
					"DiggingDown",
					"Narcolepsy"
				}
			},
			{
				"Building",
				new List<string>
				{
					"Anemic",
					"NoodleArms",
					"ConstructionDown",
					"DiggingDown",
					"Narcolepsy"
				}
			},
			{
				"Farming",
				new List<string>
				{
					"Anemic",
					"NoodleArms",
					"BotanistDown",
					"RanchingDown",
					"Narcolepsy"
				}
			},
			{
				"Ranching",
				new List<string>
				{
					"RanchingDown",
					"BotanistDown",
					"Narcolepsy"
				}
			},
			{
				"Cooking",
				new List<string>
				{
					"NoodleArms",
					"CookingDown"
				}
			},
			{
				"Art",
				new List<string>
				{
					"ArtDown",
					"DecorDown"
				}
			},
			{
				"Research",
				new List<string>
				{
					"SlowLearner"
				}
			},
			{
				"Suits",
				new List<string>
				{
					"Anemic",
					"NoodleArms"
				}
			},
			{
				"Hauling",
				new List<string>
				{
					"Anemic",
					"NoodleArms",
					"Narcolepsy"
				}
			},
			{
				"Technicals",
				new List<string>
				{
					"MachineryDown"
				}
			},
			{
				"MedicalAid",
				new List<string>
				{
					"CaringDown",
					"WeakImmuneSystem"
				}
			},
			{
				"Basekeeping",
				new List<string>
				{
					"Anemic",
					"NoodleArms"
				}
			},
			{
				"Rocketry",
				new List<string>()
			}
		};

		public static int RARITY_LEGENDARY = 5;

		public static int RARITY_EPIC = 4;

		public static int RARITY_RARE = 3;

		public static int RARITY_UNCOMMON = 2;

		public static int RARITY_COMMON = 1;

		public static int NO_STATPOINT_BONUS = 0;

		public static int TINY_STATPOINT_BONUS = 1;

		public static int SMALL_STATPOINT_BONUS = 2;

		public static int MEDIUM_STATPOINT_BONUS = 3;

		public static int LARGE_STATPOINT_BONUS = 4;

		public static int HUGE_STATPOINT_BONUS = 5;

		public static int COMMON = 1;

		public static int UNCOMMON = 2;

		public static int RARE = 3;

		public static int EPIC = 4;

		public static int LEGENDARY = 5;

		public static global::Tuple<int, int> TRAITS_ONE_POSITIVE_ONE_NEGATIVE = new global::Tuple<int, int>(1, 1);

		public static global::Tuple<int, int> TRAITS_TWO_POSITIVE_ONE_NEGATIVE = new global::Tuple<int, int>(2, 1);

		public static global::Tuple<int, int> TRAITS_ONE_POSITIVE_TWO_NEGATIVE = new global::Tuple<int, int>(1, 2);

		public static global::Tuple<int, int> TRAITS_TWO_POSITIVE_TWO_NEGATIVE = new global::Tuple<int, int>(2, 2);

		public static global::Tuple<int, int> TRAITS_THREE_POSITIVE_ONE_NEGATIVE = new global::Tuple<int, int>(3, 1);

		public static global::Tuple<int, int> TRAITS_ONE_POSITIVE_THREE_NEGATIVE = new global::Tuple<int, int>(1, 3);

		public static int MIN_STAT_POINTS = 0;

		public static int MAX_STAT_POINTS = 0;

		public static int MAX_TRAITS = 4;

		public static int APTITUDE_BONUS = 1;

		public static List<int> RARITY_DECK = new List<int>
		{
			DUPLICANTSTATS.RARITY_COMMON,
			DUPLICANTSTATS.RARITY_COMMON,
			DUPLICANTSTATS.RARITY_COMMON,
			DUPLICANTSTATS.RARITY_COMMON,
			DUPLICANTSTATS.RARITY_COMMON,
			DUPLICANTSTATS.RARITY_COMMON,
			DUPLICANTSTATS.RARITY_COMMON,
			DUPLICANTSTATS.RARITY_UNCOMMON,
			DUPLICANTSTATS.RARITY_UNCOMMON,
			DUPLICANTSTATS.RARITY_UNCOMMON,
			DUPLICANTSTATS.RARITY_UNCOMMON,
			DUPLICANTSTATS.RARITY_UNCOMMON,
			DUPLICANTSTATS.RARITY_UNCOMMON,
			DUPLICANTSTATS.RARITY_RARE,
			DUPLICANTSTATS.RARITY_RARE,
			DUPLICANTSTATS.RARITY_RARE,
			DUPLICANTSTATS.RARITY_RARE,
			DUPLICANTSTATS.RARITY_EPIC,
			DUPLICANTSTATS.RARITY_EPIC,
			DUPLICANTSTATS.RARITY_LEGENDARY
		};

		public static List<int> rarityDeckActive = new List<int>(DUPLICANTSTATS.RARITY_DECK);

		public static List<global::Tuple<int, int>> POD_TRAIT_CONFIGURATIONS_DECK = new List<global::Tuple<int, int>>
		{
			DUPLICANTSTATS.TRAITS_ONE_POSITIVE_ONE_NEGATIVE,
			DUPLICANTSTATS.TRAITS_ONE_POSITIVE_ONE_NEGATIVE,
			DUPLICANTSTATS.TRAITS_ONE_POSITIVE_ONE_NEGATIVE,
			DUPLICANTSTATS.TRAITS_ONE_POSITIVE_ONE_NEGATIVE,
			DUPLICANTSTATS.TRAITS_ONE_POSITIVE_ONE_NEGATIVE,
			DUPLICANTSTATS.TRAITS_ONE_POSITIVE_ONE_NEGATIVE,
			DUPLICANTSTATS.TRAITS_TWO_POSITIVE_ONE_NEGATIVE,
			DUPLICANTSTATS.TRAITS_TWO_POSITIVE_ONE_NEGATIVE,
			DUPLICANTSTATS.TRAITS_TWO_POSITIVE_ONE_NEGATIVE,
			DUPLICANTSTATS.TRAITS_TWO_POSITIVE_ONE_NEGATIVE,
			DUPLICANTSTATS.TRAITS_TWO_POSITIVE_ONE_NEGATIVE,
			DUPLICANTSTATS.TRAITS_ONE_POSITIVE_TWO_NEGATIVE,
			DUPLICANTSTATS.TRAITS_ONE_POSITIVE_TWO_NEGATIVE,
			DUPLICANTSTATS.TRAITS_ONE_POSITIVE_TWO_NEGATIVE,
			DUPLICANTSTATS.TRAITS_ONE_POSITIVE_TWO_NEGATIVE,
			DUPLICANTSTATS.TRAITS_TWO_POSITIVE_ONE_NEGATIVE,
			DUPLICANTSTATS.TRAITS_TWO_POSITIVE_TWO_NEGATIVE,
			DUPLICANTSTATS.TRAITS_TWO_POSITIVE_TWO_NEGATIVE,
			DUPLICANTSTATS.TRAITS_THREE_POSITIVE_ONE_NEGATIVE,
			DUPLICANTSTATS.TRAITS_ONE_POSITIVE_THREE_NEGATIVE
		};

		public static List<global::Tuple<int, int>> podTraitConfigurationsActive = new List<global::Tuple<int, int>>(DUPLICANTSTATS.POD_TRAIT_CONFIGURATIONS_DECK);

		public static readonly List<string> CONTRACTEDTRAITS_HEALING = new List<string>
		{
			"IrritableBowel",
			"Aggressive",
			"SlowLearner",
			"WeakImmuneSystem",
			"Snorer",
			"CantDig"
		};

		public static readonly List<DUPLICANTSTATS.TraitVal> CONGENITALTRAITS = new List<DUPLICANTSTATS.TraitVal>
		{
			new DUPLICANTSTATS.TraitVal
			{
				id = "None"
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "Joshua",
				mutuallyExclusiveTraits = new List<string>
				{
					"ScaredyCat",
					"Aggressive"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "Ellie",
				statBonus = DUPLICANTSTATS.TINY_STATPOINT_BONUS,
				mutuallyExclusiveTraits = new List<string>
				{
					"InteriorDecorator",
					"MouthBreather",
					"Uncultured"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "Stinky",
				mutuallyExclusiveTraits = new List<string>
				{
					"Flatulence",
					"InteriorDecorator"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "Liam",
				mutuallyExclusiveTraits = new List<string>
				{
					"Flatulence",
					"InteriorDecorator"
				}
			}
		};

		public static readonly DUPLICANTSTATS.TraitVal INVALID_TRAIT_VAL = new DUPLICANTSTATS.TraitVal
		{
			id = "INVALID"
		};

		public static readonly List<DUPLICANTSTATS.TraitVal> BADTRAITS = new List<DUPLICANTSTATS.TraitVal>
		{
			new DUPLICANTSTATS.TraitVal
			{
				id = "CantResearch",
				statBonus = DUPLICANTSTATS.NO_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_COMMON,
				dlcId = "",
				mutuallyExclusiveAptitudes = new List<HashedString>
				{
					"Research"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "CantDig",
				statBonus = DUPLICANTSTATS.LARGE_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_EPIC,
				dlcId = "",
				mutuallyExclusiveAptitudes = new List<HashedString>
				{
					"Mining"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "CantCook",
				statBonus = DUPLICANTSTATS.NO_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_UNCOMMON,
				dlcId = "",
				mutuallyExclusiveAptitudes = new List<HashedString>
				{
					"Cooking"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "CantBuild",
				statBonus = DUPLICANTSTATS.LARGE_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_EPIC,
				dlcId = "",
				mutuallyExclusiveAptitudes = new List<HashedString>
				{
					"Building"
				},
				mutuallyExclusiveTraits = new List<string>
				{
					"GrantSkill_Engineering1"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "Hemophobia",
				statBonus = DUPLICANTSTATS.NO_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_UNCOMMON,
				dlcId = "",
				mutuallyExclusiveAptitudes = new List<HashedString>
				{
					"MedicalAid"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "ScaredyCat",
				statBonus = DUPLICANTSTATS.NO_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_UNCOMMON,
				dlcId = "",
				mutuallyExclusiveAptitudes = new List<HashedString>
				{
					"Mining"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "ConstructionDown",
				statBonus = DUPLICANTSTATS.MEDIUM_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_UNCOMMON,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"ConstructionUp",
					"CantBuild"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "RanchingDown",
				statBonus = DUPLICANTSTATS.SMALL_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_COMMON,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"RanchingUp"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "CaringDown",
				statBonus = DUPLICANTSTATS.SMALL_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_COMMON,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"Hemophobia"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "BotanistDown",
				statBonus = DUPLICANTSTATS.SMALL_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_COMMON,
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "ArtDown",
				statBonus = DUPLICANTSTATS.SMALL_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_COMMON,
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "CookingDown",
				statBonus = DUPLICANTSTATS.SMALL_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_COMMON,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"Foodie",
					"CantCook"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "MachineryDown",
				statBonus = DUPLICANTSTATS.SMALL_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_COMMON,
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "DiggingDown",
				statBonus = DUPLICANTSTATS.MEDIUM_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_RARE,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"MoleHands",
					"CantDig"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "SlowLearner",
				statBonus = DUPLICANTSTATS.MEDIUM_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_RARE,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"FastLearner",
					"CantResearch"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "NoodleArms",
				statBonus = DUPLICANTSTATS.MEDIUM_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_RARE,
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "DecorDown",
				statBonus = DUPLICANTSTATS.TINY_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_COMMON,
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "Anemic",
				statBonus = DUPLICANTSTATS.HUGE_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_LEGENDARY,
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "Flatulence",
				statBonus = DUPLICANTSTATS.MEDIUM_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_RARE,
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "IrritableBowel",
				statBonus = DUPLICANTSTATS.TINY_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_UNCOMMON,
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "Snorer",
				statBonus = DUPLICANTSTATS.TINY_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_RARE,
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "MouthBreather",
				statBonus = DUPLICANTSTATS.HUGE_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_LEGENDARY,
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "SmallBladder",
				statBonus = DUPLICANTSTATS.TINY_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_UNCOMMON,
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "CalorieBurner",
				statBonus = DUPLICANTSTATS.LARGE_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_EPIC,
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "WeakImmuneSystem",
				statBonus = DUPLICANTSTATS.SMALL_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_UNCOMMON,
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "Allergies",
				statBonus = DUPLICANTSTATS.SMALL_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_RARE,
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "NightLight",
				statBonus = DUPLICANTSTATS.SMALL_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_RARE,
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "Narcolepsy",
				statBonus = DUPLICANTSTATS.HUGE_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_RARE,
				dlcId = ""
			}
		};

		public static readonly List<DUPLICANTSTATS.TraitVal> STRESSTRAITS = new List<DUPLICANTSTATS.TraitVal>
		{
			new DUPLICANTSTATS.TraitVal
			{
				id = "Aggressive",
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "StressVomiter",
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "UglyCrier",
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "BingeEater",
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "Banshee",
				dlcId = ""
			}
		};

		public static readonly List<DUPLICANTSTATS.TraitVal> JOYTRAITS = new List<DUPLICANTSTATS.TraitVal>
		{
			new DUPLICANTSTATS.TraitVal
			{
				id = "BalloonArtist",
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "SparkleStreaker",
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "StickerBomber",
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "SuperProductive",
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "HappySinger",
				dlcId = ""
			}
		};

		public static readonly List<DUPLICANTSTATS.TraitVal> GENESHUFFLERTRAITS = new List<DUPLICANTSTATS.TraitVal>
		{
			new DUPLICANTSTATS.TraitVal
			{
				id = "Regeneration",
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "DeeperDiversLungs",
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "SunnyDisposition",
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "RockCrusher",
				dlcId = ""
			}
		};

		public static readonly List<DUPLICANTSTATS.TraitVal> SPECIALTRAITS = new List<DUPLICANTSTATS.TraitVal>
		{
			new DUPLICANTSTATS.TraitVal
			{
				id = "AncientKnowledge",
				rarity = DUPLICANTSTATS.RARITY_LEGENDARY,
				dlcId = "EXPANSION1_ID",
				doNotGenerateTrait = true,
				mutuallyExclusiveTraits = new List<string>
				{
					"CantResearch",
					"CantBuild",
					"CantCook",
					"CantDig",
					"Hemophobia",
					"ScaredyCat",
					"Anemic",
					"SlowLearner",
					"NoodleArms",
					"ConstructionDown",
					"RanchingDown",
					"DiggingDown",
					"MachineryDown",
					"CookingDown",
					"ArtDown",
					"CaringDown",
					"BotanistDown"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "Chatty",
				rarity = DUPLICANTSTATS.RARITY_LEGENDARY,
				dlcId = "",
				doNotGenerateTrait = true
			}
		};

		public static readonly List<DUPLICANTSTATS.TraitVal> GOODTRAITS = new List<DUPLICANTSTATS.TraitVal>
		{
			new DUPLICANTSTATS.TraitVal
			{
				id = "Twinkletoes",
				rarity = DUPLICANTSTATS.RARITY_EPIC,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"Anemic"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "StrongArm",
				rarity = DUPLICANTSTATS.RARITY_RARE,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"NoodleArms"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "Greasemonkey",
				rarity = DUPLICANTSTATS.RARITY_UNCOMMON,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"MachineryDown"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "DiversLung",
				rarity = DUPLICANTSTATS.RARITY_EPIC,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"MouthBreather"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "IronGut",
				rarity = DUPLICANTSTATS.RARITY_COMMON,
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "StrongImmuneSystem",
				rarity = DUPLICANTSTATS.RARITY_COMMON,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"WeakImmuneSystem"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "EarlyBird",
				rarity = DUPLICANTSTATS.RARITY_RARE,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"NightOwl"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "NightOwl",
				rarity = DUPLICANTSTATS.RARITY_RARE,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"EarlyBird"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "Meteorphile",
				rarity = DUPLICANTSTATS.RARITY_RARE,
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "MoleHands",
				rarity = DUPLICANTSTATS.RARITY_RARE,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"CantDig",
					"DiggingDown"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "FastLearner",
				rarity = DUPLICANTSTATS.RARITY_RARE,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"SlowLearner",
					"CantResearch"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "InteriorDecorator",
				rarity = DUPLICANTSTATS.RARITY_COMMON,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"Uncultured",
					"ArtDown"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "Uncultured",
				rarity = DUPLICANTSTATS.RARITY_COMMON,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"InteriorDecorator"
				},
				mutuallyExclusiveAptitudes = new List<HashedString>
				{
					"Art"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "SimpleTastes",
				rarity = DUPLICANTSTATS.RARITY_UNCOMMON,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"Foodie"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "Foodie",
				rarity = DUPLICANTSTATS.RARITY_COMMON,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"SimpleTastes",
					"CantCook",
					"CookingDown"
				},
				mutuallyExclusiveAptitudes = new List<HashedString>
				{
					"Cooking"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "BedsideManner",
				rarity = DUPLICANTSTATS.RARITY_COMMON,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"Hemophobia",
					"CaringDown"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "DecorUp",
				rarity = DUPLICANTSTATS.RARITY_UNCOMMON,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"DecorDown"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "Thriver",
				rarity = DUPLICANTSTATS.RARITY_EPIC,
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "GreenThumb",
				rarity = DUPLICANTSTATS.RARITY_COMMON,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"BotanistDown"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "ConstructionUp",
				rarity = DUPLICANTSTATS.RARITY_UNCOMMON,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"ConstructionDown",
					"CantBuild"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "RanchingUp",
				rarity = DUPLICANTSTATS.RARITY_UNCOMMON,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"RanchingDown"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "Loner",
				rarity = DUPLICANTSTATS.RARITY_EPIC,
				dlcId = "EXPANSION1_ID"
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "StarryEyed",
				rarity = DUPLICANTSTATS.RARITY_RARE,
				dlcId = "EXPANSION1_ID"
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "GlowStick",
				rarity = DUPLICANTSTATS.RARITY_EPIC,
				dlcId = "EXPANSION1_ID"
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "RadiationEater",
				rarity = DUPLICANTSTATS.RARITY_EPIC,
				dlcId = "EXPANSION1_ID"
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "FrostProof",
				rarity = DUPLICANTSTATS.RARITY_COMMON,
				dlcId = "DLC2_ID"
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "GrantSkill_Mining1",
				statBonus = -DUPLICANTSTATS.LARGE_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_LEGENDARY,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"CantDig"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "GrantSkill_Mining2",
				statBonus = -DUPLICANTSTATS.LARGE_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_LEGENDARY,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"CantDig"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "GrantSkill_Mining3",
				statBonus = -DUPLICANTSTATS.LARGE_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_LEGENDARY,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"CantDig"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "GrantSkill_Farming2",
				statBonus = -DUPLICANTSTATS.LARGE_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_EPIC,
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "GrantSkill_Ranching1",
				statBonus = -DUPLICANTSTATS.LARGE_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_EPIC,
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "GrantSkill_Cooking1",
				statBonus = -DUPLICANTSTATS.LARGE_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_EPIC,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"CantCook"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "GrantSkill_Arting1",
				statBonus = -DUPLICANTSTATS.LARGE_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_EPIC,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"Uncultured"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "GrantSkill_Arting2",
				statBonus = -DUPLICANTSTATS.LARGE_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_EPIC,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"Uncultured"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "GrantSkill_Arting3",
				statBonus = -DUPLICANTSTATS.LARGE_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_EPIC,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"Uncultured"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "GrantSkill_Suits1",
				statBonus = -DUPLICANTSTATS.LARGE_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_EPIC,
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "GrantSkill_Technicals2",
				statBonus = -DUPLICANTSTATS.LARGE_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_EPIC,
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "GrantSkill_Engineering1",
				statBonus = -DUPLICANTSTATS.LARGE_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_EPIC,
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "GrantSkill_Basekeeping2",
				statBonus = -DUPLICANTSTATS.LARGE_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_EPIC,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"Anemic"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "GrantSkill_Medicine2",
				statBonus = -DUPLICANTSTATS.LARGE_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_EPIC,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"Hemophobia"
				}
			}
		};

		public static readonly List<DUPLICANTSTATS.TraitVal> NEEDTRAITS = new List<DUPLICANTSTATS.TraitVal>
		{
			new DUPLICANTSTATS.TraitVal
			{
				id = "Claustrophobic",
				rarity = DUPLICANTSTATS.RARITY_COMMON,
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "PrefersWarmer",
				rarity = DUPLICANTSTATS.RARITY_COMMON,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"PrefersColder"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "PrefersColder",
				rarity = DUPLICANTSTATS.RARITY_COMMON,
				dlcId = "",
				mutuallyExclusiveTraits = new List<string>
				{
					"PrefersWarmer"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "SensitiveFeet",
				rarity = DUPLICANTSTATS.RARITY_COMMON,
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "Fashionable",
				rarity = DUPLICANTSTATS.RARITY_COMMON,
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "Climacophobic",
				rarity = DUPLICANTSTATS.RARITY_COMMON,
				dlcId = ""
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "SolitarySleeper",
				rarity = DUPLICANTSTATS.RARITY_COMMON,
				dlcId = ""
			}
		};

		public class BASESTATS
		{
			public const float STAMINA_USED_PER_SECOND = -0.11666667f;

			public const float MAX_CALORIES = 4000000f;

			public const float CALORIES_BURNED_PER_CYCLE = -1000000f;

			public const float CALORIES_BURNED_PER_SECOND = -1666.6666f;

			public const float GUESSTIMATE_CALORIES_PER_CYCLE = -1600000f;

			public const float GUESSTIMATE_CALORIES_BURNED_PER_SECOND = -1666.6666f;

			public const float TRANSIT_TUBE_TRAVEL_SPEED = 18f;

			public const float OXYGEN_USED_PER_SECOND = 0.1f;

			public const float OXYGEN_TO_CO2_CONVERSION = 0.02f;

			public const float LOW_OXYGEN_THRESHOLD = 0.52f;

			public const float NO_OXYGEN_THRESHOLD = 0.05f;

			public const float MIN_CO2_TO_EMIT = 0.02f;

			public const float BLADDER_INCREASE_PER_SECOND = 0.16666667f;

			public const float DECOR_EXPECTATION = 0f;

			public const float FOOD_QUALITY_EXPECTATION = 0f;

			public const float RECREATION_EXPECTATION = 2f;

			public const float MAX_PROFESSION_DECOR_EXPECTATION = 75f;

			public const float MAX_PROFESSION_FOOD_EXPECTATION = 0f;

			public const int MAX_UNDERWATER_TRAVEL_COST = 8;

			public const float TOILET_EFFICIENCY = 1f;

			public const float ROOM_TEMPERATURE_PREFERENCE = 0f;

			public const int BUILDING_DAMAGE_ACTING_OUT = 100;

			public const float IMMUNE_LEVEL_MAX = 100f;

			public const float IMMUNE_LEVEL_RECOVERY = 0.025f;

			public const float CARRY_CAPACITY = 200f;

			public const float HIT_POINTS = 100f;

			public const float RADIATION_RESISTANCE = 0f;
		}

		public class RADIATION_DIFFICULTY_MODIFIERS
		{
			public static float HARDEST = 0.33f;

			public static float HARDER = 0.66f;

			public static float DEFAULT = 1f;

			public static float EASIER = 2f;

			public static float EASIEST = 100f;
		}

		public class RADIATION_EXPOSURE_LEVELS
		{
			public const float LOW = 100f;

			public const float MODERATE = 300f;

			public const float HIGH = 600f;

			public const float DEADLY = 900f;
		}

		public class CALORIES
		{
			public const float SATISFIED_THRESHOLD = 0.95f;

			public const float HUNGRY_THRESHOLD = 0.825f;

			public const float STARVING_THRESHOLD = 0.25f;
		}

		public class TEMPERATURE
		{
			public const float SKIN_THICKNESS = 0.002f;

			public const float SURFACE_AREA = 1f;

			public const float GROUND_TRANSFER_SCALE = 0f;

			public class EXTERNAL
			{
				public const float THRESHOLD_COLD = 283.15f;

				public const float THRESHOLD_HOT = 306.15f;

				public const float THRESHOLD_SCALDING = 345f;
			}

			public class INTERNAL
			{
				public const float IDEAL = 310.15f;

				public const float THRESHOLD_HYPOTHERMIA = 308.15f;

				public const float THRESHOLD_HYPERTHERMIA = 312.15f;

				public const float THRESHOLD_FATAL_HOT = 320.15f;

				public const float THRESHOLD_FATAL_COLD = 300.15f;
			}

			public class CONDUCTIVITY_BARRIER_MODIFICATION
			{
				public const float SKINNY = -0.005f;

				public const float PUDGY = 0.005f;
			}
		}

		public class NOISE
		{
			public const int THRESHOLD_PEACEFUL = 0;

			public const int THRESHOLD_QUIET = 36;

			public const int THRESHOLD_TOSS_AND_TURN = 45;

			public const int THRESHOLD_WAKE_UP = 60;

			public const int THRESHOLD_MINOR_REACTION = 80;

			public const int THRESHOLD_MAJOR_REACTION = 106;

			public const int THRESHOLD_EXTREME_REACTION = 125;
		}

		public class BREATH
		{
			private const float BREATH_BAR_TOTAL_SECONDS = 110f;

			private const float RETREAT_AT_SECONDS = 80f;

			private const float SUFFOCATION_WARN_AT_SECONDS = 50f;

			public const float BREATH_BAR_TOTAL_AMOUNT = 100f;

			public const float RETREAT_AMOUNT = 72.72727f;

			public const float SUFFOCATE_AMOUNT = 45.454548f;

			public const float BREATH_RATE = 0.90909094f;
		}

		public class LIGHT
		{
			public const int LUX_SUNBURN = 72000;

			public const float SUNBURN_DELAY_TIME = 120f;

			public const int LUX_PLEASANT_LIGHT = 40000;

			public const float LIGHT_WORK_EFFICIENCY_BONUS = 0.15f;

			public const int NO_LIGHT = 0;

			public const int VERY_LOW_LIGHT = 1;

			public const int LOW_LIGHT = 500;

			public const int MEDIUM_LIGHT = 1000;

			public const int HIGH_LIGHT = 10000;

			public const int VERY_HIGH_LIGHT = 50000;

			public const int MAX_LIGHT = 100000;
		}

		public class MOVEMENT
		{
			public static float NEUTRAL = 1f;

			public static float BONUS_1 = 1.1f;

			public static float BONUS_2 = 1.25f;

			public static float BONUS_3 = 1.5f;

			public static float BONUS_4 = 1.75f;

			public static float PENALTY_1 = 0.9f;

			public static float PENALTY_2 = 0.75f;

			public static float PENALTY_3 = 0.5f;

			public static float PENALTY_4 = 0.25f;
		}

		public class QOL_STRESS
		{
			public const float ABOVE_EXPECTATIONS = -0.016666668f;

			public const float AT_EXPECTATIONS = -0.008333334f;

			public const float MIN_STRESS = -0.033333335f;

			public class BELOW_EXPECTATIONS
			{
				public const float EASY = 0.0033333334f;

				public const float NEUTRAL = 0.004166667f;

				public const float HARD = 0.008333334f;

				public const float VERYHARD = 0.016666668f;
			}

			public class MAX_STRESS
			{
				public const float EASY = 0.016666668f;

				public const float NEUTRAL = 0.041666668f;

				public const float HARD = 0.05f;

				public const float VERYHARD = 0.083333336f;
			}
		}

		public class COMBAT
		{
			public const Health.HealthState FLEE_THRESHOLD = Health.HealthState.Critical;

			public class BASICWEAPON
			{
				public const float ATTACKS_PER_SECOND = 2f;

				public const float MIN_DAMAGE_PER_HIT = 1f;

				public const float MAX_DAMAGE_PER_HIT = 1f;

				public const AttackProperties.TargetType TARGET_TYPE = AttackProperties.TargetType.Single;

				public const AttackProperties.DamageType DAMAGE_TYPE = AttackProperties.DamageType.Standard;

				public const int MAX_HITS = 1;

				public const float AREA_OF_EFFECT_RADIUS = 0f;
			}
		}

		public class CLOTHING
		{
			public class DECOR_MODIFICATION
			{
				public const int NEGATIVE_SIGNIFICANT = -30;

				public const int NEGATIVE_MILD = -10;

				public const int BASIC = -5;

				public const int POSITIVE_MILD = 10;

				public const int POSITIVE_SIGNIFICANT = 30;

				public const int POSITIVE_MAJOR = 40;
			}

			public class CONDUCTIVITY_BARRIER_MODIFICATION
			{
				public const float THIN = 0.0005f;

				public const float BASIC = 0.0025f;

				public const float THICK = 0.008f;
			}

			public class SWEAT_EFFICIENCY_MULTIPLIER
			{
				public const float DIMINISH_SIGNIFICANT = -2.5f;

				public const float DIMINISH_MILD = -1.25f;

				public const float NEUTRAL = 0f;

				public const float IMPROVE = 2f;
			}
		}

		public class DISTRIBUTIONS
		{
			public static int[] GetRandomDistribution()
			{
				return DUPLICANTSTATS.DISTRIBUTIONS.TYPES[UnityEngine.Random.Range(0, DUPLICANTSTATS.DISTRIBUTIONS.TYPES.Count)];
			}

			public static readonly List<int[]> TYPES = new List<int[]>
			{
				new int[]
				{
					5,
					4,
					4,
					3,
					3,
					2,
					1
				},
				new int[]
				{
					5,
					3,
					2,
					1
				},
				new int[]
				{
					5,
					2,
					2,
					1
				},
				new int[]
				{
					5,
					1
				},
				new int[]
				{
					5,
					3,
					1
				},
				new int[]
				{
					3,
					3,
					3,
					3,
					1
				},
				new int[]
				{
					4
				},
				new int[]
				{
					3
				},
				new int[]
				{
					2
				},
				new int[]
				{
					1
				}
			};
		}

		public struct TraitVal
		{
			public string id;

			public int statBonus;

			public int impact;

			public int rarity;

			public string dlcId;

			public List<string> mutuallyExclusiveTraits;

			public List<HashedString> mutuallyExclusiveAptitudes;

			public bool doNotGenerateTrait;
		}

		public class ATTRIBUTE_LEVELING
		{
			public static int MAX_GAINED_ATTRIBUTE_LEVEL = 20;

			public static int TARGET_MAX_LEVEL_CYCLE = 400;

			public static float EXPERIENCE_LEVEL_POWER = 1.7f;

			public static float FULL_EXPERIENCE = 1f;

			public static float ALL_DAY_EXPERIENCE = DUPLICANTSTATS.ATTRIBUTE_LEVELING.FULL_EXPERIENCE / 0.8f;

			public static float MOST_DAY_EXPERIENCE = DUPLICANTSTATS.ATTRIBUTE_LEVELING.FULL_EXPERIENCE / 0.5f;

			public static float PART_DAY_EXPERIENCE = DUPLICANTSTATS.ATTRIBUTE_LEVELING.FULL_EXPERIENCE / 0.25f;

			public static float BARELY_EVER_EXPERIENCE = DUPLICANTSTATS.ATTRIBUTE_LEVELING.FULL_EXPERIENCE / 0.1f;
		}

		public class ROOM
		{
			public const float LABORATORY_RESEARCH_EFFICIENCY_BONUS = 0.1f;
		}
	}
}