using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001A94 RID: 6804
public class RecipeManager
{
	// Token: 0x06008E3E RID: 36414 RVA: 0x000FCE6B File Offset: 0x000FB06B
	public static RecipeManager Get()
	{
		if (RecipeManager._Instance == null)
		{
			RecipeManager._Instance = new RecipeManager();
		}
		return RecipeManager._Instance;
	}

	// Token: 0x06008E3F RID: 36415 RVA: 0x000FCE83 File Offset: 0x000FB083
	public static void DestroyInstance()
	{
		RecipeManager._Instance = null;
	}

	// Token: 0x06008E40 RID: 36416 RVA: 0x000FCE8B File Offset: 0x000FB08B
	public void Add(Recipe recipe)
	{
		this.recipes.Add(recipe);
		if (recipe.FabricationVisualizer != null)
		{
			UnityEngine.Object.DontDestroyOnLoad(recipe.FabricationVisualizer);
		}
	}

	// Token: 0x04006B33 RID: 27443
	private static RecipeManager _Instance;

	// Token: 0x04006B34 RID: 27444
	public List<Recipe> recipes = new List<Recipe>();
}
