using TUNING;
using UnityEngine;

public class MilkFatSeparatorConfig : IBuildingConfig
{
	public const string ID = "MilkFatSeparator";

	public const float INPUT_RATE = 1f;

	public const float MILK_STORED_CAPACITY = 4f;

	public const float MILK_FAT_CAPACITY = 15f;

	public const float EFFICIENCY = 0.9f;

	public const float MILKFAT_PERCENT = 0.1f;

	private const float MILK_TO_FAT_OUTPUT_RATE = 0.089999996f;

	private const float MILK_TO_BRINE_WATER_OUTPUT_RATE = 0.80999994f;

	private const float MILK_TO_CO2_RATE = 0.100000024f;

	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef obj = BuildingTemplates.CreateBuildingDef("MilkFatSeparator", 4, 4, "milk_separator_kanim", 100, 120f, BUILDINGS.CONSTRUCTION_MASS_KG.TIER5, MATERIALS.REFINED_METALS, 1600f, BuildLocationRule.OnFloor, noise: NOISE_POLLUTION.NONE, decor: BUILDINGS.DECOR.PENALTY.TIER2);
		obj.RequiresPowerInput = true;
		obj.EnergyConsumptionWhenActive = 480f;
		obj.SelfHeatKilowattsWhenActive = 8f;
		obj.ExhaustKilowattsWhenActive = 0f;
		obj.InputConduitType = ConduitType.Liquid;
		obj.OutputConduitType = ConduitType.Liquid;
		obj.UtilityInputOffset = new CellOffset(0, 0);
		obj.UtilityOutputOffset = new CellOffset(2, 2);
		obj.AudioCategory = "HollowMetal";
		obj.AudioSize = "large";
		obj.ViewMode = OverlayModes.LiquidConduits.ID;
		obj.PermittedRotations = PermittedRotations.FlipH;
		return obj;
	}

	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
	}

	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		Storage storage = go.AddOrGet<Storage>();
		storage.allowItemRemoval = false;
		storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
		storage.showInUI = true;
		go.AddOrGet<Operational>();
		go.AddOrGet<EmptyMilkSeparatorWorkable>();
		go.AddOrGetDef<MilkSeparator.Def>().MILK_FAT_CAPACITY = 15f;
		ElementConverter elementConverter = go.AddOrGet<ElementConverter>();
		elementConverter.consumedElements = new ElementConverter.ConsumedElement[1]
		{
			new ElementConverter.ConsumedElement(new Tag("Milk"), 1f)
		};
		elementConverter.outputElements = new ElementConverter.OutputElement[3]
		{
			new ElementConverter.OutputElement(0.089999996f, SimHashes.MilkFat, 0f, useEntityTemperature: false, storeOutput: true),
			new ElementConverter.OutputElement(0.80999994f, SimHashes.Brine, 0f, useEntityTemperature: false, storeOutput: true, 0f, 0.5f, 0f),
			new ElementConverter.OutputElement(0.100000024f, SimHashes.CarbonDioxide, 348.15f, useEntityTemperature: false, storeOutput: false, 1f, 3f, 0f)
		};
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.conduitType = ConduitType.Liquid;
		conduitConsumer.consumptionRate = 10f;
		conduitConsumer.capacityKG = 4f;
		conduitConsumer.capacityTag = ElementLoader.FindElementByHash(SimHashes.Milk).tag;
		conduitConsumer.forceAlwaysSatisfied = true;
		conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Store;
		ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
		conduitDispenser.conduitType = ConduitType.Liquid;
		conduitDispenser.invertElementFilter = true;
		conduitDispenser.elementFilter = new SimHashes[1] { SimHashes.Milk };
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
		Prioritizable.AddRef(go);
	}

	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	public override void ConfigurePost(BuildingDef def)
	{
	}
}
