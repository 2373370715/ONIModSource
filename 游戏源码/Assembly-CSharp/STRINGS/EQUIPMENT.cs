using System;

namespace STRINGS
{
	// Token: 0x02002879 RID: 10361
	public class EQUIPMENT
	{
		// Token: 0x0200287A RID: 10362
		public class PREFABS
		{
			// Token: 0x0200287B RID: 10363
			public class OXYGEN_MASK
			{
				// Token: 0x0400AB9B RID: 43931
				public static LocString NAME = UI.FormatAsLink("Oxygen Mask", "OXYGEN_MASK");

				// Token: 0x0400AB9C RID: 43932
				public static LocString DESC = "Ensures my Duplicants can breathe easy... for a little while, anyways.";

				// Token: 0x0400AB9D RID: 43933
				public static LocString EFFECT = "Supplies Duplicants with <style=\"oxygen\">Oxygen</style> in toxic and low breathability environments.\n\nMust be refilled with oxygen at an " + UI.FormatAsLink("Oxygen Mask Dock", "OXYGENMASKLOCKER") + " when depleted.";

				// Token: 0x0400AB9E RID: 43934
				public static LocString RECIPE_DESC = "Supplies Duplicants with <style=\"oxygen\">Oxygen</style> in toxic and low breathability environments.";

				// Token: 0x0400AB9F RID: 43935
				public static LocString GENERICNAME = "Suit";

				// Token: 0x0400ABA0 RID: 43936
				public static LocString WORN_NAME = UI.FormatAsLink("Worn Oxygen Mask", "OXYGEN_MASK");

				// Token: 0x0400ABA1 RID: 43937
				public static LocString WORN_DESC = string.Concat(new string[]
				{
					"A worn out ",
					UI.FormatAsLink("Oxygen Mask", "OXYGEN_MASK"),
					".\n\nMasks can be repaired at a ",
					UI.FormatAsLink("Crafting Station", "CRAFTINGTABLE"),
					"."
				});
			}

			// Token: 0x0200287C RID: 10364
			public class ATMO_SUIT
			{
				// Token: 0x0400ABA2 RID: 43938
				public static LocString NAME = UI.FormatAsLink("Atmo Suit", "ATMO_SUIT");

				// Token: 0x0400ABA3 RID: 43939
				public static LocString DESC = "Ensures my Duplicants can breathe easy, anytime, anywhere.";

				// Token: 0x0400ABA4 RID: 43940
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Supplies Duplicants with ",
					UI.FormatAsLink("Oxygen", "OXYGEN"),
					" in toxic and low breathability environments, and protects against extreme temperatures.\n\nMust be refilled with oxygen at an ",
					UI.FormatAsLink("Atmo Suit Dock", "SUITLOCKER"),
					" when depleted."
				});

				// Token: 0x0400ABA5 RID: 43941
				public static LocString RECIPE_DESC = "Supplies Duplicants with " + UI.FormatAsLink("Oxygen", "OXYGEN") + "  in toxic and low breathability environments.";

				// Token: 0x0400ABA6 RID: 43942
				public static LocString GENERICNAME = "Suit";

				// Token: 0x0400ABA7 RID: 43943
				public static LocString WORN_NAME = UI.FormatAsLink("Worn Atmo Suit", "ATMO_SUIT");

				// Token: 0x0400ABA8 RID: 43944
				public static LocString WORN_DESC = string.Concat(new string[]
				{
					"A worn out ",
					UI.FormatAsLink("Atmo Suit", "ATMO_SUIT"),
					".\n\nSuits can be repaired at an ",
					UI.FormatAsLink("Exosuit Forge", "SUITFABRICATOR"),
					"."
				});

				// Token: 0x0400ABA9 RID: 43945
				public static LocString REPAIR_WORN_RECIPE_NAME = "Repair " + EQUIPMENT.PREFABS.ATMO_SUIT.NAME;

				// Token: 0x0400ABAA RID: 43946
				public static LocString REPAIR_WORN_DESC = "Restore a " + UI.FormatAsLink("Worn Atmo Suit", "ATMO_SUIT") + " to working order.";
			}

			// Token: 0x0200287D RID: 10365
			public class ATMO_SUIT_SET
			{
				// Token: 0x0200287E RID: 10366
				public class PUFT
				{
					// Token: 0x0400ABAB RID: 43947
					public static LocString NAME = "Puft Atmo Suit";

					// Token: 0x0400ABAC RID: 43948
					public static LocString DESC = "Critter-forward protective gear for the intrepid explorer!\nReleased for Klei Fest 2023.";
				}
			}

			// Token: 0x0200287F RID: 10367
			public class HOLIDAY_2023_CRATE
			{
				// Token: 0x0400ABAD RID: 43949
				public static LocString NAME = "Holiday Gift Crate";

				// Token: 0x0400ABAE RID: 43950
				public static LocString DESC = "An unaddressed package has been discovered near the Printing Pod. It exudes seasonal cheer, and trace amounts of Neutronium have been detected.";
			}

			// Token: 0x02002880 RID: 10368
			public class ATMO_SUIT_HELMET
			{
				// Token: 0x0400ABAF RID: 43951
				public static LocString NAME = "Default Atmo Helmet";

				// Token: 0x0400ABB0 RID: 43952
				public static LocString DESC = "Default helmet for atmo suits.";

				// Token: 0x02002881 RID: 10369
				public class FACADES
				{
					// Token: 0x02002882 RID: 10370
					public class SPARKLE_RED
					{
						// Token: 0x0400ABB1 RID: 43953
						public static LocString NAME = "Red Glitter Atmo Helmet";

						// Token: 0x0400ABB2 RID: 43954
						public static LocString DESC = "Protective gear at its sparkliest.";
					}

					// Token: 0x02002883 RID: 10371
					public class SPARKLE_GREEN
					{
						// Token: 0x0400ABB3 RID: 43955
						public static LocString NAME = "Green Glitter Atmo Helmet";

						// Token: 0x0400ABB4 RID: 43956
						public static LocString DESC = "Protective gear at its sparkliest.";
					}

					// Token: 0x02002884 RID: 10372
					public class SPARKLE_BLUE
					{
						// Token: 0x0400ABB5 RID: 43957
						public static LocString NAME = "Blue Glitter Atmo Helmet";

						// Token: 0x0400ABB6 RID: 43958
						public static LocString DESC = "Protective gear at its sparkliest.";
					}

					// Token: 0x02002885 RID: 10373
					public class SPARKLE_PURPLE
					{
						// Token: 0x0400ABB7 RID: 43959
						public static LocString NAME = "Violet Glitter Atmo Helmet";

						// Token: 0x0400ABB8 RID: 43960
						public static LocString DESC = "Protective gear at its sparkliest.";
					}

					// Token: 0x02002886 RID: 10374
					public class LIMONE
					{
						// Token: 0x0400ABB9 RID: 43961
						public static LocString NAME = "Citrus Atmo Helmet";

						// Token: 0x0400ABBA RID: 43962
						public static LocString DESC = "Fresh, fruity and full of breathable air.";
					}

					// Token: 0x02002887 RID: 10375
					public class PUFT
					{
						// Token: 0x0400ABBB RID: 43963
						public static LocString NAME = "Puft Atmo Helmet";

						// Token: 0x0400ABBC RID: 43964
						public static LocString DESC = "Convincing enough to fool most Pufts and even a few Duplicants.\nReleased for Klei Fest 2023.";
					}

					// Token: 0x02002888 RID: 10376
					public class CLUBSHIRT_PURPLE
					{
						// Token: 0x0400ABBD RID: 43965
						public static LocString NAME = "Eggplant Atmo Helmet";

						// Token: 0x0400ABBE RID: 43966
						public static LocString DESC = "It is neither an egg, nor a plant. But it <i>is</i> a functional helmet.";
					}

					// Token: 0x02002889 RID: 10377
					public class TRIANGLES_TURQ
					{
						// Token: 0x0400ABBF RID: 43967
						public static LocString NAME = "Confetti Atmo Helmet";

						// Token: 0x0400ABC0 RID: 43968
						public static LocString DESC = "Doubles as a party hat.";
					}

					// Token: 0x0200288A RID: 10378
					public class CUMMERBUND_RED
					{
						// Token: 0x0400ABC1 RID: 43969
						public static LocString NAME = "Blastoff Atmo Helmet";

						// Token: 0x0400ABC2 RID: 43970
						public static LocString DESC = "Red means go!";
					}

					// Token: 0x0200288B RID: 10379
					public class WORKOUT_LAVENDER
					{
						// Token: 0x0400ABC3 RID: 43971
						public static LocString NAME = "Pink Punch Atmo Helmet";

						// Token: 0x0400ABC4 RID: 43972
						public static LocString DESC = "Unapologetically ostentatious.";
					}

					// Token: 0x0200288C RID: 10380
					public class CANTALOUPE
					{
						// Token: 0x0400ABC5 RID: 43973
						public static LocString NAME = "Rocketmelon Atmo Helmet";

						// Token: 0x0400ABC6 RID: 43974
						public static LocString DESC = "A melon for your melon.";
					}

					// Token: 0x0200288D RID: 10381
					public class MONDRIAN_BLUE_RED_YELLOW
					{
						// Token: 0x0400ABC7 RID: 43975
						public static LocString NAME = "Cubist Atmo Helmet";

						// Token: 0x0400ABC8 RID: 43976
						public static LocString DESC = "Abstract geometrics are both hip <i>and</i> square.";
					}

					// Token: 0x0200288E RID: 10382
					public class OVERALLS_RED
					{
						// Token: 0x0400ABC9 RID: 43977
						public static LocString NAME = "Spiffy Atmo Helmet";

						// Token: 0x0400ABCA RID: 43978
						public static LocString DESC = "The twin antennae serve as an early warning system for low ceilings.";
					}
				}
			}

			// Token: 0x0200288F RID: 10383
			public class ATMO_SUIT_BODY
			{
				// Token: 0x0400ABCB RID: 43979
				public static LocString NAME = "Default Atmo Uniform";

				// Token: 0x0400ABCC RID: 43980
				public static LocString DESC = "Default top and bottom of an atmo suit.";

				// Token: 0x02002890 RID: 10384
				public class FACADES
				{
					// Token: 0x02002891 RID: 10385
					public class SPARKLE_RED
					{
						// Token: 0x0400ABCD RID: 43981
						public static LocString NAME = "Red Glitter Atmo Suit";

						// Token: 0x0400ABCE RID: 43982
						public static LocString DESC = "Protects the wearer from hostile environments <i>and</i> drab fashion.";
					}

					// Token: 0x02002892 RID: 10386
					public class SPARKLE_GREEN
					{
						// Token: 0x0400ABCF RID: 43983
						public static LocString NAME = "Green Glitter Atmo Suit";

						// Token: 0x0400ABD0 RID: 43984
						public static LocString DESC = "Protects the wearer from hostile environments <i>and</i> drab fashion.";
					}

					// Token: 0x02002893 RID: 10387
					public class SPARKLE_BLUE
					{
						// Token: 0x0400ABD1 RID: 43985
						public static LocString NAME = "Blue Glitter Atmo Suit";

						// Token: 0x0400ABD2 RID: 43986
						public static LocString DESC = "Protects the wearer from hostile environments <i>and</i> drab fashion.";
					}

					// Token: 0x02002894 RID: 10388
					public class SPARKLE_LAVENDER
					{
						// Token: 0x0400ABD3 RID: 43987
						public static LocString NAME = "Violet Glitter Atmo Suit";

						// Token: 0x0400ABD4 RID: 43988
						public static LocString DESC = "Protects the wearer from hostile environments <i>and</i> drab fashion.";
					}

					// Token: 0x02002895 RID: 10389
					public class LIMONE
					{
						// Token: 0x0400ABD5 RID: 43989
						public static LocString NAME = "Citrus Atmo Suit";

						// Token: 0x0400ABD6 RID: 43990
						public static LocString DESC = "Perfect for summery, atmospheric excursions.";
					}

					// Token: 0x02002896 RID: 10390
					public class PUFT
					{
						// Token: 0x0400ABD7 RID: 43991
						public static LocString NAME = "Puft Atmo Suit";

						// Token: 0x0400ABD8 RID: 43992
						public static LocString DESC = "Warning: prolonged wear may result in feelings of Puft-up pride.\nReleased for Klei Fest 2023.";
					}

					// Token: 0x02002897 RID: 10391
					public class BASIC_PURPLE
					{
						// Token: 0x0400ABD9 RID: 43993
						public static LocString NAME = "Crisp Eggplant Atmo Suit";

						// Token: 0x0400ABDA RID: 43994
						public static LocString DESC = "It really emphasizes wide shoulders.";
					}

					// Token: 0x02002898 RID: 10392
					public class PRINT_TRIANGLES_TURQ
					{
						// Token: 0x0400ABDB RID: 43995
						public static LocString NAME = "Confetti Atmo Suit";

						// Token: 0x0400ABDC RID: 43996
						public static LocString DESC = "It puts the \"fun\" in \"perfunctory nods to personnel individuality\"!";
					}

					// Token: 0x02002899 RID: 10393
					public class BASIC_NEON_PINK
					{
						// Token: 0x0400ABDD RID: 43997
						public static LocString NAME = "Crisp Neon Pink Atmo Suit";

						// Token: 0x0400ABDE RID: 43998
						public static LocString DESC = "The neck is a little snug.";
					}

					// Token: 0x0200289A RID: 10394
					public class MULTI_RED_BLACK
					{
						// Token: 0x0400ABDF RID: 43999
						public static LocString NAME = "Red-bellied Atmo Suit";

						// Token: 0x0400ABE0 RID: 44000
						public static LocString DESC = "It really highlights the midsection.";
					}

					// Token: 0x0200289B RID: 10395
					public class CANTALOUPE
					{
						// Token: 0x0400ABE1 RID: 44001
						public static LocString NAME = "Rocketmelon Atmo Suit";

						// Token: 0x0400ABE2 RID: 44002
						public static LocString DESC = "It starts to smell ripe pretty quickly.";
					}

					// Token: 0x0200289C RID: 10396
					public class MULTI_BLUE_GREY_BLACK
					{
						// Token: 0x0400ABE3 RID: 44003
						public static LocString NAME = "Swagger Atmo Suit";

						// Token: 0x0400ABE4 RID: 44004
						public static LocString DESC = "Engineered to resemble stonewashed denim and black leather.";
					}

					// Token: 0x0200289D RID: 10397
					public class MULTI_BLUE_YELLOW_RED
					{
						// Token: 0x0400ABE5 RID: 44005
						public static LocString NAME = "Fundamental Stripe Atmo Suit";

						// Token: 0x0400ABE6 RID: 44006
						public static LocString DESC = "Designed by the Primary Colors Appreciation Society.";
					}
				}
			}

			// Token: 0x0200289E RID: 10398
			public class ATMO_SUIT_GLOVES
			{
				// Token: 0x0400ABE7 RID: 44007
				public static LocString NAME = "Default Atmo Gloves";

				// Token: 0x0400ABE8 RID: 44008
				public static LocString DESC = "Default atmo suit gloves.";

				// Token: 0x0200289F RID: 10399
				public class FACADES
				{
					// Token: 0x020028A0 RID: 10400
					public class SPARKLE_RED
					{
						// Token: 0x0400ABE9 RID: 44009
						public static LocString NAME = "Red Glitter Atmo Gloves";

						// Token: 0x0400ABEA RID: 44010
						public static LocString DESC = "Sparkly red gloves for hostile environments.";
					}

					// Token: 0x020028A1 RID: 10401
					public class SPARKLE_GREEN
					{
						// Token: 0x0400ABEB RID: 44011
						public static LocString NAME = "Green Glitter Atmo Gloves";

						// Token: 0x0400ABEC RID: 44012
						public static LocString DESC = "Sparkly green gloves for hostile environments.";
					}

					// Token: 0x020028A2 RID: 10402
					public class SPARKLE_BLUE
					{
						// Token: 0x0400ABED RID: 44013
						public static LocString NAME = "Blue Glitter Atmo Gloves";

						// Token: 0x0400ABEE RID: 44014
						public static LocString DESC = "Sparkly blue gloves for hostile environments.";
					}

					// Token: 0x020028A3 RID: 10403
					public class SPARKLE_LAVENDER
					{
						// Token: 0x0400ABEF RID: 44015
						public static LocString NAME = "Violet Glitter Atmo Gloves";

						// Token: 0x0400ABF0 RID: 44016
						public static LocString DESC = "Sparkly violet gloves for hostile environments.";
					}

					// Token: 0x020028A4 RID: 10404
					public class LIMONE
					{
						// Token: 0x0400ABF1 RID: 44017
						public static LocString NAME = "Citrus Atmo Gloves";

						// Token: 0x0400ABF2 RID: 44018
						public static LocString DESC = "Lime-inspired gloves brighten up hostile environments.";
					}

					// Token: 0x020028A5 RID: 10405
					public class PUFT
					{
						// Token: 0x0400ABF3 RID: 44019
						public static LocString NAME = "Puft Atmo Gloves";

						// Token: 0x0400ABF4 RID: 44020
						public static LocString DESC = "A little Puft-love for delicate extremities.\nReleased for Klei Fest 2023.";
					}

					// Token: 0x020028A6 RID: 10406
					public class GOLD
					{
						// Token: 0x0400ABF5 RID: 44021
						public static LocString NAME = "Gold Atmo Gloves";

						// Token: 0x0400ABF6 RID: 44022
						public static LocString DESC = "A golden touch! Without all the Midas-type baggage.";
					}

					// Token: 0x020028A7 RID: 10407
					public class PURPLE
					{
						// Token: 0x0400ABF7 RID: 44023
						public static LocString NAME = "Eggplant Atmo Gloves";

						// Token: 0x0400ABF8 RID: 44024
						public static LocString DESC = "Fab purple gloves for hostile environments.";
					}

					// Token: 0x020028A8 RID: 10408
					public class WHITE
					{
						// Token: 0x0400ABF9 RID: 44025
						public static LocString NAME = "White Atmo Gloves";

						// Token: 0x0400ABFA RID: 44026
						public static LocString DESC = "For the Duplicant who never gets their hands dirty.";
					}

					// Token: 0x020028A9 RID: 10409
					public class STRIPES_LAVENDER
					{
						// Token: 0x0400ABFB RID: 44027
						public static LocString NAME = "Wildberry Atmo Gloves";

						// Token: 0x0400ABFC RID: 44028
						public static LocString DESC = "Functional finger-protectors with fruity flair.";
					}

					// Token: 0x020028AA RID: 10410
					public class CANTALOUPE
					{
						// Token: 0x0400ABFD RID: 44029
						public static LocString NAME = "Rocketmelon Atmo Gloves";

						// Token: 0x0400ABFE RID: 44030
						public static LocString DESC = "It takes eighteen melon rinds to make a single glove.";
					}

					// Token: 0x020028AB RID: 10411
					public class BROWN
					{
						// Token: 0x0400ABFF RID: 44031
						public static LocString NAME = "Leather Atmo Gloves";

						// Token: 0x0400AC00 RID: 44032
						public static LocString DESC = "They creak rather loudly during the break-in period.";
					}
				}
			}

			// Token: 0x020028AC RID: 10412
			public class ATMO_SUIT_BELT
			{
				// Token: 0x0400AC01 RID: 44033
				public static LocString NAME = "Default Atmo Belt";

				// Token: 0x0400AC02 RID: 44034
				public static LocString DESC = "Default belt for atmo suits.";

				// Token: 0x020028AD RID: 10413
				public class FACADES
				{
					// Token: 0x020028AE RID: 10414
					public class SPARKLE_RED
					{
						// Token: 0x0400AC03 RID: 44035
						public static LocString NAME = "Red Glitter Atmo Belt";

						// Token: 0x0400AC04 RID: 44036
						public static LocString DESC = "It's red! It's shiny! It keeps atmo suit pants on!";
					}

					// Token: 0x020028AF RID: 10415
					public class SPARKLE_GREEN
					{
						// Token: 0x0400AC05 RID: 44037
						public static LocString NAME = "Green Glitter Atmo Belt";

						// Token: 0x0400AC06 RID: 44038
						public static LocString DESC = "It's green! It's shiny! It keeps atmo suit pants on!";
					}

					// Token: 0x020028B0 RID: 10416
					public class SPARKLE_BLUE
					{
						// Token: 0x0400AC07 RID: 44039
						public static LocString NAME = "Blue Glitter Atmo Belt";

						// Token: 0x0400AC08 RID: 44040
						public static LocString DESC = "It's blue! It's shiny! It keeps atmo suit pants on!";
					}

					// Token: 0x020028B1 RID: 10417
					public class SPARKLE_LAVENDER
					{
						// Token: 0x0400AC09 RID: 44041
						public static LocString NAME = "Violet Glitter Atmo Belt";

						// Token: 0x0400AC0A RID: 44042
						public static LocString DESC = "It's violet! It's shiny! It keeps atmo suit pants on!";
					}

					// Token: 0x020028B2 RID: 10418
					public class LIMONE
					{
						// Token: 0x0400AC0B RID: 44043
						public static LocString NAME = "Citrus Atmo Belt";

						// Token: 0x0400AC0C RID: 44044
						public static LocString DESC = "This lime-hued belt really pulls an atmo suit together.";
					}

					// Token: 0x020028B3 RID: 10419
					public class PUFT
					{
						// Token: 0x0400AC0D RID: 44045
						public static LocString NAME = "Puft Atmo Belt";

						// Token: 0x0400AC0E RID: 44046
						public static LocString DESC = "If critters wore belts...\nReleased for Klei Fest 2023.";
					}

					// Token: 0x020028B4 RID: 10420
					public class TWOTONE_PURPLE
					{
						// Token: 0x0400AC0F RID: 44047
						public static LocString NAME = "Eggplant Atmo Belt";

						// Token: 0x0400AC10 RID: 44048
						public static LocString DESC = "In the more pretentious space-fashion circles, it's known as \"aubergine.\"";
					}

					// Token: 0x020028B5 RID: 10421
					public class BASIC_GOLD
					{
						// Token: 0x0400AC11 RID: 44049
						public static LocString NAME = "Gold Atmo Belt";

						// Token: 0x0400AC12 RID: 44050
						public static LocString DESC = "Better to be overdressed than underdressed.";
					}

					// Token: 0x020028B6 RID: 10422
					public class BASIC_GREY
					{
						// Token: 0x0400AC13 RID: 44051
						public static LocString NAME = "Slate Atmo Belt";

						// Token: 0x0400AC14 RID: 44052
						public static LocString DESC = "Slick and understated space style.";
					}

					// Token: 0x020028B7 RID: 10423
					public class BASIC_NEON_PINK
					{
						// Token: 0x0400AC15 RID: 44053
						public static LocString NAME = "Neon Pink Atmo Belt";

						// Token: 0x0400AC16 RID: 44054
						public static LocString DESC = "Visible from several planetoids away.";
					}

					// Token: 0x020028B8 RID: 10424
					public class CANTALOUPE
					{
						// Token: 0x0400AC17 RID: 44055
						public static LocString NAME = "Rocketmelon Atmo Belt";

						// Token: 0x0400AC18 RID: 44056
						public static LocString DESC = "A tribute to the <i>cucumis melo cantalupensis</i>.";
					}

					// Token: 0x020028B9 RID: 10425
					public class TWOTONE_BROWN
					{
						// Token: 0x0400AC19 RID: 44057
						public static LocString NAME = "Leather Atmo Belt";

						// Token: 0x0400AC1A RID: 44058
						public static LocString DESC = "Crafted from the tanned hide of a thick-skinned critter.";
					}
				}
			}

			// Token: 0x020028BA RID: 10426
			public class ATMO_SUIT_SHOES
			{
				// Token: 0x0400AC1B RID: 44059
				public static LocString NAME = "Default Atmo Boots";

				// Token: 0x0400AC1C RID: 44060
				public static LocString DESC = "Default footwear for atmo suits.";

				// Token: 0x020028BB RID: 10427
				public class FACADES
				{
					// Token: 0x020028BC RID: 10428
					public class LIMONE
					{
						// Token: 0x0400AC1D RID: 44061
						public static LocString NAME = "Citrus Atmo Boots";

						// Token: 0x0400AC1E RID: 44062
						public static LocString DESC = "Cheery boots for stomping around in hostile environments.";
					}

					// Token: 0x020028BD RID: 10429
					public class PUFT
					{
						// Token: 0x0400AC1F RID: 44063
						public static LocString NAME = "Puft Atmo Boots";

						// Token: 0x0400AC20 RID: 44064
						public static LocString DESC = "These boots were made for puft-ing.\nReleased for Klei Fest 2023.";
					}

					// Token: 0x020028BE RID: 10430
					public class SPARKLE_BLACK
					{
						// Token: 0x0400AC21 RID: 44065
						public static LocString NAME = "Black Glitter Atmo Boots";

						// Token: 0x0400AC22 RID: 44066
						public static LocString DESC = "A timeless color, with a little pizzazz.";
					}

					// Token: 0x020028BF RID: 10431
					public class BASIC_BLACK
					{
						// Token: 0x0400AC23 RID: 44067
						public static LocString NAME = "Stealth Atmo Boots";

						// Token: 0x0400AC24 RID: 44068
						public static LocString DESC = "They attract no attention at all.";
					}

					// Token: 0x020028C0 RID: 10432
					public class BASIC_PURPLE
					{
						// Token: 0x0400AC25 RID: 44069
						public static LocString NAME = "Eggplant Atmo Boots";

						// Token: 0x0400AC26 RID: 44070
						public static LocString DESC = "Purple boots for stomping around in hostile environments.";
					}

					// Token: 0x020028C1 RID: 10433
					public class BASIC_LAVENDER
					{
						// Token: 0x0400AC27 RID: 44071
						public static LocString NAME = "Lavender Atmo Boots";

						// Token: 0x0400AC28 RID: 44072
						public static LocString DESC = "Soothing space booties for tired feet.";
					}

					// Token: 0x020028C2 RID: 10434
					public class CANTALOUPE
					{
						// Token: 0x0400AC29 RID: 44073
						public static LocString NAME = "Rocketmelon Atmo Boots";

						// Token: 0x0400AC2A RID: 44074
						public static LocString DESC = "Keeps feet safe (and juicy) in hostile environments.";
					}
				}
			}

			// Token: 0x020028C3 RID: 10435
			public class AQUA_SUIT
			{
				// Token: 0x0400AC2B RID: 44075
				public static LocString NAME = UI.FormatAsLink("Aqua Suit", "AQUA_SUIT");

				// Token: 0x0400AC2C RID: 44076
				public static LocString DESC = "Because breathing underwater is better than... not.";

				// Token: 0x0400AC2D RID: 44077
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Supplies Duplicants with <style=\"oxygen\">Oxygen</style> in underwater environments.\n\nMust be refilled with ",
					UI.FormatAsLink("Oxygen", "OXYGEN"),
					" at an ",
					UI.FormatAsLink("Atmo Suit Dock", "SUITLOCKER"),
					" when depleted."
				});

				// Token: 0x0400AC2E RID: 44078
				public static LocString RECIPE_DESC = "Supplies Duplicants with <style=\"oxygen\">Oxygen</style> in underwater environments.";

				// Token: 0x0400AC2F RID: 44079
				public static LocString WORN_NAME = UI.FormatAsLink("Worn Lead Suit", "AQUA_SUIT");

				// Token: 0x0400AC30 RID: 44080
				public static LocString WORN_DESC = string.Concat(new string[]
				{
					"A worn out ",
					UI.FormatAsLink("Aqua Suit", "AQUA_SUIT"),
					".\n\nSuits can be repaired at a ",
					UI.FormatAsLink("Crafting Station", "CRAFTINGTABLE"),
					"."
				});
			}

			// Token: 0x020028C4 RID: 10436
			public class TEMPERATURE_SUIT
			{
				// Token: 0x0400AC31 RID: 44081
				public static LocString NAME = UI.FormatAsLink("Thermo Suit", "TEMPERATURE_SUIT");

				// Token: 0x0400AC32 RID: 44082
				public static LocString DESC = "Keeps my Duplicants cool in case things heat up.";

				// Token: 0x0400AC33 RID: 44083
				public static LocString EFFECT = "Provides insulation in regions with extreme <style=\"heat\">Temperatures</style>.\n\nMust be powered at a Thermo Suit Dock when depleted.";

				// Token: 0x0400AC34 RID: 44084
				public static LocString RECIPE_DESC = "Provides insulation in regions with extreme <style=\"heat\">Temperatures</style>.";

				// Token: 0x0400AC35 RID: 44085
				public static LocString WORN_NAME = UI.FormatAsLink("Worn Lead Suit", "TEMPERATURE_SUIT");

				// Token: 0x0400AC36 RID: 44086
				public static LocString WORN_DESC = string.Concat(new string[]
				{
					"A worn out ",
					UI.FormatAsLink("Thermo Suit", "TEMPERATURE_SUIT"),
					".\n\nSuits can be repaired at a ",
					UI.FormatAsLink("Crafting Station", "CRAFTINGTABLE"),
					"."
				});
			}

			// Token: 0x020028C5 RID: 10437
			public class JET_SUIT
			{
				// Token: 0x0400AC37 RID: 44087
				public static LocString NAME = UI.FormatAsLink("Jet Suit", "JET_SUIT");

				// Token: 0x0400AC38 RID: 44088
				public static LocString DESC = "Allows my Duplicants to take to the skies, for a time.";

				// Token: 0x0400AC39 RID: 44089
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Supplies Duplicants with ",
					UI.FormatAsLink("Oxygen", "OXYGEN"),
					" in toxic and low breathability environments.\n\nMust be refilled with ",
					UI.FormatAsLink("Oxygen", "OXYGEN"),
					" and ",
					UI.FormatAsLink("Petroleum", "PETROLEUM"),
					" at a ",
					UI.FormatAsLink("Jet Suit Dock", "JETSUITLOCKER"),
					" when depleted."
				});

				// Token: 0x0400AC3A RID: 44090
				public static LocString RECIPE_DESC = "Supplies Duplicants with " + UI.FormatAsLink("Oxygen", "OXYGEN") + " in toxic and low breathability environments.\n\nAllows Duplicant flight.";

				// Token: 0x0400AC3B RID: 44091
				public static LocString GENERICNAME = "Jet Suit";

				// Token: 0x0400AC3C RID: 44092
				public static LocString TANK_EFFECT_NAME = "Fuel Tank";

				// Token: 0x0400AC3D RID: 44093
				public static LocString WORN_NAME = UI.FormatAsLink("Worn Jet Suit", "JET_SUIT");

				// Token: 0x0400AC3E RID: 44094
				public static LocString WORN_DESC = string.Concat(new string[]
				{
					"A worn out ",
					UI.FormatAsLink("Jet Suit", "JET_SUIT"),
					".\n\nSuits can be repaired at an ",
					UI.FormatAsLink("Exosuit Forge", "SUITFABRICATOR"),
					"."
				});
			}

			// Token: 0x020028C6 RID: 10438
			public class LEAD_SUIT
			{
				// Token: 0x0400AC3F RID: 44095
				public static LocString NAME = UI.FormatAsLink("Lead Suit", "LEAD_SUIT");

				// Token: 0x0400AC40 RID: 44096
				public static LocString DESC = "Because exposure to radiation doesn't grant Duplicants superpowers.";

				// Token: 0x0400AC41 RID: 44097
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Supplies Duplicants with ",
					UI.FormatAsLink("Oxygen", "OXYGEN"),
					" and protection in areas with ",
					UI.FormatAsLink("Radiation", "RADIATION"),
					".\n\nMust be refilled with ",
					UI.FormatAsLink("Oxygen", "OXYGEN"),
					" at a ",
					UI.FormatAsLink("Lead Suit Dock", "LEADSUITLOCKER"),
					" when depleted."
				});

				// Token: 0x0400AC42 RID: 44098
				public static LocString RECIPE_DESC = string.Concat(new string[]
				{
					"Supplies Duplicants with ",
					UI.FormatAsLink("Oxygen", "OXYGEN"),
					" in toxic and low breathability environments.\n\nProtects Duplicants from ",
					UI.FormatAsLink("Radiation", "RADIATION"),
					"."
				});

				// Token: 0x0400AC43 RID: 44099
				public static LocString GENERICNAME = "Lead Suit";

				// Token: 0x0400AC44 RID: 44100
				public static LocString BATTERY_EFFECT_NAME = "Suit Battery";

				// Token: 0x0400AC45 RID: 44101
				public static LocString SUIT_OUT_OF_BATTERIES = "Suit Batteries Empty";

				// Token: 0x0400AC46 RID: 44102
				public static LocString WORN_NAME = UI.FormatAsLink("Worn Lead Suit", "LEAD_SUIT");

				// Token: 0x0400AC47 RID: 44103
				public static LocString WORN_DESC = string.Concat(new string[]
				{
					"A worn out ",
					UI.FormatAsLink("Lead Suit", "LEAD_SUIT"),
					".\n\nSuits can be repaired at an ",
					UI.FormatAsLink("Exosuit Forge", "SUITFABRICATOR"),
					"."
				});
			}

			// Token: 0x020028C7 RID: 10439
			public class COOL_VEST
			{
				// Token: 0x0400AC48 RID: 44104
				public static LocString NAME = UI.FormatAsLink("Cool Vest", "COOL_VEST");

				// Token: 0x0400AC49 RID: 44105
				public static LocString GENERICNAME = "Clothing";

				// Token: 0x0400AC4A RID: 44106
				public static LocString DESC = "Don't sweat it!";

				// Token: 0x0400AC4B RID: 44107
				public static LocString EFFECT = "Protects the wearer from <style=\"heat\">Heat</style> by decreasing insulation.";

				// Token: 0x0400AC4C RID: 44108
				public static LocString RECIPE_DESC = "Protects the wearer from <style=\"heat\">Heat</style> by decreasing insulation";
			}

			// Token: 0x020028C8 RID: 10440
			public class WARM_VEST
			{
				// Token: 0x0400AC4D RID: 44109
				public static LocString NAME = UI.FormatAsLink("Warm Coat", "WARM_VEST");

				// Token: 0x0400AC4E RID: 44110
				public static LocString GENERICNAME = "Clothing";

				// Token: 0x0400AC4F RID: 44111
				public static LocString DESC = "Happiness is a warm Duplicant.";

				// Token: 0x0400AC50 RID: 44112
				public static LocString EFFECT = "Protects the wearer from <style=\"heat\">Cold</style> by increasing insulation.";

				// Token: 0x0400AC51 RID: 44113
				public static LocString RECIPE_DESC = "Protects the wearer from <style=\"heat\">Cold</style> by increasing insulation";
			}

			// Token: 0x020028C9 RID: 10441
			public class FUNKY_VEST
			{
				// Token: 0x0400AC52 RID: 44114
				public static LocString NAME = UI.FormatAsLink("Snazzy Suit", "FUNKY_VEST");

				// Token: 0x0400AC53 RID: 44115
				public static LocString GENERICNAME = "Clothing";

				// Token: 0x0400AC54 RID: 44116
				public static LocString DESC = "This transforms my Duplicant into a walking beacon of charm and style.";

				// Token: 0x0400AC55 RID: 44117
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Increases Decor in a small area effect around the wearer. Can be upgraded to ",
					UI.FormatAsLink("Primo Garb", "CUSTOMCLOTHING"),
					" at the ",
					UI.FormatAsLink("Clothing Refashionator", "CLOTHINGALTERATIONSTATION"),
					"."
				});

				// Token: 0x0400AC56 RID: 44118
				public static LocString RECIPE_DESC = "Increases Decor in a small area effect around the wearer. Can be upgraded to " + UI.FormatAsLink("Primo Garb", "CUSTOMCLOTHING") + " at the " + UI.FormatAsLink("Clothing Refashionator", "CLOTHINGALTERATIONSTATION");
			}

			// Token: 0x020028CA RID: 10442
			public class CUSTOMCLOTHING
			{
				// Token: 0x0400AC57 RID: 44119
				public static LocString NAME = UI.FormatAsLink("Primo Garb", "CUSTOMCLOTHING");

				// Token: 0x0400AC58 RID: 44120
				public static LocString GENERICNAME = "Clothing";

				// Token: 0x0400AC59 RID: 44121
				public static LocString DESC = "This transforms my Duplicant into a colony-inspiring fashion icon.";

				// Token: 0x0400AC5A RID: 44122
				public static LocString EFFECT = "Increases Decor in a small area effect around the wearer.";

				// Token: 0x0400AC5B RID: 44123
				public static LocString RECIPE_DESC = "Increases Decor in a small area effect around the wearer";

				// Token: 0x020028CB RID: 10443
				public class FACADES
				{
					// Token: 0x0400AC5C RID: 44124
					public static LocString CLUBSHIRT = UI.FormatAsLink("Purple Polyester Suit", "CUSTOMCLOTHING");

					// Token: 0x0400AC5D RID: 44125
					public static LocString CUMMERBUND = UI.FormatAsLink("Classic Cummerbund", "CUSTOMCLOTHING");

					// Token: 0x0400AC5E RID: 44126
					public static LocString DECOR_02 = UI.FormatAsLink("Snazzier Red Suit", "CUSTOMCLOTHING");

					// Token: 0x0400AC5F RID: 44127
					public static LocString DECOR_03 = UI.FormatAsLink("Snazzier Blue Suit", "CUSTOMCLOTHING");

					// Token: 0x0400AC60 RID: 44128
					public static LocString DECOR_04 = UI.FormatAsLink("Snazzier Green Suit", "CUSTOMCLOTHING");

					// Token: 0x0400AC61 RID: 44129
					public static LocString DECOR_05 = UI.FormatAsLink("Snazzier Violet Suit", "CUSTOMCLOTHING");

					// Token: 0x0400AC62 RID: 44130
					public static LocString GAUDYSWEATER = UI.FormatAsLink("Pompom Knit Suit", "CUSTOMCLOTHING");

					// Token: 0x0400AC63 RID: 44131
					public static LocString LIMONE = UI.FormatAsLink("Citrus Spandex Suit", "CUSTOMCLOTHING");

					// Token: 0x0400AC64 RID: 44132
					public static LocString MONDRIAN = UI.FormatAsLink("Cubist Knit Suit", "CUSTOMCLOTHING");

					// Token: 0x0400AC65 RID: 44133
					public static LocString OVERALLS = UI.FormatAsLink("Spiffy Overalls", "CUSTOMCLOTHING");

					// Token: 0x0400AC66 RID: 44134
					public static LocString TRIANGLES = UI.FormatAsLink("Confetti Suit", "CUSTOMCLOTHING");

					// Token: 0x0400AC67 RID: 44135
					public static LocString WORKOUT = UI.FormatAsLink("Pink Unitard", "CUSTOMCLOTHING");
				}
			}

			// Token: 0x020028CC RID: 10444
			public class CLOTHING_GLOVES
			{
				// Token: 0x0400AC68 RID: 44136
				public static LocString NAME = "Default Gloves";

				// Token: 0x0400AC69 RID: 44137
				public static LocString DESC = "The default gloves.";

				// Token: 0x020028CD RID: 10445
				public class FACADES
				{
					// Token: 0x020028CE RID: 10446
					public class BASIC_BLUE_MIDDLE
					{
						// Token: 0x0400AC6A RID: 44138
						public static LocString NAME = "Basic Aqua Gloves";

						// Token: 0x0400AC6B RID: 44139
						public static LocString DESC = "A good, solid pair of aqua-blue gloves that go with everything.";
					}

					// Token: 0x020028CF RID: 10447
					public class BASIC_YELLOW
					{
						// Token: 0x0400AC6C RID: 44140
						public static LocString NAME = "Basic Yellow Gloves";

						// Token: 0x0400AC6D RID: 44141
						public static LocString DESC = "A good, solid pair of yellow gloves that go with everything.";
					}

					// Token: 0x020028D0 RID: 10448
					public class BASIC_BLACK
					{
						// Token: 0x0400AC6E RID: 44142
						public static LocString NAME = "Basic Black Gloves";

						// Token: 0x0400AC6F RID: 44143
						public static LocString DESC = "A good, solid pair of black gloves that go with everything.";
					}

					// Token: 0x020028D1 RID: 10449
					public class BASIC_PINK_ORCHID
					{
						// Token: 0x0400AC70 RID: 44144
						public static LocString NAME = "Basic Bubblegum Gloves";

						// Token: 0x0400AC71 RID: 44145
						public static LocString DESC = "A good, solid pair of bubblegum-pink gloves that go with everything.";
					}

					// Token: 0x020028D2 RID: 10450
					public class BASIC_GREEN
					{
						// Token: 0x0400AC72 RID: 44146
						public static LocString NAME = "Basic Green Gloves";

						// Token: 0x0400AC73 RID: 44147
						public static LocString DESC = "A good, solid pair of green gloves that go with everything.";
					}

					// Token: 0x020028D3 RID: 10451
					public class BASIC_ORANGE
					{
						// Token: 0x0400AC74 RID: 44148
						public static LocString NAME = "Basic Orange Gloves";

						// Token: 0x0400AC75 RID: 44149
						public static LocString DESC = "A good, solid pair of orange gloves that go with everything.";
					}

					// Token: 0x020028D4 RID: 10452
					public class BASIC_PURPLE
					{
						// Token: 0x0400AC76 RID: 44150
						public static LocString NAME = "Basic Purple Gloves";

						// Token: 0x0400AC77 RID: 44151
						public static LocString DESC = "A good, solid pair of purple gloves that go with everything.";
					}

					// Token: 0x020028D5 RID: 10453
					public class BASIC_RED
					{
						// Token: 0x0400AC78 RID: 44152
						public static LocString NAME = "Basic Red Gloves";

						// Token: 0x0400AC79 RID: 44153
						public static LocString DESC = "A good, solid pair of red gloves that go with everything.";
					}

					// Token: 0x020028D6 RID: 10454
					public class BASIC_WHITE
					{
						// Token: 0x0400AC7A RID: 44154
						public static LocString NAME = "Basic White Gloves";

						// Token: 0x0400AC7B RID: 44155
						public static LocString DESC = "A good, solid pair of white gloves that go with everything.";
					}

					// Token: 0x020028D7 RID: 10455
					public class GLOVES_ATHLETIC_DEEPRED
					{
						// Token: 0x0400AC7C RID: 44156
						public static LocString NAME = "Team Captain Sports Gloves";

						// Token: 0x0400AC7D RID: 44157
						public static LocString DESC = "Red-striped gloves for winning at any activity.";
					}

					// Token: 0x020028D8 RID: 10456
					public class GLOVES_ATHLETIC_SATSUMA
					{
						// Token: 0x0400AC7E RID: 44158
						public static LocString NAME = "Superfan Sports Gloves";

						// Token: 0x0400AC7F RID: 44159
						public static LocString DESC = "Orange-striped gloves for enthusiastic athletes.";
					}

					// Token: 0x020028D9 RID: 10457
					public class GLOVES_ATHLETIC_LEMON
					{
						// Token: 0x0400AC80 RID: 44160
						public static LocString NAME = "Hype Sports Gloves";

						// Token: 0x0400AC81 RID: 44161
						public static LocString DESC = "Yellow-striped gloves for athletes who seek to raise the bar.";
					}

					// Token: 0x020028DA RID: 10458
					public class GLOVES_ATHLETIC_KELLYGREEN
					{
						// Token: 0x0400AC82 RID: 44162
						public static LocString NAME = "Go Team Sports Gloves";

						// Token: 0x0400AC83 RID: 44163
						public static LocString DESC = "Green-striped gloves for the perenially good sport.";
					}

					// Token: 0x020028DB RID: 10459
					public class GLOVES_ATHLETIC_COBALT
					{
						// Token: 0x0400AC84 RID: 44164
						public static LocString NAME = "True Blue Sports Gloves";

						// Token: 0x0400AC85 RID: 44165
						public static LocString DESC = "Blue-striped gloves perfect for shaking hands after the game.";
					}

					// Token: 0x020028DC RID: 10460
					public class GLOVES_ATHLETIC_FLAMINGO
					{
						// Token: 0x0400AC86 RID: 44166
						public static LocString NAME = "Pep Rally Sports Gloves";

						// Token: 0x0400AC87 RID: 44167
						public static LocString DESC = "Pink-striped glove designed to withstand countless high-fives.";
					}

					// Token: 0x020028DD RID: 10461
					public class GLOVES_ATHLETIC_CHARCOAL
					{
						// Token: 0x0400AC88 RID: 44168
						public static LocString NAME = "Underdog Sports Gloves";

						// Token: 0x0400AC89 RID: 44169
						public static LocString DESC = "The muted stripe minimizes distractions so its wearer can focus on trying very, very hard.";
					}

					// Token: 0x020028DE RID: 10462
					public class CUFFLESS_BLUEBERRY
					{
						// Token: 0x0400AC8A RID: 44170
						public static LocString NAME = "Blueberry Glovelets";

						// Token: 0x0400AC8B RID: 44171
						public static LocString DESC = "Wrist coverage is <i>so</i> overrated.";
					}

					// Token: 0x020028DF RID: 10463
					public class CUFFLESS_GRAPE
					{
						// Token: 0x0400AC8C RID: 44172
						public static LocString NAME = "Grape Glovelets";

						// Token: 0x0400AC8D RID: 44173
						public static LocString DESC = "Wrist coverage is <i>so</i> overrated.";
					}

					// Token: 0x020028E0 RID: 10464
					public class CUFFLESS_LEMON
					{
						// Token: 0x0400AC8E RID: 44174
						public static LocString NAME = "Lemon Glovelets";

						// Token: 0x0400AC8F RID: 44175
						public static LocString DESC = "Wrist coverage is <i>so</i> overrated.";
					}

					// Token: 0x020028E1 RID: 10465
					public class CUFFLESS_LIME
					{
						// Token: 0x0400AC90 RID: 44176
						public static LocString NAME = "Lime Glovelets";

						// Token: 0x0400AC91 RID: 44177
						public static LocString DESC = "Wrist coverage is <i>so</i> overrated.";
					}

					// Token: 0x020028E2 RID: 10466
					public class CUFFLESS_SATSUMA
					{
						// Token: 0x0400AC92 RID: 44178
						public static LocString NAME = "Satsuma Glovelets";

						// Token: 0x0400AC93 RID: 44179
						public static LocString DESC = "Wrist coverage is <i>so</i> overrated.";
					}

					// Token: 0x020028E3 RID: 10467
					public class CUFFLESS_STRAWBERRY
					{
						// Token: 0x0400AC94 RID: 44180
						public static LocString NAME = "Strawberry Glovelets";

						// Token: 0x0400AC95 RID: 44181
						public static LocString DESC = "Wrist coverage is <i>so</i> overrated.";
					}

					// Token: 0x020028E4 RID: 10468
					public class CUFFLESS_WATERMELON
					{
						// Token: 0x0400AC96 RID: 44182
						public static LocString NAME = "Watermelon Glovelets";

						// Token: 0x0400AC97 RID: 44183
						public static LocString DESC = "Wrist coverage is <i>so</i> overrated.";
					}

					// Token: 0x020028E5 RID: 10469
					public class CIRCUIT_GREEN
					{
						// Token: 0x0400AC98 RID: 44184
						public static LocString NAME = "LED Gloves";

						// Token: 0x0400AC99 RID: 44185
						public static LocString DESC = "Great for gesticulating at parties.";
					}

					// Token: 0x020028E6 RID: 10470
					public class ATHLETE
					{
						// Token: 0x0400AC9A RID: 44186
						public static LocString NAME = "Racing Gloves";

						// Token: 0x0400AC9B RID: 44187
						public static LocString DESC = "Crafted for high-speed handshakes.";
					}

					// Token: 0x020028E7 RID: 10471
					public class BASIC_BROWN_KHAKI
					{
						// Token: 0x0400AC9C RID: 44188
						public static LocString NAME = "Basic Khaki Gloves";

						// Token: 0x0400AC9D RID: 44189
						public static LocString DESC = "They don't show dirt.";
					}

					// Token: 0x020028E8 RID: 10472
					public class BASIC_BLUEGREY
					{
						// Token: 0x0400AC9E RID: 44190
						public static LocString NAME = "Basic Gunmetal Gloves";

						// Token: 0x0400AC9F RID: 44191
						public static LocString DESC = "A tough name for soft gloves.";
					}

					// Token: 0x020028E9 RID: 10473
					public class CUFFLESS_BLACK
					{
						// Token: 0x0400ACA0 RID: 44192
						public static LocString NAME = "Stealth Glovelets";

						// Token: 0x0400ACA1 RID: 44193
						public static LocString DESC = "It's easy to forget they're even on.";
					}

					// Token: 0x020028EA RID: 10474
					public class DENIM_BLUE
					{
						// Token: 0x0400ACA2 RID: 44194
						public static LocString NAME = "Denim Gloves";

						// Token: 0x0400ACA3 RID: 44195
						public static LocString DESC = "They're not great for dexterity.";
					}

					// Token: 0x020028EB RID: 10475
					public class BASIC_GREY
					{
						// Token: 0x0400ACA4 RID: 44196
						public static LocString NAME = "Basic Gray Gloves";

						// Token: 0x0400ACA5 RID: 44197
						public static LocString DESC = "A good, solid pair of gray gloves that go with everything.";
					}

					// Token: 0x020028EC RID: 10476
					public class BASIC_PINKSALMON
					{
						// Token: 0x0400ACA6 RID: 44198
						public static LocString NAME = "Basic Coral Gloves";

						// Token: 0x0400ACA7 RID: 44199
						public static LocString DESC = "A good, solid pair of bright pink gloves that go with everything.";
					}

					// Token: 0x020028ED RID: 10477
					public class BASIC_TAN
					{
						// Token: 0x0400ACA8 RID: 44200
						public static LocString NAME = "Basic Tan Gloves";

						// Token: 0x0400ACA9 RID: 44201
						public static LocString DESC = "A good, solid pair of tan gloves that go with everything.";
					}

					// Token: 0x020028EE RID: 10478
					public class BALLERINA_PINK
					{
						// Token: 0x0400ACAA RID: 44202
						public static LocString NAME = "Ballet Gloves";

						// Token: 0x0400ACAB RID: 44203
						public static LocString DESC = "Wrist ruffles highlight the poetic movements of the phalanges.";
					}

					// Token: 0x020028EF RID: 10479
					public class FORMAL_WHITE
					{
						// Token: 0x0400ACAC RID: 44204
						public static LocString NAME = "White Silk Gloves";

						// Token: 0x0400ACAD RID: 44205
						public static LocString DESC = "They're as soft as...well, silk.";
					}

					// Token: 0x020028F0 RID: 10480
					public class LONG_WHITE
					{
						// Token: 0x0400ACAE RID: 44206
						public static LocString NAME = "White Evening Gloves";

						// Token: 0x0400ACAF RID: 44207
						public static LocString DESC = "Super-long gloves for super-formal occasions.";
					}

					// Token: 0x020028F1 RID: 10481
					public class TWOTONE_CREAM_CHARCOAL
					{
						// Token: 0x0400ACB0 RID: 44208
						public static LocString NAME = "Contrast Cuff Gloves";

						// Token: 0x0400ACB1 RID: 44209
						public static LocString DESC = "For elegance so understated, it may go completely unnoticed.";
					}

					// Token: 0x020028F2 RID: 10482
					public class SOCKSUIT_BEIGE
					{
						// Token: 0x0400ACB2 RID: 44210
						public static LocString NAME = "Vintage Handsock";

						// Token: 0x0400ACB3 RID: 44211
						public static LocString DESC = "Designed by someone with cold hands and an excess of old socks.";
					}

					// Token: 0x020028F3 RID: 10483
					public class BASIC_SLATE
					{
						// Token: 0x0400ACB4 RID: 44212
						public static LocString NAME = "Basic Slate Gloves";

						// Token: 0x0400ACB5 RID: 44213
						public static LocString DESC = "A good, solid pair of slate gloves that go with everything.";
					}

					// Token: 0x020028F4 RID: 10484
					public class KNIT_GOLD
					{
						// Token: 0x0400ACB6 RID: 44214
						public static LocString NAME = "Gold Knit Gloves";

						// Token: 0x0400ACB7 RID: 44215
						public static LocString DESC = "Produces a pleasantly muffled \"whump\" when high-fiving.";
					}

					// Token: 0x020028F5 RID: 10485
					public class KNIT_MAGENTA
					{
						// Token: 0x0400ACB8 RID: 44216
						public static LocString NAME = "Magenta Knit Gloves";

						// Token: 0x0400ACB9 RID: 44217
						public static LocString DESC = "Produces a pleasantly muffled \"whump\" when high-fiving.";
					}

					// Token: 0x020028F6 RID: 10486
					public class SPARKLE_WHITE
					{
						// Token: 0x0400ACBA RID: 44218
						public static LocString NAME = "White Glitter Gloves";

						// Token: 0x0400ACBB RID: 44219
						public static LocString DESC = "Each sequin was attached using sealant borrowed from the rocketry department.";
					}

					// Token: 0x020028F7 RID: 10487
					public class GINCH_PINK_SALTROCK
					{
						// Token: 0x0400ACBC RID: 44220
						public static LocString NAME = "Frilly Saltrock Gloves";

						// Token: 0x0400ACBD RID: 44221
						public static LocString DESC = "Thick, soft pink gloves with added flounce.";
					}

					// Token: 0x020028F8 RID: 10488
					public class GINCH_PURPLE_DUSKY
					{
						// Token: 0x0400ACBE RID: 44222
						public static LocString NAME = "Frilly Dusk Gloves";

						// Token: 0x0400ACBF RID: 44223
						public static LocString DESC = "Thick, soft purple gloves with added flounce.";
					}

					// Token: 0x020028F9 RID: 10489
					public class GINCH_BLUE_BASIN
					{
						// Token: 0x0400ACC0 RID: 44224
						public static LocString NAME = "Frilly Basin Gloves";

						// Token: 0x0400ACC1 RID: 44225
						public static LocString DESC = "Thick, soft blue gloves with added flounce.";
					}

					// Token: 0x020028FA RID: 10490
					public class GINCH_TEAL_BALMY
					{
						// Token: 0x0400ACC2 RID: 44226
						public static LocString NAME = "Frilly Balm Gloves";

						// Token: 0x0400ACC3 RID: 44227
						public static LocString DESC = "The soft teal fabric soothes hard-working hands.";
					}

					// Token: 0x020028FB RID: 10491
					public class GINCH_GREEN_LIME
					{
						// Token: 0x0400ACC4 RID: 44228
						public static LocString NAME = "Frilly Leach Gloves";

						// Token: 0x0400ACC5 RID: 44229
						public static LocString DESC = "Thick, soft green gloves with added flounce.";
					}

					// Token: 0x020028FC RID: 10492
					public class GINCH_YELLOW_YELLOWCAKE
					{
						// Token: 0x0400ACC6 RID: 44230
						public static LocString NAME = "Frilly Yellowcake Gloves";

						// Token: 0x0400ACC7 RID: 44231
						public static LocString DESC = "Thick, soft yellow gloves with added flounce.";
					}

					// Token: 0x020028FD RID: 10493
					public class GINCH_ORANGE_ATOMIC
					{
						// Token: 0x0400ACC8 RID: 44232
						public static LocString NAME = "Frilly Atomic Gloves";

						// Token: 0x0400ACC9 RID: 44233
						public static LocString DESC = "Thick, bright orange gloves with added flounce.";
					}

					// Token: 0x020028FE RID: 10494
					public class GINCH_RED_MAGMA
					{
						// Token: 0x0400ACCA RID: 44234
						public static LocString NAME = "Frilly Magma Gloves";

						// Token: 0x0400ACCB RID: 44235
						public static LocString DESC = "Thick, soft red gloves with added flounce.";
					}

					// Token: 0x020028FF RID: 10495
					public class GINCH_GREY_GREY
					{
						// Token: 0x0400ACCC RID: 44236
						public static LocString NAME = "Frilly Slate Gloves";

						// Token: 0x0400ACCD RID: 44237
						public static LocString DESC = "Thick, soft grey gloves with added flounce.";
					}

					// Token: 0x02002900 RID: 10496
					public class GINCH_GREY_CHARCOAL
					{
						// Token: 0x0400ACCE RID: 44238
						public static LocString NAME = "Frilly Charcoal Gloves";

						// Token: 0x0400ACCF RID: 44239
						public static LocString DESC = "Thick, soft dark grey gloves with added flounce.";
					}
				}
			}

			// Token: 0x02002901 RID: 10497
			public class CLOTHING_TOPS
			{
				// Token: 0x0400ACD0 RID: 44240
				public static LocString NAME = "Default Top";

				// Token: 0x0400ACD1 RID: 44241
				public static LocString DESC = "The default shirt.";

				// Token: 0x02002902 RID: 10498
				public class FACADES
				{
					// Token: 0x02002903 RID: 10499
					public class BASIC_BLUE_MIDDLE
					{
						// Token: 0x0400ACD2 RID: 44242
						public static LocString NAME = "Basic Aqua Shirt";

						// Token: 0x0400ACD3 RID: 44243
						public static LocString DESC = "A nice aqua-blue shirt that goes with everything.";
					}

					// Token: 0x02002904 RID: 10500
					public class BASIC_BLACK
					{
						// Token: 0x0400ACD4 RID: 44244
						public static LocString NAME = "Basic Black Shirt";

						// Token: 0x0400ACD5 RID: 44245
						public static LocString DESC = "A nice black shirt that goes with everything.";
					}

					// Token: 0x02002905 RID: 10501
					public class BASIC_PINK_ORCHID
					{
						// Token: 0x0400ACD6 RID: 44246
						public static LocString NAME = "Basic Bubblegum Shirt";

						// Token: 0x0400ACD7 RID: 44247
						public static LocString DESC = "A nice bubblegum-pink shirt that goes with everything.";
					}

					// Token: 0x02002906 RID: 10502
					public class BASIC_GREEN
					{
						// Token: 0x0400ACD8 RID: 44248
						public static LocString NAME = "Basic Green Shirt";

						// Token: 0x0400ACD9 RID: 44249
						public static LocString DESC = "A nice green shirt that goes with everything.";
					}

					// Token: 0x02002907 RID: 10503
					public class BASIC_ORANGE
					{
						// Token: 0x0400ACDA RID: 44250
						public static LocString NAME = "Basic Orange Shirt";

						// Token: 0x0400ACDB RID: 44251
						public static LocString DESC = "A nice orange shirt that goes with everything.";
					}

					// Token: 0x02002908 RID: 10504
					public class BASIC_PURPLE
					{
						// Token: 0x0400ACDC RID: 44252
						public static LocString NAME = "Basic Purple Shirt";

						// Token: 0x0400ACDD RID: 44253
						public static LocString DESC = "A nice purple shirt that goes with everything.";
					}

					// Token: 0x02002909 RID: 10505
					public class BASIC_RED_BURNT
					{
						// Token: 0x0400ACDE RID: 44254
						public static LocString NAME = "Basic Red Shirt";

						// Token: 0x0400ACDF RID: 44255
						public static LocString DESC = "A nice red shirt that goes with everything.";
					}

					// Token: 0x0200290A RID: 10506
					public class BASIC_WHITE
					{
						// Token: 0x0400ACE0 RID: 44256
						public static LocString NAME = "Basic White Shirt";

						// Token: 0x0400ACE1 RID: 44257
						public static LocString DESC = "A nice white shirt that goes with everything.";
					}

					// Token: 0x0200290B RID: 10507
					public class BASIC_YELLOW
					{
						// Token: 0x0400ACE2 RID: 44258
						public static LocString NAME = "Basic Yellow Shirt";

						// Token: 0x0400ACE3 RID: 44259
						public static LocString DESC = "A nice yellow shirt that goes with everything.";
					}

					// Token: 0x0200290C RID: 10508
					public class RAGLANTOP_DEEPRED
					{
						// Token: 0x0400ACE4 RID: 44260
						public static LocString NAME = "Team Captain T-shirt";

						// Token: 0x0400ACE5 RID: 44261
						public static LocString DESC = "A slightly sweat-stained tee for natural leaders.";
					}

					// Token: 0x0200290D RID: 10509
					public class RAGLANTOP_COBALT
					{
						// Token: 0x0400ACE6 RID: 44262
						public static LocString NAME = "True Blue T-shirt";

						// Token: 0x0400ACE7 RID: 44263
						public static LocString DESC = "A slightly sweat-stained tee for the real team players.";
					}

					// Token: 0x0200290E RID: 10510
					public class RAGLANTOP_FLAMINGO
					{
						// Token: 0x0400ACE8 RID: 44264
						public static LocString NAME = "Pep Rally T-shirt";

						// Token: 0x0400ACE9 RID: 44265
						public static LocString DESC = "A slightly sweat-stained tee to boost team spirits.";
					}

					// Token: 0x0200290F RID: 10511
					public class RAGLANTOP_KELLYGREEN
					{
						// Token: 0x0400ACEA RID: 44266
						public static LocString NAME = "Go Team T-shirt";

						// Token: 0x0400ACEB RID: 44267
						public static LocString DESC = "A slightly sweat-stained tee for cheering from the sidelines.";
					}

					// Token: 0x02002910 RID: 10512
					public class RAGLANTOP_CHARCOAL
					{
						// Token: 0x0400ACEC RID: 44268
						public static LocString NAME = "Underdog T-shirt";

						// Token: 0x0400ACED RID: 44269
						public static LocString DESC = "For those who don't win a lot.";
					}

					// Token: 0x02002911 RID: 10513
					public class RAGLANTOP_LEMON
					{
						// Token: 0x0400ACEE RID: 44270
						public static LocString NAME = "Hype T-shirt";

						// Token: 0x0400ACEF RID: 44271
						public static LocString DESC = "A slightly sweat-stained tee to wear when talking a big game.";
					}

					// Token: 0x02002912 RID: 10514
					public class RAGLANTOP_SATSUMA
					{
						// Token: 0x0400ACF0 RID: 44272
						public static LocString NAME = "Superfan T-shirt";

						// Token: 0x0400ACF1 RID: 44273
						public static LocString DESC = "A slightly sweat-stained tee for the long-time supporter.";
					}

					// Token: 0x02002913 RID: 10515
					public class JELLYPUFFJACKET_BLUEBERRY
					{
						// Token: 0x0400ACF2 RID: 44274
						public static LocString NAME = "Blueberry Jelly Jacket";

						// Token: 0x0400ACF3 RID: 44275
						public static LocString DESC = "It's best to keep jelly-filled puffer jackets away from sharp corners.";
					}

					// Token: 0x02002914 RID: 10516
					public class JELLYPUFFJACKET_GRAPE
					{
						// Token: 0x0400ACF4 RID: 44276
						public static LocString NAME = "Grape Jelly Jacket";

						// Token: 0x0400ACF5 RID: 44277
						public static LocString DESC = "It's best to keep jelly-filled puffer jackets away from sharp corners.";
					}

					// Token: 0x02002915 RID: 10517
					public class JELLYPUFFJACKET_LEMON
					{
						// Token: 0x0400ACF6 RID: 44278
						public static LocString NAME = "Lemon Jelly Jacket";

						// Token: 0x0400ACF7 RID: 44279
						public static LocString DESC = "It's best to keep jelly-filled puffer jackets away from sharp corners.";
					}

					// Token: 0x02002916 RID: 10518
					public class JELLYPUFFJACKET_LIME
					{
						// Token: 0x0400ACF8 RID: 44280
						public static LocString NAME = "Lime Jelly Jacket";

						// Token: 0x0400ACF9 RID: 44281
						public static LocString DESC = "It's best to keep jelly-filled puffer jackets away from sharp corners.";
					}

					// Token: 0x02002917 RID: 10519
					public class JELLYPUFFJACKET_SATSUMA
					{
						// Token: 0x0400ACFA RID: 44282
						public static LocString NAME = "Satsuma Jelly Jacket";

						// Token: 0x0400ACFB RID: 44283
						public static LocString DESC = "It's best to keep jelly-filled puffer jackets away from sharp corners.";
					}

					// Token: 0x02002918 RID: 10520
					public class JELLYPUFFJACKET_STRAWBERRY
					{
						// Token: 0x0400ACFC RID: 44284
						public static LocString NAME = "Strawberry Jelly Jacket";

						// Token: 0x0400ACFD RID: 44285
						public static LocString DESC = "It's best to keep jelly-filled puffer jackets away from sharp corners.";
					}

					// Token: 0x02002919 RID: 10521
					public class JELLYPUFFJACKET_WATERMELON
					{
						// Token: 0x0400ACFE RID: 44286
						public static LocString NAME = "Watermelon Jelly Jacket";

						// Token: 0x0400ACFF RID: 44287
						public static LocString DESC = "It's best to keep jelly-filled puffer jackets away from sharp corners.";
					}

					// Token: 0x0200291A RID: 10522
					public class CIRCUIT_GREEN
					{
						// Token: 0x0400AD00 RID: 44288
						public static LocString NAME = "LED Jacket";

						// Token: 0x0400AD01 RID: 44289
						public static LocString DESC = "For dancing in the dark.";
					}

					// Token: 0x0200291B RID: 10523
					public class TSHIRT_WHITE
					{
						// Token: 0x0400AD02 RID: 44290
						public static LocString NAME = "Classic White Tee";

						// Token: 0x0400AD03 RID: 44291
						public static LocString DESC = "It's practically begging for a big Bog Jelly stain down the front.";
					}

					// Token: 0x0200291C RID: 10524
					public class TSHIRT_MAGENTA
					{
						// Token: 0x0400AD04 RID: 44292
						public static LocString NAME = "Classic Magenta Tee";

						// Token: 0x0400AD05 RID: 44293
						public static LocString DESC = "It will never chafe against delicate inner-elbow skin.";
					}

					// Token: 0x0200291D RID: 10525
					public class ATHLETE
					{
						// Token: 0x0400AD06 RID: 44294
						public static LocString NAME = "Racing Jacket";

						// Token: 0x0400AD07 RID: 44295
						public static LocString DESC = "The epitome of fast fashion.";
					}

					// Token: 0x0200291E RID: 10526
					public class DENIM_BLUE
					{
						// Token: 0x0400AD08 RID: 44296
						public static LocString NAME = "Denim Jacket";

						// Token: 0x0400AD09 RID: 44297
						public static LocString DESC = "The top half of a Canadian tuxedo.";
					}

					// Token: 0x0200291F RID: 10527
					public class GONCH_STRAWBERRY
					{
						// Token: 0x0400AD0A RID: 44298
						public static LocString NAME = "Executive Undershirt";

						// Token: 0x0400AD0B RID: 44299
						public static LocString DESC = "The breathable base layer every power suit needs.";
					}

					// Token: 0x02002920 RID: 10528
					public class GONCH_SATSUMA
					{
						// Token: 0x0400AD0C RID: 44300
						public static LocString NAME = "Underling Undershirt";

						// Token: 0x0400AD0D RID: 44301
						public static LocString DESC = "Extra-absorbent fabric in the underarms to mop up nervous sweat.";
					}

					// Token: 0x02002921 RID: 10529
					public class GONCH_LEMON
					{
						// Token: 0x0400AD0E RID: 44302
						public static LocString NAME = "Groupthink Undershirt";

						// Token: 0x0400AD0F RID: 44303
						public static LocString DESC = "Because the most popular choice is always the right choice.";
					}

					// Token: 0x02002922 RID: 10530
					public class GONCH_LIME
					{
						// Token: 0x0400AD10 RID: 44304
						public static LocString NAME = "Stakeholder Undershirt";

						// Token: 0x0400AD11 RID: 44305
						public static LocString DESC = "Soft against the skin, for those who have skin in the game.";
					}

					// Token: 0x02002923 RID: 10531
					public class GONCH_BLUEBERRY
					{
						// Token: 0x0400AD12 RID: 44306
						public static LocString NAME = "Admin Undershirt";

						// Token: 0x0400AD13 RID: 44307
						public static LocString DESC = "Criminally underappreciated.";
					}

					// Token: 0x02002924 RID: 10532
					public class GONCH_GRAPE
					{
						// Token: 0x0400AD14 RID: 44308
						public static LocString NAME = "Buzzword Undershirt";

						// Token: 0x0400AD15 RID: 44309
						public static LocString DESC = "A value-added vest for touching base and thinking outside the box using best practices ASAP.";
					}

					// Token: 0x02002925 RID: 10533
					public class GONCH_WATERMELON
					{
						// Token: 0x0400AD16 RID: 44310
						public static LocString NAME = "Synergy Undershirt";

						// Token: 0x0400AD17 RID: 44311
						public static LocString DESC = "Asking for it by name often triggers dramatic eye-rolls from bystanders.";
					}

					// Token: 0x02002926 RID: 10534
					public class NERD_BROWN
					{
						// Token: 0x0400AD18 RID: 44312
						public static LocString NAME = "Research Shirt";

						// Token: 0x0400AD19 RID: 44313
						public static LocString DESC = "Comes with a thoughtfully chewed-up ballpoint pen.";
					}

					// Token: 0x02002927 RID: 10535
					public class GI_WHITE
					{
						// Token: 0x0400AD1A RID: 44314
						public static LocString NAME = "Rebel Gi Jacket";

						// Token: 0x0400AD1B RID: 44315
						public static LocString DESC = "The contrasting trim hides stains from messy post-sparring snacks.";
					}

					// Token: 0x02002928 RID: 10536
					public class JACKET_SMOKING_BURGUNDY
					{
						// Token: 0x0400AD1C RID: 44316
						public static LocString NAME = "Donor Jacket";

						// Token: 0x0400AD1D RID: 44317
						public static LocString DESC = "Crafted from the softest, most philanthropic fibers.";
					}

					// Token: 0x02002929 RID: 10537
					public class MECHANIC
					{
						// Token: 0x0400AD1E RID: 44318
						public static LocString NAME = "Engineer Jacket";

						// Token: 0x0400AD1F RID: 44319
						public static LocString DESC = "Designed to withstand the rigors of applied science.";
					}

					// Token: 0x0200292A RID: 10538
					public class VELOUR_BLACK
					{
						// Token: 0x0400AD20 RID: 44320
						public static LocString NAME = "PhD Velour Jacket";

						// Token: 0x0400AD21 RID: 44321
						public static LocString DESC = "A formal jacket for those who are \"not that kind of doctor.\"";
					}

					// Token: 0x0200292B RID: 10539
					public class VELOUR_BLUE
					{
						// Token: 0x0400AD22 RID: 44322
						public static LocString NAME = "Shortwave Velour Jacket";

						// Token: 0x0400AD23 RID: 44323
						public static LocString DESC = "A luxe, pettable jacket paired with a clip-on tie.";
					}

					// Token: 0x0200292C RID: 10540
					public class VELOUR_PINK
					{
						// Token: 0x0400AD24 RID: 44324
						public static LocString NAME = "Gamma Velour Jacket";

						// Token: 0x0400AD25 RID: 44325
						public static LocString DESC = "Some scientists are less shy than others.";
					}

					// Token: 0x0200292D RID: 10541
					public class WAISTCOAT_PINSTRIPE_SLATE
					{
						// Token: 0x0400AD26 RID: 44326
						public static LocString NAME = "Nobel Pinstripe Waistcoat";

						// Token: 0x0400AD27 RID: 44327
						public static LocString DESC = "One must dress for the prize that one wishes to win.";
					}

					// Token: 0x0200292E RID: 10542
					public class WATER
					{
						// Token: 0x0400AD28 RID: 44328
						public static LocString NAME = "HVAC Khaki Shirt";

						// Token: 0x0400AD29 RID: 44329
						public static LocString DESC = "Designed to regulate temperature and humidity.";
					}

					// Token: 0x0200292F RID: 10543
					public class TWEED_PINK_ORCHID
					{
						// Token: 0x0400AD2A RID: 44330
						public static LocString NAME = "Power Brunch Blazer";

						// Token: 0x0400AD2B RID: 44331
						public static LocString DESC = "Winners never quit, quitters never win.";
					}

					// Token: 0x02002930 RID: 10544
					public class DRESS_SLEEVELESS_BOW_BW
					{
						// Token: 0x0400AD2C RID: 44332
						public static LocString NAME = "PhD Dress";

						// Token: 0x0400AD2D RID: 44333
						public static LocString DESC = "Ready for a post-thesis-defense party.";
					}

					// Token: 0x02002931 RID: 10545
					public class BODYSUIT_BALLERINA_PINK
					{
						// Token: 0x0400AD2E RID: 44334
						public static LocString NAME = "Ballet Leotard";

						// Token: 0x0400AD2F RID: 44335
						public static LocString DESC = "Lab-crafted fabric with a level of stretchiness that defies the laws of physics.";
					}

					// Token: 0x02002932 RID: 10546
					public class SOCKSUIT_BEIGE
					{
						// Token: 0x0400AD30 RID: 44336
						public static LocString NAME = "Vintage Sockshirt";

						// Token: 0x0400AD31 RID: 44337
						public static LocString DESC = "Like a sock for the torso. With sleeves.";
					}

					// Token: 0x02002933 RID: 10547
					public class X_SPORCHID
					{
						// Token: 0x0400AD32 RID: 44338
						public static LocString NAME = "Sporefest Sweater";

						// Token: 0x0400AD33 RID: 44339
						public static LocString DESC = "This soft knit can be worn anytime, not just during Zombie Spore season.";
					}

					// Token: 0x02002934 RID: 10548
					public class X1_PINCHAPEPPERNUTBELLS
					{
						// Token: 0x0400AD34 RID: 44340
						public static LocString NAME = "Pinchabell Jacket";

						// Token: 0x0400AD35 RID: 44341
						public static LocString DESC = "The peppernuts jingle just loudly enough to be distracting.";
					}

					// Token: 0x02002935 RID: 10549
					public class POMPOM_SHINEBUGS_PINK_PEPPERNUT
					{
						// Token: 0x0400AD36 RID: 44342
						public static LocString NAME = "Pom Bug Sweater";

						// Token: 0x0400AD37 RID: 44343
						public static LocString DESC = "No Shine Bugs were harmed in the making of this sweater.";
					}

					// Token: 0x02002936 RID: 10550
					public class SNOWFLAKE_BLUE
					{
						// Token: 0x0400AD38 RID: 44344
						public static LocString NAME = "Crystal-Iced Sweater";

						// Token: 0x0400AD39 RID: 44345
						public static LocString DESC = "Tiny imperfections in the front pattern ensure that no two are truly identical.";
					}

					// Token: 0x02002937 RID: 10551
					public class PJ_CLOVERS_GLITCH_KELLY
					{
						// Token: 0x0400AD3A RID: 44346
						public static LocString NAME = "Lucky Jammies";

						// Token: 0x0400AD3B RID: 44347
						public static LocString DESC = "Even the most brilliant minds need a little extra luck sometimes.";
					}

					// Token: 0x02002938 RID: 10552
					public class PJ_HEARTS_CHILLI_STRAWBERRY
					{
						// Token: 0x0400AD3C RID: 44348
						public static LocString NAME = "Sweetheart Jammies";

						// Token: 0x0400AD3D RID: 44349
						public static LocString DESC = "Plush chenille fabric and a drool-absorbent collar? This sleepsuit really <i>is</i> \"The One.\"";
					}

					// Token: 0x02002939 RID: 10553
					public class BUILDER
					{
						// Token: 0x0400AD3E RID: 44350
						public static LocString NAME = "Hi-Vis Jacket";

						// Token: 0x0400AD3F RID: 44351
						public static LocString DESC = "Unmissable style for the safety-minded.";
					}

					// Token: 0x0200293A RID: 10554
					public class FLORAL_PINK
					{
						// Token: 0x0400AD40 RID: 44352
						public static LocString NAME = "Downtime Shirt";

						// Token: 0x0400AD41 RID: 44353
						public static LocString DESC = "For maxing and relaxing when errands are too taxing.";
					}

					// Token: 0x0200293B RID: 10555
					public class GINCH_PINK_SALTROCK
					{
						// Token: 0x0400AD42 RID: 44354
						public static LocString NAME = "Frilly Saltrock Undershirt";

						// Token: 0x0400AD43 RID: 44355
						public static LocString DESC = "A seamless pink undershirt with laser-cut ruffles.";
					}

					// Token: 0x0200293C RID: 10556
					public class GINCH_PURPLE_DUSKY
					{
						// Token: 0x0400AD44 RID: 44356
						public static LocString NAME = "Frilly Dusk Undershirt";

						// Token: 0x0400AD45 RID: 44357
						public static LocString DESC = "A seamless purple undershirt with laser-cut ruffles.";
					}

					// Token: 0x0200293D RID: 10557
					public class GINCH_BLUE_BASIN
					{
						// Token: 0x0400AD46 RID: 44358
						public static LocString NAME = "Frilly Basin Undershirt";

						// Token: 0x0400AD47 RID: 44359
						public static LocString DESC = "A seamless blue undershirt with laser-cut ruffles.";
					}

					// Token: 0x0200293E RID: 10558
					public class GINCH_TEAL_BALMY
					{
						// Token: 0x0400AD48 RID: 44360
						public static LocString NAME = "Frilly Balm Undershirt";

						// Token: 0x0400AD49 RID: 44361
						public static LocString DESC = "A seamless teal undershirt with laser-cut ruffles.";
					}

					// Token: 0x0200293F RID: 10559
					public class GINCH_GREEN_LIME
					{
						// Token: 0x0400AD4A RID: 44362
						public static LocString NAME = "Frilly Leach Undershirt";

						// Token: 0x0400AD4B RID: 44363
						public static LocString DESC = "A seamless green undershirt with laser-cut ruffles.";
					}

					// Token: 0x02002940 RID: 10560
					public class GINCH_YELLOW_YELLOWCAKE
					{
						// Token: 0x0400AD4C RID: 44364
						public static LocString NAME = "Frilly Yellowcake Undershirt";

						// Token: 0x0400AD4D RID: 44365
						public static LocString DESC = "A seamless yellow undershirt with laser-cut ruffles.";
					}

					// Token: 0x02002941 RID: 10561
					public class GINCH_ORANGE_ATOMIC
					{
						// Token: 0x0400AD4E RID: 44366
						public static LocString NAME = "Frilly Atomic Undershirt";

						// Token: 0x0400AD4F RID: 44367
						public static LocString DESC = "A seamless orange undershirt with laser-cut ruffles.";
					}

					// Token: 0x02002942 RID: 10562
					public class GINCH_RED_MAGMA
					{
						// Token: 0x0400AD50 RID: 44368
						public static LocString NAME = "Frilly Magma Undershirt";

						// Token: 0x0400AD51 RID: 44369
						public static LocString DESC = "A seamless red undershirt with laser-cut ruffles.";
					}

					// Token: 0x02002943 RID: 10563
					public class GINCH_GREY_GREY
					{
						// Token: 0x0400AD52 RID: 44370
						public static LocString NAME = "Frilly Slate Undershirt";

						// Token: 0x0400AD53 RID: 44371
						public static LocString DESC = "A seamless grey undershirt with laser-cut ruffles.";
					}

					// Token: 0x02002944 RID: 10564
					public class GINCH_GREY_CHARCOAL
					{
						// Token: 0x0400AD54 RID: 44372
						public static LocString NAME = "Frilly Charcoal Undershirt";

						// Token: 0x0400AD55 RID: 44373
						public static LocString DESC = "A seamless dark grey undershirt with laser-cut ruffles.";
					}

					// Token: 0x02002945 RID: 10565
					public class KNIT_POLKADOT_TURQ
					{
						// Token: 0x0400AD56 RID: 44374
						public static LocString NAME = "Polka Dot Track Jacket";

						// Token: 0x0400AD57 RID: 44375
						public static LocString DESC = "The dots are infused with odor-neutralizing enzymes!";
					}

					// Token: 0x02002946 RID: 10566
					public class FLASHY
					{
						// Token: 0x0400AD58 RID: 44376
						public static LocString NAME = "Superstar Jacket";

						// Token: 0x0400AD59 RID: 44377
						public static LocString DESC = "Some of us were not made to be subtle.";
					}
				}
			}

			// Token: 0x02002947 RID: 10567
			public class CLOTHING_BOTTOMS
			{
				// Token: 0x0400AD5A RID: 44378
				public static LocString NAME = "Default Bottom";

				// Token: 0x0400AD5B RID: 44379
				public static LocString DESC = "The default bottoms.";

				// Token: 0x02002948 RID: 10568
				public class FACADES
				{
					// Token: 0x02002949 RID: 10569
					public class BASIC_BLUE_MIDDLE
					{
						// Token: 0x0400AD5C RID: 44380
						public static LocString NAME = "Basic Aqua Pants";

						// Token: 0x0400AD5D RID: 44381
						public static LocString DESC = "A clean pair of aqua-blue pants that go with everything.";
					}

					// Token: 0x0200294A RID: 10570
					public class BASIC_PINK_ORCHID
					{
						// Token: 0x0400AD5E RID: 44382
						public static LocString NAME = "Basic Bubblegum Pants";

						// Token: 0x0400AD5F RID: 44383
						public static LocString DESC = "A clean pair of bubblegum-pink pants that go with everything.";
					}

					// Token: 0x0200294B RID: 10571
					public class BASIC_GREEN
					{
						// Token: 0x0400AD60 RID: 44384
						public static LocString NAME = "Basic Green Pants";

						// Token: 0x0400AD61 RID: 44385
						public static LocString DESC = "A clean pair of green pants that go with everything.";
					}

					// Token: 0x0200294C RID: 10572
					public class BASIC_ORANGE
					{
						// Token: 0x0400AD62 RID: 44386
						public static LocString NAME = "Basic Orange Pants";

						// Token: 0x0400AD63 RID: 44387
						public static LocString DESC = "A clean pair of orange pants that go with everything.";
					}

					// Token: 0x0200294D RID: 10573
					public class BASIC_PURPLE
					{
						// Token: 0x0400AD64 RID: 44388
						public static LocString NAME = "Basic Purple Pants";

						// Token: 0x0400AD65 RID: 44389
						public static LocString DESC = "A clean pair of purple pants that go with everything.";
					}

					// Token: 0x0200294E RID: 10574
					public class BASIC_RED
					{
						// Token: 0x0400AD66 RID: 44390
						public static LocString NAME = "Basic Red Pants";

						// Token: 0x0400AD67 RID: 44391
						public static LocString DESC = "A clean pair of red pants that go with everything.";
					}

					// Token: 0x0200294F RID: 10575
					public class BASIC_WHITE
					{
						// Token: 0x0400AD68 RID: 44392
						public static LocString NAME = "Basic White Pants";

						// Token: 0x0400AD69 RID: 44393
						public static LocString DESC = "A clean pair of white pants that go with everything.";
					}

					// Token: 0x02002950 RID: 10576
					public class BASIC_YELLOW
					{
						// Token: 0x0400AD6A RID: 44394
						public static LocString NAME = "Basic Yellow Pants";

						// Token: 0x0400AD6B RID: 44395
						public static LocString DESC = "A clean pair of yellow pants that go with everything.";
					}

					// Token: 0x02002951 RID: 10577
					public class BASIC_BLACK
					{
						// Token: 0x0400AD6C RID: 44396
						public static LocString NAME = "Basic Black Pants";

						// Token: 0x0400AD6D RID: 44397
						public static LocString DESC = "A clean pair of black pants that go with everything.";
					}

					// Token: 0x02002952 RID: 10578
					public class SHORTS_BASIC_DEEPRED
					{
						// Token: 0x0400AD6E RID: 44398
						public static LocString NAME = "Team Captain Shorts";

						// Token: 0x0400AD6F RID: 44399
						public static LocString DESC = "A fresh pair of shorts for natural leaders.";
					}

					// Token: 0x02002953 RID: 10579
					public class SHORTS_BASIC_SATSUMA
					{
						// Token: 0x0400AD70 RID: 44400
						public static LocString NAME = "Superfan Shorts";

						// Token: 0x0400AD71 RID: 44401
						public static LocString DESC = "A fresh pair of shorts for long-time supporters of...shorts.";
					}

					// Token: 0x02002954 RID: 10580
					public class SHORTS_BASIC_YELLOWCAKE
					{
						// Token: 0x0400AD72 RID: 44402
						public static LocString NAME = "Yellowcake Shorts";

						// Token: 0x0400AD73 RID: 44403
						public static LocString DESC = "A fresh pair of uranium-powder-colored shorts that are definitely not radioactive. Probably.";
					}

					// Token: 0x02002955 RID: 10581
					public class SHORTS_BASIC_KELLYGREEN
					{
						// Token: 0x0400AD74 RID: 44404
						public static LocString NAME = "Go Team Shorts";

						// Token: 0x0400AD75 RID: 44405
						public static LocString DESC = "A fresh pair of shorts for cheering from the sidelines.";
					}

					// Token: 0x02002956 RID: 10582
					public class SHORTS_BASIC_BLUE_COBALT
					{
						// Token: 0x0400AD76 RID: 44406
						public static LocString NAME = "True Blue Shorts";

						// Token: 0x0400AD77 RID: 44407
						public static LocString DESC = "A fresh pair of shorts for the real team players.";
					}

					// Token: 0x02002957 RID: 10583
					public class SHORTS_BASIC_PINK_FLAMINGO
					{
						// Token: 0x0400AD78 RID: 44408
						public static LocString NAME = "Pep Rally Shorts";

						// Token: 0x0400AD79 RID: 44409
						public static LocString DESC = "The peppiest pair of shorts this side of the asteroid.";
					}

					// Token: 0x02002958 RID: 10584
					public class SHORTS_BASIC_CHARCOAL
					{
						// Token: 0x0400AD7A RID: 44410
						public static LocString NAME = "Underdog Shorts";

						// Token: 0x0400AD7B RID: 44411
						public static LocString DESC = "A fresh pair of shorts. They're cleaner than they look.";
					}

					// Token: 0x02002959 RID: 10585
					public class CIRCUIT_GREEN
					{
						// Token: 0x0400AD7C RID: 44412
						public static LocString NAME = "LED Pants";

						// Token: 0x0400AD7D RID: 44413
						public static LocString DESC = "These legs are lit.";
					}

					// Token: 0x0200295A RID: 10586
					public class ATHLETE
					{
						// Token: 0x0400AD7E RID: 44414
						public static LocString NAME = "Racing Pants";

						// Token: 0x0400AD7F RID: 44415
						public static LocString DESC = "Fast, furious fashion.";
					}

					// Token: 0x0200295B RID: 10587
					public class BASIC_LIGHTBROWN
					{
						// Token: 0x0400AD80 RID: 44416
						public static LocString NAME = "Basic Khaki Pants";

						// Token: 0x0400AD81 RID: 44417
						public static LocString DESC = "Transition effortlessly from subterranean day to subterranean night.";
					}

					// Token: 0x0200295C RID: 10588
					public class BASIC_REDORANGE
					{
						// Token: 0x0400AD82 RID: 44418
						public static LocString NAME = "Basic Crimson Pants";

						// Token: 0x0400AD83 RID: 44419
						public static LocString DESC = "Like red pants, but slightly fancier-sounding.";
					}

					// Token: 0x0200295D RID: 10589
					public class GONCH_STRAWBERRY
					{
						// Token: 0x0400AD84 RID: 44420
						public static LocString NAME = "Executive Briefs";

						// Token: 0x0400AD85 RID: 44421
						public static LocString DESC = "Bossy (under)pants.";
					}

					// Token: 0x0200295E RID: 10590
					public class GONCH_SATSUMA
					{
						// Token: 0x0400AD86 RID: 44422
						public static LocString NAME = "Underling Briefs";

						// Token: 0x0400AD87 RID: 44423
						public static LocString DESC = "The seams are already unraveling.";
					}

					// Token: 0x0200295F RID: 10591
					public class GONCH_LEMON
					{
						// Token: 0x0400AD88 RID: 44424
						public static LocString NAME = "Groupthink Briefs";

						// Token: 0x0400AD89 RID: 44425
						public static LocString DESC = "All the cool people are wearing them.";
					}

					// Token: 0x02002960 RID: 10592
					public class GONCH_LIME
					{
						// Token: 0x0400AD8A RID: 44426
						public static LocString NAME = "Stakeholder Briefs";

						// Token: 0x0400AD8B RID: 44427
						public static LocString DESC = "They're really invested in keeping the wearer comfortable.";
					}

					// Token: 0x02002961 RID: 10593
					public class GONCH_BLUEBERRY
					{
						// Token: 0x0400AD8C RID: 44428
						public static LocString NAME = "Admin Briefs";

						// Token: 0x0400AD8D RID: 44429
						public static LocString DESC = "The workhorse of the underwear world.";
					}

					// Token: 0x02002962 RID: 10594
					public class GONCH_GRAPE
					{
						// Token: 0x0400AD8E RID: 44430
						public static LocString NAME = "Buzzword Briefs";

						// Token: 0x0400AD8F RID: 44431
						public static LocString DESC = "Underwear that works hard, plays hard, and gives 110% to maximize the \"bottom\" line.";
					}

					// Token: 0x02002963 RID: 10595
					public class GONCH_WATERMELON
					{
						// Token: 0x0400AD90 RID: 44432
						public static LocString NAME = "Synergy Briefs";

						// Token: 0x0400AD91 RID: 44433
						public static LocString DESC = "Teamwork makes the dream work.";
					}

					// Token: 0x02002964 RID: 10596
					public class DENIM_BLUE
					{
						// Token: 0x0400AD92 RID: 44434
						public static LocString NAME = "Jeans";

						// Token: 0x0400AD93 RID: 44435
						public static LocString DESC = "The bottom half of a Canadian tuxedo.";
					}

					// Token: 0x02002965 RID: 10597
					public class GI_WHITE
					{
						// Token: 0x0400AD94 RID: 44436
						public static LocString NAME = "White Capris";

						// Token: 0x0400AD95 RID: 44437
						public static LocString DESC = "The cropped length is ideal for wading through flooded hallways.";
					}

					// Token: 0x02002966 RID: 10598
					public class NERD_BROWN
					{
						// Token: 0x0400AD96 RID: 44438
						public static LocString NAME = "Research Pants";

						// Token: 0x0400AD97 RID: 44439
						public static LocString DESC = "The pockets are full of illegible notes that didn't quite survive the wash.";
					}

					// Token: 0x02002967 RID: 10599
					public class SKIRT_BASIC_BLUE_MIDDLE
					{
						// Token: 0x0400AD98 RID: 44440
						public static LocString NAME = "Aqua Rayon Skirt";

						// Token: 0x0400AD99 RID: 44441
						public static LocString DESC = "The tag says \"Dry Clean Only.\" There are no dry cleaners in space.";
					}

					// Token: 0x02002968 RID: 10600
					public class SKIRT_BASIC_PURPLE
					{
						// Token: 0x0400AD9A RID: 44442
						public static LocString NAME = "Purple Rayon Skirt";

						// Token: 0x0400AD9B RID: 44443
						public static LocString DESC = "It's not the most breathable fabric, but it <i>is</i> a lovely shade of purple.";
					}

					// Token: 0x02002969 RID: 10601
					public class SKIRT_BASIC_GREEN
					{
						// Token: 0x0400AD9C RID: 44444
						public static LocString NAME = "Olive Rayon Skirt";

						// Token: 0x0400AD9D RID: 44445
						public static LocString DESC = "Designed not to get snagged on ladders.";
					}

					// Token: 0x0200296A RID: 10602
					public class SKIRT_BASIC_ORANGE
					{
						// Token: 0x0400AD9E RID: 44446
						public static LocString NAME = "Apricot Rayon Skirt";

						// Token: 0x0400AD9F RID: 44447
						public static LocString DESC = "Ready for spontaneous workplace twirling.";
					}

					// Token: 0x0200296B RID: 10603
					public class SKIRT_BASIC_PINK_ORCHID
					{
						// Token: 0x0400ADA0 RID: 44448
						public static LocString NAME = "Bubblegum Rayon Skirt";

						// Token: 0x0400ADA1 RID: 44449
						public static LocString DESC = "The bubblegum scent lasts 100 washes!";
					}

					// Token: 0x0200296C RID: 10604
					public class SKIRT_BASIC_RED
					{
						// Token: 0x0400ADA2 RID: 44450
						public static LocString NAME = "Garnet Rayon Skirt";

						// Token: 0x0400ADA3 RID: 44451
						public static LocString DESC = "It's business time.";
					}

					// Token: 0x0200296D RID: 10605
					public class SKIRT_BASIC_YELLOW
					{
						// Token: 0x0400ADA4 RID: 44452
						public static LocString NAME = "Yellow Rayon Skirt";

						// Token: 0x0400ADA5 RID: 44453
						public static LocString DESC = "A formerly white skirt that has not aged well.";
					}

					// Token: 0x0200296E RID: 10606
					public class SKIRT_BASIC_POLKADOT
					{
						// Token: 0x0400ADA6 RID: 44454
						public static LocString NAME = "Polka Dot Skirt";

						// Token: 0x0400ADA7 RID: 44455
						public static LocString DESC = "Polka dots are a way to infinity.";
					}

					// Token: 0x0200296F RID: 10607
					public class SKIRT_BASIC_WATERMELON
					{
						// Token: 0x0400ADA8 RID: 44456
						public static LocString NAME = "Picnic Skirt";

						// Token: 0x0400ADA9 RID: 44457
						public static LocString DESC = "The seeds are spittable, but will bear no fruit.";
					}

					// Token: 0x02002970 RID: 10608
					public class SKIRT_DENIM_BLUE
					{
						// Token: 0x0400ADAA RID: 44458
						public static LocString NAME = "Denim Tux Skirt";

						// Token: 0x0400ADAB RID: 44459
						public static LocString DESC = "Designed for the casual red carpet.";
					}

					// Token: 0x02002971 RID: 10609
					public class SKIRT_LEOPARD_PRINT_BLUE_PINK
					{
						// Token: 0x0400ADAC RID: 44460
						public static LocString NAME = "Disco Leopard Skirt";

						// Token: 0x0400ADAD RID: 44461
						public static LocString DESC = "A faux-fur party staple.";
					}

					// Token: 0x02002972 RID: 10610
					public class SKIRT_SPARKLE_BLUE
					{
						// Token: 0x0400ADAE RID: 44462
						public static LocString NAME = "Blue Tinsel Skirt";

						// Token: 0x0400ADAF RID: 44463
						public static LocString DESC = "The tinsel is scratchy, but look how shiny!";
					}

					// Token: 0x02002973 RID: 10611
					public class BASIC_ORANGE_SATSUMA
					{
						// Token: 0x0400ADB0 RID: 44464
						public static LocString NAME = "Hi-Vis Pants";

						// Token: 0x0400ADB1 RID: 44465
						public static LocString DESC = "They make the wearer feel truly seen.";
					}

					// Token: 0x02002974 RID: 10612
					public class PINSTRIPE_SLATE
					{
						// Token: 0x0400ADB2 RID: 44466
						public static LocString NAME = "Nobel Pinstripe Trousers";

						// Token: 0x0400ADB3 RID: 44467
						public static LocString DESC = "There's a waterproof pocket to keep acceptance speeches smudge-free.";
					}

					// Token: 0x02002975 RID: 10613
					public class VELOUR_BLACK
					{
						// Token: 0x0400ADB4 RID: 44468
						public static LocString NAME = "Black Velour Trousers";

						// Token: 0x0400ADB5 RID: 44469
						public static LocString DESC = "Fuzzy, formal and finely cut.";
					}

					// Token: 0x02002976 RID: 10614
					public class VELOUR_BLUE
					{
						// Token: 0x0400ADB6 RID: 44470
						public static LocString NAME = "Shortwave Velour Pants";

						// Token: 0x0400ADB7 RID: 44471
						public static LocString DESC = "Formal wear with a sensory side.";
					}

					// Token: 0x02002977 RID: 10615
					public class VELOUR_PINK
					{
						// Token: 0x0400ADB8 RID: 44472
						public static LocString NAME = "Gamma Velour Pants";

						// Token: 0x0400ADB9 RID: 44473
						public static LocString DESC = "They're stretchy <i>and</i> flame retardant.";
					}

					// Token: 0x02002978 RID: 10616
					public class SKIRT_BALLERINA_PINK
					{
						// Token: 0x0400ADBA RID: 44474
						public static LocString NAME = "Ballet Tutu";

						// Token: 0x0400ADBB RID: 44475
						public static LocString DESC = "A tulle skirt spun and assembled by an army of patent-pending nanobots.";
					}

					// Token: 0x02002979 RID: 10617
					public class SKIRT_TWEED_PINK_ORCHID
					{
						// Token: 0x0400ADBC RID: 44476
						public static LocString NAME = "Power Brunch Skirt";

						// Token: 0x0400ADBD RID: 44477
						public static LocString DESC = "It has pockets!";
					}

					// Token: 0x0200297A RID: 10618
					public class GINCH_PINK_GLUON
					{
						// Token: 0x0400ADBE RID: 44478
						public static LocString NAME = "Gluon Shorties";

						// Token: 0x0400ADBF RID: 44479
						public static LocString DESC = "Comfy pink short-shorts with a ruffled hem.";
					}

					// Token: 0x0200297B RID: 10619
					public class GINCH_PURPLE_CORTEX
					{
						// Token: 0x0400ADC0 RID: 44480
						public static LocString NAME = "Cortex Shorties";

						// Token: 0x0400ADC1 RID: 44481
						public static LocString DESC = "Comfy purple short-shorts with a ruffled hem.";
					}

					// Token: 0x0200297C RID: 10620
					public class GINCH_BLUE_FROSTY
					{
						// Token: 0x0400ADC2 RID: 44482
						public static LocString NAME = "Frosty Shorties";

						// Token: 0x0400ADC3 RID: 44483
						public static LocString DESC = "Icy blue short-shorts with a ruffled hem.";
					}

					// Token: 0x0200297D RID: 10621
					public class GINCH_TEAL_LOCUS
					{
						// Token: 0x0400ADC4 RID: 44484
						public static LocString NAME = "Locus Shorties";

						// Token: 0x0400ADC5 RID: 44485
						public static LocString DESC = "Comfy teal short-shorts with a ruffled hem.";
					}

					// Token: 0x0200297E RID: 10622
					public class GINCH_GREEN_GOOP
					{
						// Token: 0x0400ADC6 RID: 44486
						public static LocString NAME = "Goop Shorties";

						// Token: 0x0400ADC7 RID: 44487
						public static LocString DESC = "Short-shorts with a ruffled hem and one pocket full of melted snacks.";
					}

					// Token: 0x0200297F RID: 10623
					public class GINCH_YELLOW_BILE
					{
						// Token: 0x0400ADC8 RID: 44488
						public static LocString NAME = "Bile Shorties";

						// Token: 0x0400ADC9 RID: 44489
						public static LocString DESC = "Ruffled short-shorts in a stomach-turning shade of yellow.";
					}

					// Token: 0x02002980 RID: 10624
					public class GINCH_ORANGE_NYBBLE
					{
						// Token: 0x0400ADCA RID: 44490
						public static LocString NAME = "Nybble Shorties";

						// Token: 0x0400ADCB RID: 44491
						public static LocString DESC = "Comfy orange ruffled short-shorts for computer scientists.";
					}

					// Token: 0x02002981 RID: 10625
					public class GINCH_RED_IRONBOW
					{
						// Token: 0x0400ADCC RID: 44492
						public static LocString NAME = "Ironbow Shorties";

						// Token: 0x0400ADCD RID: 44493
						public static LocString DESC = "Comfy red short-shorts with a ruffled hem.";
					}

					// Token: 0x02002982 RID: 10626
					public class GINCH_GREY_PHLEGM
					{
						// Token: 0x0400ADCE RID: 44494
						public static LocString NAME = "Phlegmy Shorties";

						// Token: 0x0400ADCF RID: 44495
						public static LocString DESC = "Ruffled short-shorts in a rather sticky shade of light grey.";
					}

					// Token: 0x02002983 RID: 10627
					public class GINCH_GREY_OBELUS
					{
						// Token: 0x0400ADD0 RID: 44496
						public static LocString NAME = "Obelus Shorties";

						// Token: 0x0400ADD1 RID: 44497
						public static LocString DESC = "Comfy grey short-shorts with a ruffled hem.";
					}

					// Token: 0x02002984 RID: 10628
					public class KNIT_POLKADOT_TURQ
					{
						// Token: 0x0400ADD2 RID: 44498
						public static LocString NAME = "Polka Dot Track Pants";

						// Token: 0x0400ADD3 RID: 44499
						public static LocString DESC = "For clowning around during mandatory physical fitness week.";
					}

					// Token: 0x02002985 RID: 10629
					public class GI_BELT_WHITE_BLACK
					{
						// Token: 0x0400ADD4 RID: 44500
						public static LocString NAME = "Rebel Gi Pants";

						// Token: 0x0400ADD5 RID: 44501
						public static LocString DESC = "Relaxed-fit pants designed for roundhouse kicks.";
					}

					// Token: 0x02002986 RID: 10630
					public class BELT_KHAKI_TAN
					{
						// Token: 0x0400ADD6 RID: 44502
						public static LocString NAME = "HVAC Khaki Pants";

						// Token: 0x0400ADD7 RID: 44503
						public static LocString DESC = "Rip-resistant fabric makes crawling through ducts a breeze.";
					}
				}
			}

			// Token: 0x02002987 RID: 10631
			public class CLOTHING_SHOES
			{
				// Token: 0x0400ADD8 RID: 44504
				public static LocString NAME = "Default Footwear";

				// Token: 0x0400ADD9 RID: 44505
				public static LocString DESC = "The default style of footwear.";

				// Token: 0x02002988 RID: 10632
				public class FACADES
				{
					// Token: 0x02002989 RID: 10633
					public class BASIC_BLUE_MIDDLE
					{
						// Token: 0x0400ADDA RID: 44506
						public static LocString NAME = "Basic Aqua Shoes";

						// Token: 0x0400ADDB RID: 44507
						public static LocString DESC = "A fresh pair of aqua-blue shoes that go with everything.";
					}

					// Token: 0x0200298A RID: 10634
					public class BASIC_PINK_ORCHID
					{
						// Token: 0x0400ADDC RID: 44508
						public static LocString NAME = "Basic Bubblegum Shoes";

						// Token: 0x0400ADDD RID: 44509
						public static LocString DESC = "A fresh pair of bubblegum-pink shoes that go with everything.";
					}

					// Token: 0x0200298B RID: 10635
					public class BASIC_GREEN
					{
						// Token: 0x0400ADDE RID: 44510
						public static LocString NAME = "Basic Green Shoes";

						// Token: 0x0400ADDF RID: 44511
						public static LocString DESC = "A fresh pair of green shoes that go with everything.";
					}

					// Token: 0x0200298C RID: 10636
					public class BASIC_ORANGE
					{
						// Token: 0x0400ADE0 RID: 44512
						public static LocString NAME = "Basic Orange Shoes";

						// Token: 0x0400ADE1 RID: 44513
						public static LocString DESC = "A fresh pair of orange shoes that go with everything.";
					}

					// Token: 0x0200298D RID: 10637
					public class BASIC_PURPLE
					{
						// Token: 0x0400ADE2 RID: 44514
						public static LocString NAME = "Basic Purple Shoes";

						// Token: 0x0400ADE3 RID: 44515
						public static LocString DESC = "A fresh pair of purple shoes that go with everything.";
					}

					// Token: 0x0200298E RID: 10638
					public class BASIC_RED
					{
						// Token: 0x0400ADE4 RID: 44516
						public static LocString NAME = "Basic Red Shoes";

						// Token: 0x0400ADE5 RID: 44517
						public static LocString DESC = "A fresh pair of red shoes that go with everything.";
					}

					// Token: 0x0200298F RID: 10639
					public class BASIC_WHITE
					{
						// Token: 0x0400ADE6 RID: 44518
						public static LocString NAME = "Basic White Shoes";

						// Token: 0x0400ADE7 RID: 44519
						public static LocString DESC = "A fresh pair of white shoes that go with everything.";
					}

					// Token: 0x02002990 RID: 10640
					public class BASIC_YELLOW
					{
						// Token: 0x0400ADE8 RID: 44520
						public static LocString NAME = "Basic Yellow Shoes";

						// Token: 0x0400ADE9 RID: 44521
						public static LocString DESC = "A fresh pair of yellow shoes that go with everything.";
					}

					// Token: 0x02002991 RID: 10641
					public class BASIC_BLACK
					{
						// Token: 0x0400ADEA RID: 44522
						public static LocString NAME = "Basic Black Shoes";

						// Token: 0x0400ADEB RID: 44523
						public static LocString DESC = "A fresh pair of black shoes that go with everything.";
					}

					// Token: 0x02002992 RID: 10642
					public class BASIC_BLUEGREY
					{
						// Token: 0x0400ADEC RID: 44524
						public static LocString NAME = "Basic Gunmetal Shoes";

						// Token: 0x0400ADED RID: 44525
						public static LocString DESC = "A fresh pair of pastel shoes that go with everything.";
					}

					// Token: 0x02002993 RID: 10643
					public class BASIC_TAN
					{
						// Token: 0x0400ADEE RID: 44526
						public static LocString NAME = "Basic Tan Shoes";

						// Token: 0x0400ADEF RID: 44527
						public static LocString DESC = "They're remarkably unremarkable.";
					}

					// Token: 0x02002994 RID: 10644
					public class SOCKS_ATHLETIC_DEEPRED
					{
						// Token: 0x0400ADF0 RID: 44528
						public static LocString NAME = "Team Captain Gym Socks";

						// Token: 0x0400ADF1 RID: 44529
						public static LocString DESC = "Breathable socks with sporty red stripes.";
					}

					// Token: 0x02002995 RID: 10645
					public class SOCKS_ATHLETIC_SATSUMA
					{
						// Token: 0x0400ADF2 RID: 44530
						public static LocString NAME = "Superfan Gym Socks";

						// Token: 0x0400ADF3 RID: 44531
						public static LocString DESC = "Breathable socks with sporty orange stripes.";
					}

					// Token: 0x02002996 RID: 10646
					public class SOCKS_ATHLETIC_LEMON
					{
						// Token: 0x0400ADF4 RID: 44532
						public static LocString NAME = "Hype Gym Socks";

						// Token: 0x0400ADF5 RID: 44533
						public static LocString DESC = "Breathable socks with sporty yellow stripes.";
					}

					// Token: 0x02002997 RID: 10647
					public class SOCKS_ATHLETIC_KELLYGREEN
					{
						// Token: 0x0400ADF6 RID: 44534
						public static LocString NAME = "Go Team Gym Socks";

						// Token: 0x0400ADF7 RID: 44535
						public static LocString DESC = "Breathable socks with sporty green stripes.";
					}

					// Token: 0x02002998 RID: 10648
					public class SOCKS_ATHLETIC_COBALT
					{
						// Token: 0x0400ADF8 RID: 44536
						public static LocString NAME = "True Blue Gym Socks";

						// Token: 0x0400ADF9 RID: 44537
						public static LocString DESC = "Breathable socks with sporty blue stripes.";
					}

					// Token: 0x02002999 RID: 10649
					public class SOCKS_ATHLETIC_FLAMINGO
					{
						// Token: 0x0400ADFA RID: 44538
						public static LocString NAME = "Pep Rally Gym Socks";

						// Token: 0x0400ADFB RID: 44539
						public static LocString DESC = "Breathable socks with sporty pink stripes.";
					}

					// Token: 0x0200299A RID: 10650
					public class SOCKS_ATHLETIC_CHARCOAL
					{
						// Token: 0x0400ADFC RID: 44540
						public static LocString NAME = "Underdog Gym Socks";

						// Token: 0x0400ADFD RID: 44541
						public static LocString DESC = "Breathable socks that do nothing whatsoever to eliminate foot odor.";
					}

					// Token: 0x0200299B RID: 10651
					public class BASIC_GREY
					{
						// Token: 0x0400ADFE RID: 44542
						public static LocString NAME = "Basic Gray Shoes";

						// Token: 0x0400ADFF RID: 44543
						public static LocString DESC = "A fresh pair of gray shoes that go with everything.";
					}

					// Token: 0x0200299C RID: 10652
					public class DENIM_BLUE
					{
						// Token: 0x0400AE00 RID: 44544
						public static LocString NAME = "Denim Shoes";

						// Token: 0x0400AE01 RID: 44545
						public static LocString DESC = "Not technically essential for a Canadian tuxedo, but why not?";
					}

					// Token: 0x0200299D RID: 10653
					public class LEGWARMERS_STRAWBERRY
					{
						// Token: 0x0400AE02 RID: 44546
						public static LocString NAME = "Slouchy Strawberry Socks";

						// Token: 0x0400AE03 RID: 44547
						public static LocString DESC = "Freckly knitted socks that don't stay up.";
					}

					// Token: 0x0200299E RID: 10654
					public class LEGWARMERS_SATSUMA
					{
						// Token: 0x0400AE04 RID: 44548
						public static LocString NAME = "Slouchy Satsuma Socks";

						// Token: 0x0400AE05 RID: 44549
						public static LocString DESC = "Sweet knitted socks for spontaneous dance segments.";
					}

					// Token: 0x0200299F RID: 10655
					public class LEGWARMERS_LEMON
					{
						// Token: 0x0400AE06 RID: 44550
						public static LocString NAME = "Slouchy Lemon Socks";

						// Token: 0x0400AE07 RID: 44551
						public static LocString DESC = "Zesty knitted socks that don't stay up.";
					}

					// Token: 0x020029A0 RID: 10656
					public class LEGWARMERS_LIME
					{
						// Token: 0x0400AE08 RID: 44552
						public static LocString NAME = "Slouchy Lime Socks";

						// Token: 0x0400AE09 RID: 44553
						public static LocString DESC = "Juicy knitted socks that don't stay up.";
					}

					// Token: 0x020029A1 RID: 10657
					public class LEGWARMERS_BLUEBERRY
					{
						// Token: 0x0400AE0A RID: 44554
						public static LocString NAME = "Slouchy Blueberry Socks";

						// Token: 0x0400AE0B RID: 44555
						public static LocString DESC = "Knitted socks with a fun bobble-stitch texture.";
					}

					// Token: 0x020029A2 RID: 10658
					public class LEGWARMERS_GRAPE
					{
						// Token: 0x0400AE0C RID: 44556
						public static LocString NAME = "Slouchy Grape Socks";

						// Token: 0x0400AE0D RID: 44557
						public static LocString DESC = "These fabulous knitted socks that don't stay up are really raisin the bar.";
					}

					// Token: 0x020029A3 RID: 10659
					public class LEGWARMERS_WATERMELON
					{
						// Token: 0x0400AE0E RID: 44558
						public static LocString NAME = "Slouchy Watermelon Socks";

						// Token: 0x0400AE0F RID: 44559
						public static LocString DESC = "Summery knitted socks that don't stay up.";
					}

					// Token: 0x020029A4 RID: 10660
					public class BALLERINA_PINK
					{
						// Token: 0x0400AE10 RID: 44560
						public static LocString NAME = "Ballet Shoes";

						// Token: 0x0400AE11 RID: 44561
						public static LocString DESC = "There's no \"pointe\" in aiming for anything less than perfection.";
					}

					// Token: 0x020029A5 RID: 10661
					public class MARYJANE_SOCKS_BW
					{
						// Token: 0x0400AE12 RID: 44562
						public static LocString NAME = "Frilly Sock Shoes";

						// Token: 0x0400AE13 RID: 44563
						public static LocString DESC = "They add a little <i>je ne sais quoi</i> to everyday lab wear.";
					}

					// Token: 0x020029A6 RID: 10662
					public class CLASSICFLATS_CREAM_CHARCOAL
					{
						// Token: 0x0400AE14 RID: 44564
						public static LocString NAME = "Dressy Shoes";

						// Token: 0x0400AE15 RID: 44565
						public static LocString DESC = "An enduring style, for enduring endless small talk.";
					}

					// Token: 0x020029A7 RID: 10663
					public class VELOUR_BLUE
					{
						// Token: 0x0400AE16 RID: 44566
						public static LocString NAME = "Shortwave Velour Shoes";

						// Token: 0x0400AE17 RID: 44567
						public static LocString DESC = "Not the easiest to keep clean.";
					}

					// Token: 0x020029A8 RID: 10664
					public class VELOUR_PINK
					{
						// Token: 0x0400AE18 RID: 44568
						public static LocString NAME = "Gamma Velour Shoes";

						// Token: 0x0400AE19 RID: 44569
						public static LocString DESC = "Finally, a pair of work-appropriate fuzzy shoes.";
					}

					// Token: 0x020029A9 RID: 10665
					public class VELOUR_BLACK
					{
						// Token: 0x0400AE1A RID: 44570
						public static LocString NAME = "Black Velour Shoes";

						// Token: 0x0400AE1B RID: 44571
						public static LocString DESC = "Matching velour lining gently tickles feet with every step.";
					}

					// Token: 0x020029AA RID: 10666
					public class FLASHY
					{
						// Token: 0x0400AE1C RID: 44572
						public static LocString NAME = "Superstar Shoes";

						// Token: 0x0400AE1D RID: 44573
						public static LocString DESC = "Why walk when you can <i>moon</i>walk?";
					}

					// Token: 0x020029AB RID: 10667
					public class GINCH_PINK_SALTROCK
					{
						// Token: 0x0400AE1E RID: 44574
						public static LocString NAME = "Frilly Saltrock Socks";

						// Token: 0x0400AE1F RID: 44575
						public static LocString DESC = "Thick, soft pink socks with extra flounce.";
					}

					// Token: 0x020029AC RID: 10668
					public class GINCH_PURPLE_DUSKY
					{
						// Token: 0x0400AE20 RID: 44576
						public static LocString NAME = "Frilly Dusk Socks";

						// Token: 0x0400AE21 RID: 44577
						public static LocString DESC = "Thick, soft purple socks with extra flounce.";
					}

					// Token: 0x020029AD RID: 10669
					public class GINCH_BLUE_BASIN
					{
						// Token: 0x0400AE22 RID: 44578
						public static LocString NAME = "Frilly Basin Socks";

						// Token: 0x0400AE23 RID: 44579
						public static LocString DESC = "Thick, soft blue socks with extra flounce.";
					}

					// Token: 0x020029AE RID: 10670
					public class GINCH_TEAL_BALMY
					{
						// Token: 0x0400AE24 RID: 44580
						public static LocString NAME = "Frilly Balm Socks";

						// Token: 0x0400AE25 RID: 44581
						public static LocString DESC = "Thick, soothing teal socks with extra flounce.";
					}

					// Token: 0x020029AF RID: 10671
					public class GINCH_GREEN_LIME
					{
						// Token: 0x0400AE26 RID: 44582
						public static LocString NAME = "Frilly Leach Socks";

						// Token: 0x0400AE27 RID: 44583
						public static LocString DESC = "Thick, soft green socks with extra flounce.";
					}

					// Token: 0x020029B0 RID: 10672
					public class GINCH_YELLOW_YELLOWCAKE
					{
						// Token: 0x0400AE28 RID: 44584
						public static LocString NAME = "Frilly Yellowcake Socks";

						// Token: 0x0400AE29 RID: 44585
						public static LocString DESC = "Dangerously soft yellow socks with extra flounce.";
					}

					// Token: 0x020029B1 RID: 10673
					public class GINCH_ORANGE_ATOMIC
					{
						// Token: 0x0400AE2A RID: 44586
						public static LocString NAME = "Frilly Atomic Socks";

						// Token: 0x0400AE2B RID: 44587
						public static LocString DESC = "Thick, soft orange socks with extra flounce.";
					}

					// Token: 0x020029B2 RID: 10674
					public class GINCH_RED_MAGMA
					{
						// Token: 0x0400AE2C RID: 44588
						public static LocString NAME = "Frilly Magma Socks";

						// Token: 0x0400AE2D RID: 44589
						public static LocString DESC = "Thick, toasty red socks with extra flounce.";
					}

					// Token: 0x020029B3 RID: 10675
					public class GINCH_GREY_GREY
					{
						// Token: 0x0400AE2E RID: 44590
						public static LocString NAME = "Frilly Slate Socks";

						// Token: 0x0400AE2F RID: 44591
						public static LocString DESC = "Thick, soft grey socks with extra flounce.";
					}

					// Token: 0x020029B4 RID: 10676
					public class GINCH_GREY_CHARCOAL
					{
						// Token: 0x0400AE30 RID: 44592
						public static LocString NAME = "Frilly Charcoal Socks";

						// Token: 0x0400AE31 RID: 44593
						public static LocString DESC = "Thick, soft dark grey socks with extra flounce.";
					}
				}
			}

			// Token: 0x020029B5 RID: 10677
			public class CLOTHING_HATS
			{
				// Token: 0x0400AE32 RID: 44594
				public static LocString NAME = "Default Headgear";

				// Token: 0x0400AE33 RID: 44595
				public static LocString DESC = "<DESC>";

				// Token: 0x020029B6 RID: 10678
				public class FACADES
				{
				}
			}

			// Token: 0x020029B7 RID: 10679
			public class CLOTHING_ACCESORIES
			{
				// Token: 0x0400AE34 RID: 44596
				public static LocString NAME = "Default Accessory";

				// Token: 0x0400AE35 RID: 44597
				public static LocString DESC = "<DESC>";

				// Token: 0x020029B8 RID: 10680
				public class FACADES
				{
				}
			}

			// Token: 0x020029B9 RID: 10681
			public class OXYGEN_TANK
			{
				// Token: 0x0400AE36 RID: 44598
				public static LocString NAME = UI.FormatAsLink("Oxygen Tank", "OXYGEN_TANK");

				// Token: 0x0400AE37 RID: 44599
				public static LocString GENERICNAME = "Equipment";

				// Token: 0x0400AE38 RID: 44600
				public static LocString DESC = "It's like a to-go bag for your lungs.";

				// Token: 0x0400AE39 RID: 44601
				public static LocString EFFECT = "Allows Duplicants to breathe in hazardous environments.\n\nDoes not work when submerged in <style=\"liquid\">Liquid</style>.";

				// Token: 0x0400AE3A RID: 44602
				public static LocString RECIPE_DESC = "Allows Duplicants to breathe in hazardous environments.\n\nDoes not work when submerged in <style=\"liquid\">Liquid</style>";
			}

			// Token: 0x020029BA RID: 10682
			public class OXYGEN_TANK_UNDERWATER
			{
				// Token: 0x0400AE3B RID: 44603
				public static LocString NAME = "Oxygen Rebreather";

				// Token: 0x0400AE3C RID: 44604
				public static LocString GENERICNAME = "Equipment";

				// Token: 0x0400AE3D RID: 44605
				public static LocString DESC = "";

				// Token: 0x0400AE3E RID: 44606
				public static LocString EFFECT = "Allows Duplicants to breathe while submerged in <style=\"liquid\">Liquid</style>.\n\nDoes not work outside of liquid.";

				// Token: 0x0400AE3F RID: 44607
				public static LocString RECIPE_DESC = "Allows Duplicants to breathe while submerged in <style=\"liquid\">Liquid</style>.\n\nDoes not work outside of liquid";
			}

			// Token: 0x020029BB RID: 10683
			public class EQUIPPABLEBALLOON
			{
				// Token: 0x0400AE40 RID: 44608
				public static LocString NAME = UI.FormatAsLink("Balloon Friend", "EQUIPPABLEBALLOON");

				// Token: 0x0400AE41 RID: 44609
				public static LocString DESC = "A floating friend to reassure my Duplicants they are so very, very clever.";

				// Token: 0x0400AE42 RID: 44610
				public static LocString EFFECT = "Gives Duplicants a boost in brain function.\n\nSupplied by Duplicants with the Balloon Artist " + UI.FormatAsLink("Overjoyed", "MORALE") + " response.";

				// Token: 0x0400AE43 RID: 44611
				public static LocString RECIPE_DESC = "Gives Duplicants a boost in brain function.\n\nSupplied by Duplicants with the Balloon Artist " + UI.FormatAsLink("Overjoyed", "MORALE") + " response";

				// Token: 0x0400AE44 RID: 44612
				public static LocString GENERICNAME = "Balloon Friend";

				// Token: 0x020029BC RID: 10684
				public class FACADES
				{
					// Token: 0x020029BD RID: 10685
					public class DEFAULT_BALLOON
					{
						// Token: 0x0400AE45 RID: 44613
						public static LocString NAME = UI.FormatAsLink("Balloon Friend", "EQUIPPABLEBALLOON");

						// Token: 0x0400AE46 RID: 44614
						public static LocString DESC = "A floating friend to reassure my Duplicants that they are so very, very clever.";
					}

					// Token: 0x020029BE RID: 10686
					public class BALLOON_FIREENGINE_LONG_SPARKLES
					{
						// Token: 0x0400AE47 RID: 44615
						public static LocString NAME = UI.FormatAsLink("Magma Glitter", "EQUIPPABLEBALLOON");

						// Token: 0x0400AE48 RID: 44616
						public static LocString DESC = "They float <i>and</i> sparkle!";
					}

					// Token: 0x020029BF RID: 10687
					public class BALLOON_YELLOW_LONG_SPARKLES
					{
						// Token: 0x0400AE49 RID: 44617
						public static LocString NAME = UI.FormatAsLink("Lavatory Glitter", "EQUIPPABLEBALLOON");

						// Token: 0x0400AE4A RID: 44618
						public static LocString DESC = "Sparkly balloons in an all-too-familiar hue.";
					}

					// Token: 0x020029C0 RID: 10688
					public class BALLOON_BLUE_LONG_SPARKLES
					{
						// Token: 0x0400AE4B RID: 44619
						public static LocString NAME = UI.FormatAsLink("Wheezewort Glitter", "EQUIPPABLEBALLOON");

						// Token: 0x0400AE4C RID: 44620
						public static LocString DESC = "They float <i>and</i> sparkle!";
					}

					// Token: 0x020029C1 RID: 10689
					public class BALLOON_GREEN_LONG_SPARKLES
					{
						// Token: 0x0400AE4D RID: 44621
						public static LocString NAME = UI.FormatAsLink("Mush Bar Glitter", "EQUIPPABLEBALLOON");

						// Token: 0x0400AE4E RID: 44622
						public static LocString DESC = "They float <i>and</i> sparkle!";
					}

					// Token: 0x020029C2 RID: 10690
					public class BALLOON_PINK_LONG_SPARKLES
					{
						// Token: 0x0400AE4F RID: 44623
						public static LocString NAME = UI.FormatAsLink("Petal Glitter", "EQUIPPABLEBALLOON");

						// Token: 0x0400AE50 RID: 44624
						public static LocString DESC = "They float <i>and</i> sparkle!";
					}

					// Token: 0x020029C3 RID: 10691
					public class BALLOON_PURPLE_LONG_SPARKLES
					{
						// Token: 0x0400AE51 RID: 44625
						public static LocString NAME = UI.FormatAsLink("Dusky Glitter", "EQUIPPABLEBALLOON");

						// Token: 0x0400AE52 RID: 44626
						public static LocString DESC = "They float <i>and</i> sparkle!";
					}

					// Token: 0x020029C4 RID: 10692
					public class BALLOON_BABY_PACU_EGG
					{
						// Token: 0x0400AE53 RID: 44627
						public static LocString NAME = UI.FormatAsLink("Floatie Fish", "EQUIPPABLEBALLOON");

						// Token: 0x0400AE54 RID: 44628
						public static LocString DESC = "They do not taste as good as the real thing.";
					}

					// Token: 0x020029C5 RID: 10693
					public class BALLOON_BABY_GLOSSY_DRECKO_EGG
					{
						// Token: 0x0400AE55 RID: 44629
						public static LocString NAME = UI.FormatAsLink("Glossy Glee", "EQUIPPABLEBALLOON");

						// Token: 0x0400AE56 RID: 44630
						public static LocString DESC = "A happy little trio of inflatable critters.";
					}

					// Token: 0x020029C6 RID: 10694
					public class BALLOON_BABY_HATCH_EGG
					{
						// Token: 0x0400AE57 RID: 44631
						public static LocString NAME = UI.FormatAsLink("Helium Hatches", "EQUIPPABLEBALLOON");

						// Token: 0x0400AE58 RID: 44632
						public static LocString DESC = "A happy little trio of inflatable critters.";
					}

					// Token: 0x020029C7 RID: 10695
					public class BALLOON_BABY_POKESHELL_EGG
					{
						// Token: 0x0400AE59 RID: 44633
						public static LocString NAME = UI.FormatAsLink("Peppy Pokeshells", "EQUIPPABLEBALLOON");

						// Token: 0x0400AE5A RID: 44634
						public static LocString DESC = "A happy little trio of inflatable critters.";
					}

					// Token: 0x020029C8 RID: 10696
					public class BALLOON_BABY_PUFT_EGG
					{
						// Token: 0x0400AE5B RID: 44635
						public static LocString NAME = UI.FormatAsLink("Puffed-Up Pufts", "EQUIPPABLEBALLOON");

						// Token: 0x0400AE5C RID: 44636
						public static LocString DESC = "A happy little trio of inflatable critters.";
					}

					// Token: 0x020029C9 RID: 10697
					public class BALLOON_BABY_SHOVOLE_EGG
					{
						// Token: 0x0400AE5D RID: 44637
						public static LocString NAME = UI.FormatAsLink("Voley Voley Voles", "EQUIPPABLEBALLOON");

						// Token: 0x0400AE5E RID: 44638
						public static LocString DESC = "A happy little trio of inflatable critters.";
					}

					// Token: 0x020029CA RID: 10698
					public class BALLOON_BABY_PIP_EGG
					{
						// Token: 0x0400AE5F RID: 44639
						public static LocString NAME = UI.FormatAsLink("Pip Pip Hooray", "EQUIPPABLEBALLOON");

						// Token: 0x0400AE60 RID: 44640
						public static LocString DESC = "A happy little trio of inflatable critters.";
					}

					// Token: 0x020029CB RID: 10699
					public class CANDY_BLUEBERRY
					{
						// Token: 0x0400AE61 RID: 44641
						public static LocString NAME = UI.FormatAsLink("Candied Blueberry", "EQUIPPABLEBALLOON");

						// Token: 0x0400AE62 RID: 44642
						public static LocString DESC = "A juicy bunch of blueberry-scented balloons.";
					}

					// Token: 0x020029CC RID: 10700
					public class CANDY_GRAPE
					{
						// Token: 0x0400AE63 RID: 44643
						public static LocString NAME = UI.FormatAsLink("Candied Grape", "EQUIPPABLEBALLOON");

						// Token: 0x0400AE64 RID: 44644
						public static LocString DESC = "A juicy bunch of grape-scented balloons.";
					}

					// Token: 0x020029CD RID: 10701
					public class CANDY_LEMON
					{
						// Token: 0x0400AE65 RID: 44645
						public static LocString NAME = UI.FormatAsLink("Candied Lemon", "EQUIPPABLEBALLOON");

						// Token: 0x0400AE66 RID: 44646
						public static LocString DESC = "A juicy lemon-scented bunch of balloons.";
					}

					// Token: 0x020029CE RID: 10702
					public class CANDY_LIME
					{
						// Token: 0x0400AE67 RID: 44647
						public static LocString NAME = UI.FormatAsLink("Candied Lime", "EQUIPPABLEBALLOON");

						// Token: 0x0400AE68 RID: 44648
						public static LocString DESC = "A juicy lime-scented bunch of balloons.";
					}

					// Token: 0x020029CF RID: 10703
					public class CANDY_ORANGE
					{
						// Token: 0x0400AE69 RID: 44649
						public static LocString NAME = UI.FormatAsLink("Candied Satsuma", "EQUIPPABLEBALLOON");

						// Token: 0x0400AE6A RID: 44650
						public static LocString DESC = "A juicy satsuma-scented bunch of balloons.";
					}

					// Token: 0x020029D0 RID: 10704
					public class CANDY_STRAWBERRY
					{
						// Token: 0x0400AE6B RID: 44651
						public static LocString NAME = UI.FormatAsLink("Candied Strawberry", "EQUIPPABLEBALLOON");

						// Token: 0x0400AE6C RID: 44652
						public static LocString DESC = "A juicy strawberry-scented bunch of balloons.";
					}

					// Token: 0x020029D1 RID: 10705
					public class CANDY_WATERMELON
					{
						// Token: 0x0400AE6D RID: 44653
						public static LocString NAME = UI.FormatAsLink("Candied Watermelon", "EQUIPPABLEBALLOON");

						// Token: 0x0400AE6E RID: 44654
						public static LocString DESC = "A juicy watermelon-scented bunch of balloons.";
					}

					// Token: 0x020029D2 RID: 10706
					public class HAND_GOLD
					{
						// Token: 0x0400AE6F RID: 44655
						public static LocString NAME = UI.FormatAsLink("Gold Fingers", "EQUIPPABLEBALLOON");

						// Token: 0x0400AE70 RID: 44656
						public static LocString DESC = "Inflatable gestures of encouragement.";
					}
				}
			}

			// Token: 0x020029D3 RID: 10707
			public class SLEEPCLINICPAJAMAS
			{
				// Token: 0x0400AE71 RID: 44657
				public static LocString NAME = UI.FormatAsLink("Pajamas", "SLEEP_CLINIC_PAJAMAS");

				// Token: 0x0400AE72 RID: 44658
				public static LocString GENERICNAME = "Clothing";

				// Token: 0x0400AE73 RID: 44659
				public static LocString DESC = "A soft, fleecy ticket to dreamland.";

				// Token: 0x0400AE74 RID: 44660
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Helps Duplicants fall asleep by reducing ",
					UI.FormatAsLink("Stamina", "STAMINA"),
					".\n\nEnables the wearer to dream and produce ",
					UI.FormatAsLink("Dream Journals", "DREAMJOURNAL"),
					"."
				});

				// Token: 0x0400AE75 RID: 44661
				public static LocString DESTROY_TOAST = "Ripped Pajamas";
			}
		}
	}
}
