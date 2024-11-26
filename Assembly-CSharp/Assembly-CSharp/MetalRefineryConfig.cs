using System.Collections.Generic;
using TUNING;
using UnityEngine;

public class MetalRefineryConfig : IBuildingConfig {
    public const            string ID                         = "MetalRefinery";
    private const           float  INPUT_KG                   = 100f;
    private const           float  LIQUID_COOLED_HEAT_PORTION = 0.8f;
    private const           float  COOLANT_MASS               = 400f;
    private static readonly Tag    COOLANT_TAG                = GameTags.Liquid;

    private static readonly List<Storage.StoredItemModifier> RefineryStoredItemModifiers
        = new List<Storage.StoredItemModifier> {
            Storage.StoredItemModifier.Hide,
            Storage.StoredItemModifier.Preserve,
            Storage.StoredItemModifier.Insulate,
            Storage.StoredItemModifier.Seal
        };

    public override BuildingDef CreateBuildingDef() {
        var id                  = "MetalRefinery";
        var width               = 3;
        var height              = 4;
        var anim                = "metalrefinery_kanim";
        var hitpoints           = 30;
        var construction_time   = 60f;
        var tier                = BUILDINGS.CONSTRUCTION_MASS_KG.TIER5;
        var all_MINERALS        = MATERIALS.ALL_MINERALS;
        var melting_point       = 2400f;
        var build_location_rule = BuildLocationRule.OnFloor;
        var tier2               = NOISE_POLLUTION.NOISY.TIER6;
        var buildingDef = BuildingTemplates.CreateBuildingDef(id,
                                                              width,
                                                              height,
                                                              anim,
                                                              hitpoints,
                                                              construction_time,
                                                              tier,
                                                              all_MINERALS,
                                                              melting_point,
                                                              build_location_rule,
                                                              BUILDINGS.DECOR.PENALTY.TIER2,
                                                              tier2);

        buildingDef.RequiresPowerInput          = true;
        buildingDef.EnergyConsumptionWhenActive = 1200f;
        buildingDef.SelfHeatKilowattsWhenActive = 16f;
        buildingDef.InputConduitType            = ConduitType.Liquid;
        buildingDef.UtilityInputOffset          = new CellOffset(-1, 1);
        buildingDef.OutputConduitType           = ConduitType.Liquid;
        buildingDef.UtilityOutputOffset         = new CellOffset(1, 0);
        buildingDef.ViewMode                    = OverlayModes.Power.ID;
        buildingDef.AudioCategory               = "HollowMetal";
        buildingDef.AudioSize                   = "large";
        return buildingDef;
    }

    public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag) {
        go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
        go.AddOrGet<DropAllWorkable>();
        go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
        var liquidCooledRefinery = go.AddOrGet<LiquidCooledRefinery>();
        liquidCooledRefinery.duplicantOperated = true;
        liquidCooledRefinery.sideScreenStyle   = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;
        liquidCooledRefinery.keepExcessLiquids = true;
        go.AddOrGet<FabricatorIngredientStatusManager>();
        go.AddOrGet<CopyBuildingSettings>();
        Workable workable = go.AddOrGet<ComplexFabricatorWorkable>();
        BuildingTemplates.CreateComplexFabricatorStorage(go, liquidCooledRefinery);
        liquidCooledRefinery.coolantTag            = COOLANT_TAG;
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
                    complexRecipe.time = 40f;
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
        complexRecipe2.time        = 40f;
        complexRecipe2.nameDisplay = ComplexRecipe.RecipeNameDisplay.IngredientToResult;
        complexRecipe2.description = string.Format(STRINGS.BUILDINGS.PREFABS.METALREFINERY.RECIPE_DESCRIPTION,
                                                   ElementLoader.FindElementByHash(SimHashes.Steel).name,
                                                   ElementLoader.FindElementByHash(SimHashes.Iron).name);

        complexRecipe2.fabricators = new List<Tag> { TagManager.Create("MetalRefinery") };
        ComplexRecipeManager.Get().AddObsoleteIDMapping(obsolete_id2, text2);
        Prioritizable.AddRef(go);
    }

    public override void DoPostConfigureComplete(GameObject go) {
        SymbolOverrideControllerUtil.AddToPrefab(go);
        go.AddOrGetDef<PoweredActiveStoppableController.Def>();
        go.GetComponent<KPrefabID>().prefabSpawnFn += delegate(GameObject game_object) {
                                                          var component = game_object
                                                              .GetComponent<ComplexFabricatorWorkable>();

                                                          component.WorkerStatusItem
                                                              = Db.Get().DuplicantStatusItems.Processing;

                                                          component.AttributeConverter
                                                              = Db.Get().AttributeConverters.MachinerySpeed;

                                                          component.AttributeExperienceMultiplier
                                                              = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;

                                                          component.SkillExperienceSkillGroup
                                                              = Db.Get().SkillGroups.Technicals.Id;

                                                          component.SkillExperienceMultiplier
                                                              = SKILLS.PART_DAY_EXPERIENCE;
                                                      };
    }
}