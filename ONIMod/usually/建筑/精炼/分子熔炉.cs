using System.Collections.Generic;
using HarmonyLib;
using STRINGS;
using UnityEngine;

#if 分子熔炉
[HarmonyPatch]
public class 分子熔炉 {
    [HarmonyPatch(typeof(SupermaterialRefineryConfig), "ConfigureBuildingTemplate"), HarmonyPrefix]
    public static bool fix1(GameObject go, Tag prefab_tag) {
        go.AddOrGet<DropAllWorkable>();
        go.AddOrGet<BuildingComplete>().isManuallyOperated = false;
        var complexFabricator = go.AddOrGet<ComplexFabricator>();
        complexFabricator.heatedTemperature = 313.15f;
        complexFabricator.sideScreenStyle   = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;
        complexFabricator.duplicantOperated = false;
        go.AddOrGet<FabricatorIngredientStatusManager>();
        go.AddOrGet<CopyBuildingSettings>();
        go.AddOrGet<ComplexFabricatorWorkable>();
        BuildingTemplates.CreateComplexFabricatorStorage(go, complexFabricator);
        Prioritizable.AddRef(go);
        var num  = 0.01f;
        var num2 = (1f - num) * 0.5f;
        ComplexRecipe.RecipeElement[] array = {
            new ComplexRecipe.RecipeElement(SimHashes.Fullerene.CreateTag(), 100f * num),
            new ComplexRecipe.RecipeElement(SimHashes.Gold.CreateTag(),      100f * num2),
            new ComplexRecipe.RecipeElement(SimHashes.Petroleum.CreateTag(), 100f * num2)
        };

        ComplexRecipe.RecipeElement[] array2 = {
            new ComplexRecipe.RecipeElement(SimHashes.SuperCoolant.CreateTag(),
                                            100f,
                                            ComplexRecipe.RecipeElement.TemperatureOperation.Heated)
        };

        var complexRecipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("SupermaterialRefinery", array, array2),
                                              array,
                                              array2);

        complexRecipe.time        = 3f;
        complexRecipe.description = BUILDINGS.PREFABS.SUPERMATERIALREFINERY.SUPERCOOLANT_RECIPE_DESCRIPTION;
        complexRecipe.nameDisplay = ComplexRecipe.RecipeNameDisplay.Result;
        complexRecipe.fabricators = new List<Tag> { TagManager.Create("SupermaterialRefinery") };
        if (DlcManager.IsExpansion1Active()) {
            var num3 = 0.9f;
            var num4 = 1f - num3;
            ComplexRecipe.RecipeElement[] array3 = {
                new ComplexRecipe.RecipeElement(SimHashes.Graphite.CreateTag(), 100f        * num3),
                new ComplexRecipe.RecipeElement(SimHashes.Sulfur.CreateTag(),   100f * num4 / 2f),
                new ComplexRecipe.RecipeElement(SimHashes.Aluminum.CreateTag(), 100f * num4 / 2f)
            };

            ComplexRecipe.RecipeElement[] array4 = {
                new ComplexRecipe.RecipeElement(SimHashes.Fullerene.CreateTag(),
                                                100f,
                                                ComplexRecipe.RecipeElement.TemperatureOperation.Heated)
            };

            var complexRecipe2
                = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("SupermaterialRefinery", array3, array4),
                                    array3,
                                    array4);

            complexRecipe2.time        = 3f;
            complexRecipe2.description = BUILDINGS.PREFABS.SUPERMATERIALREFINERY.FULLERENE_RECIPE_DESCRIPTION;
            complexRecipe2.nameDisplay = ComplexRecipe.RecipeNameDisplay.Result;
            complexRecipe2.fabricators = new List<Tag> { TagManager.Create("SupermaterialRefinery") };
        }

        var num5 = 0.15f;
        var num6 = 0.7f;
        var num7 = 0.15f;
        ComplexRecipe.RecipeElement[] array5 = {
            new ComplexRecipe.RecipeElement(SimHashes.TempConductorSolid.CreateTag(), 100f * num5),
            new ComplexRecipe.RecipeElement(SimHashes.Polypropylene.CreateTag(),      100f * num6),
            new ComplexRecipe.RecipeElement(SimHashes.MilkFat.CreateTag(),            100f * num7)
        };

        ComplexRecipe.RecipeElement[] array6 = {
            new ComplexRecipe.RecipeElement(SimHashes.HardPolypropylene.CreateTag(),
                                            100f,
                                            ComplexRecipe.RecipeElement.TemperatureOperation.Heated)
        };

        var complexRecipe3
            = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("SupermaterialRefinery", array5, array6),
                                array5,
                                array6);

        complexRecipe3.time        = 3f;
        complexRecipe3.description = BUILDINGS.PREFABS.SUPERMATERIALREFINERY.HARDPLASTIC_RECIPE_DESCRIPTION;
        complexRecipe3.nameDisplay = ComplexRecipe.RecipeNameDisplay.Result;
        complexRecipe3.fabricators = new List<Tag> { TagManager.Create("SupermaterialRefinery") };
        var num8  = 0.15f;
        var num9  = 0.05f;
        var num10 = 1f - num9 - num8;
        ComplexRecipe.RecipeElement[] array7 = {
            new ComplexRecipe.RecipeElement(SimHashes.Isoresin.CreateTag(),  100f * num8),
            new ComplexRecipe.RecipeElement(SimHashes.Katairite.CreateTag(), 100f * num10),
            // new ComplexRecipe.RecipeElement(BasicFabricConfig.ID.ToTag(),    100f * num9)
        };

        ComplexRecipe.RecipeElement[] array8 = {
            new ComplexRecipe.RecipeElement(SimHashes.SuperInsulator.CreateTag(),
                                            1000f,
                                            ComplexRecipe.RecipeElement.TemperatureOperation.Heated)
        };

        var complexRecipe4
            = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("SupermaterialRefinery", array7, array8),
                                array7,
                                array8);

        complexRecipe4.time        = 3f;
        complexRecipe4.description = BUILDINGS.PREFABS.SUPERMATERIALREFINERY.SUPERINSULATOR_RECIPE_DESCRIPTION;
        complexRecipe4.nameDisplay = ComplexRecipe.RecipeNameDisplay.Result;
        complexRecipe4.fabricators = new List<Tag> { TagManager.Create("SupermaterialRefinery") };
        var num11 = 0.05f;
        ComplexRecipe.RecipeElement[] array9 = {
            new ComplexRecipe.RecipeElement(SimHashes.Niobium.CreateTag(),  100f * num11),
            new ComplexRecipe.RecipeElement(SimHashes.Tungsten.CreateTag(), 100f * (1f - num11))
        };

        ComplexRecipe.RecipeElement[] array10 = {
            new ComplexRecipe.RecipeElement(SimHashes.TempConductorSolid.CreateTag(),
                                            1000f,
                                            ComplexRecipe.RecipeElement.TemperatureOperation.Heated)
        };

        var complexRecipe5
            = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("SupermaterialRefinery", array9, array10),
                                array9,
                                array10);

        complexRecipe5.time        = 3f;
        complexRecipe5.description = BUILDINGS.PREFABS.SUPERMATERIALREFINERY.TEMPCONDUCTORSOLID_RECIPE_DESCRIPTION;
        complexRecipe5.nameDisplay = ComplexRecipe.RecipeNameDisplay.Result;
        complexRecipe5.fabricators = new List<Tag> { TagManager.Create("SupermaterialRefinery") };
        var num12 = 0.35f;
        ComplexRecipe.RecipeElement[] array11 = {
            new ComplexRecipe.RecipeElement(SimHashes.Isoresin.CreateTag(), 100f * num12)

            // new ComplexRecipe.RecipeElement(SimHashes.Petroleum.CreateTag(), 100f * (1f - num12))
        };

        ComplexRecipe.RecipeElement[] array12 = {
            new ComplexRecipe.RecipeElement(SimHashes.ViscoGel.CreateTag(),
                                            1000f,
                                            ComplexRecipe.RecipeElement.TemperatureOperation.Heated)
        };

        var complexRecipe6
            = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("SupermaterialRefinery", array11, array12),
                                array11,
                                array12);

        complexRecipe6.time        = 3f;
        complexRecipe6.description = BUILDINGS.PREFABS.SUPERMATERIALREFINERY.VISCOGEL_RECIPE_DESCRIPTION;
        complexRecipe6.nameDisplay = ComplexRecipe.RecipeNameDisplay.Result;
        complexRecipe6.fabricators = new List<Tag> { TagManager.Create("SupermaterialRefinery") };

        return false;
    }
}
#endif