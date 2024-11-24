using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

// Token: 0x020005A7 RID: 1447
public class SpiceGrinderConfig : IBuildingConfig
{
	// Token: 0x060019C6 RID: 6598 RVA: 0x001A4470 File Offset: 0x001A2670
	public override BuildingDef CreateBuildingDef()
	{
		string id = "SpiceGrinder";
		int width = 2;
		int height = 3;
		string anim = "spice_grinder_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER1;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tier2, 0.2f);
		buildingDef.ViewMode = OverlayModes.Rooms.ID;
		buildingDef.Overheatable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "large";
		buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
		return buildingDef;
	}

	// Token: 0x060019C7 RID: 6599 RVA: 0x000B0F11 File Offset: 0x000AF111
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<LoopingSounds>();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.SpiceStation, false);
	}

	// Token: 0x060019C8 RID: 6600 RVA: 0x001A44F0 File Offset: 0x001A26F0
	public override void DoPostConfigureComplete(GameObject go)
	{
		SpiceGrinder.InitializeSpices();
		SymbolOverrideControllerUtil.AddToPrefab(go);
		go.AddOrGetDef<SpiceGrinder.Def>();
		go.AddOrGet<SpiceGrinderWorkable>();
		go.AddOrGet<LogicOperationalController>();
		go.AddOrGet<TreeFilterable>().uiHeight = TreeFilterable.UISideScreenHeight.Short;
		go.AddOrGet<Prioritizable>().SetMasterPriority(new PrioritySetting(PriorityScreen.PriorityClass.basic, SpiceGrinderConfig.STORAGE_PRIORITY));
		Storage storage = go.AddComponent<Storage>();
		storage.showInUI = true;
		storage.showDescriptor = true;
		storage.storageFilters = new List<Tag>
		{
			GameTags.Edible
		};
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
		storage2.storageFilters = new List<Tag>
		{
			GameTags.Seed
		};
		storage2.allowItemRemoval = false;
		storage2.storageFullMargin = STORAGE.STORAGE_LOCKER_FILLED_MARGIN;
		storage2.fetchCategory = Storage.FetchCategory.Building;
		storage2.showCapacityStatusItem = true;
		storage2.SetDefaultStoredItemModifiers(Storage.StandardFabricatorStorage);
		RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
		roomTracker.requiredRoomType = Db.Get().RoomTypes.Kitchen.Id;
		roomTracker.requirement = RoomTracker.Requirement.Required;
	}

	// Token: 0x04001067 RID: 4199
	public const string ID = "SpiceGrinder";

	// Token: 0x04001068 RID: 4200
	public static Tag MATERIAL_FOR_TINKER = GameTags.CropSeed;

	// Token: 0x04001069 RID: 4201
	public static Tag TINKER_TOOLS = FarmStationToolsConfig.tag;

	// Token: 0x0400106A RID: 4202
	public const float MASS_PER_TINKER = 5f;

	// Token: 0x0400106B RID: 4203
	public const float OUTPUT_TEMPERATURE = 313.15f;

	// Token: 0x0400106C RID: 4204
	public const float WORK_TIME_PER_1000KCAL = 5f;

	// Token: 0x0400106D RID: 4205
	public const short SPICE_CAPACITY_PER_INGREDIENT = 10;

	// Token: 0x0400106E RID: 4206
	public const string PrimaryColorSymbol = "stripe_anim2";

	// Token: 0x0400106F RID: 4207
	public const string SecondaryColorSymbol = "stripe_anim1";

	// Token: 0x04001070 RID: 4208
	public const string GrinderColorSymbol = "grinder";

	// Token: 0x04001071 RID: 4209
	public static StatusItem SpicedStatus = Db.Get().MiscStatusItems.SpicedFood;

	// Token: 0x04001072 RID: 4210
	private static int STORAGE_PRIORITY = Chore.DefaultPrioritySetting.priority_value - 1;
}
