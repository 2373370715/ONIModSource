using System.Linq;
using STRINGS;

namespace TUNING;

public class MATERIALS
{
	public const string METAL = "Metal";

	public const string REFINED_METAL = "RefinedMetal";

	public const string GLASS = "Glass";

	public const string TRANSPARENT = "Transparent";

	public const string PLASTIC = "Plastic";

	public const string BUILDABLERAW = "BuildableRaw";

	public const string PRECIOUSROCK = "PreciousRock";

	public const string WOOD = "BuildingWood";

	public const string BUILDINGFIBER = "BuildingFiber";

	public const string LEAD = "Lead";

	public const string INSULATOR = "Insulator";

	public static readonly string[] ALL_METALS = new string[1] { "Metal" };

	public static readonly string[] RAW_METALS = new string[1] { "Metal" };

	public static readonly string[] REFINED_METALS = new string[1] { "RefinedMetal" };

	public static readonly string[] ALLOYS = new string[1] { "Alloy" };

	public static readonly string[] ALL_MINERALS = new string[1] { "BuildableRaw" };

	public static readonly string[] RAW_MINERALS = new string[1] { "BuildableRaw" };

	public static readonly string[] RAW_MINERALS_OR_METALS = new string[1] { "BuildableRaw&Metal" };

	public static readonly string[] RAW_MINERALS_OR_WOOD = new string[1] { "BuildableRaw&" + GameTags.BuildingWood.ToString() };

	public static readonly string[] WOODS = new string[1] { "BuildingWood" };

	public static readonly string[] REFINED_MINERALS = new string[1] { "BuildableProcessed" };

	public static readonly string[] PRECIOUS_ROCKS = new string[1] { "PreciousRock" };

	public static readonly string[] FARMABLE = new string[1] { "Farmable" };

	public static readonly string[] EXTRUDABLE = new string[1] { "Extrudable" };

	public static readonly string[] PLUMBABLE = new string[1] { "Plumbable" };

	public static readonly string[] PLUMBABLE_OR_METALS = new string[1] { "Plumbable&Metal" };

	public static readonly string[] PLASTICS = new string[1] { "Plastic" };

	public static readonly string[] GLASSES = new string[1] { "Glass" };

	public static readonly string[] TRANSPARENTS = new string[1] { "Transparent" };

	public static readonly string[] BUILDING_FIBER = new string[1] { "BuildingFiber" };

	public static readonly string[] ANY_BUILDABLE = new string[1] { "BuildableAny" };

	public static readonly string[] FLYING_CRITTER_FOOD = new string[1] { "FlyingCritterEdible" };

	public static readonly string[] RADIATION_CONTAINMENT = new string[2] { "Metal", "Lead" };

	public static string GetMaterialString(string materialCategory)
	{
		string[] array = materialCategory.Split('&');
		if (array.Length == 1)
		{
			return UI.FormatAsLink(Strings.Get("STRINGS.MISC.TAGS." + materialCategory.ToUpper()), materialCategory);
		}
		return string.Join(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.PREPARED_SEPARATOR, array.Select((string s) => UI.FormatAsLink(Strings.Get("STRINGS.MISC.TAGS." + s.ToUpper()), s)));
	}
}
