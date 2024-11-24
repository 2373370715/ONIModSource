using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020003AB RID: 939
public class KilnConfig : IBuildingConfig
{
	// Token: 0x06000F85 RID: 3973 RVA: 0x0017D190 File Offset: 0x0017B390
	public override BuildingDef CreateBuildingDef()
	{
		string id = "Kiln";
		int width = 2;
		int height = 2;
		string anim = "kiln_kanim";
		int hitpoints = 100;
		float construction_time = 30f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 800f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER5;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.PENALTY.TIER1, tier2, 0.2f);
		buildingDef.Overheatable = false;
		buildingDef.RequiresPowerInput = false;
		buildingDef.ExhaustKilowattsWhenActive = 16f;
		buildingDef.SelfHeatKilowattsWhenActive = 4f;
		buildingDef.AudioCategory = "HollowMetal";
		buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 1));
		return buildingDef;
	}

	// Token: 0x06000F86 RID: 3974 RVA: 0x0017D218 File Offset: 0x0017B418
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
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
		this.ConfigureRecipes();
		Prioritizable.AddRef(go);
	}

	// Token: 0x06000F87 RID: 3975 RVA: 0x0017D294 File Offset: 0x0017B494
	private void ConfigureRecipes()
	{
		Tag tag = SimHashes.Ceramic.CreateTag();
		Tag material = SimHashes.Clay.CreateTag();
		Tag material2 = SimHashes.Carbon.CreateTag();
		float num = 100f;
		float num2 = 25f;
		ComplexRecipe.RecipeElement[] array = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement(material, num),
			new ComplexRecipe.RecipeElement(material2, num2)
		};
		ComplexRecipe.RecipeElement[] array2 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement(tag, num, ComplexRecipe.RecipeElement.TemperatureOperation.Heated, false)
		};
		string obsolete_id = ComplexRecipeManager.MakeObsoleteRecipeID("Kiln", tag);
		string text = ComplexRecipeManager.MakeRecipeID("Kiln", array, array2);
		ComplexRecipe complexRecipe = new ComplexRecipe(text, array, array2);
		complexRecipe.time = 40f;
		complexRecipe.description = string.Format(STRINGS.BUILDINGS.PREFABS.EGGCRACKER.RECIPE_DESCRIPTION, ElementLoader.FindElementByHash(SimHashes.Clay).name, ElementLoader.FindElementByHash(SimHashes.Ceramic).name);
		complexRecipe.fabricators = new List<Tag>
		{
			TagManager.Create("Kiln")
		};
		complexRecipe.nameDisplay = ComplexRecipe.RecipeNameDisplay.Result;
		complexRecipe.sortOrder = 100;
		ComplexRecipeManager.Get().AddObsoleteIDMapping(obsolete_id, text);
		Tag tag2 = SimHashes.RefinedCarbon.CreateTag();
		ComplexRecipe.RecipeElement[] array3 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement(material2, num + num2)
		};
		ComplexRecipe.RecipeElement[] array4 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement(tag2, num, ComplexRecipe.RecipeElement.TemperatureOperation.Heated, false)
		};
		string obsolete_id2 = ComplexRecipeManager.MakeObsoleteRecipeID("Kiln", tag2);
		string text2 = ComplexRecipeManager.MakeRecipeID("Kiln", array3, array4);
		ComplexRecipe complexRecipe2 = new ComplexRecipe(text2, array3, array4);
		complexRecipe2.time = 40f;
		complexRecipe2.description = string.Format(STRINGS.BUILDINGS.PREFABS.EGGCRACKER.RECIPE_DESCRIPTION, ElementLoader.FindElementByHash(SimHashes.Carbon).name, ElementLoader.FindElementByHash(SimHashes.RefinedCarbon).name);
		complexRecipe2.fabricators = new List<Tag>
		{
			TagManager.Create("Kiln")
		};
		complexRecipe2.nameDisplay = ComplexRecipe.RecipeNameDisplay.IngredientToResult;
		complexRecipe2.sortOrder = 200;
		ComplexRecipeManager.Get().AddObsoleteIDMapping(obsolete_id2, text2);
		Tag tag3 = SimHashes.RefinedCarbon.CreateTag();
		ComplexRecipe.RecipeElement[] array5 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement(SimHashes.WoodLog.CreateTag(), 100f)
		};
		ComplexRecipe.RecipeElement[] array6 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement(tag3, 50f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated, false)
		};
		string obsolete_id3 = ComplexRecipeManager.MakeObsoleteRecipeID("Kiln", tag3);
		string text3 = ComplexRecipeManager.MakeRecipeID("Kiln", array5, array6);
		ComplexRecipe complexRecipe3 = new ComplexRecipe(text3, array5, array6);
		complexRecipe3.time = 40f;
		complexRecipe3.description = string.Format(STRINGS.BUILDINGS.PREFABS.EGGCRACKER.RECIPE_DESCRIPTION, ElementLoader.FindElementByHash(SimHashes.WoodLog).name, ElementLoader.FindElementByHash(SimHashes.RefinedCarbon).name);
		complexRecipe3.fabricators = new List<Tag>
		{
			TagManager.Create("Kiln")
		};
		complexRecipe3.nameDisplay = ComplexRecipe.RecipeNameDisplay.IngredientToResult;
		complexRecipe3.sortOrder = 300;
		ComplexRecipeManager.Get().AddObsoleteIDMapping(obsolete_id3, text3);
	}

	// Token: 0x06000F88 RID: 3976 RVA: 0x000ACC02 File Offset: 0x000AAE02
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<LogicOperationalController>();
		go.AddOrGetDef<PoweredActiveController.Def>();
		SymbolOverrideControllerUtil.AddToPrefab(go);
	}

	// Token: 0x04000B1D RID: 2845
	public const string ID = "Kiln";

	// Token: 0x04000B1E RID: 2846
	public const float INPUT_CLAY_PER_SECOND = 1f;

	// Token: 0x04000B1F RID: 2847
	public const float CERAMIC_PER_SECOND = 1f;

	// Token: 0x04000B20 RID: 2848
	public const float CO2_RATIO = 0.1f;

	// Token: 0x04000B21 RID: 2849
	public const float OUTPUT_TEMP = 353.15f;

	// Token: 0x04000B22 RID: 2850
	public const float REFILL_RATE = 2400f;

	// Token: 0x04000B23 RID: 2851
	public const float CERAMIC_STORAGE_AMOUNT = 2400f;

	// Token: 0x04000B24 RID: 2852
	public const float COAL_RATE = 0.1f;
}
