using System;

namespace STRINGS
{
	// Token: 0x020038AC RID: 14508
	public class ELEMENTS
	{
		// Token: 0x0400DB9F RID: 56223
		public static LocString ELEMENTDESCSOLID = "Resource Type: {0}\nMelting point: {1}\nHardness: {2}";

		// Token: 0x0400DBA0 RID: 56224
		public static LocString ELEMENTDESCLIQUID = "Resource Type: {0}\nFreezing point: {1}\nEvaporation point: {2}";

		// Token: 0x0400DBA1 RID: 56225
		public static LocString ELEMENTDESCGAS = "Resource Type: {0}\nCondensation point: {1}";

		// Token: 0x0400DBA2 RID: 56226
		public static LocString ELEMENTDESCVACUUM = "Resource Type: {0}";

		// Token: 0x0400DBA3 RID: 56227
		public static LocString BREATHABLEDESC = "<color=#{0}>({1})</color>";

		// Token: 0x0400DBA4 RID: 56228
		public static LocString THERMALPROPERTIES = "\nSpecific Heat Capacity: {SPECIFIC_HEAT_CAPACITY}\nThermal Conductivity: {THERMAL_CONDUCTIVITY}";

		// Token: 0x0400DBA5 RID: 56229
		public static LocString RADIATIONPROPERTIES = "Radiation Absorption Factor: {0}\nRadiation Emission/1000kg: {1}";

		// Token: 0x0400DBA6 RID: 56230
		public static LocString ELEMENTPROPERTIES = "Properties: {0}";

		// Token: 0x020038AD RID: 14509
		public class STATE
		{
			// Token: 0x0400DBA7 RID: 56231
			public static LocString SOLID = "Solid";

			// Token: 0x0400DBA8 RID: 56232
			public static LocString LIQUID = "Liquid";

			// Token: 0x0400DBA9 RID: 56233
			public static LocString GAS = "Gas";

			// Token: 0x0400DBAA RID: 56234
			public static LocString VACUUM = "None";
		}

		// Token: 0x020038AE RID: 14510
		public class MATERIAL_MODIFIERS
		{
			// Token: 0x0400DBAB RID: 56235
			public static LocString EFFECTS_HEADER = "<b>Resource Effects:</b>";

			// Token: 0x0400DBAC RID: 56236
			public static LocString DECOR = UI.FormatAsLink("Decor", "DECOR") + ": {0}";

			// Token: 0x0400DBAD RID: 56237
			public static LocString OVERHEATTEMPERATURE = UI.FormatAsLink("Overheat Temperature", "HEAT") + ": {0}";

			// Token: 0x0400DBAE RID: 56238
			public static LocString HIGH_THERMAL_CONDUCTIVITY = UI.FormatAsLink("High Thermal Conductivity", "HEAT");

			// Token: 0x0400DBAF RID: 56239
			public static LocString LOW_THERMAL_CONDUCTIVITY = UI.FormatAsLink("Insulator", "HEAT");

			// Token: 0x0400DBB0 RID: 56240
			public static LocString LOW_SPECIFIC_HEAT_CAPACITY = UI.FormatAsLink("Thermally Reactive", "HEAT");

			// Token: 0x0400DBB1 RID: 56241
			public static LocString HIGH_SPECIFIC_HEAT_CAPACITY = UI.FormatAsLink("Slow Heating", "HEAT");

			// Token: 0x0400DBB2 RID: 56242
			public static LocString EXCELLENT_RADIATION_SHIELD = UI.FormatAsLink("Excellent Radiation Shield", "RADIATION");

			// Token: 0x020038AF RID: 14511
			public class TOOLTIP
			{
				// Token: 0x0400DBB3 RID: 56243
				public static LocString EFFECTS_HEADER = "Buildings constructed from this material will have these properties";

				// Token: 0x0400DBB4 RID: 56244
				public static LocString DECOR = "This material will add <b>{0}</b> to the finished building's " + UI.PRE_KEYWORD + "Decor" + UI.PST_KEYWORD;

				// Token: 0x0400DBB5 RID: 56245
				public static LocString OVERHEATTEMPERATURE = "This material will add <b>{0}</b> to the finished building's " + UI.PRE_KEYWORD + "Overheat Temperature" + UI.PST_KEYWORD;

				// Token: 0x0400DBB6 RID: 56246
				public static LocString HIGH_THERMAL_CONDUCTIVITY = string.Concat(new string[]
				{
					"This material disperses ",
					UI.PRE_KEYWORD,
					"Heat",
					UI.PST_KEYWORD,
					" because energy transfers quickly through materials with high ",
					UI.PRE_KEYWORD,
					"Thermal Conductivity",
					UI.PST_KEYWORD,
					"\n\nBetween two objects, the rate of ",
					UI.PRE_KEYWORD,
					"Heat",
					UI.PST_KEYWORD,
					" transfer will be determined by the object with the <i>lowest</i> ",
					UI.PRE_KEYWORD,
					"Thermal Conductivity",
					UI.PST_KEYWORD,
					"\n\nThermal Conductivity: {1} W per degree K difference (Oxygen: 0.024 W)"
				});

				// Token: 0x0400DBB7 RID: 56247
				public static LocString LOW_THERMAL_CONDUCTIVITY = string.Concat(new string[]
				{
					"This material retains ",
					UI.PRE_KEYWORD,
					"Heat",
					UI.PST_KEYWORD,
					" because energy transfers slowly through materials with low ",
					UI.PRE_KEYWORD,
					"Thermal Conductivity",
					UI.PST_KEYWORD,
					"\n\nBetween two objects, the rate of ",
					UI.PRE_KEYWORD,
					"Heat",
					UI.PST_KEYWORD,
					" transfer will be determined by the object with the <i>lowest</i> ",
					UI.PRE_KEYWORD,
					"Thermal Conductivity",
					UI.PST_KEYWORD,
					"\n\nThermal Conductivity: {1} W per degree K difference (Oxygen: 0.024 W)"
				});

				// Token: 0x0400DBB8 RID: 56248
				public static LocString LOW_SPECIFIC_HEAT_CAPACITY = string.Concat(new string[]
				{
					UI.PRE_KEYWORD,
					"Thermally Reactive",
					UI.PST_KEYWORD,
					" materials require little energy to raise in ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					", and therefore heat and cool quickly\n\nSpecific Heat Capacity: {1} DTU to raise 1g by 1K"
				});

				// Token: 0x0400DBB9 RID: 56249
				public static LocString HIGH_SPECIFIC_HEAT_CAPACITY = string.Concat(new string[]
				{
					UI.PRE_KEYWORD,
					"Slow Heating",
					UI.PST_KEYWORD,
					" materials require a large amount of energy to raise in ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					", and therefore heat and cool slowly\n\nSpecific Heat Capacity: {1} DTU to raise 1g by 1K"
				});

				// Token: 0x0400DBBA RID: 56250
				public static LocString EXCELLENT_RADIATION_SHIELD = string.Concat(new string[]
				{
					UI.PRE_KEYWORD,
					"Excellent Radiation Shield",
					UI.PST_KEYWORD,
					" radiation has a hard time passing through materials with a high ",
					UI.PRE_KEYWORD,
					"Radiation Absorption Factor",
					UI.PST_KEYWORD,
					" value. \n\nRadiation Absorption Factor: {1}"
				});
			}
		}

		// Token: 0x020038B0 RID: 14512
		public class HARDNESS
		{
			// Token: 0x0400DBBB RID: 56251
			public static LocString NA = "N/A";

			// Token: 0x0400DBBC RID: 56252
			public static LocString SOFT = "{0} (" + ELEMENTS.HARDNESS.HARDNESS_DESCRIPTOR.SOFT + ")";

			// Token: 0x0400DBBD RID: 56253
			public static LocString VERYSOFT = "{0} (" + ELEMENTS.HARDNESS.HARDNESS_DESCRIPTOR.VERYSOFT + ")";

			// Token: 0x0400DBBE RID: 56254
			public static LocString FIRM = "{0} (" + ELEMENTS.HARDNESS.HARDNESS_DESCRIPTOR.FIRM + ")";

			// Token: 0x0400DBBF RID: 56255
			public static LocString VERYFIRM = "{0} (" + ELEMENTS.HARDNESS.HARDNESS_DESCRIPTOR.VERYFIRM + ")";

			// Token: 0x0400DBC0 RID: 56256
			public static LocString NEARLYIMPENETRABLE = "{0} (" + ELEMENTS.HARDNESS.HARDNESS_DESCRIPTOR.NEARLYIMPENETRABLE + ")";

			// Token: 0x0400DBC1 RID: 56257
			public static LocString IMPENETRABLE = "{0} (" + ELEMENTS.HARDNESS.HARDNESS_DESCRIPTOR.IMPENETRABLE + ")";

			// Token: 0x020038B1 RID: 14513
			public class HARDNESS_DESCRIPTOR
			{
				// Token: 0x0400DBC2 RID: 56258
				public static LocString SOFT = "Soft";

				// Token: 0x0400DBC3 RID: 56259
				public static LocString VERYSOFT = "Very Soft";

				// Token: 0x0400DBC4 RID: 56260
				public static LocString FIRM = "Firm";

				// Token: 0x0400DBC5 RID: 56261
				public static LocString VERYFIRM = "Very Firm";

				// Token: 0x0400DBC6 RID: 56262
				public static LocString NEARLYIMPENETRABLE = "Nearly Impenetrable";

				// Token: 0x0400DBC7 RID: 56263
				public static LocString IMPENETRABLE = "Impenetrable";
			}
		}

		// Token: 0x020038B2 RID: 14514
		public class AEROGEL
		{
			// Token: 0x0400DBC8 RID: 56264
			public static LocString NAME = UI.FormatAsLink("Aerogel", "AEROGEL");

			// Token: 0x0400DBC9 RID: 56265
			public static LocString DESC = "";
		}

		// Token: 0x020038B3 RID: 14515
		public class ALGAE
		{
			// Token: 0x0400DBCA RID: 56266
			public static LocString NAME = UI.FormatAsLink("Algae", "ALGAE");

			// Token: 0x0400DBCB RID: 56267
			public static LocString DESC = string.Concat(new string[]
			{
				"Algae is a cluster of non-motile, single-celled lifeforms.\n\nIt can be used to produce ",
				ELEMENTS.OXYGEN.NAME,
				" when used in an ",
				BUILDINGS.PREFABS.MINERALDEOXIDIZER.NAME,
				"."
			});
		}

		// Token: 0x020038B4 RID: 14516
		public class ALUMINUMORE
		{
			// Token: 0x0400DBCC RID: 56268
			public static LocString NAME = UI.FormatAsLink("Aluminum Ore", "ALUMINUMORE");

			// Token: 0x0400DBCD RID: 56269
			public static LocString DESC = "Aluminum ore, also known as Bauxite, is a sedimentary rock high in metal content.\n\nIt can be refined into " + UI.FormatAsLink("Aluminum", "ALUMINUM") + ".";
		}

		// Token: 0x020038B5 RID: 14517
		public class ALUMINUM
		{
			// Token: 0x0400DBCE RID: 56270
			public static LocString NAME = UI.FormatAsLink("Aluminum", "ALUMINUM");

			// Token: 0x0400DBCF RID: 56271
			public static LocString DESC = string.Concat(new string[]
			{
				"(Al) Aluminum is a low density ",
				UI.FormatAsLink("Metal", "REFINEDMETAL"),
				".\n\nIt has high Thermal Conductivity and is suitable for building ",
				UI.FormatAsLink("Power", "POWER"),
				" systems."
			});
		}

		// Token: 0x020038B6 RID: 14518
		public class MOLTENALUMINUM
		{
			// Token: 0x0400DBD0 RID: 56272
			public static LocString NAME = UI.FormatAsLink("Molten Aluminum", "MOLTENALUMINUM");

			// Token: 0x0400DBD1 RID: 56273
			public static LocString DESC = string.Concat(new string[]
			{
				"(Al) Molten Aluminum is a low density ",
				UI.FormatAsLink("Metal", "REFINEDMETAL"),
				" heated into a ",
				UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
				" state."
			});
		}

		// Token: 0x020038B7 RID: 14519
		public class ALUMINUMGAS
		{
			// Token: 0x0400DBD2 RID: 56274
			public static LocString NAME = UI.FormatAsLink("Aluminum Gas", "ALUMINUMGAS");

			// Token: 0x0400DBD3 RID: 56275
			public static LocString DESC = string.Concat(new string[]
			{
				"(Al) Aluminum Gas is a low density ",
				UI.FormatAsLink("Metal", "REFINEDMETAL"),
				" heated into a ",
				UI.FormatAsLink("Gaseous", "ELEMENTS_GAS"),
				" state."
			});
		}

		// Token: 0x020038B8 RID: 14520
		public class BLEACHSTONE
		{
			// Token: 0x0400DBD4 RID: 56276
			public static LocString NAME = UI.FormatAsLink("Bleach Stone", "BLEACHSTONE");

			// Token: 0x0400DBD5 RID: 56277
			public static LocString DESC = string.Concat(new string[]
			{
				"Bleach stone is an unstable compound that emits unbreathable ",
				UI.FormatAsLink("Chlorine Gas", "CHLORINEGAS"),
				".\n\nIt is useful in ",
				UI.FormatAsLink("Hygienic", "HYGIENE"),
				" processes."
			});
		}

		// Token: 0x020038B9 RID: 14521
		public class BITUMEN
		{
			// Token: 0x0400DBD6 RID: 56278
			public static LocString NAME = UI.FormatAsLink("Bitumen", "BITUMEN");

			// Token: 0x0400DBD7 RID: 56279
			public static LocString DESC = "Bitumen is a sticky viscous residue left behind from " + ELEMENTS.PETROLEUM.NAME + " production.";
		}

		// Token: 0x020038BA RID: 14522
		public class BOTTLEDWATER
		{
			// Token: 0x0400DBD8 RID: 56280
			public static LocString NAME = UI.FormatAsLink("Water", "BOTTLEDWATER");

			// Token: 0x0400DBD9 RID: 56281
			public static LocString DESC = "(H<sub>2</sub>O) Clean " + ELEMENTS.WATER.NAME + ", prepped for transport.";
		}

		// Token: 0x020038BB RID: 14523
		public class BRINEICE
		{
			// Token: 0x0400DBDA RID: 56282
			public static LocString NAME = UI.FormatAsLink("Brine Ice", "BRINEICE");

			// Token: 0x0400DBDB RID: 56283
			public static LocString DESC = string.Concat(new string[]
			{
				"Brine Ice is a natural, highly concentrated solution of ",
				UI.FormatAsLink("Salt", "SALT"),
				" dissolved in ",
				UI.FormatAsLink("Water", "WATER"),
				" and frozen into a ",
				UI.FormatAsLink("Solid", "ELEMENTS_SOLID"),
				" state.\n\nIt can be used in desalination processes, separating out usable salt."
			});
		}

		// Token: 0x020038BC RID: 14524
		public class MILKICE
		{
			// Token: 0x0400DBDC RID: 56284
			public static LocString NAME = UI.FormatAsLink("Frozen Brackene", "MILKICE");

			// Token: 0x0400DBDD RID: 56285
			public static LocString DESC = string.Concat(new string[]
			{
				"Frozen Brackene is ",
				UI.FormatAsLink("Brackene", "MILK"),
				" frozen into a ",
				UI.FormatAsLink("Solid", "ELEMENTS_SOLID"),
				" state."
			});
		}

		// Token: 0x020038BD RID: 14525
		public class BRINE
		{
			// Token: 0x0400DBDE RID: 56286
			public static LocString NAME = UI.FormatAsLink("Brine", "BRINE");

			// Token: 0x0400DBDF RID: 56287
			public static LocString DESC = string.Concat(new string[]
			{
				"Brine is a natural, highly concentrated solution of ",
				UI.FormatAsLink("Salt", "SALT"),
				" dissolved in ",
				UI.FormatAsLink("Water", "WATER"),
				".\n\nIt can be used in desalination processes, separating out usable salt."
			});
		}

		// Token: 0x020038BE RID: 14526
		public class CARBON
		{
			// Token: 0x0400DBE0 RID: 56288
			public static LocString NAME = UI.FormatAsLink("Coal", "CARBON");

			// Token: 0x0400DBE1 RID: 56289
			public static LocString DESC = "(C) Coal is a combustible fossil fuel composed of carbon.\n\nIt is useful in " + UI.FormatAsLink("Power", "POWER") + " production.";
		}

		// Token: 0x020038BF RID: 14527
		public class REFINEDCARBON
		{
			// Token: 0x0400DBE2 RID: 56290
			public static LocString NAME = UI.FormatAsLink("Refined Carbon", "REFINEDCARBON");

			// Token: 0x0400DBE3 RID: 56291
			public static LocString DESC = "(C) Refined carbon is solid element purified from raw " + ELEMENTS.CARBON.NAME + ".";
		}

		// Token: 0x020038C0 RID: 14528
		public class ETHANOLGAS
		{
			// Token: 0x0400DBE4 RID: 56292
			public static LocString NAME = UI.FormatAsLink("Ethanol Gas", "ETHANOLGAS");

			// Token: 0x0400DBE5 RID: 56293
			public static LocString DESC = "(C<sub>2</sub>H<sub>6</sub>O) Ethanol Gas is an advanced chemical compound heated into a " + UI.FormatAsLink("Gaseous", "ELEMENTS_GAS") + " state.";
		}

		// Token: 0x020038C1 RID: 14529
		public class ETHANOL
		{
			// Token: 0x0400DBE6 RID: 56294
			public static LocString NAME = UI.FormatAsLink("Ethanol", "ETHANOL");

			// Token: 0x0400DBE7 RID: 56295
			public static LocString DESC = "(C<sub>2</sub>H<sub>6</sub>O) Ethanol is an advanced chemical compound in a " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " state.\n\nIt can be used as a highly effective fuel source when burned.";
		}

		// Token: 0x020038C2 RID: 14530
		public class SOLIDETHANOL
		{
			// Token: 0x0400DBE8 RID: 56296
			public static LocString NAME = UI.FormatAsLink("Solid Ethanol", "SOLIDETHANOL");

			// Token: 0x0400DBE9 RID: 56297
			public static LocString DESC = "(C<sub>2</sub>H<sub>6</sub>O) Solid Ethanol is an advanced chemical compound.\n\nIt can be used as a highly effective fuel source when burned.";
		}

		// Token: 0x020038C3 RID: 14531
		public class CARBONDIOXIDE
		{
			// Token: 0x0400DBEA RID: 56298
			public static LocString NAME = UI.FormatAsLink("Carbon Dioxide", "CARBONDIOXIDE");

			// Token: 0x0400DBEB RID: 56299
			public static LocString DESC = "(CO<sub>2</sub>) Carbon Dioxide is an atomically heavy chemical compound in a " + UI.FormatAsLink("Gaseous", "ELEMENTS_GAS") + " state.\n\nIt tends to sink below other gases.";
		}

		// Token: 0x020038C4 RID: 14532
		public class CARBONFIBRE
		{
			// Token: 0x0400DBEC RID: 56300
			public static LocString NAME = UI.FormatAsLink("Carbon Fiber", "CARBONFIBRE");

			// Token: 0x0400DBED RID: 56301
			public static LocString DESC = "Carbon Fiber is a " + UI.FormatAsLink("Manufactured Material", "REFINEDMINERAL") + " with high tensile strength.";
		}

		// Token: 0x020038C5 RID: 14533
		public class CARBONGAS
		{
			// Token: 0x0400DBEE RID: 56302
			public static LocString NAME = UI.FormatAsLink("Carbon Gas", "CARBONGAS");

			// Token: 0x0400DBEF RID: 56303
			public static LocString DESC = "(C) Carbon is an abundant, versatile element heated into a " + UI.FormatAsLink("Gaseous", "ELEMENTS_GAS") + " state.";
		}

		// Token: 0x020038C6 RID: 14534
		public class CHLORINE
		{
			// Token: 0x0400DBF0 RID: 56304
			public static LocString NAME = UI.FormatAsLink("Liquid Chlorine", "CHLORINE");

			// Token: 0x0400DBF1 RID: 56305
			public static LocString DESC = string.Concat(new string[]
			{
				"(Cl) Chlorine is a natural ",
				UI.FormatAsLink("Germ", "DISEASE"),
				"-killing element in a ",
				UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
				" state."
			});
		}

		// Token: 0x020038C7 RID: 14535
		public class CHLORINEGAS
		{
			// Token: 0x0400DBF2 RID: 56306
			public static LocString NAME = UI.FormatAsLink("Chlorine Gas", "CHLORINEGAS");

			// Token: 0x0400DBF3 RID: 56307
			public static LocString DESC = string.Concat(new string[]
			{
				"(Cl) Chlorine is a natural ",
				UI.FormatAsLink("Germ", "DISEASE"),
				"-killing element in a ",
				UI.FormatAsLink("Gaseous", "ELEMENTS_GAS"),
				" state."
			});
		}

		// Token: 0x020038C8 RID: 14536
		public class CLAY
		{
			// Token: 0x0400DBF4 RID: 56308
			public static LocString NAME = UI.FormatAsLink("Clay", "CLAY");

			// Token: 0x0400DBF5 RID: 56309
			public static LocString DESC = "Clay is a soft, naturally occurring composite of stone and soil that hardens at high " + UI.FormatAsLink("Temperatures", "HEAT") + ".\n\nIt is a reliable <b>Construction Material</b>.";
		}

		// Token: 0x020038C9 RID: 14537
		public class BRICK
		{
			// Token: 0x0400DBF6 RID: 56310
			public static LocString NAME = UI.FormatAsLink("Brick", "BRICK");

			// Token: 0x0400DBF7 RID: 56311
			public static LocString DESC = "Brick is a hard, brittle material formed from heated " + ELEMENTS.CLAY.NAME + ".\n\nIt is a reliable <b>Construction Material</b>.";
		}

		// Token: 0x020038CA RID: 14538
		public class CERAMIC
		{
			// Token: 0x0400DBF8 RID: 56312
			public static LocString NAME = UI.FormatAsLink("Ceramic", "CERAMIC");

			// Token: 0x0400DBF9 RID: 56313
			public static LocString DESC = "Ceramic is a hard, brittle material formed from heated " + ELEMENTS.CLAY.NAME + ".\n\nIt is a reliable <b>Construction Material</b>.";
		}

		// Token: 0x020038CB RID: 14539
		public class CEMENT
		{
			// Token: 0x0400DBFA RID: 56314
			public static LocString NAME = UI.FormatAsLink("Cement", "CEMENT");

			// Token: 0x0400DBFB RID: 56315
			public static LocString DESC = "Cement is a refined building material used for assembling advanced buildings.";
		}

		// Token: 0x020038CC RID: 14540
		public class CEMENTMIX
		{
			// Token: 0x0400DBFC RID: 56316
			public static LocString NAME = UI.FormatAsLink("Cement Mix", "CEMENTMIX");

			// Token: 0x0400DBFD RID: 56317
			public static LocString DESC = "Cement Mix can be used to create " + ELEMENTS.CEMENT.NAME + " for advanced building assembly.";
		}

		// Token: 0x020038CD RID: 14541
		public class CONTAMINATEDOXYGEN
		{
			// Token: 0x0400DBFE RID: 56318
			public static LocString NAME = UI.FormatAsLink("Polluted Oxygen", "CONTAMINATEDOXYGEN");

			// Token: 0x0400DBFF RID: 56319
			public static LocString DESC = "(O<sub>2</sub>) Polluted Oxygen is dirty, unfiltered air.\n\nIt is breathable.";
		}

		// Token: 0x020038CE RID: 14542
		public class COPPER
		{
			// Token: 0x0400DC00 RID: 56320
			public static LocString NAME = UI.FormatAsLink("Copper", "COPPER");

			// Token: 0x0400DC01 RID: 56321
			public static LocString DESC = string.Concat(new string[]
			{
				"(Cu) Copper is a conductive ",
				UI.FormatAsLink("Metal", "METAL"),
				".\n\nIt is suitable for building ",
				UI.FormatAsLink("Power", "POWER"),
				" systems."
			});
		}

		// Token: 0x020038CF RID: 14543
		public class COPPERGAS
		{
			// Token: 0x0400DC02 RID: 56322
			public static LocString NAME = UI.FormatAsLink("Copper Gas", "COPPERGAS");

			// Token: 0x0400DC03 RID: 56323
			public static LocString DESC = string.Concat(new string[]
			{
				"(Cu) Copper Gas is a conductive ",
				UI.FormatAsLink("Metal", "METAL"),
				" heated into a ",
				UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
				" state."
			});
		}

		// Token: 0x020038D0 RID: 14544
		public class CREATURE
		{
			// Token: 0x0400DC04 RID: 56324
			public static LocString NAME = UI.FormatAsLink("Genetic Ooze", "CREATURE");

			// Token: 0x0400DC05 RID: 56325
			public static LocString DESC = "(DuPe) Ooze is a slurry of water, carbon, and dozens and dozens of trace elements.\n\nDuplicants are printed from pure Ooze.";
		}

		// Token: 0x020038D1 RID: 14545
		public class PHYTOOIL
		{
			// Token: 0x0400DC06 RID: 56326
			public static LocString NAME = UI.FormatAsLink("Phyto Oil", "PHYTOOIL");

			// Token: 0x0400DC07 RID: 56327
			public static LocString DESC = string.Concat(new string[]
			{
				"Phyto Oil is a thick, slippery ",
				UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
				" extracted from pureed ",
				UI.FormatAsLink("Slime", "SLIME"),
				"."
			});
		}

		// Token: 0x020038D2 RID: 14546
		public class FROZENPHYTOOIL
		{
			// Token: 0x0400DC08 RID: 56328
			public static LocString NAME = UI.FormatAsLink("Frozen Phyto Oil", "FROZENPHYTOOIL");

			// Token: 0x0400DC09 RID: 56329
			public static LocString DESC = string.Concat(new string[]
			{
				"Frozen Phyto Oil is thick, slippery ",
				UI.FormatAsLink("Slime", "SLIME"),
				" puree extract frozen into a ",
				UI.FormatAsLink("Solid", "ELEMENTS_SOLID"),
				" state."
			});
		}

		// Token: 0x020038D3 RID: 14547
		public class CRUDEOIL
		{
			// Token: 0x0400DC0A RID: 56330
			public static LocString NAME = UI.FormatAsLink("Crude Oil", "CRUDEOIL");

			// Token: 0x0400DC0B RID: 56331
			public static LocString DESC = "Crude Oil is a raw potential " + UI.FormatAsLink("Power", "POWER") + " source composed of billions of dead, primordial organisms.\n\nIt is also a useful lubricant for certain types of machinery.";
		}

		// Token: 0x020038D4 RID: 14548
		public class PETROLEUM
		{
			// Token: 0x0400DC0C RID: 56332
			public static LocString NAME = UI.FormatAsLink("Petroleum", "PETROLEUM");

			// Token: 0x0400DC0D RID: 56333
			public static LocString NAME_TWO = UI.FormatAsLink("Petroleum", "PETROLEUM");

			// Token: 0x0400DC0E RID: 56334
			public static LocString DESC = string.Concat(new string[]
			{
				"Petroleum is a ",
				UI.FormatAsLink("Power", "POWER"),
				" source refined from ",
				UI.FormatAsLink("Crude Oil", "CRUDEOIL"),
				".\n\nIt is also an essential ingredient in the production of ",
				UI.FormatAsLink("Plastic", "POLYPROPYLENE"),
				"."
			});
		}

		// Token: 0x020038D5 RID: 14549
		public class SOURGAS
		{
			// Token: 0x0400DC0F RID: 56335
			public static LocString NAME = UI.FormatAsLink("Sour Gas", "SOURGAS");

			// Token: 0x0400DC10 RID: 56336
			public static LocString NAME_TWO = UI.FormatAsLink("Sour Gas", "SOURGAS");

			// Token: 0x0400DC11 RID: 56337
			public static LocString DESC = string.Concat(new string[]
			{
				"Sour Gas is a hydrocarbon ",
				UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
				" containing high concentrations of hydrogen sulfide.\n\nIt is a byproduct of highly heated ",
				UI.FormatAsLink("Petroleum", "PETROLEUM"),
				"."
			});
		}

		// Token: 0x020038D6 RID: 14550
		public class CRUSHEDICE
		{
			// Token: 0x0400DC12 RID: 56338
			public static LocString NAME = UI.FormatAsLink("Crushed Ice", "CRUSHEDICE");

			// Token: 0x0400DC13 RID: 56339
			public static LocString DESC = "(H<sub>2</sub>O) A slush of crushed, semi-solid ice.";
		}

		// Token: 0x020038D7 RID: 14551
		public class CRUSHEDROCK
		{
			// Token: 0x0400DC14 RID: 56340
			public static LocString NAME = UI.FormatAsLink("Crushed Rock", "CRUSHEDROCK");

			// Token: 0x0400DC15 RID: 56341
			public static LocString DESC = "Crushed Rock is " + UI.FormatAsLink("Igneous Rock", "IGNEOUSROCK") + " crushed into a mechanical mixture.";
		}

		// Token: 0x020038D8 RID: 14552
		public class CUPRITE
		{
			// Token: 0x0400DC16 RID: 56342
			public static LocString NAME = UI.FormatAsLink("Copper Ore", "CUPRITE");

			// Token: 0x0400DC17 RID: 56343
			public static LocString DESC = string.Concat(new string[]
			{
				"(Cu<sub>2</sub>O) Copper Ore is a conductive ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				".\n\nIt is suitable for building ",
				UI.FormatAsLink("Power", "POWER"),
				" systems."
			});
		}

		// Token: 0x020038D9 RID: 14553
		public class DEPLETEDURANIUM
		{
			// Token: 0x0400DC18 RID: 56344
			public static LocString NAME = UI.FormatAsLink("Depleted Uranium", "DEPLETEDURANIUM");

			// Token: 0x0400DC19 RID: 56345
			public static LocString DESC = string.Concat(new string[]
			{
				"(U) Depleted Uranium is ",
				UI.FormatAsLink("Uranium", "URANIUMORE"),
				" with a low U-235 content.\n\nIt is created as a byproduct of ",
				UI.FormatAsLink("Enriched Uranium", "ENRICHEDURANIUM"),
				" and is no longer suitable as fuel."
			});
		}

		// Token: 0x020038DA RID: 14554
		public class DIAMOND
		{
			// Token: 0x0400DC1A RID: 56346
			public static LocString NAME = UI.FormatAsLink("Diamond", "DIAMOND");

			// Token: 0x0400DC1B RID: 56347
			public static LocString DESC = "(C) Diamond is industrial-grade, high density carbon.\n\nIt is very difficult to excavate.";
		}

		// Token: 0x020038DB RID: 14555
		public class DIRT
		{
			// Token: 0x0400DC1C RID: 56348
			public static LocString NAME = UI.FormatAsLink("Dirt", "DIRT");

			// Token: 0x0400DC1D RID: 56349
			public static LocString DESC = "Dirt is a soft, nutrient-rich substance capable of supporting life.\n\nIt is necessary in some forms of " + UI.FormatAsLink("Food", "FOOD") + " production.";
		}

		// Token: 0x020038DC RID: 14556
		public class DIRTYICE
		{
			// Token: 0x0400DC1E RID: 56350
			public static LocString NAME = UI.FormatAsLink("Polluted Ice", "DIRTYICE");

			// Token: 0x0400DC1F RID: 56351
			public static LocString DESC = "Polluted Ice is dirty, unfiltered water frozen into a " + UI.FormatAsLink("Solid", "ELEMENTS_SOLID") + " state.";
		}

		// Token: 0x020038DD RID: 14557
		public class DIRTYWATER
		{
			// Token: 0x0400DC20 RID: 56352
			public static LocString NAME = UI.FormatAsLink("Polluted Water", "DIRTYWATER");

			// Token: 0x0400DC21 RID: 56353
			public static LocString DESC = "Polluted Water is dirty, unfiltered " + UI.FormatAsLink("Water", "WATER") + ".\n\nIt is not fit for consumption.";
		}

		// Token: 0x020038DE RID: 14558
		public class ELECTRUM
		{
			// Token: 0x0400DC22 RID: 56354
			public static LocString NAME = UI.FormatAsLink("Electrum", "ELECTRUM");

			// Token: 0x0400DC23 RID: 56355
			public static LocString DESC = string.Concat(new string[]
			{
				"Electrum is a conductive ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				" alloy composed of gold and silver.\n\nIt is suitable for building ",
				UI.FormatAsLink("Power", "POWER"),
				" systems."
			});
		}

		// Token: 0x020038DF RID: 14559
		public class ENRICHEDURANIUM
		{
			// Token: 0x0400DC24 RID: 56356
			public static LocString NAME = UI.FormatAsLink("Enriched Uranium", "ENRICHEDURANIUM");

			// Token: 0x0400DC25 RID: 56357
			public static LocString DESC = string.Concat(new string[]
			{
				"(U) Enriched Uranium is a highly ",
				UI.FormatAsLink("Radioactive", "RADIATION"),
				", refined substance.\n\nIt is primarily used to ",
				UI.FormatAsLink("Power", "POWER"),
				" potent research reactors."
			});
		}

		// Token: 0x020038E0 RID: 14560
		public class FERTILIZER
		{
			// Token: 0x0400DC26 RID: 56358
			public static LocString NAME = UI.FormatAsLink("Fertilizer", "FERTILIZER");

			// Token: 0x0400DC27 RID: 56359
			public static LocString DESC = "Fertilizer is a processed mixture of biological nutrients.\n\nIt aids in the growth of certain " + UI.FormatAsLink("Plants", "PLANTS") + ".";
		}

		// Token: 0x020038E1 RID: 14561
		public class PONDSCUM
		{
			// Token: 0x0400DC28 RID: 56360
			public static LocString NAME = UI.FormatAsLink("Pondscum", "PONDSCUM");

			// Token: 0x0400DC29 RID: 56361
			public static LocString DESC = string.Concat(new string[]
			{
				"Pondscum is a soft, naturally occurring composite of biological nutrients.\n\nIt may be processed into ",
				UI.FormatAsLink("Fertilizer", "FERTILIZER"),
				" and aids in the growth of certain ",
				UI.FormatAsLink("Plants", "PLANTS"),
				"."
			});
		}

		// Token: 0x020038E2 RID: 14562
		public class FALLOUT
		{
			// Token: 0x0400DC2A RID: 56362
			public static LocString NAME = UI.FormatAsLink("Nuclear Fallout", "FALLOUT");

			// Token: 0x0400DC2B RID: 56363
			public static LocString DESC = string.Concat(new string[]
			{
				"Nuclear Fallout is a highly toxic gas full of ",
				UI.FormatAsLink("Radioactive Contaminants", "RADIATION"),
				". Condenses into ",
				UI.FormatAsLink("Liquid Nuclear Waste", "NUCLEARWASTE"),
				"."
			});
		}

		// Token: 0x020038E3 RID: 14563
		public class FOOLSGOLD
		{
			// Token: 0x0400DC2C RID: 56364
			public static LocString NAME = UI.FormatAsLink("Pyrite", "FOOLSGOLD");

			// Token: 0x0400DC2D RID: 56365
			public static LocString DESC = string.Concat(new string[]
			{
				"(FeS<sub>2</sub>) Pyrite is a conductive ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				".\n\nAlso known as \"Fool's Gold\", is suitable for building ",
				UI.FormatAsLink("Power", "POWER"),
				" systems."
			});
		}

		// Token: 0x020038E4 RID: 14564
		public class FULLERENE
		{
			// Token: 0x0400DC2E RID: 56366
			public static LocString NAME = UI.FormatAsLink("Fullerene", "FULLERENE");

			// Token: 0x0400DC2F RID: 56367
			public static LocString DESC = "(C<sub>60</sub>) Fullerene is a form of " + UI.FormatAsLink("Coal", "CARBON") + " consisting of spherical molecules.";
		}

		// Token: 0x020038E5 RID: 14565
		public class GLASS
		{
			// Token: 0x0400DC30 RID: 56368
			public static LocString NAME = UI.FormatAsLink("Glass", "GLASS");

			// Token: 0x0400DC31 RID: 56369
			public static LocString DESC = "Glass is a brittle, transparent substance formed from " + UI.FormatAsLink("Sand", "SAND") + " fired at high temperatures.";
		}

		// Token: 0x020038E6 RID: 14566
		public class GOLD
		{
			// Token: 0x0400DC32 RID: 56370
			public static LocString NAME = UI.FormatAsLink("Gold", "GOLD");

			// Token: 0x0400DC33 RID: 56371
			public static LocString DESC = string.Concat(new string[]
			{
				"(Au) Gold is a conductive precious ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				".\n\nIt is suitable for building ",
				UI.FormatAsLink("Power", "POWER"),
				" systems."
			});
		}

		// Token: 0x020038E7 RID: 14567
		public class GOLDAMALGAM
		{
			// Token: 0x0400DC34 RID: 56372
			public static LocString NAME = UI.FormatAsLink("Gold Amalgam", "GOLDAMALGAM");

			// Token: 0x0400DC35 RID: 56373
			public static LocString DESC = "Gold Amalgam is a conductive amalgam of gold and mercury.\n\nIt is suitable for building " + UI.FormatAsLink("Power", "POWER") + " systems.";
		}

		// Token: 0x020038E8 RID: 14568
		public class GOLDGAS
		{
			// Token: 0x0400DC36 RID: 56374
			public static LocString NAME = UI.FormatAsLink("Gold Gas", "GOLDGAS");

			// Token: 0x0400DC37 RID: 56375
			public static LocString DESC = string.Concat(new string[]
			{
				"(Au) Gold Gas is a conductive precious ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				", heated into a ",
				UI.FormatAsLink("Gaseous", "ELEMENTS_GAS"),
				" state."
			});
		}

		// Token: 0x020038E9 RID: 14569
		public class GRANITE
		{
			// Token: 0x0400DC38 RID: 56376
			public static LocString NAME = UI.FormatAsLink("Granite", "GRANITE");

			// Token: 0x0400DC39 RID: 56377
			public static LocString DESC = "Granite is a dense composite of " + UI.FormatAsLink("Igneous Rock", "IGNEOUSROCK") + ".\n\nIt is useful as a <b>Construction Material</b>.";
		}

		// Token: 0x020038EA RID: 14570
		public class GRAPHITE
		{
			// Token: 0x0400DC3A RID: 56378
			public static LocString NAME = UI.FormatAsLink("Graphite", "GRAPHITE");

			// Token: 0x0400DC3B RID: 56379
			public static LocString DESC = "(C) Graphite is the most stable form of carbon.\n\nIt has high thermal conductivity and is useful as a <b>Construction Material</b>.";
		}

		// Token: 0x020038EB RID: 14571
		public class LIQUIDGUNK
		{
			// Token: 0x0400DC3C RID: 56380
			public static LocString NAME = UI.FormatAsLink("Liquid Gunk", "LIQUIDGUNK");

			// Token: 0x0400DC3D RID: 56381
			public static LocString DESC = "Liquid Gunk is the built-up grime and grit produced by Duplicants' bionic mechanisms.\n\nIt is unpleasantly viscous.";
		}

		// Token: 0x020038EC RID: 14572
		public class GUNK
		{
			// Token: 0x0400DC3E RID: 56382
			public static LocString NAME = UI.FormatAsLink("Gunk", "GUNK");

			// Token: 0x0400DC3F RID: 56383
			public static LocString DESC = "Gunk is the built-up grime and grit produced by Duplicants' bionic mechanisms that has been frozen into a a " + UI.FormatAsLink("Solid", "ELEMENTS_SOLID") + " state.";
		}

		// Token: 0x020038ED RID: 14573
		public class SOLIDNUCLEARWASTE
		{
			// Token: 0x0400DC40 RID: 56384
			public static LocString NAME = UI.FormatAsLink("Solid Nuclear Waste", "SOLIDNUCLEARWASTE");

			// Token: 0x0400DC41 RID: 56385
			public static LocString DESC = "Highly toxic solid full of " + UI.FormatAsLink("Radioactive Contaminants", "RADIATION") + ".";
		}

		// Token: 0x020038EE RID: 14574
		public class HELIUM
		{
			// Token: 0x0400DC42 RID: 56386
			public static LocString NAME = UI.FormatAsLink("Helium", "HELIUM");

			// Token: 0x0400DC43 RID: 56387
			public static LocString DESC = "(He) Helium is an atomically lightweight, chemical " + UI.FormatAsLink("Gas", "ELEMENTS_GAS") + ".";
		}

		// Token: 0x020038EF RID: 14575
		public class HYDROGEN
		{
			// Token: 0x0400DC44 RID: 56388
			public static LocString NAME = UI.FormatAsLink("Hydrogen Gas", "HYDROGEN");

			// Token: 0x0400DC45 RID: 56389
			public static LocString DESC = "(H) Hydrogen Gas is the universe's most common and atomically light element in a " + UI.FormatAsLink("Gaseous", "ELEMENTS_GAS") + " state.";
		}

		// Token: 0x020038F0 RID: 14576
		public class ICE
		{
			// Token: 0x0400DC46 RID: 56390
			public static LocString NAME = UI.FormatAsLink("Ice", "ICE");

			// Token: 0x0400DC47 RID: 56391
			public static LocString DESC = "(H<sub>2</sub>O) Ice is clean water frozen into a " + UI.FormatAsLink("Solid", "ELEMENTS_SOLID") + " state.";
		}

		// Token: 0x020038F1 RID: 14577
		public class IGNEOUSROCK
		{
			// Token: 0x0400DC48 RID: 56392
			public static LocString NAME = UI.FormatAsLink("Igneous Rock", "IGNEOUSROCK");

			// Token: 0x0400DC49 RID: 56393
			public static LocString DESC = "Igneous Rock is a composite of solidified volcanic rock.\n\nIt is useful as a <b>Construction Material</b>.";
		}

		// Token: 0x020038F2 RID: 14578
		public class ISORESIN
		{
			// Token: 0x0400DC4A RID: 56394
			public static LocString NAME = UI.FormatAsLink("Isoresin", "ISORESIN");

			// Token: 0x0400DC4B RID: 56395
			public static LocString DESC = "Isoresin is a crystallized sap composed of long-chain polymers.\n\nIt is used in the production of rare, high grade materials.";
		}

		// Token: 0x020038F3 RID: 14579
		public class RESIN
		{
			// Token: 0x0400DC4C RID: 56396
			public static LocString NAME = UI.FormatAsLink("Liquid Resin", "RESIN");

			// Token: 0x0400DC4D RID: 56397
			public static LocString DESC = "Sticky goo harvested from a grumpy tree.\n\nIt can be polymerized into " + UI.FormatAsLink("Isoresin", "ISORESIN") + " by boiling away its excess moisture.";
		}

		// Token: 0x020038F4 RID: 14580
		public class SOLIDRESIN
		{
			// Token: 0x0400DC4E RID: 56398
			public static LocString NAME = UI.FormatAsLink("Solid Resin", "SOLIDRESIN");

			// Token: 0x0400DC4F RID: 56399
			public static LocString DESC = "Solidified goo harvested from a grumpy tree.\n\nIt is used in the production of " + UI.FormatAsLink("Isoresin", "ISORESIN") + ".";
		}

		// Token: 0x020038F5 RID: 14581
		public class IRON
		{
			// Token: 0x0400DC50 RID: 56400
			public static LocString NAME = UI.FormatAsLink("Iron", "IRON");

			// Token: 0x0400DC51 RID: 56401
			public static LocString DESC = "(Fe) Iron is a common industrial " + UI.FormatAsLink("Metal", "RAWMETAL") + ".";
		}

		// Token: 0x020038F6 RID: 14582
		public class IRONGAS
		{
			// Token: 0x0400DC52 RID: 56402
			public static LocString NAME = UI.FormatAsLink("Iron Gas", "IRONGAS");

			// Token: 0x0400DC53 RID: 56403
			public static LocString DESC = string.Concat(new string[]
			{
				"(Fe) Iron Gas is a common industrial ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				", heated into a ",
				UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
				"."
			});
		}

		// Token: 0x020038F7 RID: 14583
		public class IRONORE
		{
			// Token: 0x0400DC54 RID: 56404
			public static LocString NAME = UI.FormatAsLink("Iron Ore", "IRONORE");

			// Token: 0x0400DC55 RID: 56405
			public static LocString DESC = string.Concat(new string[]
			{
				"(Fe) Iron Ore is a soft ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				".\n\nIt is suitable for building ",
				UI.FormatAsLink("Power", "POWER"),
				" systems."
			});
		}

		// Token: 0x020038F8 RID: 14584
		public class COBALTGAS
		{
			// Token: 0x0400DC56 RID: 56406
			public static LocString NAME = UI.FormatAsLink("Cobalt Gas", "COBALTGAS");

			// Token: 0x0400DC57 RID: 56407
			public static LocString DESC = string.Concat(new string[]
			{
				"(Co) Cobalt is a ",
				UI.FormatAsLink("Refined Metal", "REFINEDMETAL"),
				", heated into a ",
				UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
				"."
			});
		}

		// Token: 0x020038F9 RID: 14585
		public class COBALT
		{
			// Token: 0x0400DC58 RID: 56408
			public static LocString NAME = UI.FormatAsLink("Cobalt", "COBALT");

			// Token: 0x0400DC59 RID: 56409
			public static LocString DESC = string.Concat(new string[]
			{
				"(Co) Cobalt is a ",
				UI.FormatAsLink("Refined Metal", "REFINEDMETAL"),
				" made from ",
				UI.FormatAsLink("Cobalt Ore", "COBALTITE"),
				"."
			});
		}

		// Token: 0x020038FA RID: 14586
		public class COBALTITE
		{
			// Token: 0x0400DC5A RID: 56410
			public static LocString NAME = UI.FormatAsLink("Cobalt Ore", "COBALTITE");

			// Token: 0x0400DC5B RID: 56411
			public static LocString DESC = string.Concat(new string[]
			{
				"(Co) Cobalt Ore is a blue-hued ",
				UI.FormatAsLink("Metal", "BUILDINGMATERIALCLASSES"),
				".\n\nIt is suitable for building ",
				UI.FormatAsLink("Power", "POWER"),
				" systems."
			});
		}

		// Token: 0x020038FB RID: 14587
		public class KATAIRITE
		{
			// Token: 0x0400DC5C RID: 56412
			public static LocString NAME = UI.FormatAsLink("Abyssalite", "KATAIRITE");

			// Token: 0x0400DC5D RID: 56413
			public static LocString DESC = "(Ab) Abyssalite is a resilient, crystalline element.";
		}

		// Token: 0x020038FC RID: 14588
		public class LIME
		{
			// Token: 0x0400DC5E RID: 56414
			public static LocString NAME = UI.FormatAsLink("Lime", "LIME");

			// Token: 0x0400DC5F RID: 56415
			public static LocString DESC = "(CaCO<sub>3</sub>) Lime is a mineral commonly found in " + UI.FormatAsLink("Critter", "CREATURES") + " egg shells.\n\nIt is useful as a <b>Construction Material</b>.";
		}

		// Token: 0x020038FD RID: 14589
		public class FOSSIL
		{
			// Token: 0x0400DC60 RID: 56416
			public static LocString NAME = UI.FormatAsLink("Fossil", "FOSSIL");

			// Token: 0x0400DC61 RID: 56417
			public static LocString DESC = "Fossil is organic matter, highly compressed and hardened into a mineral state.\n\nIt is useful as a <b>Construction Material</b>.";
		}

		// Token: 0x020038FE RID: 14590
		public class LEADGAS
		{
			// Token: 0x0400DC62 RID: 56418
			public static LocString NAME = UI.FormatAsLink("Lead Gas", "LEADGAS");

			// Token: 0x0400DC63 RID: 56419
			public static LocString DESC = string.Concat(new string[]
			{
				"(Pb) Lead Gas is a soft yet extremely dense ",
				UI.FormatAsLink("Refined Metal", "REFINEDMETAL"),
				" heated into a ",
				UI.FormatAsLink("Gaseous", "ELEMENTS_GAS"),
				"."
			});
		}

		// Token: 0x020038FF RID: 14591
		public class LEAD
		{
			// Token: 0x0400DC64 RID: 56420
			public static LocString NAME = UI.FormatAsLink("Lead", "LEAD");

			// Token: 0x0400DC65 RID: 56421
			public static LocString DESC = string.Concat(new string[]
			{
				"(Pb) Lead is a soft yet extremely dense ",
				UI.FormatAsLink("Refined Metal", "REFINEDMETAL"),
				".\n\nIt has a low Overheat Temperature and is suitable for building ",
				UI.FormatAsLink("Power", "POWER"),
				" systems."
			});
		}

		// Token: 0x02003900 RID: 14592
		public class LIQUIDCARBONDIOXIDE
		{
			// Token: 0x0400DC66 RID: 56422
			public static LocString NAME = UI.FormatAsLink("Liquid Carbon Dioxide", "LIQUIDCARBONDIOXIDE");

			// Token: 0x0400DC67 RID: 56423
			public static LocString DESC = "(CO<sub>2</sub>) Carbon Dioxide is an unbreathable chemical compound.\n\nThis selection is currently in a " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " state.";
		}

		// Token: 0x02003901 RID: 14593
		public class LIQUIDHELIUM
		{
			// Token: 0x0400DC68 RID: 56424
			public static LocString NAME = UI.FormatAsLink("Helium", "LIQUIDHELIUM");

			// Token: 0x0400DC69 RID: 56425
			public static LocString DESC = "(He) Helium is an atomically lightweight chemical element cooled into a " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " state.";
		}

		// Token: 0x02003902 RID: 14594
		public class LIQUIDHYDROGEN
		{
			// Token: 0x0400DC6A RID: 56426
			public static LocString NAME = UI.FormatAsLink("Liquid Hydrogen", "LIQUIDHYDROGEN");

			// Token: 0x0400DC6B RID: 56427
			public static LocString DESC = "(H) Hydrogen in its " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " state.\n\nIt freezes most substances that come into contact with it.";
		}

		// Token: 0x02003903 RID: 14595
		public class LIQUIDOXYGEN
		{
			// Token: 0x0400DC6C RID: 56428
			public static LocString NAME = UI.FormatAsLink("Liquid Oxygen", "LIQUIDOXYGEN");

			// Token: 0x0400DC6D RID: 56429
			public static LocString DESC = "(O<sub>2</sub>) Oxygen is a breathable chemical.\n\nThis selection is in a " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " state.";
		}

		// Token: 0x02003904 RID: 14596
		public class LIQUIDMETHANE
		{
			// Token: 0x0400DC6E RID: 56430
			public static LocString NAME = UI.FormatAsLink("Liquid Methane", "LIQUIDMETHANE");

			// Token: 0x0400DC6F RID: 56431
			public static LocString DESC = "(CH<sub>4</sub>) Methane is an alkane.\n\nThis selection is in a " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " state.";
		}

		// Token: 0x02003905 RID: 14597
		public class LIQUIDPHOSPHORUS
		{
			// Token: 0x0400DC70 RID: 56432
			public static LocString NAME = UI.FormatAsLink("Liquid Phosphorus", "LIQUIDPHOSPHORUS");

			// Token: 0x0400DC71 RID: 56433
			public static LocString DESC = "(P) Phosphorus is a chemical element.\n\nThis selection is in a " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " state.";
		}

		// Token: 0x02003906 RID: 14598
		public class LIQUIDPROPANE
		{
			// Token: 0x0400DC72 RID: 56434
			public static LocString NAME = UI.FormatAsLink("Liquid Propane", "LIQUIDPROPANE");

			// Token: 0x0400DC73 RID: 56435
			public static LocString DESC = string.Concat(new string[]
			{
				"(C<sub>3</sub>H<sub>8</sub>) Propane is an alkane.\n\nThis selection is in a ",
				UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
				" state.\n\nIt is useful in ",
				UI.FormatAsLink("Power", "POWER"),
				" production."
			});
		}

		// Token: 0x02003907 RID: 14599
		public class LIQUIDSULFUR
		{
			// Token: 0x0400DC74 RID: 56436
			public static LocString NAME = UI.FormatAsLink("Liquid Sulfur", "LIQUIDSULFUR");

			// Token: 0x0400DC75 RID: 56437
			public static LocString DESC = string.Concat(new string[]
			{
				"(S) Sulfur is a common chemical element and byproduct of ",
				UI.FormatAsLink("Natural Gas", "METHANE"),
				" production.\n\nThis selection is in a ",
				UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
				" state."
			});
		}

		// Token: 0x02003908 RID: 14600
		public class MAFICROCK
		{
			// Token: 0x0400DC76 RID: 56438
			public static LocString NAME = UI.FormatAsLink("Mafic Rock", "MAFICROCK");

			// Token: 0x0400DC77 RID: 56439
			public static LocString DESC = string.Concat(new string[]
			{
				"Mafic Rock is a variation of ",
				UI.FormatAsLink("Igneous Rock", "IGNEOUSROCK"),
				" that is rich in ",
				UI.FormatAsLink("Iron", "IRON"),
				".\n\nIt is useful as a <b>Construction Material</b>."
			});
		}

		// Token: 0x02003909 RID: 14601
		public class MAGMA
		{
			// Token: 0x0400DC78 RID: 56440
			public static LocString NAME = UI.FormatAsLink("Magma", "MAGMA");

			// Token: 0x0400DC79 RID: 56441
			public static LocString DESC = string.Concat(new string[]
			{
				"Magma is a composite of ",
				UI.FormatAsLink("Igneous Rock", "IGNEOUSROCK"),
				" heated into a molten, ",
				UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
				" state."
			});
		}

		// Token: 0x0200390A RID: 14602
		public class WOODLOG
		{
			// Token: 0x0400DC7A RID: 56442
			public static LocString NAME = UI.FormatAsLink("Wood", "WOOD");

			// Token: 0x0400DC7B RID: 56443
			public static LocString DESC = string.Concat(new string[]
			{
				"Wood is a good source of ",
				UI.FormatAsLink("Heat", "HEAT"),
				" and ",
				UI.FormatAsLink("Power", "POWER"),
				".\n\nIts insulation properties and positive ",
				UI.FormatAsLink("Decor", "DECOR"),
				" also make it a useful <b>Construction Material</b>."
			});
		}

		// Token: 0x0200390B RID: 14603
		public class CINNABAR
		{
			// Token: 0x0400DC7C RID: 56444
			public static LocString NAME = UI.FormatAsLink("Cinnabar Ore", "CINNABAR");

			// Token: 0x0400DC7D RID: 56445
			public static LocString DESC = string.Concat(new string[]
			{
				"(HgS) Cinnabar Ore, also known as mercury sulfide, is a conductive ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				" that can be refined into ",
				UI.FormatAsLink("Mercury", "MERCURY"),
				".\n\nIt is suitable for building ",
				UI.FormatAsLink("Power", "POWER"),
				" systems."
			});
		}

		// Token: 0x0200390C RID: 14604
		public class TALLOW
		{
			// Token: 0x0400DC7E RID: 56446
			public static LocString NAME = UI.FormatAsLink("Tallow", "TALLOW");

			// Token: 0x0400DC7F RID: 56447
			public static LocString DESC = "A chunk of uncooked grease from a deceased " + CREATURES.SPECIES.SEAL.NAME + ".";
		}

		// Token: 0x0200390D RID: 14605
		public class MERCURY
		{
			// Token: 0x0400DC80 RID: 56448
			public static LocString NAME = UI.FormatAsLink("Mercury", "MERCURY");

			// Token: 0x0400DC81 RID: 56449
			public static LocString DESC = "(Hg) Mercury is a metallic " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + ".";
		}

		// Token: 0x0200390E RID: 14606
		public class MERCURYGAS
		{
			// Token: 0x0400DC82 RID: 56450
			public static LocString NAME = UI.FormatAsLink("Mercury Gas", "MERCURYGAS");

			// Token: 0x0400DC83 RID: 56451
			public static LocString DESC = string.Concat(new string[]
			{
				"(Hg) Mercury Gas is a ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				" heated into a ",
				UI.FormatAsLink("Gaseous", "ELEMENTS_GAS"),
				" state."
			});
		}

		// Token: 0x0200390F RID: 14607
		public class METHANE
		{
			// Token: 0x0400DC84 RID: 56452
			public static LocString NAME = UI.FormatAsLink("Natural Gas", "METHANE");

			// Token: 0x0400DC85 RID: 56453
			public static LocString DESC = string.Concat(new string[]
			{
				"Natural Gas is a mixture of various alkanes in a ",
				UI.FormatAsLink("Gaseous", "ELEMENTS_GAS"),
				" state.\n\nIt is useful in ",
				UI.FormatAsLink("Power", "POWER"),
				" production."
			});
		}

		// Token: 0x02003910 RID: 14608
		public class MILK
		{
			// Token: 0x0400DC86 RID: 56454
			public static LocString NAME = UI.FormatAsLink("Brackene", "MILK");

			// Token: 0x0400DC87 RID: 56455
			public static LocString DESC = string.Concat(new string[]
			{
				"Brackene is a sodium-rich ",
				UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
				".\n\nIt is useful in ",
				UI.FormatAsLink("Ranching", "RANCHING"),
				"."
			});
		}

		// Token: 0x02003911 RID: 14609
		public class MILKFAT
		{
			// Token: 0x0400DC88 RID: 56456
			public static LocString NAME = UI.FormatAsLink("Brackwax", "MILKFAT");

			// Token: 0x0400DC89 RID: 56457
			public static LocString DESC = string.Concat(new string[]
			{
				"Brackwax is a ",
				UI.FormatAsLink("Solid", "ELEMENTS_SOLID"),
				" byproduct of ",
				UI.FormatAsLink("Brackene", "MILK"),
				"."
			});
		}

		// Token: 0x02003912 RID: 14610
		public class MOLTENCARBON
		{
			// Token: 0x0400DC8A RID: 56458
			public static LocString NAME = UI.FormatAsLink("Liquid Carbon", "MOLTENCARBON");

			// Token: 0x0400DC8B RID: 56459
			public static LocString DESC = "(C) Liquid Carbon is an abundant, versatile element heated into a " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " state.";
		}

		// Token: 0x02003913 RID: 14611
		public class MOLTENCOPPER
		{
			// Token: 0x0400DC8C RID: 56460
			public static LocString NAME = UI.FormatAsLink("Molten Copper", "MOLTENCOPPER");

			// Token: 0x0400DC8D RID: 56461
			public static LocString DESC = string.Concat(new string[]
			{
				"(Cu) Molten Copper is a conductive ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				" heated into a ",
				UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
				" state."
			});
		}

		// Token: 0x02003914 RID: 14612
		public class MOLTENGLASS
		{
			// Token: 0x0400DC8E RID: 56462
			public static LocString NAME = UI.FormatAsLink("Molten Glass", "MOLTENGLASS");

			// Token: 0x0400DC8F RID: 56463
			public static LocString DESC = "Molten Glass is a composite of granular rock, heated into a " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " state.";
		}

		// Token: 0x02003915 RID: 14613
		public class MOLTENGOLD
		{
			// Token: 0x0400DC90 RID: 56464
			public static LocString NAME = UI.FormatAsLink("Molten Gold", "MOLTENGOLD");

			// Token: 0x0400DC91 RID: 56465
			public static LocString DESC = string.Concat(new string[]
			{
				"(Au) Gold, a conductive precious ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				", heated into a ",
				UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
				" state."
			});
		}

		// Token: 0x02003916 RID: 14614
		public class MOLTENIRON
		{
			// Token: 0x0400DC92 RID: 56466
			public static LocString NAME = UI.FormatAsLink("Molten Iron", "MOLTENIRON");

			// Token: 0x0400DC93 RID: 56467
			public static LocString DESC = string.Concat(new string[]
			{
				"(Fe) Molten Iron is a common industrial ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				" heated into a ",
				UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
				" state."
			});
		}

		// Token: 0x02003917 RID: 14615
		public class MOLTENCOBALT
		{
			// Token: 0x0400DC94 RID: 56468
			public static LocString NAME = UI.FormatAsLink("Molten Cobalt", "MOLTENCOBALT");

			// Token: 0x0400DC95 RID: 56469
			public static LocString DESC = string.Concat(new string[]
			{
				"(Co) Molten Cobalt is a ",
				UI.FormatAsLink("Refined Metal", "REFINEDMETAL"),
				" heated into a ",
				UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
				" state."
			});
		}

		// Token: 0x02003918 RID: 14616
		public class MOLTENLEAD
		{
			// Token: 0x0400DC96 RID: 56470
			public static LocString NAME = UI.FormatAsLink("Molten Lead", "MOLTENLEAD");

			// Token: 0x0400DC97 RID: 56471
			public static LocString DESC = string.Concat(new string[]
			{
				"(Pb) Lead is an extremely dense ",
				UI.FormatAsLink("Refined Metal", "REFINEDMETAL"),
				" heated into a ",
				UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
				" state."
			});
		}

		// Token: 0x02003919 RID: 14617
		public class MOLTENNIOBIUM
		{
			// Token: 0x0400DC98 RID: 56472
			public static LocString NAME = UI.FormatAsLink("Molten Niobium", "MOLTENNIOBIUM");

			// Token: 0x0400DC99 RID: 56473
			public static LocString DESC = "(Nb) Molten Niobium is a rare metal heated into a " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " state.";
		}

		// Token: 0x0200391A RID: 14618
		public class MOLTENTUNGSTEN
		{
			// Token: 0x0400DC9A RID: 56474
			public static LocString NAME = UI.FormatAsLink("Molten Tungsten", "MOLTENTUNGSTEN");

			// Token: 0x0400DC9B RID: 56475
			public static LocString DESC = string.Concat(new string[]
			{
				"(W) Molten Tungsten is a crystalline ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				" heated into a ",
				UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
				" state."
			});
		}

		// Token: 0x0200391B RID: 14619
		public class MOLTENTUNGSTENDISELENIDE
		{
			// Token: 0x0400DC9C RID: 56476
			public static LocString NAME = UI.FormatAsLink("Tungsten Diselenide", "MOLTENTUNGSTENDISELENIDE");

			// Token: 0x0400DC9D RID: 56477
			public static LocString DESC = string.Concat(new string[]
			{
				"(WSe<sub>2</sub>) Tungsten Diselenide is an inorganic ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				" compound heated into a ",
				UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
				" state."
			});
		}

		// Token: 0x0200391C RID: 14620
		public class MOLTENSTEEL
		{
			// Token: 0x0400DC9E RID: 56478
			public static LocString NAME = UI.FormatAsLink("Molten Steel", "MOLTENSTEEL");

			// Token: 0x0400DC9F RID: 56479
			public static LocString DESC = string.Concat(new string[]
			{
				"Molten Steel is a ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				" alloy of iron and carbon, heated into a hazardous ",
				UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
				" state."
			});
		}

		// Token: 0x0200391D RID: 14621
		public class MOLTENURANIUM
		{
			// Token: 0x0400DCA0 RID: 56480
			public static LocString NAME = UI.FormatAsLink("Liquid Uranium", "MOLTENURANIUM");

			// Token: 0x0400DCA1 RID: 56481
			public static LocString DESC = string.Concat(new string[]
			{
				"(U) Liquid Uranium is a highly ",
				UI.FormatAsLink("Radioactive", "RADIATION"),
				" substance, heated into a hazardous ",
				UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
				" state.\n\nIt is a byproduct of ",
				UI.FormatAsLink("Enriched Uranium", "ENRICHEDURANIUM"),
				"."
			});
		}

		// Token: 0x0200391E RID: 14622
		public class NIOBIUM
		{
			// Token: 0x0400DCA2 RID: 56482
			public static LocString NAME = UI.FormatAsLink("Niobium", "NIOBIUM");

			// Token: 0x0400DCA3 RID: 56483
			public static LocString DESC = "(Nb) Niobium is a rare metal with many practical applications in metallurgy and superconductor " + UI.FormatAsLink("Research", "RESEARCH") + ".";
		}

		// Token: 0x0200391F RID: 14623
		public class NIOBIUMGAS
		{
			// Token: 0x0400DCA4 RID: 56484
			public static LocString NAME = UI.FormatAsLink("Niobium Gas", "NIOBIUMGAS");

			// Token: 0x0400DCA5 RID: 56485
			public static LocString DESC = "(Nb) Niobium Gas is a rare metal.\n\nThis selection is in a " + UI.FormatAsLink("Gaseous", "ELEMENTS_GAS") + " state.";
		}

		// Token: 0x02003920 RID: 14624
		public class NUCLEARWASTE
		{
			// Token: 0x0400DCA6 RID: 56486
			public static LocString NAME = UI.FormatAsLink("Liquid Nuclear Waste", "NUCLEARWASTE");

			// Token: 0x0400DCA7 RID: 56487
			public static LocString DESC = string.Concat(new string[]
			{
				"Highly toxic liquid full of ",
				UI.FormatAsLink("Radioactive Contaminants", "RADIATION"),
				" which emit ",
				UI.FormatAsLink("Radiation", "RADIATION"),
				" that can be absorbed by ",
				UI.FormatAsLink("Radbolt Generators", "HIGHENERGYPARTICLESPAWNER"),
				"."
			});
		}

		// Token: 0x02003921 RID: 14625
		public class OBSIDIAN
		{
			// Token: 0x0400DCA8 RID: 56488
			public static LocString NAME = UI.FormatAsLink("Obsidian", "OBSIDIAN");

			// Token: 0x0400DCA9 RID: 56489
			public static LocString DESC = "Obsidian is a brittle composite of volcanic " + UI.FormatAsLink("Glass", "GLASS") + ".";
		}

		// Token: 0x02003922 RID: 14626
		public class OXYGEN
		{
			// Token: 0x0400DCAA RID: 56490
			public static LocString NAME = UI.FormatAsLink("Oxygen", "OXYGEN");

			// Token: 0x0400DCAB RID: 56491
			public static LocString DESC = "(O<sub>2</sub>) Oxygen is an atomically lightweight and breathable " + UI.FormatAsLink("Gas", "ELEMENTS_GAS") + ", necessary for sustaining life.\n\nIt tends to rise above other gases.";
		}

		// Token: 0x02003923 RID: 14627
		public class OXYROCK
		{
			// Token: 0x0400DCAC RID: 56492
			public static LocString NAME = UI.FormatAsLink("Oxylite", "OXYROCK");

			// Token: 0x0400DCAD RID: 56493
			public static LocString DESC = string.Concat(new string[]
			{
				"(Ir<sub>3</sub>O<sub>2</sub>) Oxylite is a chemical compound that slowly emits breathable ",
				UI.FormatAsLink("Oxygen", "OXYGEN"),
				".\n\nExcavating ",
				ELEMENTS.OXYROCK.NAME,
				" increases its emission rate, but depletes the ore more rapidly."
			});
		}

		// Token: 0x02003924 RID: 14628
		public class PHOSPHATENODULES
		{
			// Token: 0x0400DCAE RID: 56494
			public static LocString NAME = UI.FormatAsLink("Phosphate Nodules", "PHOSPHATENODULES");

			// Token: 0x0400DCAF RID: 56495
			public static LocString DESC = "(PO<sup>3-</sup><sub>4</sub>) Nodules of sedimentary rock containing high concentrations of phosphate.";
		}

		// Token: 0x02003925 RID: 14629
		public class PHOSPHORITE
		{
			// Token: 0x0400DCB0 RID: 56496
			public static LocString NAME = UI.FormatAsLink("Phosphorite", "PHOSPHORITE");

			// Token: 0x0400DCB1 RID: 56497
			public static LocString DESC = "Phosphorite is a composite of sedimentary rock, saturated with phosphate.";
		}

		// Token: 0x02003926 RID: 14630
		public class PHOSPHORUS
		{
			// Token: 0x0400DCB2 RID: 56498
			public static LocString NAME = UI.FormatAsLink("Refined Phosphorus", "PHOSPHORUS");

			// Token: 0x0400DCB3 RID: 56499
			public static LocString DESC = "(P) Refined Phosphorus is a chemical element in its " + UI.FormatAsLink("Solid", "ELEMENTS_SOLID") + " state.";
		}

		// Token: 0x02003927 RID: 14631
		public class PHOSPHORUSGAS
		{
			// Token: 0x0400DCB4 RID: 56500
			public static LocString NAME = UI.FormatAsLink("Phosphorus Gas", "PHOSPHORUSGAS");

			// Token: 0x0400DCB5 RID: 56501
			public static LocString DESC = string.Concat(new string[]
			{
				"(P) Phosphorus Gas is the ",
				UI.FormatAsLink("Gaseous", "ELEMENTS_GAS"),
				" state of ",
				UI.FormatAsLink("Refined Phosphorus", "PHOSPHORUS"),
				"."
			});
		}

		// Token: 0x02003928 RID: 14632
		public class PROPANE
		{
			// Token: 0x0400DCB6 RID: 56502
			public static LocString NAME = UI.FormatAsLink("Propane Gas", "PROPANE");

			// Token: 0x0400DCB7 RID: 56503
			public static LocString DESC = string.Concat(new string[]
			{
				"(C<sub>3</sub>H<sub>8</sub>) Propane Gas is a natural alkane.\n\nThis selection is in a ",
				UI.FormatAsLink("Gaseous", "ELEMENTS_GAS"),
				" state.\n\nIt is useful in ",
				UI.FormatAsLink("Power", "POWER"),
				" production."
			});
		}

		// Token: 0x02003929 RID: 14633
		public class RADIUM
		{
			// Token: 0x0400DCB8 RID: 56504
			public static LocString NAME = UI.FormatAsLink("Radium", "RADIUM");

			// Token: 0x0400DCB9 RID: 56505
			public static LocString DESC = string.Concat(new string[]
			{
				"(Ra) Radium is a ",
				UI.FormatAsLink("Light", "LIGHT"),
				" emitting radioactive substance.\n\nIt is useful as a ",
				UI.FormatAsLink("Power", "POWER"),
				" source."
			});
		}

		// Token: 0x0200392A RID: 14634
		public class YELLOWCAKE
		{
			// Token: 0x0400DCBA RID: 56506
			public static LocString NAME = UI.FormatAsLink("Yellowcake", "YELLOWCAKE");

			// Token: 0x0400DCBB RID: 56507
			public static LocString DESC = string.Concat(new string[]
			{
				"(U<sub>3</sub>O<sub>8</sub>) Yellowcake is a byproduct of ",
				UI.FormatAsLink("Uranium", "URANIUM"),
				" mining.\n\nIt is useful in preparing fuel for ",
				UI.FormatAsLink("Research Reactors", "NUCLEARREACTOR"),
				".\n\nNote: Do not eat."
			});
		}

		// Token: 0x0200392B RID: 14635
		public class ROCKGAS
		{
			// Token: 0x0400DCBC RID: 56508
			public static LocString NAME = UI.FormatAsLink("Rock Gas", "ROCKGAS");

			// Token: 0x0400DCBD RID: 56509
			public static LocString DESC = "Rock Gas is rock that has been superheated into a " + UI.FormatAsLink("Gaseous", "ELEMENTS_GAS") + " state.";
		}

		// Token: 0x0200392C RID: 14636
		public class RUST
		{
			// Token: 0x0400DCBE RID: 56510
			public static LocString NAME = UI.FormatAsLink("Rust", "RUST");

			// Token: 0x0400DCBF RID: 56511
			public static LocString DESC = string.Concat(new string[]
			{
				"Rust is an iron oxide that forms from the breakdown of ",
				UI.FormatAsLink("Iron", "IRON"),
				".\n\nIt is useful in some ",
				UI.FormatAsLink("Oxygen", "OXYGEN"),
				" production processes."
			});
		}

		// Token: 0x0200392D RID: 14637
		public class REGOLITH
		{
			// Token: 0x0400DCC0 RID: 56512
			public static LocString NAME = UI.FormatAsLink("Regolith", "REGOLITH");

			// Token: 0x0400DCC1 RID: 56513
			public static LocString DESC = "Regolith is a sandy substance composed of the various particles that collect atop terrestrial objects.\n\nIt is useful as a " + UI.FormatAsLink("Filtration Medium", "REGOLITH") + ".";
		}

		// Token: 0x0200392E RID: 14638
		public class SALTGAS
		{
			// Token: 0x0400DCC2 RID: 56514
			public static LocString NAME = UI.FormatAsLink("Salt Gas", "SALTGAS");

			// Token: 0x0400DCC3 RID: 56515
			public static LocString DESC = "(NaCl) Salt Gas is an edible chemical compound that has been superheated into a " + UI.FormatAsLink("Gaseous", "ELEMENTS_GAS") + " state.";
		}

		// Token: 0x0200392F RID: 14639
		public class MOLTENSALT
		{
			// Token: 0x0400DCC4 RID: 56516
			public static LocString NAME = UI.FormatAsLink("Molten Salt", "MOLTENSALT");

			// Token: 0x0400DCC5 RID: 56517
			public static LocString DESC = "(NaCl) Molten Salt is an edible chemical compound that has been heated into a " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " state.";
		}

		// Token: 0x02003930 RID: 14640
		public class SALT
		{
			// Token: 0x0400DCC6 RID: 56518
			public static LocString NAME = UI.FormatAsLink("Salt", "SALT");

			// Token: 0x0400DCC7 RID: 56519
			public static LocString DESC = "(NaCl) Salt, also known as sodium chloride, is an edible chemical compound.\n\nWhen refined, it can be eaten with meals to increase Duplicant " + UI.FormatAsLink("Morale", "MORALE") + ".";
		}

		// Token: 0x02003931 RID: 14641
		public class SALTWATER
		{
			// Token: 0x0400DCC8 RID: 56520
			public static LocString NAME = UI.FormatAsLink("Salt Water", "SALTWATER");

			// Token: 0x0400DCC9 RID: 56521
			public static LocString DESC = string.Concat(new string[]
			{
				"Salt Water is a natural, lightly concentrated solution of ",
				UI.FormatAsLink("Salt", "SALT"),
				" dissolved in ",
				UI.FormatAsLink("Water", "WATER"),
				".\n\nIt can be used in desalination processes, separating out usable salt."
			});
		}

		// Token: 0x02003932 RID: 14642
		public class SAND
		{
			// Token: 0x0400DCCA RID: 56522
			public static LocString NAME = UI.FormatAsLink("Sand", "SAND");

			// Token: 0x0400DCCB RID: 56523
			public static LocString DESC = "Sand is a composite of granular rock.\n\nIt is useful as a " + UI.FormatAsLink("Filtration Medium", "FILTER") + ".";
		}

		// Token: 0x02003933 RID: 14643
		public class SANDCEMENT
		{
			// Token: 0x0400DCCC RID: 56524
			public static LocString NAME = UI.FormatAsLink("Sand Cement", "SANDCEMENT");

			// Token: 0x0400DCCD RID: 56525
			public static LocString DESC = "";
		}

		// Token: 0x02003934 RID: 14644
		public class SANDSTONE
		{
			// Token: 0x0400DCCE RID: 56526
			public static LocString NAME = UI.FormatAsLink("Sandstone", "SANDSTONE");

			// Token: 0x0400DCCF RID: 56527
			public static LocString DESC = "Sandstone is a composite of relatively soft sedimentary rock.\n\nIt is useful as a <b>Construction Material</b>.";
		}

		// Token: 0x02003935 RID: 14645
		public class SEDIMENTARYROCK
		{
			// Token: 0x0400DCD0 RID: 56528
			public static LocString NAME = UI.FormatAsLink("Sedimentary Rock", "SEDIMENTARYROCK");

			// Token: 0x0400DCD1 RID: 56529
			public static LocString DESC = "Sedimentary Rock is a hardened composite of sediment layers.\n\nIt is useful as a <b>Construction Material</b>.";
		}

		// Token: 0x02003936 RID: 14646
		public class SLIMEMOLD
		{
			// Token: 0x0400DCD2 RID: 56530
			public static LocString NAME = UI.FormatAsLink("Slime", "SLIMEMOLD");

			// Token: 0x0400DCD3 RID: 56531
			public static LocString DESC = string.Concat(new string[]
			{
				"Slime is a thick biomixture of algae, fungi, and mucopolysaccharides.\n\nIt can be distilled into ",
				UI.FormatAsLink("Algae", "ALGAE"),
				" and emits ",
				ELEMENTS.CONTAMINATEDOXYGEN.NAME,
				" once dug up."
			});
		}

		// Token: 0x02003937 RID: 14647
		public class SNOW
		{
			// Token: 0x0400DCD4 RID: 56532
			public static LocString NAME = UI.FormatAsLink("Snow", "SNOW");

			// Token: 0x0400DCD5 RID: 56533
			public static LocString DESC = "(H<sub>2</sub>O) Snow is a mass of loose, crystalline ice particles.\n\nIt becomes " + UI.FormatAsLink("Water", "WATER") + " when melted.";
		}

		// Token: 0x02003938 RID: 14648
		public class STABLESNOW
		{
			// Token: 0x0400DCD6 RID: 56534
			public static LocString NAME = "Packed " + ELEMENTS.SNOW.NAME;

			// Token: 0x0400DCD7 RID: 56535
			public static LocString DESC = ELEMENTS.SNOW.DESC;
		}

		// Token: 0x02003939 RID: 14649
		public class SOLIDCARBONDIOXIDE
		{
			// Token: 0x0400DCD8 RID: 56536
			public static LocString NAME = UI.FormatAsLink("Solid Carbon Dioxide", "SOLIDCARBONDIOXIDE");

			// Token: 0x0400DCD9 RID: 56537
			public static LocString DESC = "(CO<sub>2</sub>) Carbon Dioxide is an unbreathable compound in a " + UI.FormatAsLink("Solid", "ELEMENTS_SOLID") + " state.";
		}

		// Token: 0x0200393A RID: 14650
		public class SOLIDCHLORINE
		{
			// Token: 0x0400DCDA RID: 56538
			public static LocString NAME = UI.FormatAsLink("Solid Chlorine", "SOLIDCHLORINE");

			// Token: 0x0400DCDB RID: 56539
			public static LocString DESC = string.Concat(new string[]
			{
				"(Cl) Chlorine is a natural ",
				UI.FormatAsLink("Germ", "DISEASE"),
				"-killing element in a ",
				UI.FormatAsLink("Solid", "ELEMENTS_SOLID"),
				" state."
			});
		}

		// Token: 0x0200393B RID: 14651
		public class SOLIDCRUDEOIL
		{
			// Token: 0x0400DCDC RID: 56540
			public static LocString NAME = UI.FormatAsLink("Solid Crude Oil", "SOLIDCRUDEOIL");

			// Token: 0x0400DCDD RID: 56541
			public static LocString DESC = "";
		}

		// Token: 0x0200393C RID: 14652
		public class SOLIDHYDROGEN
		{
			// Token: 0x0400DCDE RID: 56542
			public static LocString NAME = UI.FormatAsLink("Solid Hydrogen", "SOLIDHYDROGEN");

			// Token: 0x0400DCDF RID: 56543
			public static LocString DESC = "(H) Solid Hydrogen is the universe's most common element in a " + UI.FormatAsLink("Solid", "ELEMENTS_SOLID") + " state.";
		}

		// Token: 0x0200393D RID: 14653
		public class SOLIDMERCURY
		{
			// Token: 0x0400DCE0 RID: 56544
			public static LocString NAME = UI.FormatAsLink("Mercury", "SOLIDMERCURY");

			// Token: 0x0400DCE1 RID: 56545
			public static LocString DESC = string.Concat(new string[]
			{
				"(Hg) Mercury is a rare ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				" in a ",
				UI.FormatAsLink("Solid", "ELEMENTS_SOLID"),
				" state."
			});
		}

		// Token: 0x0200393E RID: 14654
		public class SOLIDOXYGEN
		{
			// Token: 0x0400DCE2 RID: 56546
			public static LocString NAME = UI.FormatAsLink("Solid Oxygen", "SOLIDOXYGEN");

			// Token: 0x0400DCE3 RID: 56547
			public static LocString DESC = "(O<sub>2</sub>) Solid Oxygen is a breathable element in a " + UI.FormatAsLink("Solid", "ELEMENTS_SOLID") + " state.";
		}

		// Token: 0x0200393F RID: 14655
		public class SOLIDMETHANE
		{
			// Token: 0x0400DCE4 RID: 56548
			public static LocString NAME = UI.FormatAsLink("Solid Methane", "SOLIDMETHANE");

			// Token: 0x0400DCE5 RID: 56549
			public static LocString DESC = "(CH<sub>4</sub>) Methane is an alkane in a " + UI.FormatAsLink("Solid", "ELEMENTS_SOLID") + " state.";
		}

		// Token: 0x02003940 RID: 14656
		public class SOLIDNAPHTHA
		{
			// Token: 0x0400DCE6 RID: 56550
			public static LocString NAME = UI.FormatAsLink("Solid Naphtha", "SOLIDNAPHTHA");

			// Token: 0x0400DCE7 RID: 56551
			public static LocString DESC = "Naphtha is a distilled hydrocarbon mixture in a " + UI.FormatAsLink("Solid", "ELEMENTS_SOLID") + " state.";
		}

		// Token: 0x02003941 RID: 14657
		public class CORIUM
		{
			// Token: 0x0400DCE8 RID: 56552
			public static LocString NAME = UI.FormatAsLink("Corium", "CORIUM");

			// Token: 0x0400DCE9 RID: 56553
			public static LocString DESC = "A radioactive mixture of nuclear waste and melted reactor materials.\n\nReleases " + UI.FormatAsLink("Nuclear Fallout", "FALLOUT") + " gas.";
		}

		// Token: 0x02003942 RID: 14658
		public class SOLIDPETROLEUM
		{
			// Token: 0x0400DCEA RID: 56554
			public static LocString NAME = UI.FormatAsLink("Solid Petroleum", "SOLIDPETROLEUM");

			// Token: 0x0400DCEB RID: 56555
			public static LocString DESC = string.Concat(new string[]
			{
				"Petroleum is a ",
				UI.FormatAsLink("Power", "POWER"),
				" source.\n\nThis selection is in a ",
				UI.FormatAsLink("Solid", "ELEMENTS_SOLID"),
				" state."
			});
		}

		// Token: 0x02003943 RID: 14659
		public class SOLIDPROPANE
		{
			// Token: 0x0400DCEC RID: 56556
			public static LocString NAME = UI.FormatAsLink("Solid Propane", "SOLIDPROPANE");

			// Token: 0x0400DCED RID: 56557
			public static LocString DESC = "(C<sub>3</sub>H<sub>8</sub>) Solid Propane is a natural gas in a " + UI.FormatAsLink("Solid", "ELEMENTS_SOLID") + " state.";
		}

		// Token: 0x02003944 RID: 14660
		public class SOLIDSUPERCOOLANT
		{
			// Token: 0x0400DCEE RID: 56558
			public static LocString NAME = UI.FormatAsLink("Solid Super Coolant", "SOLIDSUPERCOOLANT");

			// Token: 0x0400DCEF RID: 56559
			public static LocString DESC = string.Concat(new string[]
			{
				"Super Coolant is an industrial-grade ",
				UI.FormatAsLink("Fullerene", "FULLERENE"),
				" coolant.\n\nThis selection is in a ",
				UI.FormatAsLink("Solid", "ELEMENTS_SOLID"),
				" state."
			});
		}

		// Token: 0x02003945 RID: 14661
		public class SOLIDVISCOGEL
		{
			// Token: 0x0400DCF0 RID: 56560
			public static LocString NAME = UI.FormatAsLink("Solid Visco-Gel", "SOLIDVISCOGEL");

			// Token: 0x0400DCF1 RID: 56561
			public static LocString DESC = string.Concat(new string[]
			{
				"Visco-Gel is a polymer that has high surface tension when in ",
				UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
				" form.\n\nThis selection is in a ",
				UI.FormatAsLink("Solid", "ELEMENTS_SOLID"),
				" state."
			});
		}

		// Token: 0x02003946 RID: 14662
		public class SYNGAS
		{
			// Token: 0x0400DCF2 RID: 56562
			public static LocString NAME = UI.FormatAsLink("Synthesis Gas", "SYNGAS");

			// Token: 0x0400DCF3 RID: 56563
			public static LocString DESC = "Synthesis Gas is an artificial, unbreathable " + UI.FormatAsLink("Gas", "ELEMENTS_GAS") + ".\n\nIt can be converted into an efficient fuel.";
		}

		// Token: 0x02003947 RID: 14663
		public class MOLTENSYNGAS
		{
			// Token: 0x0400DCF4 RID: 56564
			public static LocString NAME = UI.FormatAsLink("Molten Synthesis Gas", "SYNGAS");

			// Token: 0x0400DCF5 RID: 56565
			public static LocString DESC = "Molten Synthesis Gas is an artificial, unbreathable " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + ".\n\nIt can be converted into an efficient fuel.";
		}

		// Token: 0x02003948 RID: 14664
		public class SOLIDSYNGAS
		{
			// Token: 0x0400DCF6 RID: 56566
			public static LocString NAME = UI.FormatAsLink("Solid Synthesis Gas", "SYNGAS");

			// Token: 0x0400DCF7 RID: 56567
			public static LocString DESC = "Solid Synthesis Gas is an artificial, unbreathable " + UI.FormatAsLink("Solid", "ELEMENTS_SOLID") + ".\n\nIt can be converted into an efficient fuel.";
		}

		// Token: 0x02003949 RID: 14665
		public class STEAM
		{
			// Token: 0x0400DCF8 RID: 56568
			public static LocString NAME = UI.FormatAsLink("Steam", "STEAM");

			// Token: 0x0400DCF9 RID: 56569
			public static LocString DESC = string.Concat(new string[]
			{
				"(H<sub>2</sub>O) Steam is ",
				ELEMENTS.WATER.NAME,
				" that has been heated into a scalding ",
				UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
				"."
			});
		}

		// Token: 0x0200394A RID: 14666
		public class STEEL
		{
			// Token: 0x0400DCFA RID: 56570
			public static LocString NAME = UI.FormatAsLink("Steel", "STEEL");

			// Token: 0x0400DCFB RID: 56571
			public static LocString DESC = "Steel is a " + UI.FormatAsLink("Metal Alloy", "REFINEDMETAL") + " composed of iron and carbon.";
		}

		// Token: 0x0200394B RID: 14667
		public class STEELGAS
		{
			// Token: 0x0400DCFC RID: 56572
			public static LocString NAME = UI.FormatAsLink("Steel Gas", "STEELGAS");

			// Token: 0x0400DCFD RID: 56573
			public static LocString DESC = string.Concat(new string[]
			{
				"Steel Gas is a superheated ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				" ",
				UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
				" composed of iron and carbon."
			});
		}

		// Token: 0x0200394C RID: 14668
		public class SUGARWATER
		{
			// Token: 0x0400DCFE RID: 56574
			public static LocString NAME = UI.FormatAsLink("Nectar", "SUGARWATER");

			// Token: 0x0400DCFF RID: 56575
			public static LocString DESC = string.Concat(new string[]
			{
				"Nectar is a natural, lightly concentrated solution of ",
				UI.FormatAsLink("Sucrose", "SUCROSE"),
				" dissolved in ",
				UI.FormatAsLink("Water", "WATER"),
				"."
			});
		}

		// Token: 0x0200394D RID: 14669
		public class SULFUR
		{
			// Token: 0x0400DD00 RID: 56576
			public static LocString NAME = UI.FormatAsLink("Sulfur", "SULFUR");

			// Token: 0x0400DD01 RID: 56577
			public static LocString DESC = string.Concat(new string[]
			{
				"(S) Sulfur is a common chemical element and byproduct of ",
				UI.FormatAsLink("Natural Gas", "METHANE"),
				" production.\n\nThis selection is in a ",
				UI.FormatAsLink("Solid", "ELEMENTS_SOLID"),
				" state."
			});
		}

		// Token: 0x0200394E RID: 14670
		public class SULFURGAS
		{
			// Token: 0x0400DD02 RID: 56578
			public static LocString NAME = UI.FormatAsLink("Sulfur Gas", "SULFURGAS");

			// Token: 0x0400DD03 RID: 56579
			public static LocString DESC = string.Concat(new string[]
			{
				"(S) Sulfur is a common chemical element and byproduct of ",
				UI.FormatAsLink("Natural Gas", "METHANE"),
				" production.\n\nThis selection is in a ",
				UI.FormatAsLink("Gaseous", "ELEMENTS_GAS"),
				" state."
			});
		}

		// Token: 0x0200394F RID: 14671
		public class SUPERCOOLANT
		{
			// Token: 0x0400DD04 RID: 56580
			public static LocString NAME = UI.FormatAsLink("Super Coolant", "SUPERCOOLANT");

			// Token: 0x0400DD05 RID: 56581
			public static LocString DESC = string.Concat(new string[]
			{
				"Super Coolant is an industrial-grade coolant that utilizes the unusual energy states of ",
				UI.FormatAsLink("Fullerene", "FULLERENE"),
				".\n\nThis selection is in a ",
				UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
				" state."
			});
		}

		// Token: 0x02003950 RID: 14672
		public class SUPERCOOLANTGAS
		{
			// Token: 0x0400DD06 RID: 56582
			public static LocString NAME = UI.FormatAsLink("Super Coolant Gas", "SUPERCOOLANTGAS");

			// Token: 0x0400DD07 RID: 56583
			public static LocString DESC = string.Concat(new string[]
			{
				"Super Coolant is an industrial-grade ",
				UI.FormatAsLink("Fullerene", "FULLERENE"),
				" coolant.\n\nThis selection is in a ",
				UI.FormatAsLink("Gaseous", "ELEMENTS_GAS"),
				" state."
			});
		}

		// Token: 0x02003951 RID: 14673
		public class SUPERINSULATOR
		{
			// Token: 0x0400DD08 RID: 56584
			public static LocString NAME = UI.FormatAsLink("Insulite", "SUPERINSULATOR");

			// Token: 0x0400DD09 RID: 56585
			public static LocString DESC = string.Concat(new string[]
			{
				"Insulite reduces ",
				UI.FormatAsLink("Heat Transfer", "HEAT"),
				" and is composed of recrystallized ",
				UI.FormatAsLink("Abyssalite", "KATAIRITE"),
				"."
			});
		}

		// Token: 0x02003952 RID: 14674
		public class TEMPCONDUCTORSOLID
		{
			// Token: 0x0400DD0A RID: 56586
			public static LocString NAME = UI.FormatAsLink("Thermium", "TEMPCONDUCTORSOLID");

			// Token: 0x0400DD0B RID: 56587
			public static LocString DESC = "Thermium is an industrial metal alloy formulated to maximize " + UI.FormatAsLink("Heat Transfer", "HEAT") + " and thermal dispersion.";
		}

		// Token: 0x02003953 RID: 14675
		public class TUNGSTEN
		{
			// Token: 0x0400DD0C RID: 56588
			public static LocString NAME = UI.FormatAsLink("Tungsten", "TUNGSTEN");

			// Token: 0x0400DD0D RID: 56589
			public static LocString DESC = string.Concat(new string[]
			{
				"(W) Tungsten is an extremely tough crystalline ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				".\n\nIt is suitable for building ",
				UI.FormatAsLink("Power", "POWER"),
				" systems."
			});
		}

		// Token: 0x02003954 RID: 14676
		public class TUNGSTENGAS
		{
			// Token: 0x0400DD0E RID: 56590
			public static LocString NAME = UI.FormatAsLink("Tungsten Gas", "TUNGSTENGAS");

			// Token: 0x0400DD0F RID: 56591
			public static LocString DESC = string.Concat(new string[]
			{
				"(W) Tungsten is a superheated crystalline ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				".\n\nThis selection is in a ",
				UI.FormatAsLink("Gaseous", "ELEMENTS_GAS"),
				" state."
			});
		}

		// Token: 0x02003955 RID: 14677
		public class TUNGSTENDISELENIDE
		{
			// Token: 0x0400DD10 RID: 56592
			public static LocString NAME = UI.FormatAsLink("Tungsten Diselenide", "TUNGSTENDISELENIDE");

			// Token: 0x0400DD11 RID: 56593
			public static LocString DESC = string.Concat(new string[]
			{
				"(WSe<sub>2</sub>) Tungsten Diselenide is an inorganic ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				" compound with a crystalline structure.\n\nIt is suitable for building ",
				UI.FormatAsLink("Power", "POWER"),
				" systems."
			});
		}

		// Token: 0x02003956 RID: 14678
		public class TUNGSTENDISELENIDEGAS
		{
			// Token: 0x0400DD12 RID: 56594
			public static LocString NAME = UI.FormatAsLink("Tungsten Diselenide Gas", "TUNGSTENDISELENIDEGAS");

			// Token: 0x0400DD13 RID: 56595
			public static LocString DESC = string.Concat(new string[]
			{
				"(WSe<sub>2</sub>) Tungsten Diselenide Gasis a superheated ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				" compound in a ",
				UI.FormatAsLink("Gaseous", "ELEMENTS_GAS"),
				" state."
			});
		}

		// Token: 0x02003957 RID: 14679
		public class TOXICSAND
		{
			// Token: 0x0400DD14 RID: 56596
			public static LocString NAME = UI.FormatAsLink("Polluted Dirt", "TOXICSAND");

			// Token: 0x0400DD15 RID: 56597
			public static LocString DESC = "Polluted Dirt is unprocessed biological waste.\n\nIt emits " + UI.FormatAsLink("Polluted Oxygen", "CONTAMINATEDOXYGEN") + " over time.";
		}

		// Token: 0x02003958 RID: 14680
		public class UNOBTANIUM
		{
			// Token: 0x0400DD16 RID: 56598
			public static LocString NAME = UI.FormatAsLink("Neutronium", "UNOBTANIUM");

			// Token: 0x0400DD17 RID: 56599
			public static LocString DESC = "(Nt) Neutronium is a mysterious and extremely resilient element.\n\nIt cannot be excavated by any Duplicant mining tool.";
		}

		// Token: 0x02003959 RID: 14681
		public class URANIUMORE
		{
			// Token: 0x0400DD18 RID: 56600
			public static LocString NAME = UI.FormatAsLink("Uranium Ore", "URANIUMORE");

			// Token: 0x0400DD19 RID: 56601
			public static LocString DESC = "(U) Uranium Ore is a highly " + UI.FormatAsLink("Radioactive", "RADIATION") + " substance.\n\nIt can be refined into fuel for research reactors.";
		}

		// Token: 0x0200395A RID: 14682
		public class VACUUM
		{
			// Token: 0x0400DD1A RID: 56602
			public static LocString NAME = UI.FormatAsLink("Vacuum", "VACUUM");

			// Token: 0x0400DD1B RID: 56603
			public static LocString DESC = "A vacuum is a space devoid of all matter.";
		}

		// Token: 0x0200395B RID: 14683
		public class VISCOGEL
		{
			// Token: 0x0400DD1C RID: 56604
			public static LocString NAME = UI.FormatAsLink("Visco-Gel Fluid", "VISCOGEL");

			// Token: 0x0400DD1D RID: 56605
			public static LocString DESC = "Visco-Gel Fluid is a " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " polymer with high surface tension, preventing typical liquid flow and allowing for unusual configurations.";
		}

		// Token: 0x0200395C RID: 14684
		public class VOID
		{
			// Token: 0x0400DD1E RID: 56606
			public static LocString NAME = UI.FormatAsLink("Void", "VOID");

			// Token: 0x0400DD1F RID: 56607
			public static LocString DESC = "Cold, infinite nothingness.";
		}

		// Token: 0x0200395D RID: 14685
		public class COMPOSITION
		{
			// Token: 0x0400DD20 RID: 56608
			public static LocString NAME = UI.FormatAsLink("Composition", "COMPOSITION");

			// Token: 0x0400DD21 RID: 56609
			public static LocString DESC = "A mixture of two or more elements.";
		}

		// Token: 0x0200395E RID: 14686
		public class WATER
		{
			// Token: 0x0400DD22 RID: 56610
			public static LocString NAME = UI.FormatAsLink("Water", "WATER");

			// Token: 0x0400DD23 RID: 56611
			public static LocString DESC = "(H<sub>2</sub>O) Clean " + UI.FormatAsLink("Water", "WATER") + ", suitable for consumption.";
		}

		// Token: 0x0200395F RID: 14687
		public class WOLFRAMITE
		{
			// Token: 0x0400DD24 RID: 56612
			public static LocString NAME = UI.FormatAsLink("Wolframite", "WOLFRAMITE");

			// Token: 0x0400DD25 RID: 56613
			public static LocString DESC = string.Concat(new string[]
			{
				"((Fe,Mn)WO<sub>4</sub>) Wolframite is a dense Metallic element in a ",
				UI.FormatAsLink("Solid", "ELEMENTS_SOLID"),
				" state.\n\nIt is a source of ",
				UI.FormatAsLink("Tungsten", "TUNGSTEN"),
				" and is suitable for building ",
				UI.FormatAsLink("Power", "POWER"),
				" systems."
			});
		}

		// Token: 0x02003960 RID: 14688
		public class TESTELEMENT
		{
			// Token: 0x0400DD26 RID: 56614
			public static LocString NAME = UI.FormatAsLink("Test Element", "TESTELEMENT");

			// Token: 0x0400DD27 RID: 56615
			public static LocString DESC = string.Concat(new string[]
			{
				"((Fe,Mn)WO<sub>4</sub>) Wolframite is a dense Metallic element in a ",
				UI.FormatAsLink("Solid", "ELEMENTS_SOLID"),
				" state.\n\nIt is a source of ",
				UI.FormatAsLink("Tungsten", "TUNGSTEN"),
				" and is suitable for building ",
				UI.FormatAsLink("Power", "POWER"),
				" systems."
			});
		}

		// Token: 0x02003961 RID: 14689
		public class POLYPROPYLENE
		{
			// Token: 0x0400DD28 RID: 56616
			public static LocString NAME = UI.FormatAsLink("Plastic", "POLYPROPYLENE");

			// Token: 0x0400DD29 RID: 56617
			public static LocString DESC = "(C<sub>3</sub>H<sub>6</sub>)<sub>n</sub> " + ELEMENTS.POLYPROPYLENE.NAME + " is a thermoplastic polymer.\n\nIt is useful for constructing a variety of advanced buildings and equipment.";

			// Token: 0x0400DD2A RID: 56618
			public static LocString BUILD_DESC = "Buildings made of this " + ELEMENTS.POLYPROPYLENE.NAME + " have antiseptic properties";
		}

		// Token: 0x02003962 RID: 14690
		public class HARDPOLYPROPYLENE
		{
			// Token: 0x0400DD2B RID: 56619
			public static LocString NAME = UI.FormatAsLink("Plastium", "HARDPOLYPROPYLENE");

			// Token: 0x0400DD2C RID: 56620
			public static LocString DESC = string.Concat(new string[]
			{
				ELEMENTS.HARDPOLYPROPYLENE.NAME,
				" is an advanced thermoplastic polymer made from ",
				UI.FormatAsLink("Thermium", "TEMPCONDUCTORSOLID"),
				", ",
				UI.FormatAsLink("Plastic", "POLYPROPYLENE"),
				" and ",
				UI.FormatAsLink("Brackwax", "MILKFAT"),
				".\n\nIt is highly heat-resistant and suitable for use in space buildings."
			});
		}

		// Token: 0x02003963 RID: 14691
		public class NAPHTHA
		{
			// Token: 0x0400DD2D RID: 56621
			public static LocString NAME = UI.FormatAsLink("Liquid Naphtha", "NAPHTHA");

			// Token: 0x0400DD2E RID: 56622
			public static LocString DESC = "Naphtha a distilled hydrocarbon mixture produced from the burning of " + UI.FormatAsLink("Plastic", "POLYPROPYLENE") + ".";
		}

		// Token: 0x02003964 RID: 14692
		public class SLABS
		{
			// Token: 0x0400DD2F RID: 56623
			public static LocString NAME = UI.FormatAsLink("Building Slab", "SLABS");

			// Token: 0x0400DD30 RID: 56624
			public static LocString DESC = "Slabs are a refined mineral building block used for assembling advanced buildings.";
		}

		// Token: 0x02003965 RID: 14693
		public class TOXICMUD
		{
			// Token: 0x0400DD31 RID: 56625
			public static LocString NAME = UI.FormatAsLink("Polluted Mud", "TOXICMUD");

			// Token: 0x0400DD32 RID: 56626
			public static LocString DESC = string.Concat(new string[]
			{
				"A mixture of ",
				UI.FormatAsLink("Polluted Dirt", "TOXICSAND"),
				" and ",
				UI.FormatAsLink("Polluted Water", "DIRTYWATER"),
				".\n\nCan be separated into its base elements using a ",
				UI.FormatAsLink("Sludge Press", "SLUDGEPRESS"),
				"."
			});
		}

		// Token: 0x02003966 RID: 14694
		public class MUD
		{
			// Token: 0x0400DD33 RID: 56627
			public static LocString NAME = UI.FormatAsLink("Mud", "MUD");

			// Token: 0x0400DD34 RID: 56628
			public static LocString DESC = string.Concat(new string[]
			{
				"A mixture of ",
				UI.FormatAsLink("Dirt", "DIRT"),
				" and ",
				UI.FormatAsLink("Water", "WATER"),
				".\n\nCan be separated into its base elements using a ",
				UI.FormatAsLink("Sludge Press", "SLUDGEPRESS"),
				"."
			});
		}

		// Token: 0x02003967 RID: 14695
		public class SUCROSE
		{
			// Token: 0x0400DD35 RID: 56629
			public static LocString NAME = UI.FormatAsLink("Sucrose", "SUCROSE");

			// Token: 0x0400DD36 RID: 56630
			public static LocString DESC = "(C<sub>12</sub>H<sub>22</sub>O<sub>11</sub>) Sucrose is the raw form of sugar.\n\nIt can be used for cooking higher-quality " + UI.FormatAsLink("Food", "FOOD") + ".";
		}

		// Token: 0x02003968 RID: 14696
		public class MOLTENSUCROSE
		{
			// Token: 0x0400DD37 RID: 56631
			public static LocString NAME = UI.FormatAsLink("Liquid Sucrose", "MOLTENSUCROSE");

			// Token: 0x0400DD38 RID: 56632
			public static LocString DESC = "(C<sub>12</sub>H<sub>22</sub>O<sub>11</sub>) Liquid Sucrose is the raw form of sugar, heated into a " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " state.";
		}
	}
}
