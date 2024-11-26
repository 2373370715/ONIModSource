using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;

#if 金属精炼器
[HarmonyPatch]
public class 金属精炼机 {
    [HarmonyPatch(typeof(MetalRefineryConfig), "ConfigureBuildingTemplate"), HarmonyPrefix]
    public static bool Prefix(GameObject go) {
        var RefineryStoredItemModifiers = new List<Storage.StoredItemModifier> {
            Storage.StoredItemModifier.Hide,
            Storage.StoredItemModifier.Preserve,
            Storage.StoredItemModifier.Insulate,
            Storage.StoredItemModifier.Seal
        };

        go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
        go.AddOrGet<DropAllWorkable>();
        go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
        var liquidCooledRefinery = go.AddOrGet<LiquidCooledRefinery>();
        liquidCooledRefinery.duplicantOperated = false;
        liquidCooledRefinery.sideScreenStyle   = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;
        liquidCooledRefinery.keepExcessLiquids = true;
        go.AddOrGet<FabricatorIngredientStatusManager>();
        go.AddOrGet<CopyBuildingSettings>();
        Workable workable = go.AddOrGet<ComplexFabricatorWorkable>();
        BuildingTemplates.CreateComplexFabricatorStorage(go, liquidCooledRefinery);
        liquidCooledRefinery.coolantTag            = GameTags.Liquid;
        liquidCooledRefinery.minCoolantMass        = 400f;
        liquidCooledRefinery.outStorage.capacityKg = 2000f;
        liquidCooledRefinery.thermalFudge          = 0.8f;
        liquidCooledRefinery.inStorage.SetDefaultStoredItemModifiers(RefineryStoredItemModifiers);
        liquidCooledRefinery.buildStorage.SetDefaultStoredItemModifiers(RefineryStoredItemModifiers);
        liquidCooledRefinery.outStorage.SetDefaultStoredItemModifiers(RefineryStoredItemModifiers);
        liquidCooledRefinery.outputOffset            = new Vector3(1f, 0.5f);
        workable.overrideAnims                       = new[] { Assets.GetAnim("anim_interacts_metalrefinery_kanim") };
        go.AddOrGet<RequireOutputs>().ignoreFullPipe = true;
        var conduitConsumer = go.AddOrGet<ConduitConsumer>();
        conduitConsumer.capacityTag          = GameTags.Liquid;
        conduitConsumer.capacityKG           = 800f;
        conduitConsumer.storage              = liquidCooledRefinery.inStorage;
        conduitConsumer.alwaysConsume        = true;
        conduitConsumer.forceAlwaysSatisfied = true;
        var conduitDispenser = go.AddOrGet<ConduitDispenser>();
        conduitDispenser.storage        = liquidCooledRefinery.outStorage;
        conduitDispenser.conduitType    = ConduitType.Liquid;
        conduitDispenser.elementFilter  = null;
        conduitDispenser.alwaysDispense = true;
        foreach (var element in ElementLoader.elements.FindAll(e => e.IsSolid && e.HasTag(GameTags.Metal)))
            if (!element.HasTag(GameTags.Noncrushable)) {
                var lowTempTransition = element.highTempTransition.lowTempTransition;
                if (lowTempTransition != element) {
                    ComplexRecipe.RecipeElement[] array = { new ComplexRecipe.RecipeElement(element.tag, 100f) };
                    ComplexRecipe.RecipeElement[] array2 = {
                        new ComplexRecipe.RecipeElement(lowTempTransition.tag,
                                                        100f,
                                                        ComplexRecipe.RecipeElement.TemperatureOperation
                                                                     .AverageTemperature)
                    };

                    var obsolete_id   = ComplexRecipeManager.MakeObsoleteRecipeID("MetalRefinery", element.tag);
                    var text          = ComplexRecipeManager.MakeRecipeID("MetalRefinery", array, array2);
                    var complexRecipe = new ComplexRecipe(text, array, array2);
                    complexRecipe.time = 4f;
                    complexRecipe.description
                        = string.Format(STRINGS.BUILDINGS.PREFABS.METALREFINERY.RECIPE_DESCRIPTION,
                                        lowTempTransition.name,
                                        element.name);

                    complexRecipe.nameDisplay = ComplexRecipe.RecipeNameDisplay.IngredientToResult;
                    complexRecipe.fabricators = new List<Tag> { TagManager.Create("MetalRefinery") };
                    ComplexRecipeManager.Get().AddObsoleteIDMapping(obsolete_id, text);
                }
            }

        var element2 = ElementLoader.FindElementByHash(SimHashes.Steel);
        ComplexRecipe.RecipeElement[] array3 = {
            new ComplexRecipe.RecipeElement(ElementLoader.FindElementByHash(SimHashes.Iron).tag,          70f),
            new ComplexRecipe.RecipeElement(ElementLoader.FindElementByHash(SimHashes.RefinedCarbon).tag, 20f),
            new ComplexRecipe.RecipeElement(ElementLoader.FindElementByHash(SimHashes.Lime).tag,          10f)
        };

        ComplexRecipe.RecipeElement[] array4 = {
            new ComplexRecipe.RecipeElement(ElementLoader.FindElementByHash(SimHashes.Steel).tag,
                                            100f,
                                            ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature)
        };

        var obsolete_id2   = ComplexRecipeManager.MakeObsoleteRecipeID("MetalRefinery", element2.tag);
        var text2          = ComplexRecipeManager.MakeRecipeID("MetalRefinery", array3, array4);
        var complexRecipe2 = new ComplexRecipe(text2, array3, array4);
        complexRecipe2.time        = 4f;
        complexRecipe2.nameDisplay = ComplexRecipe.RecipeNameDisplay.IngredientToResult;
        complexRecipe2.description = string.Format(STRINGS.BUILDINGS.PREFABS.METALREFINERY.RECIPE_DESCRIPTION,
                                                   ElementLoader.FindElementByHash(SimHashes.Steel).name,
                                                   ElementLoader.FindElementByHash(SimHashes.Iron).name);

        complexRecipe2.fabricators = new List<Tag> { TagManager.Create("MetalRefinery") };
        ComplexRecipeManager.Get().AddObsoleteIDMapping(obsolete_id2, text2);
        Prioritizable.AddRef(go);
        return false;
    }
}
#endif