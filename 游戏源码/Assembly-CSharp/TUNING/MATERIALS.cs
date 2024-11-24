using System;
using System.Linq;
using STRINGS;

namespace TUNING
{
	// Token: 0x020021F1 RID: 8689
	public class MATERIALS
	{
		// Token: 0x0600B848 RID: 47176 RVA: 0x00465658 File Offset: 0x00463858
		public static string GetMaterialString(string materialCategory)
		{
			string[] array = materialCategory.Split('&', StringSplitOptions.None);
			string result;
			if (array.Length == 1)
			{
				result = UI.FormatAsLink(Strings.Get("STRINGS.MISC.TAGS." + materialCategory.ToUpper()), materialCategory);
			}
			else
			{
				result = string.Join(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.PREPARED_SEPARATOR, from s in array
				select UI.FormatAsLink(Strings.Get("STRINGS.MISC.TAGS." + s.ToUpper()), s));
			}
			return result;
		}

		// Token: 0x040096AC RID: 38572
		public const string METAL = "Metal";

		// Token: 0x040096AD RID: 38573
		public const string REFINED_METAL = "RefinedMetal";

		// Token: 0x040096AE RID: 38574
		public const string GLASS = "Glass";

		// Token: 0x040096AF RID: 38575
		public const string TRANSPARENT = "Transparent";

		// Token: 0x040096B0 RID: 38576
		public const string PLASTIC = "Plastic";

		// Token: 0x040096B1 RID: 38577
		public const string BUILDABLERAW = "BuildableRaw";

		// Token: 0x040096B2 RID: 38578
		public const string PRECIOUSROCK = "PreciousRock";

		// Token: 0x040096B3 RID: 38579
		public const string WOOD = "BuildingWood";

		// Token: 0x040096B4 RID: 38580
		public const string BUILDINGFIBER = "BuildingFiber";

		// Token: 0x040096B5 RID: 38581
		public const string LEAD = "Lead";

		// Token: 0x040096B6 RID: 38582
		public const string INSULATOR = "Insulator";

		// Token: 0x040096B7 RID: 38583
		public static readonly string[] ALL_METALS = new string[]
		{
			"Metal"
		};

		// Token: 0x040096B8 RID: 38584
		public static readonly string[] RAW_METALS = new string[]
		{
			"Metal"
		};

		// Token: 0x040096B9 RID: 38585
		public static readonly string[] REFINED_METALS = new string[]
		{
			"RefinedMetal"
		};

		// Token: 0x040096BA RID: 38586
		public static readonly string[] ALLOYS = new string[]
		{
			"Alloy"
		};

		// Token: 0x040096BB RID: 38587
		public static readonly string[] ALL_MINERALS = new string[]
		{
			"BuildableRaw"
		};

		// Token: 0x040096BC RID: 38588
		public static readonly string[] RAW_MINERALS = new string[]
		{
			"BuildableRaw"
		};

		// Token: 0x040096BD RID: 38589
		public static readonly string[] RAW_MINERALS_OR_METALS = new string[]
		{
			"BuildableRaw&Metal"
		};

		// Token: 0x040096BE RID: 38590
		public static readonly string[] RAW_MINERALS_OR_WOOD = new string[]
		{
			"BuildableRaw&" + GameTags.BuildingWood.ToString()
		};

		// Token: 0x040096BF RID: 38591
		public static readonly string[] WOODS = new string[]
		{
			"BuildingWood"
		};

		// Token: 0x040096C0 RID: 38592
		public static readonly string[] REFINED_MINERALS = new string[]
		{
			"BuildableProcessed"
		};

		// Token: 0x040096C1 RID: 38593
		public static readonly string[] PRECIOUS_ROCKS = new string[]
		{
			"PreciousRock"
		};

		// Token: 0x040096C2 RID: 38594
		public static readonly string[] FARMABLE = new string[]
		{
			"Farmable"
		};

		// Token: 0x040096C3 RID: 38595
		public static readonly string[] EXTRUDABLE = new string[]
		{
			"Extrudable"
		};

		// Token: 0x040096C4 RID: 38596
		public static readonly string[] PLUMBABLE = new string[]
		{
			"Plumbable"
		};

		// Token: 0x040096C5 RID: 38597
		public static readonly string[] PLUMBABLE_OR_METALS = new string[]
		{
			"Plumbable&Metal"
		};

		// Token: 0x040096C6 RID: 38598
		public static readonly string[] PLASTICS = new string[]
		{
			"Plastic"
		};

		// Token: 0x040096C7 RID: 38599
		public static readonly string[] GLASSES = new string[]
		{
			"Glass"
		};

		// Token: 0x040096C8 RID: 38600
		public static readonly string[] TRANSPARENTS = new string[]
		{
			"Transparent"
		};

		// Token: 0x040096C9 RID: 38601
		public static readonly string[] BUILDING_FIBER = new string[]
		{
			"BuildingFiber"
		};

		// Token: 0x040096CA RID: 38602
		public static readonly string[] ANY_BUILDABLE = new string[]
		{
			"BuildableAny"
		};

		// Token: 0x040096CB RID: 38603
		public static readonly string[] FLYING_CRITTER_FOOD = new string[]
		{
			"FlyingCritterEdible"
		};

		// Token: 0x040096CC RID: 38604
		public static readonly string[] RADIATION_CONTAINMENT = new string[]
		{
			"Metal",
			"Lead"
		};
	}
}
