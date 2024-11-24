﻿using System.Collections.Generic;
using TUNING;
using UnityEngine;

public class SuitFabricatorConfig : IBuildingConfig {
    public const string ID = "SuitFabricator";

    public override BuildingDef CreateBuildingDef() {
        var id                  = "SuitFabricator";
        var width               = 4;
        var height              = 3;
        var anim                = "suit_maker_kanim";
        var hitpoints           = 100;
        var construction_time   = 240f;
        var tier                = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
        var refined_METALS      = MATERIALS.REFINED_METALS;
        var melting_point       = 800f;
        var build_location_rule = BuildLocationRule.OnFloor;
        var tier2               = NOISE_POLLUTION.NOISY.TIER3;
        var buildingDef = BuildingTemplates.CreateBuildingDef(id,
                                                              width,
                                                              height,
                                                              anim,
                                                              hitpoints,
                                                              construction_time,
                                                              tier,
                                                              refined_METALS,
                                                              melting_point,
                                                              build_location_rule,
                                                              BUILDINGS.DECOR.NONE,
                                                              tier2);

        buildingDef.RequiresPowerInput          = true;
        buildingDef.EnergyConsumptionWhenActive = 480f;
        buildingDef.ViewMode                    = OverlayModes.Power.ID;
        buildingDef.AudioCategory               = "Metal";
        buildingDef.PowerInputOffset            = new CellOffset(1, 0);
        return buildingDef;
    }

    public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag) {
        go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
        go.AddOrGet<DropAllWorkable>();
        go.AddOrGet<Prioritizable>();
        var complexFabricator = go.AddOrGet<ComplexFabricator>();
        complexFabricator.heatedTemperature = 318.15f;
        complexFabricator.sideScreenStyle   = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;
        go.AddOrGet<FabricatorIngredientStatusManager>();
        go.AddOrGet<CopyBuildingSettings>();
        go.AddOrGet<ComplexFabricatorWorkable>().overrideAnims
            = new[] { Assets.GetAnim("anim_interacts_suit_fabricator_kanim") };

        Prioritizable.AddRef(go);
        BuildingTemplates.CreateComplexFabricatorStorage(go, complexFabricator);
        ConfigureRecipes();
    }

    private void ConfigureRecipes() {
        ComplexRecipe.RecipeElement[] array = {
            new ComplexRecipe.RecipeElement(SimHashes.Copper.CreateTag(), 300f, true),
            new ComplexRecipe.RecipeElement("BasicFabric".ToTag(),        2f)
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
            new ComplexRecipe.RecipeElement("BasicFabric".ToTag(),          2f)
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
            new ComplexRecipe.RecipeElement("BasicFabric".ToTag(),      2f)
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
                new ComplexRecipe.RecipeElement("BasicFabric".ToTag(),        2f)
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
            new ComplexRecipe.RecipeElement("BasicFabric".ToTag(),    1f)
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
            new ComplexRecipe.RecipeElement("BasicFabric".ToTag(),   1f)
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

    public override void DoPostConfigureComplete(GameObject go) {
        go.GetComponent<KPrefabID>().prefabInitFn += delegate {
                                                         Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages
                                                             .TM_Suits);
                                                     };

        go.GetComponent<KPrefabID>().prefabSpawnFn += delegate(GameObject game_object) {
                                                          var component = game_object
                                                              .GetComponent<ComplexFabricatorWorkable>();

                                                          component.WorkerStatusItem
                                                              = Db.Get().DuplicantStatusItems.Fabricating;

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