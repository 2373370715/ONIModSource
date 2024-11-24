using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

public class FossilDigSiteConfig : IBuildingConfig
{
	public static class FOSSIL_HUNT_LORE_UNLOCK_ID
	{
		public static int popupsAvailablesForSmallSites = 3;

		public static string For(int id)
		{
			return $"story_trait_fossilhunt_poi{Mathf.Clamp(id, 1, popupsAvailablesForSmallSites)}";
		}
	}

	public static int DiscoveredDigsitesRequired = 4;

	public static HashedString hashID = new HashedString("FossilDig");

	public const string ID = "FossilDig";

	public static readonly HashedString QUEST_CRITERIA = "LostSpecimen";

	public const string CODEX_ENTRY_ID = "STORYTRAITFOSSILHUNT";

	public static string GetBodyContentForFossil(int id)
	{
		return CODEX.STORY_TRAITS.FOSSILHUNT.DNADATA_ENTRY.TELEPORTFAILURE;
	}

	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef obj = BuildingTemplates.CreateBuildingDef("FossilDig", 5, 3, "fossil_dig_kanim", 30, 120f, TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER7, new string[1] { SimHashes.Fossil.ToString() }, 9999f, BuildLocationRule.OnFloor, noise: NOISE_POLLUTION.NOISY.TIER3, decor: TUNING.BUILDINGS.DECOR.BONUS.TIER1);
		obj.Floodable = true;
		obj.Entombable = false;
		obj.ShowInBuildMenu = false;
		obj.Overheatable = false;
		obj.ObjectLayer = ObjectLayer.Building;
		obj.SceneLayer = Grid.SceneLayer.Building;
		obj.AudioCategory = "Plastic";
		obj.AudioSize = "medium";
		obj.UseStructureTemperature = false;
		return obj;
	}

	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddTag(GameTags.Gravitas);
		go.GetComponent<Deconstructable>().allowDeconstruction = false;
		Prioritizable.AddRef(go);
		PrimaryElement component = go.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Fossil);
		component.Temperature = 315f;
		go.AddOrGetDef<MajorFossilDigSite.Def>().questCriteria = QUEST_CRITERIA;
		go.AddOrGetDef<FossilHuntInitializer.Def>().IsMainDigsite = true;
		go.AddOrGet<MajorDigSiteWorkable>();
		go.AddOrGet<Operational>();
		go.AddOrGet<EntombVulnerable>();
		go.AddOrGet<FossilMineWorkable>().overrideAnims = new KAnimFile[1] { Assets.GetAnim("anim_interacts_fossil_dig_kanim") };
		FossilMine fossilMine = go.AddOrGet<FossilMine>();
		fossilMine.heatedTemperature = 0f;
		fossilMine.sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;
		go.AddOrGet<FabricatorIngredientStatusManager>();
		BuildingTemplates.CreateComplexFabricatorStorage(go, fossilMine);
		go.AddOrGet<Demolishable>().allowDemolition = false;
		FossilDigsiteLampLight fossilDigsiteLampLight = go.AddOrGet<FossilDigsiteLampLight>();
		fossilDigsiteLampLight.Color = Color.yellow;
		fossilDigsiteLampLight.overlayColour = LIGHT2D.WALLLIGHT_COLOR;
		fossilDigsiteLampLight.Range = 3f;
		fossilDigsiteLampLight.Angle = 0f;
		fossilDigsiteLampLight.Direction = LIGHT2D.DEFAULT_DIRECTION;
		fossilDigsiteLampLight.Offset = LIGHT2D.MAJORFOSSILDIGSITE_LAMP_OFFSET;
		fossilDigsiteLampLight.shape = LightShape.Circle;
		fossilDigsiteLampLight.drawOverlay = true;
		fossilDigsiteLampLight.Lux = 1000;
		fossilDigsiteLampLight.enabled = false;
		ConfigureRecipes();
		go.AddOrGet<LoopingSounds>();
	}

	public override void DoPostConfigureComplete(GameObject go)
	{
		KBatchedAnimController component = go.GetComponent<KBatchedAnimController>();
		component.defaultAnim = "covered";
		component.initialAnim = "covered";
		Object.DestroyImmediate(go.GetComponent<BuildingEnabledButton>());
	}

	private void ConfigureRecipes()
	{
		ComplexRecipe.RecipeElement[] array = new ComplexRecipe.RecipeElement[1]
		{
			new ComplexRecipe.RecipeElement(SimHashes.Diamond.CreateTag(), 1f)
		};
		ComplexRecipe.RecipeElement[] array2 = new ComplexRecipe.RecipeElement[1]
		{
			new ComplexRecipe.RecipeElement(SimHashes.Fossil.CreateTag(), 100f)
		};
		new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("FossilDig", array, array2), array, array2)
		{
			time = 80f,
			description = CODEX.STORY_TRAITS.FOSSILHUNT.REWARDS.MINED_FOSSIL.DESC,
			nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
			fabricators = new List<Tag> { "FossilDig" },
			sortOrder = 21
		};
	}
}
