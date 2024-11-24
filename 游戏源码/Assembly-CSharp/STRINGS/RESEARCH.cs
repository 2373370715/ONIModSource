using System;

namespace STRINGS
{
	// Token: 0x02003361 RID: 13153
	public class RESEARCH
	{
		// Token: 0x02003362 RID: 13154
		public class MESSAGING
		{
			// Token: 0x0400CE82 RID: 52866
			public static LocString NORESEARCHSELECTED = "No research selected";

			// Token: 0x0400CE83 RID: 52867
			public static LocString RESEARCHTYPEREQUIRED = "{0} required";

			// Token: 0x0400CE84 RID: 52868
			public static LocString RESEARCHTYPEALSOREQUIRED = "{0} also required";

			// Token: 0x0400CE85 RID: 52869
			public static LocString NO_RESEARCHER_SKILL = "No Researchers assigned";

			// Token: 0x0400CE86 RID: 52870
			public static LocString NO_RESEARCHER_SKILL_TOOLTIP = "The selected research focus requires {ResearchType} to complete\n\nOpen the " + UI.FormatAsManagementMenu("Skills Panel", global::Action.ManageSkills) + " and teach a Duplicant the {ResearchType} Skill to use this building";

			// Token: 0x0400CE87 RID: 52871
			public static LocString MISSING_RESEARCH_STATION = "Missing Research Station";

			// Token: 0x0400CE88 RID: 52872
			public static LocString MISSING_RESEARCH_STATION_TOOLTIP = "The selected research focus requires a {0} to perform\n\nOpen the " + UI.FormatAsBuildMenuTab("Stations Tab", global::Action.Plan10) + " of the Build Menu to construct one";

			// Token: 0x02003363 RID: 13155
			public static class DLC
			{
				// Token: 0x0400CE89 RID: 52873
				public static LocString EXPANSION1 = string.Concat(new string[]
				{
					UI.PRE_KEYWORD,
					"\n\n<i>",
					UI.DLC1.NAME,
					"</i>",
					UI.PST_KEYWORD,
					" DLC Content"
				});

				// Token: 0x0400CE8A RID: 52874
				public static LocString DLC_CONTENT = "\n<i>{0}</i> DLC Content";
			}
		}

		// Token: 0x02003364 RID: 13156
		public class TYPES
		{
			// Token: 0x0400CE8B RID: 52875
			public static LocString MISSINGRECIPEDESC = "Missing Recipe Description";

			// Token: 0x02003365 RID: 13157
			public class ALPHA
			{
				// Token: 0x0400CE8C RID: 52876
				public static LocString NAME = "Novice Research";

				// Token: 0x0400CE8D RID: 52877
				public static LocString DESC = UI.FormatAsLink("Novice Research", "RESEARCH") + " is required to unlock basic technologies.\nIt can be conducted at a " + UI.FormatAsLink("Research Station", "RESEARCHCENTER") + ".";

				// Token: 0x0400CE8E RID: 52878
				public static LocString RECIPEDESC = "Unlocks rudimentary technologies.";
			}

			// Token: 0x02003366 RID: 13158
			public class BETA
			{
				// Token: 0x0400CE8F RID: 52879
				public static LocString NAME = "Advanced Research";

				// Token: 0x0400CE90 RID: 52880
				public static LocString DESC = UI.FormatAsLink("Advanced Research", "RESEARCH") + " is required to unlock improved technologies.\nIt can be conducted at a " + UI.FormatAsLink("Super Computer", "ADVANCEDRESEARCHCENTER") + ".";

				// Token: 0x0400CE91 RID: 52881
				public static LocString RECIPEDESC = "Unlocks improved technologies.";
			}

			// Token: 0x02003367 RID: 13159
			public class GAMMA
			{
				// Token: 0x0400CE92 RID: 52882
				public static LocString NAME = "Interstellar Research";

				// Token: 0x0400CE93 RID: 52883
				public static LocString DESC = UI.FormatAsLink("Interstellar Research", "RESEARCH") + " is required to unlock space technologies.\nIt can be conducted at a " + UI.FormatAsLink("Virtual Planetarium", "COSMICRESEARCHCENTER") + ".";

				// Token: 0x0400CE94 RID: 52884
				public static LocString RECIPEDESC = "Unlocks cutting-edge technologies.";
			}

			// Token: 0x02003368 RID: 13160
			public class DELTA
			{
				// Token: 0x0400CE95 RID: 52885
				public static LocString NAME = "Applied Sciences Research";

				// Token: 0x0400CE96 RID: 52886
				public static LocString DESC = UI.FormatAsLink("Applied Sciences Research", "RESEARCH") + " is required to unlock materials science technologies.\nIt can be conducted at a " + UI.FormatAsLink("Materials Study Terminal", "NUCLEARRESEARCHCENTER") + ".";

				// Token: 0x0400CE97 RID: 52887
				public static LocString RECIPEDESC = "Unlocks next wave technologies.";
			}

			// Token: 0x02003369 RID: 13161
			public class ORBITAL
			{
				// Token: 0x0400CE98 RID: 52888
				public static LocString NAME = "Data Analysis Research";

				// Token: 0x0400CE99 RID: 52889
				public static LocString DESC = UI.FormatAsLink("Data Analysis Research", "RESEARCH") + " is required to unlock Data Analysis technologies.\nIt can be conducted at a " + UI.FormatAsLink("Orbital Data Collection Lab", "ORBITALRESEARCHCENTER") + ".";

				// Token: 0x0400CE9A RID: 52890
				public static LocString RECIPEDESC = "Unlocks out-of-this-world technologies.";
			}
		}

		// Token: 0x0200336A RID: 13162
		public class OTHER_TECH_ITEMS
		{
			// Token: 0x0200336B RID: 13163
			public class AUTOMATION_OVERLAY
			{
				// Token: 0x0400CE9B RID: 52891
				public static LocString NAME = UI.FormatAsOverlay("Automation Overlay");

				// Token: 0x0400CE9C RID: 52892
				public static LocString DESC = "Enables access to the " + UI.FormatAsOverlay("Automation Overlay") + ".";
			}

			// Token: 0x0200336C RID: 13164
			public class SUITS_OVERLAY
			{
				// Token: 0x0400CE9D RID: 52893
				public static LocString NAME = UI.FormatAsOverlay("Exosuit Overlay");

				// Token: 0x0400CE9E RID: 52894
				public static LocString DESC = "Enables access to the " + UI.FormatAsOverlay("Exosuit Overlay") + ".";
			}

			// Token: 0x0200336D RID: 13165
			public class JET_SUIT
			{
				// Token: 0x0400CE9F RID: 52895
				public static LocString NAME = UI.PRE_KEYWORD + "Jet Suit" + UI.PST_KEYWORD + " Pattern";

				// Token: 0x0400CEA0 RID: 52896
				public static LocString DESC = string.Concat(new string[]
				{
					"Enables fabrication of ",
					UI.PRE_KEYWORD,
					"Jet Suits",
					UI.PST_KEYWORD,
					" at the ",
					BUILDINGS.PREFABS.SUITFABRICATOR.NAME
				});
			}

			// Token: 0x0200336E RID: 13166
			public class OXYGEN_MASK
			{
				// Token: 0x0400CEA1 RID: 52897
				public static LocString NAME = UI.PRE_KEYWORD + "Oxygen Mask" + UI.PST_KEYWORD + " Pattern";

				// Token: 0x0400CEA2 RID: 52898
				public static LocString DESC = string.Concat(new string[]
				{
					"Enables fabrication of ",
					UI.PRE_KEYWORD,
					"Oxygen Masks",
					UI.PST_KEYWORD,
					" at the ",
					BUILDINGS.PREFABS.CRAFTINGTABLE.NAME
				});
			}

			// Token: 0x0200336F RID: 13167
			public class LEAD_SUIT
			{
				// Token: 0x0400CEA3 RID: 52899
				public static LocString NAME = UI.PRE_KEYWORD + "Lead Suit" + UI.PST_KEYWORD + " Pattern";

				// Token: 0x0400CEA4 RID: 52900
				public static LocString DESC = string.Concat(new string[]
				{
					"Enables fabrication of ",
					UI.PRE_KEYWORD,
					"Lead Suits",
					UI.PST_KEYWORD,
					" at the ",
					BUILDINGS.PREFABS.SUITFABRICATOR.NAME
				});
			}

			// Token: 0x02003370 RID: 13168
			public class ATMO_SUIT
			{
				// Token: 0x0400CEA5 RID: 52901
				public static LocString NAME = UI.PRE_KEYWORD + "Atmo Suit" + UI.PST_KEYWORD + " Pattern";

				// Token: 0x0400CEA6 RID: 52902
				public static LocString DESC = string.Concat(new string[]
				{
					"Enables fabrication of ",
					UI.PRE_KEYWORD,
					"Atmo Suits",
					UI.PST_KEYWORD,
					" at the ",
					BUILDINGS.PREFABS.SUITFABRICATOR.NAME
				});
			}

			// Token: 0x02003371 RID: 13169
			public class BETA_RESEARCH_POINT
			{
				// Token: 0x0400CEA7 RID: 52903
				public static LocString NAME = UI.PRE_KEYWORD + "Advanced Research" + UI.PST_KEYWORD + " Capability";

				// Token: 0x0400CEA8 RID: 52904
				public static LocString DESC = string.Concat(new string[]
				{
					"Allows ",
					UI.PRE_KEYWORD,
					"Advanced Research",
					UI.PST_KEYWORD,
					" points to be accumulated, unlocking higher technology tiers."
				});
			}

			// Token: 0x02003372 RID: 13170
			public class GAMMA_RESEARCH_POINT
			{
				// Token: 0x0400CEA9 RID: 52905
				public static LocString NAME = UI.PRE_KEYWORD + "Interstellar Research" + UI.PST_KEYWORD + " Capability";

				// Token: 0x0400CEAA RID: 52906
				public static LocString DESC = string.Concat(new string[]
				{
					"Allows ",
					UI.PRE_KEYWORD,
					"Interstellar Research",
					UI.PST_KEYWORD,
					" points to be accumulated, unlocking higher technology tiers."
				});
			}

			// Token: 0x02003373 RID: 13171
			public class DELTA_RESEARCH_POINT
			{
				// Token: 0x0400CEAB RID: 52907
				public static LocString NAME = UI.PRE_KEYWORD + "Materials Science Research" + UI.PST_KEYWORD + " Capability";

				// Token: 0x0400CEAC RID: 52908
				public static LocString DESC = string.Concat(new string[]
				{
					"Allows ",
					UI.PRE_KEYWORD,
					"Materials Science Research",
					UI.PST_KEYWORD,
					" points to be accumulated, unlocking higher technology tiers."
				});
			}

			// Token: 0x02003374 RID: 13172
			public class ORBITAL_RESEARCH_POINT
			{
				// Token: 0x0400CEAD RID: 52909
				public static LocString NAME = UI.PRE_KEYWORD + "Data Analysis Research" + UI.PST_KEYWORD + " Capability";

				// Token: 0x0400CEAE RID: 52910
				public static LocString DESC = string.Concat(new string[]
				{
					"Allows ",
					UI.PRE_KEYWORD,
					"Data Analysis Research",
					UI.PST_KEYWORD,
					" points to be accumulated, unlocking higher technology tiers."
				});
			}

			// Token: 0x02003375 RID: 13173
			public class CONVEYOR_OVERLAY
			{
				// Token: 0x0400CEAF RID: 52911
				public static LocString NAME = UI.FormatAsOverlay("Conveyor Overlay");

				// Token: 0x0400CEB0 RID: 52912
				public static LocString DESC = "Enables access to the " + UI.FormatAsOverlay("Conveyor Overlay") + ".";
			}

			// Token: 0x02003376 RID: 13174
			public class DISPOSABLE_ELECTROBANK_ORGANIC
			{
				// Token: 0x0400CEB1 RID: 52913
				public static LocString NAME = UI.PRE_KEYWORD + "Organic Power Bank" + UI.PST_KEYWORD + " Pattern";

				// Token: 0x0400CEB2 RID: 52914
				public static LocString DESC = string.Concat(new string[]
				{
					"Enables fabrication of ",
					UI.PRE_KEYWORD,
					"Organic Power Banks",
					UI.PST_KEYWORD,
					" at the ",
					BUILDINGS.PREFABS.CRAFTINGTABLE.NAME
				});
			}

			// Token: 0x02003377 RID: 13175
			public class DISPOSABLE_ELECTROBANK_URANIUM_ORE
			{
				// Token: 0x0400CEB3 RID: 52915
				public static LocString NAME = UI.PRE_KEYWORD + "Nuclear Power Bank" + UI.PST_KEYWORD + " Pattern";

				// Token: 0x0400CEB4 RID: 52916
				public static LocString DESC = string.Concat(new string[]
				{
					"Enables fabrication of ",
					UI.PRE_KEYWORD,
					"Nuclear Power Banks",
					UI.PST_KEYWORD,
					" at the ",
					BUILDINGS.PREFABS.CRAFTINGTABLE.NAME
				});
			}

			// Token: 0x02003378 RID: 13176
			public class ELECTROBANK
			{
				// Token: 0x0400CEB5 RID: 52917
				public static LocString NAME = UI.PRE_KEYWORD + "Eco Power Bank" + UI.PST_KEYWORD + " Pattern";

				// Token: 0x0400CEB6 RID: 52918
				public static LocString DESC = string.Concat(new string[]
				{
					"Enables fabrication of ",
					UI.PRE_KEYWORD,
					"Eco Power Banks",
					UI.PST_KEYWORD,
					" at the ",
					BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.NAME
				});
			}

			// Token: 0x02003379 RID: 13177
			public class PILOTINGBOOSTER
			{
				// Token: 0x0400CEB7 RID: 52919
				public static LocString NAME = UI.PRE_KEYWORD + "Rocketry Booster" + UI.PST_KEYWORD + " Pattern";

				// Token: 0x0400CEB8 RID: 52920
				public static LocString DESC = string.Concat(new string[]
				{
					"Enables fabrication of ",
					UI.PRE_KEYWORD,
					"Rocketry Boosters",
					UI.PST_KEYWORD,
					" for Bionic Duplicants at the ",
					BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.NAME
				});
			}

			// Token: 0x0200337A RID: 13178
			public class CONSTRUCTIONBOOSTER
			{
				// Token: 0x0400CEB9 RID: 52921
				public static LocString NAME = UI.PRE_KEYWORD + "Building Booster" + UI.PST_KEYWORD + " Pattern";

				// Token: 0x0400CEBA RID: 52922
				public static LocString DESC = string.Concat(new string[]
				{
					"Enables fabrication of ",
					UI.PRE_KEYWORD,
					"Building Boosters",
					UI.PST_KEYWORD,
					" for Bionic Duplicants at the ",
					BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.NAME
				});
			}

			// Token: 0x0200337B RID: 13179
			public class EXCAVATIONBOOSTER
			{
				// Token: 0x0400CEBB RID: 52923
				public static LocString NAME = UI.PRE_KEYWORD + "Digging Booster" + UI.PST_KEYWORD + " Pattern";

				// Token: 0x0400CEBC RID: 52924
				public static LocString DESC = string.Concat(new string[]
				{
					"Enables fabrication of ",
					UI.PRE_KEYWORD,
					"Digging Boosters",
					UI.PST_KEYWORD,
					" for Bionic Duplicants at the ",
					BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.NAME
				});
			}

			// Token: 0x0200337C RID: 13180
			public class EXPLORERBOOSTER
			{
				// Token: 0x0400CEBD RID: 52925
				public static LocString NAME = UI.PRE_KEYWORD + "Dowsing Booster" + UI.PST_KEYWORD + " Pattern";

				// Token: 0x0400CEBE RID: 52926
				public static LocString DESC = string.Concat(new string[]
				{
					"Enables fabrication of ",
					UI.PRE_KEYWORD,
					"Dowsing Boosters",
					UI.PST_KEYWORD,
					" for Bionic Duplicants at the ",
					BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.NAME
				});
			}

			// Token: 0x0200337D RID: 13181
			public class MACHINERYBOOSTER
			{
				// Token: 0x0400CEBF RID: 52927
				public static LocString NAME = UI.PRE_KEYWORD + "Operating Booster" + UI.PST_KEYWORD + " Pattern";

				// Token: 0x0400CEC0 RID: 52928
				public static LocString DESC = string.Concat(new string[]
				{
					"Enables fabrication of ",
					UI.PRE_KEYWORD,
					"Operating Boosters",
					UI.PST_KEYWORD,
					" for Bionic Duplicants at the ",
					BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.NAME
				});
			}

			// Token: 0x0200337E RID: 13182
			public class ATHLETICSBOOSTER
			{
				// Token: 0x0400CEC1 RID: 52929
				public static LocString NAME = UI.PRE_KEYWORD + "Athletics Booster" + UI.PST_KEYWORD + " Pattern";

				// Token: 0x0400CEC2 RID: 52930
				public static LocString DESC = string.Concat(new string[]
				{
					"Enables fabrication of ",
					UI.PRE_KEYWORD,
					"Athletics Boosters",
					UI.PST_KEYWORD,
					" for Bionic Duplicants at the ",
					BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.NAME
				});
			}

			// Token: 0x0200337F RID: 13183
			public class SCIENCEBOOSTER
			{
				// Token: 0x0400CEC3 RID: 52931
				public static LocString NAME = UI.PRE_KEYWORD + "Researching Booster" + UI.PST_KEYWORD + " Pattern";

				// Token: 0x0400CEC4 RID: 52932
				public static LocString DESC = string.Concat(new string[]
				{
					"Enables fabrication of ",
					UI.PRE_KEYWORD,
					"Researching Boosters",
					UI.PST_KEYWORD,
					" for Bionic Duplicants at the ",
					BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.NAME
				});
			}

			// Token: 0x02003380 RID: 13184
			public class COOKINGBOOSTER
			{
				// Token: 0x0400CEC5 RID: 52933
				public static LocString NAME = UI.PRE_KEYWORD + "Cooking Booster" + UI.PST_KEYWORD + " Pattern";

				// Token: 0x0400CEC6 RID: 52934
				public static LocString DESC = string.Concat(new string[]
				{
					"Enables fabrication of ",
					UI.PRE_KEYWORD,
					"Cooking Boosters",
					UI.PST_KEYWORD,
					" for Bionic Duplicants at the ",
					BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.NAME
				});
			}

			// Token: 0x02003381 RID: 13185
			public class MEDICINEBOOSTER
			{
				// Token: 0x0400CEC7 RID: 52935
				public static LocString NAME = UI.PRE_KEYWORD + "Doctoring Booster" + UI.PST_KEYWORD + " Pattern";

				// Token: 0x0400CEC8 RID: 52936
				public static LocString DESC = string.Concat(new string[]
				{
					"Enables fabrication of ",
					UI.PRE_KEYWORD,
					"Doctoring Boosters",
					UI.PST_KEYWORD,
					" for Bionic Duplicants at the ",
					BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.NAME
				});
			}

			// Token: 0x02003382 RID: 13186
			public class STRENGTHBOOSTER
			{
				// Token: 0x0400CEC9 RID: 52937
				public static LocString NAME = UI.PRE_KEYWORD + "Strength Booster" + UI.PST_KEYWORD + " Pattern";

				// Token: 0x0400CECA RID: 52938
				public static LocString DESC = string.Concat(new string[]
				{
					"Enables fabrication of ",
					UI.PRE_KEYWORD,
					"Strength Boosters",
					UI.PST_KEYWORD,
					" for Bionic Duplicants at the ",
					BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.NAME
				});
			}

			// Token: 0x02003383 RID: 13187
			public class CREATIVITYBOOSTER
			{
				// Token: 0x0400CECB RID: 52939
				public static LocString NAME = UI.PRE_KEYWORD + "Decorating Booster" + UI.PST_KEYWORD + " Pattern";

				// Token: 0x0400CECC RID: 52940
				public static LocString DESC = string.Concat(new string[]
				{
					"Enables fabrication of ",
					UI.PRE_KEYWORD,
					"Decorating Boosters",
					UI.PST_KEYWORD,
					" for Bionic Duplicants at the ",
					BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.NAME
				});
			}

			// Token: 0x02003384 RID: 13188
			public class AGRICULTUREBOOSTER
			{
				// Token: 0x0400CECD RID: 52941
				public static LocString NAME = UI.PRE_KEYWORD + "Farming Booster" + UI.PST_KEYWORD + " Pattern";

				// Token: 0x0400CECE RID: 52942
				public static LocString DESC = string.Concat(new string[]
				{
					"Enables fabrication of ",
					UI.PRE_KEYWORD,
					"Farming Boosters",
					UI.PST_KEYWORD,
					" for Bionic Duplicants at the ",
					BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.NAME
				});
			}

			// Token: 0x02003385 RID: 13189
			public class HUSBANDRYBOOSTER
			{
				// Token: 0x0400CECF RID: 52943
				public static LocString NAME = UI.PRE_KEYWORD + "Ranching Booster" + UI.PST_KEYWORD + " Pattern";

				// Token: 0x0400CED0 RID: 52944
				public static LocString DESC = string.Concat(new string[]
				{
					"Enables fabrication of ",
					UI.PRE_KEYWORD,
					"Ranching Boosters",
					UI.PST_KEYWORD,
					" for Bionic Duplicants at the ",
					BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.NAME
				});
			}
		}

		// Token: 0x02003386 RID: 13190
		public class TREES
		{
			// Token: 0x0400CED1 RID: 52945
			public static LocString TITLE_FOOD = "Food";

			// Token: 0x0400CED2 RID: 52946
			public static LocString TITLE_POWER = "Power";

			// Token: 0x0400CED3 RID: 52947
			public static LocString TITLE_SOLIDS = "Solid Material";

			// Token: 0x0400CED4 RID: 52948
			public static LocString TITLE_COLONYDEVELOPMENT = "Colony Development";

			// Token: 0x0400CED5 RID: 52949
			public static LocString TITLE_RADIATIONTECH = "Radiation Technologies";

			// Token: 0x0400CED6 RID: 52950
			public static LocString TITLE_MEDICINE = "Medicine";

			// Token: 0x0400CED7 RID: 52951
			public static LocString TITLE_LIQUIDS = "Liquids";

			// Token: 0x0400CED8 RID: 52952
			public static LocString TITLE_GASES = "Gases";

			// Token: 0x0400CED9 RID: 52953
			public static LocString TITLE_SUITS = "Exosuits";

			// Token: 0x0400CEDA RID: 52954
			public static LocString TITLE_DECOR = "Decor";

			// Token: 0x0400CEDB RID: 52955
			public static LocString TITLE_COMPUTERS = "Computers";

			// Token: 0x0400CEDC RID: 52956
			public static LocString TITLE_ROCKETS = "Rocketry";
		}

		// Token: 0x02003387 RID: 13191
		public class TECHS
		{
			// Token: 0x02003388 RID: 13192
			public class JOBS
			{
				// Token: 0x0400CEDD RID: 52957
				public static LocString NAME = UI.FormatAsLink("Employment", "JOBS");

				// Token: 0x0400CEDE RID: 52958
				public static LocString DESC = "Exchange the skill points earned by Duplicants for new traits and abilities.";
			}

			// Token: 0x02003389 RID: 13193
			public class IMPROVEDOXYGEN
			{
				// Token: 0x0400CEDF RID: 52959
				public static LocString NAME = UI.FormatAsLink("Air Systems", "IMPROVEDOXYGEN");

				// Token: 0x0400CEE0 RID: 52960
				public static LocString DESC = "Maintain clean, breathable air in the colony.";
			}

			// Token: 0x0200338A RID: 13194
			public class FARMINGTECH
			{
				// Token: 0x0400CEE1 RID: 52961
				public static LocString NAME = UI.FormatAsLink("Basic Farming", "FARMINGTECH");

				// Token: 0x0400CEE2 RID: 52962
				public static LocString DESC = "Learn the introductory principles of " + UI.FormatAsLink("Plant", "PLANTS") + " domestication.";
			}

			// Token: 0x0200338B RID: 13195
			public class AGRICULTURE
			{
				// Token: 0x0400CEE3 RID: 52963
				public static LocString NAME = UI.FormatAsLink("Agriculture", "AGRICULTURE");

				// Token: 0x0400CEE4 RID: 52964
				public static LocString DESC = "Master the agricultural art of crop raising.";
			}

			// Token: 0x0200338C RID: 13196
			public class RANCHING
			{
				// Token: 0x0400CEE5 RID: 52965
				public static LocString NAME = UI.FormatAsLink("Ranching", "RANCHING");

				// Token: 0x0400CEE6 RID: 52966
				public static LocString DESC = "Tame and care for wild critters.";
			}

			// Token: 0x0200338D RID: 13197
			public class ANIMALCONTROL
			{
				// Token: 0x0400CEE7 RID: 52967
				public static LocString NAME = UI.FormatAsLink("Animal Control", "ANIMALCONTROL");

				// Token: 0x0400CEE8 RID: 52968
				public static LocString DESC = "Useful techniques to manage critter populations in the colony.";
			}

			// Token: 0x0200338E RID: 13198
			public class ANIMALCOMFORT
			{
				// Token: 0x0400CEE9 RID: 52969
				public static LocString NAME = UI.FormatAsLink("Creature Comforts", "ANIMALCOMFORT");

				// Token: 0x0400CEEA RID: 52970
				public static LocString DESC = "Strategies for maximizing critters' quality of life.";
			}

			// Token: 0x0200338F RID: 13199
			public class DAIRYOPERATION
			{
				// Token: 0x0400CEEB RID: 52971
				public static LocString NAME = UI.FormatAsLink("Brackene Flow", "DAIRYOPERATION");

				// Token: 0x0400CEEC RID: 52972
				public static LocString DESC = "Advanced production, processing and distribution of this fluid resource.";
			}

			// Token: 0x02003390 RID: 13200
			public class FOODREPURPOSING
			{
				// Token: 0x0400CEED RID: 52973
				public static LocString NAME = UI.FormatAsLink("Food Repurposing", "FOODREPURPOSING");

				// Token: 0x0400CEEE RID: 52974
				public static LocString DESC = string.Concat(new string[]
				{
					"Blend that leftover ",
					UI.FormatAsLink("Food", "FOOD"),
					" into a ",
					UI.FormatAsLink("Morale", "MORALE"),
					"-boosting slurry."
				});
			}

			// Token: 0x02003391 RID: 13201
			public class FINEDINING
			{
				// Token: 0x0400CEEF RID: 52975
				public static LocString NAME = UI.FormatAsLink("Meal Preparation", "FINEDINING");

				// Token: 0x0400CEF0 RID: 52976
				public static LocString DESC = "Prepare more nutritious " + UI.FormatAsLink("Food", "FOOD") + " and store it longer before spoiling.";
			}

			// Token: 0x02003392 RID: 13202
			public class FINERDINING
			{
				// Token: 0x0400CEF1 RID: 52977
				public static LocString NAME = UI.FormatAsLink("Gourmet Meal Preparation", "FINERDINING");

				// Token: 0x0400CEF2 RID: 52978
				public static LocString DESC = "Raise colony Morale by cooking the most delicious, high-quality " + UI.FormatAsLink("Foods", "FOOD") + ".";
			}

			// Token: 0x02003393 RID: 13203
			public class GASPIPING
			{
				// Token: 0x0400CEF3 RID: 52979
				public static LocString NAME = UI.FormatAsLink("Ventilation", "GASPIPING");

				// Token: 0x0400CEF4 RID: 52980
				public static LocString DESC = "Rudimentary technologies for installing " + UI.FormatAsLink("Gas", "ELEMENTS_GAS") + " infrastructure.";
			}

			// Token: 0x02003394 RID: 13204
			public class IMPROVEDGASPIPING
			{
				// Token: 0x0400CEF5 RID: 52981
				public static LocString NAME = UI.FormatAsLink("Improved Ventilation", "IMPROVEDGASPIPING");

				// Token: 0x0400CEF6 RID: 52982
				public static LocString DESC = UI.FormatAsLink("Gas", "ELEMENTS_GAS") + " infrastructure capable of withstanding more intense conditions, such as " + UI.FormatAsLink("Heat", "Heat") + " and pressure.";
			}

			// Token: 0x02003395 RID: 13205
			public class FLOWREDIRECTION
			{
				// Token: 0x0400CEF7 RID: 52983
				public static LocString NAME = UI.FormatAsLink("Flow Redirection", "FLOWREDIRECTION");

				// Token: 0x0400CEF8 RID: 52984
				public static LocString DESC = UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " management for " + UI.FormatAsLink("Morale", "MORALE") + " and industry.";
			}

			// Token: 0x02003396 RID: 13206
			public class LIQUIDDISTRIBUTION
			{
				// Token: 0x0400CEF9 RID: 52985
				public static LocString NAME = UI.FormatAsLink("Liquid Distribution", "LIQUIDDISTRIBUTION");

				// Token: 0x0400CEFA RID: 52986
				public static LocString DESC = "Advanced fittings ensure that " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " resources get where they need to go.";
			}

			// Token: 0x02003397 RID: 13207
			public class TEMPERATUREMODULATION
			{
				// Token: 0x0400CEFB RID: 52987
				public static LocString NAME = UI.FormatAsLink("Temperature Modulation", "TEMPERATUREMODULATION");

				// Token: 0x0400CEFC RID: 52988
				public static LocString DESC = "Precise " + UI.FormatAsLink("Temperature", "HEAT") + " altering technologies to keep my colony at the perfect Kelvin.";
			}

			// Token: 0x02003398 RID: 13208
			public class HVAC
			{
				// Token: 0x0400CEFD RID: 52989
				public static LocString NAME = UI.FormatAsLink("HVAC", "HVAC");

				// Token: 0x0400CEFE RID: 52990
				public static LocString DESC = string.Concat(new string[]
				{
					"Regulate ",
					UI.FormatAsLink("Temperature", "HEAT"),
					" in the colony for ",
					UI.FormatAsLink("Plant", "PLANTS"),
					" cultivation and Duplicant comfort."
				});
			}

			// Token: 0x02003399 RID: 13209
			public class GASDISTRIBUTION
			{
				// Token: 0x0400CEFF RID: 52991
				public static LocString NAME = UI.FormatAsLink("Gas Distribution", "GASDISTRIBUTION");

				// Token: 0x0400CF00 RID: 52992
				public static LocString DESC = "Design building hookups to get " + UI.FormatAsLink("Gas", "ELEMENTS_GAS") + " resources circulating properly.";
			}

			// Token: 0x0200339A RID: 13210
			public class LIQUIDTEMPERATURE
			{
				// Token: 0x0400CF01 RID: 52993
				public static LocString NAME = UI.FormatAsLink("Liquid Tuning", "LIQUIDTEMPERATURE");

				// Token: 0x0400CF02 RID: 52994
				public static LocString DESC = string.Concat(new string[]
				{
					"Easily manipulate ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" ",
					UI.FormatAsLink("Heat", "Temperatures"),
					" with these temperature regulating technologies."
				});
			}

			// Token: 0x0200339B RID: 13211
			public class INSULATION
			{
				// Token: 0x0400CF03 RID: 52995
				public static LocString NAME = UI.FormatAsLink("Insulation", "INSULATION");

				// Token: 0x0400CF04 RID: 52996
				public static LocString DESC = "Improve " + UI.FormatAsLink("Heat", "Heat") + " distribution within the colony and guard buildings from extreme temperatures.";
			}

			// Token: 0x0200339C RID: 13212
			public class PRESSUREMANAGEMENT
			{
				// Token: 0x0400CF05 RID: 52997
				public static LocString NAME = UI.FormatAsLink("Pressure Management", "PRESSUREMANAGEMENT");

				// Token: 0x0400CF06 RID: 52998
				public static LocString DESC = "Unlock technologies to manage colony pressure and atmosphere.";
			}

			// Token: 0x0200339D RID: 13213
			public class PORTABLEGASSES
			{
				// Token: 0x0400CF07 RID: 52999
				public static LocString NAME = UI.FormatAsLink("Portable Gases", "PORTABLEGASSES");

				// Token: 0x0400CF08 RID: 53000
				public static LocString DESC = "Unlock technologies to easily move gases around your colony.";
			}

			// Token: 0x0200339E RID: 13214
			public class DIRECTEDAIRSTREAMS
			{
				// Token: 0x0400CF09 RID: 53001
				public static LocString NAME = UI.FormatAsLink("Decontamination", "DIRECTEDAIRSTREAMS");

				// Token: 0x0400CF0A RID: 53002
				public static LocString DESC = "Instruments to help reduce " + UI.FormatAsLink("Germ", "DISEASE") + " spread within the base.";
			}

			// Token: 0x0200339F RID: 13215
			public class LIQUIDFILTERING
			{
				// Token: 0x0400CF0B RID: 53003
				public static LocString NAME = UI.FormatAsLink("Liquid-Based Refinement Processes", "LIQUIDFILTERING");

				// Token: 0x0400CF0C RID: 53004
				public static LocString DESC = "Use pumped " + UI.FormatAsLink("Liquids", "ELEMENTS_LIQUID") + " to filter out unwanted elements.";
			}

			// Token: 0x020033A0 RID: 13216
			public class LIQUIDPIPING
			{
				// Token: 0x0400CF0D RID: 53005
				public static LocString NAME = UI.FormatAsLink("Plumbing", "LIQUIDPIPING");

				// Token: 0x0400CF0E RID: 53006
				public static LocString DESC = "Rudimentary technologies for installing " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " infrastructure.";
			}

			// Token: 0x020033A1 RID: 13217
			public class IMPROVEDLIQUIDPIPING
			{
				// Token: 0x0400CF0F RID: 53007
				public static LocString NAME = UI.FormatAsLink("Improved Plumbing", "IMPROVEDLIQUIDPIPING");

				// Token: 0x0400CF10 RID: 53008
				public static LocString DESC = UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " infrastructure capable of withstanding more intense conditions, such as " + UI.FormatAsLink("Heat", "Heat") + " and pressure.";
			}

			// Token: 0x020033A2 RID: 13218
			public class PRECISIONPLUMBING
			{
				// Token: 0x0400CF11 RID: 53009
				public static LocString NAME = UI.FormatAsLink("Advanced Caffeination", "PRECISIONPLUMBING");

				// Token: 0x0400CF12 RID: 53010
				public static LocString DESC = "Let Duplicants relax after a long day of subterranean digging with a shot of warm beanjuice.";
			}

			// Token: 0x020033A3 RID: 13219
			public class SANITATIONSCIENCES
			{
				// Token: 0x0400CF13 RID: 53011
				public static LocString NAME = UI.FormatAsLink("Sanitation", "SANITATIONSCIENCES");

				// Token: 0x0400CF14 RID: 53012
				public static LocString DESC = "Make daily ablutions less of a hassle.";
			}

			// Token: 0x020033A4 RID: 13220
			public class ADVANCEDSANITATION
			{
				// Token: 0x0400CF15 RID: 53013
				public static LocString NAME = UI.FormatAsLink("Advanced Sanitation", "ADVANCEDSANITATION");

				// Token: 0x0400CF16 RID: 53014
				public static LocString DESC = "Clean up dirty Duplicants.";
			}

			// Token: 0x020033A5 RID: 13221
			public class MEDICINEI
			{
				// Token: 0x0400CF17 RID: 53015
				public static LocString NAME = UI.FormatAsLink("Pharmacology", "MEDICINEI");

				// Token: 0x0400CF18 RID: 53016
				public static LocString DESC = "Compound natural cures to fight the most common " + UI.FormatAsLink("Sicknesses", "SICKNESSES") + " that plague Duplicants.";
			}

			// Token: 0x020033A6 RID: 13222
			public class MEDICINEII
			{
				// Token: 0x0400CF19 RID: 53017
				public static LocString NAME = UI.FormatAsLink("Medical Equipment", "MEDICINEII");

				// Token: 0x0400CF1A RID: 53018
				public static LocString DESC = "The basic necessities doctors need to facilitate patient care.";
			}

			// Token: 0x020033A7 RID: 13223
			public class MEDICINEIII
			{
				// Token: 0x0400CF1B RID: 53019
				public static LocString NAME = UI.FormatAsLink("Pathogen Diagnostics", "MEDICINEIII");

				// Token: 0x0400CF1C RID: 53020
				public static LocString DESC = "Stop Germs at the source using special medical automation technology.";
			}

			// Token: 0x020033A8 RID: 13224
			public class MEDICINEIV
			{
				// Token: 0x0400CF1D RID: 53021
				public static LocString NAME = UI.FormatAsLink("Micro-Targeted Medicine", "MEDICINEIV");

				// Token: 0x0400CF1E RID: 53022
				public static LocString DESC = "State of the art equipment to conquer the most stubborn of illnesses.";
			}

			// Token: 0x020033A9 RID: 13225
			public class ADVANCEDFILTRATION
			{
				// Token: 0x0400CF1F RID: 53023
				public static LocString NAME = UI.FormatAsLink("Filtration", "ADVANCEDFILTRATION");

				// Token: 0x0400CF20 RID: 53024
				public static LocString DESC = string.Concat(new string[]
				{
					"Basic technologies for filtering ",
					UI.FormatAsLink("Liquids", "ELEMENTS_LIQUID"),
					" and ",
					UI.FormatAsLink("Gases", "ELEMENTS_GAS"),
					"."
				});
			}

			// Token: 0x020033AA RID: 13226
			public class POWERREGULATION
			{
				// Token: 0x0400CF21 RID: 53025
				public static LocString NAME = UI.FormatAsLink("Power Regulation", "POWERREGULATION");

				// Token: 0x0400CF22 RID: 53026
				public static LocString DESC = "Prevent wasted " + UI.FormatAsLink("Power", "POWER") + " with improved electrical tools.";
			}

			// Token: 0x020033AB RID: 13227
			public class COMBUSTION
			{
				// Token: 0x0400CF23 RID: 53027
				public static LocString NAME = UI.FormatAsLink("Internal Combustion", "COMBUSTION");

				// Token: 0x0400CF24 RID: 53028
				public static LocString DESC = "Fuel-powered generators for crude yet powerful " + UI.FormatAsLink("Power", "POWER") + " production.";
			}

			// Token: 0x020033AC RID: 13228
			public class IMPROVEDCOMBUSTION
			{
				// Token: 0x0400CF25 RID: 53029
				public static LocString NAME = UI.FormatAsLink("Fossil Fuels", "IMPROVEDCOMBUSTION");

				// Token: 0x0400CF26 RID: 53030
				public static LocString DESC = "Burn dirty fuels for exceptional " + UI.FormatAsLink("Power", "POWER") + " production.";
			}

			// Token: 0x020033AD RID: 13229
			public class INTERIORDECOR
			{
				// Token: 0x0400CF27 RID: 53031
				public static LocString NAME = UI.FormatAsLink("Interior Decor", "INTERIORDECOR");

				// Token: 0x0400CF28 RID: 53032
				public static LocString DESC = UI.FormatAsLink("Decor", "DECOR") + " boosting items to counteract the gloom of underground living.";
			}

			// Token: 0x020033AE RID: 13230
			public class ARTISTRY
			{
				// Token: 0x0400CF29 RID: 53033
				public static LocString NAME = UI.FormatAsLink("Artistic Expression", "ARTISTRY");

				// Token: 0x0400CF2A RID: 53034
				public static LocString DESC = "Majorly improve " + UI.FormatAsLink("Decor", "DECOR") + " by giving Duplicants the tools of artistic and emotional expression.";
			}

			// Token: 0x020033AF RID: 13231
			public class CLOTHING
			{
				// Token: 0x0400CF2B RID: 53035
				public static LocString NAME = UI.FormatAsLink("Textile Production", "CLOTHING");

				// Token: 0x0400CF2C RID: 53036
				public static LocString DESC = "Bring Duplicants the " + UI.FormatAsLink("Morale", "MORALE") + " boosting benefits of soft, cushy fabrics.";
			}

			// Token: 0x020033B0 RID: 13232
			public class ACOUSTICS
			{
				// Token: 0x0400CF2D RID: 53037
				public static LocString NAME = UI.FormatAsLink("Sound Amplifiers", "ACOUSTICS");

				// Token: 0x0400CF2E RID: 53038
				public static LocString DESC = "Precise control of the audio spectrum allows Duplicants to get funky.";
			}

			// Token: 0x020033B1 RID: 13233
			public class SPACEPOWER
			{
				// Token: 0x0400CF2F RID: 53039
				public static LocString NAME = UI.FormatAsLink("Space Power", "SPACEPOWER");

				// Token: 0x0400CF30 RID: 53040
				public static LocString DESC = "It's like power... in space!";
			}

			// Token: 0x020033B2 RID: 13234
			public class AMPLIFIERS
			{
				// Token: 0x0400CF31 RID: 53041
				public static LocString NAME = UI.FormatAsLink("Power Amplifiers", "AMPLIFIERS");

				// Token: 0x0400CF32 RID: 53042
				public static LocString DESC = "Further increased efficacy of " + UI.FormatAsLink("Power", "POWER") + " management to prevent those wasted joules.";
			}

			// Token: 0x020033B3 RID: 13235
			public class LUXURY
			{
				// Token: 0x0400CF33 RID: 53043
				public static LocString NAME = UI.FormatAsLink("Home Luxuries", "LUXURY");

				// Token: 0x0400CF34 RID: 53044
				public static LocString DESC = "Luxury amenities for advanced " + UI.FormatAsLink("Stress", "STRESS") + " reduction.";
			}

			// Token: 0x020033B4 RID: 13236
			public class ENVIRONMENTALAPPRECIATION
			{
				// Token: 0x0400CF35 RID: 53045
				public static LocString NAME = UI.FormatAsLink("Environmental Appreciation", "ENVIRONMENTALAPPRECIATION");

				// Token: 0x0400CF36 RID: 53046
				public static LocString DESC = string.Concat(new string[]
				{
					"Improve ",
					UI.FormatAsLink("Morale", "MORALE"),
					" by lazing around in ",
					UI.FormatAsLink("Light", "LIGHT"),
					" with a high Lux value."
				});
			}

			// Token: 0x020033B5 RID: 13237
			public class FINEART
			{
				// Token: 0x0400CF37 RID: 53047
				public static LocString NAME = UI.FormatAsLink("Fine Art", "FINEART");

				// Token: 0x0400CF38 RID: 53048
				public static LocString DESC = "Broader options for artistic " + UI.FormatAsLink("Decor", "DECOR") + " improvements.";
			}

			// Token: 0x020033B6 RID: 13238
			public class REFRACTIVEDECOR
			{
				// Token: 0x0400CF39 RID: 53049
				public static LocString NAME = UI.FormatAsLink("High Culture", "REFRACTIVEDECOR");

				// Token: 0x0400CF3A RID: 53050
				public static LocString DESC = "New methods for working with extremely high quality art materials.";
			}

			// Token: 0x020033B7 RID: 13239
			public class RENAISSANCEART
			{
				// Token: 0x0400CF3B RID: 53051
				public static LocString NAME = UI.FormatAsLink("Renaissance Art", "RENAISSANCEART");

				// Token: 0x0400CF3C RID: 53052
				public static LocString DESC = "The kind of art that culture legacies are made of.";
			}

			// Token: 0x020033B8 RID: 13240
			public class GLASSFURNISHINGS
			{
				// Token: 0x0400CF3D RID: 53053
				public static LocString NAME = UI.FormatAsLink("Glass Blowing", "GLASSFURNISHINGS");

				// Token: 0x0400CF3E RID: 53054
				public static LocString DESC = "The decorative benefits of glass are both apparent and transparent.";
			}

			// Token: 0x020033B9 RID: 13241
			public class SCREENS
			{
				// Token: 0x0400CF3F RID: 53055
				public static LocString NAME = UI.FormatAsLink("New Media", "SCREENS");

				// Token: 0x0400CF40 RID: 53056
				public static LocString DESC = "High tech displays with lots of pretty colors.";
			}

			// Token: 0x020033BA RID: 13242
			public class ADVANCEDPOWERREGULATION
			{
				// Token: 0x0400CF41 RID: 53057
				public static LocString NAME = UI.FormatAsLink("Advanced Power Regulation", "ADVANCEDPOWERREGULATION");

				// Token: 0x0400CF42 RID: 53058
				public static LocString DESC = "Circuit components required for large scale " + UI.FormatAsLink("Power", "POWER") + " management.";
			}

			// Token: 0x020033BB RID: 13243
			public class PLASTICS
			{
				// Token: 0x0400CF43 RID: 53059
				public static LocString NAME = UI.FormatAsLink("Plastic Manufacturing", "PLASTICS");

				// Token: 0x0400CF44 RID: 53060
				public static LocString DESC = "Stable, lightweight, durable. Plastics are useful for a wide array of applications.";
			}

			// Token: 0x020033BC RID: 13244
			public class SUITS
			{
				// Token: 0x0400CF45 RID: 53061
				public static LocString NAME = UI.FormatAsLink("Hazard Protection", "SUITS");

				// Token: 0x0400CF46 RID: 53062
				public static LocString DESC = "Vital gear for surviving in extreme conditions and environments.";
			}

			// Token: 0x020033BD RID: 13245
			public class DISTILLATION
			{
				// Token: 0x0400CF47 RID: 53063
				public static LocString NAME = UI.FormatAsLink("Distillation", "DISTILLATION");

				// Token: 0x0400CF48 RID: 53064
				public static LocString DESC = "Distill difficult mixtures down to their most useful parts.";
			}

			// Token: 0x020033BE RID: 13246
			public class CATALYTICS
			{
				// Token: 0x0400CF49 RID: 53065
				public static LocString NAME = UI.FormatAsLink("Catalytics", "CATALYTICS");

				// Token: 0x0400CF4A RID: 53066
				public static LocString DESC = "Advanced gas manipulation using unique catalysts.";
			}

			// Token: 0x020033BF RID: 13247
			public class ADVANCEDRESEARCH
			{
				// Token: 0x0400CF4B RID: 53067
				public static LocString NAME = UI.FormatAsLink("Advanced Research", "ADVANCEDRESEARCH");

				// Token: 0x0400CF4C RID: 53068
				public static LocString DESC = "The tools my colony needs to conduct more advanced, in-depth research.";
			}

			// Token: 0x020033C0 RID: 13248
			public class SPACEPROGRAM
			{
				// Token: 0x0400CF4D RID: 53069
				public static LocString NAME = UI.FormatAsLink("Space Program", "SPACEPROGRAM");

				// Token: 0x0400CF4E RID: 53070
				public static LocString DESC = "The first steps in getting a Duplicant to space.";
			}

			// Token: 0x020033C1 RID: 13249
			public class CRASHPLAN
			{
				// Token: 0x0400CF4F RID: 53071
				public static LocString NAME = UI.FormatAsLink("Crash Plan", "CRASHPLAN");

				// Token: 0x0400CF50 RID: 53072
				public static LocString DESC = "What goes up, must come down.";
			}

			// Token: 0x020033C2 RID: 13250
			public class DURABLELIFESUPPORT
			{
				// Token: 0x0400CF51 RID: 53073
				public static LocString NAME = UI.FormatAsLink("Durable Life Support", "DURABLELIFESUPPORT");

				// Token: 0x0400CF52 RID: 53074
				public static LocString DESC = "Improved devices for extended missions into space.";
			}

			// Token: 0x020033C3 RID: 13251
			public class ARTIFICIALFRIENDS
			{
				// Token: 0x0400CF53 RID: 53075
				public static LocString NAME = UI.FormatAsLink("Artificial Friends", "ARTIFICIALFRIENDS");

				// Token: 0x0400CF54 RID: 53076
				public static LocString DESC = "Sweeping advances in companion technology.";
			}

			// Token: 0x020033C4 RID: 13252
			public class ROBOTICTOOLS
			{
				// Token: 0x0400CF55 RID: 53077
				public static LocString NAME = UI.FormatAsLink("Robotic Tools", "ROBOTICTOOLS");

				// Token: 0x0400CF56 RID: 53078
				public static LocString DESC = "The goal of every great civilization is to one day make itself obsolete.";
			}

			// Token: 0x020033C5 RID: 13253
			public class LOGICCONTROL
			{
				// Token: 0x0400CF57 RID: 53079
				public static LocString NAME = UI.FormatAsLink("Smart Home", "LOGICCONTROL");

				// Token: 0x0400CF58 RID: 53080
				public static LocString DESC = "Switches that grant full control of building operations within the colony.";
			}

			// Token: 0x020033C6 RID: 13254
			public class LOGICCIRCUITS
			{
				// Token: 0x0400CF59 RID: 53081
				public static LocString NAME = UI.FormatAsLink("Advanced Automation", "LOGICCIRCUITS");

				// Token: 0x0400CF5A RID: 53082
				public static LocString DESC = "The only limit to colony automation is my own imagination.";
			}

			// Token: 0x020033C7 RID: 13255
			public class PARALLELAUTOMATION
			{
				// Token: 0x0400CF5B RID: 53083
				public static LocString NAME = UI.FormatAsLink("Parallel Automation", "PARALLELAUTOMATION");

				// Token: 0x0400CF5C RID: 53084
				public static LocString DESC = "Multi-wire automation at a fraction of the space.";
			}

			// Token: 0x020033C8 RID: 13256
			public class MULTIPLEXING
			{
				// Token: 0x0400CF5D RID: 53085
				public static LocString NAME = UI.FormatAsLink("Multiplexing", "MULTIPLEXING");

				// Token: 0x0400CF5E RID: 53086
				public static LocString DESC = "More choices for Automation signal distribution.";
			}

			// Token: 0x020033C9 RID: 13257
			public class VALVEMINIATURIZATION
			{
				// Token: 0x0400CF5F RID: 53087
				public static LocString NAME = UI.FormatAsLink("Valve Miniaturization", "VALVEMINIATURIZATION");

				// Token: 0x0400CF60 RID: 53088
				public static LocString DESC = "Smaller, more efficient pumps for those low-throughput situations.";
			}

			// Token: 0x020033CA RID: 13258
			public class HYDROCARBONPROPULSION
			{
				// Token: 0x0400CF61 RID: 53089
				public static LocString NAME = UI.FormatAsLink("Hydrocarbon Propulsion", "HYDROCARBONPROPULSION");

				// Token: 0x0400CF62 RID: 53090
				public static LocString DESC = "Low-range rocket engines with lots of smoke.";
			}

			// Token: 0x020033CB RID: 13259
			public class BETTERHYDROCARBONPROPULSION
			{
				// Token: 0x0400CF63 RID: 53091
				public static LocString NAME = UI.FormatAsLink("Improved Hydrocarbon Propulsion", "BETTERHYDROCARBONPROPULSION");

				// Token: 0x0400CF64 RID: 53092
				public static LocString DESC = "Mid-range rocket engines with lots of smoke.";
			}

			// Token: 0x020033CC RID: 13260
			public class PRETTYGOODCONDUCTORS
			{
				// Token: 0x0400CF65 RID: 53093
				public static LocString NAME = UI.FormatAsLink("Low-Resistance Conductors", "PRETTYGOODCONDUCTORS");

				// Token: 0x0400CF66 RID: 53094
				public static LocString DESC = "Pure-core wires that can handle more " + UI.FormatAsLink("Electrical", "POWER") + " current without overloading.";
			}

			// Token: 0x020033CD RID: 13261
			public class RENEWABLEENERGY
			{
				// Token: 0x0400CF67 RID: 53095
				public static LocString NAME = UI.FormatAsLink("Renewable Energy", "RENEWABLEENERGY");

				// Token: 0x0400CF68 RID: 53096
				public static LocString DESC = "Clean, sustainable " + UI.FormatAsLink("Power", "POWER") + " production that produces little to no waste.";
			}

			// Token: 0x020033CE RID: 13262
			public class BASICREFINEMENT
			{
				// Token: 0x0400CF69 RID: 53097
				public static LocString NAME = UI.FormatAsLink("Brute-Force Refinement", "BASICREFINEMENT");

				// Token: 0x0400CF6A RID: 53098
				public static LocString DESC = "Low-tech refinement methods for producing clay and renewable sources of sand.";
			}

			// Token: 0x020033CF RID: 13263
			public class REFINEDOBJECTS
			{
				// Token: 0x0400CF6B RID: 53099
				public static LocString NAME = UI.FormatAsLink("Refined Renovations", "REFINEDOBJECTS");

				// Token: 0x0400CF6C RID: 53100
				public static LocString DESC = "Improve base infrastructure with new objects crafted from " + UI.FormatAsLink("Refined Metals", "REFINEDMETAL") + ".";
			}

			// Token: 0x020033D0 RID: 13264
			public class GENERICSENSORS
			{
				// Token: 0x0400CF6D RID: 53101
				public static LocString NAME = UI.FormatAsLink("Generic Sensors", "GENERICSENSORS");

				// Token: 0x0400CF6E RID: 53102
				public static LocString DESC = "Drive automation in a variety of new, inventive ways.";
			}

			// Token: 0x020033D1 RID: 13265
			public class DUPETRAFFICCONTROL
			{
				// Token: 0x0400CF6F RID: 53103
				public static LocString NAME = UI.FormatAsLink("Computing", "DUPETRAFFICCONTROL");

				// Token: 0x0400CF70 RID: 53104
				public static LocString DESC = "Virtually extend the boundaries of Duplicant imagination.";
			}

			// Token: 0x020033D2 RID: 13266
			public class ADVANCEDSCANNERS
			{
				// Token: 0x0400CF71 RID: 53105
				public static LocString NAME = UI.FormatAsLink("Sensitive Microimaging", "ADVANCEDSCANNERS");

				// Token: 0x0400CF72 RID: 53106
				public static LocString DESC = "Computerized systems do the looking, so Duplicants don't have to.";
			}

			// Token: 0x020033D3 RID: 13267
			public class SMELTING
			{
				// Token: 0x0400CF73 RID: 53107
				public static LocString NAME = UI.FormatAsLink("Smelting", "SMELTING");

				// Token: 0x0400CF74 RID: 53108
				public static LocString DESC = "High temperatures facilitate the production of purer, special use metal resources.";
			}

			// Token: 0x020033D4 RID: 13268
			public class TRAVELTUBES
			{
				// Token: 0x0400CF75 RID: 53109
				public static LocString NAME = UI.FormatAsLink("Transit Tubes", "TRAVELTUBES");

				// Token: 0x0400CF76 RID: 53110
				public static LocString DESC = "A wholly futuristic way to move Duplicants around the base.";
			}

			// Token: 0x020033D5 RID: 13269
			public class SMARTSTORAGE
			{
				// Token: 0x0400CF77 RID: 53111
				public static LocString NAME = UI.FormatAsLink("Smart Storage", "SMARTSTORAGE");

				// Token: 0x0400CF78 RID: 53112
				public static LocString DESC = "Completely automate the storage of solid resources.";
			}

			// Token: 0x020033D6 RID: 13270
			public class SOLIDTRANSPORT
			{
				// Token: 0x0400CF79 RID: 53113
				public static LocString NAME = UI.FormatAsLink("Solid Transport", "SOLIDTRANSPORT");

				// Token: 0x0400CF7A RID: 53114
				public static LocString DESC = "Free Duplicants from the drudgery of day-to-day material deliveries with new methods of automation.";
			}

			// Token: 0x020033D7 RID: 13271
			public class SOLIDMANAGEMENT
			{
				// Token: 0x0400CF7B RID: 53115
				public static LocString NAME = UI.FormatAsLink("Solid Management", "SOLIDMANAGEMENT");

				// Token: 0x0400CF7C RID: 53116
				public static LocString DESC = "Make solid decisions in " + UI.FormatAsLink("Solid", "ELEMENTS_SOLID") + " sorting.";
			}

			// Token: 0x020033D8 RID: 13272
			public class SOLIDDISTRIBUTION
			{
				// Token: 0x0400CF7D RID: 53117
				public static LocString NAME = UI.FormatAsLink("Solid Distribution", "SOLIDDISTRIBUTION");

				// Token: 0x0400CF7E RID: 53118
				public static LocString DESC = "Internal rocket hookups for " + UI.FormatAsLink("Solid", "ELEMENTS_SOLID") + " resources.";
			}

			// Token: 0x020033D9 RID: 13273
			public class HIGHTEMPFORGING
			{
				// Token: 0x0400CF7F RID: 53119
				public static LocString NAME = UI.FormatAsLink("Superheated Forging", "HIGHTEMPFORGING");

				// Token: 0x0400CF80 RID: 53120
				public static LocString DESC = "Craft entirely new materials by harnessing the most extreme temperatures.";
			}

			// Token: 0x020033DA RID: 13274
			public class HIGHPRESSUREFORGING
			{
				// Token: 0x0400CF81 RID: 53121
				public static LocString NAME = UI.FormatAsLink("Pressurized Forging", "HIGHPRESSUREFORGING");

				// Token: 0x0400CF82 RID: 53122
				public static LocString DESC = "High pressure diamond forging.";
			}

			// Token: 0x020033DB RID: 13275
			public class RADIATIONPROTECTION
			{
				// Token: 0x0400CF83 RID: 53123
				public static LocString NAME = UI.FormatAsLink("Radiation Protection", "RADIATIONPROTECTION");

				// Token: 0x0400CF84 RID: 53124
				public static LocString DESC = "Shield Duplicants from dangerous amounts of radiation.";
			}

			// Token: 0x020033DC RID: 13276
			public class SKYDETECTORS
			{
				// Token: 0x0400CF85 RID: 53125
				public static LocString NAME = UI.FormatAsLink("Celestial Detection", "SKYDETECTORS");

				// Token: 0x0400CF86 RID: 53126
				public static LocString DESC = "Turn Duplicants' eyes to the skies and discover what undiscovered wonders await out there.";
			}

			// Token: 0x020033DD RID: 13277
			public class JETPACKS
			{
				// Token: 0x0400CF87 RID: 53127
				public static LocString NAME = UI.FormatAsLink("Jetpacks", "JETPACKS");

				// Token: 0x0400CF88 RID: 53128
				public static LocString DESC = "Objectively the most stylish way for Duplicants to get around.";
			}

			// Token: 0x020033DE RID: 13278
			public class BASICROCKETRY
			{
				// Token: 0x0400CF89 RID: 53129
				public static LocString NAME = UI.FormatAsLink("Introductory Rocketry", "BASICROCKETRY");

				// Token: 0x0400CF8A RID: 53130
				public static LocString DESC = "Everything required for launching the colony's very first space program.";
			}

			// Token: 0x020033DF RID: 13279
			public class ENGINESI
			{
				// Token: 0x0400CF8B RID: 53131
				public static LocString NAME = UI.FormatAsLink("Solid Fuel Combustion", "ENGINESI");

				// Token: 0x0400CF8C RID: 53132
				public static LocString DESC = "Rockets that fly further, longer.";
			}

			// Token: 0x020033E0 RID: 13280
			public class ENGINESII
			{
				// Token: 0x0400CF8D RID: 53133
				public static LocString NAME = UI.FormatAsLink("Hydrocarbon Combustion", "ENGINESII");

				// Token: 0x0400CF8E RID: 53134
				public static LocString DESC = "Delve deeper into the vastness of space than ever before.";
			}

			// Token: 0x020033E1 RID: 13281
			public class ENGINESIII
			{
				// Token: 0x0400CF8F RID: 53135
				public static LocString NAME = UI.FormatAsLink("Cryofuel Combustion", "ENGINESIII");

				// Token: 0x0400CF90 RID: 53136
				public static LocString DESC = "With this technology, the sky is your oyster. Go exploring!";
			}

			// Token: 0x020033E2 RID: 13282
			public class CRYOFUELPROPULSION
			{
				// Token: 0x0400CF91 RID: 53137
				public static LocString NAME = UI.FormatAsLink("Cryofuel Propulsion", "CRYOFUELPROPULSION");

				// Token: 0x0400CF92 RID: 53138
				public static LocString DESC = "A semi-powerful engine to propel you further into the galaxy.";
			}

			// Token: 0x020033E3 RID: 13283
			public class NUCLEARPROPULSION
			{
				// Token: 0x0400CF93 RID: 53139
				public static LocString NAME = UI.FormatAsLink("Radbolt Propulsion", "NUCLEARPROPULSION");

				// Token: 0x0400CF94 RID: 53140
				public static LocString DESC = "Radical technology to get you to the stars.";
			}

			// Token: 0x020033E4 RID: 13284
			public class ADVANCEDRESOURCEEXTRACTION
			{
				// Token: 0x0400CF95 RID: 53141
				public static LocString NAME = UI.FormatAsLink("Advanced Resource Extraction", "ADVANCEDRESOURCEEXTRACTION");

				// Token: 0x0400CF96 RID: 53142
				public static LocString DESC = "Bring back souvieners from the stars.";
			}

			// Token: 0x020033E5 RID: 13285
			public class CARGOI
			{
				// Token: 0x0400CF97 RID: 53143
				public static LocString NAME = UI.FormatAsLink("Solid Cargo", "CARGOI");

				// Token: 0x0400CF98 RID: 53144
				public static LocString DESC = "Make extra use of journeys into space by mining and storing useful resources.";
			}

			// Token: 0x020033E6 RID: 13286
			public class CARGOII
			{
				// Token: 0x0400CF99 RID: 53145
				public static LocString NAME = UI.FormatAsLink("Liquid and Gas Cargo", "CARGOII");

				// Token: 0x0400CF9A RID: 53146
				public static LocString DESC = "Extract precious liquids and gases from the far reaches of space, and return with them to the colony.";
			}

			// Token: 0x020033E7 RID: 13287
			public class CARGOIII
			{
				// Token: 0x0400CF9B RID: 53147
				public static LocString NAME = UI.FormatAsLink("Unique Cargo", "CARGOIII");

				// Token: 0x0400CF9C RID: 53148
				public static LocString DESC = "Allow Duplicants to take their friends to see the stars... or simply bring souvenirs back from their travels.";
			}

			// Token: 0x020033E8 RID: 13288
			public class NOTIFICATIONSYSTEMS
			{
				// Token: 0x0400CF9D RID: 53149
				public static LocString NAME = UI.FormatAsLink("Notification Systems", "NOTIFICATIONSYSTEMS");

				// Token: 0x0400CF9E RID: 53150
				public static LocString DESC = "Get all the news you need to know about your complex colony.";
			}

			// Token: 0x020033E9 RID: 13289
			public class NUCLEARREFINEMENT
			{
				// Token: 0x0400CF9F RID: 53151
				public static LocString NAME = UI.FormatAsLink("Radiation Refinement", "NUCLEAR");

				// Token: 0x0400CFA0 RID: 53152
				public static LocString DESC = "Refine uranium and generate radiation.";
			}

			// Token: 0x020033EA RID: 13290
			public class NUCLEARRESEARCH
			{
				// Token: 0x0400CFA1 RID: 53153
				public static LocString NAME = UI.FormatAsLink("Materials Science Research", "NUCLEARRESEARCH");

				// Token: 0x0400CFA2 RID: 53154
				public static LocString DESC = "Harness sub-atomic particles to study the properties of matter.";
			}

			// Token: 0x020033EB RID: 13291
			public class ADVANCEDNUCLEARRESEARCH
			{
				// Token: 0x0400CFA3 RID: 53155
				public static LocString NAME = UI.FormatAsLink("More Materials Science Research", "ADVANCEDNUCLEARRESEARCH");

				// Token: 0x0400CFA4 RID: 53156
				public static LocString DESC = "Harness sub-atomic particles to study the properties of matter even more.";
			}

			// Token: 0x020033EC RID: 13292
			public class NUCLEARSTORAGE
			{
				// Token: 0x0400CFA5 RID: 53157
				public static LocString NAME = UI.FormatAsLink("Radbolt Containment", "NUCLEARSTORAGE");

				// Token: 0x0400CFA6 RID: 53158
				public static LocString DESC = "Build a quality cache of radbolts.";
			}

			// Token: 0x020033ED RID: 13293
			public class SOLIDSPACE
			{
				// Token: 0x0400CFA7 RID: 53159
				public static LocString NAME = UI.FormatAsLink("Solid Control", "SOLIDSPACE");

				// Token: 0x0400CFA8 RID: 53160
				public static LocString DESC = "Transport and sort " + UI.FormatAsLink("Solid", "ELEMENTS_SOLID") + " resources.";
			}

			// Token: 0x020033EE RID: 13294
			public class HIGHVELOCITYTRANSPORT
			{
				// Token: 0x0400CFA9 RID: 53161
				public static LocString NAME = UI.FormatAsLink("High Velocity Transport", "HIGHVELOCITY");

				// Token: 0x0400CFAA RID: 53162
				public static LocString DESC = "Hurl things through space.";
			}

			// Token: 0x020033EF RID: 13295
			public class MONUMENTS
			{
				// Token: 0x0400CFAB RID: 53163
				public static LocString NAME = UI.FormatAsLink("Monuments", "MONUMENTS");

				// Token: 0x0400CFAC RID: 53164
				public static LocString DESC = "Monumental art projects.";
			}

			// Token: 0x020033F0 RID: 13296
			public class BIOENGINEERING
			{
				// Token: 0x0400CFAD RID: 53165
				public static LocString NAME = UI.FormatAsLink("Bioengineering", "BIOENGINEERING");

				// Token: 0x0400CFAE RID: 53166
				public static LocString DESC = "Mutation station.";
			}

			// Token: 0x020033F1 RID: 13297
			public class SPACECOMBUSTION
			{
				// Token: 0x0400CFAF RID: 53167
				public static LocString NAME = UI.FormatAsLink("Advanced Combustion", "SPACECOMBUSTION");

				// Token: 0x0400CFB0 RID: 53168
				public static LocString DESC = "Sweet advancements in rocket engines.";
			}

			// Token: 0x020033F2 RID: 13298
			public class HIGHVELOCITYDESTRUCTION
			{
				// Token: 0x0400CFB1 RID: 53169
				public static LocString NAME = UI.FormatAsLink("High Velocity Destruction", "HIGHVELOCITYDESTRUCTION");

				// Token: 0x0400CFB2 RID: 53170
				public static LocString DESC = "Mine the skies.";
			}

			// Token: 0x020033F3 RID: 13299
			public class SPACEGAS
			{
				// Token: 0x0400CFB3 RID: 53171
				public static LocString NAME = UI.FormatAsLink("Advanced Gas Flow", "SPACEGAS");

				// Token: 0x0400CFB4 RID: 53172
				public static LocString DESC = UI.FormatAsLink("Gas", "ELEMENTS_GASSES") + " engines and transportation for rockets.";
			}

			// Token: 0x020033F4 RID: 13300
			public class DATASCIENCE
			{
				// Token: 0x0400CFB5 RID: 53173
				public static LocString NAME = UI.FormatAsLink("Data Science", "DATASCIENCE");

				// Token: 0x0400CFB6 RID: 53174
				public static LocString DESC = "The science of making the data work for my Duplicants, instead of the other way around.";
			}

			// Token: 0x020033F5 RID: 13301
			public class DATASCIENCEBASEGAME
			{
				// Token: 0x0400CFB7 RID: 53175
				public static LocString NAME = UI.FormatAsLink("Data Science", "DATASCIENCEBASEGAME");

				// Token: 0x0400CFB8 RID: 53176
				public static LocString DESC = "The science of making the data work for my Duplicants, instead of the other way around.";
			}
		}
	}
}
