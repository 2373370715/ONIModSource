using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020005C0 RID: 1472
public class SuitFabricatorConfig : IBuildingConfig
{
	// Token: 0x06001A60 RID: 6752 RVA: 0x001A6E60 File Offset: 0x001A5060
	public override BuildingDef CreateBuildingDef()
	{
		string id = "SuitFabricator";
		int width = 4;
		int height = 3;
		string anim = "suit_maker_kanim";
		int hitpoints = 100;
		float construction_time = 240f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 800f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER3;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, refined_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.NONE, tier2, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 480f;
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.PowerInputOffset = new CellOffset(1, 0);
		return buildingDef;
	}

	// Token: 0x06001A61 RID: 6753 RVA: 0x001A6EDC File Offset: 0x001A50DC
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		go.AddOrGet<DropAllWorkable>();
		go.AddOrGet<Prioritizable>();
		ComplexFabricator complexFabricator = go.AddOrGet<ComplexFabricator>();
		complexFabricator.heatedTemperature = 318.15f;
		complexFabricator.sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;
		go.AddOrGet<FabricatorIngredientStatusManager>();
		go.AddOrGet<CopyBuildingSettings>();
		go.AddOrGet<ComplexFabricatorWorkable>().overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_suit_fabricator_kanim")
		};
		Prioritizable.AddRef(go);
		BuildingTemplates.CreateComplexFabricatorStorage(go, complexFabricator);
		this.ConfigureRecipes();
	}

	// Token: 0x06001A62 RID: 6754 RVA: 0x001A6F68 File Offset: 0x001A5168
	private void ConfigureRecipes()
	{
		ComplexRecipe.RecipeElement[] array = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement(SimHashes.Copper.CreateTag(), 300f, true),
			new ComplexRecipe.RecipeElement("BasicFabric".ToTag(), 2f)
		};
		ComplexRecipe.RecipeElement[] array2 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement("Atmo_Suit".ToTag(), 1f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated, false)
		};
		AtmoSuitConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("SuitFabricator", array, array2), array, array2)
		{
			time = (float)TUNING.EQUIPMENT.SUITS.ATMOSUIT_FABTIME,
			description = STRINGS.EQUIPMENT.PREFABS.ATMO_SUIT.RECIPE_DESC,
			nameDisplay = ComplexRecipe.RecipeNameDisplay.ResultWithIngredient,
			fabricators = new List<Tag>
			{
				"SuitFabricator"
			},
			requiredTech = Db.Get().TechItems.atmoSuit.parentTechId,
			sortOrder = 1
		};
		AtmoSuitConfig.recipe.RequiresAllIngredientsDiscovered = true;
		ComplexRecipe.RecipeElement[] array3 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement(SimHashes.Aluminum.CreateTag(), 300f, true),
			new ComplexRecipe.RecipeElement("BasicFabric".ToTag(), 2f)
		};
		ComplexRecipe.RecipeElement[] array4 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement("Atmo_Suit".ToTag(), 1f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated, false)
		};
		AtmoSuitConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("SuitFabricator", array3, array4), array3, array4)
		{
			time = (float)TUNING.EQUIPMENT.SUITS.ATMOSUIT_FABTIME,
			description = STRINGS.EQUIPMENT.PREFABS.ATMO_SUIT.RECIPE_DESC,
			nameDisplay = ComplexRecipe.RecipeNameDisplay.ResultWithIngredient,
			fabricators = new List<Tag>
			{
				"SuitFabricator"
			},
			requiredTech = Db.Get().TechItems.atmoSuit.parentTechId,
			sortOrder = 1
		};
		AtmoSuitConfig.recipe.RequiresAllIngredientsDiscovered = true;
		ComplexRecipe.RecipeElement[] array5 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement(SimHashes.Iron.CreateTag(), 300f, true),
			new ComplexRecipe.RecipeElement("BasicFabric".ToTag(), 2f)
		};
		ComplexRecipe.RecipeElement[] array6 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement("Atmo_Suit".ToTag(), 1f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated, false)
		};
		AtmoSuitConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("SuitFabricator", array5, array6), array5, array6)
		{
			time = (float)TUNING.EQUIPMENT.SUITS.ATMOSUIT_FABTIME,
			description = STRINGS.EQUIPMENT.PREFABS.ATMO_SUIT.RECIPE_DESC,
			nameDisplay = ComplexRecipe.RecipeNameDisplay.ResultWithIngredient,
			fabricators = new List<Tag>
			{
				"SuitFabricator"
			},
			requiredTech = Db.Get().TechItems.atmoSuit.parentTechId,
			sortOrder = 1
		};
		AtmoSuitConfig.recipe.RequiresAllIngredientsDiscovered = true;
		if (ElementLoader.FindElementByHash(SimHashes.Cobalt) != null)
		{
			ComplexRecipe.RecipeElement[] array7 = new ComplexRecipe.RecipeElement[]
			{
				new ComplexRecipe.RecipeElement(SimHashes.Cobalt.CreateTag(), 300f, true),
				new ComplexRecipe.RecipeElement("BasicFabric".ToTag(), 2f)
			};
			ComplexRecipe.RecipeElement[] array8 = new ComplexRecipe.RecipeElement[]
			{
				new ComplexRecipe.RecipeElement("Atmo_Suit".ToTag(), 1f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated, false)
			};
			AtmoSuitConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("SuitFabricator", array7, array8), array7, array8)
			{
				time = (float)TUNING.EQUIPMENT.SUITS.ATMOSUIT_FABTIME,
				description = STRINGS.EQUIPMENT.PREFABS.ATMO_SUIT.RECIPE_DESC,
				nameDisplay = ComplexRecipe.RecipeNameDisplay.ResultWithIngredient,
				fabricators = new List<Tag>
				{
					"SuitFabricator"
				},
				requiredTech = Db.Get().TechItems.atmoSuit.parentTechId,
				sortOrder = 1
			};
			AtmoSuitConfig.recipe.RequiresAllIngredientsDiscovered = true;
		}
		ComplexRecipe.RecipeElement[] array9 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement("Worn_Atmo_Suit".ToTag(), 1f, true),
			new ComplexRecipe.RecipeElement("BasicFabric".ToTag(), 1f)
		};
		ComplexRecipe.RecipeElement[] array10 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement("Atmo_Suit".ToTag(), 1f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated, false)
		};
		AtmoSuitConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("SuitFabricator", array9, array10), array9, array10)
		{
			time = (float)TUNING.EQUIPMENT.SUITS.ATMOSUIT_FABTIME,
			description = STRINGS.EQUIPMENT.PREFABS.ATMO_SUIT.REPAIR_WORN_DESC,
			nameDisplay = ComplexRecipe.RecipeNameDisplay.Custom,
			fabricators = new List<Tag>
			{
				"SuitFabricator"
			},
			requiredTech = Db.Get().TechItems.atmoSuit.parentTechId,
			sortOrder = 2
		};
		AtmoSuitConfig.recipe.customName = STRINGS.EQUIPMENT.PREFABS.ATMO_SUIT.REPAIR_WORN_RECIPE_NAME;
		AtmoSuitConfig.recipe.ProductHasFacade = true;
		ComplexRecipe.RecipeElement[] array11 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement(SimHashes.Steel.ToString(), 200f),
			new ComplexRecipe.RecipeElement(SimHashes.Petroleum.ToString(), 25f)
		};
		ComplexRecipe.RecipeElement[] array12 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement("Jet_Suit".ToTag(), 1f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated, false)
		};
		JetSuitConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("SuitFabricator", array11, array12), array11, array12)
		{
			time = (float)TUNING.EQUIPMENT.SUITS.ATMOSUIT_FABTIME,
			description = STRINGS.EQUIPMENT.PREFABS.JET_SUIT.RECIPE_DESC,
			nameDisplay = ComplexRecipe.RecipeNameDisplay.ResultWithIngredient,
			fabricators = new List<Tag>
			{
				"SuitFabricator"
			},
			requiredTech = Db.Get().TechItems.jetSuit.parentTechId,
			sortOrder = 3
		};
		ComplexRecipe.RecipeElement[] array13 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement("Worn_Jet_Suit".ToTag(), 1f),
			new ComplexRecipe.RecipeElement("BasicFabric".ToTag(), 1f)
		};
		ComplexRecipe.RecipeElement[] array14 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement("Jet_Suit".ToTag(), 1f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated, false)
		};
		JetSuitConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("SuitFabricator", array13, array14), array13, array14)
		{
			time = (float)TUNING.EQUIPMENT.SUITS.ATMOSUIT_FABTIME,
			description = STRINGS.EQUIPMENT.PREFABS.JET_SUIT.RECIPE_DESC,
			nameDisplay = ComplexRecipe.RecipeNameDisplay.ResultWithIngredient,
			fabricators = new List<Tag>
			{
				"SuitFabricator"
			},
			requiredTech = Db.Get().TechItems.jetSuit.parentTechId,
			sortOrder = 4
		};
		if (DlcManager.FeatureRadiationEnabled())
		{
			ComplexRecipe.RecipeElement[] array15 = new ComplexRecipe.RecipeElement[]
			{
				new ComplexRecipe.RecipeElement(SimHashes.Lead.ToString(), 200f),
				new ComplexRecipe.RecipeElement(SimHashes.Glass.ToString(), 10f)
			};
			ComplexRecipe.RecipeElement[] array16 = new ComplexRecipe.RecipeElement[]
			{
				new ComplexRecipe.RecipeElement("Lead_Suit".ToTag(), 1f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated, false)
			};
			LeadSuitConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("SuitFabricator", array15, array16), array15, array16)
			{
				time = (float)TUNING.EQUIPMENT.SUITS.ATMOSUIT_FABTIME,
				description = STRINGS.EQUIPMENT.PREFABS.LEAD_SUIT.RECIPE_DESC,
				nameDisplay = ComplexRecipe.RecipeNameDisplay.ResultWithIngredient,
				fabricators = new List<Tag>
				{
					"SuitFabricator"
				},
				requiredTech = Db.Get().TechItems.leadSuit.parentTechId,
				sortOrder = 5
			};
		}
		if (DlcManager.FeatureRadiationEnabled())
		{
			ComplexRecipe.RecipeElement[] array17 = new ComplexRecipe.RecipeElement[]
			{
				new ComplexRecipe.RecipeElement("Worn_Lead_Suit".ToTag(), 1f),
				new ComplexRecipe.RecipeElement(SimHashes.Glass.ToString(), 5f)
			};
			ComplexRecipe.RecipeElement[] array18 = new ComplexRecipe.RecipeElement[]
			{
				new ComplexRecipe.RecipeElement("Lead_Suit".ToTag(), 1f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated, false)
			};
			LeadSuitConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("SuitFabricator", array17, array18), array17, array18)
			{
				time = (float)TUNING.EQUIPMENT.SUITS.ATMOSUIT_FABTIME,
				description = STRINGS.EQUIPMENT.PREFABS.LEAD_SUIT.RECIPE_DESC,
				nameDisplay = ComplexRecipe.RecipeNameDisplay.ResultWithIngredient,
				fabricators = new List<Tag>
				{
					"SuitFabricator"
				},
				requiredTech = Db.Get().TechItems.leadSuit.parentTechId,
				sortOrder = 6
			};
		}
	}

	// Token: 0x06001A63 RID: 6755 RVA: 0x001A778C File Offset: 0x001A598C
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.GetComponent<KPrefabID>().prefabInitFn += delegate(GameObject game_object)
		{
			Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Suits, true);
		};
		go.GetComponent<KPrefabID>().prefabSpawnFn += delegate(GameObject game_object)
		{
			ComplexFabricatorWorkable component = game_object.GetComponent<ComplexFabricatorWorkable>();
			component.WorkerStatusItem = Db.Get().DuplicantStatusItems.Fabricating;
			component.AttributeConverter = Db.Get().AttributeConverters.MachinerySpeed;
			component.AttributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
			component.SkillExperienceSkillGroup = Db.Get().SkillGroups.Technicals.Id;
			component.SkillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		};
	}

	// Token: 0x040010C7 RID: 4295
	public const string ID = "SuitFabricator";
}
