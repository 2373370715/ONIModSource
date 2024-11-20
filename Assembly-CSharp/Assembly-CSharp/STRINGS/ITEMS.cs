using System;

namespace STRINGS
{
	// Token: 0x020035C9 RID: 13769
	public class ITEMS
	{
		// Token: 0x020035CA RID: 13770
		public class PILLS
		{
			// Token: 0x020035CB RID: 13771
			public class PLACEBO
			{
				// Token: 0x0400D1DD RID: 53725
				public static LocString NAME = "Placebo";

				// Token: 0x0400D1DE RID: 53726
				public static LocString DESC = "A general, all-purpose " + UI.FormatAsLink("Medicine", "MEDICINE") + ".\n\nThe less one knows about it, the better it works.";

				// Token: 0x0400D1DF RID: 53727
				public static LocString RECIPEDESC = "All-purpose " + UI.FormatAsLink("Medicine", "MEDICINE") + ".";
			}

			// Token: 0x020035CC RID: 13772
			public class BASICBOOSTER
			{
				// Token: 0x0400D1E0 RID: 53728
				public static LocString NAME = "Vitamin Chews";

				// Token: 0x0400D1E1 RID: 53729
				public static LocString DESC = "Minorly reduces the chance of becoming sick.";

				// Token: 0x0400D1E2 RID: 53730
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

			// Token: 0x020035CD RID: 13773
			public class INTERMEDIATEBOOSTER
			{
				// Token: 0x0400D1E3 RID: 53731
				public static LocString NAME = "Immuno Booster";

				// Token: 0x0400D1E4 RID: 53732
				public static LocString DESC = "Significantly reduces the chance of becoming sick.";

				// Token: 0x0400D1E5 RID: 53733
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

			// Token: 0x020035CE RID: 13774
			public class ANTIHISTAMINE
			{
				// Token: 0x0400D1E6 RID: 53734
				public static LocString NAME = "Allergy Medication";

				// Token: 0x0400D1E7 RID: 53735
				public static LocString DESC = "Suppresses and prevents allergic reactions.";

				// Token: 0x0400D1E8 RID: 53736
				public static LocString RECIPEDESC = "A strong antihistamine Duplicants can take to halt an allergic reaction. " + ITEMS.PILLS.ANTIHISTAMINE.NAME + " will also prevent further reactions from occurring for a short time after ingestion.";
			}

			// Token: 0x020035CF RID: 13775
			public class BASICCURE
			{
				// Token: 0x0400D1E9 RID: 53737
				public static LocString NAME = "Curative Tablet";

				// Token: 0x0400D1EA RID: 53738
				public static LocString DESC = "A simple, easy-to-take remedy for minor germ-based diseases.";

				// Token: 0x0400D1EB RID: 53739
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

			// Token: 0x020035D0 RID: 13776
			public class INTERMEDIATECURE
			{
				// Token: 0x0400D1EC RID: 53740
				public static LocString NAME = "Medical Pack";

				// Token: 0x0400D1ED RID: 53741
				public static LocString DESC = "A doctor-administered cure for moderate ailments.";

				// Token: 0x0400D1EE RID: 53742
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

			// Token: 0x020035D1 RID: 13777
			public class ADVANCEDCURE
			{
				// Token: 0x0400D1EF RID: 53743
				public static LocString NAME = "Serum Vial";

				// Token: 0x0400D1F0 RID: 53744
				public static LocString DESC = "A doctor-administered cure for severe ailments.";

				// Token: 0x0400D1F1 RID: 53745
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

			// Token: 0x020035D2 RID: 13778
			public class BASICRADPILL
			{
				// Token: 0x0400D1F2 RID: 53746
				public static LocString NAME = "Basic Rad Pill";

				// Token: 0x0400D1F3 RID: 53747
				public static LocString DESC = "Increases a Duplicant's natural radiation absorption rate.";

				// Token: 0x0400D1F4 RID: 53748
				public static LocString RECIPEDESC = "A supplement that speeds up the rate at which a Duplicant body absorbs radiation, allowing them to manage increased radiation exposure.\n\nMust be taken daily.";
			}

			// Token: 0x020035D3 RID: 13779
			public class INTERMEDIATERADPILL
			{
				// Token: 0x0400D1F5 RID: 53749
				public static LocString NAME = "Intermediate Rad Pill";

				// Token: 0x0400D1F6 RID: 53750
				public static LocString DESC = "Increases a Duplicant's natural radiation absorption rate.";

				// Token: 0x0400D1F7 RID: 53751
				public static LocString RECIPEDESC = "A supplement that speeds up the rate at which a Duplicant body absorbs radiation, allowing them to manage increased radiation exposure.\n\nMust be taken daily.";
			}
		}

		// Token: 0x020035D4 RID: 13780
		public class FOOD
		{
			// Token: 0x0400D1F8 RID: 53752
			public static LocString COMPOST = "Compost";

			// Token: 0x020035D5 RID: 13781
			public class FOODSPLAT
			{
				// Token: 0x0400D1F9 RID: 53753
				public static LocString NAME = "Food Splatter";

				// Token: 0x0400D1FA RID: 53754
				public static LocString DESC = "Food smeared on the wall from a recent Food Fight";
			}

			// Token: 0x020035D6 RID: 13782
			public class BURGER
			{
				// Token: 0x0400D1FB RID: 53755
				public static LocString NAME = UI.FormatAsLink("Frost Burger", "BURGER");

				// Token: 0x0400D1FC RID: 53756
				public static LocString DESC = string.Concat(new string[]
				{
					UI.FormatAsLink("Meat", "MEAT"),
					" and ",
					UI.FormatAsLink("Lettuce", "LETTUCE"),
					" on a chilled ",
					UI.FormatAsLink("Frost Bun", "COLDWHEATBREAD"),
					".\n\nIt's the only burger best served cold."
				});

				// Token: 0x0400D1FD RID: 53757
				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					UI.FormatAsLink("Meat", "MEAT"),
					" and ",
					UI.FormatAsLink("Lettuce", "LETTUCE"),
					" on a chilled ",
					UI.FormatAsLink("Frost Bun", "COLDWHEATBREAD"),
					"."
				});

				// Token: 0x020035D7 RID: 13783
				public class DEHYDRATED
				{
					// Token: 0x0400D1FE RID: 53758
					public static LocString NAME = "Dried Frost Burger";

					// Token: 0x0400D1FF RID: 53759
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

			// Token: 0x020035D8 RID: 13784
			public class FIELDRATION
			{
				// Token: 0x0400D200 RID: 53760
				public static LocString NAME = UI.FormatAsLink("Nutrient Bar", "FIELDRATION");

				// Token: 0x0400D201 RID: 53761
				public static LocString DESC = "A nourishing nutrient paste, sandwiched between thin wafer layers.";
			}

			// Token: 0x020035D9 RID: 13785
			public class MUSHBAR
			{
				// Token: 0x0400D202 RID: 53762
				public static LocString NAME = UI.FormatAsLink("Mush Bar", "MUSHBAR");

				// Token: 0x0400D203 RID: 53763
				public static LocString DESC = "An edible, putrefied mudslop.\n\nMush Bars are preferable to starvation, but only just barely.";

				// Token: 0x0400D204 RID: 53764
				public static LocString RECIPEDESC = "An edible, putrefied mudslop.\n\n" + ITEMS.FOOD.MUSHBAR.NAME + "s are preferable to starvation, but only just barely.";
			}

			// Token: 0x020035DA RID: 13786
			public class MUSHROOMWRAP
			{
				// Token: 0x0400D205 RID: 53765
				public static LocString NAME = UI.FormatAsLink("Mushroom Wrap", "MUSHROOMWRAP");

				// Token: 0x0400D206 RID: 53766
				public static LocString DESC = string.Concat(new string[]
				{
					"Flavorful ",
					UI.FormatAsLink("Mushrooms", "MUSHROOM"),
					" wrapped in ",
					UI.FormatAsLink("Lettuce", "LETTUCE"),
					".\n\nIt has an earthy flavor punctuated by a refreshing crunch."
				});

				// Token: 0x0400D207 RID: 53767
				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					"Flavorful ",
					UI.FormatAsLink("Mushrooms", "MUSHROOM"),
					" wrapped in ",
					UI.FormatAsLink("Lettuce", "LETTUCE"),
					"."
				});

				// Token: 0x020035DB RID: 13787
				public class DEHYDRATED
				{
					// Token: 0x0400D208 RID: 53768
					public static LocString NAME = "Dried Mushroom Wrap";

					// Token: 0x0400D209 RID: 53769
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

			// Token: 0x020035DC RID: 13788
			public class MICROWAVEDLETTUCE
			{
				// Token: 0x0400D20A RID: 53770
				public static LocString NAME = UI.FormatAsLink("Microwaved Lettuce", "MICROWAVEDLETTUCE");

				// Token: 0x0400D20B RID: 53771
				public static LocString DESC = UI.FormatAsLink("Lettuce", "LETTUCE") + " scrumptiously wilted in the " + BUILDINGS.PREFABS.GAMMARAYOVEN.NAME + ".";

				// Token: 0x0400D20C RID: 53772
				public static LocString RECIPEDESC = UI.FormatAsLink("Lettuce", "LETTUCE") + " scrumptiously wilted in the " + BUILDINGS.PREFABS.GAMMARAYOVEN.NAME + ".";
			}

			// Token: 0x020035DD RID: 13789
			public class GAMMAMUSH
			{
				// Token: 0x0400D20D RID: 53773
				public static LocString NAME = UI.FormatAsLink("Gamma Mush", "GAMMAMUSH");

				// Token: 0x0400D20E RID: 53774
				public static LocString DESC = "A disturbingly delicious mixture of irradiated dirt and water.";

				// Token: 0x0400D20F RID: 53775
				public static LocString RECIPEDESC = UI.FormatAsLink("Mush Fry", "FRIEDMUSHBAR") + " reheated in a " + BUILDINGS.PREFABS.GAMMARAYOVEN.NAME + ".";
			}

			// Token: 0x020035DE RID: 13790
			public class FRUITCAKE
			{
				// Token: 0x0400D210 RID: 53776
				public static LocString NAME = UI.FormatAsLink("Berry Sludge", "FRUITCAKE");

				// Token: 0x0400D211 RID: 53777
				public static LocString DESC = "A mashed up " + UI.FormatAsLink("Bristle Berry", "PRICKLEFRUIT") + " sludge with an exceptionally long shelf life.\n\nIts aggressive, overbearing sweetness can leave the tongue feeling temporarily numb.";

				// Token: 0x0400D212 RID: 53778
				public static LocString RECIPEDESC = "A mashed up " + UI.FormatAsLink("Bristle Berry", "PRICKLEFRUIT") + " sludge with an exceptionally long shelf life.";
			}

			// Token: 0x020035DF RID: 13791
			public class POPCORN
			{
				// Token: 0x0400D213 RID: 53779
				public static LocString NAME = UI.FormatAsLink("Popcorn", "POPCORN");

				// Token: 0x0400D214 RID: 53780
				public static LocString DESC = UI.FormatAsLink("Sleet Wheat Grain", "COLDWHEATSEED") + " popped in a " + BUILDINGS.PREFABS.GAMMARAYOVEN.NAME + ".\n\nCompletely devoid of any fancy flavorings.";

				// Token: 0x0400D215 RID: 53781
				public static LocString RECIPEDESC = "Gamma-radiated " + UI.FormatAsLink("Sleet Wheat Grain", "COLDWHEATSEED") + ".";
			}

			// Token: 0x020035E0 RID: 13792
			public class SUSHI
			{
				// Token: 0x0400D216 RID: 53782
				public static LocString NAME = UI.FormatAsLink("Sushi", "SUSHI");

				// Token: 0x0400D217 RID: 53783
				public static LocString DESC = string.Concat(new string[]
				{
					"Raw ",
					UI.FormatAsLink("Pacu Fillet", "FISHMEAT"),
					" wrapped with fresh ",
					UI.FormatAsLink("Lettuce", "LETTUCE"),
					".\n\nWhile the salt of the lettuce may initially overpower the flavor, a keen palate can discern the subtle sweetness of the fillet beneath."
				});

				// Token: 0x0400D218 RID: 53784
				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					"Raw ",
					UI.FormatAsLink("Pacu Fillet", "FISHMEAT"),
					" wrapped with fresh ",
					UI.FormatAsLink("Lettuce", "LETTUCE"),
					"."
				});
			}

			// Token: 0x020035E1 RID: 13793
			public class HATCHEGG
			{
				// Token: 0x0400D219 RID: 53785
				public static LocString NAME = CREATURES.SPECIES.HATCH.EGG_NAME;

				// Token: 0x0400D21A RID: 53786
				public static LocString DESC = string.Concat(new string[]
				{
					"An egg laid by a ",
					UI.FormatAsLink("Hatch", "HATCH"),
					".\n\nIf incubated, it will hatch into a ",
					UI.FormatAsLink("Hatchling", "HATCH"),
					"."
				});

				// Token: 0x0400D21B RID: 53787
				public static LocString RECIPEDESC = "An egg laid by a " + UI.FormatAsLink("Hatch", "HATCH") + ".";
			}

			// Token: 0x020035E2 RID: 13794
			public class DRECKOEGG
			{
				// Token: 0x0400D21C RID: 53788
				public static LocString NAME = CREATURES.SPECIES.DRECKO.EGG_NAME;

				// Token: 0x0400D21D RID: 53789
				public static LocString DESC = string.Concat(new string[]
				{
					"An egg laid by a ",
					UI.FormatAsLink("Drecko", "DRECKO"),
					".\n\nIf incubated, it will hatch into a new ",
					UI.FormatAsLink("Drecklet", "DRECKO"),
					"."
				});

				// Token: 0x0400D21E RID: 53790
				public static LocString RECIPEDESC = "An egg laid by a " + UI.FormatAsLink("Drecko", "DRECKO") + ".";
			}

			// Token: 0x020035E3 RID: 13795
			public class LIGHTBUGEGG
			{
				// Token: 0x0400D21F RID: 53791
				public static LocString NAME = CREATURES.SPECIES.LIGHTBUG.EGG_NAME;

				// Token: 0x0400D220 RID: 53792
				public static LocString DESC = string.Concat(new string[]
				{
					"An egg laid by a ",
					UI.FormatAsLink("Shine Bug", "LIGHTBUG"),
					".\n\nIf incubated, it will hatch into a ",
					UI.FormatAsLink("Shine Nymph", "LIGHTBUG"),
					"."
				});

				// Token: 0x0400D221 RID: 53793
				public static LocString RECIPEDESC = "An egg laid by a " + UI.FormatAsLink("Shine Bug", "LIGHTBUG") + ".";
			}

			// Token: 0x020035E4 RID: 13796
			public class LETTUCE
			{
				// Token: 0x0400D222 RID: 53794
				public static LocString NAME = UI.FormatAsLink("Lettuce", "LETTUCE");

				// Token: 0x0400D223 RID: 53795
				public static LocString DESC = "Crunchy, slightly salty leaves from a " + UI.FormatAsLink("Waterweed", "SEALETTUCE") + " plant.";

				// Token: 0x0400D224 RID: 53796
				public static LocString RECIPEDESC = "Edible roughage from a " + UI.FormatAsLink("Waterweed", "SEALETTUCE") + ".";
			}

			// Token: 0x020035E5 RID: 13797
			public class PASTA
			{
				// Token: 0x0400D225 RID: 53797
				public static LocString NAME = UI.FormatAsLink("Pasta", "PASTA");

				// Token: 0x0400D226 RID: 53798
				public static LocString DESC = "pasta made from egg and wheat";

				// Token: 0x0400D227 RID: 53799
				public static LocString RECIPEDESC = "pasta made from egg and wheat";
			}

			// Token: 0x020035E6 RID: 13798
			public class PANCAKES
			{
				// Token: 0x0400D228 RID: 53800
				public static LocString NAME = UI.FormatAsLink("Soufflé Pancakes", "PANCAKES");

				// Token: 0x0400D229 RID: 53801
				public static LocString DESC = string.Concat(new string[]
				{
					"Sweet discs made from ",
					UI.FormatAsLink("Raw Egg", "RAWEGG"),
					" and ",
					UI.FormatAsLink("Sleet Wheat Grain", "COLDWHEATSEED"),
					".\n\nThey're so thick!"
				});

				// Token: 0x0400D22A RID: 53802
				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					"Sweet discs made from ",
					UI.FormatAsLink("Raw Egg", "RAWEGG"),
					" and ",
					UI.FormatAsLink("Sleet Wheat Grain", "COLDWHEATSEED"),
					"."
				});
			}

			// Token: 0x020035E7 RID: 13799
			public class OILFLOATEREGG
			{
				// Token: 0x0400D22B RID: 53803
				public static LocString NAME = CREATURES.SPECIES.OILFLOATER.EGG_NAME;

				// Token: 0x0400D22C RID: 53804
				public static LocString DESC = string.Concat(new string[]
				{
					"An egg laid by a ",
					UI.FormatAsLink("Slickster", "OILFLOATER"),
					".\n\nIf incubated, it will hatch into a ",
					UI.FormatAsLink("Slickster Larva", "OILFLOATER"),
					"."
				});

				// Token: 0x0400D22D RID: 53805
				public static LocString RECIPEDESC = "An egg laid by a " + UI.FormatAsLink("Slickster", "OILFLOATER") + ".";
			}

			// Token: 0x020035E8 RID: 13800
			public class PUFTEGG
			{
				// Token: 0x0400D22E RID: 53806
				public static LocString NAME = CREATURES.SPECIES.PUFT.EGG_NAME;

				// Token: 0x0400D22F RID: 53807
				public static LocString DESC = string.Concat(new string[]
				{
					"An egg laid by a ",
					UI.FormatAsLink("Puft", "PUFT"),
					".\n\nIf incubated, it will hatch into a ",
					UI.FormatAsLink("Puftlet", "PUFT"),
					"."
				});

				// Token: 0x0400D230 RID: 53808
				public static LocString RECIPEDESC = "An egg laid by a " + CREATURES.SPECIES.PUFT.NAME + ".";
			}

			// Token: 0x020035E9 RID: 13801
			public class FISHMEAT
			{
				// Token: 0x0400D231 RID: 53809
				public static LocString NAME = UI.FormatAsLink("Pacu Fillet", "FISHMEAT");

				// Token: 0x0400D232 RID: 53810
				public static LocString DESC = "An uncooked fillet from a very dead " + CREATURES.SPECIES.PACU.NAME + ". Yum!";
			}

			// Token: 0x020035EA RID: 13802
			public class MEAT
			{
				// Token: 0x0400D233 RID: 53811
				public static LocString NAME = UI.FormatAsLink("Meat", "MEAT");

				// Token: 0x0400D234 RID: 53812
				public static LocString DESC = "Uncooked meat from a very dead critter. Yum!";
			}

			// Token: 0x020035EB RID: 13803
			public class PLANTMEAT
			{
				// Token: 0x0400D235 RID: 53813
				public static LocString NAME = UI.FormatAsLink("Plant Meat", "PLANTMEAT");

				// Token: 0x0400D236 RID: 53814
				public static LocString DESC = "Planty plant meat from a plant. How nice!";
			}

			// Token: 0x020035EC RID: 13804
			public class SHELLFISHMEAT
			{
				// Token: 0x0400D237 RID: 53815
				public static LocString NAME = UI.FormatAsLink("Raw Shellfish", "SHELLFISHMEAT");

				// Token: 0x0400D238 RID: 53816
				public static LocString DESC = "An uncooked chunk of very dead " + CREATURES.SPECIES.CRAB.VARIANT_FRESH_WATER.NAME + ". Yum!";
			}

			// Token: 0x020035ED RID: 13805
			public class MUSHROOM
			{
				// Token: 0x0400D239 RID: 53817
				public static LocString NAME = UI.FormatAsLink("Mushroom", "MUSHROOM");

				// Token: 0x0400D23A RID: 53818
				public static LocString DESC = "An edible, flavorless fungus that grew in the dark.";
			}

			// Token: 0x020035EE RID: 13806
			public class COOKEDFISH
			{
				// Token: 0x0400D23B RID: 53819
				public static LocString NAME = UI.FormatAsLink("Cooked Seafood", "COOKEDFISH");

				// Token: 0x0400D23C RID: 53820
				public static LocString DESC = "A cooked piece of freshly caught aquatic critter.\n\nUnsurprisingly, it tastes a bit fishy.";

				// Token: 0x0400D23D RID: 53821
				public static LocString RECIPEDESC = "A cooked piece of freshly caught aquatic critter.";
			}

			// Token: 0x020035EF RID: 13807
			public class COOKEDMEAT
			{
				// Token: 0x0400D23E RID: 53822
				public static LocString NAME = UI.FormatAsLink("Barbeque", "COOKEDMEAT");

				// Token: 0x0400D23F RID: 53823
				public static LocString DESC = "The cooked meat of a defeated critter.\n\nIt has a delightful smoky aftertaste.";

				// Token: 0x0400D240 RID: 53824
				public static LocString RECIPEDESC = "The cooked meat of a defeated critter.";
			}

			// Token: 0x020035F0 RID: 13808
			public class FRIESCARROT
			{
				// Token: 0x0400D241 RID: 53825
				public static LocString NAME = UI.FormatAsLink("Squash Fries", "FRIESCARROT");

				// Token: 0x0400D242 RID: 53826
				public static LocString DESC = "Irresistibly crunchy.\n\nBest eaten hot.";

				// Token: 0x0400D243 RID: 53827
				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					"Crunchy sticks of ",
					UI.FormatAsLink("Plume Squash", "CARROT"),
					" deep-fried in ",
					UI.FormatAsLink("Tallow", "TALLOW"),
					"."
				});
			}

			// Token: 0x020035F1 RID: 13809
			public class DEEPFRIEDFISH
			{
				// Token: 0x0400D244 RID: 53828
				public static LocString NAME = UI.FormatAsLink("Fish Taco", "DEEPFRIEDFISH");

				// Token: 0x0400D245 RID: 53829
				public static LocString DESC = "Deep-fried fish cradled in a crunchy fin.";

				// Token: 0x0400D246 RID: 53830
				public static LocString RECIPEDESC = UI.FormatAsLink("Pacu Fillet", "FISHMEAT") + " lightly battered and deep-fried in " + UI.FormatAsLink("Tallow", "TALLOW") + ".";
			}

			// Token: 0x020035F2 RID: 13810
			public class DEEPFRIEDSHELLFISH
			{
				// Token: 0x0400D247 RID: 53831
				public static LocString NAME = UI.FormatAsLink("Shellfish Tempura", "DEEPFRIEDSHELLFISH");

				// Token: 0x0400D248 RID: 53832
				public static LocString DESC = "A crispy deep-fried critter claw.";

				// Token: 0x0400D249 RID: 53833
				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					"A tender chunk of battered ",
					UI.FormatAsLink("Raw Shellfish", "SHELLFISHMEAT"),
					" deep-fried in ",
					UI.FormatAsLink("Tallow", "TALLOW"),
					"."
				});
			}

			// Token: 0x020035F3 RID: 13811
			public class DEEPFRIEDMEAT
			{
				// Token: 0x0400D24A RID: 53834
				public static LocString NAME = UI.FormatAsLink("Deep Fried Steak", "DEEPFRIEDMEAT");

				// Token: 0x0400D24B RID: 53835
				public static LocString DESC = "A juicy slab of meat with a crunchy deep-fried upper layer.";

				// Token: 0x0400D24C RID: 53836
				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					"A juicy slab of ",
					UI.FormatAsLink("Raw Meat", "MEAT"),
					" deep-fried in ",
					UI.FormatAsLink("Tallow", "TALLOW"),
					"."
				});
			}

			// Token: 0x020035F4 RID: 13812
			public class DEEPFRIEDNOSH
			{
				// Token: 0x0400D24D RID: 53837
				public static LocString NAME = UI.FormatAsLink("Nosh Noms", "DEEPFRIEDNOSH");

				// Token: 0x0400D24E RID: 53838
				public static LocString DESC = "A snackable handful of crunchy beans.";

				// Token: 0x0400D24F RID: 53839
				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					"A crunchy stack of ",
					UI.FormatAsLink("Nosh Beans", "BEAN_PLANT"),
					" deep-fried in ",
					UI.FormatAsLink("Tallow", "TALLOW"),
					"."
				});
			}

			// Token: 0x020035F5 RID: 13813
			public class PICKLEDMEAL
			{
				// Token: 0x0400D250 RID: 53840
				public static LocString NAME = UI.FormatAsLink("Pickled Meal", "PICKLEDMEAL");

				// Token: 0x0400D251 RID: 53841
				public static LocString DESC = "Meal Lice preserved in vinegar.\n\nIt's a rarely acquired taste.";

				// Token: 0x0400D252 RID: 53842
				public static LocString RECIPEDESC = ITEMS.FOOD.BASICPLANTFOOD.NAME + " regrettably preserved in vinegar.";
			}

			// Token: 0x020035F6 RID: 13814
			public class FRIEDMUSHBAR
			{
				// Token: 0x0400D253 RID: 53843
				public static LocString NAME = UI.FormatAsLink("Mush Fry", "FRIEDMUSHBAR");

				// Token: 0x0400D254 RID: 53844
				public static LocString DESC = "Pan-fried, solidified mudslop.\n\nThe inside is almost completely uncooked, despite the crunch on the outside.";

				// Token: 0x0400D255 RID: 53845
				public static LocString RECIPEDESC = "Pan-fried, solidified mudslop.";
			}

			// Token: 0x020035F7 RID: 13815
			public class RAWEGG
			{
				// Token: 0x0400D256 RID: 53846
				public static LocString NAME = UI.FormatAsLink("Raw Egg", "RAWEGG");

				// Token: 0x0400D257 RID: 53847
				public static LocString DESC = "A raw Egg that has been cracked open for use in " + UI.FormatAsLink("Food", "FOOD") + " preparation.\n\nIt will never hatch.";

				// Token: 0x0400D258 RID: 53848
				public static LocString RECIPEDESC = "A raw egg that has been cracked open for use in " + UI.FormatAsLink("Food", "FOOD") + " preparation.";
			}

			// Token: 0x020035F8 RID: 13816
			public class COOKEDEGG
			{
				// Token: 0x0400D259 RID: 53849
				public static LocString NAME = UI.FormatAsLink("Omelette", "COOKEDEGG");

				// Token: 0x0400D25A RID: 53850
				public static LocString DESC = "Fluffed and folded Egg innards.\n\nIt turns out you do, in fact, have to break a few eggs to make it.";

				// Token: 0x0400D25B RID: 53851
				public static LocString RECIPEDESC = "Fluffed and folded egg innards.";
			}

			// Token: 0x020035F9 RID: 13817
			public class FRIEDMUSHROOM
			{
				// Token: 0x0400D25C RID: 53852
				public static LocString NAME = UI.FormatAsLink("Fried Mushroom", "FRIEDMUSHROOM");

				// Token: 0x0400D25D RID: 53853
				public static LocString DESC = "A pan-fried dish made with a fruiting " + UI.FormatAsLink("Dusk Cap", "MUSHROOM") + ".\n\nIt has a thick, savory flavor with subtle earthy undertones.";

				// Token: 0x0400D25E RID: 53854
				public static LocString RECIPEDESC = "A pan-fried dish made with a fruiting " + UI.FormatAsLink("Dusk Cap", "MUSHROOM") + ".";
			}

			// Token: 0x020035FA RID: 13818
			public class COOKEDPIKEAPPLE
			{
				// Token: 0x0400D25F RID: 53855
				public static LocString NAME = UI.FormatAsLink("Pikeapple Skewer", "COOKEDPIKEAPPLE");

				// Token: 0x0400D260 RID: 53856
				public static LocString DESC = "Grilling a " + UI.FormatAsLink("Pikeapple", "HARDSKINBERRY") + " softens its spikes, making it slighly less awkward to eat.\n\nIt does not diminish the smell.";

				// Token: 0x0400D261 RID: 53857
				public static LocString RECIPEDESC = "A grilled dish made with a fruiting " + UI.FormatAsLink("Pikeapple", "HARDSKINBERRY") + ".";
			}

			// Token: 0x020035FB RID: 13819
			public class PRICKLEFRUIT
			{
				// Token: 0x0400D262 RID: 53858
				public static LocString NAME = UI.FormatAsLink("Bristle Berry", "PRICKLEFRUIT");

				// Token: 0x0400D263 RID: 53859
				public static LocString DESC = "A sweet, mostly pleasant-tasting fruit covered in prickly barbs.";
			}

			// Token: 0x020035FC RID: 13820
			public class GRILLEDPRICKLEFRUIT
			{
				// Token: 0x0400D264 RID: 53860
				public static LocString NAME = UI.FormatAsLink("Gristle Berry", "GRILLEDPRICKLEFRUIT");

				// Token: 0x0400D265 RID: 53861
				public static LocString DESC = "The grilled bud of a " + UI.FormatAsLink("Bristle Berry", "PRICKLEFRUIT") + ".\n\nHeat unlocked an exquisite taste in the fruit, though the burnt spines leave something to be desired.";

				// Token: 0x0400D266 RID: 53862
				public static LocString RECIPEDESC = "The grilled bud of a " + UI.FormatAsLink("Bristle Berry", "PRICKLEFRUIT") + ".";
			}

			// Token: 0x020035FD RID: 13821
			public class SWAMPFRUIT
			{
				// Token: 0x0400D267 RID: 53863
				public static LocString NAME = UI.FormatAsLink("Bog Jelly", "SWAMPFRUIT");

				// Token: 0x0400D268 RID: 53864
				public static LocString DESC = "A fruit with an outer film that contains chewy gelatinous cubes.";
			}

			// Token: 0x020035FE RID: 13822
			public class SWAMPDELIGHTS
			{
				// Token: 0x0400D269 RID: 53865
				public static LocString NAME = UI.FormatAsLink("Swampy Delights", "SWAMPDELIGHTS");

				// Token: 0x0400D26A RID: 53866
				public static LocString DESC = "Dried gelatinous cubes from a " + UI.FormatAsLink("Bog Jelly", "SWAMPFRUIT") + ".\n\nEach cube has a wonderfully chewy texture and is lightly coated in a delicate powder.";

				// Token: 0x0400D26B RID: 53867
				public static LocString RECIPEDESC = "Dried gelatinous cubes from a " + UI.FormatAsLink("Bog Jelly", "SWAMPFRUIT") + ".";
			}

			// Token: 0x020035FF RID: 13823
			public class WORMBASICFRUIT
			{
				// Token: 0x0400D26C RID: 53868
				public static LocString NAME = UI.FormatAsLink("Spindly Grubfruit", "WORMBASICFRUIT");

				// Token: 0x0400D26D RID: 53869
				public static LocString DESC = "A " + UI.FormatAsLink("Grubfruit", "WORMSUPERFRUIT") + " that failed to develop properly.\n\nIt is nonetheless edible, and vaguely tasty.";
			}

			// Token: 0x02003600 RID: 13824
			public class WORMBASICFOOD
			{
				// Token: 0x0400D26E RID: 53870
				public static LocString NAME = UI.FormatAsLink("Roast Grubfruit Nut", "WORMBASICFOOD");

				// Token: 0x0400D26F RID: 53871
				public static LocString DESC = "Slow roasted " + UI.FormatAsLink("Spindly Grubfruit", "WORMBASICFRUIT") + ".\n\nIt has a smoky aroma and tastes of coziness.";

				// Token: 0x0400D270 RID: 53872
				public static LocString RECIPEDESC = "Slow roasted " + UI.FormatAsLink("Spindly Grubfruit", "WORMBASICFRUIT") + ".";
			}

			// Token: 0x02003601 RID: 13825
			public class WORMSUPERFRUIT
			{
				// Token: 0x0400D271 RID: 53873
				public static LocString NAME = UI.FormatAsLink("Grubfruit", "WORMSUPERFRUIT");

				// Token: 0x0400D272 RID: 53874
				public static LocString DESC = "A plump, healthy fruit with a honey-like taste.";
			}

			// Token: 0x02003602 RID: 13826
			public class WORMSUPERFOOD
			{
				// Token: 0x0400D273 RID: 53875
				public static LocString NAME = UI.FormatAsLink("Grubfruit Preserve", "WORMSUPERFOOD");

				// Token: 0x0400D274 RID: 53876
				public static LocString DESC = string.Concat(new string[]
				{
					"A long lasting ",
					UI.FormatAsLink("Grubfruit", "WORMSUPERFRUIT"),
					" jam preserved in ",
					UI.FormatAsLink("Sucrose", "SUCROSE"),
					".\n\nThe thick, goopy jam retains the shape of the jar when poured out, but the sweet taste can't be matched."
				});

				// Token: 0x0400D275 RID: 53877
				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					"A long lasting ",
					UI.FormatAsLink("Grubfruit", "WORMSUPERFRUIT"),
					" jam preserved in ",
					UI.FormatAsLink("Sucrose", "SUCROSE"),
					"."
				});
			}

			// Token: 0x02003603 RID: 13827
			public class BERRYPIE
			{
				// Token: 0x0400D276 RID: 53878
				public static LocString NAME = UI.FormatAsLink("Mixed Berry Pie", "BERRYPIE");

				// Token: 0x0400D277 RID: 53879
				public static LocString DESC = string.Concat(new string[]
				{
					"A pie made primarily of ",
					UI.FormatAsLink("Grubfruit", "WORMSUPERFRUIT"),
					" and ",
					UI.FormatAsLink("Gristle Berries", "PRICKLEFRUIT"),
					".\n\nThe mixture of berries creates a fragrant, colorful filling that packs a sweet punch."
				});

				// Token: 0x0400D278 RID: 53880
				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					"A pie made primarily of ",
					UI.FormatAsLink("Grubfruit", "WORMSUPERFRUIT"),
					" and ",
					UI.FormatAsLink("Gristle Berries", "PRICKLEFRUIT"),
					"."
				});

				// Token: 0x02003604 RID: 13828
				public class DEHYDRATED
				{
					// Token: 0x0400D279 RID: 53881
					public static LocString NAME = "Dried Berry Pie";

					// Token: 0x0400D27A RID: 53882
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

			// Token: 0x02003605 RID: 13829
			public class COLDWHEATBREAD
			{
				// Token: 0x0400D27B RID: 53883
				public static LocString NAME = UI.FormatAsLink("Frost Bun", "COLDWHEATBREAD");

				// Token: 0x0400D27C RID: 53884
				public static LocString DESC = "A simple bun baked from " + UI.FormatAsLink("Sleet Wheat Grain", "COLDWHEATSEED") + " grain.\n\nEach bite leaves a mild cooling sensation in one's mouth, even when the bun itself is warm.";

				// Token: 0x0400D27D RID: 53885
				public static LocString RECIPEDESC = "A simple bun baked from " + UI.FormatAsLink("Sleet Wheat Grain", "COLDWHEATSEED") + " grain.";
			}

			// Token: 0x02003606 RID: 13830
			public class BEAN
			{
				// Token: 0x0400D27E RID: 53886
				public static LocString NAME = UI.FormatAsLink("Nosh Bean", "BEAN");

				// Token: 0x0400D27F RID: 53887
				public static LocString DESC = "The crisp bean of a " + UI.FormatAsLink("Nosh Sprout", "BEAN_PLANT") + ".\n\nEach bite tastes refreshingly natural and wholesome.";
			}

			// Token: 0x02003607 RID: 13831
			public class SPICENUT
			{
				// Token: 0x0400D280 RID: 53888
				public static LocString NAME = UI.FormatAsLink("Pincha Peppernut", "SPICENUT");

				// Token: 0x0400D281 RID: 53889
				public static LocString DESC = "The flavorful nut of a " + UI.FormatAsLink("Pincha Pepperplant", "SPICE_VINE") + ".\n\nThe bitter outer rind hides a rich, peppery core that is useful in cooking.";
			}

			// Token: 0x02003608 RID: 13832
			public class SPICEBREAD
			{
				// Token: 0x0400D282 RID: 53890
				public static LocString NAME = UI.FormatAsLink("Pepper Bread", "SPICEBREAD");

				// Token: 0x0400D283 RID: 53891
				public static LocString DESC = "A loaf of bread, lightly spiced with " + UI.FormatAsLink("Pincha Peppernut", "SPICENUT") + " for a mild bite.\n\nThere's a simple joy to be had in pulling it apart in one's fingers.";

				// Token: 0x0400D284 RID: 53892
				public static LocString RECIPEDESC = "A loaf of bread, lightly spiced with " + UI.FormatAsLink("Pincha Peppernut", "SPICENUT") + " for a mild bite.";

				// Token: 0x02003609 RID: 13833
				public class DEHYDRATED
				{
					// Token: 0x0400D285 RID: 53893
					public static LocString NAME = "Dried Pepper Bread";

					// Token: 0x0400D286 RID: 53894
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

			// Token: 0x0200360A RID: 13834
			public class SURFANDTURF
			{
				// Token: 0x0400D287 RID: 53895
				public static LocString NAME = UI.FormatAsLink("Surf'n'Turf", "SURFANDTURF");

				// Token: 0x0400D288 RID: 53896
				public static LocString DESC = string.Concat(new string[]
				{
					"A bit of ",
					UI.FormatAsLink("Meat", "MEAT"),
					" from the land and ",
					UI.FormatAsLink("Cooked Seafood", "COOKEDFISH"),
					" from the sea.\n\nIt's hearty and satisfying."
				});

				// Token: 0x0400D289 RID: 53897
				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					"A bit of ",
					UI.FormatAsLink("Meat", "MEAT"),
					" from the land and ",
					UI.FormatAsLink("Cooked Seafood", "COOKEDFISH"),
					" from the sea."
				});

				// Token: 0x0200360B RID: 13835
				public class DEHYDRATED
				{
					// Token: 0x0400D28A RID: 53898
					public static LocString NAME = "Dried Surf'n'Turf";

					// Token: 0x0400D28B RID: 53899
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

			// Token: 0x0200360C RID: 13836
			public class TOFU
			{
				// Token: 0x0400D28C RID: 53900
				public static LocString NAME = UI.FormatAsLink("Tofu", "TOFU");

				// Token: 0x0400D28D RID: 53901
				public static LocString DESC = "A bland curd made from " + UI.FormatAsLink("Nosh Beans", "BEAN") + ".\n\nIt has an unusual but pleasant consistency.";

				// Token: 0x0400D28E RID: 53902
				public static LocString RECIPEDESC = "A bland curd made from " + UI.FormatAsLink("Nosh Beans", "BEAN") + ".";
			}

			// Token: 0x0200360D RID: 13837
			public class SPICYTOFU
			{
				// Token: 0x0400D28F RID: 53903
				public static LocString NAME = UI.FormatAsLink("Spicy Tofu", "SPICYTOFU");

				// Token: 0x0400D290 RID: 53904
				public static LocString DESC = ITEMS.FOOD.TOFU.NAME + " marinated in a flavorful " + UI.FormatAsLink("Pincha Peppernut", "SPICENUT") + " sauce.\n\nIt packs a delightful punch.";

				// Token: 0x0400D291 RID: 53905
				public static LocString RECIPEDESC = ITEMS.FOOD.TOFU.NAME + " marinated in a flavorful " + UI.FormatAsLink("Pincha Peppernut", "SPICENUT") + " sauce.";

				// Token: 0x0200360E RID: 13838
				public class DEHYDRATED
				{
					// Token: 0x0400D292 RID: 53906
					public static LocString NAME = "Dried Spicy Tofu";

					// Token: 0x0400D293 RID: 53907
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

			// Token: 0x0200360F RID: 13839
			public class CURRY
			{
				// Token: 0x0400D294 RID: 53908
				public static LocString NAME = UI.FormatAsLink("Curried Beans", "CURRY");

				// Token: 0x0400D295 RID: 53909
				public static LocString DESC = string.Concat(new string[]
				{
					"Chewy ",
					UI.FormatAsLink("Nosh Beans", "BEANPLANTSEED"),
					" simmered with chunks of ",
					ITEMS.INGREDIENTS.GINGER.NAME,
					".\n\nIt's so spicy!"
				});

				// Token: 0x0400D296 RID: 53910
				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					"Chewy ",
					UI.FormatAsLink("Nosh Beans", "BEANPLANTSEED"),
					" simmered with chunks of ",
					ITEMS.INGREDIENTS.GINGER.NAME,
					"."
				});

				// Token: 0x02003610 RID: 13840
				public class DEHYDRATED
				{
					// Token: 0x0400D297 RID: 53911
					public static LocString NAME = "Dried Curried Beans";

					// Token: 0x0400D298 RID: 53912
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

			// Token: 0x02003611 RID: 13841
			public class SALSA
			{
				// Token: 0x0400D299 RID: 53913
				public static LocString NAME = UI.FormatAsLink("Stuffed Berry", "SALSA");

				// Token: 0x0400D29A RID: 53914
				public static LocString DESC = "A baked " + UI.FormatAsLink("Bristle Berry", "PRICKLEFRUIT") + " stuffed with delectable spices and vibrantly flavored.";

				// Token: 0x0400D29B RID: 53915
				public static LocString RECIPEDESC = "A baked " + UI.FormatAsLink("Bristle Berry", "PRICKLEFRUIT") + " stuffed with delectable spices and vibrantly flavored.";

				// Token: 0x02003612 RID: 13842
				public class DEHYDRATED
				{
					// Token: 0x0400D29C RID: 53916
					public static LocString NAME = "Dried Stuffed Berry";

					// Token: 0x0400D29D RID: 53917
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

			// Token: 0x02003613 RID: 13843
			public class HARDSKINBERRY
			{
				// Token: 0x0400D29E RID: 53918
				public static LocString NAME = UI.FormatAsLink("Pikeapple", "HARDSKINBERRY");

				// Token: 0x0400D29F RID: 53919
				public static LocString DESC = "An edible fruit encased in a thorny husk.";
			}

			// Token: 0x02003614 RID: 13844
			public class CARROT
			{
				// Token: 0x0400D2A0 RID: 53920
				public static LocString NAME = UI.FormatAsLink("Plume Squash", "CARROT");

				// Token: 0x0400D2A1 RID: 53921
				public static LocString DESC = "An edible tuber with an earthy, elegant flavor.";
			}

			// Token: 0x02003615 RID: 13845
			public class PEMMICAN
			{
				// Token: 0x0400D2A2 RID: 53922
				public static LocString NAME = UI.FormatAsLink("Pemmican", "PEMMICAN");

				// Token: 0x0400D2A3 RID: 53923
				public static LocString DESC = UI.FormatAsLink("Meat", "MEAT") + " and " + UI.FormatAsLink("Tallow", "TALLOW") + " pounded into a calorie-dense brick with an exceptionally long shelf life.\n\nSurvival never tasted so good.";

				// Token: 0x0400D2A4 RID: 53924
				public static LocString RECIPEDESC = UI.FormatAsLink("Meat", "MEAT") + " and " + UI.FormatAsLink("Tallow", "TALLOW") + " pounded into a nutrient-dense brick with an exceptionally long shelf life.";
			}

			// Token: 0x02003616 RID: 13846
			public class BASICPLANTFOOD
			{
				// Token: 0x0400D2A5 RID: 53925
				public static LocString NAME = UI.FormatAsLink("Meal Lice", "BASICPLANTFOOD");

				// Token: 0x0400D2A6 RID: 53926
				public static LocString DESC = "A flavorless grain that almost never wiggles on its own.";
			}

			// Token: 0x02003617 RID: 13847
			public class BASICPLANTBAR
			{
				// Token: 0x0400D2A7 RID: 53927
				public static LocString NAME = UI.FormatAsLink("Liceloaf", "BASICPLANTBAR");

				// Token: 0x0400D2A8 RID: 53928
				public static LocString DESC = UI.FormatAsLink("Meal Lice", "BASICPLANTFOOD") + " compacted into a dense, immobile loaf.";

				// Token: 0x0400D2A9 RID: 53929
				public static LocString RECIPEDESC = UI.FormatAsLink("Meal Lice", "BASICPLANTFOOD") + " compacted into a dense, immobile loaf.";
			}

			// Token: 0x02003618 RID: 13848
			public class BASICFORAGEPLANT
			{
				// Token: 0x0400D2AA RID: 53930
				public static LocString NAME = UI.FormatAsLink("Muckroot", "BASICFORAGEPLANT");

				// Token: 0x0400D2AB RID: 53931
				public static LocString DESC = "A seedless fruit with an upsettingly bland aftertaste.\n\nIt cannot be replanted.\n\nDigging up Buried Objects may uncover a " + ITEMS.FOOD.BASICFORAGEPLANT.NAME + ".";
			}

			// Token: 0x02003619 RID: 13849
			public class FORESTFORAGEPLANT
			{
				// Token: 0x0400D2AC RID: 53932
				public static LocString NAME = UI.FormatAsLink("Hexalent Fruit", "FORESTFORAGEPLANT");

				// Token: 0x0400D2AD RID: 53933
				public static LocString DESC = "A seedless fruit with an unusual rubbery texture.\n\nIt cannot be replanted.\n\nHexalent fruit is much more calorie dense than Muckroot fruit.";
			}

			// Token: 0x0200361A RID: 13850
			public class SWAMPFORAGEPLANT
			{
				// Token: 0x0400D2AE RID: 53934
				public static LocString NAME = UI.FormatAsLink("Swamp Chard Heart", "SWAMPFORAGEPLANT");

				// Token: 0x0400D2AF RID: 53935
				public static LocString DESC = "A seedless plant with a squishy, juicy center and an awful smell.\n\nIt cannot be replanted.";
			}

			// Token: 0x0200361B RID: 13851
			public class ICECAVESFORAGEPLANT
			{
				// Token: 0x0400D2B0 RID: 53936
				public static LocString NAME = UI.FormatAsLink("Sherberry", "ICECAVESFORAGEPLANT");

				// Token: 0x0400D2B1 RID: 53937
				public static LocString DESC = "A cold seedless fruit that triggers mild brain freeze.\n\nIt cannot be replanted.";
			}

			// Token: 0x0200361C RID: 13852
			public class ROTPILE
			{
				// Token: 0x0400D2B2 RID: 53938
				public static LocString NAME = UI.FormatAsLink("Rot Pile", "COMPOST");

				// Token: 0x0400D2B3 RID: 53939
				public static LocString DESC = string.Concat(new string[]
				{
					"An inedible glop of former foodstuff.\n\n",
					ITEMS.FOOD.ROTPILE.NAME,
					"s break down into ",
					UI.FormatAsLink("Polluted Dirt", "TOXICSAND"),
					" over time."
				});
			}

			// Token: 0x0200361D RID: 13853
			public class COLDWHEATSEED
			{
				// Token: 0x0400D2B4 RID: 53940
				public static LocString NAME = UI.FormatAsLink("Sleet Wheat Grain", "COLDWHEATSEED");

				// Token: 0x0400D2B5 RID: 53941
				public static LocString DESC = "An edible grain that leaves a cool taste on the tongue.";
			}

			// Token: 0x0200361E RID: 13854
			public class BEANPLANTSEED
			{
				// Token: 0x0400D2B6 RID: 53942
				public static LocString NAME = UI.FormatAsLink("Nosh Bean", "BEANPLANTSEED");

				// Token: 0x0400D2B7 RID: 53943
				public static LocString DESC = "An inedible bean that can be processed into delicious foods.";
			}

			// Token: 0x0200361F RID: 13855
			public class QUICHE
			{
				// Token: 0x0400D2B8 RID: 53944
				public static LocString NAME = UI.FormatAsLink("Mushroom Quiche", "QUICHE");

				// Token: 0x0400D2B9 RID: 53945
				public static LocString DESC = string.Concat(new string[]
				{
					UI.FormatAsLink("Omelette", "COOKEDEGG"),
					", ",
					UI.FormatAsLink("Fried Mushroom", "FRIEDMUSHROOM"),
					" and ",
					UI.FormatAsLink("Lettuce", "LETTUCE"),
					" piled onto a yummy crust.\n\nSomehow, it's both soggy <i>and</i> crispy."
				});

				// Token: 0x0400D2BA RID: 53946
				public static LocString RECIPEDESC = string.Concat(new string[]
				{
					UI.FormatAsLink("Omelette", "COOKEDEGG"),
					", ",
					UI.FormatAsLink("Fried Mushroom", "FRIEDMUSHROOM"),
					" and ",
					UI.FormatAsLink("Lettuce", "LETTUCE"),
					" piled onto a yummy crust."
				});

				// Token: 0x02003620 RID: 13856
				public class DEHYDRATED
				{
					// Token: 0x0400D2BB RID: 53947
					public static LocString NAME = "Dried Mushroom Quiche";

					// Token: 0x0400D2BC RID: 53948
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

		// Token: 0x02003621 RID: 13857
		public class INGREDIENTS
		{
			// Token: 0x02003622 RID: 13858
			public class SWAMPLILYFLOWER
			{
				// Token: 0x0400D2BD RID: 53949
				public static LocString NAME = UI.FormatAsLink("Balm Lily Flower", "SWAMPLILYFLOWER");

				// Token: 0x0400D2BE RID: 53950
				public static LocString DESC = "A medicinal flower that soothes most minor maladies.\n\nIt is exceptionally fragrant.";
			}

			// Token: 0x02003623 RID: 13859
			public class GINGER
			{
				// Token: 0x0400D2BF RID: 53951
				public static LocString NAME = UI.FormatAsLink("Tonic Root", "GINGERCONFIG");

				// Token: 0x0400D2C0 RID: 53952
				public static LocString DESC = "A chewy, fibrous rhizome with a fiery aftertaste.";
			}
		}

		// Token: 0x02003624 RID: 13860
		public class INDUSTRIAL_PRODUCTS
		{
			// Token: 0x02003625 RID: 13861
			public class FUEL_BRICK
			{
				// Token: 0x0400D2C1 RID: 53953
				public static LocString NAME = "Fuel Brick";

				// Token: 0x0400D2C2 RID: 53954
				public static LocString DESC = "A densely compressed brick of combustible material.\n\nIt can be burned to produce a one-time burst of " + UI.FormatAsLink("Power", "POWER") + ".";
			}

			// Token: 0x02003626 RID: 13862
			public class BASIC_FABRIC
			{
				// Token: 0x0400D2C3 RID: 53955
				public static LocString NAME = UI.FormatAsLink("Reed Fiber", "BASIC_FABRIC");

				// Token: 0x0400D2C4 RID: 53956
				public static LocString DESC = "A ball of raw cellulose used in the production of " + UI.FormatAsLink("Clothing", "EQUIPMENT") + " and textiles.";
			}

			// Token: 0x02003627 RID: 13863
			public class TRAP_PARTS
			{
				// Token: 0x0400D2C5 RID: 53957
				public static LocString NAME = "Trap Components";

				// Token: 0x0400D2C6 RID: 53958
				public static LocString DESC = string.Concat(new string[]
				{
					"These components can be assembled into a ",
					BUILDINGS.PREFABS.CREATURETRAP.NAME,
					" and used to catch ",
					UI.FormatAsLink("Critters", "CREATURES"),
					"."
				});
			}

			// Token: 0x02003628 RID: 13864
			public class POWER_STATION_TOOLS
			{
				// Token: 0x0400D2C7 RID: 53959
				public static LocString NAME = "Microchip";

				// Token: 0x0400D2C8 RID: 53960
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

				// Token: 0x0400D2C9 RID: 53961
				public static LocString TINKER_REQUIREMENT_NAME = "Skill: " + DUPLICANTS.ROLES.POWER_TECHNICIAN.NAME;

				// Token: 0x0400D2CA RID: 53962
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

				// Token: 0x0400D2CB RID: 53963
				public static LocString TINKER_EFFECT_NAME = "Engie's Tune-Up: {0} {1}";

				// Token: 0x0400D2CC RID: 53964
				public static LocString TINKER_EFFECT_TOOLTIP = string.Concat(new string[]
				{
					"Can be used to ",
					UI.PRE_KEYWORD,
					"Tune Up",
					UI.PST_KEYWORD,
					" a generator, increasing its {0} by <b>{1}</b>."
				});
			}

			// Token: 0x02003629 RID: 13865
			public class FARM_STATION_TOOLS
			{
				// Token: 0x0400D2CD RID: 53965
				public static LocString NAME = "Micronutrient Fertilizer";

				// Token: 0x0400D2CE RID: 53966
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

			// Token: 0x0200362A RID: 13866
			public class MACHINE_PARTS
			{
				// Token: 0x0400D2CF RID: 53967
				public static LocString NAME = "Custom Parts";

				// Token: 0x0400D2D0 RID: 53968
				public static LocString DESC = string.Concat(new string[]
				{
					"Specialized Parts crafted by a professional engineer.\n\n",
					UI.PRE_KEYWORD,
					"Jerry Rig",
					UI.PST_KEYWORD,
					" machine buildings to increase their efficiency."
				});

				// Token: 0x0400D2D1 RID: 53969
				public static LocString TINKER_REQUIREMENT_NAME = "Job: " + DUPLICANTS.ROLES.MECHATRONIC_ENGINEER.NAME;

				// Token: 0x0400D2D2 RID: 53970
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

				// Token: 0x0400D2D3 RID: 53971
				public static LocString TINKER_EFFECT_NAME = "Engineer's Jerry Rig: {0} {1}";

				// Token: 0x0400D2D4 RID: 53972
				public static LocString TINKER_EFFECT_TOOLTIP = string.Concat(new string[]
				{
					"Can be used to ",
					UI.PRE_KEYWORD,
					"Jerry Rig",
					UI.PST_KEYWORD,
					" upgrades to a machine building, increasing its {0} by <b>{1}</b>."
				});
			}

			// Token: 0x0200362B RID: 13867
			public class RESEARCH_DATABANK
			{
				// Token: 0x0400D2D5 RID: 53973
				public static LocString NAME = "Data Bank";

				// Token: 0x0400D2D6 RID: 53974
				public static LocString DESC = "Raw data that can be processed into " + UI.FormatAsLink("Interstellar Research", "RESEARCH") + " points.";
			}

			// Token: 0x0200362C RID: 13868
			public class ORBITAL_RESEARCH_DATABANK
			{
				// Token: 0x0400D2D7 RID: 53975
				public static LocString NAME = "Data Bank";

				// Token: 0x0400D2D8 RID: 53976
				public static LocString DESC = "Raw Data that can be processed into " + UI.FormatAsLink("Data Analysis Research", "RESEARCH") + " points.";

				// Token: 0x0400D2D9 RID: 53977
				public static LocString RECIPE_DESC = string.Concat(new string[]
				{
					"Databanks of raw data generated from exploring, either by exploring new areas with Duplicants, or by using an ",
					UI.FormatAsLink("Orbital Data Collection Lab", "ORBITALRESEARCHCENTER"),
					".\n\nUsed by the ",
					UI.FormatAsLink("Virtual Planetarium", "DLC1COSMICRESEARCHCENTER"),
					" to conduct research."
				});
			}

			// Token: 0x0200362D RID: 13869
			public class EGG_SHELL
			{
				// Token: 0x0400D2DA RID: 53978
				public static LocString NAME = "Egg Shell";

				// Token: 0x0400D2DB RID: 53979
				public static LocString DESC = "Can be crushed to produce " + UI.FormatAsLink("Lime", "LIME") + ".";
			}

			// Token: 0x0200362E RID: 13870
			public class CRAB_SHELL
			{
				// Token: 0x0400D2DC RID: 53980
				public static LocString NAME = "Pokeshell Molt";

				// Token: 0x0400D2DD RID: 53981
				public static LocString DESC = "Can be crushed to produce " + UI.FormatAsLink("Lime", "LIME") + ".";

				// Token: 0x0200362F RID: 13871
				public class VARIANT_WOOD
				{
					// Token: 0x0400D2DE RID: 53982
					public static LocString NAME = "Oakshell Molt";

					// Token: 0x0400D2DF RID: 53983
					public static LocString DESC = "Can be crushed to produce " + UI.FormatAsLink("Wood", "WOOD") + ".";
				}
			}

			// Token: 0x02003630 RID: 13872
			public class BABY_CRAB_SHELL
			{
				// Token: 0x0400D2E0 RID: 53984
				public static LocString NAME = "Small Pokeshell Molt";

				// Token: 0x0400D2E1 RID: 53985
				public static LocString DESC = "Can be crushed to produce " + UI.FormatAsLink("Lime", "LIME") + ".";

				// Token: 0x02003631 RID: 13873
				public class VARIANT_WOOD
				{
					// Token: 0x0400D2E2 RID: 53986
					public static LocString NAME = "Small Oakshell Molt";

					// Token: 0x0400D2E3 RID: 53987
					public static LocString DESC = "Can be crushed to produce " + UI.FormatAsLink("Wood", "WOOD") + ".";
				}
			}

			// Token: 0x02003632 RID: 13874
			public class WOOD
			{
				// Token: 0x0400D2E4 RID: 53988
				public static LocString NAME = UI.FormatAsLink("Wood", "WOOD");

				// Token: 0x0400D2E5 RID: 53989
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

			// Token: 0x02003633 RID: 13875
			public class GENE_SHUFFLER_RECHARGE
			{
				// Token: 0x0400D2E6 RID: 53990
				public static LocString NAME = "Vacillator Recharge";

				// Token: 0x0400D2E7 RID: 53991
				public static LocString DESC = "Replenishes one charge to a depleted " + BUILDINGS.PREFABS.GENESHUFFLER.NAME + ".";
			}

			// Token: 0x02003634 RID: 13876
			public class TABLE_SALT
			{
				// Token: 0x0400D2E8 RID: 53992
				public static LocString NAME = "Table Salt";

				// Token: 0x0400D2E9 RID: 53993
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

			// Token: 0x02003635 RID: 13877
			public class REFINED_SUGAR
			{
				// Token: 0x0400D2EA RID: 53994
				public static LocString NAME = "Refined Sugar";

				// Token: 0x0400D2EB RID: 53995
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

			// Token: 0x02003636 RID: 13878
			public class ICE_BELLY_POOP
			{
				// Token: 0x0400D2EC RID: 53996
				public static LocString NAME = UI.FormatAsLink("Bammoth Patty", "ICE_BELLY_POOP");

				// Token: 0x0400D2ED RID: 53997
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

		// Token: 0x02003637 RID: 13879
		public class CARGO_CAPSULE
		{
			// Token: 0x0400D2EE RID: 53998
			public static LocString NAME = "Care Package";

			// Token: 0x0400D2EF RID: 53999
			public static LocString DESC = "A delivery system for recently printed resources.\n\nIt will dematerialize shortly.";
		}

		// Token: 0x02003638 RID: 13880
		public class RAILGUNPAYLOAD
		{
			// Token: 0x0400D2F0 RID: 54000
			public static LocString NAME = UI.FormatAsLink("Interplanetary Payload", "RAILGUNPAYLOAD");

			// Token: 0x0400D2F1 RID: 54001
			public static LocString DESC = string.Concat(new string[]
			{
				"Contains resources packed for interstellar shipping.\n\nCan be launched by a ",
				BUILDINGS.PREFABS.RAILGUN.NAME,
				" or unpacked with a ",
				BUILDINGS.PREFABS.RAILGUNPAYLOADOPENER.NAME,
				"."
			});
		}

		// Token: 0x02003639 RID: 13881
		public class MISSILE_BASIC
		{
			// Token: 0x0400D2F2 RID: 54002
			public static LocString NAME = UI.FormatAsLink("Blastshot", "MISSILELAUNCHER");

			// Token: 0x0400D2F3 RID: 54003
			public static LocString DESC = "An explosive projectile designed to defend against meteor showers.\n\nMust be launched by a " + UI.FormatAsLink("Meteor Blaster", "MISSILELAUNCHER") + ".";
		}

		// Token: 0x0200363A RID: 13882
		public class DEBRISPAYLOAD
		{
			// Token: 0x0400D2F4 RID: 54004
			public static LocString NAME = "Rocket Debris";

			// Token: 0x0400D2F5 RID: 54005
			public static LocString DESC = "Whatever is left over from a Rocket Self-Destruct can be recovered once it has crash-landed.";
		}

		// Token: 0x0200363B RID: 13883
		public class RADIATION
		{
			// Token: 0x0200363C RID: 13884
			public class HIGHENERGYPARITCLE
			{
				// Token: 0x0400D2F6 RID: 54006
				public static LocString NAME = "Radbolts";

				// Token: 0x0400D2F7 RID: 54007
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

		// Token: 0x0200363D RID: 13885
		public class DREAMJOURNAL
		{
			// Token: 0x0400D2F8 RID: 54008
			public static LocString NAME = "Dream Journal";

			// Token: 0x0400D2F9 RID: 54009
			public static LocString DESC = string.Concat(new string[]
			{
				"A hand-scrawled account of ",
				UI.FormatAsLink("Pajama", "SLEEP_CLINIC_PAJAMAS"),
				"-induced dreams.\n\nCan be analyzed using a ",
				UI.FormatAsLink("Somnium Synthesizer", "MEGABRAINTANK"),
				"."
			});
		}

		// Token: 0x0200363E RID: 13886
		public class DEHYDRATEDFOODPACKAGE
		{
			// Token: 0x0400D2FA RID: 54010
			public static LocString NAME = "Dry Ration";

			// Token: 0x0400D2FB RID: 54011
			public static LocString DESC = "A package of non-perishable dehydrated food.\n\nIt requires no refrigeration, but must be rehydrated before consumption.";

			// Token: 0x0400D2FC RID: 54012
			public static LocString CONSUMED = "Ate Rehydrated Food";

			// Token: 0x0400D2FD RID: 54013
			public static LocString CONTENTS = "Dried {0}";
		}

		// Token: 0x0200363F RID: 13887
		public class SPICES
		{
			// Token: 0x02003640 RID: 13888
			public class MACHINERY_SPICE
			{
				// Token: 0x0400D2FE RID: 54014
				public static LocString NAME = UI.FormatAsLink("Machinist Spice", "MACHINERY_SPICE");

				// Token: 0x0400D2FF RID: 54015
				public static LocString DESC = "Improves operating skills when ingested.";
			}

			// Token: 0x02003641 RID: 13889
			public class PILOTING_SPICE
			{
				// Token: 0x0400D300 RID: 54016
				public static LocString NAME = UI.FormatAsLink("Rocketeer Spice", "PILOTING_SPICE");

				// Token: 0x0400D301 RID: 54017
				public static LocString DESC = "Provides a boost to piloting abilities.";
			}

			// Token: 0x02003642 RID: 13890
			public class PRESERVING_SPICE
			{
				// Token: 0x0400D302 RID: 54018
				public static LocString NAME = UI.FormatAsLink("Freshener Spice", "PRESERVING_SPICE");

				// Token: 0x0400D303 RID: 54019
				public static LocString DESC = "Slows the decomposition of perishable foods.";
			}

			// Token: 0x02003643 RID: 13891
			public class STRENGTH_SPICE
			{
				// Token: 0x0400D304 RID: 54020
				public static LocString NAME = UI.FormatAsLink("Brawny Spice", "STRENGTH_SPICE");

				// Token: 0x0400D305 RID: 54021
				public static LocString DESC = "Strengthens even the weakest of muscles.";
			}
		}
	}
}
