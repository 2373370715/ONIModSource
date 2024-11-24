using System;
using System.Collections.Generic;
using System.Diagnostics;
using Klei;
using STRINGS;
using UnityEngine;

// Token: 0x0200173A RID: 5946
[DebuggerDisplay("{Name}")]
public class Recipe : IHasSortOrder
{
	// Token: 0x170007AD RID: 1965
	// (get) Token: 0x06007A70 RID: 31344 RVA: 0x000F06A2 File Offset: 0x000EE8A2
	// (set) Token: 0x06007A71 RID: 31345 RVA: 0x000F06AA File Offset: 0x000EE8AA
	public int sortOrder { get; set; }

	// Token: 0x170007AE RID: 1966
	// (get) Token: 0x06007A73 RID: 31347 RVA: 0x000F06BC File Offset: 0x000EE8BC
	// (set) Token: 0x06007A72 RID: 31346 RVA: 0x000F06B3 File Offset: 0x000EE8B3
	public string Name
	{
		get
		{
			if (this.nameOverride != null)
			{
				return this.nameOverride;
			}
			return this.Result.ProperName();
		}
		set
		{
			this.nameOverride = value;
		}
	}

	// Token: 0x06007A74 RID: 31348 RVA: 0x000F06D8 File Offset: 0x000EE8D8
	public Recipe()
	{
	}

	// Token: 0x06007A75 RID: 31349 RVA: 0x00318C94 File Offset: 0x00316E94
	public Recipe(string prefabId, float outputUnits = 1f, SimHashes elementOverride = (SimHashes)0, string nameOverride = null, string recipeDescription = null, int sortOrder = 0)
	{
		global::Debug.Assert(prefabId != null);
		this.Result = TagManager.Create(prefabId);
		this.ResultElementOverride = elementOverride;
		this.nameOverride = nameOverride;
		this.OutputUnits = outputUnits;
		this.Ingredients = new List<Recipe.Ingredient>();
		this.recipeDescription = recipeDescription;
		this.sortOrder = sortOrder;
		this.FabricationVisualizer = null;
	}

	// Token: 0x06007A76 RID: 31350 RVA: 0x000F06EB File Offset: 0x000EE8EB
	public Recipe SetFabricator(string fabricator, float fabricationTime)
	{
		this.fabricators = new string[]
		{
			fabricator
		};
		this.FabricationTime = fabricationTime;
		RecipeManager.Get().Add(this);
		return this;
	}

	// Token: 0x06007A77 RID: 31351 RVA: 0x000F0710 File Offset: 0x000EE910
	public Recipe SetFabricators(string[] fabricators, float fabricationTime)
	{
		this.fabricators = fabricators;
		this.FabricationTime = fabricationTime;
		RecipeManager.Get().Add(this);
		return this;
	}

	// Token: 0x06007A78 RID: 31352 RVA: 0x000F072C File Offset: 0x000EE92C
	public Recipe SetIcon(Sprite Icon)
	{
		this.Icon = Icon;
		this.IconColor = Color.white;
		return this;
	}

	// Token: 0x06007A79 RID: 31353 RVA: 0x000F0741 File Offset: 0x000EE941
	public Recipe SetIcon(Sprite Icon, Color IconColor)
	{
		this.Icon = Icon;
		this.IconColor = IconColor;
		return this;
	}

	// Token: 0x06007A7A RID: 31354 RVA: 0x000F0752 File Offset: 0x000EE952
	public Recipe AddIngredient(Recipe.Ingredient ingredient)
	{
		this.Ingredients.Add(ingredient);
		return this;
	}

	// Token: 0x06007A7B RID: 31355 RVA: 0x00318D00 File Offset: 0x00316F00
	public Recipe.Ingredient[] GetAllIngredients(IList<Tag> selectedTags)
	{
		List<Recipe.Ingredient> list = new List<Recipe.Ingredient>();
		for (int i = 0; i < this.Ingredients.Count; i++)
		{
			float amount = this.Ingredients[i].amount;
			if (i < selectedTags.Count)
			{
				list.Add(new Recipe.Ingredient(selectedTags[i], amount));
			}
			else
			{
				list.Add(new Recipe.Ingredient(this.Ingredients[i].tag, amount));
			}
		}
		return list.ToArray();
	}

	// Token: 0x06007A7C RID: 31356 RVA: 0x00318D7C File Offset: 0x00316F7C
	public Recipe.Ingredient[] GetAllIngredients(IList<Element> selected_elements)
	{
		List<Recipe.Ingredient> list = new List<Recipe.Ingredient>();
		for (int i = 0; i < this.Ingredients.Count; i++)
		{
			int num = (int)this.Ingredients[i].amount;
			bool flag = false;
			if (i < selected_elements.Count)
			{
				Element element = selected_elements[i];
				if (element != null && element.HasTag(this.Ingredients[i].tag))
				{
					list.Add(new Recipe.Ingredient(GameTagExtensions.Create(element.id), (float)num));
					flag = true;
				}
			}
			if (!flag)
			{
				list.Add(new Recipe.Ingredient(this.Ingredients[i].tag, (float)num));
			}
		}
		return list.ToArray();
	}

	// Token: 0x06007A7D RID: 31357 RVA: 0x00318E34 File Offset: 0x00317034
	public GameObject Craft(Storage resource_storage, IList<Tag> selectedTags)
	{
		Recipe.Ingredient[] allIngredients = this.GetAllIngredients(selectedTags);
		return this.CraftRecipe(resource_storage, allIngredients);
	}

	// Token: 0x06007A7E RID: 31358 RVA: 0x00318E54 File Offset: 0x00317054
	private GameObject CraftRecipe(Storage resource_storage, Recipe.Ingredient[] ingredientTags)
	{
		SimUtil.DiseaseInfo diseaseInfo = SimUtil.DiseaseInfo.Invalid;
		float num = 0f;
		float num2 = 0f;
		foreach (Recipe.Ingredient ingredient in ingredientTags)
		{
			GameObject gameObject = resource_storage.FindFirst(ingredient.tag);
			if (gameObject != null)
			{
				Edible component = gameObject.GetComponent<Edible>();
				if (component)
				{
					ReportManager.Instance.ReportValue(ReportManager.ReportType.CaloriesCreated, -component.Calories, StringFormatter.Replace(UI.ENDOFDAYREPORT.NOTES.CRAFTED_USED, "{0}", component.GetProperName()), UI.ENDOFDAYREPORT.NOTES.CRAFTED_CONTEXT);
				}
			}
			SimUtil.DiseaseInfo b;
			float temp;
			resource_storage.ConsumeAndGetDisease(ingredient, out b, out temp);
			diseaseInfo = SimUtil.CalculateFinalDiseaseInfo(diseaseInfo, b);
			num = SimUtil.CalculateFinalTemperature(num2, num, ingredient.amount, temp);
			num2 += ingredient.amount;
		}
		GameObject prefab = Assets.GetPrefab(this.Result);
		GameObject gameObject2 = null;
		if (prefab != null)
		{
			gameObject2 = GameUtil.KInstantiate(prefab, Grid.SceneLayer.Ore, null, 0);
			PrimaryElement component2 = gameObject2.GetComponent<PrimaryElement>();
			gameObject2.GetComponent<KSelectable>().entityName = this.Name;
			if (component2 != null)
			{
				gameObject2.GetComponent<KPrefabID>().RemoveTag(TagManager.Create("Vacuum"));
				if (this.ResultElementOverride != (SimHashes)0)
				{
					if (component2.GetComponent<ElementChunk>() != null)
					{
						component2.SetElement(this.ResultElementOverride, true);
					}
					else
					{
						component2.ElementID = this.ResultElementOverride;
					}
				}
				component2.Temperature = num;
				component2.Units = this.OutputUnits;
			}
			Edible component3 = gameObject2.GetComponent<Edible>();
			if (component3)
			{
				ReportManager.Instance.ReportValue(ReportManager.ReportType.CaloriesCreated, component3.Calories, StringFormatter.Replace(UI.ENDOFDAYREPORT.NOTES.CRAFTED, "{0}", component3.GetProperName()), UI.ENDOFDAYREPORT.NOTES.CRAFTED_CONTEXT);
			}
			gameObject2.SetActive(true);
			if (component2 != null)
			{
				component2.AddDisease(diseaseInfo.idx, diseaseInfo.count, "Recipe.CraftRecipe");
			}
			gameObject2.GetComponent<KMonoBehaviour>().Trigger(748399584, null);
		}
		return gameObject2;
	}

	// Token: 0x170007AF RID: 1967
	// (get) Token: 0x06007A7F RID: 31359 RVA: 0x0031905C File Offset: 0x0031725C
	public string[] MaterialOptionNames
	{
		get
		{
			List<string> list = new List<string>();
			foreach (Element element in ElementLoader.elements)
			{
				if (Array.IndexOf<Tag>(element.oreTags, this.Ingredients[0].tag) >= 0)
				{
					list.Add(element.id.ToString());
				}
			}
			return list.ToArray();
		}
	}

	// Token: 0x06007A80 RID: 31360 RVA: 0x003190EC File Offset: 0x003172EC
	public Element[] MaterialOptions()
	{
		List<Element> list = new List<Element>();
		foreach (Element element in ElementLoader.elements)
		{
			if (Array.IndexOf<Tag>(element.oreTags, this.Ingredients[0].tag) >= 0)
			{
				list.Add(element);
			}
		}
		return list.ToArray();
	}

	// Token: 0x06007A81 RID: 31361 RVA: 0x0031916C File Offset: 0x0031736C
	public BuildingDef GetBuildingDef()
	{
		BuildingComplete component = Assets.GetPrefab(this.Result).GetComponent<BuildingComplete>();
		if (component != null)
		{
			return component.Def;
		}
		return null;
	}

	// Token: 0x06007A82 RID: 31362 RVA: 0x0031919C File Offset: 0x0031739C
	public Sprite GetUIIcon()
	{
		Sprite result = null;
		if (this.Icon != null)
		{
			result = this.Icon;
		}
		else
		{
			KBatchedAnimController component = Assets.GetPrefab(this.Result).GetComponent<KBatchedAnimController>();
			if (component != null)
			{
				result = Def.GetUISpriteFromMultiObjectAnim(component.AnimFiles[0], "ui", false, "");
			}
		}
		return result;
	}

	// Token: 0x06007A83 RID: 31363 RVA: 0x000F0761 File Offset: 0x000EE961
	public Color GetUIColor()
	{
		if (!(this.Icon != null))
		{
			return Color.white;
		}
		return this.IconColor;
	}

	// Token: 0x04005BE2 RID: 23522
	private string nameOverride;

	// Token: 0x04005BE3 RID: 23523
	public string HotKey;

	// Token: 0x04005BE4 RID: 23524
	public string Type;

	// Token: 0x04005BE5 RID: 23525
	public List<Recipe.Ingredient> Ingredients;

	// Token: 0x04005BE6 RID: 23526
	public string recipeDescription;

	// Token: 0x04005BE7 RID: 23527
	public Tag Result;

	// Token: 0x04005BE8 RID: 23528
	public GameObject FabricationVisualizer;

	// Token: 0x04005BE9 RID: 23529
	public SimHashes ResultElementOverride;

	// Token: 0x04005BEA RID: 23530
	public Sprite Icon;

	// Token: 0x04005BEB RID: 23531
	public Color IconColor = Color.white;

	// Token: 0x04005BEC RID: 23532
	public string[] fabricators;

	// Token: 0x04005BED RID: 23533
	public float OutputUnits;

	// Token: 0x04005BEE RID: 23534
	public float FabricationTime;

	// Token: 0x04005BEF RID: 23535
	public string TechUnlock;

	// Token: 0x0200173B RID: 5947
	[DebuggerDisplay("{tag} {amount}")]
	[Serializable]
	public class Ingredient
	{
		// Token: 0x06007A84 RID: 31364 RVA: 0x000F077D File Offset: 0x000EE97D
		public Ingredient(string tag, float amount)
		{
			this.tag = TagManager.Create(tag);
			this.amount = amount;
		}

		// Token: 0x06007A85 RID: 31365 RVA: 0x000F0798 File Offset: 0x000EE998
		public Ingredient(Tag tag, float amount)
		{
			this.tag = tag;
			this.amount = amount;
		}

		// Token: 0x06007A86 RID: 31366 RVA: 0x003191F8 File Offset: 0x003173F8
		public List<Element> GetElementOptions()
		{
			List<Element> list = new List<Element>(ElementLoader.elements);
			list.RemoveAll((Element e) => !e.IsSolid);
			list.RemoveAll((Element e) => !e.HasTag(this.tag));
			return list;
		}

		// Token: 0x04005BF0 RID: 23536
		public Tag tag;

		// Token: 0x04005BF1 RID: 23537
		public float amount;
	}
}
