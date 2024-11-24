using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

public class SupermaterialRefineryConfig : IBuildingConfig
{
	public const string ID = "SupermaterialRefinery";

	private const float INPUT_KG = 100f;

	private const float OUTPUT_KG = 100f;

	private const float OUTPUT_TEMPERATURE = 313.15f;

	private HashedString[] dupeInteractAnims;

	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef obj = BuildingTemplates.CreateBuildingDef("SupermaterialRefinery", 4, 5, "supermaterial_refinery_kanim", 30, 480f, TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER5, MATERIALS.ALL_METALS, 2400f, BuildLocationRule.OnFloor, noise: NOISE_POLLUTION.NOISY.TIER6, decor: TUNING.BUILDINGS.DECOR.PENALTY.TIER2);
		obj.RequiresPowerInput = true;
		obj.EnergyConsumptionWhenActive = 1600f;
		obj.SelfHeatKilowattsWhenActive = 16f;
		obj.ViewMode = OverlayModes.Power.ID;
		obj.AudioCategory = "HollowMetal";
		obj.AudioSize = "large";
		return obj;
	}

	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<DropAllWorkable>();
		go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
		ComplexFabricator complexFabricator = go.AddOrGet<ComplexFabricator>();
		complexFabricator.heatedTemperature = 313.15f;
		complexFabricator.sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;
		complexFabricator.duplicantOperated = true;
		go.AddOrGet<FabricatorIngredientStatusManager>();
		go.AddOrGet<CopyBuildingSettings>();
		go.AddOrGet<ComplexFabricatorWorkable>();
		BuildingTemplates.CreateComplexFabricatorStorage(go, complexFabricator);
		Prioritizable.AddRef(go);
		float num = 0.01f;
		float num2 = (1f - num) * 0.5f;
		ComplexRecipe.RecipeElement[] array = new ComplexRecipe.RecipeElement[3]
		{
			new ComplexRecipe.RecipeElement(SimHashes.Fullerene.CreateTag(), 100f * num),
			new ComplexRecipe.RecipeElement(SimHashes.Gold.CreateTag(), 100f * num2),
			new ComplexRecipe.RecipeElement(SimHashes.Petroleum.CreateTag(), 100f * num2)
		};
		ComplexRecipe.RecipeElement[] array2 = new ComplexRecipe.RecipeElement[1]
		{
			new ComplexRecipe.RecipeElement(SimHashes.SuperCoolant.CreateTag(), 100f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated)
		};
		new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("SupermaterialRefinery", array, array2), array, array2)
		{
			time = 80f,
			description = STRINGS.BUILDINGS.PREFABS.SUPERMATERIALREFINERY.SUPERCOOLANT_RECIPE_DESCRIPTION,
			nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
			fabricators = new List<Tag> { TagManager.Create("SupermaterialRefinery") }
		};
		if (DlcManager.IsExpansion1Active())
		{
			float num3 = 0.9f;
			float num4 = 1f - num3;
			ComplexRecipe.RecipeElement[] array3 = new ComplexRecipe.RecipeElement[3]
			{
				new ComplexRecipe.RecipeElement(SimHashes.Graphite.CreateTag(), 100f * num3),
				new ComplexRecipe.RecipeElement(SimHashes.Sulfur.CreateTag(), 100f * num4 / 2f),
				new ComplexRecipe.RecipeElement(SimHashes.Aluminum.CreateTag(), 100f * num4 / 2f)
			};
			ComplexRecipe.RecipeElement[] array4 = new ComplexRecipe.RecipeElement[1]
			{
				new ComplexRecipe.RecipeElement(SimHashes.Fullerene.CreateTag(), 100f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated)
			};
			new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("SupermaterialRefinery", array3, array4), array3, array4)
			{
				time = 80f,
				description = STRINGS.BUILDINGS.PREFABS.SUPERMATERIALREFINERY.FULLERENE_RECIPE_DESCRIPTION,
				nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
				fabricators = new List<Tag> { TagManager.Create("SupermaterialRefinery") }
			};
		}
		float num5 = 0.15f;
		float num6 = 0.7f;
		float num7 = 0.15f;
		ComplexRecipe.RecipeElement[] array5 = new ComplexRecipe.RecipeElement[3]
		{
			new ComplexRecipe.RecipeElement(SimHashes.TempConductorSolid.CreateTag(), 100f * num5),
			new ComplexRecipe.RecipeElement(SimHashes.Polypropylene.CreateTag(), 100f * num6),
			new ComplexRecipe.RecipeElement(SimHashes.MilkFat.CreateTag(), 100f * num7)
		};
		ComplexRecipe.RecipeElement[] array6 = new ComplexRecipe.RecipeElement[1]
		{
			new ComplexRecipe.RecipeElement(SimHashes.HardPolypropylene.CreateTag(), 100f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated)
		};
		new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("SupermaterialRefinery", array5, array6), array5, array6)
		{
			time = 80f,
			description = STRINGS.BUILDINGS.PREFABS.SUPERMATERIALREFINERY.HARDPLASTIC_RECIPE_DESCRIPTION,
			nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
			fabricators = new List<Tag> { TagManager.Create("SupermaterialRefinery") }
		};
		float num8 = 0.15f;
		float num9 = 0.05f;
		float num10 = 1f - num9 - num8;
		ComplexRecipe.RecipeElement[] array7 = new ComplexRecipe.RecipeElement[3]
		{
			new ComplexRecipe.RecipeElement(SimHashes.Isoresin.CreateTag(), 100f * num8),
			new ComplexRecipe.RecipeElement(SimHashes.Katairite.CreateTag(), 100f * num10),
			new ComplexRecipe.RecipeElement(BasicFabricConfig.ID.ToTag(), 100f * num9)
		};
		ComplexRecipe.RecipeElement[] array8 = new ComplexRecipe.RecipeElement[1]
		{
			new ComplexRecipe.RecipeElement(SimHashes.SuperInsulator.CreateTag(), 100f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated)
		};
		new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("SupermaterialRefinery", array7, array8), array7, array8)
		{
			time = 80f,
			description = STRINGS.BUILDINGS.PREFABS.SUPERMATERIALREFINERY.SUPERINSULATOR_RECIPE_DESCRIPTION,
			nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
			fabricators = new List<Tag> { TagManager.Create("SupermaterialRefinery") }
		};
		float num11 = 0.05f;
		ComplexRecipe.RecipeElement[] array9 = new ComplexRecipe.RecipeElement[2]
		{
			new ComplexRecipe.RecipeElement(SimHashes.Niobium.CreateTag(), 100f * num11),
			new ComplexRecipe.RecipeElement(SimHashes.Tungsten.CreateTag(), 100f * (1f - num11))
		};
		ComplexRecipe.RecipeElement[] array10 = new ComplexRecipe.RecipeElement[1]
		{
			new ComplexRecipe.RecipeElement(SimHashes.TempConductorSolid.CreateTag(), 100f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated)
		};
		new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("SupermaterialRefinery", array9, array10), array9, array10)
		{
			time = 80f,
			description = STRINGS.BUILDINGS.PREFABS.SUPERMATERIALREFINERY.TEMPCONDUCTORSOLID_RECIPE_DESCRIPTION,
			nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
			fabricators = new List<Tag> { TagManager.Create("SupermaterialRefinery") }
		};
		float num12 = 0.35f;
		ComplexRecipe.RecipeElement[] array11 = new ComplexRecipe.RecipeElement[2]
		{
			new ComplexRecipe.RecipeElement(SimHashes.Isoresin.CreateTag(), 100f * num12),
			new ComplexRecipe.RecipeElement(SimHashes.Petroleum.CreateTag(), 100f * (1f - num12))
		};
		ComplexRecipe.RecipeElement[] array12 = new ComplexRecipe.RecipeElement[1]
		{
			new ComplexRecipe.RecipeElement(SimHashes.ViscoGel.CreateTag(), 100f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated)
		};
		new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("SupermaterialRefinery", array11, array12), array11, array12)
		{
			time = 80f,
			description = STRINGS.BUILDINGS.PREFABS.SUPERMATERIALREFINERY.VISCOGEL_RECIPE_DESCRIPTION,
			nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
			fabricators = new List<Tag> { TagManager.Create("SupermaterialRefinery") }
		};
	}

	public override void DoPostConfigureComplete(GameObject go)
	{
		go.GetComponent<KPrefabID>().prefabSpawnFn += delegate(GameObject game_object)
		{
			ComplexFabricatorWorkable component = game_object.GetComponent<ComplexFabricatorWorkable>();
			component.WorkerStatusItem = Db.Get().DuplicantStatusItems.Processing;
			component.AttributeConverter = Db.Get().AttributeConverters.MachinerySpeed;
			component.AttributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
			component.SkillExperienceSkillGroup = Db.Get().SkillGroups.Technicals.Id;
			component.SkillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
			KAnimFile anim = Assets.GetAnim("anim_interacts_supermaterial_refinery_kanim");
			KAnimFile[] overrideAnims = new KAnimFile[1] { anim };
			component.overrideAnims = overrideAnims;
			component.workAnims = new HashedString[2] { "working_pre", "working_loop" };
			component.synchronizeAnims = false;
			KAnimFileData data = anim.GetData();
			int animCount = data.animCount;
			dupeInteractAnims = new HashedString[animCount - 2];
			int i = 0;
			int num = 0;
			for (; i < animCount; i++)
			{
				HashedString hashedString = data.GetAnim(i).name;
				if (hashedString != "working_pre" && hashedString != "working_pst")
				{
					dupeInteractAnims[num] = hashedString;
					num++;
				}
			}
			component.GetDupeInteract = () => new HashedString[2]
			{
				"working_loop",
				dupeInteractAnims.GetRandom()
			};
		};
	}
}
