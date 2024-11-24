using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

public class MilkPressConfig : IBuildingConfig
{
	public const string ID = "MilkPress";

	private static readonly List<Storage.StoredItemModifier> RefineryStoredItemModifiers = new List<Storage.StoredItemModifier>
	{
		Storage.StoredItemModifier.Hide,
		Storage.StoredItemModifier.Preserve,
		Storage.StoredItemModifier.Insulate,
		Storage.StoredItemModifier.Seal
	};

	public override string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef obj = BuildingTemplates.CreateBuildingDef("MilkPress", 2, 3, "milkpress_kanim", 100, 30f, TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4, MATERIALS.ALL_MINERALS, 1600f, BuildLocationRule.OnFloor, noise: NOISE_POLLUTION.NOISY.TIER4, decor: TUNING.BUILDINGS.DECOR.PENALTY.TIER1);
		obj.RequiresPowerInput = false;
		obj.EnergyConsumptionWhenActive = 0f;
		obj.SelfHeatKilowattsWhenActive = 2f;
		obj.OutputConduitType = ConduitType.Liquid;
		obj.UtilityOutputOffset = new CellOffset(1, 0);
		obj.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(1, 0));
		obj.ViewMode = OverlayModes.LiquidConduits.ID;
		obj.AudioCategory = "HollowMetal";
		obj.AudioSize = "Metal";
		return obj;
	}

	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<LogicOperationalController>();
		go.AddOrGet<DropAllWorkable>();
		go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
		ComplexFabricator complexFabricator = go.AddOrGet<ComplexFabricator>();
		complexFabricator.sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;
		complexFabricator.duplicantOperated = true;
		go.AddOrGet<FabricatorIngredientStatusManager>();
		go.AddOrGet<CopyBuildingSettings>();
		ComplexFabricatorWorkable complexFabricatorWorkable = go.AddOrGet<ComplexFabricatorWorkable>();
		BuildingTemplates.CreateComplexFabricatorStorage(go, complexFabricator);
		complexFabricatorWorkable.overrideAnims = new KAnimFile[1] { Assets.GetAnim("anim_interacts_milkpress_kanim") };
		complexFabricatorWorkable.workingPstComplete = new HashedString[1] { "working_pst_complete" };
		complexFabricator.storeProduced = true;
		complexFabricator.inStorage.SetDefaultStoredItemModifiers(RefineryStoredItemModifiers);
		complexFabricator.outStorage.SetDefaultStoredItemModifiers(RefineryStoredItemModifiers);
		ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
		conduitDispenser.conduitType = ConduitType.Liquid;
		conduitDispenser.alwaysDispense = true;
		conduitDispenser.elementFilter = null;
		conduitDispenser.storage = go.GetComponent<ComplexFabricator>().outStorage;
		AddRecipes(go);
		Prioritizable.AddRef(go);
	}

	private void AddRecipes(GameObject go)
	{
		ComplexRecipe.RecipeElement[] array = new ComplexRecipe.RecipeElement[2]
		{
			new ComplexRecipe.RecipeElement("ColdWheatSeed", 10f),
			new ComplexRecipe.RecipeElement(SimHashes.Water.CreateTag(), 15f)
		};
		ComplexRecipe.RecipeElement[] array2 = new ComplexRecipe.RecipeElement[1]
		{
			new ComplexRecipe.RecipeElement(SimHashes.Milk.CreateTag(), 20f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature)
		};
		new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("MilkPress", array, array2), array, array2, 0, 0)
		{
			time = 40f,
			description = string.Format(STRINGS.BUILDINGS.PREFABS.MILKPRESS.WHEAT_MILK_RECIPE_DESCRIPTION, ITEMS.FOOD.COLDWHEATSEED.NAME, SimHashes.Milk.CreateTag().ProperName()),
			nameDisplay = ComplexRecipe.RecipeNameDisplay.IngredientToResult,
			fabricators = new List<Tag> { TagManager.Create("MilkPress") }
		};
		ComplexRecipe.RecipeElement[] array3 = new ComplexRecipe.RecipeElement[2]
		{
			new ComplexRecipe.RecipeElement(SpiceNutConfig.ID, 3f),
			new ComplexRecipe.RecipeElement(SimHashes.Water.CreateTag(), 17f)
		};
		ComplexRecipe.RecipeElement[] array4 = new ComplexRecipe.RecipeElement[1]
		{
			new ComplexRecipe.RecipeElement(SimHashes.Milk.CreateTag(), 20f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature)
		};
		new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("MilkPress", array3, array4), array3, array4, 0, 0)
		{
			time = 40f,
			description = string.Format(STRINGS.BUILDINGS.PREFABS.MILKPRESS.NUT_MILK_RECIPE_DESCRIPTION, ITEMS.FOOD.SPICENUT.NAME, SimHashes.Milk.CreateTag().ProperName()),
			nameDisplay = ComplexRecipe.RecipeNameDisplay.IngredientToResult,
			fabricators = new List<Tag> { TagManager.Create("MilkPress") }
		};
		ComplexRecipe.RecipeElement[] array5 = new ComplexRecipe.RecipeElement[2]
		{
			new ComplexRecipe.RecipeElement("BeanPlantSeed", 2f),
			new ComplexRecipe.RecipeElement(SimHashes.Water.CreateTag(), 18f)
		};
		ComplexRecipe.RecipeElement[] array6 = new ComplexRecipe.RecipeElement[1]
		{
			new ComplexRecipe.RecipeElement(SimHashes.Milk.CreateTag(), 20f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature)
		};
		new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("MilkPress", array5, array6), array5, array6, 0, 0)
		{
			time = 40f,
			description = string.Format(STRINGS.BUILDINGS.PREFABS.MILKPRESS.NUT_MILK_RECIPE_DESCRIPTION, ITEMS.FOOD.BEANPLANTSEED.NAME, SimHashes.Milk.CreateTag().ProperName()),
			nameDisplay = ComplexRecipe.RecipeNameDisplay.IngredientToResult,
			fabricators = new List<Tag> { TagManager.Create("MilkPress") }
		};
	}

	public override void DoPostConfigureComplete(GameObject go)
	{
		SymbolOverrideControllerUtil.AddToPrefab(go);
		go.GetComponent<KPrefabID>().prefabSpawnFn += delegate(GameObject game_object)
		{
			ComplexFabricatorWorkable component = game_object.GetComponent<ComplexFabricatorWorkable>();
			component.WorkerStatusItem = Db.Get().DuplicantStatusItems.Processing;
			component.AttributeConverter = Db.Get().AttributeConverters.MachinerySpeed;
			component.AttributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
			component.SkillExperienceSkillGroup = Db.Get().SkillGroups.Technicals.Id;
			component.SkillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		};
	}
}
