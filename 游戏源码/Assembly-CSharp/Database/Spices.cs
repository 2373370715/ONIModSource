using Klei.AI;
using UnityEngine;

namespace Database;

public class Spices : ResourceSet<Spice>
{
	public Spice PreservingSpice;

	public Spice PilotingSpice;

	public Spice StrengthSpice;

	public Spice MachinerySpice;

	public Spices(ResourceSet parent)
		: base("Spices", parent)
	{
		PreservingSpice = new Spice(this, "PRESERVING_SPICE", new Spice.Ingredient[2]
		{
			new Spice.Ingredient
			{
				IngredientSet = new Tag[1] { "BasicSingleHarvestPlantSeed" },
				AmountKG = 0.1f
			},
			new Spice.Ingredient
			{
				IngredientSet = new Tag[1] { SimHashes.Salt.CreateTag() },
				AmountKG = 3f
			}
		}, new Color(0.961f, 0.827f, 0.29f), Color.white, new AttributeModifier("RotDelta", 0.5f, "Spices"), null, "spice_recipe1");
		PilotingSpice = new Spice(this, "PILOTING_SPICE", new Spice.Ingredient[2]
		{
			new Spice.Ingredient
			{
				IngredientSet = new Tag[1] { "MushroomSeed" },
				AmountKG = 0.1f
			},
			new Spice.Ingredient
			{
				IngredientSet = new Tag[1] { SimHashes.Sucrose.CreateTag() },
				AmountKG = 3f
			}
		}, new Color(0.039f, 0.725f, 0.831f), Color.white, null, new AttributeModifier("SpaceNavigation", 3f, "Spices"), "spice_recipe2", DlcManager.AVAILABLE_EXPANSION1_ONLY);
		StrengthSpice = new Spice(this, "STRENGTH_SPICE", new Spice.Ingredient[2]
		{
			new Spice.Ingredient
			{
				IngredientSet = new Tag[1] { "SeaLettuceSeed" },
				AmountKG = 0.1f
			},
			new Spice.Ingredient
			{
				IngredientSet = new Tag[1] { SimHashes.Iron.CreateTag() },
				AmountKG = 3f
			}
		}, new Color(0.588f, 0.278f, 0.788f), Color.white, null, new AttributeModifier("Strength", 3f, "Spices"), "spice_recipe3");
		MachinerySpice = new Spice(this, "MACHINERY_SPICE", new Spice.Ingredient[2]
		{
			new Spice.Ingredient
			{
				IngredientSet = new Tag[1] { "PrickleFlowerSeed" },
				AmountKG = 0.1f
			},
			new Spice.Ingredient
			{
				IngredientSet = new Tag[1] { SimHashes.SlimeMold.CreateTag() },
				AmountKG = 3f
			}
		}, new Color(0.788f, 0.443f, 0.792f), Color.white, null, new AttributeModifier("Machinery", 3f, "Spices"), "spice_recipe4");
	}
}
