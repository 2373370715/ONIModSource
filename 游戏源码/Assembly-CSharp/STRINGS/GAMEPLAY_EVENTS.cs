using System;

namespace STRINGS
{
	// Token: 0x02002F6C RID: 12140
	public class GAMEPLAY_EVENTS
	{
		// Token: 0x0400C49B RID: 50331
		public static LocString CANCELED = "{0} Canceled";

		// Token: 0x0400C49C RID: 50332
		public static LocString CANCELED_TOOLTIP = "The {0} event was canceled";

		// Token: 0x0400C49D RID: 50333
		public static LocString DEFAULT_OPTION_NAME = "OK";

		// Token: 0x0400C49E RID: 50334
		public static LocString DEFAULT_OPTION_CONSIDER_NAME = "Let me think about it";

		// Token: 0x0400C49F RID: 50335
		public static LocString CHAIN_EVENT_TOOLTIP = "This event is a chain event";

		// Token: 0x0400C4A0 RID: 50336
		public static LocString BONUS_EVENT_DESCRIPTION = "{effects} for {duration}";

		// Token: 0x02002F6D RID: 12141
		public class LOCATIONS
		{
			// Token: 0x0400C4A1 RID: 50337
			public static LocString NONE_AVAILABLE = "No location currently available";

			// Token: 0x0400C4A2 RID: 50338
			public static LocString SUN = "The Sun";

			// Token: 0x0400C4A3 RID: 50339
			public static LocString SURFACE = "Planetary Surface";

			// Token: 0x0400C4A4 RID: 50340
			public static LocString PRINTING_POD = BUILDINGS.PREFABS.HEADQUARTERS.NAME;

			// Token: 0x0400C4A5 RID: 50341
			public static LocString COLONY_WIDE = "Colonywide";
		}

		// Token: 0x02002F6E RID: 12142
		public class TIMES
		{
			// Token: 0x0400C4A6 RID: 50342
			public static LocString NOW = "Right now";

			// Token: 0x0400C4A7 RID: 50343
			public static LocString IN_CYCLES = "In {0} cycles";

			// Token: 0x0400C4A8 RID: 50344
			public static LocString UNKNOWN = "Sometime";
		}

		// Token: 0x02002F6F RID: 12143
		public class EVENT_TYPES
		{
			// Token: 0x02002F70 RID: 12144
			public class PARTY
			{
				// Token: 0x0400C4A9 RID: 50345
				public static LocString NAME = "Party";

				// Token: 0x0400C4AA RID: 50346
				public static LocString DESCRIPTION = "THIS EVENT IS NOT WORKING\n{host} is throwing a birthday party for {dupe}. Make sure there is an available " + ROOMS.TYPES.REC_ROOM.NAME + " for the party.\n\nSocial events are good for Duplicant morale. Rejecting this party will hurt {host} and {dupe}'s fragile ego.";

				// Token: 0x0400C4AB RID: 50347
				public static LocString CANCELED_NO_ROOM_TITLE = "Party Canceled";

				// Token: 0x0400C4AC RID: 50348
				public static LocString CANCELED_NO_ROOM_DESCRIPTION = "The party was canceled because no " + ROOMS.TYPES.REC_ROOM.NAME + " was available.";

				// Token: 0x0400C4AD RID: 50349
				public static LocString UNDERWAY = "Party Happening";

				// Token: 0x0400C4AE RID: 50350
				public static LocString UNDERWAY_TOOLTIP = "There's a party going on";

				// Token: 0x0400C4AF RID: 50351
				public static LocString ACCEPT_OPTION_NAME = "Allow the party to happen";

				// Token: 0x0400C4B0 RID: 50352
				public static LocString ACCEPT_OPTION_DESC = "Party goers will get {goodEffect}";

				// Token: 0x0400C4B1 RID: 50353
				public static LocString ACCEPT_OPTION_INVALID_TOOLTIP = "A cake must be built for this event to take place.";

				// Token: 0x0400C4B2 RID: 50354
				public static LocString REJECT_OPTION_NAME = "Cancel the party";

				// Token: 0x0400C4B3 RID: 50355
				public static LocString REJECT_OPTION_DESC = "{host} and {dupe} gain {badEffect}";
			}

			// Token: 0x02002F71 RID: 12145
			public class ECLIPSE
			{
				// Token: 0x0400C4B4 RID: 50356
				public static LocString NAME = "Eclipse";

				// Token: 0x0400C4B5 RID: 50357
				public static LocString DESCRIPTION = "A celestial object has obscured the sunlight";
			}

			// Token: 0x02002F72 RID: 12146
			public class SOLAR_FLARE
			{
				// Token: 0x0400C4B6 RID: 50358
				public static LocString NAME = "Solar Storm";

				// Token: 0x0400C4B7 RID: 50359
				public static LocString DESCRIPTION = "A solar flare is headed this way";
			}

			// Token: 0x02002F73 RID: 12147
			public class CREATURE_SPAWN
			{
				// Token: 0x0400C4B8 RID: 50360
				public static LocString NAME = "Critter Infestation";

				// Token: 0x0400C4B9 RID: 50361
				public static LocString DESCRIPTION = "There was a massive influx of destructive critters";
			}

			// Token: 0x02002F74 RID: 12148
			public class SATELLITE_CRASH
			{
				// Token: 0x0400C4BA RID: 50362
				public static LocString NAME = "Satellite Crash";

				// Token: 0x0400C4BB RID: 50363
				public static LocString DESCRIPTION = "Mysterious space junk has crashed into the surface.\n\nIt may contain useful resources or information, but it may also be dangerous. Approach with caution.";
			}

			// Token: 0x02002F75 RID: 12149
			public class FOOD_FIGHT
			{
				// Token: 0x0400C4BC RID: 50364
				public static LocString NAME = "Food Fight";

				// Token: 0x0400C4BD RID: 50365
				public static LocString DESCRIPTION = "Duplicants will throw food at each other for recreation\n\nIt may be wasteful, but everyone who participates will benefit from a major stress reduction.";

				// Token: 0x0400C4BE RID: 50366
				public static LocString UNDERWAY = "Food Fight";

				// Token: 0x0400C4BF RID: 50367
				public static LocString UNDERWAY_TOOLTIP = "There is a food fight happening now";

				// Token: 0x0400C4C0 RID: 50368
				public static LocString ACCEPT_OPTION_NAME = "Duplicants start preparing to fight.";

				// Token: 0x0400C4C1 RID: 50369
				public static LocString ACCEPT_OPTION_DETAILS = "(Plus morale)";

				// Token: 0x0400C4C2 RID: 50370
				public static LocString REJECT_OPTION_NAME = "No food fight today";

				// Token: 0x0400C4C3 RID: 50371
				public static LocString REJECT_OPTION_DETAILS = "Sadface";
			}

			// Token: 0x02002F76 RID: 12150
			public class PLANT_BLIGHT
			{
				// Token: 0x0400C4C4 RID: 50372
				public static LocString NAME = "Plant Blight: {plant}";

				// Token: 0x0400C4C5 RID: 50373
				public static LocString DESCRIPTION = "Our {plant} crops have been afflicted by a fungal sickness!\n\nI must get the Duplicants to uproot and compost the sick plants to save our farms.";

				// Token: 0x0400C4C6 RID: 50374
				public static LocString SUCCESS = "Blight Managed: {plant}";

				// Token: 0x0400C4C7 RID: 50375
				public static LocString SUCCESS_TOOLTIP = "All the blighted {plant} plants have been dealt with, halting the infection.";
			}

			// Token: 0x02002F77 RID: 12151
			public class CRYOFRIEND
			{
				// Token: 0x0400C4C8 RID: 50376
				public static LocString NAME = "New Event: A Frozen Friend";

				// Token: 0x0400C4C9 RID: 50377
				public static LocString DESCRIPTION = string.Concat(new string[]
				{
					"{dupe} has made an amazing discovery! A barely working ",
					BUILDINGS.PREFABS.CRYOTANK.NAME,
					" has been uncovered containing a {friend} inside in a frozen state.\n\n{dupe} was successful in thawing {friend} and this encounter has filled both Duplicants with a sense of hope, something they will desperately need to keep their ",
					UI.FormatAsLink("Morale", "MORALE"),
					" up when facing the dangers ahead."
				});

				// Token: 0x0400C4CA RID: 50378
				public static LocString BUTTON = "{friend} is thawed!";
			}

			// Token: 0x02002F78 RID: 12152
			public class WARPWORLDREVEAL
			{
				// Token: 0x0400C4CB RID: 50379
				public static LocString NAME = "New Event: Personnel Teleporter";

				// Token: 0x0400C4CC RID: 50380
				public static LocString DESCRIPTION = "I've discovered a functioning teleportation device with a pre-programmed destination.\n\nIt appears to go to another " + UI.CLUSTERMAP.PLANETOID + ", and I'm fairly certain there's a return device on the other end.\n\nI could send a Duplicant through safely if I desired.";

				// Token: 0x0400C4CD RID: 50381
				public static LocString BUTTON = "See Destination";
			}

			// Token: 0x02002F79 RID: 12153
			public class ARTIFACT_REVEAL
			{
				// Token: 0x0400C4CE RID: 50382
				public static LocString NAME = "New Event: Artifact Analyzed";

				// Token: 0x0400C4CF RID: 50383
				public static LocString DESCRIPTION = "An artifact from a past civilization was analyzed.\n\n{desc}";

				// Token: 0x0400C4D0 RID: 50384
				public static LocString BUTTON = "Close";
			}
		}

		// Token: 0x02002F7A RID: 12154
		public class BONUS
		{
			// Token: 0x02002F7B RID: 12155
			public class BONUSDREAM1
			{
				// Token: 0x0400C4D1 RID: 50385
				public static LocString NAME = "Good Dream";

				// Token: 0x0400C4D2 RID: 50386
				public static LocString DESCRIPTION = "I've observed many improvements to {dupe}'s demeanor today. Analysis indicates unusually high amounts of dopamine in their system. There's a good chance this is due to an exceptionally good dream and analysis indicates that current sleeping conditions may have contributed to this occurrence.\n\nFurther improvements to sleeping conditions may have additional positive effects to the " + UI.FormatAsLink("Morale", "MORALE") + " of {dupe} and other Duplicants.";

				// Token: 0x0400C4D3 RID: 50387
				public static LocString CHAIN_TOOLTIP = "Improving the living conditions of {dupe} will lead to more good dreams.";
			}

			// Token: 0x02002F7C RID: 12156
			public class BONUSDREAM2
			{
				// Token: 0x0400C4D4 RID: 50388
				public static LocString NAME = "Really Good Dream";

				// Token: 0x0400C4D5 RID: 50389
				public static LocString DESCRIPTION = "{dupe} had another really good dream and the resulting release of dopamine has made this Duplicant energetic and full of possibilities! This is an encouraging byproduct of improving the living conditions of the colony.\n\nBased on these observations, building a better sleeping area for my Duplicants will have a similar effect on their " + UI.FormatAsLink("Morale", "MORALE") + ".";
			}

			// Token: 0x02002F7D RID: 12157
			public class BONUSDREAM3
			{
				// Token: 0x0400C4D6 RID: 50390
				public static LocString NAME = "Great Dream";

				// Token: 0x0400C4D7 RID: 50391
				public static LocString DESCRIPTION = "I have detected a distinct spring in {dupe}'s step today. There is a good chance that this Duplicant had another great dream last night. Such incidents are further indications that working on the care and comfort of the colony is not a waste of time.\n\nI do wonder though: What do Duplicants dream of?";
			}

			// Token: 0x02002F7E RID: 12158
			public class BONUSDREAM4
			{
				// Token: 0x0400C4D8 RID: 50392
				public static LocString NAME = "Amazing Dream";

				// Token: 0x0400C4D9 RID: 50393
				public static LocString DESCRIPTION = "{dupe}'s dream last night must have been simply amazing! Their dopamine levels are at an all time high. Based on these results, it can be safely assumed that improving the living conditions of my Duplicants will reduce " + UI.FormatAsLink("Stress", "STRESS") + " and have similar positive effects on their well-being.\n\nObservations such as this are an integral and enjoyable part of science. When I see my Duplicants happy, I can't help but share in some of their joy.";
			}

			// Token: 0x02002F7F RID: 12159
			public class BONUSTOILET1
			{
				// Token: 0x0400C4DA RID: 50394
				public static LocString NAME = "Small Comforts";

				// Token: 0x0400C4DB RID: 50395
				public static LocString DESCRIPTION = string.Concat(new string[]
				{
					"{dupe} recently visited an Outhouse and appears to have appreciated the small comforts based on the marked increase to their ",
					UI.FormatAsLink("Morale", "MORALE"),
					".\n\nHigh ",
					UI.FormatAsLink("Morale", "MORALE"),
					" has been linked to a better work ethic and greater enthusiasm for complex jobs, which are essential in building a successful new colony."
				});
			}

			// Token: 0x02002F80 RID: 12160
			public class BONUSTOILET2
			{
				// Token: 0x0400C4DC RID: 50396
				public static LocString NAME = "Greater Comforts";

				// Token: 0x0400C4DD RID: 50397
				public static LocString DESCRIPTION = "{dupe} used a Lavatory and analysis shows a decided improvement to this Duplicant's " + UI.FormatAsLink("Morale", "MORALE") + ".\n\nAs my colony grows and expands, it's important not to ignore the benefits of giving my Duplicants a pleasant place to relieve themselves.";
			}

			// Token: 0x02002F81 RID: 12161
			public class BONUSTOILET3
			{
				// Token: 0x0400C4DE RID: 50398
				public static LocString NAME = "Small Luxury";

				// Token: 0x0400C4DF RID: 50399
				public static LocString DESCRIPTION = string.Concat(new string[]
				{
					"{dupe} visited a ",
					ROOMS.TYPES.LATRINE.NAME,
					" and experienced luxury unlike they anything this Duplicant had previously experienced as analysis has revealed yet another boost to their ",
					UI.FormatAsLink("Morale", "MORALE"),
					".\n\nIt is unclear whether this development is a result of increased hygiene or whether there is something else inherently about working plumbing which would improve ",
					UI.FormatAsLink("Morale", "MORALE"),
					" in this way. Further analysis is needed."
				});
			}

			// Token: 0x02002F82 RID: 12162
			public class BONUSTOILET4
			{
				// Token: 0x0400C4E0 RID: 50400
				public static LocString NAME = "Greater Luxury";

				// Token: 0x0400C4E1 RID: 50401
				public static LocString DESCRIPTION = "{dupe} visited a Washroom and the experience has left this Duplicant with significantly improved " + UI.FormatAsLink("Morale", "MORALE") + ". Analysis indicates this improvement should continue for many cycles.\n\nThe relationship of my Duplicants and their surroundings is an interesting aspect of colony life. I should continue to watch future developments in this department closely.";
			}

			// Token: 0x02002F83 RID: 12163
			public class BONUSRESEARCH
			{
				// Token: 0x0400C4E2 RID: 50402
				public static LocString NAME = "Inspired Learner";

				// Token: 0x0400C4E3 RID: 50403
				public static LocString DESCRIPTION = string.Concat(new string[]
				{
					"Analysis indicates that the appearance of a ",
					UI.PRE_KEYWORD,
					"Research Station",
					UI.PST_KEYWORD,
					" has inspired {dupe} and heightened their brain activity on a cellular level.\n\nBrain stimulation is important if my Duplicants are going to adapt and innovate in their increasingly harsh environment."
				});
			}

			// Token: 0x02002F84 RID: 12164
			public class BONUSDIGGING1
			{
				// Token: 0x0400C4E4 RID: 50404
				public static LocString NAME = "Hot Diggity!";

				// Token: 0x0400C4E5 RID: 50405
				public static LocString DESCRIPTION = "Some interesting data has revealed that {dupe} has had a marked increase in physical abilities, an increase that cannot entirely be attributed to the usual improvements that occur after regular physical activity.\n\nBased on previous observations this Duplicant's positive associations with digging appear to account for this additional physical boost.\n\nThis would mean the personal preferences of my Duplicants are directly correlated to how hard they work. How interesting...";
			}

			// Token: 0x02002F85 RID: 12165
			public class BONUSSTORAGE
			{
				// Token: 0x0400C4E6 RID: 50406
				public static LocString NAME = "Something in Store";

				// Token: 0x0400C4E7 RID: 50407
				public static LocString DESCRIPTION = "Data indicates that {dupe}'s activity in storing something in a Storage Bin has led to an increase in this Duplicant's physical strength as well as an overall improvement to their general demeanor.\n\nThere have been many studies connecting organization with an increase in well-being. It is possible this explains {dupe}'s " + UI.FormatAsLink("Morale", "MORALE") + " improvements.";
			}

			// Token: 0x02002F86 RID: 12166
			public class BONUSBUILDER
			{
				// Token: 0x0400C4E8 RID: 50408
				public static LocString NAME = "Accomplished Builder";

				// Token: 0x0400C4E9 RID: 50409
				public static LocString DESCRIPTION = "{dupe} has been hard at work building many structures crucial to the future of the colony. It seems this activity has improved this Duplicant's budding construction and mechanical skills beyond what my models predicted.\n\nWhether this increase in ability is due to them learning new skills or simply gaining self-confidence I cannot say, but this unexpected development is a welcome surprise development.";
			}

			// Token: 0x02002F87 RID: 12167
			public class BONUSOXYGEN
			{
				// Token: 0x0400C4EA RID: 50410
				public static LocString NAME = "Fresh Air";

				// Token: 0x0400C4EB RID: 50411
				public static LocString DESCRIPTION = "{dupe} is experiencing a sudden unexpected improvement to their physical prowess which appears to be a result of exposure to elevated levels of oxygen from passing by an Oxygen Diffuser.\n\nObservations such as this are important in documenting just how beneficial having access to oxygen is to my colony.";
			}

			// Token: 0x02002F88 RID: 12168
			public class BONUSALGAE
			{
				// Token: 0x0400C4EC RID: 50412
				public static LocString NAME = "Fresh Algae Smell";

				// Token: 0x0400C4ED RID: 50413
				public static LocString DESCRIPTION = "{dupe}'s recent proximity to an Algae Terrarium has left them feeling refreshed and exuberant and is correlated to an increase in their physical attributes. It is unclear whether these physical improvements came from the excess of oxygen or the invigorating smell of algae.\n\nIt's curious that I find myself nostalgic for the smell of algae growing in a lab. But how could this be...?";
			}

			// Token: 0x02002F89 RID: 12169
			public class BONUSGENERATOR
			{
				// Token: 0x0400C4EE RID: 50414
				public static LocString NAME = "Exercised";

				// Token: 0x0400C4EF RID: 50415
				public static LocString DESCRIPTION = "{dupe} ran in a Manual Generator and the physical activity appears to have given this Duplicant increased strength and sense of well-being.\n\nWhile not the primary reason for building Manual Generators, I am very pleased to see my Duplicants reaping the " + UI.FormatAsLink("Stress", "STRESS") + " relieving benefits to physical activity.";
			}

			// Token: 0x02002F8A RID: 12170
			public class BONUSDOOR
			{
				// Token: 0x0400C4F0 RID: 50416
				public static LocString NAME = "Open and Shut";

				// Token: 0x0400C4F1 RID: 50417
				public static LocString DESCRIPTION = string.Concat(new string[]
				{
					"The act of closing a door has apparently lead to a decrease in the ",
					UI.FormatAsLink("Stress", "STRESS"),
					" levels of {dupe}, as well as decreased the exposure of this Duplicant to harmful ",
					UI.FormatAsLink("Germs", "GERMS"),
					".\n\nWhile it may be more efficient to group all my Duplicants together in common sleeping quarters, it's important to remember the mental benefits to privacy and space to express their individuality."
				});
			}

			// Token: 0x02002F8B RID: 12171
			public class BONUSHITTHEBOOKS
			{
				// Token: 0x0400C4F2 RID: 50418
				public static LocString NAME = "Hit the Books";

				// Token: 0x0400C4F3 RID: 50419
				public static LocString DESCRIPTION = "{dupe}'s recent Research errand has resulted in a significant increase to this Duplicant's brain activity. The discovery of newly found knowledge has given {dupe} an invigorating jolt of excitement.\n\nI am all too familiar with this feeling.";
			}

			// Token: 0x02002F8C RID: 12172
			public class BONUSLITWORKSPACE
			{
				// Token: 0x0400C4F4 RID: 50420
				public static LocString NAME = "Lit-erally Great";

				// Token: 0x0400C4F5 RID: 50421
				public static LocString DESCRIPTION = "{dupe}'s recent time in a well-lit area has greatly improved this Duplicant's ability to work with, and on, machinery.\n\nThis supports the prevailing theory that a well-lit workspace has many benefits beyond just improving my Duplicant's ability to see.";
			}

			// Token: 0x02002F8D RID: 12173
			public class BONUSTALKER
			{
				// Token: 0x0400C4F6 RID: 50422
				public static LocString NAME = "Big Small Talker";

				// Token: 0x0400C4F7 RID: 50423
				public static LocString DESCRIPTION = "{dupe}'s recent conversation with another Duplicant shows a correlation to improved serotonin and " + UI.FormatAsLink("Morale", "MORALE") + " levels in this Duplicant. It is very possible that small talk with a co-worker, however short and seemingly insignificant, will make my Duplicant's feel connected to the colony as a whole.\n\nAs the colony gets bigger and more sophisticated, I must ensure that the opportunity for such connections continue, for the good of my Duplicants' mental well being.";
			}
		}
	}
}
