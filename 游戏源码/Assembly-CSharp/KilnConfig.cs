using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

public class KilnConfig : IBuildingConfig
{
	public const string ID = "Kiln";

	public const float INPUT_CLAY_PER_SECOND = 1f;

	public const float CERAMIC_PER_SECOND = 1f;

	public const float CO2_RATIO = 0.1f;

	public const float OUTPUT_TEMP = 353.15f;

	public const float REFILL_RATE = 2400f;

	public const float CERAMIC_STORAGE_AMOUNT = 2400f;

	public const float COAL_RATE = 0.1f;

	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef obj = BuildingTemplates.CreateBuildingDef("Kiln", 2, 2, "kiln_kanim", 100, 30f, TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER3, MATERIALS.ALL_METALS, 800f, BuildLocationRule.OnFloor, noise: NOISE_POLLUTION.NOISY.TIER5, decor: TUNING.BUILDINGS.DECOR.PENALTY.TIER1);
		obj.Overheatable = false;
		obj.RequiresPowerInput = false;
		obj.ExhaustKilowattsWhenActive = 16f;
		obj.SelfHeatKilowattsWhenActive = 4f;
		obj.AudioCategory = "HollowMetal";
		obj.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 1));
		return obj;
	}

	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
		go.AddOrGet<DropAllWorkable>();
		go.AddOrGet<BuildingComplete>().isManuallyOperated = false;
		ComplexFabricator complexFabricator = go.AddOrGet<ComplexFabricator>();
		complexFabricator.heatedTemperature = 353.15f;
		complexFabricator.duplicantOperated = false;
		complexFabricator.showProgressBar = true;
		complexFabricator.sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;
		go.AddOrGet<FabricatorIngredientStatusManager>();
		go.AddOrGet<CopyBuildingSettings>();
		BuildingTemplates.CreateComplexFabricatorStorage(go, complexFabricator);
		ConfigureRecipes();
		Prioritizable.AddRef(go);
	}

	private void ConfigureRecipes()
	{
		Tag tag = SimHashes.Ceramic.CreateTag();
		Tag material = SimHashes.Clay.CreateTag();
		Tag material2 = SimHashes.Carbon.CreateTag();
		float num = 100f;
		float num2 = 25f;
		ComplexRecipe.RecipeElement[] array = new ComplexRecipe.RecipeElement[2]
		{
			new ComplexRecipe.RecipeElement(material, num),
			new ComplexRecipe.RecipeElement(material2, num2)
		};
		ComplexRecipe.RecipeElement[] array2 = new ComplexRecipe.RecipeElement[1]
		{
			new ComplexRecipe.RecipeElement(tag, num, ComplexRecipe.RecipeElement.TemperatureOperation.Heated)
		};
		string obsolete_id = ComplexRecipeManager.MakeObsoleteRecipeID("Kiln", tag);
		string text = ComplexRecipeManager.MakeRecipeID("Kiln", array, array2);
		new ComplexRecipe(text, array, array2)
		{
			time = 40f,
			description = string.Format(STRINGS.BUILDINGS.PREFABS.EGGCRACKER.RECIPE_DESCRIPTION, ElementLoader.FindElementByHash(SimHashes.Clay).name, ElementLoader.FindElementByHash(SimHashes.Ceramic).name),
			fabricators = new List<Tag> { TagManager.Create("Kiln") },
			nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
			sortOrder = 100
		};
		ComplexRecipeManager.Get().AddObsoleteIDMapping(obsolete_id, text);
		Tag tag2 = SimHashes.RefinedCarbon.CreateTag();
		ComplexRecipe.RecipeElement[] array3 = new ComplexRecipe.RecipeElement[1]
		{
			new ComplexRecipe.RecipeElement(material2, num + num2)
		};
		ComplexRecipe.RecipeElement[] array4 = new ComplexRecipe.RecipeElement[1]
		{
			new ComplexRecipe.RecipeElement(tag2, num, ComplexRecipe.RecipeElement.TemperatureOperation.Heated)
		};
		string obsolete_id2 = ComplexRecipeManager.MakeObsoleteRecipeID("Kiln", tag2);
		string text2 = ComplexRecipeManager.MakeRecipeID("Kiln", array3, array4);
		new ComplexRecipe(text2, array3, array4)
		{
			time = 40f,
			description = string.Format(STRINGS.BUILDINGS.PREFABS.EGGCRACKER.RECIPE_DESCRIPTION, ElementLoader.FindElementByHash(SimHashes.Carbon).name, ElementLoader.FindElementByHash(SimHashes.RefinedCarbon).name),
			fabricators = new List<Tag> { TagManager.Create("Kiln") },
			nameDisplay = ComplexRecipe.RecipeNameDisplay.IngredientToResult,
			sortOrder = 200
		};
		ComplexRecipeManager.Get().AddObsoleteIDMapping(obsolete_id2, text2);
		Tag tag3 = SimHashes.RefinedCarbon.CreateTag();
		ComplexRecipe.RecipeElement[] array5 = new ComplexRecipe.RecipeElement[1]
		{
			new ComplexRecipe.RecipeElement(SimHashes.WoodLog.CreateTag(), 100f)
		};
		ComplexRecipe.RecipeElement[] array6 = new ComplexRecipe.RecipeElement[1]
		{
			new ComplexRecipe.RecipeElement(tag3, 50f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated)
		};
		string obsolete_id3 = ComplexRecipeManager.MakeObsoleteRecipeID("Kiln", tag3);
		string text3 = ComplexRecipeManager.MakeRecipeID("Kiln", array5, array6);
		new ComplexRecipe(text3, array5, array6)
		{
			time = 40f,
			description = string.Format(STRINGS.BUILDINGS.PREFABS.EGGCRACKER.RECIPE_DESCRIPTION, ElementLoader.FindElementByHash(SimHashes.WoodLog).name, ElementLoader.FindElementByHash(SimHashes.RefinedCarbon).name),
			fabricators = new List<Tag> { TagManager.Create("Kiln") },
			nameDisplay = ComplexRecipe.RecipeNameDisplay.IngredientToResult,
			sortOrder = 300
		};
		ComplexRecipeManager.Get().AddObsoleteIDMapping(obsolete_id3, text3);
	}

	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<LogicOperationalController>();
		go.AddOrGetDef<PoweredActiveController.Def>();
		SymbolOverrideControllerUtil.AddToPrefab(go);
	}
}
