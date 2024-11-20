using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

public class MissileFabricatorConfig : IBuildingConfig
{
	public override BuildingDef CreateBuildingDef()
	{
		string id = "MissileFabricator";
		int width = 5;
		int height = 4;
		string anim = "missile_fabricator_kanim";
		int hitpoints = 250;
		float construction_time = 60f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER5;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER6;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, refined_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.PENALTY.TIER2, tier2, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 960f;
		buildingDef.SelfHeatKilowattsWhenActive = 8f;
		buildingDef.ExhaustKilowattsWhenActive = 0f;
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.PowerInputOffset = new CellOffset(1, 0);
		buildingDef.InputConduitType = ConduitType.Liquid;
		buildingDef.UtilityInputOffset = new CellOffset(-1, 1);
		return buildingDef;
	}

	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		go.AddOrGet<DropAllWorkable>();
		go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
		ComplexFabricator complexFabricator = go.AddOrGet<ComplexFabricator>();
		complexFabricator.sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;
		go.AddOrGet<FabricatorIngredientStatusManager>();
		go.AddOrGet<CopyBuildingSettings>();
		complexFabricator.keepExcessLiquids = true;
		complexFabricator.allowManualFluidDelivery = false;
		Workable workable = go.AddOrGet<ComplexFabricatorWorkable>();
		complexFabricator.duplicantOperated = true;
		BuildingTemplates.CreateComplexFabricatorStorage(go, complexFabricator);
		complexFabricator.storeProduced = false;
		complexFabricator.inStorage.SetDefaultStoredItemModifiers(MissileFabricatorConfig.RefineryStoredItemModifiers);
		complexFabricator.buildStorage.SetDefaultStoredItemModifiers(MissileFabricatorConfig.RefineryStoredItemModifiers);
		complexFabricator.outputOffset = new Vector3(1f, 0.5f);
		workable.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_missile_fabricator_kanim")
		};
		BuildingElementEmitter buildingElementEmitter = go.AddOrGet<BuildingElementEmitter>();
		buildingElementEmitter.emitRate = 0.0125f;
		buildingElementEmitter.temperature = 313.15f;
		buildingElementEmitter.element = SimHashes.CarbonDioxide;
		buildingElementEmitter.modifierOffset = new Vector2(2f, 2f);
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.capacityTag = GameTags.Liquid;
		conduitConsumer.capacityKG = 100f;
		conduitConsumer.storage = complexFabricator.inStorage;
		conduitConsumer.alwaysConsume = false;
		conduitConsumer.forceAlwaysSatisfied = true;
		conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Store;
		ComplexRecipe.RecipeElement[] array = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement(ElementLoader.FindElementByHash(SimHashes.Iron).tag, 25f, true),
			new ComplexRecipe.RecipeElement(ElementLoader.FindElementByHash(SimHashes.Petroleum).tag, 50f)
		};
		ComplexRecipe.RecipeElement[] array2 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement("MissileBasic", 5f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, false)
		};
		string obsolete_id = ComplexRecipeManager.MakeObsoleteRecipeID("MissileFabricator", array[0].material);
		string text = ComplexRecipeManager.MakeRecipeID("MissileFabricator", array, array2);
		MissileBasicConfig.recipe = new ComplexRecipe(text, array, array2)
		{
			time = 80f,
			nameDisplay = ComplexRecipe.RecipeNameDisplay.ResultWithIngredient,
			description = string.Format(STRINGS.BUILDINGS.PREFABS.MISSILEFABRICATOR.RECIPE_DESCRIPTION, ITEMS.MISSILE_BASIC.NAME, ElementLoader.GetElement(array[0].material).name, ElementLoader.GetElement(array[1].material).name),
			fabricators = new List<Tag>
			{
				TagManager.Create("MissileFabricator")
			}
		};
		ComplexRecipeManager.Get().AddObsoleteIDMapping(obsolete_id, text);
		ComplexRecipe.RecipeElement[] array3 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement(ElementLoader.FindElementByHash(SimHashes.Copper).tag, 25f, true),
			new ComplexRecipe.RecipeElement(ElementLoader.FindElementByHash(SimHashes.Petroleum).tag, 50f)
		};
		ComplexRecipe.RecipeElement[] array4 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement("MissileBasic", 5f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, false)
		};
		string obsolete_id2 = ComplexRecipeManager.MakeObsoleteRecipeID("MissileFabricator", array3[0].material);
		string text2 = ComplexRecipeManager.MakeRecipeID("MissileFabricator", array3, array4);
		MissileBasicConfig.recipe = new ComplexRecipe(text2, array3, array4)
		{
			time = 80f,
			nameDisplay = ComplexRecipe.RecipeNameDisplay.ResultWithIngredient,
			description = string.Format(STRINGS.BUILDINGS.PREFABS.MISSILEFABRICATOR.RECIPE_DESCRIPTION, ITEMS.MISSILE_BASIC.NAME, ElementLoader.GetElement(array3[0].material).name, ElementLoader.GetElement(array3[1].material).name),
			fabricators = new List<Tag>
			{
				TagManager.Create("MissileFabricator")
			}
		};
		ComplexRecipeManager.Get().AddObsoleteIDMapping(obsolete_id2, text2);
		ComplexRecipe.RecipeElement[] array5 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement(ElementLoader.FindElementByHash(SimHashes.Aluminum).tag, 25f, true),
			new ComplexRecipe.RecipeElement(ElementLoader.FindElementByHash(SimHashes.Petroleum).tag, 50f)
		};
		ComplexRecipe.RecipeElement[] array6 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement("MissileBasic", 5f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, false)
		};
		string obsolete_id3 = ComplexRecipeManager.MakeObsoleteRecipeID("MissileFabricator", array5[0].material);
		string text3 = ComplexRecipeManager.MakeRecipeID("MissileFabricator", array5, array6);
		MissileBasicConfig.recipe = new ComplexRecipe(text3, array5, array6)
		{
			time = 80f,
			nameDisplay = ComplexRecipe.RecipeNameDisplay.ResultWithIngredient,
			description = string.Format(STRINGS.BUILDINGS.PREFABS.MISSILEFABRICATOR.RECIPE_DESCRIPTION, ITEMS.MISSILE_BASIC.NAME, ElementLoader.GetElement(array5[0].material).name, ElementLoader.GetElement(array5[1].material).name),
			fabricators = new List<Tag>
			{
				TagManager.Create("MissileFabricator")
			}
		};
		ComplexRecipeManager.Get().AddObsoleteIDMapping(obsolete_id3, text3);
		if (ElementLoader.FindElementByHash(SimHashes.Cobalt) != null)
		{
			ComplexRecipe.RecipeElement[] array7 = new ComplexRecipe.RecipeElement[]
			{
				new ComplexRecipe.RecipeElement(ElementLoader.FindElementByHash(SimHashes.Cobalt).tag, 25f, true),
				new ComplexRecipe.RecipeElement(ElementLoader.FindElementByHash(SimHashes.Petroleum).tag, 50f)
			};
			ComplexRecipe.RecipeElement[] array8 = new ComplexRecipe.RecipeElement[]
			{
				new ComplexRecipe.RecipeElement("MissileBasic", 5f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, false)
			};
			string obsolete_id4 = ComplexRecipeManager.MakeObsoleteRecipeID("MissileFabricator", array7[0].material);
			string text4 = ComplexRecipeManager.MakeRecipeID("MissileFabricator", array7, array8);
			MissileBasicConfig.recipe = new ComplexRecipe(text4, array7, array8)
			{
				time = 80f,
				nameDisplay = ComplexRecipe.RecipeNameDisplay.ResultWithIngredient,
				description = string.Format(STRINGS.BUILDINGS.PREFABS.MISSILEFABRICATOR.RECIPE_DESCRIPTION, ITEMS.MISSILE_BASIC.NAME, ElementLoader.GetElement(array7[0].material).name, ElementLoader.GetElement(array7[1].material).name),
				fabricators = new List<Tag>
				{
					TagManager.Create("MissileFabricator")
				}
			};
			ComplexRecipeManager.Get().AddObsoleteIDMapping(obsolete_id4, text4);
		}
		Prioritizable.AddRef(go);
	}

	public override void DoPostConfigureComplete(GameObject go)
	{
		go.GetComponent<KPrefabID>().prefabSpawnFn += delegate(GameObject game_object)
		{
			ComplexFabricatorWorkable component = game_object.GetComponent<ComplexFabricatorWorkable>();
			component.WorkerStatusItem = Db.Get().DuplicantStatusItems.Fabricating;
			component.AttributeConverter = Db.Get().AttributeConverters.MachinerySpeed;
			component.AttributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
			component.SkillExperienceSkillGroup = Db.Get().SkillGroups.Technicals.Id;
			component.SkillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
			component.requiredSkillPerk = Db.Get().SkillPerks.CanMakeMissiles.Id;
		};
	}

	public const string ID = "MissileFabricator";

	public const float MISSILE_FABRICATION_TIME = 80f;

	public const float CO2_PRODUCTION_RATE = 0.0125f;

	private static readonly List<Storage.StoredItemModifier> RefineryStoredItemModifiers = new List<Storage.StoredItemModifier>
	{
		Storage.StoredItemModifier.Hide,
		Storage.StoredItemModifier.Preserve,
		Storage.StoredItemModifier.Seal
	};
}
