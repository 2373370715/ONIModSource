using Klei.AI;
using UnityEngine;

namespace Database;

public class Spice : Resource
{
	public class Ingredient : IConfigurableConsumerIngredient
	{
		public Tag[] IngredientSet;

		public float AmountKG;

		public float GetAmount()
		{
			return AmountKG;
		}

		public Tag[] GetIDSets()
		{
			return IngredientSet;
		}
	}

	public readonly Ingredient[] Ingredients;

	public readonly float TotalKG;

	public AttributeModifier StatBonus { get; private set; }

	public AttributeModifier FoodModifier { get; private set; }

	public AttributeModifier CalorieModifier { get; private set; }

	public Color PrimaryColor { get; private set; }

	public Color SecondaryColor { get; private set; }

	public string Image { get; private set; }

	public string[] DlcIds { get; private set; } = DlcManager.AVAILABLE_ALL_VERSIONS;


	public Spice(ResourceSet parent, string id, Ingredient[] ingredients, Color primaryColor, Color secondaryColor, AttributeModifier foodMod = null, AttributeModifier statBonus = null, string imageName = "unknown", string[] dlcID = null)
		: base(id, parent)
	{
		if (dlcID != null)
		{
			DlcIds = dlcID;
		}
		StatBonus = statBonus;
		FoodModifier = foodMod;
		Ingredients = ingredients;
		Image = imageName;
		PrimaryColor = primaryColor;
		SecondaryColor = secondaryColor;
		for (int i = 0; i < Ingredients.Length; i++)
		{
			TotalKG += Ingredients[i].AmountKG;
		}
	}
}
