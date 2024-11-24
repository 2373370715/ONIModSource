using System;
using TUNING;

namespace STRINGS
{
	// Token: 0x02002F8E RID: 12174
	public class ROOMS
	{
		// Token: 0x02002F8F RID: 12175
		public class CATEGORY
		{
			// Token: 0x02002F90 RID: 12176
			public class NONE
			{
				// Token: 0x0400C4F8 RID: 50424
				public static LocString NAME = "None";
			}

			// Token: 0x02002F91 RID: 12177
			public class FOOD
			{
				// Token: 0x0400C4F9 RID: 50425
				public static LocString NAME = "Dining";
			}

			// Token: 0x02002F92 RID: 12178
			public class SLEEP
			{
				// Token: 0x0400C4FA RID: 50426
				public static LocString NAME = "Sleep";
			}

			// Token: 0x02002F93 RID: 12179
			public class RECREATION
			{
				// Token: 0x0400C4FB RID: 50427
				public static LocString NAME = "Recreation";
			}

			// Token: 0x02002F94 RID: 12180
			public class BATHROOM
			{
				// Token: 0x0400C4FC RID: 50428
				public static LocString NAME = "Washroom";
			}

			// Token: 0x02002F95 RID: 12181
			public class BIONIC
			{
				// Token: 0x0400C4FD RID: 50429
				public static LocString NAME = "Workshop";
			}

			// Token: 0x02002F96 RID: 12182
			public class HOSPITAL
			{
				// Token: 0x0400C4FE RID: 50430
				public static LocString NAME = "Medical";
			}

			// Token: 0x02002F97 RID: 12183
			public class INDUSTRIAL
			{
				// Token: 0x0400C4FF RID: 50431
				public static LocString NAME = "Industrial";
			}

			// Token: 0x02002F98 RID: 12184
			public class AGRICULTURAL
			{
				// Token: 0x0400C500 RID: 50432
				public static LocString NAME = "Agriculture";
			}

			// Token: 0x02002F99 RID: 12185
			public class PARK
			{
				// Token: 0x0400C501 RID: 50433
				public static LocString NAME = "Parks";
			}

			// Token: 0x02002F9A RID: 12186
			public class SCIENCE
			{
				// Token: 0x0400C502 RID: 50434
				public static LocString NAME = "Science";
			}
		}

		// Token: 0x02002F9B RID: 12187
		public class TYPES
		{
			// Token: 0x0400C503 RID: 50435
			public static LocString CONFLICTED = "Conflicted Room";

			// Token: 0x02002F9C RID: 12188
			public class NEUTRAL
			{
				// Token: 0x0400C504 RID: 50436
				public static LocString NAME = "Miscellaneous Room";

				// Token: 0x0400C505 RID: 50437
				public static LocString DESCRIPTION = "An enclosed space with plenty of potential and no dedicated use.";

				// Token: 0x0400C506 RID: 50438
				public static LocString EFFECT = "- No effect";

				// Token: 0x0400C507 RID: 50439
				public static LocString TOOLTIP = "This area has walls and doors but no dedicated use";
			}

			// Token: 0x02002F9D RID: 12189
			public class LATRINE
			{
				// Token: 0x0400C508 RID: 50440
				public static LocString NAME = "Latrine";

				// Token: 0x0400C509 RID: 50441
				public static LocString DESCRIPTION = "It's a step up from doing one's business in full view of the rest of the colony.\n\nUsing a toilet in an enclosed room will improve Duplicants' Morale.";

				// Token: 0x0400C50A RID: 50442
				public static LocString EFFECT = "- Morale bonus";

				// Token: 0x0400C50B RID: 50443
				public static LocString TOOLTIP = "Using a toilet in an enclosed room will improve Duplicants' Morale";
			}

			// Token: 0x02002F9E RID: 12190
			public class BIONICUPKEEP
			{
				// Token: 0x0400C50C RID: 50444
				public static LocString NAME = "Workshop";

				// Token: 0x0400C50D RID: 50445
				public static LocString DESCRIPTION = "Where Bionic Duplicants can get the specialized care they need.\n\nUsing a " + BUILDINGS.PREFABS.GUNKEMPTIER.NAME + " in a Workshop will improve Bionic Duplicants' Morale.";

				// Token: 0x0400C50E RID: 50446
				public static LocString EFFECT = "- Morale bonus";

				// Token: 0x0400C50F RID: 50447
				public static LocString TOOLTIP = "Using a gunk extractor in a Workshop will improve Bionic Duplicants' Morale";
			}

			// Token: 0x02002F9F RID: 12191
			public class PLUMBEDBATHROOM
			{
				// Token: 0x0400C510 RID: 50448
				public static LocString NAME = "Washroom";

				// Token: 0x0400C511 RID: 50449
				public static LocString DESCRIPTION = "A sanctuary of personal hygiene.\n\nUsing a fully plumbed Washroom will improve Duplicants' Morale.";

				// Token: 0x0400C512 RID: 50450
				public static LocString EFFECT = "- Morale bonus";

				// Token: 0x0400C513 RID: 50451
				public static LocString TOOLTIP = "Using a fully plumbed Washroom will improve Duplicants' Morale";
			}

			// Token: 0x02002FA0 RID: 12192
			public class BARRACKS
			{
				// Token: 0x0400C514 RID: 50452
				public static LocString NAME = "Barracks";

				// Token: 0x0400C515 RID: 50453
				public static LocString DESCRIPTION = "A basic communal sleeping area for up-and-coming colonies.\n\nSleeping in Barracks will improve Duplicants' Morale.";

				// Token: 0x0400C516 RID: 50454
				public static LocString EFFECT = "- Morale bonus";

				// Token: 0x0400C517 RID: 50455
				public static LocString TOOLTIP = "Sleeping in Barracks will improve Duplicants' Morale";
			}

			// Token: 0x02002FA1 RID: 12193
			public class BEDROOM
			{
				// Token: 0x0400C518 RID: 50456
				public static LocString NAME = "Luxury Barracks";

				// Token: 0x0400C519 RID: 50457
				public static LocString DESCRIPTION = "An upscale communal sleeping area full of things that greatly enhance quality of rest for occupants.\n\nSleeping in a Luxury Barracks will improve Duplicants' Morale.";

				// Token: 0x0400C51A RID: 50458
				public static LocString EFFECT = "- Morale bonus";

				// Token: 0x0400C51B RID: 50459
				public static LocString TOOLTIP = "Sleeping in a Luxury Barracks will improve Duplicants' Morale";
			}

			// Token: 0x02002FA2 RID: 12194
			public class PRIVATE_BEDROOM
			{
				// Token: 0x0400C51C RID: 50460
				public static LocString NAME = "Private Bedroom";

				// Token: 0x0400C51D RID: 50461
				public static LocString DESCRIPTION = "A comfortable, roommate-free retreat where tired Duplicants can get uninterrupted rest.\n\nSleeping in a Private Bedroom will greatly improve Duplicants' Morale.";

				// Token: 0x0400C51E RID: 50462
				public static LocString EFFECT = "- Morale bonus";

				// Token: 0x0400C51F RID: 50463
				public static LocString TOOLTIP = "Sleeping in a Private Bedroom will greatly improve Duplicants' Morale";
			}

			// Token: 0x02002FA3 RID: 12195
			public class MESSHALL
			{
				// Token: 0x0400C520 RID: 50464
				public static LocString NAME = "Mess Hall";

				// Token: 0x0400C521 RID: 50465
				public static LocString DESCRIPTION = "A simple dining room setup that's easy to improve upon.\n\nEating at a mess table in a Mess Hall will increase Duplicants' Morale.";

				// Token: 0x0400C522 RID: 50466
				public static LocString EFFECT = "- Morale bonus";

				// Token: 0x0400C523 RID: 50467
				public static LocString TOOLTIP = "Eating at a Mess Table in a Mess Hall will improve Duplicants' Morale";
			}

			// Token: 0x02002FA4 RID: 12196
			public class KITCHEN
			{
				// Token: 0x0400C524 RID: 50468
				public static LocString NAME = "Kitchen";

				// Token: 0x0400C525 RID: 50469
				public static LocString DESCRIPTION = "A cooking area equipped to take meals to the next level.\n\nAdding ingredients from a Spice Grinder to foods cooked on an Electric Grill or Gas Range provides a variety of positive benefits.";

				// Token: 0x0400C526 RID: 50470
				public static LocString EFFECT = "- Enables Spice Grinder use";

				// Token: 0x0400C527 RID: 50471
				public static LocString TOOLTIP = "Using a Spice Grinder in a Kitchen adds benefits to foods cooked on Electric Grill or Gas Range";
			}

			// Token: 0x02002FA5 RID: 12197
			public class GREATHALL
			{
				// Token: 0x0400C528 RID: 50472
				public static LocString NAME = "Great Hall";

				// Token: 0x0400C529 RID: 50473
				public static LocString DESCRIPTION = "A great place to eat, with great decor and great company. Great!\n\nEating in a Great Hall will significantly improve Duplicants' Morale.";

				// Token: 0x0400C52A RID: 50474
				public static LocString EFFECT = "- Morale bonus";

				// Token: 0x0400C52B RID: 50475
				public static LocString TOOLTIP = "Eating in a Great Hall will significantly improve Duplicants' Morale";
			}

			// Token: 0x02002FA6 RID: 12198
			public class HOSPITAL
			{
				// Token: 0x0400C52C RID: 50476
				public static LocString NAME = "Hospital";

				// Token: 0x0400C52D RID: 50477
				public static LocString DESCRIPTION = "A dedicated medical facility that helps minimize recovery time.\n\nSick Duplicants assigned to medical buildings located within a Hospital are also less likely to spread Disease.";

				// Token: 0x0400C52E RID: 50478
				public static LocString EFFECT = "- Quarantine sick Duplicants";

				// Token: 0x0400C52F RID: 50479
				public static LocString TOOLTIP = "Sick Duplicants assigned to medical buildings located within a Hospital are less likely to spread Disease";
			}

			// Token: 0x02002FA7 RID: 12199
			public class MASSAGE_CLINIC
			{
				// Token: 0x0400C530 RID: 50480
				public static LocString NAME = "Massage Clinic";

				// Token: 0x0400C531 RID: 50481
				public static LocString DESCRIPTION = "A soothing space with a very relaxing ambience, especially when well-decorated.\n\nReceiving massages at a Massage Clinic will significantly improve Stress reduction.";

				// Token: 0x0400C532 RID: 50482
				public static LocString EFFECT = "- Massage stress relief bonus";

				// Token: 0x0400C533 RID: 50483
				public static LocString TOOLTIP = "Receiving massages at a Massage Clinic will significantly improve Stress reduction";
			}

			// Token: 0x02002FA8 RID: 12200
			public class POWER_PLANT
			{
				// Token: 0x0400C534 RID: 50484
				public static LocString NAME = "Power Plant";

				// Token: 0x0400C535 RID: 50485
				public static LocString DESCRIPTION = "The perfect place for Duplicants to flex their Electrical Engineering skills.\n\nHeavy-duty generators built within a Power Plant can be tuned up using microchips from power control stations to improve their " + UI.FormatAsLink("Power", "POWER") + " power production.";

				// Token: 0x0400C536 RID: 50486
				public static LocString EFFECT = "- Enables " + ITEMS.INDUSTRIAL_PRODUCTS.POWER_STATION_TOOLS.NAME + " tune-ups on heavy-duty generators";

				// Token: 0x0400C537 RID: 50487
				public static LocString TOOLTIP = "Heavy-duty generators built in a Power Plant can be tuned up using microchips from Power Control Stations to improve their Power production";
			}

			// Token: 0x02002FA9 RID: 12201
			public class MACHINE_SHOP
			{
				// Token: 0x0400C538 RID: 50488
				public static LocString NAME = "Machine Shop";

				// Token: 0x0400C539 RID: 50489
				public static LocString DESCRIPTION = "It smells like elbow grease.\n\nDuplicants working in a Machine Shop can maintain buildings and increase their production speed.";

				// Token: 0x0400C53A RID: 50490
				public static LocString EFFECT = "- Increased fabrication efficiency";

				// Token: 0x0400C53B RID: 50491
				public static LocString TOOLTIP = "Duplicants working in a Machine Shop can maintain buildings and increase their production speed";
			}

			// Token: 0x02002FAA RID: 12202
			public class FARM
			{
				// Token: 0x0400C53C RID: 50492
				public static LocString NAME = "Greenhouse";

				// Token: 0x0400C53D RID: 50493
				public static LocString DESCRIPTION = "An enclosed agricultural space best utilized by Duplicants with Crop Tending skills.\n\nCrops grown within a Greenhouse can be tended with Farm Station fertilizer to increase their growth speed.";

				// Token: 0x0400C53E RID: 50494
				public static LocString EFFECT = "- Enables Farm Station use";

				// Token: 0x0400C53F RID: 50495
				public static LocString TOOLTIP = "Crops grown within a Greenhouse can be tended with Farm Station fertilizer to increase their growth speed";
			}

			// Token: 0x02002FAB RID: 12203
			public class CREATUREPEN
			{
				// Token: 0x0400C540 RID: 50496
				public static LocString NAME = "Stable";

				// Token: 0x0400C541 RID: 50497
				public static LocString DESCRIPTION = "Critters don't mind it here, as long as things don't get too crowded.\n\nStabled critters can be tended to in order to improve their happiness, hasten their domestication and increase their production.\n\nEnables the use of Grooming Stations, Shearing Stations, Critter Condos, Critter Fountains and Milking Stations.";

				// Token: 0x0400C542 RID: 50498
				public static LocString EFFECT = "- Critter taming and mood bonus";

				// Token: 0x0400C543 RID: 50499
				public static LocString TOOLTIP = "A stable enables Grooming Station, Critter Condo, Critter Fountain, Shearing Station and Milking Station use";
			}

			// Token: 0x02002FAC RID: 12204
			public class REC_ROOM
			{
				// Token: 0x0400C544 RID: 50500
				public static LocString NAME = "Recreation Room";

				// Token: 0x0400C545 RID: 50501
				public static LocString DESCRIPTION = "Where Duplicants go to mingle with off-duty peers and indulge in a little R&R.\n\nScheduled Downtime will further improve Morale for Duplicants visiting a Recreation Room.";

				// Token: 0x0400C546 RID: 50502
				public static LocString EFFECT = "- Morale bonus";

				// Token: 0x0400C547 RID: 50503
				public static LocString TOOLTIP = "Scheduled Downtime will further improve Morale for Duplicants visiting a Recreation Room";
			}

			// Token: 0x02002FAD RID: 12205
			public class PARK
			{
				// Token: 0x0400C548 RID: 50504
				public static LocString NAME = "Park";

				// Token: 0x0400C549 RID: 50505
				public static LocString DESCRIPTION = "A little greenery goes a long way.\n\nPassing through natural spaces throughout the day will raise the Morale of Duplicants.";

				// Token: 0x0400C54A RID: 50506
				public static LocString EFFECT = "- Morale bonus";

				// Token: 0x0400C54B RID: 50507
				public static LocString TOOLTIP = "Passing through natural spaces throughout the day will raise the Morale of Duplicants";
			}

			// Token: 0x02002FAE RID: 12206
			public class NATURERESERVE
			{
				// Token: 0x0400C54C RID: 50508
				public static LocString NAME = "Nature Reserve";

				// Token: 0x0400C54D RID: 50509
				public static LocString DESCRIPTION = "A lot of greenery goes an even longer way.\n\nPassing through a Nature Reserve will grant higher Morale bonuses to Duplicants than a Park.";

				// Token: 0x0400C54E RID: 50510
				public static LocString EFFECT = "- Morale bonus";

				// Token: 0x0400C54F RID: 50511
				public static LocString TOOLTIP = "A Nature Reserve will grant higher Morale bonuses to Duplicants than a Park";
			}

			// Token: 0x02002FAF RID: 12207
			public class LABORATORY
			{
				// Token: 0x0400C550 RID: 50512
				public static LocString NAME = "Laboratory";

				// Token: 0x0400C551 RID: 50513
				public static LocString DESCRIPTION = "Where wild hypotheses meet rigorous scientific experimentation.\n\nScience stations built in a Laboratory function more efficiently.\n\nA Laboratory enables the use of the Geotuner and the Mission Control Station.";

				// Token: 0x0400C552 RID: 50514
				public static LocString EFFECT = "- Efficiency bonus";

				// Token: 0x0400C553 RID: 50515
				public static LocString TOOLTIP = "Science buildings built in a Laboratory function more efficiently\n\nA Laboratory enables Geotuner and Mission Control Station use";
			}

			// Token: 0x02002FB0 RID: 12208
			public class PRIVATE_BATHROOM
			{
				// Token: 0x0400C554 RID: 50516
				public static LocString NAME = "Private Bathroom";

				// Token: 0x0400C555 RID: 50517
				public static LocString DESCRIPTION = "Finally, a place to truly be alone with one's thoughts.\n\nDuplicants relieve even more Stress when using the toilet in a Private Bathroom than in a Latrine.";

				// Token: 0x0400C556 RID: 50518
				public static LocString EFFECT = "- Stress relief bonus";

				// Token: 0x0400C557 RID: 50519
				public static LocString TOOLTIP = "Duplicants relieve even more stress when using the toilet in a Private Bathroom than in a Latrine";
			}

			// Token: 0x02002FB1 RID: 12209
			public class BIONIC_UPKEEP
			{
				// Token: 0x0400C558 RID: 50520
				public static LocString NAME = "Workshop";

				// Token: 0x0400C559 RID: 50521
				public static LocString DESCRIPTION = "A spa of sorts, for Duplicants who were built different.\n\nBionic Duplicants who access bionic service stations in a Workshop will get a nice little Morale boost.";

				// Token: 0x0400C55A RID: 50522
				public static LocString EFFECT = "- Morale bonus";

				// Token: 0x0400C55B RID: 50523
				public static LocString TOOLTIP = "Bionic Duplicants get a Morale boost when using bionic service stations in a Workshop";
			}
		}

		// Token: 0x02002FB2 RID: 12210
		public class CRITERIA
		{
			// Token: 0x0400C55C RID: 50524
			public static LocString HEADER = "<b>Requirements:</b>";

			// Token: 0x0400C55D RID: 50525
			public static LocString NEUTRAL_TYPE = "Enclosed by wall tile";

			// Token: 0x0400C55E RID: 50526
			public static LocString POSSIBLE_TYPES_HEADER = "Possible Room Types";

			// Token: 0x0400C55F RID: 50527
			public static LocString NO_TYPE_CONFLICTS = "Remove conflicting buildings";

			// Token: 0x0400C560 RID: 50528
			public static LocString IN_CODE_ERROR = "String Key Not Found: {0}";

			// Token: 0x02002FB3 RID: 12211
			public class CRITERIA_FAILED
			{
				// Token: 0x0400C561 RID: 50529
				public static LocString MISSING_BUILDING = "Missing {0}";

				// Token: 0x0400C562 RID: 50530
				public static LocString FAILED = "{0}";
			}

			// Token: 0x02002FB4 RID: 12212
			public static class DECORATION
			{
				// Token: 0x0400C563 RID: 50531
				public static LocString NAME = UI.FormatAsLink("Decor item", "BUILDCATEGORYREQUIREMENTCLASSDECORATION");

				// Token: 0x0400C564 RID: 50532
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.DECORATION.NAME;
			}

			// Token: 0x02002FB5 RID: 12213
			public class CEILING_HEIGHT
			{
				// Token: 0x0400C565 RID: 50533
				public static LocString NAME = "Minimum height: {0} tiles";

				// Token: 0x0400C566 RID: 50534
				public static LocString DESCRIPTION = "Must have a ceiling height of at least {0} tiles";

				// Token: 0x0400C567 RID: 50535
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.CEILING_HEIGHT.NAME;
			}

			// Token: 0x02002FB6 RID: 12214
			public class MINIMUM_SIZE
			{
				// Token: 0x0400C568 RID: 50536
				public static LocString NAME = "Minimum size: {0} tiles";

				// Token: 0x0400C569 RID: 50537
				public static LocString DESCRIPTION = "Must have an area of at least {0} tiles";

				// Token: 0x0400C56A RID: 50538
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.MINIMUM_SIZE.NAME;
			}

			// Token: 0x02002FB7 RID: 12215
			public class MAXIMUM_SIZE
			{
				// Token: 0x0400C56B RID: 50539
				public static LocString NAME = "Maximum size: {0} tiles";

				// Token: 0x0400C56C RID: 50540
				public static LocString DESCRIPTION = "Must have an area no larger than {0} tiles";

				// Token: 0x0400C56D RID: 50541
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.MAXIMUM_SIZE.NAME;
			}

			// Token: 0x02002FB8 RID: 12216
			public class INDUSTRIALMACHINERY
			{
				// Token: 0x0400C56E RID: 50542
				public static LocString NAME = UI.FormatAsLink("Industrial machinery", "BUILDCATEGORYREQUIREMENTCLASSINDUSTRIALMACHINERY");

				// Token: 0x0400C56F RID: 50543
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.INDUSTRIALMACHINERY.NAME;
			}

			// Token: 0x02002FB9 RID: 12217
			public class HAS_BED
			{
				// Token: 0x0400C570 RID: 50544
				public static LocString NAME = "One or more " + UI.FormatAsLink("beds", "BUILDCATEGORYREQUIREMENTCLASSBEDTYPE");

				// Token: 0x0400C571 RID: 50545
				public static LocString DESCRIPTION = "Requires at least one Cot or Comfy Bed";

				// Token: 0x0400C572 RID: 50546
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.HAS_BED.NAME;
			}

			// Token: 0x02002FBA RID: 12218
			public class HAS_LUXURY_BED
			{
				// Token: 0x0400C573 RID: 50547
				public static LocString NAME = "One or more " + UI.FormatAsLink("Comfy Beds", "LUXURYBED");

				// Token: 0x0400C574 RID: 50548
				public static LocString DESCRIPTION = "Requires at least one Comfy Bed";

				// Token: 0x0400C575 RID: 50549
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.HAS_LUXURY_BED.NAME;
			}

			// Token: 0x02002FBB RID: 12219
			public class LUXURYBEDTYPE
			{
				// Token: 0x0400C576 RID: 50550
				public static LocString NAME = "Single " + UI.FormatAsLink("Comfy Bed", "LUXURYBED");

				// Token: 0x0400C577 RID: 50551
				public static LocString DESCRIPTION = "Must have no more than one Comfy Bed";

				// Token: 0x0400C578 RID: 50552
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.LUXURYBEDTYPE.NAME;
			}

			// Token: 0x02002FBC RID: 12220
			public class BED_SINGLE
			{
				// Token: 0x0400C579 RID: 50553
				public static LocString NAME = "Single " + UI.FormatAsLink("beds", "BUILDCATEGORYREQUIREMENTCLASSBEDTYPE");

				// Token: 0x0400C57A RID: 50554
				public static LocString DESCRIPTION = "Must have no more than one Cot or Comfy Bed";

				// Token: 0x0400C57B RID: 50555
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.BED_SINGLE.NAME;
			}

			// Token: 0x02002FBD RID: 12221
			public class IS_BACKWALLED
			{
				// Token: 0x0400C57C RID: 50556
				public static LocString NAME = "Has backwall tiles";

				// Token: 0x0400C57D RID: 50557
				public static LocString DESCRIPTION = "Must be covered in backwall tiles";

				// Token: 0x0400C57E RID: 50558
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.IS_BACKWALLED.NAME;
			}

			// Token: 0x02002FBE RID: 12222
			public class NO_COTS
			{
				// Token: 0x0400C57F RID: 50559
				public static LocString NAME = "No " + UI.FormatAsLink("Cots", "BED");

				// Token: 0x0400C580 RID: 50560
				public static LocString DESCRIPTION = "Room cannot contain a Cot";

				// Token: 0x0400C581 RID: 50561
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.NO_COTS.NAME;
			}

			// Token: 0x02002FBF RID: 12223
			public class NO_LUXURY_BEDS
			{
				// Token: 0x0400C582 RID: 50562
				public static LocString NAME = "No " + UI.FormatAsLink("Comfy Beds", "LUXURYBED");

				// Token: 0x0400C583 RID: 50563
				public static LocString DESCRIPTION = "Room cannot contain a Comfy Bed";

				// Token: 0x0400C584 RID: 50564
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.NO_LUXURY_BEDS.NAME;
			}

			// Token: 0x02002FC0 RID: 12224
			public class BEDTYPE
			{
				// Token: 0x0400C585 RID: 50565
				public static LocString NAME = UI.FormatAsLink("Beds", "BUILDCATEGORYREQUIREMENTCLASSBEDTYPE");

				// Token: 0x0400C586 RID: 50566
				public static LocString DESCRIPTION = "Requires two or more Cots or Comfy Beds";

				// Token: 0x0400C587 RID: 50567
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.BEDTYPE.NAME;
			}

			// Token: 0x02002FC1 RID: 12225
			public class BUILDING_DECOR_POSITIVE
			{
				// Token: 0x0400C588 RID: 50568
				public static LocString NAME = "Positive " + UI.FormatAsLink("decor", "BUILDCATEGORYREQUIREMENTCLASSDECORATION");

				// Token: 0x0400C589 RID: 50569
				public static LocString DESCRIPTION = "Requires at least one building with positive decor";

				// Token: 0x0400C58A RID: 50570
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.BUILDING_DECOR_POSITIVE.NAME;
			}

			// Token: 0x02002FC2 RID: 12226
			public class DECORATIVE_ITEM
			{
				// Token: 0x0400C58B RID: 50571
				public static LocString NAME = UI.FormatAsLink("Decor item", "BUILDCATEGORYREQUIREMENTCLASSDECORATION") + " ({0})";

				// Token: 0x0400C58C RID: 50572
				public static LocString DESCRIPTION = "Requires {0} or more Decor items";

				// Token: 0x0400C58D RID: 50573
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.DECORATIVE_ITEM.NAME;
			}

			// Token: 0x02002FC3 RID: 12227
			public class DECOR20
			{
				// Token: 0x0600D235 RID: 53813 RVA: 0x00498190 File Offset: 0x00496390
				// Note: this type is marked as 'beforefieldinit'.
				static DECOR20()
				{
					string str = "Requires a decorative item with a minimum Decor value of ";
					int amount = BUILDINGS.DECOR.BONUS.TIER3.amount;
					ROOMS.CRITERIA.DECOR20.DESCRIPTION = str + amount.ToString();
					ROOMS.CRITERIA.DECOR20.CONFLICT_DESCRIPTION = ROOMS.CRITERIA.DECOR20.NAME;
				}

				// Token: 0x0400C58E RID: 50574
				public static LocString NAME = UI.FormatAsLink("Fancy decor item", "BUILDCATEGORYREQUIREMENTCLASSDECORATION");

				// Token: 0x0400C58F RID: 50575
				public static LocString DESCRIPTION;

				// Token: 0x0400C590 RID: 50576
				public static LocString CONFLICT_DESCRIPTION;
			}

			// Token: 0x02002FC4 RID: 12228
			public class CLINIC
			{
				// Token: 0x0400C591 RID: 50577
				public static LocString NAME = UI.FormatAsLink("Medical equipment", "BUILDCATEGORYREQUIREMENTCLASSCLINIC");

				// Token: 0x0400C592 RID: 50578
				public static LocString DESCRIPTION = "Requires one or more Sick Bays or Disease Clinics";

				// Token: 0x0400C593 RID: 50579
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.CLINIC.NAME;
			}

			// Token: 0x02002FC5 RID: 12229
			public class POWERPLANT
			{
				// Token: 0x0400C594 RID: 50580
				public static LocString NAME = UI.FormatAsLink("Heavy-Duty Generator", "BUILDCATEGORYREQUIREMENTCLASSGENERATORTYPE") + "\n    • Two or more " + UI.FormatAsLink("Power Buildings", "BUILDCATEGORYREQUIREMENTCLASSPOWERBUILDING");

				// Token: 0x0400C595 RID: 50581
				public static LocString DESCRIPTION = "Requires a Heavy-Duty Generator and two or more Power Buildings";

				// Token: 0x0400C596 RID: 50582
				public static LocString CONFLICT_DESCRIPTION = "Heavy-Duty Generator and two or more Power buildings";
			}

			// Token: 0x02002FC6 RID: 12230
			public class FARMSTATIONTYPE
			{
				// Token: 0x0400C597 RID: 50583
				public static LocString NAME = UI.FormatAsLink("Farm Station", "FARMSTATION");

				// Token: 0x0400C598 RID: 50584
				public static LocString DESCRIPTION = "Requires a single Farm Station";

				// Token: 0x0400C599 RID: 50585
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.FARMSTATIONTYPE.NAME;
			}

			// Token: 0x02002FC7 RID: 12231
			public class CREATURERELOCATOR
			{
				// Token: 0x0400C59A RID: 50586
				public static LocString NAME = UI.FormatAsLink("Critter relocator", "BUILDCATEGORYREQUIREMENTCLASSCREATURERELOCATOR");

				// Token: 0x0400C59B RID: 50587
				public static LocString DESCRIPTION = "Requires a single Critter Drop-Off or Fish Release";

				// Token: 0x0400C59C RID: 50588
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.CREATURERELOCATOR.NAME;
			}

			// Token: 0x02002FC8 RID: 12232
			public class CREATURE_FEEDER
			{
				// Token: 0x0400C59D RID: 50589
				public static LocString NAME = UI.FormatAsLink("Critter Feeder", "CREATUREFEEDER");

				// Token: 0x0400C59E RID: 50590
				public static LocString DESCRIPTION = "Requires a single Critter Feeder";

				// Token: 0x0400C59F RID: 50591
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.CREATURE_FEEDER.NAME;
			}

			// Token: 0x02002FC9 RID: 12233
			public class RANCHSTATIONTYPE
			{
				// Token: 0x0400C5A0 RID: 50592
				public static LocString NAME = UI.FormatAsLink("Ranching building", "BUILDCATEGORYREQUIREMENTCLASSRANCHSTATIONTYPE");

				// Token: 0x0400C5A1 RID: 50593
				public static LocString DESCRIPTION = "Requires a single Grooming Station, Critter Condo, Critter Fountain, Shearing Station or Milking Station";

				// Token: 0x0400C5A2 RID: 50594
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.RANCHSTATIONTYPE.NAME;
			}

			// Token: 0x02002FCA RID: 12234
			public class SPICESTATION
			{
				// Token: 0x0400C5A3 RID: 50595
				public static LocString NAME = UI.FormatAsLink("Spice Grinder", "SPICEGRINDER");

				// Token: 0x0400C5A4 RID: 50596
				public static LocString DESCRIPTION = "Requires a single Spice Grinder";

				// Token: 0x0400C5A5 RID: 50597
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.SPICESTATION.NAME;
			}

			// Token: 0x02002FCB RID: 12235
			public class COOKTOP
			{
				// Token: 0x0400C5A6 RID: 50598
				public static LocString NAME = UI.FormatAsLink("Cooking station", "BUILDCATEGORYREQUIREMENTCLASSCOOKTOP");

				// Token: 0x0400C5A7 RID: 50599
				public static LocString DESCRIPTION = "Requires a single Electric Grill or Gas Range";

				// Token: 0x0400C5A8 RID: 50600
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.COOKTOP.NAME;
			}

			// Token: 0x02002FCC RID: 12236
			public class REFRIGERATOR
			{
				// Token: 0x0400C5A9 RID: 50601
				public static LocString NAME = UI.FormatAsLink("Refrigerator", "REFRIGERATOR");

				// Token: 0x0400C5AA RID: 50602
				public static LocString DESCRIPTION = "Requires a single Refrigerator";

				// Token: 0x0400C5AB RID: 50603
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.REFRIGERATOR.NAME;
			}

			// Token: 0x02002FCD RID: 12237
			public class RECBUILDING
			{
				// Token: 0x0400C5AC RID: 50604
				public static LocString NAME = UI.FormatAsLink("Recreational building", "BUILDCATEGORYREQUIREMENTCLASSRECBUILDING");

				// Token: 0x0400C5AD RID: 50605
				public static LocString DESCRIPTION = "Requires one or more recreational buildings";

				// Token: 0x0400C5AE RID: 50606
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.RECBUILDING.NAME;
			}

			// Token: 0x02002FCE RID: 12238
			public class PARK
			{
				// Token: 0x0400C5AF RID: 50607
				public static LocString NAME = UI.FormatAsLink("Park Sign", "PARKSIGN");

				// Token: 0x0400C5B0 RID: 50608
				public static LocString DESCRIPTION = "Requires one or more Park Signs";

				// Token: 0x0400C5B1 RID: 50609
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.PARK.NAME;
			}

			// Token: 0x02002FCF RID: 12239
			public class MACHINESHOPTYPE
			{
				// Token: 0x0400C5B2 RID: 50610
				public static LocString NAME = "Mechanics Station";

				// Token: 0x0400C5B3 RID: 50611
				public static LocString DESCRIPTION = "Requires requires one or more Mechanics Stations";

				// Token: 0x0400C5B4 RID: 50612
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.MACHINESHOPTYPE.NAME;
			}

			// Token: 0x02002FD0 RID: 12240
			public class FOOD_BOX
			{
				// Token: 0x0400C5B5 RID: 50613
				public static LocString NAME = "Food storage";

				// Token: 0x0400C5B6 RID: 50614
				public static LocString DESCRIPTION = "Requires one or more Ration Boxes or Refrigerators";

				// Token: 0x0400C5B7 RID: 50615
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.FOOD_BOX.NAME;
			}

			// Token: 0x02002FD1 RID: 12241
			public class LIGHTSOURCE
			{
				// Token: 0x0400C5B8 RID: 50616
				public static LocString NAME = UI.FormatAsLink("Light source", "BUILDCATEGORYREQUIREMENTCLASSLIGHTSOURCE");

				// Token: 0x0400C5B9 RID: 50617
				public static LocString DESCRIPTION = "Requires one or more light sources";

				// Token: 0x0400C5BA RID: 50618
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.LIGHTSOURCE.NAME;
			}

			// Token: 0x02002FD2 RID: 12242
			public class DESTRESSINGBUILDING
			{
				// Token: 0x0400C5BB RID: 50619
				public static LocString NAME = UI.FormatAsLink("De-Stressing Building", "MASSAGETABLE");

				// Token: 0x0400C5BC RID: 50620
				public static LocString DESCRIPTION = "Requires one or more De-Stressing buildings";

				// Token: 0x0400C5BD RID: 50621
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.DESTRESSINGBUILDING.NAME;
			}

			// Token: 0x02002FD3 RID: 12243
			public class MASSAGE_TABLE
			{
				// Token: 0x0400C5BE RID: 50622
				public static LocString NAME = UI.FormatAsLink("Massage Table", "MASSAGETABLE");

				// Token: 0x0400C5BF RID: 50623
				public static LocString DESCRIPTION = "Requires one or more Massage Tables";

				// Token: 0x0400C5C0 RID: 50624
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.MASSAGE_TABLE.NAME;
			}

			// Token: 0x02002FD4 RID: 12244
			public class MESSTABLE
			{
				// Token: 0x0400C5C1 RID: 50625
				public static LocString NAME = UI.FormatAsLink("Mess Table", "DININGTABLE");

				// Token: 0x0400C5C2 RID: 50626
				public static LocString DESCRIPTION = "Requires a single Mess Table";

				// Token: 0x0400C5C3 RID: 50627
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.MESSTABLE.NAME;
			}

			// Token: 0x02002FD5 RID: 12245
			public class NO_MESS_STATION
			{
				// Token: 0x0400C5C4 RID: 50628
				public static LocString NAME = "No " + UI.FormatAsLink("Mess Table", "DININGTABLE");

				// Token: 0x0400C5C5 RID: 50629
				public static LocString DESCRIPTION = "Cannot contain a Mess Table";

				// Token: 0x0400C5C6 RID: 50630
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.NO_MESS_STATION.NAME;
			}

			// Token: 0x02002FD6 RID: 12246
			public class MESS_STATION_MULTIPLE
			{
				// Token: 0x0400C5C7 RID: 50631
				public static LocString NAME = UI.FormatAsLink("Mess Tables", "DININGTABLE");

				// Token: 0x0400C5C8 RID: 50632
				public static LocString DESCRIPTION = "Requires two or more Mess Tables";

				// Token: 0x0400C5C9 RID: 50633
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.MESS_STATION_MULTIPLE.NAME;
			}

			// Token: 0x02002FD7 RID: 12247
			public class RESEARCH_STATION
			{
				// Token: 0x0400C5CA RID: 50634
				public static LocString NAME = UI.FormatAsLink("Research station", "BUILDCATEGORYREQUIREMENTCLASSRESEARCH_STATION");

				// Token: 0x0400C5CB RID: 50635
				public static LocString DESCRIPTION = "Requires one or more Research Stations or Super Computers";

				// Token: 0x0400C5CC RID: 50636
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.RESEARCH_STATION.NAME;
			}

			// Token: 0x02002FD8 RID: 12248
			public class BIONICUPKEEP
			{
				// Token: 0x0400C5CD RID: 50637
				public static LocString NAME = UI.FormatAsLink("Bionic service station", "BUILDCATEGORYREQUIREMENTCLASSBIONICUPKEEP");

				// Token: 0x0400C5CE RID: 50638
				public static LocString DESCRIPTION = "Requires at least one Lubrication Station and one Gunk Extractor";

				// Token: 0x0400C5CF RID: 50639
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.BIONICUPKEEP.NAME;
			}

			// Token: 0x02002FD9 RID: 12249
			public class BIONIC_GUNKEMPTIER
			{
				// Token: 0x0400C5D0 RID: 50640
				public static LocString NAME = UI.FormatAsLink("Gunk Extractor", "BUILDCATEGORYREQUIREMENTCLASSBIONIC_GUNKEMPTIER");

				// Token: 0x0400C5D1 RID: 50641
				public static LocString DESCRIPTION = "Requires one or more Gunk Extractors";

				// Token: 0x0400C5D2 RID: 50642
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.BIONIC_GUNKEMPTIER.NAME;
			}

			// Token: 0x02002FDA RID: 12250
			public class BIONIC_LUBRICATION
			{
				// Token: 0x0400C5D3 RID: 50643
				public static LocString NAME = UI.FormatAsLink("Lubrication Station", "BUILDCATEGORYREQUIREMENTCLASSBIONIC_LUBRICATION");

				// Token: 0x0400C5D4 RID: 50644
				public static LocString DESCRIPTION = "Requires one or more Lubrication Stations";

				// Token: 0x0400C5D5 RID: 50645
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.BIONIC_LUBRICATION.NAME;
			}

			// Token: 0x02002FDB RID: 12251
			public class TOILETTYPE
			{
				// Token: 0x0400C5D6 RID: 50646
				public static LocString NAME = UI.FormatAsLink("Toilet", "BUILDCATEGORYREQUIREMENTCLASSTOILETTYPE");

				// Token: 0x0400C5D7 RID: 50647
				public static LocString DESCRIPTION = "Requires one or more Outhouses or Lavatories";

				// Token: 0x0400C5D8 RID: 50648
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.TOILETTYPE.NAME;
			}

			// Token: 0x02002FDC RID: 12252
			public class FLUSHTOILETTYPE
			{
				// Token: 0x0400C5D9 RID: 50649
				public static LocString NAME = UI.FormatAsLink("Flush Toilet", "BUILDCATEGORYREQUIREMENTCLASSFLUSHTOILETTYPE");

				// Token: 0x0400C5DA RID: 50650
				public static LocString DESCRIPTION = "Requires one or more Lavatories";

				// Token: 0x0400C5DB RID: 50651
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.FLUSHTOILETTYPE.NAME;
			}

			// Token: 0x02002FDD RID: 12253
			public class NO_OUTHOUSES
			{
				// Token: 0x0400C5DC RID: 50652
				public static LocString NAME = "No " + UI.FormatAsLink("Outhouses", "OUTHOUSE");

				// Token: 0x0400C5DD RID: 50653
				public static LocString DESCRIPTION = "Cannot contain basic Outhouses";

				// Token: 0x0400C5DE RID: 50654
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.NO_OUTHOUSES.NAME;
			}

			// Token: 0x02002FDE RID: 12254
			public class WASHSTATION
			{
				// Token: 0x0400C5DF RID: 50655
				public static LocString NAME = UI.FormatAsLink("Wash station", "BUILDCATEGORYREQUIREMENTCLASSWASHSTATION");

				// Token: 0x0400C5E0 RID: 50656
				public static LocString DESCRIPTION = "Requires one or more Wash Basins, Sinks, Hand Sanitizers, or Showers";

				// Token: 0x0400C5E1 RID: 50657
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.WASHSTATION.NAME;
			}

			// Token: 0x02002FDF RID: 12255
			public class ADVANCEDWASHSTATION
			{
				// Token: 0x0400C5E2 RID: 50658
				public static LocString NAME = UI.FormatAsLink("Plumbed wash station", "BUILDCATEGORYREQUIREMENTCLASSWASHSTATION");

				// Token: 0x0400C5E3 RID: 50659
				public static LocString DESCRIPTION = "Requires one or more Sinks, Hand Sanitizers, or Showers";

				// Token: 0x0400C5E4 RID: 50660
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.ADVANCEDWASHSTATION.NAME;
			}

			// Token: 0x02002FE0 RID: 12256
			public class NO_INDUSTRIAL_MACHINERY
			{
				// Token: 0x0400C5E5 RID: 50661
				public static LocString NAME = "No " + UI.FormatAsLink("industrial machinery", "BUILDCATEGORYREQUIREMENTCLASSINDUSTRIALMACHINERY");

				// Token: 0x0400C5E6 RID: 50662
				public static LocString DESCRIPTION = "Cannot contain any building labeled Industrial Machinery";

				// Token: 0x0400C5E7 RID: 50663
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.NO_INDUSTRIAL_MACHINERY.NAME;
			}

			// Token: 0x02002FE1 RID: 12257
			public class WILDANIMAL
			{
				// Token: 0x0400C5E8 RID: 50664
				public static LocString NAME = "Wildlife";

				// Token: 0x0400C5E9 RID: 50665
				public static LocString DESCRIPTION = "Requires at least one wild critter";

				// Token: 0x0400C5EA RID: 50666
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.WILDANIMAL.NAME;
			}

			// Token: 0x02002FE2 RID: 12258
			public class WILDANIMALS
			{
				// Token: 0x0400C5EB RID: 50667
				public static LocString NAME = "More wildlife";

				// Token: 0x0400C5EC RID: 50668
				public static LocString DESCRIPTION = "Requires two or more wild critters";

				// Token: 0x0400C5ED RID: 50669
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.WILDANIMALS.NAME;
			}

			// Token: 0x02002FE3 RID: 12259
			public class WILDPLANT
			{
				// Token: 0x0400C5EE RID: 50670
				public static LocString NAME = "Two wild plants";

				// Token: 0x0400C5EF RID: 50671
				public static LocString DESCRIPTION = "Requires two or more wild plants";

				// Token: 0x0400C5F0 RID: 50672
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.WILDPLANT.NAME;
			}

			// Token: 0x02002FE4 RID: 12260
			public class WILDPLANTS
			{
				// Token: 0x0400C5F1 RID: 50673
				public static LocString NAME = "Four wild plants";

				// Token: 0x0400C5F2 RID: 50674
				public static LocString DESCRIPTION = "Requires four or more wild plants";

				// Token: 0x0400C5F3 RID: 50675
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.WILDPLANTS.NAME;
			}

			// Token: 0x02002FE5 RID: 12261
			public class SCIENCEBUILDING
			{
				// Token: 0x0400C5F4 RID: 50676
				public static LocString NAME = UI.FormatAsLink("Science building", "BUILDCATEGORYREQUIREMENTCLASSSCIENCEBUILDING");

				// Token: 0x0400C5F5 RID: 50677
				public static LocString DESCRIPTION = "Requires one or more science buildings";

				// Token: 0x0400C5F6 RID: 50678
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.SCIENCEBUILDING.NAME;
			}

			// Token: 0x02002FE6 RID: 12262
			public class SCIENCE_BUILDINGS
			{
				// Token: 0x0400C5F7 RID: 50679
				public static LocString NAME = "Two " + UI.FormatAsLink("science buildings", "BUILDCATEGORYREQUIREMENTCLASSSCIENCEBUILDING");

				// Token: 0x0400C5F8 RID: 50680
				public static LocString DESCRIPTION = "Requires two or more science buildings";

				// Token: 0x0400C5F9 RID: 50681
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.SCIENCE_BUILDINGS.NAME;
			}

			// Token: 0x02002FE7 RID: 12263
			public class ROCKETINTERIOR
			{
				// Token: 0x0400C5FA RID: 50682
				public static LocString NAME = UI.FormatAsLink("Rocket interior", "BUILDCATEGORYREQUIREMENTCLASSROCKETINTERIOR");

				// Token: 0x0400C5FB RID: 50683
				public static LocString DESCRIPTION = "Must be built inside a rocket";

				// Token: 0x0400C5FC RID: 50684
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.ROCKETINTERIOR.NAME;
			}

			// Token: 0x02002FE8 RID: 12264
			public class WARMINGSTATION
			{
				// Token: 0x0400C5FD RID: 50685
				public static LocString NAME = UI.FormatAsLink("Warming station", "BUILDCATEGORYREQUIREMENTCLASSWARMINGSTATION");

				// Token: 0x0400C5FE RID: 50686
				public static LocString DESCRIPTION = "Raises the ambient temperature";

				// Token: 0x0400C5FF RID: 50687
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.WARMINGSTATION.NAME;
			}

			// Token: 0x02002FE9 RID: 12265
			public class GENERATORTYPE
			{
				// Token: 0x0400C600 RID: 50688
				public static LocString NAME = UI.FormatAsLink("Generator", "BUILDCATEGORYREQUIREMENTCLASSGENERATORTYPE");

				// Token: 0x0400C601 RID: 50689
				public static LocString DESCRIPTION = "Generates electrical power";

				// Token: 0x0400C602 RID: 50690
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.GENERATORTYPE.NAME;
			}

			// Token: 0x02002FEA RID: 12266
			public class HEAVYDUTYGENERATORTYPE
			{
				// Token: 0x0400C603 RID: 50691
				public static LocString NAME = UI.FormatAsLink("Heavy-duty generator", "BUILDCATEGORYREQUIREMENTCLASSGENERATORTYPE");

				// Token: 0x0400C604 RID: 50692
				public static LocString DESCRIPTION = "For big power needs";

				// Token: 0x0400C605 RID: 50693
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.HEAVYDUTYGENERATORTYPE.NAME;
			}

			// Token: 0x02002FEB RID: 12267
			public class LIGHTDUTYGENERATORTYPE
			{
				// Token: 0x0400C606 RID: 50694
				public static LocString NAME = UI.FormatAsLink("Basic generator", "BUILDCATEGORYREQUIREMENTCLASSGENERATORTYPE");

				// Token: 0x0400C607 RID: 50695
				public static LocString DESCRIPTION = "For regular power needs";

				// Token: 0x0400C608 RID: 50696
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.LIGHTDUTYGENERATORTYPE.NAME;
			}

			// Token: 0x02002FEC RID: 12268
			public class POWERBUILDING
			{
				// Token: 0x0400C609 RID: 50697
				public static LocString NAME = UI.FormatAsLink("Power building", "BUILDCATEGORYREQUIREMENTCLASSPOWERBUILDING");

				// Token: 0x0400C60A RID: 50698
				public static LocString DESCRIPTION = "Power buildings";

				// Token: 0x0400C60B RID: 50699
				public static LocString CONFLICT_DESCRIPTION = ROOMS.CRITERIA.POWERBUILDING.NAME;
			}
		}

		// Token: 0x02002FED RID: 12269
		public class DETAILS
		{
			// Token: 0x0400C60C RID: 50700
			public static LocString HEADER = "Room Details";

			// Token: 0x02002FEE RID: 12270
			public class ASSIGNED_TO
			{
				// Token: 0x0400C60D RID: 50701
				public static LocString NAME = "<b>Assignments:</b>\n{0}";

				// Token: 0x0400C60E RID: 50702
				public static LocString UNASSIGNED = "Unassigned";
			}

			// Token: 0x02002FEF RID: 12271
			public class AVERAGE_TEMPERATURE
			{
				// Token: 0x0400C60F RID: 50703
				public static LocString NAME = "Average temperature: {0}";
			}

			// Token: 0x02002FF0 RID: 12272
			public class AVERAGE_ATMO_MASS
			{
				// Token: 0x0400C610 RID: 50704
				public static LocString NAME = "Average air pressure: {0}";
			}

			// Token: 0x02002FF1 RID: 12273
			public class SIZE
			{
				// Token: 0x0400C611 RID: 50705
				public static LocString NAME = "Room size: {0} Tiles";
			}

			// Token: 0x02002FF2 RID: 12274
			public class BUILDING_COUNT
			{
				// Token: 0x0400C612 RID: 50706
				public static LocString NAME = "Buildings: {0}";
			}

			// Token: 0x02002FF3 RID: 12275
			public class CREATURE_COUNT
			{
				// Token: 0x0400C613 RID: 50707
				public static LocString NAME = "Critters: {0}";
			}

			// Token: 0x02002FF4 RID: 12276
			public class PLANT_COUNT
			{
				// Token: 0x0400C614 RID: 50708
				public static LocString NAME = "Plants: {0}";
			}
		}

		// Token: 0x02002FF5 RID: 12277
		public class EFFECTS
		{
			// Token: 0x0400C615 RID: 50709
			public static LocString HEADER = "<b>Effects:</b>";
		}
	}
}
