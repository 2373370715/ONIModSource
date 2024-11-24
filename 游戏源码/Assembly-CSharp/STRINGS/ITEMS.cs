using System;

namespace STRINGS
{
	// Token: 0x020037FE RID: 14334
	public class ITEMS
	{
		// Token: 0x020037FF RID: 14335
		public class PILLS
		{
			// Token: 0x02003800 RID: 14336
			public class PLACEBO
			{
				// Token: 0x0400DA0D RID: 55821
				public static LocString NAME = "Placebo";

				// Token: 0x0400DA0E RID: 55822
				public static LocString DESC = "A general, all-purpose " + UI.FormatAsLink("Medicine", "MEDICINE") + ".\n\nThe less one knows about it, the better it works.";

				// Token: 0x0400DA0F RID: 55823
				public static LocString RECIPEDESC = "All-purpose " + UI.FormatAsLink("Medicine", "MEDICINE") + ".";
			}

			// Token: 0x02003801 RID: 14337
			public class BASICBOOSTER
			{
				// Token: 0x0400DA10 RID: 55824
				public static LocString NAME = "Vitamin Chews";

				// Token: 0x0400DA11 RID: 55825
				public static LocString DESC = "Minorly reduces the chance of becoming sick.";

				// Token: 0x0400DA12 RID: 55826
				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					"A supplement that minorly reduces the chance of contracting a ",
					UI.PRE_KEYWORD,
					"Germ",
					UI.PST_KEYWORD,
					"-based ",
					UI.FormatAsLink("Disease", "DISEASE"),
					".\n\nMust be taken daily."
				});
			}

			// Token: 0x02003802 RID: 14338
			public class INTERMEDIATEBOOSTER
			{
				// Token: 0x0400DA13 RID: 55827
				public static LocString NAME = "Immuno Booster";

				// Token: 0x0400DA14 RID: 55828
				public static LocString DESC = "Significantly reduces the chance of becoming sick.";

				// Token: 0x0400DA15 RID: 55829
				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					"A supplement that significantly reduces the chance of contracting a ",
					UI.PRE_KEYWORD,
					"Germ",
					UI.PST_KEYWORD,
					"-based ",
					UI.FormatAsLink("Disease", "DISEASE"),
					".\n\nMust be taken daily."
				});
			}

			// Token: 0x02003803 RID: 14339
			public class ANTIHISTAMINE
			{
				// Token: 0x0400DA16 RID: 55830
				public static LocString NAME = "Allergy Medication";

				// Token: 0x0400DA17 RID: 55831
				public static LocString DESC = "Suppresses and prevents allergic reactions.";

				// Token: 0x0400DA18 RID: 55832
				public static LocString RECIPEDESC = "A strong antihistamine Duplicants can take to halt an allergic reaction. " + ITEMS.PILLS.ANTIHISTAMINE.NAME + " will also prevent further reactions from occurring for a short time after ingestion.";
			}

			// Token: 0x02003804 RID: 14340
			public class BASICCURE
			{
				// Token: 0x0400DA19 RID: 55833
				public static LocString NAME = "Curative Tablet";

				// Token: 0x0400DA1A RID: 55834
				public static LocString DESC = "A simple, easy-to-take remedy for minor germ-based diseases.";

				// Token: 0x0400DA1B RID: 55835
				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					"Duplicants can take this to cure themselves of minor ",
					UI.PRE_KEYWORD,
					"Germ",
					UI.PST_KEYWORD,
					"-based ",
					UI.FormatAsLink("Diseases", "DISEASE"),
					".\n\nCurative Tablets are very effective against ",
					UI.FormatAsLink("Food Poisoning", "FOODSICKNESS"),
					"."
				});
			}

			// Token: 0x02003805 RID: 14341
			public class INTERMEDIATECURE
			{
				// Token: 0x0400DA1C RID: 55836
				public static LocString NAME = "Medical Pack";

				// Token: 0x0400DA1D RID: 55837
				public static LocString DESC = "A doctor-administered cure for moderate ailments.";

				// Token: 0x0400DA1E RID: 55838
				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					"A doctor-administered cure for moderate ",
					UI.FormatAsLink("Diseases", "DISEASE"),
					". ",
					ITEMS.PILLS.INTERMEDIATECURE.NAME,
					"s are very effective against ",
					UI.FormatAsLink("Slimelung", "SLIMESICKNESS"),
					".\n\nMust be administered by a Duplicant with the ",
					DUPLICANTS.ROLES.MEDIC.NAME,
					" Skill."
				});
			}

			// Token: 0x02003806 RID: 14342
			public class ADVANCEDCURE
			{
				// Token: 0x0400DA1F RID: 55839
				public static LocString NAME = "Serum Vial";

				// Token: 0x0400DA20 RID: 55840
				public static LocString DESC = "A doctor-administered cure for severe ailments.";

				// Token: 0x0400DA21 RID: 55841
				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					"An extremely powerful medication created to treat severe ",
					UI.FormatAsLink("Diseases", "DISEASE"),
					". ",
					ITEMS.PILLS.ADVANCEDCURE.NAME,
					" is very effective against ",
					UI.FormatAsLink("Zombie Spores", "ZOMBIESPORES"),
					".\n\nMust be administered by a Duplicant with the ",
					DUPLICANTS.ROLES.SENIOR_MEDIC.NAME,
					" Skill."
				});
			}

			// Token: 0x02003807 RID: 14343
			public class BASICRADPILL
			{
				// Token: 0x0400DA22 RID: 55842
				public static LocString NAME = "Basic Rad Pill";

				// Token: 0x0400DA23 RID: 55843
				public static LocString DESC = "Increases a Duplicant's natural radiation absorption rate.";

				// Token: 0x0400DA24 RID: 55844
				public static LocString RECIPEDESC = "A supplement that speeds up the rate at which a Duplicant body absorbs radiation, allowing them to manage increased radiation exposure.\n\nMust be taken daily.";
			}

			// Token: 0x02003808 RID: 14344
			public class INTERMEDIATERADPILL
			{
				// Token: 0x0400DA25 RID: 55845
				public static LocString NAME = "Intermediate Rad Pill";

				// Token: 0x0400DA26 RID: 55846
				public static LocString DESC = "Increases a Duplicant's natural radiation absorption rate.";

				// Token: 0x0400DA27 RID: 55847
				public static LocString RECIPEDESC = "A supplement that speeds up the rate at which a Duplicant body absorbs radiation, allowing them to manage increased radiation exposure.\n\nMust be taken daily.";
			}
		}

		// Token: 0x02003809 RID: 14345
		public class BIONIC_BOOSTERS
		{
			// Token: 0x0200380A RID: 14346
			public class EXPLORER_BOOSTER
			{
				// Token: 0x0400DA28 RID: 55848
				public static LocString NAME = "Dowsing Booster";

				// Token: 0x0400DA29 RID: 55849
				public static LocString DESC = "Enables a Bionic Duplicant to regularly uncover hidden geysers.";
			}

			// Token: 0x0200380B RID: 14347
			public class PILOTING_BOOSTER
			{
				// Token: 0x0400DA2A RID: 55850
				public static LocString NAME = "Rocketry Booster";

				// Token: 0x0400DA2B RID: 55851
				public static LocString DESC = "Increases a Bionic Duplicant's rocket piloting skills.";
			}

			// Token: 0x0200380C RID: 14348
			public class CONSTRUCTION_BOOSTER
			{
				// Token: 0x0400DA2C RID: 55852
				public static LocString NAME = "Building Booster";

				// Token: 0x0400DA2D RID: 55853
				public static LocString DESC = "Increases a Bionic Duplicant's construction skills.";
			}

			// Token: 0x0200380D RID: 14349
			public class EXCAVATION_BOOSTER
			{
				// Token: 0x0400DA2E RID: 55854
				public static LocString NAME = "Excavation Booster";

				// Token: 0x0400DA2F RID: 55855
				public static LocString DESC = "Increases a Bionic Duplicant's digging skills.";
			}

			// Token: 0x0200380E RID: 14350
			public class MACHINERY_BOOSTER
			{
				// Token: 0x0400DA30 RID: 55856
				public static LocString NAME = "Operating Booster";

				// Token: 0x0400DA31 RID: 55857
				public static LocString DESC = "Increases a Bionic Duplicant's machinery skills.";
			}

			// Token: 0x0200380F RID: 14351
			public class ATHLETICS_BOOSTER
			{
				// Token: 0x0400DA32 RID: 55858
				public static LocString NAME = "Athletics Booster";

				// Token: 0x0400DA33 RID: 55859
				public static LocString DESC = "Increases a Bionic Duplicant's runspeed.";
			}

			// Token: 0x02003810 RID: 14352
			public class SCIENCE_BOOSTER
			{
				// Token: 0x0400DA34 RID: 55860
				public static LocString NAME = "Researching Booster";

				// Token: 0x0400DA35 RID: 55861
				public static LocString DESC = "Increases a Bionic Duplicant's science researching skills.";
			}

			// Token: 0x02003811 RID: 14353
			public class COOKING_BOOSTER
			{
				// Token: 0x0400DA36 RID: 55862
				public static LocString NAME = "Cooking Booster";

				// Token: 0x0400DA37 RID: 55863
				public static LocString DESC = "Increases a Bionic Duplicant's culinary skills.";
			}

			// Token: 0x02003812 RID: 14354
			public class MEDICINE_BOOSTER
			{
				// Token: 0x0400DA38 RID: 55864
				public static LocString NAME = "Doctoring Booster";

				// Token: 0x0400DA39 RID: 55865
				public static LocString DESC = "Increases a Bionic Duplicant's doctoring skills.";
			}

			// Token: 0x02003813 RID: 14355
			public class STRENGTH_BOOSTER
			{
				// Token: 0x0400DA3A RID: 55866
				public static LocString NAME = "Strength Booster";

				// Token: 0x0400DA3B RID: 55867
				public static LocString DESC = "Increases a Bionic Duplicant's carrying capacity and tidying speed.";
			}

			// Token: 0x02003814 RID: 14356
			public class CREATIVITY_BOOSTER
			{
				// Token: 0x0400DA3C RID: 55868
				public static LocString NAME = "Decorating Booster";

				// Token: 0x0400DA3D RID: 55869
				public static LocString DESC = "Increases a Bionic Duplicant's creativity.";
			}

			// Token: 0x02003815 RID: 14357
			public class AGRICULTURE_BOOSTER
			{
				// Token: 0x0400DA3E RID: 55870
				public static LocString NAME = "Farming Booster";

				// Token: 0x0400DA3F RID: 55871
				public static LocString DESC = "Increases a Bionic Duplicant's agricultural skills.";
			}

			// Token: 0x02003816 RID: 14358
			public class HUSBANDRY_BOOSTER
			{
				// Token: 0x0400DA40 RID: 55872
				public static LocString NAME = "Ranching Booster";

				// Token: 0x0400DA41 RID: 55873
				public static LocString DESC = "Increases a Bionic Duplicant's husbandry skills.";
			}
		}

		// Token: 0x02003817 RID: 14359
		public class FOOD
		{
			// Token: 0x0400DA42 RID: 55874
			public static LocString COMPOST = "Compost";

			// Token: 0x02003818 RID: 14360
			public class FOODSPLAT
			{
				// Token: 0x0400DA43 RID: 55875
				public static LocString NAME = "Food Splatter";

				// Token: 0x0400DA44 RID: 55876
				public static LocString DESC = "Food smeared on the wall from a recent Food Fight";
			}

			// Token: 0x02003819 RID: 14361
			public class BURGER
			{
				// Token: 0x0400DA45 RID: 55877
				public static LocString NAME = UI.FormatAsLink("Frost Burger", "BURGER");

				// Token: 0x0400DA46 RID: 55878
				public static LocString DESC = string.Concat(new string[]
				{
					UI.FormatAsLink("Meat", "MEAT"),
					" and ",
					UI.FormatAsLink("Lettuce", "LETTUCE"),
					" on a chilled ",
					UI.FormatAsLink("Frost Bun", "COLDWHEATBREAD"),
					".\n\nIt's the only burger best served cold."
				});

				// Token: 0x0400DA47 RID: 55879
				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					UI.FormatAsLink("Meat", "MEAT"),
					" and ",
					UI.FormatAsLink("Lettuce", "LETTUCE"),
					" on a chilled ",
					UI.FormatAsLink("Frost Bun", "COLDWHEATBREAD"),
					"."
				});

				// Token: 0x0200381A RID: 14362
				public class DEHYDRATED
				{
					// Token: 0x0400DA48 RID: 55880
					public static LocString NAME = "Dried Frost Burger";

					// Token: 0x0400DA49 RID: 55881
					public static LocString DESC = string.Concat(new string[]
					{
						"A dehydrated ",
						UI.FormatAsLink("Frost Burger", "BURGER"),
						" ration. It must be rehydrated in order to be considered ",
						UI.FormatAsLink("Food", "FOOD"),
						".\n\nDry rations have no expiry date."
					});
				}
			}

			// Token: 0x0200381B RID: 14363
			public class FIELDRATION
			{
				// Token: 0x0400DA4A RID: 55882
				public static LocString NAME = UI.FormatAsLink("Nutrient Bar", "FIELDRATION");

				// Token: 0x0400DA4B RID: 55883
				public static LocString DESC = "A nourishing nutrient paste, sandwiched between thin wafer layers.";
			}

			// Token: 0x0200381C RID: 14364
			public class MUSHBAR
			{
				// Token: 0x0400DA4C RID: 55884
				public static LocString NAME = UI.FormatAsLink("Mush Bar", "MUSHBAR");

				// Token: 0x0400DA4D RID: 55885
				public static LocString DESC = "An edible, putrefied mudslop.\n\nMush Bars are preferable to starvation, but only just barely.";

				// Token: 0x0400DA4E RID: 55886
				public static LocString RECIPEDESC = "An edible, putrefied mudslop.\n\n" + ITEMS.FOOD.MUSHBAR.NAME + "s are preferable to starvation, but only just barely.";
			}

			// Token: 0x0200381D RID: 14365
			public class MUSHROOMWRAP
			{
				// Token: 0x0400DA4F RID: 55887
				public static LocString NAME = UI.FormatAsLink("Mushroom Wrap", "MUSHROOMWRAP");

				// Token: 0x0400DA50 RID: 55888
				public static LocString DESC = string.Concat(new string[]
				{
					"Flavorful ",
					UI.FormatAsLink("Mushrooms", "MUSHROOM"),
					" wrapped in ",
					UI.FormatAsLink("Lettuce", "LETTUCE"),
					".\n\nIt has an earthy flavor punctuated by a refreshing crunch."
				});

				// Token: 0x0400DA51 RID: 55889
				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					"Flavorful ",
					UI.FormatAsLink("Mushrooms", "MUSHROOM"),
					" wrapped in ",
					UI.FormatAsLink("Lettuce", "LETTUCE"),
					"."
				});

				// Token: 0x0200381E RID: 14366
				public class DEHYDRATED
				{
					// Token: 0x0400DA52 RID: 55890
					public static LocString NAME = "Dried Mushroom Wrap";

					// Token: 0x0400DA53 RID: 55891
					public static LocString DESC = string.Concat(new string[]
					{
						"A dehydrated ",
						UI.FormatAsLink("Mushroom Wrap", "MUSHROOMWRAP"),
						" ration. It must be rehydrated in order to be considered ",
						UI.FormatAsLink("Food", "FOOD"),
						".\n\nDry rations have no expiry date."
					});
				}
			}

			// Token: 0x0200381F RID: 14367
			public class MICROWAVEDLETTUCE
			{
				// Token: 0x0400DA54 RID: 55892
				public static LocString NAME = UI.FormatAsLink("Microwaved Lettuce", "MICROWAVEDLETTUCE");

				// Token: 0x0400DA55 RID: 55893
				public static LocString DESC = UI.FormatAsLink("Lettuce", "LETTUCE") + " scrumptiously wilted in the " + BUILDINGS.PREFABS.GAMMARAYOVEN.NAME + ".";

				// Token: 0x0400DA56 RID: 55894
				public static LocString RECIPEDESC = UI.FormatAsLink("Lettuce", "LETTUCE") + " scrumptiously wilted in the " + BUILDINGS.PREFABS.GAMMARAYOVEN.NAME + ".";
			}

			// Token: 0x02003820 RID: 14368
			public class GAMMAMUSH
			{
				// Token: 0x0400DA57 RID: 55895
				public static LocString NAME = UI.FormatAsLink("Gamma Mush", "GAMMAMUSH");

				// Token: 0x0400DA58 RID: 55896
				public static LocString DESC = "A disturbingly delicious mixture of irradiated dirt and water.";

				// Token: 0x0400DA59 RID: 55897
				public static LocString RECIPEDESC = UI.FormatAsLink("Mush Fry", "FRIEDMUSHBAR") + " reheated in a " + BUILDINGS.PREFABS.GAMMARAYOVEN.NAME + ".";
			}

			// Token: 0x02003821 RID: 14369
			public class FRUITCAKE
			{
				// Token: 0x0400DA5A RID: 55898
				public static LocString NAME = UI.FormatAsLink("Berry Sludge", "FRUITCAKE");

				// Token: 0x0400DA5B RID: 55899
				public static LocString DESC = "A mashed up " + UI.FormatAsLink("Bristle Berry", "PRICKLEFRUIT") + " sludge with an exceptionally long shelf life.\n\nIts aggressive, overbearing sweetness can leave the tongue feeling temporarily numb.";

				// Token: 0x0400DA5C RID: 55900
				public static LocString RECIPEDESC = "A mashed up " + UI.FormatAsLink("Bristle Berry", "PRICKLEFRUIT") + " sludge with an exceptionally long shelf life.";
			}

			// Token: 0x02003822 RID: 14370
			public class POPCORN
			{
				// Token: 0x0400DA5D RID: 55901
				public static LocString NAME = UI.FormatAsLink("Popcorn", "POPCORN");

				// Token: 0x0400DA5E RID: 55902
				public static LocString DESC = UI.FormatAsLink("Sleet Wheat Grain", "COLDWHEATSEED") + " popped in a " + BUILDINGS.PREFABS.GAMMARAYOVEN.NAME + ".\n\nCompletely devoid of any fancy flavorings.";

				// Token: 0x0400DA5F RID: 55903
				public static LocString RECIPEDESC = "Gamma-radiated " + UI.FormatAsLink("Sleet Wheat Grain", "COLDWHEATSEED") + ".";
			}

			// Token: 0x02003823 RID: 14371
			public class SUSHI
			{
				// Token: 0x0400DA60 RID: 55904
				public static LocString NAME = UI.FormatAsLink("Sushi", "SUSHI");

				// Token: 0x0400DA61 RID: 55905
				public static LocString DESC = string.Concat(new string[]
				{
					"Raw ",
					UI.FormatAsLink("Pacu Fillet", "FISHMEAT"),
					" wrapped with fresh ",
					UI.FormatAsLink("Lettuce", "LETTUCE"),
					".\n\nWhile the salt of the lettuce may initially overpower the flavor, a keen palate can discern the subtle sweetness of the fillet beneath."
				});

				// Token: 0x0400DA62 RID: 55906
				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					"Raw ",
					UI.FormatAsLink("Pacu Fillet", "FISHMEAT"),
					" wrapped with fresh ",
					UI.FormatAsLink("Lettuce", "LETTUCE"),
					"."
				});
			}

			// Token: 0x02003824 RID: 14372
			public class HATCHEGG
			{
				// Token: 0x0400DA63 RID: 55907
				public static LocString NAME = CREATURES.SPECIES.HATCH.EGG_NAME;

				// Token: 0x0400DA64 RID: 55908
				public static LocString DESC = string.Concat(new string[]
				{
					"An egg laid by a ",
					UI.FormatAsLink("Hatch", "HATCH"),
					".\n\nIf incubated, it will hatch into a ",
					UI.FormatAsLink("Hatchling", "HATCH"),
					"."
				});

				// Token: 0x0400DA65 RID: 55909
				public static LocString RECIPEDESC = "An egg laid by a " + UI.FormatAsLink("Hatch", "HATCH") + ".";
			}

			// Token: 0x02003825 RID: 14373
			public class DRECKOEGG
			{
				// Token: 0x0400DA66 RID: 55910
				public static LocString NAME = CREATURES.SPECIES.DRECKO.EGG_NAME;

				// Token: 0x0400DA67 RID: 55911
				public static LocString DESC = string.Concat(new string[]
				{
					"An egg laid by a ",
					UI.FormatAsLink("Drecko", "DRECKO"),
					".\n\nIf incubated, it will hatch into a new ",
					UI.FormatAsLink("Drecklet", "DRECKO"),
					"."
				});

				// Token: 0x0400DA68 RID: 55912
				public static LocString RECIPEDESC = "An egg laid by a " + UI.FormatAsLink("Drecko", "DRECKO") + ".";
			}

			// Token: 0x02003826 RID: 14374
			public class LIGHTBUGEGG
			{
				// Token: 0x0400DA69 RID: 55913
				public static LocString NAME = CREATURES.SPECIES.LIGHTBUG.EGG_NAME;

				// Token: 0x0400DA6A RID: 55914
				public static LocString DESC = string.Concat(new string[]
				{
					"An egg laid by a ",
					UI.FormatAsLink("Shine Bug", "LIGHTBUG"),
					".\n\nIf incubated, it will hatch into a ",
					UI.FormatAsLink("Shine Nymph", "LIGHTBUG"),
					"."
				});

				// Token: 0x0400DA6B RID: 55915
				public static LocString RECIPEDESC = "An egg laid by a " + UI.FormatAsLink("Shine Bug", "LIGHTBUG") + ".";
			}

			// Token: 0x02003827 RID: 14375
			public class LETTUCE
			{
				// Token: 0x0400DA6C RID: 55916
				public static LocString NAME = UI.FormatAsLink("Lettuce", "LETTUCE");

				// Token: 0x0400DA6D RID: 55917
				public static LocString DESC = "Crunchy, slightly salty leaves from a " + UI.FormatAsLink("Waterweed", "SEALETTUCE") + " plant.";

				// Token: 0x0400DA6E RID: 55918
				public static LocString RECIPEDESC = "Edible roughage from a " + UI.FormatAsLink("Waterweed", "SEALETTUCE") + ".";
			}

			// Token: 0x02003828 RID: 14376
			public class PASTA
			{
				// Token: 0x0400DA6F RID: 55919
				public static LocString NAME = UI.FormatAsLink("Pasta", "PASTA");

				// Token: 0x0400DA70 RID: 55920
				public static LocString DESC = "pasta made from egg and wheat";

				// Token: 0x0400DA71 RID: 55921
				public static LocString RECIPEDESC = "pasta made from egg and wheat";
			}

			// Token: 0x02003829 RID: 14377
			public class PANCAKES
			{
				// Token: 0x0400DA72 RID: 55922
				public static LocString NAME = UI.FormatAsLink("Soufflé Pancakes", "PANCAKES");

				// Token: 0x0400DA73 RID: 55923
				public static LocString DESC = string.Concat(new string[]
				{
					"Sweet discs made from ",
					UI.FormatAsLink("Raw Egg", "RAWEGG"),
					" and ",
					UI.FormatAsLink("Sleet Wheat Grain", "COLDWHEATSEED"),
					".\n\nThey're so thick!"
				});

				// Token: 0x0400DA74 RID: 55924
				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					"Sweet discs made from ",
					UI.FormatAsLink("Raw Egg", "RAWEGG"),
					" and ",
					UI.FormatAsLink("Sleet Wheat Grain", "COLDWHEATSEED"),
					"."
				});
			}

			// Token: 0x0200382A RID: 14378
			public class OILFLOATEREGG
			{
				// Token: 0x0400DA75 RID: 55925
				public static LocString NAME = CREATURES.SPECIES.OILFLOATER.EGG_NAME;

				// Token: 0x0400DA76 RID: 55926
				public static LocString DESC = string.Concat(new string[]
				{
					"An egg laid by a ",
					UI.FormatAsLink("Slickster", "OILFLOATER"),
					".\n\nIf incubated, it will hatch into a ",
					UI.FormatAsLink("Slickster Larva", "OILFLOATER"),
					"."
				});

				// Token: 0x0400DA77 RID: 55927
				public static LocString RECIPEDESC = "An egg laid by a " + UI.FormatAsLink("Slickster", "OILFLOATER") + ".";
			}

			// Token: 0x0200382B RID: 14379
			public class PUFTEGG
			{
				// Token: 0x0400DA78 RID: 55928
				public static LocString NAME = CREATURES.SPECIES.PUFT.EGG_NAME;

				// Token: 0x0400DA79 RID: 55929
				public static LocString DESC = string.Concat(new string[]
				{
					"An egg laid by a ",
					UI.FormatAsLink("Puft", "PUFT"),
					".\n\nIf incubated, it will hatch into a ",
					UI.FormatAsLink("Puftlet", "PUFT"),
					"."
				});

				// Token: 0x0400DA7A RID: 55930
				public static LocString RECIPEDESC = "An egg laid by a " + CREATURES.SPECIES.PUFT.NAME + ".";
			}

			// Token: 0x0200382C RID: 14380
			public class FISHMEAT
			{
				// Token: 0x0400DA7B RID: 55931
				public static LocString NAME = UI.FormatAsLink("Pacu Fillet", "FISHMEAT");

				// Token: 0x0400DA7C RID: 55932
				public static LocString DESC = "An uncooked fillet from a very dead " + CREATURES.SPECIES.PACU.NAME + ". Yum!";
			}

			// Token: 0x0200382D RID: 14381
			public class MEAT
			{
				// Token: 0x0400DA7D RID: 55933
				public static LocString NAME = UI.FormatAsLink("Meat", "MEAT");

				// Token: 0x0400DA7E RID: 55934
				public static LocString DESC = "Uncooked meat from a very dead critter. Yum!";
			}

			// Token: 0x0200382E RID: 14382
			public class PLANTMEAT
			{
				// Token: 0x0400DA7F RID: 55935
				public static LocString NAME = UI.FormatAsLink("Plant Meat", "PLANTMEAT");

				// Token: 0x0400DA80 RID: 55936
				public static LocString DESC = "Planty plant meat from a plant. How nice!";
			}

			// Token: 0x0200382F RID: 14383
			public class SHELLFISHMEAT
			{
				// Token: 0x0400DA81 RID: 55937
				public static LocString NAME = UI.FormatAsLink("Raw Shellfish", "SHELLFISHMEAT");

				// Token: 0x0400DA82 RID: 55938
				public static LocString DESC = "An uncooked chunk of very dead " + CREATURES.SPECIES.CRAB.VARIANT_FRESH_WATER.NAME + ". Yum!";
			}

			// Token: 0x02003830 RID: 14384
			public class MUSHROOM
			{
				// Token: 0x0400DA83 RID: 55939
				public static LocString NAME = UI.FormatAsLink("Mushroom", "MUSHROOM");

				// Token: 0x0400DA84 RID: 55940
				public static LocString DESC = "An edible, flavorless fungus that grew in the dark.";
			}

			// Token: 0x02003831 RID: 14385
			public class COOKEDFISH
			{
				// Token: 0x0400DA85 RID: 55941
				public static LocString NAME = UI.FormatAsLink("Cooked Seafood", "COOKEDFISH");

				// Token: 0x0400DA86 RID: 55942
				public static LocString DESC = "A cooked piece of freshly caught aquatic critter.\n\nUnsurprisingly, it tastes a bit fishy.";

				// Token: 0x0400DA87 RID: 55943
				public static LocString RECIPEDESC = "A cooked piece of freshly caught aquatic critter.";
			}

			// Token: 0x02003832 RID: 14386
			public class COOKEDMEAT
			{
				// Token: 0x0400DA88 RID: 55944
				public static LocString NAME = UI.FormatAsLink("Barbeque", "COOKEDMEAT");

				// Token: 0x0400DA89 RID: 55945
				public static LocString DESC = "The cooked meat of a defeated critter.\n\nIt has a delightful smoky aftertaste.";

				// Token: 0x0400DA8A RID: 55946
				public static LocString RECIPEDESC = "The cooked meat of a defeated critter.";
			}

			// Token: 0x02003833 RID: 14387
			public class FRIESCARROT
			{
				// Token: 0x0400DA8B RID: 55947
				public static LocString NAME = UI.FormatAsLink("Squash Fries", "FRIESCARROT");

				// Token: 0x0400DA8C RID: 55948
				public static LocString DESC = "Irresistibly crunchy.\n\nBest eaten hot.";

				// Token: 0x0400DA8D RID: 55949
				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					"Crunchy sticks of ",
					UI.FormatAsLink("Plume Squash", "CARROT"),
					" deep-fried in ",
					UI.FormatAsLink("Tallow", "TALLOW"),
					"."
				});
			}

			// Token: 0x02003834 RID: 14388
			public class DEEPFRIEDFISH
			{
				// Token: 0x0400DA8E RID: 55950
				public static LocString NAME = UI.FormatAsLink("Fish Taco", "DEEPFRIEDFISH");

				// Token: 0x0400DA8F RID: 55951
				public static LocString DESC = "Deep-fried fish cradled in a crunchy fin.";

				// Token: 0x0400DA90 RID: 55952
				public static LocString RECIPEDESC = UI.FormatAsLink("Pacu Fillet", "FISHMEAT") + " lightly battered and deep-fried in " + UI.FormatAsLink("Tallow", "TALLOW") + ".";
			}

			// Token: 0x02003835 RID: 14389
			public class DEEPFRIEDSHELLFISH
			{
				// Token: 0x0400DA91 RID: 55953
				public static LocString NAME = UI.FormatAsLink("Shellfish Tempura", "DEEPFRIEDSHELLFISH");

				// Token: 0x0400DA92 RID: 55954
				public static LocString DESC = "A crispy deep-fried critter claw.";

				// Token: 0x0400DA93 RID: 55955
				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					"A tender chunk of battered ",
					UI.FormatAsLink("Raw Shellfish", "SHELLFISHMEAT"),
					" deep-fried in ",
					UI.FormatAsLink("Tallow", "TALLOW"),
					"."
				});
			}

			// Token: 0x02003836 RID: 14390
			public class DEEPFRIEDMEAT
			{
				// Token: 0x0400DA94 RID: 55956
				public static LocString NAME = UI.FormatAsLink("Deep Fried Steak", "DEEPFRIEDMEAT");

				// Token: 0x0400DA95 RID: 55957
				public static LocString DESC = "A juicy slab of meat with a crunchy deep-fried upper layer.";

				// Token: 0x0400DA96 RID: 55958
				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					"A juicy slab of ",
					UI.FormatAsLink("Raw Meat", "MEAT"),
					" deep-fried in ",
					UI.FormatAsLink("Tallow", "TALLOW"),
					"."
				});
			}

			// Token: 0x02003837 RID: 14391
			public class DEEPFRIEDNOSH
			{
				// Token: 0x0400DA97 RID: 55959
				public static LocString NAME = UI.FormatAsLink("Nosh Noms", "DEEPFRIEDNOSH");

				// Token: 0x0400DA98 RID: 55960
				public static LocString DESC = "A snackable handful of crunchy beans.";

				// Token: 0x0400DA99 RID: 55961
				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					"A crunchy stack of ",
					UI.FormatAsLink("Nosh Beans", "BEANPLANTSEED"),
					" deep-fried in ",
					UI.FormatAsLink("Tallow", "TALLOW"),
					"."
				});
			}

			// Token: 0x02003838 RID: 14392
			public class PICKLEDMEAL
			{
				// Token: 0x0400DA9A RID: 55962
				public static LocString NAME = UI.FormatAsLink("Pickled Meal", "PICKLEDMEAL");

				// Token: 0x0400DA9B RID: 55963
				public static LocString DESC = "Meal Lice preserved in vinegar.\n\nIt's a rarely acquired taste.";

				// Token: 0x0400DA9C RID: 55964
				public static LocString RECIPEDESC = ITEMS.FOOD.BASICPLANTFOOD.NAME + " regrettably preserved in vinegar.";
			}

			// Token: 0x02003839 RID: 14393
			public class FRIEDMUSHBAR
			{
				// Token: 0x0400DA9D RID: 55965
				public static LocString NAME = UI.FormatAsLink("Mush Fry", "FRIEDMUSHBAR");

				// Token: 0x0400DA9E RID: 55966
				public static LocString DESC = "Pan-fried, solidified mudslop.\n\nThe inside is almost completely uncooked, despite the crunch on the outside.";

				// Token: 0x0400DA9F RID: 55967
				public static LocString RECIPEDESC = "Pan-fried, solidified mudslop.";
			}

			// Token: 0x0200383A RID: 14394
			public class RAWEGG
			{
				// Token: 0x0400DAA0 RID: 55968
				public static LocString NAME = UI.FormatAsLink("Raw Egg", "RAWEGG");

				// Token: 0x0400DAA1 RID: 55969
				public static LocString DESC = "A raw Egg that has been cracked open for use in " + UI.FormatAsLink("Food", "FOOD") + " preparation.\n\nIt will never hatch.";

				// Token: 0x0400DAA2 RID: 55970
				public static LocString RECIPEDESC = "A raw egg that has been cracked open for use in " + UI.FormatAsLink("Food", "FOOD") + " preparation.";
			}

			// Token: 0x0200383B RID: 14395
			public class COOKEDEGG
			{
				// Token: 0x0400DAA3 RID: 55971
				public static LocString NAME = UI.FormatAsLink("Omelette", "COOKEDEGG");

				// Token: 0x0400DAA4 RID: 55972
				public static LocString DESC = "Fluffed and folded Egg innards.\n\nIt turns out you do, in fact, have to break a few eggs to make it.";

				// Token: 0x0400DAA5 RID: 55973
				public static LocString RECIPEDESC = "Fluffed and folded egg innards.";
			}

			// Token: 0x0200383C RID: 14396
			public class FRIEDMUSHROOM
			{
				// Token: 0x0400DAA6 RID: 55974
				public static LocString NAME = UI.FormatAsLink("Fried Mushroom", "FRIEDMUSHROOM");

				// Token: 0x0400DAA7 RID: 55975
				public static LocString DESC = "A pan-fried dish made with a fruiting " + UI.FormatAsLink("Dusk Cap", "MUSHROOM") + ".\n\nIt has a thick, savory flavor with subtle earthy undertones.";

				// Token: 0x0400DAA8 RID: 55976
				public static LocString RECIPEDESC = "A pan-fried dish made with a fruiting " + UI.FormatAsLink("Dusk Cap", "MUSHROOM") + ".";
			}

			// Token: 0x0200383D RID: 14397
			public class COOKEDPIKEAPPLE
			{
				// Token: 0x0400DAA9 RID: 55977
				public static LocString NAME = UI.FormatAsLink("Pikeapple Skewer", "COOKEDPIKEAPPLE");

				// Token: 0x0400DAAA RID: 55978
				public static LocString DESC = "Grilling a " + UI.FormatAsLink("Pikeapple", "HARDSKINBERRY") + " softens its spikes, making it slighly less awkward to eat.\n\nIt does not diminish the smell.";

				// Token: 0x0400DAAB RID: 55979
				public static LocString RECIPEDESC = "A grilled dish made with a fruiting " + UI.FormatAsLink("Pikeapple", "HARDSKINBERRY") + ".";
			}

			// Token: 0x0200383E RID: 14398
			public class PRICKLEFRUIT
			{
				// Token: 0x0400DAAC RID: 55980
				public static LocString NAME = UI.FormatAsLink("Bristle Berry", "PRICKLEFRUIT");

				// Token: 0x0400DAAD RID: 55981
				public static LocString DESC = "A sweet, mostly pleasant-tasting fruit covered in prickly barbs.";
			}

			// Token: 0x0200383F RID: 14399
			public class GRILLEDPRICKLEFRUIT
			{
				// Token: 0x0400DAAE RID: 55982
				public static LocString NAME = UI.FormatAsLink("Gristle Berry", "GRILLEDPRICKLEFRUIT");

				// Token: 0x0400DAAF RID: 55983
				public static LocString DESC = "The grilled bud of a " + UI.FormatAsLink("Bristle Berry", "PRICKLEFRUIT") + ".\n\nHeat unlocked an exquisite taste in the fruit, though the burnt spines leave something to be desired.";

				// Token: 0x0400DAB0 RID: 55984
				public static LocString RECIPEDESC = "The grilled bud of a " + UI.FormatAsLink("Bristle Berry", "PRICKLEFRUIT") + ".";
			}

			// Token: 0x02003840 RID: 14400
			public class SWAMPFRUIT
			{
				// Token: 0x0400DAB1 RID: 55985
				public static LocString NAME = UI.FormatAsLink("Bog Jelly", "SWAMPFRUIT");

				// Token: 0x0400DAB2 RID: 55986
				public static LocString DESC = "A fruit with an outer film that contains chewy gelatinous cubes.";
			}

			// Token: 0x02003841 RID: 14401
			public class SWAMPDELIGHTS
			{
				// Token: 0x0400DAB3 RID: 55987
				public static LocString NAME = UI.FormatAsLink("Swampy Delights", "SWAMPDELIGHTS");

				// Token: 0x0400DAB4 RID: 55988
				public static LocString DESC = "Dried gelatinous cubes from a " + UI.FormatAsLink("Bog Jelly", "SWAMPFRUIT") + ".\n\nEach cube has a wonderfully chewy texture and is lightly coated in a delicate powder.";

				// Token: 0x0400DAB5 RID: 55989
				public static LocString RECIPEDESC = "Dried gelatinous cubes from a " + UI.FormatAsLink("Bog Jelly", "SWAMPFRUIT") + ".";
			}

			// Token: 0x02003842 RID: 14402
			public class WORMBASICFRUIT
			{
				// Token: 0x0400DAB6 RID: 55990
				public static LocString NAME = UI.FormatAsLink("Spindly Grubfruit", "WORMBASICFRUIT");

				// Token: 0x0400DAB7 RID: 55991
				public static LocString DESC = "A " + UI.FormatAsLink("Grubfruit", "WORMSUPERFRUIT") + " that failed to develop properly.\n\nIt is nonetheless edible, and vaguely tasty.";
			}

			// Token: 0x02003843 RID: 14403
			public class WORMBASICFOOD
			{
				// Token: 0x0400DAB8 RID: 55992
				public static LocString NAME = UI.FormatAsLink("Roast Grubfruit Nut", "WORMBASICFOOD");

				// Token: 0x0400DAB9 RID: 55993
				public static LocString DESC = "Slow roasted " + UI.FormatAsLink("Spindly Grubfruit", "WORMBASICFRUIT") + ".\n\nIt has a smoky aroma and tastes of coziness.";

				// Token: 0x0400DABA RID: 55994
				public static LocString RECIPEDESC = "Slow roasted " + UI.FormatAsLink("Spindly Grubfruit", "WORMBASICFRUIT") + ".";
			}

			// Token: 0x02003844 RID: 14404
			public class WORMSUPERFRUIT
			{
				// Token: 0x0400DABB RID: 55995
				public static LocString NAME = UI.FormatAsLink("Grubfruit", "WORMSUPERFRUIT");

				// Token: 0x0400DABC RID: 55996
				public static LocString DESC = "A plump, healthy fruit with a honey-like taste.";
			}

			// Token: 0x02003845 RID: 14405
			public class WORMSUPERFOOD
			{
				// Token: 0x0400DABD RID: 55997
				public static LocString NAME = UI.FormatAsLink("Grubfruit Preserve", "WORMSUPERFOOD");

				// Token: 0x0400DABE RID: 55998
				public static LocString DESC = string.Concat(new string[]
				{
					"A long lasting ",
					UI.FormatAsLink("Grubfruit", "WORMSUPERFRUIT"),
					" jam preserved in ",
					UI.FormatAsLink("Sucrose", "SUCROSE"),
					".\n\nThe thick, goopy jam retains the shape of the jar when poured out, but the sweet taste can't be matched."
				});

				// Token: 0x0400DABF RID: 55999
				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					"A long lasting ",
					UI.FormatAsLink("Grubfruit", "WORMSUPERFRUIT"),
					" jam preserved in ",
					UI.FormatAsLink("Sucrose", "SUCROSE"),
					"."
				});
			}

			// Token: 0x02003846 RID: 14406
			public class BERRYPIE
			{
				// Token: 0x0400DAC0 RID: 56000
				public static LocString NAME = UI.FormatAsLink("Mixed Berry Pie", "BERRYPIE");

				// Token: 0x0400DAC1 RID: 56001
				public static LocString DESC = string.Concat(new string[]
				{
					"A pie made primarily of ",
					UI.FormatAsLink("Grubfruit", "WORMSUPERFRUIT"),
					" and ",
					UI.FormatAsLink("Gristle Berries", "PRICKLEFRUIT"),
					".\n\nThe mixture of berries creates a fragrant, colorful filling that packs a sweet punch."
				});

				// Token: 0x0400DAC2 RID: 56002
				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					"A pie made primarily of ",
					UI.FormatAsLink("Grubfruit", "WORMSUPERFRUIT"),
					" and ",
					UI.FormatAsLink("Gristle Berries", "PRICKLEFRUIT"),
					"."
				});

				// Token: 0x02003847 RID: 14407
				public class DEHYDRATED
				{
					// Token: 0x0400DAC3 RID: 56003
					public static LocString NAME = "Dried Berry Pie";

					// Token: 0x0400DAC4 RID: 56004
					public static LocString DESC = string.Concat(new string[]
					{
						"A dehydrated ",
						UI.FormatAsLink("Mixed Berry Pie", "BERRYPIE"),
						" ration. It must be rehydrated in order to be considered ",
						UI.FormatAsLink("Food", "FOOD"),
						".\n\nDry rations have no expiry date."
					});
				}
			}

			// Token: 0x02003848 RID: 14408
			public class COLDWHEATBREAD
			{
				// Token: 0x0400DAC5 RID: 56005
				public static LocString NAME = UI.FormatAsLink("Frost Bun", "COLDWHEATBREAD");

				// Token: 0x0400DAC6 RID: 56006
				public static LocString DESC = "A simple bun baked from " + UI.FormatAsLink("Sleet Wheat Grain", "COLDWHEATSEED") + " grain.\n\nEach bite leaves a mild cooling sensation in one's mouth, even when the bun itself is warm.";

				// Token: 0x0400DAC7 RID: 56007
				public static LocString RECIPEDESC = "A simple bun baked from " + UI.FormatAsLink("Sleet Wheat Grain", "COLDWHEATSEED") + " grain.";
			}

			// Token: 0x02003849 RID: 14409
			public class BEAN
			{
				// Token: 0x0400DAC8 RID: 56008
				public static LocString NAME = UI.FormatAsLink("Nosh Bean", "BEAN");

				// Token: 0x0400DAC9 RID: 56009
				public static LocString DESC = "The crisp bean of a " + UI.FormatAsLink("Nosh Sprout", "BEAN_PLANT") + ".\n\nEach bite tastes refreshingly natural and wholesome.";
			}

			// Token: 0x0200384A RID: 14410
			public class SPICENUT
			{
				// Token: 0x0400DACA RID: 56010
				public static LocString NAME = UI.FormatAsLink("Pincha Peppernut", "SPICENUT");

				// Token: 0x0400DACB RID: 56011
				public static LocString DESC = "The flavorful nut of a " + UI.FormatAsLink("Pincha Pepperplant", "SPICE_VINE") + ".\n\nThe bitter outer rind hides a rich, peppery core that is useful in cooking.";
			}

			// Token: 0x0200384B RID: 14411
			public class SPICEBREAD
			{
				// Token: 0x0400DACC RID: 56012
				public static LocString NAME = UI.FormatAsLink("Pepper Bread", "SPICEBREAD");

				// Token: 0x0400DACD RID: 56013
				public static LocString DESC = "A loaf of bread, lightly spiced with " + UI.FormatAsLink("Pincha Peppernut", "SPICENUT") + " for a mild bite.\n\nThere's a simple joy to be had in pulling it apart in one's fingers.";

				// Token: 0x0400DACE RID: 56014
				public static LocString RECIPEDESC = "A loaf of bread, lightly spiced with " + UI.FormatAsLink("Pincha Peppernut", "SPICENUT") + " for a mild bite.";

				// Token: 0x0200384C RID: 14412
				public class DEHYDRATED
				{
					// Token: 0x0400DACF RID: 56015
					public static LocString NAME = "Dried Pepper Bread";

					// Token: 0x0400DAD0 RID: 56016
					public static LocString DESC = string.Concat(new string[]
					{
						"A dehydrated ",
						UI.FormatAsLink("Pepper Bread", "SPICEBREAD"),
						" ration. It must be rehydrated in order to be considered ",
						UI.FormatAsLink("Food", "FOOD"),
						".\n\nDry rations have no expiry date."
					});
				}
			}

			// Token: 0x0200384D RID: 14413
			public class SURFANDTURF
			{
				// Token: 0x0400DAD1 RID: 56017
				public static LocString NAME = UI.FormatAsLink("Surf'n'Turf", "SURFANDTURF");

				// Token: 0x0400DAD2 RID: 56018
				public static LocString DESC = string.Concat(new string[]
				{
					"A bit of ",
					UI.FormatAsLink("Meat", "MEAT"),
					" from the land and ",
					UI.FormatAsLink("Cooked Seafood", "COOKEDFISH"),
					" from the sea.\n\nIt's hearty and satisfying."
				});

				// Token: 0x0400DAD3 RID: 56019
				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					"A bit of ",
					UI.FormatAsLink("Meat", "MEAT"),
					" from the land and ",
					UI.FormatAsLink("Cooked Seafood", "COOKEDFISH"),
					" from the sea."
				});

				// Token: 0x0200384E RID: 14414
				public class DEHYDRATED
				{
					// Token: 0x0400DAD4 RID: 56020
					public static LocString NAME = "Dried Surf'n'Turf";

					// Token: 0x0400DAD5 RID: 56021
					public static LocString DESC = string.Concat(new string[]
					{
						"A dehydrated ",
						UI.FormatAsLink("Surf'n'Turf", "SURFANDTURF"),
						" ration. It must be rehydrated in order to be considered ",
						UI.FormatAsLink("Food", "FOOD"),
						".\n\nDry rations have no expiry date."
					});
				}
			}

			// Token: 0x0200384F RID: 14415
			public class TOFU
			{
				// Token: 0x0400DAD6 RID: 56022
				public static LocString NAME = UI.FormatAsLink("Tofu", "TOFU");

				// Token: 0x0400DAD7 RID: 56023
				public static LocString DESC = "A bland curd made from " + UI.FormatAsLink("Nosh Beans", "BEANPLANTSEED") + ".\n\nIt has an unusual but pleasant consistency.";

				// Token: 0x0400DAD8 RID: 56024
				public static LocString RECIPEDESC = "A bland curd made from " + UI.FormatAsLink("Nosh Beans", "BEANPLANTSEED") + ".";
			}

			// Token: 0x02003850 RID: 14416
			public class SPICYTOFU
			{
				// Token: 0x0400DAD9 RID: 56025
				public static LocString NAME = UI.FormatAsLink("Spicy Tofu", "SPICYTOFU");

				// Token: 0x0400DADA RID: 56026
				public static LocString DESC = ITEMS.FOOD.TOFU.NAME + " marinated in a flavorful " + UI.FormatAsLink("Pincha Peppernut", "SPICENUT") + " sauce.\n\nIt packs a delightful punch.";

				// Token: 0x0400DADB RID: 56027
				public static LocString RECIPEDESC = ITEMS.FOOD.TOFU.NAME + " marinated in a flavorful " + UI.FormatAsLink("Pincha Peppernut", "SPICENUT") + " sauce.";

				// Token: 0x02003851 RID: 14417
				public class DEHYDRATED
				{
					// Token: 0x0400DADC RID: 56028
					public static LocString NAME = "Dried Spicy Tofu";

					// Token: 0x0400DADD RID: 56029
					public static LocString DESC = string.Concat(new string[]
					{
						"A dehydrated ",
						UI.FormatAsLink("Spicy Tofu", "SPICYTOFU"),
						" ration. It must be rehydrated in order to be considered ",
						UI.FormatAsLink("Food", "FOOD"),
						".\n\nDry rations have no expiry date."
					});
				}
			}

			// Token: 0x02003852 RID: 14418
			public class CURRY
			{
				// Token: 0x0400DADE RID: 56030
				public static LocString NAME = UI.FormatAsLink("Curried Beans", "CURRY");

				// Token: 0x0400DADF RID: 56031
				public static LocString DESC = string.Concat(new string[]
				{
					"Chewy ",
					UI.FormatAsLink("Nosh Beans", "BEANPLANTSEED"),
					" simmered with chunks of ",
					ITEMS.INGREDIENTS.GINGER.NAME,
					".\n\nIt's so spicy!"
				});

				// Token: 0x0400DAE0 RID: 56032
				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					"Chewy ",
					UI.FormatAsLink("Nosh Beans", "BEANPLANTSEED"),
					" simmered with chunks of ",
					ITEMS.INGREDIENTS.GINGER.NAME,
					"."
				});

				// Token: 0x02003853 RID: 14419
				public class DEHYDRATED
				{
					// Token: 0x0400DAE1 RID: 56033
					public static LocString NAME = "Dried Curried Beans";

					// Token: 0x0400DAE2 RID: 56034
					public static LocString DESC = string.Concat(new string[]
					{
						"A dehydrated ",
						UI.FormatAsLink("Curried Beans", "CURRY"),
						" ration. It must be rehydrated in order to be considered ",
						UI.FormatAsLink("Food", "FOOD"),
						".\n\nDry rations have no expiry date."
					});
				}
			}

			// Token: 0x02003854 RID: 14420
			public class SALSA
			{
				// Token: 0x0400DAE3 RID: 56035
				public static LocString NAME = UI.FormatAsLink("Stuffed Berry", "SALSA");

				// Token: 0x0400DAE4 RID: 56036
				public static LocString DESC = "A baked " + UI.FormatAsLink("Bristle Berry", "PRICKLEFRUIT") + " stuffed with delectable spices and vibrantly flavored.";

				// Token: 0x0400DAE5 RID: 56037
				public static LocString RECIPEDESC = "A baked " + UI.FormatAsLink("Bristle Berry", "PRICKLEFRUIT") + " stuffed with delectable spices and vibrantly flavored.";

				// Token: 0x02003855 RID: 14421
				public class DEHYDRATED
				{
					// Token: 0x0400DAE6 RID: 56038
					public static LocString NAME = "Dried Stuffed Berry";

					// Token: 0x0400DAE7 RID: 56039
					public static LocString DESC = string.Concat(new string[]
					{
						"A dehydrated ",
						UI.FormatAsLink("Stuffed Berry", "SALSA"),
						" ration. It must be rehydrated in order to be considered ",
						UI.FormatAsLink("Food", "FOOD"),
						".\n\nDry rations have no expiry date."
					});
				}
			}

			// Token: 0x02003856 RID: 14422
			public class HARDSKINBERRY
			{
				// Token: 0x0400DAE8 RID: 56040
				public static LocString NAME = UI.FormatAsLink("Pikeapple", "HARDSKINBERRY");

				// Token: 0x0400DAE9 RID: 56041
				public static LocString DESC = "An edible fruit encased in a thorny husk.";
			}

			// Token: 0x02003857 RID: 14423
			public class CARROT
			{
				// Token: 0x0400DAEA RID: 56042
				public static LocString NAME = UI.FormatAsLink("Plume Squash", "CARROT");

				// Token: 0x0400DAEB RID: 56043
				public static LocString DESC = "An edible tuber with an earthy, elegant flavor.";
			}

			// Token: 0x02003858 RID: 14424
			public class PEMMICAN
			{
				// Token: 0x0400DAEC RID: 56044
				public static LocString NAME = UI.FormatAsLink("Pemmican", "PEMMICAN");

				// Token: 0x0400DAED RID: 56045
				public static LocString DESC = UI.FormatAsLink("Meat", "MEAT") + " and " + UI.FormatAsLink("Tallow", "TALLOW") + " pounded into a calorie-dense brick with an exceptionally long shelf life.\n\nSurvival never tasted so good.";

				// Token: 0x0400DAEE RID: 56046
				public static LocString RECIPEDESC = UI.FormatAsLink("Meat", "MEAT") + " and " + UI.FormatAsLink("Tallow", "TALLOW") + " pounded into a nutrient-dense brick with an exceptionally long shelf life.";
			}

			// Token: 0x02003859 RID: 14425
			public class BASICPLANTFOOD
			{
				// Token: 0x0400DAEF RID: 56047
				public static LocString NAME = UI.FormatAsLink("Meal Lice", "BASICPLANTFOOD");

				// Token: 0x0400DAF0 RID: 56048
				public static LocString DESC = "A flavorless grain that almost never wiggles on its own.";
			}

			// Token: 0x0200385A RID: 14426
			public class BASICPLANTBAR
			{
				// Token: 0x0400DAF1 RID: 56049
				public static LocString NAME = UI.FormatAsLink("Liceloaf", "BASICPLANTBAR");

				// Token: 0x0400DAF2 RID: 56050
				public static LocString DESC = UI.FormatAsLink("Meal Lice", "BASICPLANTFOOD") + " compacted into a dense, immobile loaf.";

				// Token: 0x0400DAF3 RID: 56051
				public static LocString RECIPEDESC = UI.FormatAsLink("Meal Lice", "BASICPLANTFOOD") + " compacted into a dense, immobile loaf.";
			}

			// Token: 0x0200385B RID: 14427
			public class BASICFORAGEPLANT
			{
				// Token: 0x0400DAF4 RID: 56052
				public static LocString NAME = UI.FormatAsLink("Muckroot", "BASICFORAGEPLANT");

				// Token: 0x0400DAF5 RID: 56053
				public static LocString DESC = "A seedless fruit with an upsettingly bland aftertaste.\n\nIt cannot be replanted.\n\nDigging up Buried Objects may uncover a " + ITEMS.FOOD.BASICFORAGEPLANT.NAME + ".";
			}

			// Token: 0x0200385C RID: 14428
			public class FORESTFORAGEPLANT
			{
				// Token: 0x0400DAF6 RID: 56054
				public static LocString NAME = UI.FormatAsLink("Hexalent Fruit", "FORESTFORAGEPLANT");

				// Token: 0x0400DAF7 RID: 56055
				public static LocString DESC = "A seedless fruit with an unusual rubbery texture.\n\nIt cannot be replanted.\n\nHexalent fruit is much more calorie dense than Muckroot fruit.";
			}

			// Token: 0x0200385D RID: 14429
			public class SWAMPFORAGEPLANT
			{
				// Token: 0x0400DAF8 RID: 56056
				public static LocString NAME = UI.FormatAsLink("Swamp Chard Heart", "SWAMPFORAGEPLANT");

				// Token: 0x0400DAF9 RID: 56057
				public static LocString DESC = "A seedless plant with a squishy, juicy center and an awful smell.\n\nIt cannot be replanted.";
			}

			// Token: 0x0200385E RID: 14430
			public class ICECAVESFORAGEPLANT
			{
				// Token: 0x0400DAFA RID: 56058
				public static LocString NAME = UI.FormatAsLink("Sherberry", "ICECAVESFORAGEPLANT");

				// Token: 0x0400DAFB RID: 56059
				public static LocString DESC = "A cold seedless fruit that triggers mild brain freeze.\n\nIt cannot be replanted.";
			}

			// Token: 0x0200385F RID: 14431
			public class ROTPILE
			{
				// Token: 0x0400DAFC RID: 56060
				public static LocString NAME = UI.FormatAsLink("Rot Pile", "COMPOST");

				// Token: 0x0400DAFD RID: 56061
				public static LocString DESC = string.Concat(new string[]
				{
					"An inedible glop of former foodstuff.\n\n",
					ITEMS.FOOD.ROTPILE.NAME,
					"s break down into ",
					UI.FormatAsLink("Polluted Dirt", "TOXICSAND"),
					" over time."
				});
			}

			// Token: 0x02003860 RID: 14432
			public class COLDWHEATSEED
			{
				// Token: 0x0400DAFE RID: 56062
				public static LocString NAME = UI.FormatAsLink("Sleet Wheat Grain", "COLDWHEATSEED");

				// Token: 0x0400DAFF RID: 56063
				public static LocString DESC = "An edible grain that leaves a cool taste on the tongue.";
			}

			// Token: 0x02003861 RID: 14433
			public class BEANPLANTSEED
			{
				// Token: 0x0400DB00 RID: 56064
				public static LocString NAME = UI.FormatAsLink("Nosh Bean", "BEANPLANTSEED");

				// Token: 0x0400DB01 RID: 56065
				public static LocString DESC = "An inedible bean that can be processed into delicious foods.";
			}

			// Token: 0x02003862 RID: 14434
			public class QUICHE
			{
				// Token: 0x0400DB02 RID: 56066
				public static LocString NAME = UI.FormatAsLink("Mushroom Quiche", "QUICHE");

				// Token: 0x0400DB03 RID: 56067
				public static LocString DESC = string.Concat(new string[]
				{
					UI.FormatAsLink("Omelette", "COOKEDEGG"),
					", ",
					UI.FormatAsLink("Fried Mushroom", "FRIEDMUSHROOM"),
					" and ",
					UI.FormatAsLink("Lettuce", "LETTUCE"),
					" piled onto a yummy crust.\n\nSomehow, it's both soggy <i>and</i> crispy."
				});

				// Token: 0x0400DB04 RID: 56068
				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					UI.FormatAsLink("Omelette", "COOKEDEGG"),
					", ",
					UI.FormatAsLink("Fried Mushroom", "FRIEDMUSHROOM"),
					" and ",
					UI.FormatAsLink("Lettuce", "LETTUCE"),
					" piled onto a yummy crust."
				});

				// Token: 0x02003863 RID: 14435
				public class DEHYDRATED
				{
					// Token: 0x0400DB05 RID: 56069
					public static LocString NAME = "Dried Mushroom Quiche";

					// Token: 0x0400DB06 RID: 56070
					public static LocString DESC = string.Concat(new string[]
					{
						"A dehydrated ",
						UI.FormatAsLink("Mushroom Quiche", "QUICHE"),
						" ration. It must be rehydrated in order to be considered ",
						UI.FormatAsLink("Food", "FOOD"),
						".\n\nDry rations have no expiry date."
					});
				}
			}
		}

		// Token: 0x02003864 RID: 14436
		public class INGREDIENTS
		{
			// Token: 0x02003865 RID: 14437
			public class SWAMPLILYFLOWER
			{
				// Token: 0x0400DB07 RID: 56071
				public static LocString NAME = UI.FormatAsLink("Balm Lily Flower", "SWAMPLILYFLOWER");

				// Token: 0x0400DB08 RID: 56072
				public static LocString DESC = "A medicinal flower that soothes most minor maladies.\n\nIt is exceptionally fragrant.";
			}

			// Token: 0x02003866 RID: 14438
			public class GINGER
			{
				// Token: 0x0400DB09 RID: 56073
				public static LocString NAME = UI.FormatAsLink("Tonic Root", "GINGERCONFIG");

				// Token: 0x0400DB0A RID: 56074
				public static LocString DESC = "A chewy, fibrous rhizome with a fiery aftertaste.";
			}
		}

		// Token: 0x02003867 RID: 14439
		public class INDUSTRIAL_PRODUCTS
		{
			// Token: 0x02003868 RID: 14440
			public class ELECTROBANK_MUCKROOT
			{
				// Token: 0x0400DB0B RID: 56075
				public static LocString NAME = UI.FormatAsLink("Muckroot Power Bank", "ELECTROBANK");

				// Token: 0x0400DB0C RID: 56076
				public static LocString DESC = string.Concat(new string[]
				{
					"A disposable organic ",
					UI.FormatAsLink("Power Bank", "ELECTROBANK"),
					" made with ",
					UI.FormatAsLink("Muckroot", "BASICFORAGEPLANT"),
					".\n\nIt can power buildings via ",
					UI.FormatAsLink("Socket Stations", "LARGEELECTROBANKDISCHARGER"),
					" or ",
					UI.FormatAsLink("Wall Sockets", "SMALLELECTROBANKDISCHARGER"),
					".\n\nDuplicants can produce new ",
					UI.FormatAsLink("Muckroot Power Banks", "ELECTROBANK"),
					" at the ",
					UI.FormatAsLink("Crafting Station", "CRAFTINGTABLE"),
					"."
				});
			}

			// Token: 0x02003869 RID: 14441
			public class ELECTROBANK_CARROT
			{
				// Token: 0x0400DB0D RID: 56077
				public static LocString NAME = UI.FormatAsLink("Squash Power Bank", "ELECTROBANK");

				// Token: 0x0400DB0E RID: 56078
				public static LocString DESC = string.Concat(new string[]
				{
					"A disposable organic ",
					UI.FormatAsLink("Power Bank", "ELECTROBANK"),
					" made with ",
					UI.FormatAsLink("Plume Squash", "CARROT"),
					".\n\nIt can power buildings via ",
					UI.FormatAsLink("Socket Stations", "LARGEELECTROBANKDISCHARGER"),
					" or ",
					UI.FormatAsLink("Wall Sockets", "SMALLELECTROBANKDISCHARGER"),
					".\n\nDuplicants can produce new ",
					UI.FormatAsLink("Squash Power Banks", "ELECTROBANK"),
					" at the ",
					UI.FormatAsLink("Crafting Station", "CRAFTINGTABLE"),
					"."
				});
			}

			// Token: 0x0200386A RID: 14442
			public class ELECTROBANK_LIGHTBUGEGG
			{
				// Token: 0x0400DB0F RID: 56079
				public static LocString NAME = UI.FormatAsLink("Shine Egg Power Bank", "ELECTROBANK");

				// Token: 0x0400DB10 RID: 56080
				public static LocString DESC = string.Concat(new string[]
				{
					"A disposable organic ",
					UI.FormatAsLink("Power Bank", "ELECTROBANK"),
					" made with ",
					UI.FormatAsLink("Shine Nymph Egg", "LIGHTBUG"),
					".\n\nIt can power buildings via ",
					UI.FormatAsLink("Socket Stations", "LARGEELECTROBANKDISCHARGER"),
					" or ",
					UI.FormatAsLink("Wall Sockets", "SMALLELECTROBANKDISCHARGER"),
					".\n\nDuplicants can produce new ",
					UI.FormatAsLink("Shine Egg Power Banks", "ELECTROBANK"),
					" at the ",
					UI.FormatAsLink("Crafting Station", "CRAFTINGTABLE"),
					"."
				});
			}

			// Token: 0x0200386B RID: 14443
			public class ELECTROBANK_SUCROSE
			{
				// Token: 0x0400DB11 RID: 56081
				public static LocString NAME = UI.FormatAsLink("Sucrose Power Bank", "ELECTROBANK");

				// Token: 0x0400DB12 RID: 56082
				public static LocString DESC = string.Concat(new string[]
				{
					"A disposable organic ",
					UI.FormatAsLink("Power Bank", "ELECTROBANK"),
					" made with ",
					UI.FormatAsLink("Sucrose", "SUCROSE"),
					".\n\nIt can power buildings via ",
					UI.FormatAsLink("Socket Stations", "LARGEELECTROBANKDISCHARGER"),
					" or ",
					UI.FormatAsLink("Wall Sockets", "SMALLELECTROBANKDISCHARGER"),
					".\n\nDuplicants can produce new ",
					UI.FormatAsLink("Sucrose Power Banks", "ELECTROBANK"),
					" at the ",
					UI.FormatAsLink("Crafting Station", "CRAFTINGTABLE"),
					"."
				});
			}

			// Token: 0x0200386C RID: 14444
			public class ELECTROBANK_STATERPILLAR
			{
				// Token: 0x0400DB13 RID: 56083
				public static LocString NAME = UI.FormatAsLink("Slug Egg Power Bank", "ELECTROBANK");

				// Token: 0x0400DB14 RID: 56084
				public static LocString DESC = string.Concat(new string[]
				{
					"A disposable organic ",
					UI.FormatAsLink("Power Bank", "ELECTROBANK"),
					" made with ",
					UI.FormatAsLink("Plug Slug Egg", "STATERPILLAR"),
					".\n\nIt can power buildings via ",
					UI.FormatAsLink("Socket Stations", "LARGEELECTROBANKDISCHARGER"),
					" or ",
					UI.FormatAsLink("Wall Sockets", "SMALLELECTROBANKDISCHARGER"),
					".\n\nDuplicants can produce new ",
					UI.FormatAsLink("Slug Egg Power Banks", "ELECTROBANK"),
					" at the ",
					UI.FormatAsLink("Crafting Station", "CRAFTINGTABLE"),
					"."
				});
			}

			// Token: 0x0200386D RID: 14445
			public class ELECTROBANK_URANIUM_ORE
			{
				// Token: 0x0400DB15 RID: 56085
				public static LocString NAME = UI.FormatAsLink("Nuclear Power Bank", "ELECTROBANK");

				// Token: 0x0400DB16 RID: 56086
				public static LocString DESC = string.Concat(new string[]
				{
					"A disposable ",
					UI.FormatAsLink("Power Bank", "ELECTROBANK"),
					" made with ",
					UI.FormatAsLink("Uranium Ore", "URANIUMORE"),
					".\n\nIt can power buildings via ",
					UI.FormatAsLink("Socket Stations", "LARGEELECTROBANKDISCHARGER"),
					" or ",
					UI.FormatAsLink("Wall Sockets", "SMALLELECTROBANKDISCHARGER"),
					".\n\nDuplicants can produce new ",
					UI.FormatAsLink("Nuclear Power Banks", "ELECTROBANK"),
					" at the ",
					UI.FormatAsLink("Crafting Station", "CRAFTINGTABLE"),
					"."
				});
			}

			// Token: 0x0200386E RID: 14446
			public class ELECTROBANK_METAL_ORE
			{
				// Token: 0x0400DB17 RID: 56087
				public static LocString NAME = UI.FormatAsLink("Metal Power Bank", "ELECTROBANK");

				// Token: 0x0400DB18 RID: 56088
				public static LocString DESC = string.Concat(new string[]
				{
					"A disposable ",
					UI.FormatAsLink("Power Bank", "ELECTROBANK"),
					" made with ",
					UI.FormatAsLink("Metal Ore", "METAL"),
					".\n\nIt can power buildings via ",
					UI.FormatAsLink("Socket Stations", "LARGEELECTROBANKDISCHARGER"),
					" or ",
					UI.FormatAsLink("Wall Sockets", "SMALLELECTROBANKDISCHARGER"),
					".\n\nDuplicants can produce new ",
					UI.FormatAsLink("Metal Power Banks", "ELECTROBANK"),
					" at the ",
					UI.FormatAsLink("Crafting Station", "CRAFTINGTABLE"),
					"."
				});
			}

			// Token: 0x0200386F RID: 14447
			public class ELECTROBANK
			{
				// Token: 0x0400DB19 RID: 56089
				public static LocString NAME = UI.FormatAsLink("Eco Power Bank", "ELECTROBANK");

				// Token: 0x0400DB1A RID: 56090
				public static LocString DESC = string.Concat(new string[]
				{
					"A rechargeable ",
					UI.FormatAsLink("Power Bank", "ELECTROBANK"),
					".\n\nIt can power buildings via ",
					UI.FormatAsLink("Socket Stations", "LARGEELECTROBANKDISCHARGER"),
					" or ",
					UI.FormatAsLink("Wall Sockets", "SMALLELECTROBANKDISCHARGER"),
					".\n\nDuplicants can produce new ",
					UI.FormatAsLink("Eco Power Banks", "ELECTROBANK"),
					" at the ",
					UI.FormatAsLink("Soldering Station", "ADVANCEDCRAFTINGTABLE"),
					"."
				});
			}

			// Token: 0x02003870 RID: 14448
			public class ELECTROBANK_EMPTY
			{
				// Token: 0x0400DB1B RID: 56091
				public static LocString NAME = UI.FormatAsLink("Empty Eco Power Bank", "ELECTROBANK");

				// Token: 0x0400DB1C RID: 56092
				public static LocString DESC = string.Concat(new string[]
				{
					"A depleted ",
					UI.FormatAsLink("Power Bank", "ELECTROBANK"),
					".\n\nIt must be recharged at a ",
					UI.FormatAsLink("Power Bank Charger", "ELECTROBANKCHARGER"),
					" before it can be reused."
				});
			}

			// Token: 0x02003871 RID: 14449
			public class ELECTROBANK_GARBAGE
			{
				// Token: 0x0400DB1D RID: 56093
				public static LocString NAME = UI.FormatAsLink("Power Bank Scrap", "ELECTROBANK");

				// Token: 0x0400DB1E RID: 56094
				public static LocString DESC = "A " + UI.FormatAsLink("Power Bank", "ELECTROBANK") + " that has reached the end of its life cycle.\n\nIt can be salvaged for metal ore.";
			}

			// Token: 0x02003872 RID: 14450
			public class FUEL_BRICK
			{
				// Token: 0x0400DB1F RID: 56095
				public static LocString NAME = "Fuel Brick";

				// Token: 0x0400DB20 RID: 56096
				public static LocString DESC = "A densely compressed brick of combustible material.\n\nIt can be burned to produce a one-time burst of " + UI.FormatAsLink("Power", "POWER") + ".";
			}

			// Token: 0x02003873 RID: 14451
			public class BASIC_FABRIC
			{
				// Token: 0x0400DB21 RID: 56097
				public static LocString NAME = UI.FormatAsLink("Reed Fiber", "BASIC_FABRIC");

				// Token: 0x0400DB22 RID: 56098
				public static LocString DESC = "A ball of raw cellulose used in the production of " + UI.FormatAsLink("Clothing", "EQUIPMENT") + " and textiles.";
			}

			// Token: 0x02003874 RID: 14452
			public class TRAP_PARTS
			{
				// Token: 0x0400DB23 RID: 56099
				public static LocString NAME = "Trap Components";

				// Token: 0x0400DB24 RID: 56100
				public static LocString DESC = string.Concat(new string[]
				{
					"These components can be assembled into a ",
					BUILDINGS.PREFABS.CREATURETRAP.NAME,
					" and used to catch ",
					UI.FormatAsLink("Critters", "CREATURES"),
					"."
				});
			}

			// Token: 0x02003875 RID: 14453
			public class POWER_STATION_TOOLS
			{
				// Token: 0x0400DB25 RID: 56101
				public static LocString NAME = "Microchip";

				// Token: 0x0400DB26 RID: 56102
				public static LocString DESC = string.Concat(new string[]
				{
					"A specialized ",
					ITEMS.INDUSTRIAL_PRODUCTS.POWER_STATION_TOOLS.NAME,
					" created by a professional engineer.\n\nTunes up ",
					UI.PRE_KEYWORD,
					"Generators",
					UI.PST_KEYWORD,
					" to increase their ",
					UI.FormatAsLink("Power", "POWER"),
					" output."
				});

				// Token: 0x0400DB27 RID: 56103
				public static LocString TINKER_REQUIREMENT_NAME = "Skill: " + DUPLICANTS.ROLES.POWER_TECHNICIAN.NAME;

				// Token: 0x0400DB28 RID: 56104
				public static LocString TINKER_REQUIREMENT_TOOLTIP = string.Concat(new string[]
				{
					"Can only be used by a Duplicant with ",
					DUPLICANTS.ROLES.POWER_TECHNICIAN.NAME,
					" to apply a ",
					UI.PRE_KEYWORD,
					"Tune Up",
					UI.PST_KEYWORD,
					"."
				});

				// Token: 0x0400DB29 RID: 56105
				public static LocString TINKER_EFFECT_NAME = "Engie's Tune-Up: {0} {1}";

				// Token: 0x0400DB2A RID: 56106
				public static LocString TINKER_EFFECT_TOOLTIP = string.Concat(new string[]
				{
					"Can be used to ",
					UI.PRE_KEYWORD,
					"Tune Up",
					UI.PST_KEYWORD,
					" a generator, increasing its {0} by <b>{1}</b>."
				});

				// Token: 0x0400DB2B RID: 56107
				public static LocString RECIPE_DESCRIPTION = "Make " + ITEMS.INDUSTRIAL_PRODUCTS.POWER_STATION_TOOLS.NAME + " from {0}";
			}

			// Token: 0x02003876 RID: 14454
			public class FARM_STATION_TOOLS
			{
				// Token: 0x0400DB2C RID: 56108
				public static LocString NAME = "Micronutrient Fertilizer";

				// Token: 0x0400DB2D RID: 56109
				public static LocString DESC = string.Concat(new string[]
				{
					"Specialized ",
					UI.FormatAsLink("Fertilizer", "FERTILIZER"),
					" mixed by a Duplicant with the ",
					DUPLICANTS.ROLES.FARMER.NAME,
					" Skill.\n\nIncreases the ",
					UI.PRE_KEYWORD,
					"Growth Rate",
					UI.PST_KEYWORD,
					" of one ",
					UI.FormatAsLink("Plant", "PLANTS"),
					"."
				});
			}

			// Token: 0x02003877 RID: 14455
			public class MACHINE_PARTS
			{
				// Token: 0x0400DB2E RID: 56110
				public static LocString NAME = "Custom Parts";

				// Token: 0x0400DB2F RID: 56111
				public static LocString DESC = string.Concat(new string[]
				{
					"Specialized Parts crafted by a professional engineer.\n\n",
					UI.PRE_KEYWORD,
					"Jerry Rig",
					UI.PST_KEYWORD,
					" machine buildings to increase their efficiency."
				});

				// Token: 0x0400DB30 RID: 56112
				public static LocString TINKER_REQUIREMENT_NAME = "Job: " + DUPLICANTS.ROLES.MECHATRONIC_ENGINEER.NAME;

				// Token: 0x0400DB31 RID: 56113
				public static LocString TINKER_REQUIREMENT_TOOLTIP = string.Concat(new string[]
				{
					"Can only be used by a Duplicant with ",
					DUPLICANTS.ROLES.MECHATRONIC_ENGINEER.NAME,
					" to apply a ",
					UI.PRE_KEYWORD,
					"Jerry Rig",
					UI.PST_KEYWORD,
					"."
				});

				// Token: 0x0400DB32 RID: 56114
				public static LocString TINKER_EFFECT_NAME = "Engineer's Jerry Rig: {0} {1}";

				// Token: 0x0400DB33 RID: 56115
				public static LocString TINKER_EFFECT_TOOLTIP = string.Concat(new string[]
				{
					"Can be used to ",
					UI.PRE_KEYWORD,
					"Jerry Rig",
					UI.PST_KEYWORD,
					" upgrades to a machine building, increasing its {0} by <b>{1}</b>."
				});
			}

			// Token: 0x02003878 RID: 14456
			public class RESEARCH_DATABANK
			{
				// Token: 0x0400DB34 RID: 56116
				public static LocString NAME = UI.FormatAsLink("Data Bank", "RESEARCH_DATABANK");

				// Token: 0x0400DB35 RID: 56117
				public static LocString DESC = "Raw data that can be processed into " + UI.FormatAsLink("Interstellar Research", "RESEARCH") + " points.";
			}

			// Token: 0x02003879 RID: 14457
			public class ORBITAL_RESEARCH_DATABANK
			{
				// Token: 0x0400DB36 RID: 56118
				public static LocString NAME = UI.FormatAsLink("Data Bank", "ORBITAL_RESEARCH_DATABANK");

				// Token: 0x0400DB37 RID: 56119
				public static LocString DESC = "Raw Data that can be processed into " + UI.FormatAsLink("Data Analysis Research", "RESEARCH") + " points.";

				// Token: 0x0400DB38 RID: 56120
				public static LocString RECIPE_DESC = string.Concat(new string[]
				{
					"Data Banks of raw data generated from exploring, either by exploring new areas with Duplicants, or by using an ",
					UI.FormatAsLink("Orbital Data Collection Lab", "ORBITALRESEARCHCENTER"),
					".\n\nUsed by the ",
					UI.FormatAsLink("Virtual Planetarium", "DLC1COSMICRESEARCHCENTER"),
					" to conduct research."
				});
			}

			// Token: 0x0200387A RID: 14458
			public class EGG_SHELL
			{
				// Token: 0x0400DB39 RID: 56121
				public static LocString NAME = UI.FormatAsLink("Egg Shell", "EGG_SHELL");

				// Token: 0x0400DB3A RID: 56122
				public static LocString DESC = "Can be crushed to produce " + UI.FormatAsLink("Lime", "LIME") + ".";
			}

			// Token: 0x0200387B RID: 14459
			public class GOLD_BELLY_CROWN
			{
				// Token: 0x0400DB3B RID: 56123
				public static LocString NAME = UI.FormatAsLink("Regal Bammoth Crest", "GOLD_BELLY_CROWN");

				// Token: 0x0400DB3C RID: 56124
				public static LocString DESC = "Can be crushed to produce " + ELEMENTS.GOLDAMALGAM.NAME + ".";
			}

			// Token: 0x0200387C RID: 14460
			public class CRAB_SHELL
			{
				// Token: 0x0400DB3D RID: 56125
				public static LocString NAME = UI.FormatAsLink("Pokeshell Molt", "CRAB_SHELL");

				// Token: 0x0400DB3E RID: 56126
				public static LocString DESC = "Can be crushed to produce " + UI.FormatAsLink("Lime", "LIME") + ".";

				// Token: 0x0200387D RID: 14461
				public class VARIANT_WOOD
				{
					// Token: 0x0400DB3F RID: 56127
					public static LocString NAME = UI.FormatAsLink("Oakshell Molt", "VARIANT_WOOD_SHELL");

					// Token: 0x0400DB40 RID: 56128
					public static LocString DESC = "Can be crushed to produce " + UI.FormatAsLink("Wood", "WOOD") + ".";
				}
			}

			// Token: 0x0200387E RID: 14462
			public class BABY_CRAB_SHELL
			{
				// Token: 0x0400DB41 RID: 56129
				public static LocString NAME = UI.FormatAsLink("Small Pokeshell Molt", "CRAB_SHELL");

				// Token: 0x0400DB42 RID: 56130
				public static LocString DESC = "Can be crushed to produce " + UI.FormatAsLink("Lime", "LIME") + ".";

				// Token: 0x0200387F RID: 14463
				public class VARIANT_WOOD
				{
					// Token: 0x0400DB43 RID: 56131
					public static LocString NAME = UI.FormatAsLink("Small Oakshell Molt", "VARIANT_WOOD_SHELL");

					// Token: 0x0400DB44 RID: 56132
					public static LocString DESC = "Can be crushed to produce " + UI.FormatAsLink("Wood", "WOOD") + ".";
				}
			}

			// Token: 0x02003880 RID: 14464
			public class WOOD
			{
				// Token: 0x0400DB45 RID: 56133
				public static LocString NAME = UI.FormatAsLink("Wood", "WOOD");

				// Token: 0x0400DB46 RID: 56134
				public static LocString DESC = string.Concat(new string[]
				{
					"Natural resource harvested from certain ",
					UI.FormatAsLink("Critters", "CREATURES"),
					" and ",
					UI.FormatAsLink("Plants", "PLANTS"),
					".\n\nUsed in construction or ",
					UI.FormatAsLink("Heat", "HEAT"),
					" production."
				});
			}

			// Token: 0x02003881 RID: 14465
			public class GENE_SHUFFLER_RECHARGE
			{
				// Token: 0x0400DB47 RID: 56135
				public static LocString NAME = "Vacillator Recharge";

				// Token: 0x0400DB48 RID: 56136
				public static LocString DESC = "Replenishes one charge to a depleted " + BUILDINGS.PREFABS.GENESHUFFLER.NAME + ".";
			}

			// Token: 0x02003882 RID: 14466
			public class TABLE_SALT
			{
				// Token: 0x0400DB49 RID: 56137
				public static LocString NAME = "Table Salt";

				// Token: 0x0400DB4A RID: 56138
				public static LocString DESC = string.Concat(new string[]
				{
					"A seasoning that Duplicants can add to their ",
					UI.FormatAsLink("Food", "FOOD"),
					" to boost ",
					UI.FormatAsLink("Morale", "MORALE"),
					".\n\nDuplicants will automatically use Table Salt while sitting at a ",
					BUILDINGS.PREFABS.DININGTABLE.NAME,
					" during mealtime.\n\n<i>Only the finest grains are chosen.</i>"
				});
			}

			// Token: 0x02003883 RID: 14467
			public class REFINED_SUGAR
			{
				// Token: 0x0400DB4B RID: 56139
				public static LocString NAME = "Refined Sugar";

				// Token: 0x0400DB4C RID: 56140
				public static LocString DESC = string.Concat(new string[]
				{
					"A seasoning that Duplicants can add to their ",
					UI.FormatAsLink("Food", "FOOD"),
					" to boost ",
					UI.FormatAsLink("Morale", "MORALE"),
					".\n\nDuplicants will automatically use Refined Sugar while sitting at a ",
					BUILDINGS.PREFABS.DININGTABLE.NAME,
					" during mealtime.\n\n<i>Only the finest grains are chosen.</i>"
				});
			}

			// Token: 0x02003884 RID: 14468
			public class ICE_BELLY_POOP
			{
				// Token: 0x0400DB4D RID: 56141
				public static LocString NAME = UI.FormatAsLink("Bammoth Patty", "ICE_BELLY_POOP");

				// Token: 0x0400DB4E RID: 56142
				public static LocString DESC = string.Concat(new string[]
				{
					"A little treat left behind by a very large critter.\n\nIt can be crushed to extract ",
					UI.FormatAsLink("Phosphorite", "PHOSPHORITE"),
					" and ",
					UI.FormatAsLink("Clay", "CLAY"),
					"."
				});
			}
		}

		// Token: 0x02003885 RID: 14469
		public class CARGO_CAPSULE
		{
			// Token: 0x0400DB4F RID: 56143
			public static LocString NAME = "Care Package";

			// Token: 0x0400DB50 RID: 56144
			public static LocString DESC = "A delivery system for recently printed resources.\n\nIt will dematerialize shortly.";
		}

		// Token: 0x02003886 RID: 14470
		public class RAILGUNPAYLOAD
		{
			// Token: 0x0400DB51 RID: 56145
			public static LocString NAME = UI.FormatAsLink("Interplanetary Payload", "RAILGUNPAYLOAD");

			// Token: 0x0400DB52 RID: 56146
			public static LocString DESC = string.Concat(new string[]
			{
				"Contains resources packed for interstellar shipping.\n\nCan be launched by a ",
				BUILDINGS.PREFABS.RAILGUN.NAME,
				" or unpacked with a ",
				BUILDINGS.PREFABS.RAILGUNPAYLOADOPENER.NAME,
				"."
			});
		}

		// Token: 0x02003887 RID: 14471
		public class MISSILE_BASIC
		{
			// Token: 0x0400DB53 RID: 56147
			public static LocString NAME = UI.FormatAsLink("Blastshot", "MISSILELAUNCHER");

			// Token: 0x0400DB54 RID: 56148
			public static LocString DESC = "An explosive projectile designed to defend against meteor showers.\n\nMust be launched by a " + UI.FormatAsLink("Meteor Blaster", "MISSILELAUNCHER") + ".";
		}

		// Token: 0x02003888 RID: 14472
		public class DEBRISPAYLOAD
		{
			// Token: 0x0400DB55 RID: 56149
			public static LocString NAME = "Rocket Debris";

			// Token: 0x0400DB56 RID: 56150
			public static LocString DESC = "Whatever is left over from a Rocket Self-Destruct can be recovered once it has crash-landed.";
		}

		// Token: 0x02003889 RID: 14473
		public class RADIATION
		{
			// Token: 0x0200388A RID: 14474
			public class HIGHENERGYPARITCLE
			{
				// Token: 0x0400DB57 RID: 56151
				public static LocString NAME = "Radbolts";

				// Token: 0x0400DB58 RID: 56152
				public static LocString DESC = string.Concat(new string[]
				{
					"A concentrated field of ",
					UI.FormatAsKeyWord("Radbolts"),
					" that can be largely redirected using a ",
					UI.FormatAsLink("Radbolt Reflector", "HIGHENERGYPARTICLEREDIRECTOR"),
					"."
				});
			}
		}

		// Token: 0x0200388B RID: 14475
		public class DREAMJOURNAL
		{
			// Token: 0x0400DB59 RID: 56153
			public static LocString NAME = "Dream Journal";

			// Token: 0x0400DB5A RID: 56154
			public static LocString DESC = string.Concat(new string[]
			{
				"A hand-scrawled account of ",
				UI.FormatAsLink("Pajama", "SLEEP_CLINIC_PAJAMAS"),
				"-induced dreams.\n\nCan be analyzed using a ",
				UI.FormatAsLink("Somnium Synthesizer", "MEGABRAINTANK"),
				"."
			});
		}

		// Token: 0x0200388C RID: 14476
		public class DEHYDRATEDFOODPACKAGE
		{
			// Token: 0x0400DB5B RID: 56155
			public static LocString NAME = "Dry Ration";

			// Token: 0x0400DB5C RID: 56156
			public static LocString DESC = "A package of non-perishable dehydrated food.\n\nIt requires no refrigeration, but must be rehydrated before consumption.";

			// Token: 0x0400DB5D RID: 56157
			public static LocString CONSUMED = "Ate Rehydrated Food";

			// Token: 0x0400DB5E RID: 56158
			public static LocString CONTENTS = "Dried {0}";
		}

		// Token: 0x0200388D RID: 14477
		public class SPICES
		{
			// Token: 0x0200388E RID: 14478
			public class MACHINERY_SPICE
			{
				// Token: 0x0400DB5F RID: 56159
				public static LocString NAME = UI.FormatAsLink("Machinist Spice", "MACHINERY_SPICE");

				// Token: 0x0400DB60 RID: 56160
				public static LocString DESC = "Improves operating skills when ingested.";
			}

			// Token: 0x0200388F RID: 14479
			public class PILOTING_SPICE
			{
				// Token: 0x0400DB61 RID: 56161
				public static LocString NAME = UI.FormatAsLink("Rocketeer Spice", "PILOTING_SPICE");

				// Token: 0x0400DB62 RID: 56162
				public static LocString DESC = "Provides a boost to piloting abilities.";
			}

			// Token: 0x02003890 RID: 14480
			public class PRESERVING_SPICE
			{
				// Token: 0x0400DB63 RID: 56163
				public static LocString NAME = UI.FormatAsLink("Freshener Spice", "PRESERVING_SPICE");

				// Token: 0x0400DB64 RID: 56164
				public static LocString DESC = "Slows the decomposition of perishable foods.";
			}

			// Token: 0x02003891 RID: 14481
			public class STRENGTH_SPICE
			{
				// Token: 0x0400DB65 RID: 56165
				public static LocString NAME = UI.FormatAsLink("Brawny Spice", "STRENGTH_SPICE");

				// Token: 0x0400DB66 RID: 56166
				public static LocString DESC = "Strengthens even the weakest of muscles.";
			}
		}
	}
}
