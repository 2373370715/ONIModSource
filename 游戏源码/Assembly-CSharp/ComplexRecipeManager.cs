using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

// Token: 0x02001A95 RID: 6805
public class ComplexRecipeManager
{
	// Token: 0x06008E42 RID: 36418 RVA: 0x000FCEC5 File Offset: 0x000FB0C5
	public static ComplexRecipeManager Get()
	{
		if (ComplexRecipeManager._Instance == null)
		{
			ComplexRecipeManager._Instance = new ComplexRecipeManager();
		}
		return ComplexRecipeManager._Instance;
	}

	// Token: 0x06008E43 RID: 36419 RVA: 0x000FCEDD File Offset: 0x000FB0DD
	public static void DestroyInstance()
	{
		ComplexRecipeManager._Instance = null;
	}

	// Token: 0x06008E44 RID: 36420 RVA: 0x0036FE6C File Offset: 0x0036E06C
	public static string MakeObsoleteRecipeID(string fabricator, Tag signatureElement)
	{
		string str = "_";
		Tag tag = signatureElement;
		return fabricator + str + tag.ToString();
	}

	// Token: 0x06008E45 RID: 36421 RVA: 0x0036FE94 File Offset: 0x0036E094
	public static string MakeRecipeID(string fabricator, IList<ComplexRecipe.RecipeElement> inputs, IList<ComplexRecipe.RecipeElement> outputs)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(fabricator);
		stringBuilder.Append("_I");
		foreach (ComplexRecipe.RecipeElement recipeElement in inputs)
		{
			stringBuilder.Append("_");
			stringBuilder.Append(recipeElement.material.ToString());
		}
		stringBuilder.Append("_O");
		foreach (ComplexRecipe.RecipeElement recipeElement2 in outputs)
		{
			stringBuilder.Append("_");
			stringBuilder.Append(recipeElement2.material.ToString());
		}
		return stringBuilder.ToString();
	}

	// Token: 0x06008E46 RID: 36422 RVA: 0x0036FF7C File Offset: 0x0036E17C
	public static string MakeRecipeID(string fabricator, IList<ComplexRecipe.RecipeElement> inputs, IList<ComplexRecipe.RecipeElement> outputs, string facadeID)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(fabricator);
		stringBuilder.Append("_I");
		foreach (ComplexRecipe.RecipeElement recipeElement in inputs)
		{
			stringBuilder.Append("_");
			stringBuilder.Append(recipeElement.material.ToString());
		}
		stringBuilder.Append("_O");
		foreach (ComplexRecipe.RecipeElement recipeElement2 in outputs)
		{
			stringBuilder.Append("_");
			stringBuilder.Append(recipeElement2.material.ToString());
		}
		stringBuilder.Append("_" + facadeID);
		return stringBuilder.ToString();
	}

	// Token: 0x06008E47 RID: 36423 RVA: 0x00370074 File Offset: 0x0036E274
	public void Add(ComplexRecipe recipe)
	{
		using (List<ComplexRecipe>.Enumerator enumerator = this.recipes.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.id == recipe.id)
				{
					global::Debug.LogError(string.Format("DUPLICATE RECIPE ID! '{0}' is being added to the recipe manager multiple times. This will result in the failure to save/load certain queued recipes at fabricators.", recipe.id));
				}
			}
		}
		this.recipes.Add(recipe);
		if (recipe.FabricationVisualizer != null)
		{
			UnityEngine.Object.DontDestroyOnLoad(recipe.FabricationVisualizer);
		}
	}

	// Token: 0x06008E48 RID: 36424 RVA: 0x0037010C File Offset: 0x0036E30C
	public ComplexRecipe GetRecipe(string id)
	{
		if (string.IsNullOrEmpty(id))
		{
			return null;
		}
		return this.recipes.Find((ComplexRecipe r) => r.id == id);
	}

	// Token: 0x06008E49 RID: 36425 RVA: 0x000FCEE5 File Offset: 0x000FB0E5
	public void AddObsoleteIDMapping(string obsolete_id, string new_id)
	{
		this.obsoleteIDMapping[obsolete_id] = new_id;
	}

	// Token: 0x06008E4A RID: 36426 RVA: 0x0037014C File Offset: 0x0036E34C
	public ComplexRecipe GetObsoleteRecipe(string id)
	{
		if (string.IsNullOrEmpty(id))
		{
			return null;
		}
		ComplexRecipe result = null;
		string id2 = null;
		if (this.obsoleteIDMapping.TryGetValue(id, out id2))
		{
			result = this.GetRecipe(id2);
		}
		return result;
	}

	// Token: 0x04006B35 RID: 27445
	private static ComplexRecipeManager _Instance;

	// Token: 0x04006B36 RID: 27446
	public List<ComplexRecipe> recipes = new List<ComplexRecipe>();

	// Token: 0x04006B37 RID: 27447
	private Dictionary<string, string> obsoleteIDMapping = new Dictionary<string, string>();
}
