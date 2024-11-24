using System.Collections.Generic;
using TUNING;
using UnityEngine;

public class SpiceGrinderConfig : IBuildingConfig
{
	public const string ID = "SpiceGrinder";

	public static Tag MATERIAL_FOR_TINKER = GameTags.CropSeed;

	public static Tag TINKER_TOOLS = FarmStationToolsConfig.tag;

	public const float MASS_PER_TINKER = 5f;

	public const float OUTPUT_TEMPERATURE = 313.15f;

	public const float WORK_TIME_PER_1000KCAL = 5f;

	public const short SPICE_CAPACITY_PER_INGREDIENT = 10;

	public const string PrimaryColorSymbol = "stripe_anim2";

	public const string SecondaryColorSymbol = "stripe_anim1";

	public const string GrinderColorSymbol = "grinder";

	public static StatusItem SpicedStatus = Db.Get().MiscStatusItems.SpicedFood;

	private static int STORAGE_PRIORITY = Chore.DefaultPrioritySetting.priority_value - 1;

	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef obj = BuildingTemplates.CreateBuildingDef("SpiceGrinder", 2, 3, "spice_grinder_kanim", 30, 30f, BUILDINGS.CONSTRUCTION_MASS_KG.TIER4, MATERIALS.ALL_METALS, 1600f, BuildLocationRule.OnFloor, noise: NOISE_POLLUTION.NOISY.TIER1, decor: BUILDINGS.DECOR.NONE);
		obj.ViewMode = OverlayModes.Rooms.ID;
		obj.Overheatable = false;
		obj.AudioCategory = "Metal";
		obj.AudioSize = "large";
		obj.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
		return obj;
	}

	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<LoopingSounds>();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.SpiceStation);
	}

	public override void DoPostConfigureComplete(GameObject go)
	{
		SpiceGrinder.InitializeSpices();
		SymbolOverrideControllerUtil.AddToPrefab(go);
		go.AddOrGetDef<SpiceGrinder.Def>();
		go.AddOrGet<SpiceGrinderWorkable>();
		go.AddOrGet<LogicOperationalController>();
		go.AddOrGet<TreeFilterable>().uiHeight = TreeFilterable.UISideScreenHeight.Short;
		go.AddOrGet<Prioritizable>().SetMasterPriority(new PrioritySetting(PriorityScreen.PriorityClass.basic, STORAGE_PRIORITY));
		Storage storage = go.AddComponent<Storage>();
		storage.showInUI = true;
		storage.showDescriptor = true;
		storage.storageFilters = new List<Tag> { GameTags.Edible };
		storage.allowItemRemoval = false;
		storage.capacityKg = 1f;
		storage.storageFullMargin = STORAGE.STORAGE_LOCKER_FILLED_MARGIN;
		storage.fetchCategory = Storage.FetchCategory.Building;
		storage.showCapacityStatusItem = false;
		storage.allowSettingOnlyFetchMarkedItems = false;
		storage.showSideScreenTitleBar = true;
		storage.SetDefaultStoredItemModifiers(Storage.StandardFabricatorStorage);
		Storage storage2 = go.AddComponent<Storage>();
		storage2.showInUI = true;
		storage2.showDescriptor = true;
		storage2.storageFilters = new List<Tag> { GameTags.Seed };
		storage2.allowItemRemoval = false;
		storage2.storageFullMargin = STORAGE.STORAGE_LOCKER_FILLED_MARGIN;
		storage2.fetchCategory = Storage.FetchCategory.Building;
		storage2.showCapacityStatusItem = true;
		storage2.SetDefaultStoredItemModifiers(Storage.StandardFabricatorStorage);
		RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
		roomTracker.requiredRoomType = Db.Get().RoomTypes.Kitchen.Id;
		roomTracker.requirement = RoomTracker.Requirement.Required;
	}
}
