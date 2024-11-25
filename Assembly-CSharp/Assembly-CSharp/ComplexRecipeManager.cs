﻿using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class ComplexRecipeManager
{
		public static ComplexRecipeManager Get()
	{
		if (ComplexRecipeManager._Instance == null)
		{
			ComplexRecipeManager._Instance = new ComplexRecipeManager();
		}
		return ComplexRecipeManager._Instance;
	}

		public static void DestroyInstance()
	{
		ComplexRecipeManager._Instance = null;
	}

		public static string MakeObsoleteRecipeID(string fabricator, Tag signatureElement)
	{
		string str = "_";
		Tag tag = signatureElement;
		return fabricator + str + tag.ToString();
	}

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

		public ComplexRecipe GetRecipe(string id)
	{
		if (string.IsNullOrEmpty(id))
		{
			return null;
		}
		return this.recipes.Find((ComplexRecipe r) => r.id == id);
	}

		public void AddObsoleteIDMapping(string obsolete_id, string new_id)
	{
		this.obsoleteIDMapping[obsolete_id] = new_id;
	}

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

		private static ComplexRecipeManager _Instance;

		public List<ComplexRecipe> recipes = new List<ComplexRecipe>();

		private Dictionary<string, string> obsoleteIDMapping = new Dictionary<string, string>();
}
