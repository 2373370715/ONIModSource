using System;
using System.Collections.Generic;
using UnityEngine;

namespace TUNING
{
	// Token: 0x0200223F RID: 8767
	public class DUPLICANTSTATS
	{
		// Token: 0x0600B8E8 RID: 47336 RVA: 0x0046B7A8 File Offset: 0x004699A8
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

		// Token: 0x0600B8E9 RID: 47337 RVA: 0x0046B910 File Offset: 0x00469B10
		public static DUPLICANTSTATS GetStatsFor(GameObject gameObject)
		{
			KPrefabID component = gameObject.GetComponent<KPrefabID>();
			if (component != null)
			{
				return DUPLICANTSTATS.GetStatsFor(component);
			}
			return null;
		}

		// Token: 0x0600B8EA RID: 47338 RVA: 0x0046B938 File Offset: 0x00469B38
		public static DUPLICANTSTATS GetStatsFor(KPrefabID prefabID)
		{
			if (!prefabID.HasTag(GameTags.BaseMinion))
			{
				return null;
			}
			foreach (Tag tag in GameTags.Minions.Models.AllModels)
			{
				if (prefabID.HasTag(tag))
				{
					return DUPLICANTSTATS.GetStatsFor(tag);
				}
			}
			return null;
		}

		// Token: 0x0600B8EB RID: 47339 RVA: 0x00116EFC File Offset: 0x001150FC
		public static DUPLICANTSTATS GetStatsFor(Tag type)
		{
			if (DUPLICANTSTATS.DUPLICANT_TYPES.ContainsKey(type))
			{
				return DUPLICANTSTATS.DUPLICANT_TYPES[type];
			}
			return null;
		}

		// Token: 0x0400987D RID: 39037
		public const float RANCHING_DURATION_MULTIPLIER_BONUS_PER_POINT = 0.1f;

		// Token: 0x0400987E RID: 39038
		public const float FARMING_DURATION_MULTIPLIER_BONUS_PER_POINT = 0.1f;

		// Token: 0x0400987F RID: 39039
		public const float POWER_DURATION_MULTIPLIER_BONUS_PER_POINT = 0.025f;

		// Token: 0x04009880 RID: 39040
		public const float RANCHING_CAPTURABLE_MULTIPLIER_BONUS_PER_POINT = 0.05f;

		// Token: 0x04009881 RID: 39041
		public const float STANDARD_STRESS_PENALTY = 0.016666668f;

		// Token: 0x04009882 RID: 39042
		public const float STANDARD_STRESS_BONUS = -0.033333335f;

		// Token: 0x04009883 RID: 39043
		public const float STRESS_BELOW_EXPECTATIONS_FOOD = 0.25f;

		// Token: 0x04009884 RID: 39044
		public const float STRESS_ABOVE_EXPECTATIONS_FOOD = -0.5f;

		// Token: 0x04009885 RID: 39045
		public const float STANDARD_STRESS_PENALTY_SECOND = 0.25f;

		// Token: 0x04009886 RID: 39046
		public const float STANDARD_STRESS_BONUS_SECOND = -0.5f;

		// Token: 0x04009887 RID: 39047
		public const float TRAVEL_TIME_WARNING_THRESHOLD = 0.4f;

		// Token: 0x04009888 RID: 39048
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

		// Token: 0x04009889 RID: 39049
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

		// Token: 0x0400988A RID: 39050
		public static readonly string[] ROLLED_ATTRIBUTES = new string[]
		{
			"Athletics"
		};

		// Token: 0x0400988B RID: 39051
		public static readonly int[] APTITUDE_ATTRIBUTE_BONUSES = new int[]
		{
			7,
			3,
			1
		};

		// Token: 0x0400988C RID: 39052
		public static int ROLLED_ATTRIBUTE_MAX = 5;

		// Token: 0x0400988D RID: 39053
		public static float ROLLED_ATTRIBUTE_POWER = 4f;

		// Token: 0x0400988E RID: 39054
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

		// Token: 0x0400988F RID: 39055
		public static int RARITY_LEGENDARY = 5;

		// Token: 0x04009890 RID: 39056
		public static int RARITY_EPIC = 4;

		// Token: 0x04009891 RID: 39057
		public static int RARITY_RARE = 3;

		// Token: 0x04009892 RID: 39058
		public static int RARITY_UNCOMMON = 2;

		// Token: 0x04009893 RID: 39059
		public static int RARITY_COMMON = 1;

		// Token: 0x04009894 RID: 39060
		public static int NO_STATPOINT_BONUS = 0;

		// Token: 0x04009895 RID: 39061
		public static int TINY_STATPOINT_BONUS = 1;

		// Token: 0x04009896 RID: 39062
		public static int SMALL_STATPOINT_BONUS = 2;

		// Token: 0x04009897 RID: 39063
		public static int MEDIUM_STATPOINT_BONUS = 3;

		// Token: 0x04009898 RID: 39064
		public static int LARGE_STATPOINT_BONUS = 4;

		// Token: 0x04009899 RID: 39065
		public static int HUGE_STATPOINT_BONUS = 5;

		// Token: 0x0400989A RID: 39066
		public static int COMMON = 1;

		// Token: 0x0400989B RID: 39067
		public static int UNCOMMON = 2;

		// Token: 0x0400989C RID: 39068
		public static int RARE = 3;

		// Token: 0x0400989D RID: 39069
		public static int EPIC = 4;

		// Token: 0x0400989E RID: 39070
		public static int LEGENDARY = 5;

		// Token: 0x0400989F RID: 39071
		public static global::Tuple<int, int> TRAITS_ONE_POSITIVE_ONE_NEGATIVE = new global::Tuple<int, int>(1, 1);

		// Token: 0x040098A0 RID: 39072
		public static global::Tuple<int, int> TRAITS_TWO_POSITIVE_ONE_NEGATIVE = new global::Tuple<int, int>(2, 1);

		// Token: 0x040098A1 RID: 39073
		public static global::Tuple<int, int> TRAITS_ONE_POSITIVE_TWO_NEGATIVE = new global::Tuple<int, int>(1, 2);

		// Token: 0x040098A2 RID: 39074
		public static global::Tuple<int, int> TRAITS_TWO_POSITIVE_TWO_NEGATIVE = new global::Tuple<int, int>(2, 2);

		// Token: 0x040098A3 RID: 39075
		public static global::Tuple<int, int> TRAITS_THREE_POSITIVE_ONE_NEGATIVE = new global::Tuple<int, int>(3, 1);

		// Token: 0x040098A4 RID: 39076
		public static global::Tuple<int, int> TRAITS_ONE_POSITIVE_THREE_NEGATIVE = new global::Tuple<int, int>(1, 3);

		// Token: 0x040098A5 RID: 39077
		public static int MIN_STAT_POINTS = 0;

		// Token: 0x040098A6 RID: 39078
		public static int MAX_STAT_POINTS = 0;

		// Token: 0x040098A7 RID: 39079
		public static int MAX_TRAITS = 4;

		// Token: 0x040098A8 RID: 39080
		public static int APTITUDE_BONUS = 1;

		// Token: 0x040098A9 RID: 39081
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

		// Token: 0x040098AA RID: 39082
		public static List<int> rarityDeckActive = new List<int>(DUPLICANTSTATS.RARITY_DECK);

		// Token: 0x040098AB RID: 39083
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

		// Token: 0x040098AC RID: 39084
		public static List<global::Tuple<int, int>> podTraitConfigurationsActive = new List<global::Tuple<int, int>>(DUPLICANTSTATS.POD_TRAIT_CONFIGURATIONS_DECK);

		// Token: 0x040098AD RID: 39085
		public static readonly List<string> CONTRACTEDTRAITS_HEALING = new List<string>
		{
			"IrritableBowel",
			"Aggressive",
			"SlowLearner",
			"WeakImmuneSystem",
			"Snorer",
			"CantDig"
		};

		// Token: 0x040098AE RID: 39086
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

		// Token: 0x040098AF RID: 39087
		public static readonly DUPLICANTSTATS.TraitVal INVALID_TRAIT_VAL = new DUPLICANTSTATS.TraitVal
		{
			id = "INVALID"
		};

		// Token: 0x040098B0 RID: 39088
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

		// Token: 0x040098B1 RID: 39089
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

		// Token: 0x040098B2 RID: 39090
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
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "DataRainer",
				dlcId = "DLC3_ID"
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "RoboDancer",
				dlcId = "DLC3_ID"
			}
		};

		// Token: 0x040098B3 RID: 39091
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

		// Token: 0x040098B4 RID: 39092
		public static readonly List<DUPLICANTSTATS.TraitVal> BIONICTRAITS = new List<DUPLICANTSTATS.TraitVal>
		{
			new DUPLICANTSTATS.TraitVal
			{
				id = "BionicBaseline",
				dlcId = "DLC3_ID"
			}
		};

		// Token: 0x040098B5 RID: 39093
		public static readonly List<DUPLICANTSTATS.TraitVal> BIONICUPGRADETRAITS = new List<DUPLICANTSTATS.TraitVal>
		{
			new DUPLICANTSTATS.TraitVal
			{
				id = "DefaultBionicBoostDigging",
				dlcId = "DLC3_ID"
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "DefaultBionicBoostBuilding",
				dlcId = "DLC3_ID"
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "DefaultBionicBoostCooking",
				dlcId = "DLC3_ID"
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "DefaultBionicBoostArt",
				dlcId = "DLC3_ID"
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "DefaultBionicBoostFarming",
				dlcId = "DLC3_ID"
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "DefaultBionicBoostRanching",
				dlcId = "DLC3_ID"
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "DefaultBionicBoostMedicine",
				dlcId = "DLC3_ID"
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "DefaultBionicBoostExplorer",
				dlcId = "DLC3_ID"
			}
		};

		// Token: 0x040098B6 RID: 39094
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

		// Token: 0x040098B7 RID: 39095
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

		// Token: 0x040098B8 RID: 39096
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

		// Token: 0x040098B9 RID: 39097
		public static DUPLICANTSTATS STANDARD = new DUPLICANTSTATS();

		// Token: 0x040098BA RID: 39098
		public static DUPLICANTSTATS BIONICS = new DUPLICANTSTATS
		{
			BaseStats = new DUPLICANTSTATS.BASESTATS
			{
				NO_OXYGEN_THRESHOLD = 0.5f,
				MAX_CALORIES = 0f
			}
		};

		// Token: 0x040098BB RID: 39099
		private static readonly Dictionary<Tag, DUPLICANTSTATS> DUPLICANT_TYPES = new Dictionary<Tag, DUPLICANTSTATS>
		{
			{
				GameTags.Minions.Models.Standard,
				DUPLICANTSTATS.STANDARD
			},
			{
				GameTags.Minions.Models.Bionic,
				DUPLICANTSTATS.BIONICS
			}
		};

		// Token: 0x040098BC RID: 39100
		public DUPLICANTSTATS.BASESTATS BaseStats = new DUPLICANTSTATS.BASESTATS();

		// Token: 0x040098BD RID: 39101
		public DUPLICANTSTATS.TEMPERATURE Temperature = new DUPLICANTSTATS.TEMPERATURE();

		// Token: 0x040098BE RID: 39102
		public DUPLICANTSTATS.BREATH Breath = new DUPLICANTSTATS.BREATH();

		// Token: 0x040098BF RID: 39103
		public DUPLICANTSTATS.LIGHT Light = new DUPLICANTSTATS.LIGHT();

		// Token: 0x040098C0 RID: 39104
		public DUPLICANTSTATS.COMBAT Combat = new DUPLICANTSTATS.COMBAT();

		// Token: 0x040098C1 RID: 39105
		public DUPLICANTSTATS.SECRETIONS Secretions = new DUPLICANTSTATS.SECRETIONS();

		// Token: 0x02002240 RID: 8768
		public static class RADIATION_DIFFICULTY_MODIFIERS
		{
			// Token: 0x040098C2 RID: 39106
			public static float HARDEST = 0.33f;

			// Token: 0x040098C3 RID: 39107
			public static float HARDER = 0.66f;

			// Token: 0x040098C4 RID: 39108
			public static float DEFAULT = 1f;

			// Token: 0x040098C5 RID: 39109
			public static float EASIER = 2f;

			// Token: 0x040098C6 RID: 39110
			public static float EASIEST = 100f;
		}

		// Token: 0x02002241 RID: 8769
		public static class RADIATION_EXPOSURE_LEVELS
		{
			// Token: 0x040098C7 RID: 39111
			public const float LOW = 100f;

			// Token: 0x040098C8 RID: 39112
			public const float MODERATE = 300f;

			// Token: 0x040098C9 RID: 39113
			public const float HIGH = 600f;

			// Token: 0x040098CA RID: 39114
			public const float DEADLY = 900f;
		}

		// Token: 0x02002242 RID: 8770
		public static class MOVEMENT_MODIFIERS
		{
			// Token: 0x040098CB RID: 39115
			public static float NEUTRAL = 1f;

			// Token: 0x040098CC RID: 39116
			public static float BONUS_1 = 1.1f;

			// Token: 0x040098CD RID: 39117
			public static float BONUS_2 = 1.25f;

			// Token: 0x040098CE RID: 39118
			public static float BONUS_3 = 1.5f;

			// Token: 0x040098CF RID: 39119
			public static float BONUS_4 = 1.75f;

			// Token: 0x040098D0 RID: 39120
			public static float PENALTY_1 = 0.9f;

			// Token: 0x040098D1 RID: 39121
			public static float PENALTY_2 = 0.75f;

			// Token: 0x040098D2 RID: 39122
			public static float PENALTY_3 = 0.5f;

			// Token: 0x040098D3 RID: 39123
			public static float PENALTY_4 = 0.25f;
		}

		// Token: 0x02002243 RID: 8771
		public static class QOL_STRESS
		{
			// Token: 0x040098D4 RID: 39124
			public const float ABOVE_EXPECTATIONS = -0.016666668f;

			// Token: 0x040098D5 RID: 39125
			public const float AT_EXPECTATIONS = -0.008333334f;

			// Token: 0x040098D6 RID: 39126
			public const float MIN_STRESS = -0.033333335f;

			// Token: 0x02002244 RID: 8772
			public static class BELOW_EXPECTATIONS
			{
				// Token: 0x040098D7 RID: 39127
				public const float EASY = 0.0033333334f;

				// Token: 0x040098D8 RID: 39128
				public const float NEUTRAL = 0.004166667f;

				// Token: 0x040098D9 RID: 39129
				public const float HARD = 0.008333334f;

				// Token: 0x040098DA RID: 39130
				public const float VERYHARD = 0.016666668f;
			}

			// Token: 0x02002245 RID: 8773
			public static class MAX_STRESS
			{
				// Token: 0x040098DB RID: 39131
				public const float EASY = 0.016666668f;

				// Token: 0x040098DC RID: 39132
				public const float NEUTRAL = 0.041666668f;

				// Token: 0x040098DD RID: 39133
				public const float HARD = 0.05f;

				// Token: 0x040098DE RID: 39134
				public const float VERYHARD = 0.083333336f;
			}
		}

		// Token: 0x02002246 RID: 8774
		public static class CLOTHING
		{
			// Token: 0x02002247 RID: 8775
			public class DECOR_MODIFICATION
			{
				// Token: 0x040098DF RID: 39135
				public const int NEGATIVE_SIGNIFICANT = -30;

				// Token: 0x040098E0 RID: 39136
				public const int NEGATIVE_MILD = -10;

				// Token: 0x040098E1 RID: 39137
				public const int BASIC = -5;

				// Token: 0x040098E2 RID: 39138
				public const int POSITIVE_MILD = 10;

				// Token: 0x040098E3 RID: 39139
				public const int POSITIVE_SIGNIFICANT = 30;

				// Token: 0x040098E4 RID: 39140
				public const int POSITIVE_MAJOR = 40;
			}

			// Token: 0x02002248 RID: 8776
			public class CONDUCTIVITY_BARRIER_MODIFICATION
			{
				// Token: 0x040098E5 RID: 39141
				public const float THIN = 0.0005f;

				// Token: 0x040098E6 RID: 39142
				public const float BASIC = 0.0025f;

				// Token: 0x040098E7 RID: 39143
				public const float THICK = 0.008f;
			}

			// Token: 0x02002249 RID: 8777
			public class SWEAT_EFFICIENCY_MULTIPLIER
			{
				// Token: 0x040098E8 RID: 39144
				public const float DIMINISH_SIGNIFICANT = -2.5f;

				// Token: 0x040098E9 RID: 39145
				public const float DIMINISH_MILD = -1.25f;

				// Token: 0x040098EA RID: 39146
				public const float NEUTRAL = 0f;

				// Token: 0x040098EB RID: 39147
				public const float IMPROVE = 2f;
			}
		}

		// Token: 0x0200224A RID: 8778
		public static class NOISE
		{
			// Token: 0x040098EC RID: 39148
			public const int THRESHOLD_PEACEFUL = 0;

			// Token: 0x040098ED RID: 39149
			public const int THRESHOLD_QUIET = 36;

			// Token: 0x040098EE RID: 39150
			public const int THRESHOLD_TOSS_AND_TURN = 45;

			// Token: 0x040098EF RID: 39151
			public const int THRESHOLD_WAKE_UP = 60;

			// Token: 0x040098F0 RID: 39152
			public const int THRESHOLD_MINOR_REACTION = 80;

			// Token: 0x040098F1 RID: 39153
			public const int THRESHOLD_MAJOR_REACTION = 106;

			// Token: 0x040098F2 RID: 39154
			public const int THRESHOLD_EXTREME_REACTION = 125;
		}

		// Token: 0x0200224B RID: 8779
		public static class ROOM
		{
			// Token: 0x040098F3 RID: 39155
			public const float LABORATORY_RESEARCH_EFFICIENCY_BONUS = 0.1f;
		}

		// Token: 0x0200224C RID: 8780
		public class DISTRIBUTIONS
		{
			// Token: 0x0600B8F3 RID: 47347 RVA: 0x00116F4C File Offset: 0x0011514C
			public static int[] GetRandomDistribution()
			{
				return DUPLICANTSTATS.DISTRIBUTIONS.TYPES[UnityEngine.Random.Range(0, DUPLICANTSTATS.DISTRIBUTIONS.TYPES.Count)];
			}

			// Token: 0x040098F4 RID: 39156
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

		// Token: 0x0200224D RID: 8781
		public struct TraitVal
		{
			// Token: 0x040098F5 RID: 39157
			public string id;

			// Token: 0x040098F6 RID: 39158
			public int statBonus;

			// Token: 0x040098F7 RID: 39159
			public int impact;

			// Token: 0x040098F8 RID: 39160
			public int rarity;

			// Token: 0x040098F9 RID: 39161
			public string dlcId;

			// Token: 0x040098FA RID: 39162
			public List<string> mutuallyExclusiveTraits;

			// Token: 0x040098FB RID: 39163
			public List<HashedString> mutuallyExclusiveAptitudes;

			// Token: 0x040098FC RID: 39164
			public bool doNotGenerateTrait;
		}

		// Token: 0x0200224E RID: 8782
		public class ATTRIBUTE_LEVELING
		{
			// Token: 0x040098FD RID: 39165
			public static int MAX_GAINED_ATTRIBUTE_LEVEL = 20;

			// Token: 0x040098FE RID: 39166
			public static int TARGET_MAX_LEVEL_CYCLE = 400;

			// Token: 0x040098FF RID: 39167
			public static float EXPERIENCE_LEVEL_POWER = 1.7f;

			// Token: 0x04009900 RID: 39168
			public static float FULL_EXPERIENCE = 1f;

			// Token: 0x04009901 RID: 39169
			public static float ALL_DAY_EXPERIENCE = DUPLICANTSTATS.ATTRIBUTE_LEVELING.FULL_EXPERIENCE / 0.8f;

			// Token: 0x04009902 RID: 39170
			public static float MOST_DAY_EXPERIENCE = DUPLICANTSTATS.ATTRIBUTE_LEVELING.FULL_EXPERIENCE / 0.5f;

			// Token: 0x04009903 RID: 39171
			public static float PART_DAY_EXPERIENCE = DUPLICANTSTATS.ATTRIBUTE_LEVELING.FULL_EXPERIENCE / 0.25f;

			// Token: 0x04009904 RID: 39172
			public static float BARELY_EVER_EXPERIENCE = DUPLICANTSTATS.ATTRIBUTE_LEVELING.FULL_EXPERIENCE / 0.1f;
		}

		// Token: 0x0200224F RID: 8783
		public class BASESTATS
		{
			// Token: 0x17000BE3 RID: 3043
			// (get) Token: 0x0600B8F8 RID: 47352 RVA: 0x00116F68 File Offset: 0x00115168
			public float CALORIES_BURNED_PER_SECOND
			{
				get
				{
					return this.CALORIES_BURNED_PER_CYCLE / 600f;
				}
			}

			// Token: 0x17000BE4 RID: 3044
			// (get) Token: 0x0600B8F9 RID: 47353 RVA: 0x00116F76 File Offset: 0x00115176
			public float HUNGRY_THRESHOLD
			{
				get
				{
					return this.SATISFIED_THRESHOLD - -this.CALORIES_BURNED_PER_CYCLE * 0.5f / this.MAX_CALORIES;
				}
			}

			// Token: 0x17000BE5 RID: 3045
			// (get) Token: 0x0600B8FA RID: 47354 RVA: 0x00116F93 File Offset: 0x00115193
			public float STARVING_THRESHOLD
			{
				get
				{
					return -this.CALORIES_BURNED_PER_CYCLE / this.MAX_CALORIES;
				}
			}

			// Token: 0x17000BE6 RID: 3046
			// (get) Token: 0x0600B8FB RID: 47355 RVA: 0x00116FA3 File Offset: 0x001151A3
			public float DUPLICANT_COOLING_KILOWATTS
			{
				get
				{
					return this.COOLING_EFFICIENCY * -this.CALORIES_BURNED_PER_SECOND * 0.001f * this.KCAL2JOULES / 1000f;
				}
			}

			// Token: 0x17000BE7 RID: 3047
			// (get) Token: 0x0600B8FC RID: 47356 RVA: 0x00116FC6 File Offset: 0x001151C6
			public float DUPLICANT_WARMING_KILOWATTS
			{
				get
				{
					return this.WARMING_EFFICIENCY * -this.CALORIES_BURNED_PER_SECOND * 0.001f * this.KCAL2JOULES / 1000f;
				}
			}

			// Token: 0x17000BE8 RID: 3048
			// (get) Token: 0x0600B8FD RID: 47357 RVA: 0x00116FE9 File Offset: 0x001151E9
			public float DUPLICANT_BASE_GENERATION_KILOWATTS
			{
				get
				{
					return this.HEAT_GENERATION_EFFICIENCY * -this.CALORIES_BURNED_PER_SECOND * 0.001f * this.KCAL2JOULES / 1000f;
				}
			}

			// Token: 0x17000BE9 RID: 3049
			// (get) Token: 0x0600B8FE RID: 47358 RVA: 0x00116F68 File Offset: 0x00115168
			public float GUESSTIMATE_CALORIES_BURNED_PER_SECOND
			{
				get
				{
					return this.CALORIES_BURNED_PER_CYCLE / 600f;
				}
			}

			// Token: 0x04009905 RID: 39173
			public float DEFAULT_MASS = 30f;

			// Token: 0x04009906 RID: 39174
			public float STAMINA_USED_PER_SECOND = -0.11666667f;

			// Token: 0x04009907 RID: 39175
			public float TRANSIT_TUBE_TRAVEL_SPEED = 18f;

			// Token: 0x04009908 RID: 39176
			public float OXYGEN_USED_PER_SECOND = 0.1f;

			// Token: 0x04009909 RID: 39177
			public float OXYGEN_TO_CO2_CONVERSION = 0.02f;

			// Token: 0x0400990A RID: 39178
			public float LOW_OXYGEN_THRESHOLD = 0.52f;

			// Token: 0x0400990B RID: 39179
			public float NO_OXYGEN_THRESHOLD = 0.05f;

			// Token: 0x0400990C RID: 39180
			public float RECOVER_BREATH_DELTA = 3f;

			// Token: 0x0400990D RID: 39181
			public float MIN_CO2_TO_EMIT = 0.02f;

			// Token: 0x0400990E RID: 39182
			public float BLADDER_INCREASE_PER_SECOND = 0.16666667f;

			// Token: 0x0400990F RID: 39183
			public float DECOR_EXPECTATION;

			// Token: 0x04009910 RID: 39184
			public float FOOD_QUALITY_EXPECTATION;

			// Token: 0x04009911 RID: 39185
			public float RECREATION_EXPECTATION = 2f;

			// Token: 0x04009912 RID: 39186
			public float MAX_PROFESSION_DECOR_EXPECTATION = 75f;

			// Token: 0x04009913 RID: 39187
			public float MAX_PROFESSION_FOOD_EXPECTATION;

			// Token: 0x04009914 RID: 39188
			public int MAX_UNDERWATER_TRAVEL_COST = 8;

			// Token: 0x04009915 RID: 39189
			public float TOILET_EFFICIENCY = 1f;

			// Token: 0x04009916 RID: 39190
			public float ROOM_TEMPERATURE_PREFERENCE;

			// Token: 0x04009917 RID: 39191
			public int BUILDING_DAMAGE_ACTING_OUT = 100;

			// Token: 0x04009918 RID: 39192
			public float IMMUNE_LEVEL_MAX = 100f;

			// Token: 0x04009919 RID: 39193
			public float IMMUNE_LEVEL_RECOVERY = 0.025f;

			// Token: 0x0400991A RID: 39194
			public float CARRY_CAPACITY = 200f;

			// Token: 0x0400991B RID: 39195
			public float HIT_POINTS = 100f;

			// Token: 0x0400991C RID: 39196
			public float RADIATION_RESISTANCE;

			// Token: 0x0400991D RID: 39197
			public string NAV_GRID_NAME = "MinionNavGrid";

			// Token: 0x0400991E RID: 39198
			public float KCAL2JOULES = 4184f;

			// Token: 0x0400991F RID: 39199
			public float MAX_CALORIES = 4000000f;

			// Token: 0x04009920 RID: 39200
			public float CALORIES_BURNED_PER_CYCLE = -1000000f;

			// Token: 0x04009921 RID: 39201
			public float SATISFIED_THRESHOLD = 0.95f;

			// Token: 0x04009922 RID: 39202
			public float COOLING_EFFICIENCY = 0.08f;

			// Token: 0x04009923 RID: 39203
			public float WARMING_EFFICIENCY = 0.08f;

			// Token: 0x04009924 RID: 39204
			public float HEAT_GENERATION_EFFICIENCY = 0.012f;

			// Token: 0x04009925 RID: 39205
			public float GUESSTIMATE_CALORIES_PER_CYCLE = -1600000f;
		}

		// Token: 0x02002250 RID: 8784
		public class TEMPERATURE
		{
			// Token: 0x04009926 RID: 39206
			public DUPLICANTSTATS.TEMPERATURE.EXTERNAL External = new DUPLICANTSTATS.TEMPERATURE.EXTERNAL();

			// Token: 0x04009927 RID: 39207
			public DUPLICANTSTATS.TEMPERATURE.INTERNAL Internal = new DUPLICANTSTATS.TEMPERATURE.INTERNAL();

			// Token: 0x04009928 RID: 39208
			public DUPLICANTSTATS.TEMPERATURE.CONDUCTIVITY_BARRIER_MODIFICATION Conductivity_Barrier_Modification = new DUPLICANTSTATS.TEMPERATURE.CONDUCTIVITY_BARRIER_MODIFICATION();

			// Token: 0x04009929 RID: 39209
			public float SKIN_THICKNESS = 0.002f;

			// Token: 0x0400992A RID: 39210
			public float SURFACE_AREA = 1f;

			// Token: 0x0400992B RID: 39211
			public float GROUND_TRANSFER_SCALE;

			// Token: 0x02002251 RID: 8785
			public class EXTERNAL
			{
				// Token: 0x0400992C RID: 39212
				public float THRESHOLD_COLD = 283.15f;

				// Token: 0x0400992D RID: 39213
				public float THRESHOLD_HOT = 306.15f;

				// Token: 0x0400992E RID: 39214
				public float THRESHOLD_SCALDING = 345f;
			}

			// Token: 0x02002252 RID: 8786
			public class INTERNAL
			{
				// Token: 0x0400992F RID: 39215
				public float IDEAL = 310.15f;

				// Token: 0x04009930 RID: 39216
				public float THRESHOLD_HYPOTHERMIA = 308.15f;

				// Token: 0x04009931 RID: 39217
				public float THRESHOLD_HYPERTHERMIA = 312.15f;

				// Token: 0x04009932 RID: 39218
				public float THRESHOLD_FATAL_HOT = 320.15f;

				// Token: 0x04009933 RID: 39219
				public float THRESHOLD_FATAL_COLD = 300.15f;
			}

			// Token: 0x02002253 RID: 8787
			public class CONDUCTIVITY_BARRIER_MODIFICATION
			{
				// Token: 0x04009934 RID: 39220
				public float SKINNY = -0.005f;

				// Token: 0x04009935 RID: 39221
				public float PUDGY = 0.005f;
			}
		}

		// Token: 0x02002254 RID: 8788
		public class BREATH
		{
			// Token: 0x17000BEA RID: 3050
			// (get) Token: 0x0600B904 RID: 47364 RVA: 0x001170D1 File Offset: 0x001152D1
			public float RETREAT_AMOUNT
			{
				get
				{
					return this.RETREAT_AT_SECONDS / this.BREATH_BAR_TOTAL_SECONDS * this.BREATH_BAR_TOTAL_AMOUNT;
				}
			}

			// Token: 0x17000BEB RID: 3051
			// (get) Token: 0x0600B905 RID: 47365 RVA: 0x001170E7 File Offset: 0x001152E7
			public float SUFFOCATE_AMOUNT
			{
				get
				{
					return this.SUFFOCATION_WARN_AT_SECONDS / this.BREATH_BAR_TOTAL_SECONDS * this.BREATH_BAR_TOTAL_AMOUNT;
				}
			}

			// Token: 0x17000BEC RID: 3052
			// (get) Token: 0x0600B906 RID: 47366 RVA: 0x001170FD File Offset: 0x001152FD
			public float BREATH_RATE
			{
				get
				{
					return this.BREATH_BAR_TOTAL_AMOUNT / this.BREATH_BAR_TOTAL_SECONDS;
				}
			}

			// Token: 0x04009936 RID: 39222
			private float BREATH_BAR_TOTAL_SECONDS = 110f;

			// Token: 0x04009937 RID: 39223
			private float RETREAT_AT_SECONDS = 80f;

			// Token: 0x04009938 RID: 39224
			private float SUFFOCATION_WARN_AT_SECONDS = 50f;

			// Token: 0x04009939 RID: 39225
			public float BREATH_BAR_TOTAL_AMOUNT = 100f;
		}

		// Token: 0x02002255 RID: 8789
		public class LIGHT
		{
			// Token: 0x0400993A RID: 39226
			public int LUX_SUNBURN = 72000;

			// Token: 0x0400993B RID: 39227
			public float SUNBURN_DELAY_TIME = 120f;

			// Token: 0x0400993C RID: 39228
			public int LUX_PLEASANT_LIGHT = 40000;

			// Token: 0x0400993D RID: 39229
			public float LIGHT_WORK_EFFICIENCY_BONUS = 0.15f;

			// Token: 0x0400993E RID: 39230
			public int NO_LIGHT;

			// Token: 0x0400993F RID: 39231
			public int VERY_LOW_LIGHT = 1;

			// Token: 0x04009940 RID: 39232
			public int LOW_LIGHT = 500;

			// Token: 0x04009941 RID: 39233
			public int MEDIUM_LIGHT = 1000;

			// Token: 0x04009942 RID: 39234
			public int HIGH_LIGHT = 10000;

			// Token: 0x04009943 RID: 39235
			public int VERY_HIGH_LIGHT = 50000;

			// Token: 0x04009944 RID: 39236
			public int MAX_LIGHT = 100000;
		}

		// Token: 0x02002256 RID: 8790
		public class COMBAT
		{
			// Token: 0x04009945 RID: 39237
			public DUPLICANTSTATS.COMBAT.BASICWEAPON BasicWeapon = new DUPLICANTSTATS.COMBAT.BASICWEAPON();

			// Token: 0x04009946 RID: 39238
			public Health.HealthState FLEE_THRESHOLD = Health.HealthState.Critical;

			// Token: 0x02002257 RID: 8791
			public class BASICWEAPON
			{
				// Token: 0x04009947 RID: 39239
				public float ATTACKS_PER_SECOND = 2f;

				// Token: 0x04009948 RID: 39240
				public float MIN_DAMAGE_PER_HIT = 1f;

				// Token: 0x04009949 RID: 39241
				public float MAX_DAMAGE_PER_HIT = 1f;

				// Token: 0x0400994A RID: 39242
				public AttackProperties.TargetType TARGET_TYPE;

				// Token: 0x0400994B RID: 39243
				public AttackProperties.DamageType DAMAGE_TYPE;

				// Token: 0x0400994C RID: 39244
				public int MAX_HITS = 1;

				// Token: 0x0400994D RID: 39245
				public float AREA_OF_EFFECT_RADIUS;
			}
		}

		// Token: 0x02002258 RID: 8792
		public class SECRETIONS
		{
			// Token: 0x0400994E RID: 39246
			public float PEE_FUSE_TIME = 120f;

			// Token: 0x0400994F RID: 39247
			public float PEE_PER_FLOOR_PEE = 2f;

			// Token: 0x04009950 RID: 39248
			public float PEE_PER_TOILET_PEE = 6.7f;

			// Token: 0x04009951 RID: 39249
			public string PEE_DISEASE = "FoodPoisoning";

			// Token: 0x04009952 RID: 39250
			public int DISEASE_PER_PEE = 100000;

			// Token: 0x04009953 RID: 39251
			public int DISEASE_PER_VOMIT = 100000;
		}
	}
}
