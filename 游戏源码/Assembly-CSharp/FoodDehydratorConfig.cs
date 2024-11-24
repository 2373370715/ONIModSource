using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

public class FoodDehydratorConfig : IBuildingConfig
{
	public const string ID = "FoodDehydrator";

	public ComplexRecipe DehydratedFoodRecipe;

	private static readonly List<Storage.StoredItemModifier> GourmetCookingStationStoredItemModifiers = new List<Storage.StoredItemModifier>
	{
		Storage.StoredItemModifier.Hide,
		Storage.StoredItemModifier.Preserve,
		Storage.StoredItemModifier.Insulate,
		Storage.StoredItemModifier.Seal
	};

	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef obj = BuildingTemplates.CreateBuildingDef("FoodDehydrator", 3, 3, "dehydrator_kanim", 30, 30f, new float[2]
		{
			TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER3[0],
			TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER2[0]
		}, new string[2] { "RefinedMetal", "Plastic" }, 1600f, BuildLocationRule.OnFloor, noise: NOISE_POLLUTION.NOISY.TIER3, decor: TUNING.BUILDINGS.DECOR.NONE);
		BuildingTemplates.CreateStandardBuildingDef(obj);
		obj.AudioCategory = "Metal";
		obj.AudioSize = "large";
		obj.InputConduitType = ConduitType.Gas;
		obj.UtilityInputOffset = new CellOffset(-1, 0);
		obj.SelfHeatKilowattsWhenActive = 4f;
		obj.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
		obj.ForegroundLayer = Grid.SceneLayer.BuildingFront;
		return obj;
	}

	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<DropAllWorkable>();
		go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
		ComplexFabricator complexFabricator = go.AddOrGet<ComplexFabricator>();
		complexFabricator.heatedTemperature = 368.15f;
		complexFabricator.duplicantOperated = false;
		complexFabricator.showProgressBar = true;
		complexFabricator.sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;
		complexFabricator.keepAdditionalTag = FOODDEHYDRATORTUNING.FUEL_TAG;
		complexFabricator.storeProduced = true;
		go.AddOrGet<FabricatorIngredientStatusManager>();
		go.AddOrGet<CopyBuildingSettings>();
		BuildingTemplates.CreateComplexFabricatorStorage(go, complexFabricator);
		complexFabricator.inStorage.SetDefaultStoredItemModifiers(GourmetCookingStationStoredItemModifiers);
		complexFabricator.buildStorage.SetDefaultStoredItemModifiers(GourmetCookingStationStoredItemModifiers);
		complexFabricator.outStorage.SetDefaultStoredItemModifiers(GourmetCookingStationStoredItemModifiers);
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.capacityTag = FOODDEHYDRATORTUNING.FUEL_TAG;
		conduitConsumer.capacityKG = 5.0000005f;
		conduitConsumer.alwaysConsume = true;
		conduitConsumer.storage = complexFabricator.inStorage;
		conduitConsumer.forceAlwaysSatisfied = true;
		ElementConverter elementConverter = go.AddOrGet<ElementConverter>();
		elementConverter.consumedElements = new ElementConverter.ConsumedElement[1]
		{
			new ElementConverter.ConsumedElement(FOODDEHYDRATORTUNING.FUEL_TAG, 0.020000001f)
		};
		elementConverter.outputElements = new ElementConverter.OutputElement[1]
		{
			new ElementConverter.OutputElement(0.0050000004f, SimHashes.CarbonDioxide, 348.15f, useEntityTemperature: false, storeOutput: false, 0f, 1f)
		};
		ConfigureRecipes();
		Prioritizable.AddRef(go);
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
		KAnimFile[] overrideAnims = new KAnimFile[1] { Assets.GetAnim("anim_interacts_dehydrator_kanim") };
		FoodDehydratorWorkableEmpty foodDehydratorWorkableEmpty = go.AddOrGet<FoodDehydratorWorkableEmpty>();
		foodDehydratorWorkableEmpty.workTime = 50f;
		foodDehydratorWorkableEmpty.overrideAnims = overrideAnims;
		foodDehydratorWorkableEmpty.workLayer = Grid.SceneLayer.Front;
		go.AddOrGet<LogicOperationalController>();
		go.AddOrGetDef<FoodDehydrator.Def>();
	}

	private void ConfigureRecipes()
	{
		List<(EdiblesManager.FoodInfo, Tag)> list = new List<(EdiblesManager.FoodInfo, Tag)>
		{
			(FOOD.FOOD_TYPES.SALSA, DehydratedSalsaConfig.ID),
			(FOOD.FOOD_TYPES.MUSHROOM_WRAP, DehydratedMushroomWrapConfig.ID),
			(FOOD.FOOD_TYPES.SURF_AND_TURF, DehydratedSurfAndTurfConfig.ID),
			(FOOD.FOOD_TYPES.SPICEBREAD, DehydratedSpiceBreadConfig.ID),
			(FOOD.FOOD_TYPES.QUICHE, DehydratedQuicheConfig.ID),
			(FOOD.FOOD_TYPES.CURRY, DehydratedCurryConfig.ID),
			(FOOD.FOOD_TYPES.SPICY_TOFU, DehydratedSpicyTofuConfig.ID),
			(FOOD.FOOD_TYPES.BURGER, DehydratedFoodPackageConfig.ID)
		};
		if (DlcManager.IsExpansion1Active())
		{
			list.Add((FOOD.FOOD_TYPES.BERRY_PIE, DehydratedBerryPieConfig.ID));
		}
		int num = 100;
		foreach (var item3 in list)
		{
			EdiblesManager.FoodInfo item = item3.Item1;
			Tag item2 = item3.Item2;
			ComplexRecipe.RecipeElement[] array = new ComplexRecipe.RecipeElement[2]
			{
				new ComplexRecipe.RecipeElement(item, 6000000f / item.CaloriesPerUnit),
				new ComplexRecipe.RecipeElement(SimHashes.Polypropylene.CreateTag(), 12f)
			};
			ComplexRecipe.RecipeElement[] array2 = new ComplexRecipe.RecipeElement[2]
			{
				new ComplexRecipe.RecipeElement(item2, 6f, ComplexRecipe.RecipeElement.TemperatureOperation.Dehydrated),
				new ComplexRecipe.RecipeElement(SimHashes.Water.CreateTag(), 6f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated)
			};
			new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("FoodDehydrator", array, array2), array, array2)
			{
				time = 250f,
				nameDisplay = ComplexRecipe.RecipeNameDisplay.Custom,
				customName = string.Format(STRINGS.BUILDINGS.PREFABS.FOODDEHYDRATOR.RECIPE_NAME, item.Name),
				description = string.Format(STRINGS.BUILDINGS.PREFABS.FOODDEHYDRATOR.RESULT_DESCRIPTION, item.Name),
				fabricators = new List<Tag> { TagManager.Create("FoodDehydrator") },
				sortOrder = num
			};
			num++;
		}
	}

	public override void DoPostConfigureComplete(GameObject go)
	{
	}
}
