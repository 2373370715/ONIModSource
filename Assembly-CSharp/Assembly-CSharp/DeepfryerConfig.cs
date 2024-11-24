﻿using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

public class DeepfryerConfig : IBuildingConfig
{
	public override string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_DLC_2;
	}

	public override BuildingDef CreateBuildingDef()
	{
		string id = "Deepfryer";
		int width = 2;
		int height = 2;
		string anim = "deepfryer_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER3;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.NONE, tier2, 0.2f);
		buildingDef.ViewMode = OverlayModes.Rooms.ID;
		BuildingTemplates.CreateElectricalBuildingDef(buildingDef);
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "large";
		buildingDef.EnergyConsumptionWhenActive = 480f;
		buildingDef.ExhaustKilowattsWhenActive = 2f;
		buildingDef.SelfHeatKilowattsWhenActive = 8f;
		return buildingDef;
	}

	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
		Deepfryer deepfryer = go.AddOrGet<Deepfryer>();
		deepfryer.heatedTemperature = 368.15f;
		go.AddOrGet<FabricatorIngredientStatusManager>();
		go.AddOrGet<CopyBuildingSettings>();
		RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
		roomTracker.requiredRoomType = Db.Get().RoomTypes.Kitchen.Id;
		roomTracker.requirement = RoomTracker.Requirement.Required;
		go.AddOrGet<ComplexFabricatorWorkable>().overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_deepfryer_kanim")
		};
		deepfryer.sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;
		Prioritizable.AddRef(go);
		go.AddOrGet<DropAllWorkable>();
		this.ConfigureRecipes();
		go.AddOrGetDef<PoweredController.Def>();
		BuildingTemplates.CreateComplexFabricatorStorage(go, deepfryer);
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.CookTop, false);
	}

	private void ConfigureRecipes()
	{
		ComplexRecipe.RecipeElement[] array = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement(CarrotConfig.ID, 1f),
			new ComplexRecipe.RecipeElement("Tallow", 1f)
		};
		ComplexRecipe.RecipeElement[] array2 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement("FriesCarrot", 1f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated, false)
		};
		FriesCarrotConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("Deepfryer", array, array2), array, array2, this.GetDlcIds())
		{
			time = FOOD.RECIPES.STANDARD_COOK_TIME,
			description = ITEMS.FOOD.FRIESCARROT.RECIPEDESC,
			nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
			fabricators = new List<Tag>
			{
				"Deepfryer"
			},
			sortOrder = 100
		};
		ComplexRecipe.RecipeElement[] array3 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement("BeanPlantSeed", 6f),
			new ComplexRecipe.RecipeElement("Tallow", 1f)
		};
		ComplexRecipe.RecipeElement[] array4 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement("DeepFriedNosh", 1f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated, false)
		};
		FriesCarrotConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("Deepfryer", array3, array4), array3, array4, this.GetDlcIds())
		{
			time = FOOD.RECIPES.STANDARD_COOK_TIME,
			description = ITEMS.FOOD.DEEPFRIEDNOSH.RECIPEDESC,
			nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
			fabricators = new List<Tag>
			{
				"Deepfryer"
			},
			sortOrder = 200
		};
		ComplexRecipe.RecipeElement[] array5 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement("FishMeat", 1f),
			new ComplexRecipe.RecipeElement("Tallow", 2.4f),
			new ComplexRecipe.RecipeElement("ColdWheatSeed", 2f)
		};
		ComplexRecipe.RecipeElement[] array6 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement("DeepFriedFish", 1f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated, false)
		};
		DeepFriedFishConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("Deepfryer", array5, array6), array5, array6, this.GetDlcIds())
		{
			time = FOOD.RECIPES.STANDARD_COOK_TIME,
			description = ITEMS.FOOD.DEEPFRIEDFISH.RECIPEDESC,
			nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
			fabricators = new List<Tag>
			{
				"Deepfryer"
			},
			sortOrder = 300
		};
		ComplexRecipe.RecipeElement[] array7 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement("ShellfishMeat", 1f),
			new ComplexRecipe.RecipeElement("Tallow", 2.4f),
			new ComplexRecipe.RecipeElement("ColdWheatSeed", 2f)
		};
		ComplexRecipe.RecipeElement[] array8 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement("DeepFriedShellfish", 1f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated, false)
		};
		DeepFriedShellfishConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("Deepfryer", array7, array8), array7, array8, this.GetDlcIds())
		{
			time = FOOD.RECIPES.STANDARD_COOK_TIME,
			description = ITEMS.FOOD.DEEPFRIEDSHELLFISH.RECIPEDESC,
			nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
			fabricators = new List<Tag>
			{
				"Deepfryer"
			},
			sortOrder = 300
		};
	}

	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	public const string ID = "Deepfryer";
}
