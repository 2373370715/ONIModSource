using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200184E RID: 6222
public class MaterialsAvailable : SelectModuleCondition
{
	// Token: 0x060080B1 RID: 32945 RVA: 0x000A65EC File Offset: 0x000A47EC
	public override bool IgnoreInSanboxMode()
	{
		return true;
	}

	// Token: 0x060080B2 RID: 32946 RVA: 0x000F4B00 File Offset: 0x000F2D00
	public override bool EvaluateCondition(GameObject existingModule, BuildingDef selectedPart, SelectModuleCondition.SelectionContext selectionContext)
	{
		return existingModule == null || ProductInfoScreen.MaterialsMet(selectedPart.CraftRecipe);
	}

	// Token: 0x060080B3 RID: 32947 RVA: 0x003356E8 File Offset: 0x003338E8
	public override string GetStatusTooltip(bool ready, GameObject moduleBase, BuildingDef selectedPart)
	{
		if (ready)
		{
			return UI.UISIDESCREENS.SELECTMODULESIDESCREEN.CONSTRAINTS.MATERIALS_AVAILABLE.COMPLETE;
		}
		string text = UI.UISIDESCREENS.SELECTMODULESIDESCREEN.CONSTRAINTS.MATERIALS_AVAILABLE.FAILED;
		foreach (Recipe.Ingredient ingredient in selectedPart.CraftRecipe.Ingredients)
		{
			string str = "\n" + string.Format("{0}{1}: {2}", "    • ", ingredient.tag.ProperName(), GameUtil.GetFormattedMass(ingredient.amount, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
			text += str;
		}
		return text;
	}
}
