using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;
using TUNING;

#if 太空服锻造台
[HarmonyPatch]
public class 太空服锻造台 {
    [HarmonyPatch(typeof(SuitFabricatorConfig), "ConfigureBuildingTemplate"), HarmonyPostfix]
    public static void Postfix(GameObject go) {
        go.AddOrGet<ComplexFabricator>().duplicantOperated = false;
    }

    [HarmonyPatch(typeof(SuitFabricatorConfig), "ConfigureRecipes"), HarmonyPrefix]
    public static bool 材料修改() {
        ConfigureRecipes();
        return false;
    }
    
    private static void ConfigureRecipes() {
        ComplexRecipe.RecipeElement[] array = {
            new ComplexRecipe.RecipeElement(SimHashes.Copper.CreateTag(), 300f, true),
            new ComplexRecipe.RecipeElement("BasicFabric".ToTag(),        0f)
        };

        ComplexRecipe.RecipeElement[] array2 = {
            new ComplexRecipe.RecipeElement("Atmo_Suit".ToTag(),
                                            1f,
                                            ComplexRecipe.RecipeElement.TemperatureOperation.Heated)
        };

        AtmoSuitConfig.recipe
            = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("SuitFabricator", array, array2), array, array2) {
                time         = EQUIPMENT.SUITS.ATMOSUIT_FABTIME,
                description  = STRINGS.EQUIPMENT.PREFABS.ATMO_SUIT.RECIPE_DESC,
                nameDisplay  = ComplexRecipe.RecipeNameDisplay.ResultWithIngredient,
                fabricators  = new List<Tag> { "SuitFabricator" },
                requiredTech = Db.Get().TechItems.atmoSuit.parentTechId,
                sortOrder    = 1
            };

        ComplexRecipe.RecipeElement[] array3 = {
            new ComplexRecipe.RecipeElement(SimHashes.Aluminum.CreateTag(), 300f, true),
            new ComplexRecipe.RecipeElement("BasicFabric".ToTag(),          0f)
        };

        ComplexRecipe.RecipeElement[] array4 = {
            new ComplexRecipe.RecipeElement("Atmo_Suit".ToTag(),
                                            1f,
                                            ComplexRecipe.RecipeElement.TemperatureOperation.Heated)
        };

        AtmoSuitConfig.recipe
            = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("SuitFabricator", array3, array4), array3, array4) {
                time         = EQUIPMENT.SUITS.ATMOSUIT_FABTIME,
                description  = STRINGS.EQUIPMENT.PREFABS.ATMO_SUIT.RECIPE_DESC,
                nameDisplay  = ComplexRecipe.RecipeNameDisplay.ResultWithIngredient,
                fabricators  = new List<Tag> { "SuitFabricator" },
                requiredTech = Db.Get().TechItems.atmoSuit.parentTechId,
                sortOrder    = 1
            };

        ComplexRecipe.RecipeElement[] array5 = {
            new ComplexRecipe.RecipeElement(SimHashes.Iron.CreateTag(), 300f, true),
            new ComplexRecipe.RecipeElement("BasicFabric".ToTag(),      0f)
        };

        ComplexRecipe.RecipeElement[] array6 = {
            new ComplexRecipe.RecipeElement("Atmo_Suit".ToTag(),
                                            1f,
                                            ComplexRecipe.RecipeElement.TemperatureOperation.Heated)
        };

        AtmoSuitConfig.recipe
            = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("SuitFabricator", array5, array6), array5, array6) {
                time         = EQUIPMENT.SUITS.ATMOSUIT_FABTIME,
                description  = STRINGS.EQUIPMENT.PREFABS.ATMO_SUIT.RECIPE_DESC,
                nameDisplay  = ComplexRecipe.RecipeNameDisplay.ResultWithIngredient,
                fabricators  = new List<Tag> { "SuitFabricator" },
                requiredTech = Db.Get().TechItems.atmoSuit.parentTechId,
                sortOrder    = 1
            };

        if (ElementLoader.FindElementByHash(SimHashes.Cobalt) != null) {
            ComplexRecipe.RecipeElement[] array7 = {
                new ComplexRecipe.RecipeElement(SimHashes.Cobalt.CreateTag(), 300f, true),
                new ComplexRecipe.RecipeElement("BasicFabric".ToTag(),        0f)
            };

            ComplexRecipe.RecipeElement[] array8 = {
                new ComplexRecipe.RecipeElement("Atmo_Suit".ToTag(),
                                                1f,
                                                ComplexRecipe.RecipeElement.TemperatureOperation.Heated)
            };

            AtmoSuitConfig.recipe
                = new
                    ComplexRecipe(ComplexRecipeManager.MakeRecipeID("SuitFabricator", array7, array8), array7, array8) {
                        time         = EQUIPMENT.SUITS.ATMOSUIT_FABTIME,
                        description  = STRINGS.EQUIPMENT.PREFABS.ATMO_SUIT.RECIPE_DESC,
                        nameDisplay  = ComplexRecipe.RecipeNameDisplay.ResultWithIngredient,
                        fabricators  = new List<Tag> { "SuitFabricator" },
                        requiredTech = Db.Get().TechItems.atmoSuit.parentTechId,
                        sortOrder    = 1
                    };
        }

        ComplexRecipe.RecipeElement[] array9 = {
            new ComplexRecipe.RecipeElement("Worn_Atmo_Suit".ToTag(), 1f, true),
            new ComplexRecipe.RecipeElement("BasicFabric".ToTag(),    0f)
        };

        ComplexRecipe.RecipeElement[] array10 = {
            new ComplexRecipe.RecipeElement("Atmo_Suit".ToTag(),
                                            1f,
                                            ComplexRecipe.RecipeElement.TemperatureOperation.Heated)
        };

        AtmoSuitConfig.recipe
            = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("SuitFabricator", array9, array10), array9, array10) {
                time         = EQUIPMENT.SUITS.ATMOSUIT_FABTIME,
                description  = STRINGS.EQUIPMENT.PREFABS.ATMO_SUIT.REPAIR_WORN_DESC,
                nameDisplay  = ComplexRecipe.RecipeNameDisplay.Custom,
                fabricators  = new List<Tag> { "SuitFabricator" },
                requiredTech = Db.Get().TechItems.atmoSuit.parentTechId,
                sortOrder    = 2
            };

        AtmoSuitConfig.recipe.customName       = STRINGS.EQUIPMENT.PREFABS.ATMO_SUIT.REPAIR_WORN_RECIPE_NAME;
        AtmoSuitConfig.recipe.ProductHasFacade = true;
        ComplexRecipe.RecipeElement[] array11 = {
            new ComplexRecipe.RecipeElement(SimHashes.Steel.ToString(),     200f),
            new ComplexRecipe.RecipeElement(SimHashes.Petroleum.ToString(), 25f)
        };

        ComplexRecipe.RecipeElement[] array12 = {
            new ComplexRecipe.RecipeElement("Jet_Suit".ToTag(),
                                            1f,
                                            ComplexRecipe.RecipeElement.TemperatureOperation.Heated)
        };

        JetSuitConfig.recipe
            = new
                ComplexRecipe(ComplexRecipeManager.MakeRecipeID("SuitFabricator", array11, array12), array11, array12) {
                    time         = EQUIPMENT.SUITS.ATMOSUIT_FABTIME,
                    description  = STRINGS.EQUIPMENT.PREFABS.JET_SUIT.RECIPE_DESC,
                    nameDisplay  = ComplexRecipe.RecipeNameDisplay.ResultWithIngredient,
                    fabricators  = new List<Tag> { "SuitFabricator" },
                    requiredTech = Db.Get().TechItems.jetSuit.parentTechId,
                    sortOrder    = 3
                };

        ComplexRecipe.RecipeElement[] array13 = {
            new ComplexRecipe.RecipeElement("Worn_Jet_Suit".ToTag(), 1f),
            new ComplexRecipe.RecipeElement("BasicFabric".ToTag(),   0f)
        };

        ComplexRecipe.RecipeElement[] array14 = {
            new ComplexRecipe.RecipeElement("Jet_Suit".ToTag(),
                                            1f,
                                            ComplexRecipe.RecipeElement.TemperatureOperation.Heated)
        };

        JetSuitConfig.recipe
            = new
                ComplexRecipe(ComplexRecipeManager.MakeRecipeID("SuitFabricator", array13, array14), array13, array14) {
                    time         = EQUIPMENT.SUITS.ATMOSUIT_FABTIME,
                    description  = STRINGS.EQUIPMENT.PREFABS.JET_SUIT.RECIPE_DESC,
                    nameDisplay  = ComplexRecipe.RecipeNameDisplay.ResultWithIngredient,
                    fabricators  = new List<Tag> { "SuitFabricator" },
                    requiredTech = Db.Get().TechItems.jetSuit.parentTechId,
                    sortOrder    = 4
                };

        if (DlcManager.FeatureRadiationEnabled()) {
            ComplexRecipe.RecipeElement[] array15 = {
                new ComplexRecipe.RecipeElement(SimHashes.Lead.ToString(),  200f),
                new ComplexRecipe.RecipeElement(SimHashes.Glass.ToString(), 10f)
            };

            ComplexRecipe.RecipeElement[] array16 = {
                new ComplexRecipe.RecipeElement("Lead_Suit".ToTag(),
                                                1f,
                                                ComplexRecipe.RecipeElement.TemperatureOperation.Heated)
            };

            LeadSuitConfig.recipe
                = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("SuitFabricator", array15, array16),
                                    array15,
                                    array16) {
                    time         = EQUIPMENT.SUITS.ATMOSUIT_FABTIME,
                    description  = STRINGS.EQUIPMENT.PREFABS.LEAD_SUIT.RECIPE_DESC,
                    nameDisplay  = ComplexRecipe.RecipeNameDisplay.ResultWithIngredient,
                    fabricators  = new List<Tag> { "SuitFabricator" },
                    requiredTech = Db.Get().TechItems.leadSuit.parentTechId,
                    sortOrder    = 5
                };
        }

        if (DlcManager.FeatureRadiationEnabled()) {
            ComplexRecipe.RecipeElement[] array17 = {
                new ComplexRecipe.RecipeElement("Worn_Lead_Suit".ToTag(),   1f),
                new ComplexRecipe.RecipeElement(SimHashes.Glass.ToString(), 5f)
            };

            ComplexRecipe.RecipeElement[] array18 = {
                new ComplexRecipe.RecipeElement("Lead_Suit".ToTag(),
                                                1f,
                                                ComplexRecipe.RecipeElement.TemperatureOperation.Heated)
            };

            LeadSuitConfig.recipe
                = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("SuitFabricator", array17, array18),
                                    array17,
                                    array18) {
                    time         = EQUIPMENT.SUITS.ATMOSUIT_FABTIME,
                    description  = STRINGS.EQUIPMENT.PREFABS.LEAD_SUIT.RECIPE_DESC,
                    nameDisplay  = ComplexRecipe.RecipeNameDisplay.ResultWithIngredient,
                    fabricators  = new List<Tag> { "SuitFabricator" },
                    requiredTech = Db.Get().TechItems.leadSuit.parentTechId,
                    sortOrder    = 6
                };
        }
    }
}
#endif