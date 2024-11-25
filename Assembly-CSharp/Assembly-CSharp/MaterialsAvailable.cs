using STRINGS;
using UnityEngine;

public class MaterialsAvailable : SelectModuleCondition {
    public override bool IgnoreInSanboxMode() { return true; }

    public override bool EvaluateCondition(GameObject       existingModule,
                                           BuildingDef      selectedPart,
                                           SelectionContext selectionContext) {
        return existingModule == null || ProductInfoScreen.MaterialsMet(selectedPart.CraftRecipe);
    }

    public override string GetStatusTooltip(bool ready, GameObject moduleBase, BuildingDef selectedPart) {
        if (ready) return UI.UISIDESCREENS.SELECTMODULESIDESCREEN.CONSTRAINTS.MATERIALS_AVAILABLE.COMPLETE;

        string text = UI.UISIDESCREENS.SELECTMODULESIDESCREEN.CONSTRAINTS.MATERIALS_AVAILABLE.FAILED;
        foreach (var ingredient in selectedPart.CraftRecipe.Ingredients) {
            var str = "\n" +
                      string.Format("{0}{1}: {2}",
                                    "    • ",
                                    ingredient.tag.ProperName(),
                                    GameUtil.GetFormattedMass(ingredient.amount));

            text += str;
        }

        return text;
    }
}