using System.Collections.Generic;
using TUNING;
using UnityEngine;

public class DataMinerConfig : IBuildingConfig {
    public const    string          ID                                = "DataMiner";
    public const    float           POWER_USAGE_W                     = 2000f;
    public const    float           BASE_UNITS_PRODUCED_PER_CYCLE     = 2f;
    public const    float           BASE_DTU_PRODUCTION               = 5f;
    public const    float           STORAGE_CAPACITY_KG               = 1000f;
    public const    float           MASS_CONSUMED_PER_BANK_KG         = 5f;
    public const    float           BASE_DURATION                     = 0.0033333334f;
    public const    float           BASE_PRODUCTION_PROGRESS_PER_TICK = 0.00066666666f;
    public static   MathUtil.MinMax PRODUCTION_RATE_SCALE             = new MathUtil.MinMax(0.6f, 4f);
    public static   MathUtil.MinMax TEMPERATURE_SCALING_RANGE         = new MathUtil.MinMax(5f,   350f);
    public          SimHashes       INPUT_MATERIAL                    = SimHashes.Polypropylene;
    public          Tag             INPUT_MATERIAL_TAG                = SimHashes.Polypropylene.CreateTag();
    public          Tag             OUTPUT_MATERIAL_TAG               = OrbitalResearchDatabankConfig.TAG;
    public override string[]        GetRequiredDlcIds() { return DlcManager.DLC3; }

    public override BuildingDef CreateBuildingDef() {
        var id                  = "DataMiner";
        var width               = 3;
        var height              = 2;
        var anim                = "data_miner_kanim";
        var hitpoints           = 30;
        var construction_time   = 30f;
        var tier                = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
        var all_METALS          = MATERIALS.ALL_METALS;
        var melting_point       = 1600f;
        var build_location_rule = BuildLocationRule.OnFloor;
        var tier2               = NOISE_POLLUTION.NOISY.TIER1;
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
                                                              BUILDINGS.DECOR.NONE,
                                                              tier2);

        buildingDef.RequiresPowerInput = true;
        buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
        buildingDef.EnergyConsumptionWhenActive = 2000f;
        buildingDef.ExhaustKilowattsWhenActive = 0.5f;
        buildingDef.SelfHeatKilowattsWhenActive = 5f;
        buildingDef.ViewMode = OverlayModes.Power.ID;
        buildingDef.AudioCategory = "Metal";
        buildingDef.AudioSize = "large";
        return buildingDef;
    }

    public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag) {
        go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
        go.AddOrGet<DropAllWorkable>();
        go.AddOrGet<BuildingComplete>().isManuallyOperated = false;
        go.AddOrGet<LogicOperationalController>();
        var dataMiner = go.AddOrGet<DataMiner>();
        dataMiner.duplicantOperated = false;
        dataMiner.showProgressBar   = true;
        dataMiner.sideScreenStyle   = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;
        BuildingTemplates.CreateComplexFabricatorStorage(go, dataMiner);
        ComplexRecipe.RecipeElement[] array = { new ComplexRecipe.RecipeElement(INPUT_MATERIAL_TAG, 5f) };
        ComplexRecipe.RecipeElement[] array2 = {
            new ComplexRecipe.RecipeElement(OUTPUT_MATERIAL_TAG,
                                            1f,
                                            ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature)
        };

        var obsolete_id   = ComplexRecipeManager.MakeObsoleteRecipeID("DataMiner", OUTPUT_MATERIAL_TAG);
        var text          = ComplexRecipeManager.MakeRecipeID("DataMiner", array, array2);
        var complexRecipe = new ComplexRecipe(text, array, array2);
        complexRecipe.time = 0.0033333334f;
        complexRecipe.description = string.Format(STRINGS.BUILDINGS.PREFABS.EGGCRACKER.RECIPE_DESCRIPTION,
                                                  ElementLoader.FindElementByHash(INPUT_MATERIAL).name,
                                                  "TODO");

        complexRecipe.fabricators = new List<Tag> { TagManager.Create("DataMiner") };
        complexRecipe.nameDisplay = ComplexRecipe.RecipeNameDisplay.IngredientToResult;
        complexRecipe.sortOrder   = 300;
        ComplexRecipeManager.Get().AddObsoleteIDMapping(obsolete_id, text);
        Prioritizable.AddRef(go);
    }

    public override void DoPostConfigureComplete(GameObject go) { }
}