using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

public class ChlorinatorConfig : IBuildingConfig
{
	public const string ID = "Chlorinator";

	public static readonly Tag BLEACH_STONE_TAG = SimHashes.BleachStone.CreateTag();

	public static readonly Tag SAND_TAG = SimHashes.Sand.CreateTag();

	private const float BLEACH_STONE_PER_CYCLE = 150f;

	public const float BLEACH_STONE_OUTPUT_PER_RECIPE = 10f;

	public const float INPUT_KG = 30f;

	public const float OUTPUT_BLEACH_STONE_PERCENT = 1f / 3f;

	public const float OUTPUT_BLEACHSTONE_ORE_SIZE = 2f;

	public const float OUTPUT_SAND_ORE_SIZE = 6f;

	public static readonly MathUtil.MinMax POP_TIMING = new MathUtil.MinMax(0.1f, 0.4f);

	public static readonly MathUtil.MinMaxInt EMIT_ORE_COUNT_RANGE_BLEACH_STONE = new MathUtil.MinMaxInt(2, 3);

	public static readonly MathUtil.MinMaxInt EMIT_ORE_COUNT_RANGE_SAND = new MathUtil.MinMaxInt(1, 1);

	public static readonly MathUtil.MinMax EMIT_ORE_INITIAL_VELOCITY_RANGE = new MathUtil.MinMax(2f, 4f);

	public static readonly MathUtil.MinMax EMIT_ORE_INITIAL_DIRECTION_HALF_ANGLE_IN_DEGREES_RANGE = new MathUtil.MinMax(40f, 0f);

	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef obj = BuildingTemplates.CreateBuildingDef("Chlorinator", 3, 3, "chlorinator_kanim", 100, 120f, TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER5, MATERIALS.REFINED_METALS, 1600f, BuildLocationRule.OnFloor, noise: NOISE_POLLUTION.NONE, decor: TUNING.BUILDINGS.DECOR.PENALTY.TIER2);
		obj.RequiresPowerInput = true;
		obj.PowerInputOffset = new CellOffset(0, 0);
		obj.EnergyConsumptionWhenActive = 480f;
		obj.ExhaustKilowattsWhenActive = 1f;
		obj.SelfHeatKilowattsWhenActive = 2f;
		obj.AudioCategory = "Metal";
		obj.PermittedRotations = PermittedRotations.FlipH;
		obj.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 1));
		return obj;
	}

	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
	}

	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
		go.AddOrGet<DropAllWorkable>();
		go.AddOrGet<BuildingComplete>().isManuallyOperated = false;
		go.AddOrGet<FabricatorIngredientStatusManager>();
		ComplexFabricator complexFabricator = go.AddOrGet<ComplexFabricator>();
		complexFabricator.duplicantOperated = false;
		complexFabricator.sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;
		complexFabricator.storeProduced = true;
		BuildingTemplates.CreateComplexFabricatorStorage(go, complexFabricator);
		ConfigureRecipes();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
	}

	private void ConfigureRecipes()
	{
		ComplexRecipe.RecipeElement[] array = new ComplexRecipe.RecipeElement[2]
		{
			new ComplexRecipe.RecipeElement(SimHashes.Salt.CreateTag(), 30f),
			new ComplexRecipe.RecipeElement(SimHashes.Gold.CreateTag(), 0.5f)
		};
		ComplexRecipe.RecipeElement[] array2 = new ComplexRecipe.RecipeElement[2]
		{
			new ComplexRecipe.RecipeElement(BLEACH_STONE_TAG, 10f),
			new ComplexRecipe.RecipeElement(SAND_TAG, 19.999998f)
		};
		new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("Chlorinator", array, array2), array, array2)
		{
			time = 40f,
			description = string.Format(STRINGS.BUILDINGS.PREFABS.EGGCRACKER.RECIPE_DESCRIPTION, ElementLoader.FindElementByHash(SimHashes.Salt).name, ElementLoader.FindElementByHash(SimHashes.BleachStone).name),
			fabricators = new List<Tag> { TagManager.Create("Chlorinator") },
			nameDisplay = ComplexRecipe.RecipeNameDisplay.Result
		};
	}

	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<LogicOperationalController>();
		Chlorinator.Def def = go.AddOrGetDef<Chlorinator.Def>();
		def.primaryOreTag = BLEACH_STONE_TAG;
		def.primaryOreMassPerOre = 2f;
		def.primaryOreCount = EMIT_ORE_COUNT_RANGE_BLEACH_STONE;
		def.secondaryOreTag = SAND_TAG;
		def.secondaryOreMassPerOre = 6f;
		def.secondaryOreCount = EMIT_ORE_COUNT_RANGE_SAND;
		def.initialVelocity = EMIT_ORE_INITIAL_VELOCITY_RANGE;
		def.initialDirectionHalfAngleDegreesRange = EMIT_ORE_INITIAL_DIRECTION_HALF_ANGLE_IN_DEGREES_RANGE;
		def.offset = new Vector3(0.6f, 2.2f, 0f);
		def.popWaitRange = POP_TIMING;
	}

	public override void ConfigurePost(BuildingDef def)
	{
	}
}
