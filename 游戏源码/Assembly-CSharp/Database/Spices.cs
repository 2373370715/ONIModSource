using System;
using Klei.AI;
using UnityEngine;

namespace Database
{
	// Token: 0x02002169 RID: 8553
	public class Spices : ResourceSet<Spice>
	{
		// Token: 0x0600B602 RID: 46594 RVA: 0x00456340 File Offset: 0x00454540
		public Spices(ResourceSet parent) : base("Spices", parent)
		{
			this.PreservingSpice = new Spice(this, "PRESERVING_SPICE", new Spice.Ingredient[]
			{
				new Spice.Ingredient
				{
					IngredientSet = new Tag[]
					{
						"BasicSingleHarvestPlantSeed"
					},
					AmountKG = 0.1f
				},
				new Spice.Ingredient
				{
					IngredientSet = new Tag[]
					{
						SimHashes.Salt.CreateTag()
					},
					AmountKG = 3f
				}
			}, new Color(0.961f, 0.827f, 0.29f), Color.white, new AttributeModifier("RotDelta", 0.5f, "Spices", false, false, true), null, "spice_recipe1", null);
			this.PilotingSpice = new Spice(this, "PILOTING_SPICE", new Spice.Ingredient[]
			{
				new Spice.Ingredient
				{
					IngredientSet = new Tag[]
					{
						"MushroomSeed"
					},
					AmountKG = 0.1f
				},
				new Spice.Ingredient
				{
					IngredientSet = new Tag[]
					{
						SimHashes.Sucrose.CreateTag()
					},
					AmountKG = 3f
				}
			}, new Color(0.039f, 0.725f, 0.831f), Color.white, null, new AttributeModifier("SpaceNavigation", 3f, "Spices", false, false, true), "spice_recipe2", DlcManager.AVAILABLE_EXPANSION1_ONLY);
			this.StrengthSpice = new Spice(this, "STRENGTH_SPICE", new Spice.Ingredient[]
			{
				new Spice.Ingredient
				{
					IngredientSet = new Tag[]
					{
						"SeaLettuceSeed"
					},
					AmountKG = 0.1f
				},
				new Spice.Ingredient
				{
					IngredientSet = new Tag[]
					{
						SimHashes.Iron.CreateTag()
					},
					AmountKG = 3f
				}
			}, new Color(0.588f, 0.278f, 0.788f), Color.white, null, new AttributeModifier("Strength", 3f, "Spices", false, false, true), "spice_recipe3", null);
			this.MachinerySpice = new Spice(this, "MACHINERY_SPICE", new Spice.Ingredient[]
			{
				new Spice.Ingredient
				{
					IngredientSet = new Tag[]
					{
						"PrickleFlowerSeed"
					},
					AmountKG = 0.1f
				},
				new Spice.Ingredient
				{
					IngredientSet = new Tag[]
					{
						SimHashes.SlimeMold.CreateTag()
					},
					AmountKG = 3f
				}
			}, new Color(0.788f, 0.443f, 0.792f), Color.white, null, new AttributeModifier("Machinery", 3f, "Spices", false, false, true), "spice_recipe4", null);
		}

		// Token: 0x04009458 RID: 37976
		public Spice PreservingSpice;

		// Token: 0x04009459 RID: 37977
		public Spice PilotingSpice;

		// Token: 0x0400945A RID: 37978
		public Spice StrengthSpice;

		// Token: 0x0400945B RID: 37979
		public Spice MachinerySpice;
	}
}
