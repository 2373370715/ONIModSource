using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02001A97 RID: 6807
public class ComplexRecipe
{
	// Token: 0x06008E4E RID: 36430 RVA: 0x000FCF25 File Offset: 0x000FB125
	public string[] GetDlcIds()
	{
		return this.dlcIds;
	}

	// Token: 0x17000977 RID: 2423
	// (get) Token: 0x06008E4F RID: 36431 RVA: 0x000FCF2D File Offset: 0x000FB12D
	// (set) Token: 0x06008E50 RID: 36432 RVA: 0x000FCF35 File Offset: 0x000FB135
	public bool ProductHasFacade { get; set; }

	// Token: 0x17000978 RID: 2424
	// (get) Token: 0x06008E51 RID: 36433 RVA: 0x000FCF3E File Offset: 0x000FB13E
	// (set) Token: 0x06008E52 RID: 36434 RVA: 0x000FCF46 File Offset: 0x000FB146
	public bool RequiresAllIngredientsDiscovered { get; set; }

	// Token: 0x17000979 RID: 2425
	// (get) Token: 0x06008E53 RID: 36435 RVA: 0x000FCF4F File Offset: 0x000FB14F
	public Tag FirstResult
	{
		get
		{
			return this.results[0].material;
		}
	}

	// Token: 0x06008E54 RID: 36436 RVA: 0x000FCF5E File Offset: 0x000FB15E
	public ComplexRecipe(string id, ComplexRecipe.RecipeElement[] ingredients, ComplexRecipe.RecipeElement[] results)
	{
		this.id = id;
		this.ingredients = ingredients;
		this.results = results;
		ComplexRecipeManager.Get().Add(this);
	}

	// Token: 0x06008E55 RID: 36437 RVA: 0x000FCF9C File Offset: 0x000FB19C
	public ComplexRecipe(string id, ComplexRecipe.RecipeElement[] ingredients, ComplexRecipe.RecipeElement[] results, int consumedHEP, int producedHEP) : this(id, ingredients, results)
	{
		this.consumedHEP = consumedHEP;
		this.producedHEP = producedHEP;
	}

	// Token: 0x06008E56 RID: 36438 RVA: 0x000FCFB7 File Offset: 0x000FB1B7
	public ComplexRecipe(string id, ComplexRecipe.RecipeElement[] ingredients, ComplexRecipe.RecipeElement[] results, int consumedHEP) : this(id, ingredients, results, consumedHEP, 0)
	{
	}

	// Token: 0x06008E57 RID: 36439 RVA: 0x000FCFC5 File Offset: 0x000FB1C5
	public ComplexRecipe(string id, ComplexRecipe.RecipeElement[] ingredients, ComplexRecipe.RecipeElement[] results, string[] dlcIds) : this(id, ingredients, results)
	{
		this.dlcIds = dlcIds;
	}

	// Token: 0x06008E58 RID: 36440 RVA: 0x000FCFD8 File Offset: 0x000FB1D8
	public ComplexRecipe(string id, ComplexRecipe.RecipeElement[] ingredients, ComplexRecipe.RecipeElement[] results, int consumedHEP, int producedHEP, string[] dlcIds) : this(id, ingredients, results, consumedHEP, producedHEP)
	{
		this.dlcIds = dlcIds;
	}

	// Token: 0x06008E59 RID: 36441 RVA: 0x00370180 File Offset: 0x0036E380
	public float TotalResultUnits()
	{
		float num = 0f;
		foreach (ComplexRecipe.RecipeElement recipeElement in this.results)
		{
			num += recipeElement.amount;
		}
		return num;
	}

	// Token: 0x06008E5A RID: 36442 RVA: 0x000FCFEF File Offset: 0x000FB1EF
	public bool RequiresTechUnlock()
	{
		return !string.IsNullOrEmpty(this.requiredTech);
	}

	// Token: 0x06008E5B RID: 36443 RVA: 0x000FCFFF File Offset: 0x000FB1FF
	public bool IsRequiredTechUnlocked()
	{
		return string.IsNullOrEmpty(this.requiredTech) || Db.Get().Techs.Get(this.requiredTech).IsComplete();
	}

	// Token: 0x06008E5C RID: 36444 RVA: 0x003701B8 File Offset: 0x0036E3B8
	public Sprite GetUIIcon()
	{
		Sprite result = null;
		KBatchedAnimController component = Assets.GetPrefab((this.nameDisplay == ComplexRecipe.RecipeNameDisplay.Ingredient) ? this.ingredients[0].material : this.results[0].material).GetComponent<KBatchedAnimController>();
		if (component != null)
		{
			result = Def.GetUISpriteFromMultiObjectAnim(component.AnimFiles[0], "ui", false, "");
		}
		return result;
	}

	// Token: 0x06008E5D RID: 36445 RVA: 0x000FD02A File Offset: 0x000FB22A
	public Color GetUIColor()
	{
		return Color.white;
	}

	// Token: 0x06008E5E RID: 36446 RVA: 0x0037021C File Offset: 0x0036E41C
	public string GetUIName(bool includeAmounts)
	{
		string text = this.results[0].facadeID.IsNullOrWhiteSpace() ? this.results[0].material.ProperName() : this.results[0].facadeID.ProperName();
		switch (this.nameDisplay)
		{
		case ComplexRecipe.RecipeNameDisplay.Result:
			if (includeAmounts)
			{
				return string.Format(UI.UISIDESCREENS.REFINERYSIDESCREEN.RECIPE_SIMPLE_INCLUDE_AMOUNTS, text, this.results[0].amount);
			}
			return text;
		case ComplexRecipe.RecipeNameDisplay.IngredientToResult:
			if (includeAmounts)
			{
				return string.Format(UI.UISIDESCREENS.REFINERYSIDESCREEN.RECIPE_FROM_TO_INCLUDE_AMOUNTS, new object[]
				{
					this.ingredients[0].material.ProperName(),
					text,
					this.ingredients[0].amount,
					this.results[0].amount
				});
			}
			return string.Format(UI.UISIDESCREENS.REFINERYSIDESCREEN.RECIPE_FROM_TO, this.ingredients[0].material.ProperName(), text);
		case ComplexRecipe.RecipeNameDisplay.ResultWithIngredient:
			if (includeAmounts)
			{
				return string.Format(UI.UISIDESCREENS.REFINERYSIDESCREEN.RECIPE_WITH_INCLUDE_AMOUNTS, new object[]
				{
					this.ingredients[0].material.ProperName(),
					text,
					this.ingredients[0].amount,
					this.results[0].amount
				});
			}
			return string.Format(UI.UISIDESCREENS.REFINERYSIDESCREEN.RECIPE_WITH, this.ingredients[0].material.ProperName(), text);
		case ComplexRecipe.RecipeNameDisplay.Composite:
			if (includeAmounts)
			{
				return string.Format(UI.UISIDESCREENS.REFINERYSIDESCREEN.RECIPE_FROM_TO_COMPOSITE_INCLUDE_AMOUNTS, new object[]
				{
					this.ingredients[0].material.ProperName(),
					text,
					this.results[1].material.ProperName(),
					this.ingredients[0].amount,
					this.results[0].amount,
					this.results[1].amount
				});
			}
			return string.Format(UI.UISIDESCREENS.REFINERYSIDESCREEN.RECIPE_FROM_TO_COMPOSITE, this.ingredients[0].material.ProperName(), text, this.results[1].material.ProperName());
		case ComplexRecipe.RecipeNameDisplay.HEP:
			if (includeAmounts)
			{
				return string.Format(UI.UISIDESCREENS.REFINERYSIDESCREEN.RECIPE_FROM_TO_HEP_INCLUDE_AMOUNTS, new object[]
				{
					this.ingredients[0].material.ProperName(),
					this.results[1].material.ProperName(),
					this.ingredients[0].amount,
					this.producedHEP,
					this.results[1].amount
				});
			}
			return string.Format(UI.UISIDESCREENS.REFINERYSIDESCREEN.RECIPE_FROM_TO_HEP, this.ingredients[0].material.ProperName(), text);
		case ComplexRecipe.RecipeNameDisplay.Custom:
			return this.customName;
		}
		if (includeAmounts)
		{
			return string.Format(UI.UISIDESCREENS.REFINERYSIDESCREEN.RECIPE_SIMPLE_INCLUDE_AMOUNTS, this.ingredients[0].material.ProperName(), this.ingredients[0].amount);
		}
		return this.ingredients[0].material.ProperName();
	}

	// Token: 0x04006B39 RID: 27449
	public string id;

	// Token: 0x04006B3A RID: 27450
	public ComplexRecipe.RecipeElement[] ingredients;

	// Token: 0x04006B3B RID: 27451
	public ComplexRecipe.RecipeElement[] results;

	// Token: 0x04006B3C RID: 27452
	public float time;

	// Token: 0x04006B3D RID: 27453
	public GameObject FabricationVisualizer;

	// Token: 0x04006B3E RID: 27454
	public int consumedHEP;

	// Token: 0x04006B3F RID: 27455
	public int producedHEP;

	// Token: 0x04006B40 RID: 27456
	public string recipeCategoryID = "";

	// Token: 0x04006B41 RID: 27457
	private string[] dlcIds = DlcManager.AVAILABLE_ALL_VERSIONS;

	// Token: 0x04006B43 RID: 27459
	public ComplexRecipe.RecipeNameDisplay nameDisplay;

	// Token: 0x04006B44 RID: 27460
	public string customName;

	// Token: 0x04006B45 RID: 27461
	public string description;

	// Token: 0x04006B46 RID: 27462
	public List<Tag> fabricators;

	// Token: 0x04006B47 RID: 27463
	public int sortOrder;

	// Token: 0x04006B48 RID: 27464
	public string requiredTech;

	// Token: 0x02001A98 RID: 6808
	public enum RecipeNameDisplay
	{
		// Token: 0x04006B4B RID: 27467
		Ingredient,
		// Token: 0x04006B4C RID: 27468
		Result,
		// Token: 0x04006B4D RID: 27469
		IngredientToResult,
		// Token: 0x04006B4E RID: 27470
		ResultWithIngredient,
		// Token: 0x04006B4F RID: 27471
		Composite,
		// Token: 0x04006B50 RID: 27472
		HEP,
		// Token: 0x04006B51 RID: 27473
		Custom
	}

	// Token: 0x02001A99 RID: 6809
	public class RecipeElement
	{
		// Token: 0x06008E5F RID: 36447 RVA: 0x000FD031 File Offset: 0x000FB231
		public RecipeElement(Tag material, float amount, bool inheritElement)
		{
			this.material = material;
			this.amount = amount;
			this.temperatureOperation = ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature;
			this.inheritElement = inheritElement;
		}

		// Token: 0x06008E60 RID: 36448 RVA: 0x000FD055 File Offset: 0x000FB255
		public RecipeElement(Tag material, float amount)
		{
			this.material = material;
			this.amount = amount;
			this.temperatureOperation = ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature;
		}

		// Token: 0x06008E61 RID: 36449 RVA: 0x000FD072 File Offset: 0x000FB272
		public RecipeElement(Tag material, float amount, ComplexRecipe.RecipeElement.TemperatureOperation temperatureOperation, bool storeElement = false)
		{
			this.material = material;
			this.amount = amount;
			this.temperatureOperation = temperatureOperation;
			this.storeElement = storeElement;
		}

		// Token: 0x06008E62 RID: 36450 RVA: 0x000FD097 File Offset: 0x000FB297
		public RecipeElement(Tag material, float amount, ComplexRecipe.RecipeElement.TemperatureOperation temperatureOperation, string facadeID, bool storeElement = false)
		{
			this.material = material;
			this.amount = amount;
			this.temperatureOperation = temperatureOperation;
			this.storeElement = storeElement;
			this.facadeID = facadeID;
		}

		// Token: 0x06008E63 RID: 36451 RVA: 0x000FD0C4 File Offset: 0x000FB2C4
		public RecipeElement(EdiblesManager.FoodInfo foodInfo, float amount)
		{
			this.material = foodInfo.Id;
			this.amount = amount;
			this.Edible = true;
		}

		// Token: 0x1700097A RID: 2426
		// (get) Token: 0x06008E64 RID: 36452 RVA: 0x000FD0EB File Offset: 0x000FB2EB
		// (set) Token: 0x06008E65 RID: 36453 RVA: 0x000FD0F3 File Offset: 0x000FB2F3
		public float amount { get; private set; }

		// Token: 0x04006B52 RID: 27474
		public Tag material;

		// Token: 0x04006B54 RID: 27476
		public ComplexRecipe.RecipeElement.TemperatureOperation temperatureOperation;

		// Token: 0x04006B55 RID: 27477
		public bool storeElement;

		// Token: 0x04006B56 RID: 27478
		public bool inheritElement;

		// Token: 0x04006B57 RID: 27479
		public string facadeID;

		// Token: 0x04006B58 RID: 27480
		public bool Edible;

		// Token: 0x02001A9A RID: 6810
		public enum TemperatureOperation
		{
			// Token: 0x04006B5A RID: 27482
			AverageTemperature,
			// Token: 0x04006B5B RID: 27483
			Heated,
			// Token: 0x04006B5C RID: 27484
			Melted,
			// Token: 0x04006B5D RID: 27485
			Dehydrated
		}
	}
}
