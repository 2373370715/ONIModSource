using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;
using BUILDINGS = TUNING.BUILDINGS;

public class RockCrusherConfig : IBuildingConfig {
    public const  string ID                   = "RockCrusher";
    private const float  INPUT_KG             = 100f;
    private const float  METAL_ORE_EFFICIENCY = 0.5f;

    public override BuildingDef CreateBuildingDef() {
        var id                  = "RockCrusher";
        var width               = 4;
        var height              = 4;
        var anim                = "rockrefinery_kanim";
        var hitpoints           = 30;
        var construction_time   = 60f;
        var tier                = BUILDINGS.CONSTRUCTION_MASS_KG.TIER5;
        var all_METALS          = MATERIALS.ALL_METALS;
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
                                                              all_METALS,
                                                              melting_point,
                                                              build_location_rule,
                                                              BUILDINGS.DECOR.PENALTY.TIER2,
                                                              tier2);

        buildingDef.RequiresPowerInput          = true;
        buildingDef.EnergyConsumptionWhenActive = 240f;
        buildingDef.SelfHeatKilowattsWhenActive = 16f;
        buildingDef.ViewMode                    = OverlayModes.Power.ID;
        buildingDef.AudioCategory               = "HollowMetal";
        buildingDef.AudioSize                   = "large";
        return buildingDef;
    }

    public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag) {
        go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
        go.AddOrGet<DropAllWorkable>();
        go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
        var complexFabricator = go.AddOrGet<ComplexFabricator>();
        complexFabricator.sideScreenStyle   = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;
        complexFabricator.duplicantOperated = true;
        go.AddOrGet<FabricatorIngredientStatusManager>();
        go.AddOrGet<CopyBuildingSettings>();
        var complexFabricatorWorkable = go.AddOrGet<ComplexFabricatorWorkable>();
        BuildingTemplates.CreateComplexFabricatorStorage(go, complexFabricator);
        complexFabricatorWorkable.overrideAnims      = new[] { Assets.GetAnim("anim_interacts_rockrefinery_kanim") };
        complexFabricatorWorkable.workingPstComplete = new HashedString[] { "working_pst_complete" };
        var tag = SimHashes.Sand.CreateTag();
        foreach (var element in ElementLoader.elements.FindAll(e => e.HasTag(GameTags.Crushable))) {
            ComplexRecipe.RecipeElement[] array = { new ComplexRecipe.RecipeElement(element.tag, 100f) };
            ComplexRecipe.RecipeElement[] array2 = { new ComplexRecipe.RecipeElement(tag, 100f) };
            var obsolete_id = ComplexRecipeManager.MakeObsoleteRecipeID("RockCrusher", element.tag);
            var text = ComplexRecipeManager.MakeRecipeID("RockCrusher", array, array2);
            var complexRecipe = new ComplexRecipe(text, array, array2);
            complexRecipe.time = 40f;
            complexRecipe.description = string.Format(STRINGS.BUILDINGS.PREFABS.ROCKCRUSHER.RECIPE_DESCRIPTION,
                                                      element.name,
                                                      tag.ProperName());

            complexRecipe.nameDisplay = ComplexRecipe.RecipeNameDisplay.IngredientToResult;
            complexRecipe.fabricators = new List<Tag> { TagManager.Create("RockCrusher") };
            ComplexRecipeManager.Get().AddObsoleteIDMapping(obsolete_id, text);
        }

        foreach (var element2 in ElementLoader.elements.FindAll(e => e.IsSolid && e.HasTag(GameTags.Metal)))
            if (!element2.HasTag(GameTags.Noncrushable)) {
                var lowTempTransition = element2.highTempTransition.lowTempTransition;
                if (lowTempTransition != element2) {
                    ComplexRecipe.RecipeElement[] array3 = { new ComplexRecipe.RecipeElement(element2.tag, 100f) };
                    ComplexRecipe.RecipeElement[] array4 = {
                        new ComplexRecipe.RecipeElement(lowTempTransition.tag, 50f),
                        new ComplexRecipe.RecipeElement(tag,
                                                        50f,
                                                        ComplexRecipe.RecipeElement.TemperatureOperation
                                                                     .AverageTemperature)
                    };

                    var obsolete_id2 = ComplexRecipeManager.MakeObsoleteRecipeID("RockCrusher", lowTempTransition.tag);
                    var text2 = ComplexRecipeManager.MakeRecipeID("RockCrusher", array3, array4);
                    var complexRecipe2 = new ComplexRecipe(text2, array3, array4);
                    complexRecipe2.time = 40f;
                    complexRecipe2.description
                        = string.Format(STRINGS.BUILDINGS.PREFABS.ROCKCRUSHER.METAL_RECIPE_DESCRIPTION,
                                        lowTempTransition.name,
                                        element2.name);

                    complexRecipe2.nameDisplay = ComplexRecipe.RecipeNameDisplay.IngredientToResult;
                    complexRecipe2.fabricators = new List<Tag> { TagManager.Create("RockCrusher") };
                    ComplexRecipeManager.Get().AddObsoleteIDMapping(obsolete_id2, text2);
                }
            }

        var                           element3 = ElementLoader.FindElementByHash(SimHashes.Lime);
        ComplexRecipe.RecipeElement[] array5   = { new ComplexRecipe.RecipeElement("EggShell", 5f) };
        ComplexRecipe.RecipeElement[] array6 = {
            new ComplexRecipe.RecipeElement(ElementLoader.FindElementByHash(SimHashes.Lime).tag,
                                            5f,
                                            ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature)
        };

        var obsolete_id3   = ComplexRecipeManager.MakeObsoleteRecipeID("RockCrusher", element3.tag);
        var text3          = ComplexRecipeManager.MakeRecipeID("RockCrusher", array5, array6);
        var complexRecipe3 = new ComplexRecipe(text3, array5, array6);
        complexRecipe3.time = 40f;
        complexRecipe3.description = string.Format(STRINGS.BUILDINGS.PREFABS.ROCKCRUSHER.LIME_RECIPE_DESCRIPTION,
                                                   SimHashes.Lime.CreateTag().ProperName(),
                                                   MISC.TAGS.EGGSHELL);

        complexRecipe3.nameDisplay = ComplexRecipe.RecipeNameDisplay.IngredientToResult;
        complexRecipe3.fabricators = new List<Tag> { TagManager.Create("RockCrusher") };
        ComplexRecipeManager.Get().AddObsoleteIDMapping(obsolete_id3, text3);
        var                           element4 = ElementLoader.FindElementByHash(SimHashes.Lime);
        ComplexRecipe.RecipeElement[] array7   = { new ComplexRecipe.RecipeElement("BabyCrabShell", 1f) };
        ComplexRecipe.RecipeElement[] array8 = {
            new ComplexRecipe.RecipeElement(element4.tag,
                                            5f,
                                            ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature)
        };

        var complexRecipe4 = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("RockCrusher", array7, array8),
                                               array7,
                                               array8);

        complexRecipe4.time = 40f;
        complexRecipe4.description = string.Format(STRINGS.BUILDINGS.PREFABS.ROCKCRUSHER.LIME_RECIPE_DESCRIPTION,
                                                   SimHashes.Lime.CreateTag().ProperName(),
                                                   ITEMS.INDUSTRIAL_PRODUCTS.CRAB_SHELL.NAME);

        complexRecipe4.nameDisplay = ComplexRecipe.RecipeNameDisplay.IngredientToResult;
        complexRecipe4.fabricators = new List<Tag> { TagManager.Create("RockCrusher") };
        var                           element5 = ElementLoader.FindElementByHash(SimHashes.Lime);
        ComplexRecipe.RecipeElement[] array9   = { new ComplexRecipe.RecipeElement("CrabShell", 1f) };
        ComplexRecipe.RecipeElement[] array10 = {
            new ComplexRecipe.RecipeElement(element5.tag,
                                            10f,
                                            ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature)
        };

        var complexRecipe5 = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("RockCrusher", array9, array10),
                                               array9,
                                               array10);

        complexRecipe5.time = 40f;
        complexRecipe5.description = string.Format(STRINGS.BUILDINGS.PREFABS.ROCKCRUSHER.LIME_RECIPE_DESCRIPTION,
                                                   SimHashes.Lime.CreateTag().ProperName(),
                                                   ITEMS.INDUSTRIAL_PRODUCTS.CRAB_SHELL.NAME);

        complexRecipe5.nameDisplay = ComplexRecipe.RecipeNameDisplay.IngredientToResult;
        complexRecipe5.fabricators = new List<Tag> { TagManager.Create("RockCrusher") };
        ComplexRecipe.RecipeElement[] array11 = { new ComplexRecipe.RecipeElement("BabyCrabWoodShell", 1f) };
        ComplexRecipe.RecipeElement[] array12 = {
            new ComplexRecipe.RecipeElement("WoodLog",
                                            10f,
                                            ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature)
        };

        var complexRecipe6 = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("RockCrusher", array11, array12),
                                               array11,
                                               array12);

        complexRecipe6.time = 40f;
        complexRecipe6.description = string.Format(STRINGS.BUILDINGS.PREFABS.ROCKCRUSHER.LIME_RECIPE_DESCRIPTION,
                                                   WoodLogConfig.TAG.ProperName(),
                                                   ITEMS.INDUSTRIAL_PRODUCTS.BABY_CRAB_SHELL.VARIANT_WOOD.NAME);

        complexRecipe6.nameDisplay = ComplexRecipe.RecipeNameDisplay.IngredientToResult;
        complexRecipe6.fabricators = new List<Tag> { TagManager.Create("RockCrusher") };
        var                           num     = 5f;
        ComplexRecipe.RecipeElement[] array13 = { new ComplexRecipe.RecipeElement("CrabWoodShell", num) };
        ComplexRecipe.RecipeElement[] array14 = {
            new ComplexRecipe.RecipeElement("WoodLog",
                                            100f * num,
                                            ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature)
        };

        var complexRecipe7 = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("RockCrusher", array13, array14),
                                               array13,
                                               array14);

        complexRecipe7.time = 40f;
        complexRecipe7.description = string.Format(STRINGS.BUILDINGS.PREFABS.ROCKCRUSHER.LIME_RECIPE_DESCRIPTION,
                                                   WoodLogConfig.TAG.ProperName(),
                                                   ITEMS.INDUSTRIAL_PRODUCTS.CRAB_SHELL.VARIANT_WOOD.NAME);

        complexRecipe7.nameDisplay = ComplexRecipe.RecipeNameDisplay.IngredientToResult;
        complexRecipe7.fabricators = new List<Tag> { TagManager.Create("RockCrusher") };
        ComplexRecipe.RecipeElement[] array15 = {
            new ComplexRecipe.RecipeElement(ElementLoader.FindElementByHash(SimHashes.Fossil).tag, 100f)
        };

        ComplexRecipe.RecipeElement[] array16 = {
            new ComplexRecipe.RecipeElement(ElementLoader.FindElementByHash(SimHashes.Lime).tag,
                                            5f,
                                            ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature),
            new ComplexRecipe.RecipeElement(ElementLoader.FindElementByHash(SimHashes.SedimentaryRock).tag,
                                            95f,
                                            ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature)
        };

        var complexRecipe8 = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("RockCrusher", array15, array16),
                                               array15,
                                               array16);

        complexRecipe8.time = 40f;
        complexRecipe8.description
            = string.Format(STRINGS.BUILDINGS.PREFABS.ROCKCRUSHER.LIME_FROM_LIMESTONE_RECIPE_DESCRIPTION,
                            SimHashes.Fossil.CreateTag().ProperName(),
                            SimHashes.SedimentaryRock.CreateTag().ProperName(),
                            SimHashes.Lime.CreateTag().ProperName());

        complexRecipe8.nameDisplay = ComplexRecipe.RecipeNameDisplay.IngredientToResult;
        complexRecipe8.fabricators = new List<Tag> { TagManager.Create("RockCrusher") };
        var                           num2    = 5E-05f;
        ComplexRecipe.RecipeElement[] array17 = { new ComplexRecipe.RecipeElement(SimHashes.Salt.CreateTag(), 100f) };
        ComplexRecipe.RecipeElement[] array18 = {
            new ComplexRecipe.RecipeElement(TableSaltConfig.ID.ToTag(),
                                            100f * num2,
                                            ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature),
            new ComplexRecipe.RecipeElement(SimHashes.Sand.CreateTag(),
                                            100f * (1f - num2),
                                            ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature)
        };

        var complexRecipe9 = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("RockCrusher", array17, array18),
                                               array17,
                                               array18);

        complexRecipe9.time = 40f;
        complexRecipe9.description = string.Format(STRINGS.BUILDINGS.PREFABS.ROCKCRUSHER.RECIPE_DESCRIPTION,
                                                   SimHashes.Salt.CreateTag().ProperName(),
                                                   ITEMS.INDUSTRIAL_PRODUCTS.TABLE_SALT.NAME);

        complexRecipe9.nameDisplay = ComplexRecipe.RecipeNameDisplay.IngredientToResult;
        complexRecipe9.fabricators = new List<Tag> { TagManager.Create("RockCrusher") };
        if (ElementLoader.FindElementByHash(SimHashes.Graphite) != null) {
            var num3 = 0.9f;
            ComplexRecipe.RecipeElement[] array19 = {
                new ComplexRecipe.RecipeElement(SimHashes.Fullerene.CreateTag(), 100f)
            };

            ComplexRecipe.RecipeElement[] array20 = {
                new ComplexRecipe.RecipeElement(SimHashes.Graphite.CreateTag(),
                                                100f * num3,
                                                ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature),
                new ComplexRecipe.RecipeElement(SimHashes.Sand.CreateTag(),
                                                100f * (1f - num3),
                                                ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature)
            };

            var complexRecipe10 = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("RockCrusher", array19, array20),
                                                    array19,
                                                    array20,
                                                    DlcManager.AVAILABLE_EXPANSION1_ONLY);

            complexRecipe10.time = 40f;
            complexRecipe10.description = string.Format(STRINGS.BUILDINGS.PREFABS.ROCKCRUSHER.RECIPE_DESCRIPTION,
                                                        SimHashes.Fullerene.CreateTag().ProperName(),
                                                        SimHashes.Graphite.CreateTag().ProperName());

            complexRecipe10.nameDisplay = ComplexRecipe.RecipeNameDisplay.IngredientToResult;
            complexRecipe10.fabricators = new List<Tag> { TagManager.Create("RockCrusher") };
        }

        var                           num4    = 120f;
        var                           num5    = num4 * 0.2667f;
        ComplexRecipe.RecipeElement[] array21 = { new ComplexRecipe.RecipeElement("IceBellyPoop", num4) };
        ComplexRecipe.RecipeElement[] array22 = {
            new ComplexRecipe.RecipeElement(SimHashes.Phosphorite.CreateTag(),
                                            num5,
                                            ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature),
            new ComplexRecipe.RecipeElement(SimHashes.Clay.CreateTag(),
                                            num4 - num5,
                                            ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature)
        };

        var complexRecipe11 = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("RockCrusher", array21, array22),
                                                array21,
                                                array22,
                                                DlcManager.AVAILABLE_DLC_2);

        complexRecipe11.time = 40f;
        complexRecipe11.description = string.Format(STRINGS.BUILDINGS.PREFABS.ROCKCRUSHER.RECIPE_DESCRIPTION_TWO_OUTPUT,
                                                    ITEMS.INDUSTRIAL_PRODUCTS.ICE_BELLY_POOP.NAME,
                                                    SimHashes.Phosphorite.CreateTag().ProperName(),
                                                    SimHashes.Clay.CreateTag().ProperName());

        complexRecipe11.nameDisplay = ComplexRecipe.RecipeNameDisplay.Ingredient;
        complexRecipe11.fabricators = new List<Tag> { TagManager.Create("RockCrusher") };
        Prioritizable.AddRef(go);
    }

    public override void DoPostConfigureComplete(GameObject go) {
        SymbolOverrideControllerUtil.AddToPrefab(go);
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