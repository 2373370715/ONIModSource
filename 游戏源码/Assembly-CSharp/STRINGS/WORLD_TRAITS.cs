using System;

namespace STRINGS
{
	// Token: 0x02003ABC RID: 15036
	public static class WORLD_TRAITS
	{
		// Token: 0x0400E282 RID: 57986
		public static LocString MISSING_TRAIT = "<missing traits>";

		// Token: 0x02003ABD RID: 15037
		public static class NO_TRAITS
		{
			// Token: 0x0400E283 RID: 57987
			public static LocString NAME = "<i>This world is stable and has no unusual features.</i>";

			// Token: 0x0400E284 RID: 57988
			public static LocString NAME_SHORTHAND = "No unusual features";

			// Token: 0x0400E285 RID: 57989
			public static LocString DESCRIPTION = "This world exists in a particularly stable configuration each time it is encountered";
		}

		// Token: 0x02003ABE RID: 15038
		public static class BOULDERS_LARGE
		{
			// Token: 0x0400E286 RID: 57990
			public static LocString NAME = "Large Boulders";

			// Token: 0x0400E287 RID: 57991
			public static LocString DESCRIPTION = "Huge boulders make digging through this world more difficult";
		}

		// Token: 0x02003ABF RID: 15039
		public static class BOULDERS_MEDIUM
		{
			// Token: 0x0400E288 RID: 57992
			public static LocString NAME = "Medium Boulders";

			// Token: 0x0400E289 RID: 57993
			public static LocString DESCRIPTION = "Mid-sized boulders make digging through this world more difficult";
		}

		// Token: 0x02003AC0 RID: 15040
		public static class BOULDERS_MIXED
		{
			// Token: 0x0400E28A RID: 57994
			public static LocString NAME = "Mixed Boulders";

			// Token: 0x0400E28B RID: 57995
			public static LocString DESCRIPTION = "Boulders of various sizes make digging through this world more difficult";
		}

		// Token: 0x02003AC1 RID: 15041
		public static class BOULDERS_SMALL
		{
			// Token: 0x0400E28C RID: 57996
			public static LocString NAME = "Small Boulders";

			// Token: 0x0400E28D RID: 57997
			public static LocString DESCRIPTION = "Tiny boulders make digging through this world more difficult";
		}

		// Token: 0x02003AC2 RID: 15042
		public static class DEEP_OIL
		{
			// Token: 0x0400E28E RID: 57998
			public static LocString NAME = "Trapped Oil";

			// Token: 0x0400E28F RID: 57999
			public static LocString DESCRIPTION = string.Concat(new string[]
			{
				"Most of the ",
				UI.PRE_KEYWORD,
				"Oil",
				UI.PST_KEYWORD,
				" in this world will need to be extracted with ",
				BUILDINGS.PREFABS.OILWELLCAP.NAME,
				"s"
			});
		}

		// Token: 0x02003AC3 RID: 15043
		public static class FROZEN_CORE
		{
			// Token: 0x0400E290 RID: 58000
			public static LocString NAME = "Frozen Core";

			// Token: 0x0400E291 RID: 58001
			public static LocString DESCRIPTION = "This world has a chilly core of solid " + ELEMENTS.ICE.NAME;
		}

		// Token: 0x02003AC4 RID: 15044
		public static class GEOACTIVE
		{
			// Token: 0x0400E292 RID: 58002
			public static LocString NAME = "Geoactive";

			// Token: 0x0400E293 RID: 58003
			public static LocString DESCRIPTION = string.Concat(new string[]
			{
				"This world has more ",
				UI.PRE_KEYWORD,
				"Geysers",
				UI.PST_KEYWORD,
				" and ",
				UI.PRE_KEYWORD,
				"Vents",
				UI.PST_KEYWORD,
				" than usual"
			});
		}

		// Token: 0x02003AC5 RID: 15045
		public static class GEODES
		{
			// Token: 0x0400E294 RID: 58004
			public static LocString NAME = "Geodes";

			// Token: 0x0400E295 RID: 58005
			public static LocString DESCRIPTION = "Large geodes containing rare material caches are deposited across this world";
		}

		// Token: 0x02003AC6 RID: 15046
		public static class GEODORMANT
		{
			// Token: 0x0400E296 RID: 58006
			public static LocString NAME = "Geodormant";

			// Token: 0x0400E297 RID: 58007
			public static LocString DESCRIPTION = string.Concat(new string[]
			{
				"This world has fewer ",
				UI.PRE_KEYWORD,
				"Geysers",
				UI.PST_KEYWORD,
				" and ",
				UI.PRE_KEYWORD,
				"Vents",
				UI.PST_KEYWORD,
				" than usual"
			});
		}

		// Token: 0x02003AC7 RID: 15047
		public static class GLACIERS_LARGE
		{
			// Token: 0x0400E298 RID: 58008
			public static LocString NAME = "Large Glaciers";

			// Token: 0x0400E299 RID: 58009
			public static LocString DESCRIPTION = "Huge chunks of primordial " + ELEMENTS.ICE.NAME + " are scattered across this world";
		}

		// Token: 0x02003AC8 RID: 15048
		public static class IRREGULAR_OIL
		{
			// Token: 0x0400E29A RID: 58010
			public static LocString NAME = "Irregular Oil";

			// Token: 0x0400E29B RID: 58011
			public static LocString DESCRIPTION = string.Concat(new string[]
			{
				"The ",
				UI.PRE_KEYWORD,
				"Oil",
				UI.PST_KEYWORD,
				" on this asteroid is anything but regular!"
			});
		}

		// Token: 0x02003AC9 RID: 15049
		public static class MAGMA_VENTS
		{
			// Token: 0x0400E29C RID: 58012
			public static LocString NAME = "Magma Channels";

			// Token: 0x0400E29D RID: 58013
			public static LocString DESCRIPTION = "The " + ELEMENTS.MAGMA.NAME + " from this world's core has leaked into the mantle and crust";
		}

		// Token: 0x02003ACA RID: 15050
		public static class METAL_POOR
		{
			// Token: 0x0400E29E RID: 58014
			public static LocString NAME = "Metal Poor";

			// Token: 0x0400E29F RID: 58015
			public static LocString DESCRIPTION = string.Concat(new string[]
			{
				"There is a reduced amount of ",
				UI.PRE_KEYWORD,
				"Metal Ore",
				UI.PST_KEYWORD,
				" on this world, proceed with caution!"
			});
		}

		// Token: 0x02003ACB RID: 15051
		public static class METAL_RICH
		{
			// Token: 0x0400E2A0 RID: 58016
			public static LocString NAME = "Metal Rich";

			// Token: 0x0400E2A1 RID: 58017
			public static LocString DESCRIPTION = "This asteroid is an abundant source of " + UI.PRE_KEYWORD + "Metal Ore" + UI.PST_KEYWORD;
		}

		// Token: 0x02003ACC RID: 15052
		public static class MISALIGNED_START
		{
			// Token: 0x0400E2A2 RID: 58018
			public static LocString NAME = "Alternate Pod Location";

			// Token: 0x0400E2A3 RID: 58019
			public static LocString DESCRIPTION = "The " + BUILDINGS.PREFABS.HEADQUARTERSCOMPLETE.NAME + " didn't end up in the asteroid's exact center this time... but it's still nowhere near the surface";
		}

		// Token: 0x02003ACD RID: 15053
		public static class SLIME_SPLATS
		{
			// Token: 0x0400E2A4 RID: 58020
			public static LocString NAME = "Slime Molds";

			// Token: 0x0400E2A5 RID: 58021
			public static LocString DESCRIPTION = "Sickly " + ELEMENTS.SLIMEMOLD.NAME + " growths have crept all over this world";
		}

		// Token: 0x02003ACE RID: 15054
		public static class SUBSURFACE_OCEAN
		{
			// Token: 0x0400E2A6 RID: 58022
			public static LocString NAME = "Subsurface Ocean";

			// Token: 0x0400E2A7 RID: 58023
			public static LocString DESCRIPTION = "Below the crust of this world is a " + ELEMENTS.SALTWATER.NAME + " sea";
		}

		// Token: 0x02003ACF RID: 15055
		public static class VOLCANOES
		{
			// Token: 0x0400E2A8 RID: 58024
			public static LocString NAME = "Volcanic Activity";

			// Token: 0x0400E2A9 RID: 58025
			public static LocString DESCRIPTION = string.Concat(new string[]
			{
				"Several active ",
				UI.PRE_KEYWORD,
				"Volcanoes",
				UI.PST_KEYWORD,
				" have been detected in this world"
			});
		}

		// Token: 0x02003AD0 RID: 15056
		public static class RADIOACTIVE_CRUST
		{
			// Token: 0x0400E2AA RID: 58026
			public static LocString NAME = "Radioactive Crust";

			// Token: 0x0400E2AB RID: 58027
			public static LocString DESCRIPTION = "Deposits of " + ELEMENTS.URANIUMORE.NAME + " are found in this world's crust";
		}

		// Token: 0x02003AD1 RID: 15057
		public static class LUSH_CORE
		{
			// Token: 0x0400E2AC RID: 58028
			public static LocString NAME = "Lush Core";

			// Token: 0x0400E2AD RID: 58029
			public static LocString DESCRIPTION = "This world has a lush forest core";
		}

		// Token: 0x02003AD2 RID: 15058
		public static class METAL_CAVES
		{
			// Token: 0x0400E2AE RID: 58030
			public static LocString NAME = "Metallic Caves";

			// Token: 0x0400E2AF RID: 58031
			public static LocString DESCRIPTION = "This world has caves of metal ore";
		}

		// Token: 0x02003AD3 RID: 15059
		public static class DISTRESS_SIGNAL
		{
			// Token: 0x0400E2B0 RID: 58032
			public static LocString NAME = "Frozen Friend";

			// Token: 0x0400E2B1 RID: 58033
			public static LocString DESCRIPTION = "This world contains a frozen friend from a long time ago";
		}

		// Token: 0x02003AD4 RID: 15060
		public static class CRASHED_SATELLITES
		{
			// Token: 0x0400E2B2 RID: 58034
			public static LocString NAME = "Crashed Satellites";

			// Token: 0x0400E2B3 RID: 58035
			public static LocString DESCRIPTION = "This world contains crashed radioactive satellites";
		}
	}
}
