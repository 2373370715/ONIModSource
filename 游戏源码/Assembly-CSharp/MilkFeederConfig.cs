using TUNING;
using UnityEngine;

public class MilkFeederConfig : IBuildingConfig
{
	public const string ID = "MilkFeeder";

	public const string HAD_CONSUMED_MILK_RECENTLY_EFFECT_ID = "HadMilk";

	public const float EFFECT_DURATION_IN_SECONDS = 600f;

	public static readonly CellOffset DRINK_FROM_OFFSET = new CellOffset(1, 0);

	public static readonly Tag MILK_TAG = SimHashes.Milk.CreateTag();

	public const float UNITS_OF_MILK_CONSUMED_PER_FEEDING = 5f;

	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef obj = BuildingTemplates.CreateBuildingDef("MilkFeeder", 3, 3, "critter_milk_feeder_kanim", 100, 120f, BUILDINGS.CONSTRUCTION_MASS_KG.TIER4, MATERIALS.REFINED_METALS, 1600f, BuildLocationRule.OnFloor, noise: NOISE_POLLUTION.NONE, decor: BUILDINGS.DECOR.PENALTY.TIER2);
		obj.AudioCategory = "Metal";
		obj.PermittedRotations = PermittedRotations.FlipH;
		obj.InputConduitType = ConduitType.Liquid;
		obj.UtilityInputOffset = new CellOffset(0, 0);
		obj.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
		return obj;
	}

	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
	}

	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		Prioritizable.AddRef(go);
		go.AddOrGet<LogicOperationalController>();
		RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
		roomTracker.requiredRoomType = Db.Get().RoomTypes.CreaturePen.Id;
		roomTracker.requirement = RoomTracker.Requirement.Required;
		Storage storage = go.AddOrGet<Storage>();
		storage.capacityKg = 80f;
		storage.showInUI = true;
		storage.showDescriptor = true;
		storage.allowItemRemoval = false;
		storage.allowSettingOnlyFetchMarkedItems = false;
		storage.showCapacityStatusItem = true;
		storage.showCapacityAsMainStatus = true;
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.conduitType = ConduitType.Liquid;
		conduitConsumer.consumptionRate = 10f;
		conduitConsumer.capacityTag = GameTagExtensions.Create(SimHashes.Milk);
		conduitConsumer.forceAlwaysSatisfied = true;
		conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
		conduitConsumer.storage = storage;
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.RanchStationType);
	}

	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGetDef<MilkFeeder.Def>();
	}

	public override void ConfigurePost(BuildingDef def)
	{
	}
}
